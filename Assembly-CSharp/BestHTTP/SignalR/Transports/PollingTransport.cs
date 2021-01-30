using System;
using BestHTTP.Extensions;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x020005BA RID: 1466
	public sealed class PollingTransport : PostSendTransportBase, IHeartbeat
	{
		// Token: 0x170006EB RID: 1771
		// (get) Token: 0x060035EB RID: 13803 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool SupportsKeepAlive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006EC RID: 1772
		// (get) Token: 0x060035EC RID: 13804 RVA: 0x000A5319 File Offset: 0x000A3519
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.LongPoll;
			}
		}

		// Token: 0x060035ED RID: 13805 RVA: 0x0010D55C File Offset: 0x0010B75C
		public PollingTransport(Connection connection) : base("longPolling", connection)
		{
			this.LastPoll = DateTime.MinValue;
			this.PollTimeout = connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
		}

		// Token: 0x060035EE RID: 13806 RVA: 0x0010D59C File Offset: 0x0010B79C
		public override void Connect()
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "Sending Open Request");
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			HTTPRequest httprequest = new HTTPRequest(base.Connection.BuildUri(type, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnConnectRequestFinished));
			base.Connection.PrepareRequest(httprequest, type);
			httprequest.Send();
		}

		// Token: 0x060035EF RID: 13807 RVA: 0x0010D61D File Offset: 0x0010B81D
		public override void Stop()
		{
			HTTPManager.Heartbeats.Unsubscribe(this);
			if (this.pollRequest != null)
			{
				this.pollRequest.Abort();
				this.pollRequest = null;
			}
		}

		// Token: 0x060035F0 RID: 13808 RVA: 0x0010D644 File Offset: 0x0010B844
		protected override void Started()
		{
			this.LastPoll = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
		}

		// Token: 0x060035F1 RID: 13809 RVA: 0x0010D65C File Offset: 0x0010B85C
		protected override void Aborted()
		{
			HTTPManager.Heartbeats.Unsubscribe(this);
		}

		// Token: 0x060035F2 RID: 13810 RVA: 0x0010D66C File Offset: 0x0010B86C
		private void OnConnectRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Connect - Request Finished Successfully! " + resp.DataAsText);
					base.OnConnected();
					IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
					if (serverMessage != null)
					{
						base.Connection.OnMessage(serverMessage);
						MultiMessage multiMessage = serverMessage as MultiMessage;
						if (multiMessage != null && multiMessage.PollDelay != null)
						{
							this.PollDelay = multiMessage.PollDelay.Value;
						}
					}
				}
				else
				{
					text = string.Format("Connect - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Connect - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.Aborted:
				text = "Connect - Request Aborted!";
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Connect - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Connect - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x060035F3 RID: 13811 RVA: 0x0010D7D0 File Offset: 0x0010B9D0
		private void OnPollRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			if (req.State == HTTPRequestStates.Aborted)
			{
				HTTPManager.Logger.Warning("Transport - " + base.Name, "Poll - Request Aborted!");
				return;
			}
			this.pollRequest = null;
			string text = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + base.Name, "Poll - Request Finished Successfully! " + resp.DataAsText);
					IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, resp.DataAsText);
					if (serverMessage != null)
					{
						base.Connection.OnMessage(serverMessage);
						MultiMessage multiMessage = serverMessage as MultiMessage;
						if (multiMessage != null && multiMessage.PollDelay != null)
						{
							this.PollDelay = multiMessage.PollDelay.Value;
						}
						this.LastPoll = DateTime.UtcNow;
					}
				}
				else
				{
					text = string.Format("Poll - Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText);
				}
				break;
			case HTTPRequestStates.Error:
				text = "Poll - Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				text = "Poll - Connection Timed Out!";
				break;
			case HTTPRequestStates.TimedOut:
				text = "Poll - Processing the request Timed Out!";
				break;
			}
			if (!string.IsNullOrEmpty(text))
			{
				base.Connection.Error(text);
			}
		}

		// Token: 0x060035F4 RID: 13812 RVA: 0x0010D958 File Offset: 0x0010BB58
		private void Poll()
		{
			this.pollRequest = new HTTPRequest(base.Connection.BuildUri(RequestTypes.Poll, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnPollRequestFinished));
			base.Connection.PrepareRequest(this.pollRequest, RequestTypes.Poll);
			this.pollRequest.Timeout = this.PollTimeout;
			this.pollRequest.Send();
		}

		// Token: 0x060035F5 RID: 13813 RVA: 0x0010D9BC File Offset: 0x0010BBBC
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			TransportStates state = base.State;
			if (state == TransportStates.Started && this.pollRequest == null && DateTime.UtcNow >= this.LastPoll + this.PollDelay + base.Connection.NegotiationResult.LongPollDelay)
			{
				this.Poll();
			}
		}

		// Token: 0x04002326 RID: 8998
		private DateTime LastPoll;

		// Token: 0x04002327 RID: 8999
		private TimeSpan PollDelay;

		// Token: 0x04002328 RID: 9000
		private TimeSpan PollTimeout;

		// Token: 0x04002329 RID: 9001
		private HTTPRequest pollRequest;
	}
}
