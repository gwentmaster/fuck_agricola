using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x020005E1 RID: 1505
	public sealed class EventSourceResponse : HTTPResponse, IProtocol
	{
		// Token: 0x17000727 RID: 1831
		// (get) Token: 0x06003715 RID: 14101 RVA: 0x00110390 File Offset: 0x0010E590
		// (set) Token: 0x06003716 RID: 14102 RVA: 0x00110398 File Offset: 0x0010E598
		public bool IsClosed { get; private set; }

		// Token: 0x06003717 RID: 14103 RVA: 0x001103A1 File Offset: 0x0010E5A1
		public EventSourceResponse(HTTPRequest request, Stream stream, bool isStreamed, bool isFromCache) : base(request, stream, isStreamed, isFromCache)
		{
			base.IsClosedManually = true;
		}

		// Token: 0x06003718 RID: 14104 RVA: 0x001103DC File Offset: 0x0010E5DC
		internal override bool Receive(int forceReadRawContentLength = -1, bool readPayloadData = true)
		{
			bool flag = base.Receive(forceReadRawContentLength, false);
			string firstHeaderValue = base.GetFirstHeaderValue("content-type");
			base.IsUpgraded = (flag && base.StatusCode == 200 && !string.IsNullOrEmpty(firstHeaderValue) && firstHeaderValue.ToLower().StartsWith("text/event-stream"));
			if (!base.IsUpgraded)
			{
				base.ReadPayload(forceReadRawContentLength);
			}
			return flag;
		}

		// Token: 0x06003719 RID: 14105 RVA: 0x00110440 File Offset: 0x0010E640
		internal void StartReceive()
		{
			if (base.IsUpgraded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ReceiveThreadFunc));
			}
		}

		// Token: 0x0600371A RID: 14106 RVA: 0x0011045C File Offset: 0x0010E65C
		private void ReceiveThreadFunc(object param)
		{
			try
			{
				if (base.HasHeaderWithValue("transfer-encoding", "chunked"))
				{
					this.ReadChunked(this.Stream);
				}
				else
				{
					this.ReadRaw(this.Stream, -1L);
				}
			}
			catch (ThreadAbortException)
			{
				this.baseRequest.State = HTTPRequestStates.Aborted;
			}
			catch (Exception exception)
			{
				if (HTTPUpdateDelegator.IsCreated)
				{
					this.baseRequest.Exception = exception;
					this.baseRequest.State = HTTPRequestStates.Error;
				}
				else
				{
					this.baseRequest.State = HTTPRequestStates.Aborted;
				}
			}
			finally
			{
				this.IsClosed = true;
			}
		}

		// Token: 0x0600371B RID: 14107 RVA: 0x00110508 File Offset: 0x0010E708
		private new void ReadChunked(Stream stream)
		{
			int num = base.ReadChunkLength(stream);
			byte[] array = new byte[num];
			while (num != 0)
			{
				if (array.Length < num)
				{
					Array.Resize<byte>(ref array, num);
				}
				int num2 = 0;
				do
				{
					int num3 = stream.Read(array, num2, num - num2);
					if (num3 == 0)
					{
						goto Block_2;
					}
					num2 += num3;
				}
				while (num2 < num);
				this.FeedData(array, num2);
				HTTPResponse.ReadTo(stream, 10);
				num = base.ReadChunkLength(stream);
				continue;
				Block_2:
				throw new Exception("The remote server closed the connection unexpectedly!");
			}
			base.ReadHeaders(stream);
		}

		// Token: 0x0600371C RID: 14108 RVA: 0x0011057C File Offset: 0x0010E77C
		private new void ReadRaw(Stream stream, long contentLength)
		{
			byte[] array = new byte[1024];
			int num;
			do
			{
				num = stream.Read(array, 0, array.Length);
				this.FeedData(array, num);
			}
			while (num > 0);
		}

		// Token: 0x0600371D RID: 14109 RVA: 0x001105AC File Offset: 0x0010E7AC
		public void FeedData(byte[] buffer, int count)
		{
			if (count == -1)
			{
				count = buffer.Length;
			}
			if (count == 0)
			{
				return;
			}
			int num = 0;
			for (;;)
			{
				int num2 = -1;
				int num3 = 1;
				int num4 = num;
				while (num4 < count && num2 == -1)
				{
					if (buffer[num4] == 13)
					{
						if (num4 + 1 < count && buffer[num4 + 1] == 10)
						{
							num3 = 2;
						}
						num2 = num4;
					}
					else if (buffer[num4] == 10)
					{
						num2 = num4;
					}
					num4++;
				}
				int num5 = (num2 == -1) ? count : num2;
				if (this.LineBuffer.Length < this.LineBufferPos + (num5 - num))
				{
					Array.Resize<byte>(ref this.LineBuffer, this.LineBufferPos + (num5 - num));
				}
				Array.Copy(buffer, num, this.LineBuffer, this.LineBufferPos, num5 - num);
				this.LineBufferPos += num5 - num;
				if (num2 == -1)
				{
					break;
				}
				this.ParseLine(this.LineBuffer, this.LineBufferPos);
				this.LineBufferPos = 0;
				num = num2 + num3;
				if (num2 == -1 || num >= count)
				{
					return;
				}
			}
		}

		// Token: 0x0600371E RID: 14110 RVA: 0x00110690 File Offset: 0x0010E890
		private void ParseLine(byte[] buffer, int count)
		{
			if (count == 0)
			{
				if (this.CurrentMessage != null)
				{
					object frameLock = this.FrameLock;
					lock (frameLock)
					{
						this.CompletedMessages.Add(this.CurrentMessage);
					}
					this.CurrentMessage = null;
				}
				return;
			}
			if (buffer[0] == 58)
			{
				return;
			}
			int num = -1;
			int num2 = 0;
			while (num2 < count && num == -1)
			{
				if (buffer[num2] == 58)
				{
					num = num2;
				}
				num2++;
			}
			string @string;
			string text;
			if (num != -1)
			{
				@string = Encoding.UTF8.GetString(buffer, 0, num);
				if (num + 1 < count && buffer[num + 1] == 32)
				{
					num++;
				}
				num++;
				if (num >= count)
				{
					return;
				}
				text = Encoding.UTF8.GetString(buffer, num, count - num);
			}
			else
			{
				@string = Encoding.UTF8.GetString(buffer, 0, count);
				text = string.Empty;
			}
			if (this.CurrentMessage == null)
			{
				this.CurrentMessage = new Message();
			}
			if (@string == "id")
			{
				this.CurrentMessage.Id = text;
				return;
			}
			if (@string == "event")
			{
				this.CurrentMessage.Event = text;
				return;
			}
			if (@string == "data")
			{
				if (this.CurrentMessage.Data != null)
				{
					Message currentMessage = this.CurrentMessage;
					currentMessage.Data += Environment.NewLine;
				}
				Message currentMessage2 = this.CurrentMessage;
				currentMessage2.Data += text;
				return;
			}
			if (!(@string == "retry"))
			{
				return;
			}
			int num3;
			if (int.TryParse(text, out num3))
			{
				this.CurrentMessage.Retry = TimeSpan.FromMilliseconds((double)num3);
			}
		}

		// Token: 0x0600371F RID: 14111 RVA: 0x0011082C File Offset: 0x0010EA2C
		void IProtocol.HandleEvents()
		{
			object frameLock = this.FrameLock;
			lock (frameLock)
			{
				if (this.CompletedMessages.Count > 0)
				{
					if (this.OnMessage != null)
					{
						for (int i = 0; i < this.CompletedMessages.Count; i++)
						{
							try
							{
								this.OnMessage(this, this.CompletedMessages[i]);
							}
							catch (Exception ex)
							{
								HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnMessage", ex);
							}
						}
					}
					this.CompletedMessages.Clear();
				}
			}
			if (this.IsClosed)
			{
				this.CompletedMessages.Clear();
				if (this.OnClosed != null)
				{
					try
					{
						this.OnClosed(this);
					}
					catch (Exception ex2)
					{
						HTTPManager.Logger.Exception("EventSourceMessage", "HandleEvents - OnClosed", ex2);
					}
					finally
					{
						this.OnClosed = null;
					}
				}
			}
		}

		// Token: 0x04002379 RID: 9081
		public Action<EventSourceResponse, Message> OnMessage;

		// Token: 0x0400237A RID: 9082
		public Action<EventSourceResponse> OnClosed;

		// Token: 0x0400237B RID: 9083
		private object FrameLock = new object();

		// Token: 0x0400237C RID: 9084
		private byte[] LineBuffer = new byte[1024];

		// Token: 0x0400237D RID: 9085
		private int LineBufferPos;

		// Token: 0x0400237E RID: 9086
		private Message CurrentMessage;

		// Token: 0x0400237F RID: 9087
		private List<Message> CompletedMessages = new List<Message>();
	}
}
