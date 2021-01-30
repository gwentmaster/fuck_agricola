using System;
using System.Collections.Generic;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x020005BE RID: 1470
	public abstract class TransportBase
	{
		// Token: 0x170006EF RID: 1775
		// (get) Token: 0x06003609 RID: 13833 RVA: 0x0010DE7C File Offset: 0x0010C07C
		// (set) Token: 0x0600360A RID: 13834 RVA: 0x0010DE84 File Offset: 0x0010C084
		public string Name { get; protected set; }

		// Token: 0x170006F0 RID: 1776
		// (get) Token: 0x0600360B RID: 13835
		public abstract bool SupportsKeepAlive { get; }

		// Token: 0x170006F1 RID: 1777
		// (get) Token: 0x0600360C RID: 13836
		public abstract TransportTypes Type { get; }

		// Token: 0x170006F2 RID: 1778
		// (get) Token: 0x0600360D RID: 13837 RVA: 0x0010DE8D File Offset: 0x0010C08D
		// (set) Token: 0x0600360E RID: 13838 RVA: 0x0010DE95 File Offset: 0x0010C095
		public IConnection Connection { get; protected set; }

		// Token: 0x170006F3 RID: 1779
		// (get) Token: 0x0600360F RID: 13839 RVA: 0x0010DE9E File Offset: 0x0010C09E
		// (set) Token: 0x06003610 RID: 13840 RVA: 0x0010DEA8 File Offset: 0x0010C0A8
		public TransportStates State
		{
			get
			{
				return this._state;
			}
			protected set
			{
				TransportStates state = this._state;
				this._state = value;
				if (this.OnStateChanged != null)
				{
					this.OnStateChanged(this, state, this._state);
				}
			}
		}

		// Token: 0x14000018 RID: 24
		// (add) Token: 0x06003611 RID: 13841 RVA: 0x0010DEE0 File Offset: 0x0010C0E0
		// (remove) Token: 0x06003612 RID: 13842 RVA: 0x0010DF18 File Offset: 0x0010C118
		public event OnTransportStateChangedDelegate OnStateChanged;

		// Token: 0x06003613 RID: 13843 RVA: 0x0010DF4D File Offset: 0x0010C14D
		public TransportBase(string name, Connection connection)
		{
			this.Name = name;
			this.Connection = connection;
			this.State = TransportStates.Initial;
		}

		// Token: 0x06003614 RID: 13844
		public abstract void Connect();

		// Token: 0x06003615 RID: 13845
		public abstract void Stop();

		// Token: 0x06003616 RID: 13846
		protected abstract void SendImpl(string json);

		// Token: 0x06003617 RID: 13847
		protected abstract void Started();

		// Token: 0x06003618 RID: 13848
		protected abstract void Aborted();

		// Token: 0x06003619 RID: 13849 RVA: 0x0010DF6A File Offset: 0x0010C16A
		protected void OnConnected()
		{
			if (this.State != TransportStates.Reconnecting)
			{
				this.Start();
				return;
			}
			this.Connection.TransportReconnected();
			this.Started();
			this.State = TransportStates.Started;
		}

		// Token: 0x0600361A RID: 13850 RVA: 0x0010DF94 File Offset: 0x0010C194
		protected void Start()
		{
			HTTPManager.Logger.Information("Transport - " + this.Name, "Sending Start Request");
			this.State = TransportStates.Starting;
			if (this.Connection.Protocol > ProtocolVersions.Protocol_2_0)
			{
				HTTPRequest httprequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Start, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnStartRequestFinished));
				httprequest.Tag = 0;
				httprequest.DisableRetry = true;
				httprequest.Timeout = this.Connection.NegotiationResult.ConnectionTimeout + TimeSpan.FromSeconds(10.0);
				this.Connection.PrepareRequest(httprequest, RequestTypes.Start);
				httprequest.Send();
				return;
			}
			this.State = TransportStates.Started;
			this.Started();
			this.Connection.TransportStarted();
		}

		// Token: 0x0600361B RID: 13851 RVA: 0x0010E064 File Offset: 0x0010C264
		private void OnStartRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			HTTPRequestStates state = req.State;
			if (state == HTTPRequestStates.Finished)
			{
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + this.Name, "Start - Returned: " + resp.DataAsText);
					string text = this.Connection.ParseResponse(resp.DataAsText);
					if (text != "started")
					{
						this.Connection.Error(string.Format("Expected 'started' response, but '{0}' found!", text));
						return;
					}
					this.State = TransportStates.Started;
					this.Started();
					this.Connection.TransportStarted();
					return;
				}
				else
				{
					HTTPManager.Logger.Warning("Transport - " + this.Name, string.Format("Start - request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
				}
			}
			HTTPManager.Logger.Information("Transport - " + this.Name, "Start request state: " + req.State.ToString());
			int num = (int)req.Tag;
			if (num++ < 5)
			{
				req.Tag = num;
				req.Send();
				return;
			}
			this.Connection.Error("Failed to send Start request.");
		}

		// Token: 0x0600361C RID: 13852 RVA: 0x0010E1C4 File Offset: 0x0010C3C4
		public virtual void Abort()
		{
			if (this.State != TransportStates.Started)
			{
				return;
			}
			this.State = TransportStates.Closing;
			HTTPRequest httprequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Abort, this), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnAbortRequestFinished));
			httprequest.Tag = 0;
			httprequest.DisableRetry = true;
			this.Connection.PrepareRequest(httprequest, RequestTypes.Abort);
			httprequest.Send();
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0010E22C File Offset: 0x0010C42C
		protected void AbortFinished()
		{
			this.State = TransportStates.Closed;
			this.Connection.TransportAborted();
			this.Aborted();
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0010E248 File Offset: 0x0010C448
		private void OnAbortRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			HTTPRequestStates state = req.State;
			if (state == HTTPRequestStates.Finished)
			{
				if (resp.IsSuccess)
				{
					HTTPManager.Logger.Information("Transport - " + this.Name, "Abort - Returned: " + resp.DataAsText);
					if (this.State == TransportStates.Closing)
					{
						this.AbortFinished();
						return;
					}
					return;
				}
				else
				{
					HTTPManager.Logger.Warning("Transport - " + this.Name, string.Format("Abort - Handshake request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
				}
			}
			HTTPManager.Logger.Information("Transport - " + this.Name, "Abort request state: " + req.State.ToString());
			int num = (int)req.Tag;
			if (num++ < 5)
			{
				req.Tag = num;
				req.Send();
				return;
			}
			this.Connection.Error("Failed to send Abort request!");
		}

		// Token: 0x0600361F RID: 13855 RVA: 0x0010E36C File Offset: 0x0010C56C
		public void Send(string jsonStr)
		{
			try
			{
				HTTPManager.Logger.Information("Transport - " + this.Name, "Sending: " + jsonStr);
				this.SendImpl(jsonStr);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("Transport - " + this.Name, "Send", ex);
			}
		}

		// Token: 0x06003620 RID: 13856 RVA: 0x0010E3DC File Offset: 0x0010C5DC
		public void Reconnect()
		{
			HTTPManager.Logger.Information("Transport - " + this.Name, "Reconnecting");
			this.Stop();
			this.State = TransportStates.Reconnecting;
			this.Connect();
		}

		// Token: 0x06003621 RID: 13857 RVA: 0x0010E410 File Offset: 0x0010C610
		public static IServerMessage Parse(IJsonEncoder encoder, string json)
		{
			if (string.IsNullOrEmpty(json))
			{
				HTTPManager.Logger.Error("MessageFactory", "Parse - called with empty or null string!");
				return null;
			}
			if (json.Length == 2 && json == "{}")
			{
				return new KeepAliveMessage();
			}
			IDictionary<string, object> dictionary = null;
			try
			{
				dictionary = encoder.DecodeMessage(json);
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("MessageFactory", "Parse - encoder.DecodeMessage", ex);
				return null;
			}
			if (dictionary == null)
			{
				HTTPManager.Logger.Error("MessageFactory", "Parse - Json Decode failed for json string: \"" + json + "\"");
				return null;
			}
			IServerMessage serverMessage;
			if (!dictionary.ContainsKey("C"))
			{
				if (!dictionary.ContainsKey("E"))
				{
					serverMessage = new ResultMessage();
				}
				else
				{
					serverMessage = new FailureMessage();
				}
			}
			else
			{
				serverMessage = new MultiMessage();
			}
			serverMessage.Parse(dictionary);
			return serverMessage;
		}

		// Token: 0x0400232C RID: 9004
		private const int MaxRetryCount = 5;

		// Token: 0x0400232F RID: 9007
		public TransportStates _state;
	}
}
