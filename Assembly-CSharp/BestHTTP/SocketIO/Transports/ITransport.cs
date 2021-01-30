using System;
using System.Collections.Generic;

namespace BestHTTP.SocketIO.Transports
{
	// Token: 0x020005A0 RID: 1440
	public interface ITransport
	{
		// Token: 0x170006B6 RID: 1718
		// (get) Token: 0x06003501 RID: 13569
		TransportTypes Type { get; }

		// Token: 0x170006B7 RID: 1719
		// (get) Token: 0x06003502 RID: 13570
		TransportStates State { get; }

		// Token: 0x170006B8 RID: 1720
		// (get) Token: 0x06003503 RID: 13571
		SocketManager Manager { get; }

		// Token: 0x170006B9 RID: 1721
		// (get) Token: 0x06003504 RID: 13572
		bool IsRequestInProgress { get; }

		// Token: 0x170006BA RID: 1722
		// (get) Token: 0x06003505 RID: 13573
		bool IsPollingInProgress { get; }

		// Token: 0x06003506 RID: 13574
		void Open();

		// Token: 0x06003507 RID: 13575
		void Poll();

		// Token: 0x06003508 RID: 13576
		void Send(Packet packet);

		// Token: 0x06003509 RID: 13577
		void Send(List<Packet> packets);

		// Token: 0x0600350A RID: 13578
		void Close();
	}
}
