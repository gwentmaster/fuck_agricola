using System;
using System.IO;

namespace BestHTTP
{
	// Token: 0x02000565 RID: 1381
	internal sealed class FileConnection : ConnectionBase
	{
		// Token: 0x0600322A RID: 12842 RVA: 0x000FF7C1 File Offset: 0x000FD9C1
		public FileConnection(string serverAddress) : base(serverAddress)
		{
		}

		// Token: 0x0600322B RID: 12843 RVA: 0x000FF7CC File Offset: 0x000FD9CC
		internal override void Abort(HTTPConnectionStates newState)
		{
			base.State = newState;
			HTTPConnectionStates state = base.State;
			if (state == HTTPConnectionStates.TimedOut)
			{
				base.TimedOutStart = DateTime.UtcNow;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600322C RID: 12844 RVA: 0x000FF7FC File Offset: 0x000FD9FC
		protected override void ThreadFunc(object param)
		{
			try
			{
				using (FileStream fileStream = new FileStream(base.CurrentRequest.CurrentUri.LocalPath, FileMode.Open, FileAccess.Read))
				{
					using (StreamList streamList = new StreamList(new Stream[]
					{
						new MemoryStream(),
						fileStream
					}))
					{
						streamList.Write("HTTP/1.1 200 Ok\r\n");
						streamList.Write("Content-Type: application/octet-stream\r\n");
						streamList.Write("Content-Length: " + fileStream.Length.ToString() + "\r\n");
						streamList.Write("\r\n");
						streamList.Seek(0L, SeekOrigin.Begin);
						base.CurrentRequest.Response = new HTTPResponse(base.CurrentRequest, streamList, base.CurrentRequest.UseStreaming, false);
						if (!base.CurrentRequest.Response.Receive(-1, true))
						{
							base.CurrentRequest.Response = null;
						}
					}
				}
			}
			catch (Exception exception)
			{
				if (base.CurrentRequest != null)
				{
					base.CurrentRequest.Response = null;
					HTTPConnectionStates state = base.State;
					if (state != HTTPConnectionStates.AbortRequested)
					{
						if (state != HTTPConnectionStates.TimedOut)
						{
							base.CurrentRequest.Exception = exception;
							base.CurrentRequest.State = HTTPRequestStates.Error;
						}
						else
						{
							base.CurrentRequest.State = HTTPRequestStates.TimedOut;
						}
					}
					else
					{
						base.CurrentRequest.State = HTTPRequestStates.Aborted;
					}
				}
			}
			finally
			{
				base.State = HTTPConnectionStates.Closed;
				if (base.CurrentRequest.State == HTTPRequestStates.Processing)
				{
					if (base.CurrentRequest.Response != null)
					{
						base.CurrentRequest.State = HTTPRequestStates.Finished;
					}
					else
					{
						base.CurrentRequest.State = HTTPRequestStates.Error;
					}
				}
			}
		}
	}
}
