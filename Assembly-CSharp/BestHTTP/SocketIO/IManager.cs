using System;
using BestHTTP.SocketIO.Transports;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000598 RID: 1432
	public interface IManager
	{
		// Token: 0x0600345F RID: 13407
		void Remove(Socket socket);

		// Token: 0x06003460 RID: 13408
		void Close(bool removeSockets = true);

		// Token: 0x06003461 RID: 13409
		void TryToReconnect();

		// Token: 0x06003462 RID: 13410
		bool OnTransportConnected(ITransport transport);

		// Token: 0x06003463 RID: 13411
		void OnTransportError(ITransport trans, string err);

		// Token: 0x06003464 RID: 13412
		void OnTransportProbed(ITransport trans);

		// Token: 0x06003465 RID: 13413
		void SendPacket(Packet packet);

		// Token: 0x06003466 RID: 13414
		void OnPacket(Packet packet);

		// Token: 0x06003467 RID: 13415
		void EmitEvent(string eventName, params object[] args);

		// Token: 0x06003468 RID: 13416
		void EmitEvent(SocketIOEventTypes type, params object[] args);

		// Token: 0x06003469 RID: 13417
		void EmitError(SocketIOErrors errCode, string msg);

		// Token: 0x0600346A RID: 13418
		void EmitAll(string eventName, params object[] args);
	}
}
