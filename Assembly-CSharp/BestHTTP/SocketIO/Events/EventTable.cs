using System;
using System.Collections.Generic;
using BestHTTP.Logger;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020005AA RID: 1450
	internal sealed class EventTable
	{
		// Token: 0x170006C9 RID: 1737
		// (get) Token: 0x0600354D RID: 13645 RVA: 0x0010B388 File Offset: 0x00109588
		// (set) Token: 0x0600354E RID: 13646 RVA: 0x0010B390 File Offset: 0x00109590
		private Socket Socket { get; set; }

		// Token: 0x0600354F RID: 13647 RVA: 0x0010B399 File Offset: 0x00109599
		public EventTable(Socket socket)
		{
			this.Socket = socket;
		}

		// Token: 0x06003550 RID: 13648 RVA: 0x0010B3B4 File Offset: 0x001095B4
		public void Register(string eventName, SocketIOCallback callback, bool onlyOnce, bool autoDecodePayload)
		{
			List<EventDescriptor> list;
			if (!this.Table.TryGetValue(eventName, out list))
			{
				this.Table.Add(eventName, list = new List<EventDescriptor>(1));
			}
			EventDescriptor eventDescriptor = list.Find((EventDescriptor d) => d.OnlyOnce == onlyOnce && d.AutoDecodePayload == autoDecodePayload);
			if (eventDescriptor == null)
			{
				list.Add(new EventDescriptor(onlyOnce, autoDecodePayload, callback));
				return;
			}
			eventDescriptor.Callbacks.Add(callback);
		}

		// Token: 0x06003551 RID: 13649 RVA: 0x0010B435 File Offset: 0x00109635
		public void Unregister(string eventName)
		{
			this.Table.Remove(eventName);
		}

		// Token: 0x06003552 RID: 13650 RVA: 0x0010B444 File Offset: 0x00109644
		public void Unregister(string eventName, SocketIOCallback callback)
		{
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Callbacks.Remove(callback);
				}
			}
		}

		// Token: 0x06003553 RID: 13651 RVA: 0x0010B488 File Offset: 0x00109688
		public void Call(string eventName, Packet packet, params object[] args)
		{
			if (HTTPManager.Logger.Level <= Loglevels.All)
			{
				HTTPManager.Logger.Verbose("EventTable", "Call - " + eventName);
			}
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					list[i].Call(this.Socket, packet, args);
				}
			}
		}

		// Token: 0x06003554 RID: 13652 RVA: 0x0010B4F4 File Offset: 0x001096F4
		public void Call(Packet packet)
		{
			string text = packet.DecodeEventName();
			string text2 = (packet.SocketIOEvent != SocketIOEventTypes.Unknown) ? EventNames.GetNameFor(packet.SocketIOEvent) : EventNames.GetNameFor(packet.TransportEvent);
			object[] args = null;
			if (!this.HasSubsciber(text) && !this.HasSubsciber(text2))
			{
				return;
			}
			if (packet.TransportEvent == TransportEventTypes.Message && (packet.SocketIOEvent == SocketIOEventTypes.Event || packet.SocketIOEvent == SocketIOEventTypes.BinaryEvent) && this.ShouldDecodePayload(text))
			{
				args = packet.Decode(this.Socket.Manager.Encoder);
			}
			if (!string.IsNullOrEmpty(text))
			{
				this.Call(text, packet, args);
			}
			if (!packet.IsDecoded && this.ShouldDecodePayload(text2))
			{
				args = packet.Decode(this.Socket.Manager.Encoder);
			}
			if (!string.IsNullOrEmpty(text2))
			{
				this.Call(text2, packet, args);
			}
		}

		// Token: 0x06003555 RID: 13653 RVA: 0x0010B5C4 File Offset: 0x001097C4
		public void Clear()
		{
			this.Table.Clear();
		}

		// Token: 0x06003556 RID: 13654 RVA: 0x0010B5D4 File Offset: 0x001097D4
		private bool ShouldDecodePayload(string eventName)
		{
			List<EventDescriptor> list;
			if (this.Table.TryGetValue(eventName, out list))
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].AutoDecodePayload && list[i].Callbacks.Count > 0)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06003557 RID: 13655 RVA: 0x0010B627 File Offset: 0x00109827
		private bool HasSubsciber(string eventName)
		{
			return this.Table.ContainsKey(eventName);
		}

		// Token: 0x040022C6 RID: 8902
		private Dictionary<string, List<EventDescriptor>> Table = new Dictionary<string, List<EventDescriptor>>();
	}
}
