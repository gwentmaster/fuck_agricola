using System;
using System.Collections.Generic;
using System.Threading;
using BestHTTP.Extensions;
using BestHTTP.SocketIO.Events;
using BestHTTP.SocketIO.JsonEncoders;
using BestHTTP.SocketIO.Transports;

namespace BestHTTP.SocketIO
{
	// Token: 0x0200059C RID: 1436
	public sealed class SocketManager : IHeartbeat, IManager
	{
		// Token: 0x1700069E RID: 1694
		// (get) Token: 0x060034BB RID: 13499 RVA: 0x001091AA File Offset: 0x001073AA
		// (set) Token: 0x060034BC RID: 13500 RVA: 0x001091B2 File Offset: 0x001073B2
		public SocketManager.States State
		{
			get
			{
				return this.state;
			}
			private set
			{
				this.PreviousState = this.state;
				this.state = value;
			}
		}

		// Token: 0x1700069F RID: 1695
		// (get) Token: 0x060034BD RID: 13501 RVA: 0x001091C7 File Offset: 0x001073C7
		// (set) Token: 0x060034BE RID: 13502 RVA: 0x001091CF File Offset: 0x001073CF
		public SocketOptions Options { get; private set; }

		// Token: 0x170006A0 RID: 1696
		// (get) Token: 0x060034BF RID: 13503 RVA: 0x001091D8 File Offset: 0x001073D8
		// (set) Token: 0x060034C0 RID: 13504 RVA: 0x001091E0 File Offset: 0x001073E0
		public Uri Uri { get; private set; }

		// Token: 0x170006A1 RID: 1697
		// (get) Token: 0x060034C1 RID: 13505 RVA: 0x001091E9 File Offset: 0x001073E9
		// (set) Token: 0x060034C2 RID: 13506 RVA: 0x001091F1 File Offset: 0x001073F1
		public HandshakeData Handshake { get; private set; }

		// Token: 0x170006A2 RID: 1698
		// (get) Token: 0x060034C3 RID: 13507 RVA: 0x001091FA File Offset: 0x001073FA
		// (set) Token: 0x060034C4 RID: 13508 RVA: 0x00109202 File Offset: 0x00107402
		public ITransport Transport { get; private set; }

		// Token: 0x170006A3 RID: 1699
		// (get) Token: 0x060034C5 RID: 13509 RVA: 0x0010920B File Offset: 0x0010740B
		// (set) Token: 0x060034C6 RID: 13510 RVA: 0x00109213 File Offset: 0x00107413
		public ulong RequestCounter { get; internal set; }

		// Token: 0x170006A4 RID: 1700
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x0010921C File Offset: 0x0010741C
		public Socket Socket
		{
			get
			{
				return this.GetSocket();
			}
		}

		// Token: 0x170006A5 RID: 1701
		public Socket this[string nsp]
		{
			get
			{
				return this.GetSocket(nsp);
			}
		}

		// Token: 0x170006A6 RID: 1702
		// (get) Token: 0x060034C9 RID: 13513 RVA: 0x0010922D File Offset: 0x0010742D
		// (set) Token: 0x060034CA RID: 13514 RVA: 0x00109235 File Offset: 0x00107435
		public int ReconnectAttempts { get; private set; }

		// Token: 0x170006A7 RID: 1703
		// (get) Token: 0x060034CB RID: 13515 RVA: 0x0010923E File Offset: 0x0010743E
		// (set) Token: 0x060034CC RID: 13516 RVA: 0x00109246 File Offset: 0x00107446
		public IJsonEncoder Encoder { get; set; }

		// Token: 0x170006A8 RID: 1704
		// (get) Token: 0x060034CD RID: 13517 RVA: 0x00109250 File Offset: 0x00107450
		internal uint Timestamp
		{
			get
			{
				return (uint)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalMilliseconds;
			}
		}

		// Token: 0x170006A9 RID: 1705
		// (get) Token: 0x060034CE RID: 13518 RVA: 0x0010927F File Offset: 0x0010747F
		internal int NextAckId
		{
			get
			{
				return Interlocked.Increment(ref this.nextAckId);
			}
		}

