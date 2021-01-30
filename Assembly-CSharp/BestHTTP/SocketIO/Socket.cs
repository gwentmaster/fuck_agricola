using System;
using System.Collections.Generic;
using BestHTTP.JSON;
using BestHTTP.SocketIO.Events;

namespace BestHTTP.SocketIO
{
	// Token: 0x0200059B RID: 1435
	public sealed class Socket : ISocket
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x06003498 RID: 13464 RVA: 0x001089FA File Offset: 0x00106BFA
		// (set) Token: 0x06003499 RID: 13465 RVA: 0x00108A02 File Offset: 0x00106C02
		public SocketManager Manager { get; private set; }

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600349A RID: 13466 RVA: 0x00108A0B File Offset: 0x00106C0B
		// (set) Token: 0x0600349B RID: 13467 RVA: 0x00108A13 File Offset: 0x00106C13
		public string Namespace { get; private set; }

		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x0600349C RID: 13468 RVA: 0x00108A1C File Offset: 0x00106C1C
		// (set) Token: 0x0600349D RID: 13469 RVA: 0x00108A24 File Offset: 0x00106C24
		public string Id { get; private set; }

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x0600349E RID: 13470 RVA: 0x00108A2D File Offset: 0x00106C2D
		// (set) Token: 0x0600349F RID: 13471 RVA: 0x00108A35 File Offset: 0x00106C35
		public bool IsOpen { get; private set; }

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x060034A0 RID: 13472 RVA: 0x00108A3E File Offset: 0x00106C3E
		// (set) Token: 0x060034A1 RID: 13473 RVA: 0x00108A46 File Offset: 0x00106C46
		public bool AutoDecodePayload { get; set; }

