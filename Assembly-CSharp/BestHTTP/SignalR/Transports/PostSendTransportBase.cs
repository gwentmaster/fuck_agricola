using System;
using System.Collections.Generic;
using BestHTTP.Forms;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x020005BB RID: 1467
	public abstract class PostSendTransportBase : TransportBase
	{
		// Token: 0x060035F6 RID: 13814 RVA: 0x0010DA14 File Offset: 0x0010BC14
		public PostSendTransportBase(string name, Connection con) : base(name, con)
		{
		}

		// Token: 0x060035F7 RID: 13815 RVA: 0x0010DA2C File Offset: 0x0010BC2C
		protected override void SendImpl(string json)
		{
			HTTPRequest httprequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Send, this), HTTPMethods.Post, true, true, new OnRequestFinishedDelegate(this.OnSendRequestFinished));
			httprequest.FormUsage = HTTPFormUsage.UrlEncoded;
			httprequest.AddField("data", json);
			base.Connection.PrepareRequest(httprequest, RequestTypes.Send);
			httprequest.Priority = -1;
			httprequest.Send();
			this.sendRequestQueue.Add(httprequest);
		}

		// Token: 0x060035F8 RID: 13816 RVA: 0x0010DA98 File Offset: 0x0010BC98
		private void OnSendRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.sendRequestQueue.Remove(req);
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Send - Request Finished Successfully! " + resp.DataAsText);
					if (!string.IsNullOrEmpty(resp.DataAsText))
					{
						IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
						if (serverMessage != null)
						{
							base.Connection.OnMessage(serverMessage);
						}
					}
				}
				else
				{
					text = string.Format("Send - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Send - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "Send - Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Send - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Send - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x0400232A RID: 9002
		protected List<HTTPRequest> sendRequestQueue = new List<HTTPRequest>();
	}
}