		// Token: 0x170006AA RID: 1706
		// (get) Token: 0x060034CF RID: 13519 RVA: 0x0010928C File Offset: 0x0010748C
		// (set) Token: 0x060034D0 RID: 13520 RVA: 0x00109294 File Offset: 0x00107494
		internal SocketManager.States PreviousState { get; private set; }

		// Token: 0x170006AB RID: 1707
		// (get) Token: 0x060034D1 RID: 13521 RVA: 0x0010929D File Offset: 0x0010749D
		// (set) Token: 0x060034D2 RID: 13522 RVA: 0x001092A5 File Offset: 0x001074A5
		internal ITransport UpgradingTransport { get; set; }

		// Token: 0x060034D3 RID: 13523 RVA: 0x001092AE File Offset: 0x001074AE
		public SocketManager(Uri uri) : this(uri, new SocketOptions())
		{
		}

		// Token: 0x060034D4 RID: 13524 RVA: 0x001092BC File Offset: 0x001074BC
		public SocketManager(Uri uri, SocketOptions options)
		{
			this.Uri = uri;
			this.Options = options;
			this.State = SocketManager.States.Initial;
			this.PreviousState = SocketManager.States.Initial;
			this.Encoder = SocketManager.DefaultEncoder;
		}

		// Token: 0x060034D5 RID: 13525 RVA: 0x00109322 File Offset: 0x00107522
		public Socket GetSocket()
		{
			return this.GetSocket("/");
		}

		// Token: 0x060034D6 RID: 13526 RVA: 0x00109330 File Offset: 0x00107530
		public Socket GetSocket(string nsp)
		{
			if (string.IsNullOrEmpty(nsp))
			{
				throw new ArgumentNullException("Namespace parameter is null or empty!");
			}
			Socket socket = null;
			if (!this.Namespaces.TryGetValue(nsp, out socket))
			{
				socket = new Socket(nsp, this);
				this.Namespaces.Add(nsp, socket);
				this.Sockets.Add(socket);
				((ISocket)socket).Open();
			}
			return socket;
		}

		// Token: 0x060034D7 RID: 13527 RVA: 0x0010938A File Offset: 0x0010758A
		void IManager.Remove(Socket socket)
		{
			this.Namespaces.Remove(socket.Namespace);
			this.Sockets.Remove(socket);
			if (this.Sockets.Count == 0)
			{
				this.Close();
			}
		}

		// Token: 0x060034D8 RID: 13528 RVA: 0x001093C0 File Offset: 0x001075C0
		public void Open()
		{
			if (this.State != SocketManager.States.Initial && this.State != SocketManager.States.Closed && this.State != SocketManager.States.Reconnecting)
			{
				return;
			}
			HTTPManager.Logger.Information("SocketManager", "Opening");
			this.ReconnectAt = DateTime.MinValue;
			TransportTypes connectWith = this.Options.ConnectWith;
			if (connectWith != TransportTypes.Polling)
			{
				if (connectWith == TransportTypes.WebSocket)
				{
					this.Transport = new WebSocketTransport(this);
				}
			}
			else
			{
				this.Transport = new PollingTransport(this);
			}
			this.Transport.Open();
			((IManager)this).EmitEvent("connecting", Array.Empty<object>());
			this.State = SocketManager.States.Opening;
			this.ConnectionStarted = DateTime.UtcNow;
			HTTPManager.Heartbeats.Subscribe(this);
			this.GetSocket("/");
		}

		// Token: 0x060034D9 RID: 13529 RVA: 0x0010947A File Offset: 0x0010767A
		public void Close()
		{
			((IManager)this).Close(true);
		}

