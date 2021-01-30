using System;
using BestHTTP.SignalR.Messages;
using BestHTTP.WebSocket;

namespace BestHTTP.SignalR.Transports
{
	// Token: 0x020005BF RID: 1471
	public sealed class WebSocketTransport : TransportBase
	{
		// Token: 0x170006F4 RID: 1780
		// (get) Token: 0x06003622 RID: 13858 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool SupportsKeepAlive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06003623 RID: 13859 RVA: 0x0002A062 File Offset: 0x00028262
		public override TransportTypes Type
		{
			get
			{
				return TransportTypes.WebSocket;
			}
		}

		// Token: 0x06003624 RID: 13860 RVA: 0x0010E4F0 File Offset: 0x0010C6F0
		public WebSocketTransport(Connection connection) : base("webSockets", connection)
		{
		}

		// Token: 0x06003625 RID: 13861 RVA: 0x0010E500 File Offset: 0x0010C700
		public override void Connect()
		{
			if (this.wSocket != null)
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Start - WebSocket already created!");
				return;
			}
			if (base.State != TransportStates.Reconnecting)
			{
				base.State = TransportStates.Connecting;
			}
			RequestTypes type = (base.State == TransportStates.Reconnecting) ? RequestTypes.Reconnect : RequestTypes.Connect;
			Uri uri = base.Connection.BuildUri(type, this);
			this.wSocket = new WebSocket(uri);
			WebSocket webSocket = this.wSocket;
			webSocket.OnOpen = (OnWebSocketOpenDelegate)Delegate.Combine(webSocket.OnOpen, new OnWebSocketOpenDelegate(this.WSocket_OnOpen));
			WebSocket webSocket2 = this.wSocket;
			webSocket2.OnMessage = (OnWebSocketMessageDelegate)Delegate.Combine(webSocket2.OnMessage, new OnWebSocketMessageDelegate(this.WSocket_OnMessage));
			WebSocket webSocket3 = this.wSocket;
			webSocket3.OnClosed = (OnWebSocketClosedDelegate)Delegate.Combine(webSocket3.OnClosed, new OnWebSocketClosedDelegate(this.WSocket_OnClosed));
			WebSocket webSocket4 = this.wSocket;
			webSocket4.OnErrorDesc = (OnWebSocketErrorDescriptionDelegate)Delegate.Combine(webSocket4.OnErrorDesc, new OnWebSocketErrorDescriptionDelegate(this.WSocket_OnError));
			base.Connection.PrepareRequest(this.wSocket.InternalRequest, type);
			this.wSocket.Open();
		}

		// Token: 0x06003626 RID: 13862 RVA: 0x0010E621 File Offset: 0x0010C821
		protected override void SendImpl(string json)
		{
			if (this.wSocket != null && this.wSocket.IsOpen)
			{
				this.wSocket.Send(json);
			}
		}

		// Token: 0x06003627 RID: 13863 RVA: 0x0010E644 File Offset: 0x0010C844
		public override void Stop()
		{
			if (this.wSocket != null)
			{
				this.wSocket.OnOpen = null;
				this.wSocket.OnMessage = null;
				this.wSocket.OnClosed = null;
				this.wSocket.OnErrorDesc = null;
				this.wSocket.Close();
				this.wSocket = null;
			}
		}

		// Token: 0x06003628 RID: 13864 RVA: 0x00003022 File Offset: 0x00001222
		protected override void Started()
		{
		}

		// Token: 0x06003629 RID: 13865 RVA: 0x0010E69B File Offset: 0x0010C89B
		protected override void Aborted()
		{
			if (this.wSocket != null && this.wSocket.IsOpen)
			{
				this.wSocket.Close();
				this.wSocket = null;
			}
		}

		// Token: 0x0600362A RID: 13866 RVA: 0x0010E6C4 File Offset: 0x0010C8C4
		private void WSocket_OnOpen(WebSocket webSocket)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnOpen");
			base.OnConnected();
		}

		// Token: 0x0600362B RID: 13867 RVA: 0x0010E6EC File Offset: 0x0010C8EC
		private void WSocket_OnMessage(WebSocket webSocket, string message)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			IServerMessage serverMessage = TransportBase.Parse(base.Connection.JsonEncoder, message);
			if (serverMessage != null)
			{
				base.Connection.OnMessage(serverMessage);
			}
		}

		// Token: 0x0600362C RID: 13868 RVA: 0x0010E724 File Offset: 0x0010C924
		private void WSocket_OnClosed(WebSocket webSocket, ushort code, string message)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			string text = code.ToString() + " : " + message;
			HTTPManager.Logger.Information("WebSocketTransport", "WSocket_OnClosed " + text);
			if (base.State == TransportStates.Closing)
			{
				base.State = TransportStates.Closed;
				return;
			}
			base.Connection.Error(text);
		}

		// Token: 0x0600362D RID: 13869 RVA: 0x0010E788 File Offset: 0x0010C988
		private void WSocket_OnError(WebSocket webSocket, string reason)
		{
			if (webSocket != this.wSocket)
			{
				return;
			}
			if (base.State == TransportStates.Closing || base.State == TransportStates.Closed)
			{
				base.AbortFinished();
				return;
			}
			HTTPManager.Logger.Error("WebSocketTransport", "WSocket_OnError " + reason);
			base.Connection.Error(reason);
		}

		// Token: 0x04002331 RID: 9009
		private WebSocket wSocket;
	}
}
