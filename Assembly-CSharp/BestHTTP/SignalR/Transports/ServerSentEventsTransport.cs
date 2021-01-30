using System;
using BestHTTP.ServerSentEvents;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x020005BC RID: 1468
	public sealed class ServerSentEventsTransport : PostSendTransportBase
	{
		// Token: 0x170006ED RID: 1773
		// (get) Token: 0x060035F9 RID: 13817 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool SupportsKeepAlive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006EE RID: 1774
		// (get) Token: 0x060035FA RID: 13818 RVA: 0x0000900B File Offset: 0x0000720B
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.ServerSentEvents;
			}
		}

		// Token: 0x060035FB RID: 13819 RVA: 0x0010DBD7 File Offset: 0x0010BDD7
		public ServerSentEventsTransport(Connection con) : base("serverSentEvents", con)
		{
		}

		// Token: 0x060035FC RID: 13820 RVA: 0x0010DBE8 File Offset: 0x0010BDE8
		public override void Connect()
		{
			if (this.EventSource != null)
			{
				HTTPManager.Logger.Warning("ServerSentEventsTransport", "Start - EventSource already created!");
				return;
			}
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			Uri uri = base.Connection.BuildUri(type, this);
			this.EventSource = new EventSource(uri);
			this.EventSource.OnOpen += this.OnEventSourceOpen;
			this.EventSource.OnMessage += this.OnEventSourceMessage;
			this.EventSource.OnError += this.OnEventSourceError;
			this.EventSource.OnClosed += this.OnEventSourceClosed;
			this.EventSource.OnRetry += ((EventSource es) => false);
			this.EventSource.Open();
		}

		// Token: 0x060035FD RID: 13821 RVA: 0x0010DCDC File Offset: 0x0010BEDC
		public override void Stop()
		{
			this.EventSource.OnOpen -= this.OnEventSourceOpen;
			this.EventSource.OnMessage -= this.OnEventSourceMessage;
			this.EventSource.OnError -= this.OnEventSourceError;
			this.EventSource.OnClosed -= this.OnEventSourceClosed;
			this.EventSource.Close();
			this.EventSource = null;
		}

		// Token: 0x060035FE RID: 13822 RVA: 0x00003022 File Offset: 0x00001222
		protected override void Started()
		{
		}

		// Token: 0x060035FF RID: 13823 RVA: 0x0010DD57 File Offset: 0x0010BF57
		public override void Abort()
		{
			base.Abort();
			this.EventSource.Close();
		}

		// Token: 0x06003600 RID: 13824 RVA: 0x0010DD6A File Offset: 0x0010BF6A
		protected override void Aborted()
		{
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
			}
		}

		// Token: 0x06003601 RID: 13825 RVA: 0x0010DD7C File Offset: 0x0010BF7C
		private void OnEventSourceOpen(EventSource eventSource)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceOpen");
		}

		// Token: 0x06003602 RID: 13826 RVA: 0x0010DDA0 File Offset: 0x0010BFA0
		private void OnEventSourceMessage(EventSource eventSource, Message message)
		{
			if (message.Data.Equals("initialized"))
			{
				base.OnConnected();
				return;
			}
			IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, message.Data);
			if (serverMessage != null)
			{
				base.Connection.OnMessage(serverMessage);
			}
		}

		// Token: 0x06003603 RID: 13827 RVA: 0x0010DDEC File Offset: 0x0010BFEC
		private void OnEventSourceError(EventSource eventSource, string error)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceError");
			if (base.State == TransportStates.Reconnecting)
			{
				this.Connect();
				return;
			}
			if (base.State == TransportStates.Closed)
			{
				return;
			}
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
				return;
			}
			base.Connection.Error(error);
		}

		// Token: 0x06003604 RID: 13828 RVA: 0x0010DE4F File Offset: 0x0010C04F
		private void OnEventSourceClosed(EventSource eventSource)
		{
			HTTPManager.Logger.Information("Transport - " + base.Name, "OnEventSourceClosed");
			this.OnEventSourceError(eventSource, "EventSource Closed!");
		}

		// Token: 0x0400232B RID: 9003
		private EventSource EventSource;
	}
}