		// Token: 0x060034DA RID: 13530 RVA: 0x00109484 File Offset: 0x00107684
		void IManager.Close(bool removeSockets)
		{
			if (this.State == SocketManager.States.Closed || this.closing)
			{
				return;
			}
			this.closing = true;
			HTTPManager.Logger.Information("SocketManager", "Closing");
			HTTPManager.Heartbeats.Unsubscribe(this);
			if (removeSockets)
			{
				while (this.Sockets.Count > 0)
				{
					((ISocket)this.Sockets[this.Sockets.Count - 1]).Disconnect(removeSockets);
				}
			}
			else
			{
				for (int i = 0; i < this.Sockets.Count; i++)
				{
					((ISocket)this.Sockets[i]).Disconnect(removeSockets);
				}
			}
			this.State = SocketManager.States.Closed;
			this.LastHeartbeat = DateTime.MinValue;
			if (this.OfflinePackets != null)
			{
				this.OfflinePackets.Clear();
			}
			if (removeSockets)
			{
				this.Namespaces.Clear();
			}
			this.Handshake = null;
			if (this.Transport != null)
			{
				this.Transport.Close();
			}
			this.Transport = null;
			this.closing = false;
		}

		// Token: 0x060034DB RID: 13531 RVA: 0x00109580 File Offset: 0x00107780
		void IManager.TryToReconnect()
		{
			if (this.State == SocketManager.States.Reconnecting || this.State == SocketManager.States.Closed)
			{
				return;
			}
			if (!this.Options.Reconnection)
			{
				this.Close();
				return;
			}
			int num = this.ReconnectAttempts + 1;
			this.ReconnectAttempts = num;
			if (num >= this.Options.ReconnectionAttempts)
			{
				((IManager)this).EmitEvent("reconnect_failed", Array.Empty<object>());
				this.Close();
				return;
			}
			Random random = new Random();
			int num2 = (int)this.Options.ReconnectionDelay.TotalMilliseconds * this.ReconnectAttempts;
			this.ReconnectAt = DateTime.UtcNow + TimeSpan.FromMilliseconds((double)Math.Min(random.Next((int)((float)num2 - (float)num2 * this.Options.RandomizationFactor), (int)((float)num2 + (float)num2 * this.Options.RandomizationFactor)), (int)this.Options.ReconnectionDelayMax.TotalMilliseconds));
			((IManager)this).Close(false);
			this.State = SocketManager.States.Reconnecting;
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				((ISocket)this.Sockets[i]).Open();
			}
			HTTPManager.Heartbeats.Subscribe(this);
			HTTPManager.Logger.Information("SocketManager", "Reconnecting");
		}

		// Token: 0x060034DC RID: 13532 RVA: 0x001096C0 File Offset: 0x001078C0
		bool IManager.OnTransportConnected(ITransport trans)
		{
			if (this.State != SocketManager.States.Opening)
			{
				return false;
			}
			if (this.PreviousState == SocketManager.States.Reconnecting)
			{
				((IManager)this).EmitEvent("reconnect", Array.Empty<object>());
			}
			this.State = SocketManager.States.Open;
			this.LastPongReceived = DateTime.UtcNow;
			this.ReconnectAttempts = 0;
			this.SendOfflinePackets();
			HTTPManager.Logger.Information("SocketManager", "Open");
			if (this.Transport.Type != TransportTypes.WebSocket && this.Handshake.Upgrades.Contains("websocket"))
			{
				this.UpgradingTransport = new WebSocketTransport(this);
				this.UpgradingTransport.Open();
			}
			return true;
		}

		// Token: 0x060034DD RID: 13533 RVA: 0x00109761 File Offset: 0x00107961
		void IManager.OnTransportError(ITransport trans, string err)
		{
			((IManager)this).EmitError(SocketIOErrors.Internal, err);
			trans.Close();
			((IManager)this).TryToReconnect();
		}

		// Token: 0x060034DE RID: 13534 RVA: 0x00109777 File Offset: 0x00107977
		void IManager.OnTransportProbed(ITransport trans)
		{
			HTTPManager.Logger.Information("SocketManager", "\"probe\" packet received");
			this.Options.ConnectWith = trans.Type;
			this.State = SocketManager.States.Paused;
		}

		// Token: 0x060034DF RID: 13535 RVA: 0x001097A5 File Offset: 0x001079A5
		private ITransport SelectTransport()
		{
			if (this.State != SocketManager.States.Open)
			{
				return null;
			}
			if (!this.Transport.IsRequestInProgress)
			{
				return this.Transport;
			}
			return null;
		}

