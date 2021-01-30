using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.Events
{
	// Token: 0x020005A8 RID: 1448
	internal sealed class EventDescriptor
	{
		// Token: 0x170006C6 RID: 1734
		// (get) Token: 0x06003541 RID: 13633 RVA: 0x0010B0EC File Offset: 0x001092EC
		// (set) Token: 0x06003542 RID: 13634 RVA: 0x0010B0F4 File Offset: 0x001092F4
		public List<SocketIOCallback> Callbacks { get; private set; }

		// Token: 0x170006C7 RID: 1735
		// (get) Token: 0x06003543 RID: 13635 RVA: 0x0010B0FD File Offset: 0x001092FD
		// (set) Token: 0x06003544 RID: 13636 RVA: 0x0010B105 File Offset: 0x00109305
		public bool OnlyOnce { get; private set; }

		// Token: 0x170006C8 RID: 1736
		// (get) Token: 0x06003545 RID: 13637 RVA: 0x0010B10E File Offset: 0x0010930E
		// (set) Token: 0x06003546 RID: 13638 RVA: 0x0010B116 File Offset: 0x00109316
		public bool AutoDecodePayload { get; private set; }

		// Token: 0x06003547 RID: 13639 RVA: 0x0010B11F File Offset: 0x0010931F
		public EventDescriptor(bool onlyOnce, bool autoDecodePayload, SocketIOCallback callback)
		{
			this.OnlyOnce = onlyOnce;
			this.AutoDecodePayload = autoDecodePayload;
			this.Callbacks = new List<SocketIOCallback>(1);
			if (callback != null)
			{
				this.Callbacks.Add(callback);
			}
		}

		// Token: 0x06003548 RID: 13640 RVA: 0x0010B150 File Offset: 0x00109350
		public void Call(Socket socket, Packet packet, params object[] args)
		{
			if (this.CallbackArray == null || this.CallbackArray.Length < this.Callbacks.Count)
			{
				Array.Resize<SocketIOCallback>(ref this.CallbackArray, this.Callbacks.Count);
			}
			this.Callbacks.CopyTo(this.CallbackArray);
			for (int i = 0; i < this.CallbackArray.Length; i++)
			{
				try
				{
					SocketIOCallback socketIOCallback = this.CallbackArray[i];
					if (socketIOCallback != null)
					{
						socketIOCallback(socket, packet, args);
					}
				}
				catch (Exception ex)
				{
					((ISocket)socket).EmitError(SocketIOErrors.User, ex.Message + " " + ex.StackTrace);
					HTTPManager.Logger.Exception("EventDescriptor", "Call", ex);
				}
				if (this.OnlyOnce)
				{
					this.Callbacks.Remove(this.CallbackArray[i]);
				}
				this.CallbackArray[i] = null;
			}
		}

		// Token: 0x040022BA RID: 8890
		private SocketIOCallback[] CallbackArray;
	}
}