		// Token: 0x060034A2 RID: 13474 RVA: 0x00108A4F File Offset: 0x00106C4F
		internal Socket(string nsp, SocketManager manager)
		{
			this.Namespace = nsp;
			this.Manager = manager;
			this.IsOpen = false;
			this.AutoDecodePayload = true;
			this.EventCallbacks = new EventTable(this);
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x00108A8C File Offset: 0x00106C8C
		void ISocket.Open()
		{
			if (this.Manager.State == SocketManager.States.Open)
			{
				this.OnTransportOpen(this.Manager.Socket, null, Array.Empty<object>());
				return;
			}
			this.Manager.Socket.Off("connect", new SocketIOCallback(this.OnTransportOpen));
			this.Manager.Socket.On("connect", new SocketIOCallback(this.OnTransportOpen));
			if (this.Manager.Options.AutoConnect && this.Manager.State == SocketManager.States.Initial)
			{
				this.Manager.Open();
			}
		}

		// Token: 0x060034A4 RID: 13476 RVA: 0x00108B2B File Offset: 0x00106D2B
		public void Disconnect()
		{
			((ISocket)this).Disconnect(true);
		}

		// Token: 0x060034A5 RID: 13477 RVA: 0x00108B34 File Offset: 0x00106D34
		void ISocket.Disconnect(bool remove)
		{
			if (this.IsOpen)
			{
				Packet packet = new Packet(TransportEventTypes.Message, SocketIOEventTypes.Disconnect, this.Namespace, string.Empty, 0, 0);
				((IManager)this.Manager).SendPacket(packet);
				this.IsOpen = false;
				((ISocket)this).OnPacket(packet);
			}
			if (this.AckCallbacks != null)
			{
				this.AckCallbacks.Clear();
			}
			if (remove)
			{
				this.EventCallbacks.Clear();
				((IManager)this.Manager).Remove(this);
			}
		}

		// Token: 0x060034A6 RID: 13478 RVA: 0x00108BA5 File Offset: 0x00106DA5
		public Socket Emit(string eventName, params object[] args)
		{
			return this.Emit(eventName, null, args);
		}

		// Token: 0x060034A7 RID: 13479 RVA: 0x00108BB0 File Offset: 0x00106DB0
		public Socket Emit(string eventName, SocketIOAckCallback callback, params object[] args)
		{
			if (EventNames.IsBlacklisted(eventName))
			{
				throw new ArgumentException("Blacklisted event: " + eventName);
			}
			this.arguments.Clear();
			this.arguments.Add(eventName);
			List<byte[]> list = null;
			if (args != null && args.Length != 0)
			{
				int num = 0;
				for (int i = 0; i < args.Length; i++)
				{
					byte[] array = args[i] as byte[];
					if (array != null)
					{
						if (list == null)
						{
							list = new List<byte[]>();
						}
						Dictionary<string, object> dictionary = new Dictionary<string, object>(2);
						dictionary.Add("_placeholder", true);
						dictionary.Add("num", num++);
						this.arguments.Add(dictionary);
						list.Add(array);
					}
					else
					{
						this.arguments.Add(args[i]);
					}
				}
			}
			string text = null;
			try
			{
				text = this.Manager.Encoder.Encode(this.arguments);
			}
			catch (Exception ex)
			{
				((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
				return this;
			}
			this.arguments.Clear();
			if (text == null)
			{
				throw new ArgumentException("Encoding the arguments to JSON failed!");
			}
			int num2 = 0;
			if (callback != null)
			{
				num2 = this.Manager.NextAckId;
				if (this.AckCallbacks == null)
				{
					this.AckCallbacks = new Dictionary<int, SocketIOAckCallback>();
				}
				this.AckCallbacks[num2] = callback;
			}
			Packet packet = new Packet(TransportEventTypes.Message, (list == null) ? SocketIOEventTypes.Event : SocketIOEventTypes.BinaryEvent, this.Namespace, text, 0, num2);
			if (list != null)
			{
				packet.Attachments = list;
			}
			((IManager)this.Manager).SendPacket(packet);
			return this;
		}

		// Token: 0x060034A8 RID: 13480 RVA: 0x00108D54 File Offset: 0x00106F54
		public Socket EmitAck(Packet originalPacket, params object[] args)
		{
			if (originalPacket == null)
			{
				throw new ArgumentNullException("originalPacket == null!");
			}
			if (originalPacket.SocketIOEvent != SocketIOEventTypes.Event && originalPacket.SocketIOEvent != SocketIOEventTypes.BinaryEvent)
			{
				throw new ArgumentException("Wrong packet - you can't send an Ack for a packet with id == 0 and SocketIOEvent != Event or SocketIOEvent != BinaryEvent!");
			}
			this.arguments.Clear();
			if (args != null && args.Length != 0)
			{
				this.arguments.AddRange(args);
			}
			string text = null;
			try
			{
				text = this.Manager.Encoder.Encode(this.arguments);
			}
			catch (Exception ex)
			{
				((ISocket)this).EmitError(SocketIOErrors.Internal, "Error while encoding payload: " + ex.Message + " " + ex.StackTrace);
				return this;
			}
			if (text == null)
			{
				throw new ArgumentException("Encoding the arguments to JSON failed!");
			}
			Packet packet = new Packet(TransportEventTypes.Message, (originalPacket.SocketIOEvent == SocketIOEventTypes.Event) ? SocketIOEventTypes.Ack : SocketIOEventTypes.BinaryAck, this.Namespace, text, 0, originalPacket.Id);
			((IManager)this.Manager).SendPacket(packet);
			return this;
		}

		// Token: 0x060034A9 RID: 13481 RVA: 0x00108E3C File Offset: 0x0010703C
		public void On(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(eventName, callback, false, this.AutoDecodePayload);
		}

		// Token: 0x060034AA RID: 13482 RVA: 0x00108E54 File Offset: 0x00107054
		public void On(SocketIOEventTypes type, SocketIOCallback callback)
		{
			string nameFor = EventNames.GetNameFor(type);
			this.EventCallbacks.Register(nameFor, callback, false, this.AutoDecodePayload);
		}

		// Token: 0x060034AB RID: 13483 RVA: 0x00108E7C File Offset: 0x0010707C
		public void On(string eventName, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(eventName, callback, false, autoDecodePayload);
		}

		// Token: 0x060034AC RID: 13484 RVA: 0x00108E90 File Offset: 0x00107090
		public void On(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
		{
			string nameFor = EventNames.GetNameFor(type);
			this.EventCallbacks.Register(nameFor, callback, false, autoDecodePayload);
		}

		// Token: 0x060034AD RID: 13485 RVA: 0x00108EB3 File Offset: 0x001070B3
		public void Once(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(eventName, callback, true, this.AutoDecodePayload);
		}

		// Token: 0x060034AE RID: 13486 RVA: 0x00108EC9 File Offset: 0x001070C9
		public void Once(SocketIOEventTypes type, SocketIOCallback callback)
		{
			this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, this.AutoDecodePayload);
		}

		// Token: 0x060034AF RID: 13487 RVA: 0x00108EE4 File Offset: 0x001070E4
		public void Once(string eventName, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(eventName, callback, true, autoDecodePayload);
		}

		// Token: 0x060034B0 RID: 13488 RVA: 0x00108EF5 File Offset: 0x001070F5
		public void Once(SocketIOEventTypes type, SocketIOCallback callback, bool autoDecodePayload)
		{
			this.EventCallbacks.Register(EventNames.GetNameFor(type), callback, true, autoDecodePayload);
		}

		// Token: 0x060034B1 RID: 13489 RVA: 0x00108F0B File Offset: 0x0010710B
		public void Off()
		{
			this.EventCallbacks.Clear();
		}

		// Token: 0x060034B2 RID: 13490 RVA: 0x00108F18 File Offset: 0x00107118
		public void Off(string eventName)
		{
			this.EventCallbacks.Unregister(eventName);
		}

		// Token: 0x060034B3 RID: 13491 RVA: 0x00108F26 File Offset: 0x00107126
		public void Off(SocketIOEventTypes type)
		{
			this.Off(EventNames.GetNameFor(type));
		}

		// Token: 0x060034B4 RID: 13492 RVA: 0x00108F34 File Offset: 0x00107134
		public void Off(string eventName, SocketIOCallback callback)
		{
			this.EventCallbacks.Unregister(eventName, callback);
		}

		// Token: 0x060034B5 RID: 13493 RVA: 0x00108F43 File Offset: 0x00107143
		public void Off(SocketIOEventTypes type, SocketIOCallback callback)
		{
			this.EventCallbacks.Unregister(EventNames.GetNameFor(type), callback);
		}

		// Token: 0x060034B6 RID: 13494 RVA: 0x00108F58 File Offset: 0x00107158
		void ISocket.OnPacket(Packet packet)
		{
			switch (packet.SocketIOEvent)
			{
			case SocketIOEventTypes.Connect:
				this.Id = ((this.Namespace != "/") ? (this.Namespace + "#" + this.Manager.Handshake.Sid) : this.Manager.Handshake.Sid);
				break;
			case SocketIOEventTypes.Disconnect:
				if (this.IsOpen)
				{
					this.IsOpen = false;
					this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Disconnect), packet, Array.Empty<object>());
					this.Disconnect();
				}
				break;
			case SocketIOEventTypes.Error:
			{
				bool flag = false;
				object obj = Json.Decode(packet.Payload, ref flag);
				if (flag)
				{
					Dictionary<string, object> dictionary = obj as Dictionary<string, object>;
					Error error;
					if (dictionary != null && dictionary.ContainsKey("code"))
					{
						error = new Error((SocketIOErrors)Convert.ToInt32(dictionary["code"]), dictionary["message"] as string);
					}
					else
					{
						error = new Error(SocketIOErrors.Custom, packet.Payload);
					}
					this.EventCallbacks.Call(EventNames.GetNameFor(SocketIOEventTypes.Error), packet, new object[]
					{
						error
					});
					return;
				}
				break;
			}
			}
			this.EventCallbacks.Call(packet);
			if ((packet.SocketIOEvent == SocketIOEventTypes.Ack || packet.SocketIOEvent == SocketIOEventTypes.BinaryAck) && this.AckCallbacks != null)
			{
				SocketIOAckCallback socketIOAckCallback = null;
				if (this.AckCallbacks.TryGetValue(packet.Id, out socketIOAckCallback) && socketIOAckCallback != null)
				{
					try
					{
						socketIOAckCallback(this, packet, this.AutoDecodePayload ? packet.Decode(this.Manager.Encoder) : null);
					}
					catch (Exception ex)
					{
						HTTPManager.Logger.Exception("Socket", "ackCallback", ex);
					}
				}
				this.AckCallbacks.Remove(packet.Id);
			}
		}

		// Token: 0x060034B7 RID: 13495 RVA: 0x00109130 File Offset: 0x00107330
		void ISocket.EmitEvent(SocketIOEventTypes type, params object[] args)
		{
			((ISocket)this).EmitEvent(EventNames.GetNameFor(type), args);
		}

		// Token: 0x060034B8 RID: 13496 RVA: 0x0010913F File Offset: 0x0010733F
		void ISocket.EmitEvent(string eventName, params object[] args)
		{
			if (!string.IsNullOrEmpty(eventName))
			{
				this.EventCallbacks.Call(eventName, null, args);
			}
		}

		// Token: 0x060034B9 RID: 13497 RVA: 0x00109157 File Offset: 0x00107357
		void ISocket.EmitError(SocketIOErrors errCode, string msg)
		{
			((ISocket)this).EmitEvent(SocketIOEventTypes.Error, new object[]
			{
				new Error(errCode, msg)
			});
		}

		// Token: 0x060034BA RID: 13498 RVA: 0x00109170 File Offset: 0x00107370
		private void OnTransportOpen(Socket socket, Packet packet, params object[] args)
		{
			if (this.Namespace != "/")
			{
				((IManager)this.Manager).SendPacket(new Packet(TransportEventTypes.Message, SocketIOEventTypes.Connect, this.Namespace, string.Empty, 0, 0));
			}
			this.IsOpen = true;
		}

		// Token: 0x04002280 RID: 8832
		private Dictionary<int, SocketIOAckCallback> AckCallbacks;

		// Token: 0x04002281 RID: 8833
		private EventTable EventCallbacks;

		// Token: 0x04002282 RID: 8834
		private List<object> arguments = new List<object>();
	}
}