		// Token: 0x060034E0 RID: 13536 RVA: 0x001097C8 File Offset: 0x001079C8
		private void SendOfflinePackets()
		{
			ITransport transport = this.SelectTransport();
			if (this.OfflinePackets != null && this.OfflinePackets.Count > 0 && transport != null)
			{
				transport.Send(this.OfflinePackets);
				this.OfflinePackets.Clear();
			}
		}

		// Token: 0x060034E1 RID: 13537 RVA: 0x0010980C File Offset: 0x00107A0C
		void IManager.SendPacket(Packet packet)
		{
			ITransport transport = this.SelectTransport();
			if (transport != null)
			{
				try
				{
					transport.Send(packet);
					return;
				}
				catch (Exception ex)
				{
					((IManager)this).EmitError(SocketIOErrors.Internal, ex.Message + " " + ex.StackTrace);
					return;
				}
			}
			if (this.OfflinePackets == null)
			{
				this.OfflinePackets = new List<Packet>();
			}
			this.OfflinePackets.Add(packet.Clone());
		}

		// Token: 0x060034E2 RID: 13538 RVA: 0x00109880 File Offset: 0x00107A80
		void IManager.OnPacket(Packet packet)
		{
			if (this.State == SocketManager.States.Closed)
			{
				return;
			}
			switch (packet.TransportEvent)
			{
			case TransportEventTypes.Open:
				if (this.Handshake == null)
				{
					this.Handshake = new HandshakeData();
					if (!this.Handshake.Parse(packet.Payload))
					{
						HTTPManager.Logger.Warning("SocketManager", "Expected handshake data, but wasn't able to pars. Payload: " + packet.Payload);
					}
					((IManager)this).OnTransportConnected(this.Transport);
					return;
				}
				break;
			case TransportEventTypes.Ping:
				((IManager)this).SendPacket(new Packet(TransportEventTypes.Pong, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				break;
			case TransportEventTypes.Pong:
				this.LastPongReceived = DateTime.UtcNow;
				break;
			}
			Socket socket = null;
			if (this.Namespaces.TryGetValue(packet.Namespace, out socket))
			{
				((ISocket)socket).OnPacket(packet);
				return;
			}
			HTTPManager.Logger.Warning("SocketManager", "Namespace \"" + packet.Namespace + "\" not found!");
		}

		// Token: 0x060034E3 RID: 13539 RVA: 0x00109974 File Offset: 0x00107B74
		public void EmitAll(string eventName, params object[] args)
		{
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				this.Sockets[i].Emit(eventName, args);
			}
		}

		// Token: 0x060034E4 RID: 13540 RVA: 0x001099AC File Offset: 0x00107BAC
		void IManager.EmitEvent(string eventName, params object[] args)
		{
			Socket socket = null;
			if (this.Namespaces.TryGetValue("/", out socket))
			{
				((ISocket)socket).EmitEvent(eventName, args);
			}
		}

		// Token: 0x060034E5 RID: 13541 RVA: 0x001099D7 File Offset: 0x00107BD7
		void IManager.EmitEvent(SocketIOEventTypes type, params object[] args)
		{
			((IManager)this).EmitEvent(EventNames.GetNameFor(type), args);
		}

		// Token: 0x060034E6 RID: 13542 RVA: 0x001099E6 File Offset: 0x00107BE6
		void IManager.EmitError(SocketIOErrors errCode, string msg)
		{
			((IManager)this).EmitEvent(SocketIOEventTypes.Error, new object[]
			{
				new Error(errCode, msg)
			});
		}

		// Token: 0x060034E7 RID: 13543 RVA: 0x00109A00 File Offset: 0x00107C00
		void IManager.EmitAll(string eventName, params object[] args)
		{
			for (int i = 0; i < this.Sockets.Count; i++)
			{
				((ISocket)this.Sockets[i]).EmitEvent(eventName, args);
			}
		}

		// Token: 0x060034E8 RID: 13544 RVA: 0x00109A38 File Offset: 0x00107C38
		void IHeartbeat.OnHeartbeatUpdate(TimeSpan dif)
		{
			switch (this.State)
			{
			case SocketManager.States.Opening:
				if (DateTime.UtcNow - this.ConnectionStarted >= this.Options.Timeout)
				{
					((IManager)this).EmitError(SocketIOErrors.Internal, "Connection timed out!");
					((IManager)this).EmitEvent("connect_error", Array.Empty<object>());
					((IManager)this).EmitEvent("connect_timeout", Array.Empty<object>());
					((IManager)this).TryToReconnect();
					return;
				}
				return;
			case SocketManager.States.Open:
				break;
			case SocketManager.States.Paused:
				if (this.Transport.IsRequestInProgress || this.Transport.IsPollingInProgress)
				{
					return;
				}
				this.State = SocketManager.States.Open;
				this.Transport.Close();
				this.Transport = this.UpgradingTransport;
				this.UpgradingTransport = null;
				this.Transport.Send(new Packet(TransportEventTypes.Upgrade, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				break;
			case SocketManager.States.Reconnecting:
				if (this.ReconnectAt != DateTime.MinValue && DateTime.UtcNow >= this.ReconnectAt)
				{
					((IManager)this).EmitEvent("reconnect_attempt", Array.Empty<object>());
					((IManager)this).EmitEvent("reconnecting", Array.Empty<object>());
					this.Open();
					return;
				}
				return;
			default:
				return;
			}
			ITransport transport = null;
			if (this.Transport != null && this.Transport.State == TransportStates.Open)
			{
				transport = this.Transport;
			}
			if (transport == null || transport.State != TransportStates.Open)
			{
				return;
			}
			transport.Poll();
			this.SendOfflinePackets();
			if (this.LastHeartbeat == DateTime.MinValue)
			{
				this.LastHeartbeat = DateTime.UtcNow;
				return;
			}
			if (DateTime.UtcNow - this.LastHeartbeat > this.Handshake.PingInterval)
			{
				((IManager)this).SendPacket(new Packet(TransportEventTypes.Ping, SocketIOEventTypes.Unknown, "/", string.Empty, 0, 0));
				this.LastHeartbeat = DateTime.UtcNow;
			}
			if (DateTime.UtcNow - this.LastPongReceived > this.Handshake.PingTimeout)
			{
				((IManager)this).TryToReconnect();
			}
		}

		// Token: 0x04002283 RID: 8835
		public static IJsonEncoder DefaultEncoder = new DefaultJSonEncoder();

		// Token: 0x04002284 RID: 8836
		public const int MinProtocolVersion = 4;

		// Token: 0x04002285 RID: 8837
		private SocketManager.States state;

		// Token: 0x0400228D RID: 8845
		private int nextAckId;

		// Token: 0x04002290 RID: 8848
		private Dictionary<string, Socket> Namespaces = new Dictionary<string, Socket>();

		// Token: 0x04002291 RID: 8849
		private List<Socket> Sockets = new List<Socket>();

		// Token: 0x04002292 RID: 8850
		private List<Packet> OfflinePackets;

		// Token: 0x04002293 RID: 8851
		private DateTime LastHeartbeat = DateTime.MinValue;

		// Token: 0x04002294 RID: 8852
		private DateTime LastPongReceived = DateTime.MinValue;

		// Token: 0x04002295 RID: 8853
		private DateTime ReconnectAt;

		// Token: 0x04002296 RID: 8854
		private DateTime ConnectionStarted;

		// Token: 0x04002297 RID: 8855
		private bool closing;

		// Token: 0x020008FA RID: 2298
		public enum States
		{
			// Token: 0x04003027 RID: 12327
			Initial,
			// Token: 0x04003028 RID: 12328
			Closed,
			// Token: 0x04003029 RID: 12329
			Opening,
			// Token: 0x0400302A RID: 12330
			Open,
			// Token: 0x0400302B RID: 12331
			Paused,
			// Token: 0x0400302C RID: 12332
			Reconnecting
		}
	}
}
