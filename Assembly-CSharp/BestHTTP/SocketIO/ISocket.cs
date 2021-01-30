using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000599 RID: 1433
	public interface ISocket
	{
		// Token: 0x0600346B RID: 13419
		void Open();

		// Token: 0x0600346C RID: 13420
		void Disconnect(bool remove);

		// Token: 0x0600346D RID: 13421
		void OnPacket(Packet packet);

		// Token: 0x0600346E RID: 13422
		void EmitEvent(SocketIOEventTypes type, params object[] args);

		// Token: 0x0600346F RID: 13423
		void EmitEvent(string eventName, params object[] args);

		// Token: 0x06003470 RID: 13424
		void EmitError(SocketIOErrors errCode, string msg);
	}
}
