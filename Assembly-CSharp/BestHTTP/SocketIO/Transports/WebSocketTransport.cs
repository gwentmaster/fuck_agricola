using System;
using System.Collections.Generic;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.WebSocket;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020005A2 RID: 1442
	internal sealed class WebSocketTransport : ITransport
	{
		// Token: 0x170006C0 RID: 1728
		// (get) Token: 0x0600351C RID: 13596 RVA: 0x0000900B File Offset: 0x0000720B
		public TransportTypes Type
		{
			get
			{
				return TransportTypes.WebSocket;
			}
		}

		// Token: 0x170006C1 RID: 1729
		// (get) Token: 0x0600351D RID: 13597 RVA: 0x0010A87C File Offset: 0x00108A7C
		// (set) Token: 0x0600351E RID: 13598 RVA: 0x0010A884 File Offset: 0x00108A84
		public TransportStates State { get; private set; }

		// Token: 0x170006C2 RID: 1730
		// (get) Token: 0x0600351F RID: 13599 RVA: 0x0010A88D File Offset: 0x00108A8D
		// (set) Token: 0x06003520 RID: 13600 RVA: 0x0010A895 File Offset: 0x00108A95
		public SocketManager Manager { get; private set; }

		// Token: 0x170006C3 RID: 1731
		// (get) Token: 0x06003521 RID: 13601 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsRequestInProgress
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C4 RID: 1732
		// (get) Token: 0x06003522 RID: 13602 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsPollingInProgress
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170006C5 RID: 1733
		// (get) Token: 0x06003523 RID: 13603 RVA: 0x0010A89E File Offset: 0x00108A9E
		// (set) Token: 0x06003524 RID: 13604 RVA: 0x0010A8A6 File Offset: 0x00108AA6
		public WebSocket Implementation { get; private set; }

		// Token: 0x06003525 RID: 13605 RVA: 0x0010A8AF File Offset: 0x00108AAF
		public WebSocketTransport(SocketManager manager)
		{
			this.State = TransportStates.Closed;
			this.Manager = manager;
		}

		// Token: 0x06003526 RID: 13606 RVA: 0x0010A8C8 File Offset: 0x00108AC8
		public void Open()
		{
			if (this.State != TransportStates.Closed)
			{
				return;
			}
			string text = new UriBuilder(HTTPProtocolFactory.IsSecureProtocol(this.Manager.Uri) ? "wss" : "ws", this.Manager.Uri.Host, this.Manager.Uri.Port, this.Manager.Uri.GetRequestPathAndQueryURL()).Uri.ToString();
			string text2 = "{0}?EIO={1}&transport=websocket{3}";
			if (this.Manager.Handshake != null)
			{
				text2 += "&sid={2}";
			}
			bool flag = !this.Manager.Options.QueryParamsOnlyForHandshake || (this.Manager.Options.QueryParamsOnlyForHandshake && this.Manager.Handshake == null);
			Uri uri = new Uri(string.Format(text2, new object[]
			{
				text,
				4,
				(this.Manager.Handshake != null) ? this.Manager.Handshake.Sid : string.Empty,
				flag ? this.Manager.Options.BuildQueryParams() : string.Empty
			}));
			this.Implementation = new WebSocket(uri);
			this.Implementation.OnOpen = new OnWebSocketOpenDelegate(this.OnOpen);
			this.Implementation.OnMessage = new OnWebSocketMessageDelegate(this.OnMessage);
			this.Implementation.OnBinary = new OnWebSocketBinaryDelegate(this.OnBinary);
			this.Implementation.OnError = new OnWebSocketErrorDelegate(this.OnError);
			this.Implementation.OnClosed = new OnWebSocketClosedDelegate(this.OnClosed);
			this.Implementation.Open();
			this.State = TransportStates.Connecting;
		}

		// Token: 0x06003527 RID: 13607 RVA: 0x0010AA8C File Offset: 0x00108C8C
		public void Close()
		{
			if (this.State == TransportStates.Closed)
			{
				return;
			}
			this.State = TransportStates.Closed;
			if (this.Implementation != null)
			{
				this.Implementation.Close();
			}
			else
			{
				HTTPManager.Logger.Warning("WebSocketTransport", "Close - WebSocket Implementation already null!");
			}
			this.Implementation = null;
		}

		// Token: 0x06003528 RID: 13608 RVA: 0x00003022 File Offset: 0x00001222
		public void Poll()
		{
		}

		// Token: 0x06003529 RID: 13609 RVA: 0x0010AADC File Offset: 0x00108CDC
		private void OnOpen(WebSocket ws)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "OnOpen");
			this.State = TransportStates.Opening;
			if (this.Manager.UpgradingTransport == this)
			{
				this.Send(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", "probe", 0, 0));
			}
		}

		// Token: 0x0600352A RID: 13610 RVA: 0x0010AB38 File Offset: 0x00108D38
		private void OnMessage(WebSocket ws, string message)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "OnMessage: " + message);
			}
			try
			{
				Packet packet = new Packet(message);
				if (packet.AttachmentCount == 0)
				{
					this.OnPacket(packet);
				}
				else
				{
					this.PacketWithAttachment = packet;
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("WebSocketTransport", "OnMessage", ex);
			}
		}

		// Token: 0x0600352B RID: 13611 RVA: 0x0010ABC0 File Offset: 0x00108DC0
		private void OnBinary(WebSocket ws, byte[] data)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "OnBinary");
			}
			if (this.PacketWithAttachment != null)
			{
				this.PacketWithAttachment.AddAttachmentFromServer(data, false);
				if (this.PacketWithAttachment.HasAllAttachment)
				{
					try
					{
						this.OnPacket(this.PacketWithAttachment);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("WebSocketTransport", "OnBinary", ex);
					}
					finally
					{
						this.PacketWithAttachment = null;
					}
				}
			}
		}

		// Token: 0x0600352C RID: 13612 RVA: 0x0010AC64 File Offset: 0x00108E64
		private void OnError(WebSocket ws, Exception ex)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			string err = string.Empty;
			if (ex != null)
			{
				err = ex.Message + " " + ex.StackTrace;
			}
			else
			{
				switch (ws.InternalRequest.State)
				{
				case HTTPRequestStates.Finished:
					if (ws.InternalRequest.Response.IsSuccess || ws.InternalRequest.Response.StatusCode == 101)
					{
						err = string.Format("Request finished. Status Code: {0} Message: {1}", ws.InternalRequest.Response.StatusCode.ToString(), ws.InternalRequest.Response.Message);
					}
					else
					{
						err = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", ws.InternalRequest.Response.StatusCode, ws.InternalRequest.Response.Message, ws.InternalRequest.Response.DataAsText);
					}
					break;
				case HTTPRequestStates.Error:
					err = (("Request Finished with Error! : " + ws.InternalRequest.Exception != null) ? (ws.InternalRequest.Exception.Message + " " + ws.InternalRequest.Exception.StackTrace) : string.Empty);
					break;
				case HTTPRequestStates.Aborted:
					err = "Request Aborted!";
					break;
				case HTTPRequestStates.ConnectionTimedOut:
					err = "Connection Timed Out!";
					break;
				case HTTPRequestStates.TimedOut:
					err = "Processing the request Timed Out!";
					break;
				}
			}
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).OnTransportError(this, err);
				return;
			}
			this.Manager.UpgradingTransport = null;
		}

		// Token: 0x0600352D RID: 13613 RVA: 0x0010ADF8 File Offset: 0x00108FF8
		private void OnClosed(WebSocket ws, ushort code, string message)
		{
			if (ws != this.Implementation)
			{
				return;
			}
			HTTPManager.Logger.Information("WebSocketTransport", "OnClosed");
			this.Close();
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).TryToReconnect();
				return;
			}
			this.Manager.UpgradingTransport = null;
		}

		// Token: 0x0600352E RID: 13614 RVA: 0x0010AE50 File Offset: 0x00109050
		public void Send(Packet packet)
		{
			if (this.State == TransportStates.Closed || this.State == TransportStates.Paused)
			{
				return;
			}
			string text = packet.Encode();
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("WebSocketTransport", "Send: " + text);
			}
			if (packet.AttachmentCount != 0 || (packet.Attachments != null && packet.Attachments.Count != 0))
			{
				if (packet.Attachments == null)
				{
					throw new ArgumentException("packet.Attachments are null!");
				}
				if (packet.AttachmentCount != packet.Attachments.Count)
				{
					throw new ArgumentException("packet.AttachmentCount != packet.Attachments.Count. Use the packet.AddAttachment function to add data to a packet!");
				}
			}
			this.Implementation.Send(text);
			if (packet.AttachmentCount != 0)
			{
				int num = packet.Attachments[0].Length + 1;
				for (int i = 1; i < packet.Attachments.Count; i++)
				{
					if (packet.Attachments[i].Length + 1 > num)
					{
						num = packet.Attachments[i].Length + 1;
					}
				}
				if (this.Buffer == null || this.Buffer.Length < num)
				{
					Array.Resize<byte>(ref this.Buffer, num);
				}
				for (int j = 0; j < packet.AttachmentCount; j++)
				{
					this.Buffer[0] = 4;
					Array.Copy(packet.Attachments[j], 0, this.Buffer, 1, packet.Attachments[j].Length);
					this.Implementation.Send(this.Buffer, 0UL, (ulong)((long)packet.Attachments[j].Length + 1L));
				}
			}
		}

		// Token: 0x0600352F RID: 13615 RVA: 0x0010AFD4 File Offset: 0x001091D4
		public void Send(List<Packet> packets)
		{
			for (int i = 0; i < packets.Count; i++)
			{
				this.Send(packets[i]);
			}
			packets.Clear();
		}

		// Token: 0x06003530 RID: 13616 RVA: 0x0010B008 File Offset: 0x00109208
		private void OnPacket(Packet packet)
		{
			TransportEventTypes transportEvent = packet.TransportEvent;
			if (transportEvent != TransportEventTypes.Open)
			{
				if (transportEvent == TransportEventTypes.Pong)
				{
					if (packet.Payload == "probe")
					{
						this.State = TransportStates.Open;
						((IManager)this.Manager).OnTransportProbed(this);
					}
				}
			}
			else if (this.State != TransportStates.Opening)
			{
				HTTPManager.Logger.Warning("PollingTransport", "Received 'Open' packet while state is '" + this.State.ToString() + "'");
			}
			else
			{
				this.State = TransportStates.Open;
			}
			if (this.Manager.UpgradingTransport != this)
			{
				((IManager)this.Manager).OnPacket(packet);
			}
		}

		// Token: 0x040022B5 RID: 8885
		private Packet PacketWithAttachment;

		// Token: 0x040022B6 RID: 8886
		private byte[] Buffer;
	}
}
