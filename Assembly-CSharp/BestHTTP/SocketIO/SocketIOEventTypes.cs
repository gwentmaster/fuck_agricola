using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000594 RID: 1428
	public enum SocketIOEventTypes
	{
		// Token: 0x0400225A RID: 8794
		Unknown = -1,
		// Token: 0x0400225B RID: 8795
		Connect,
		// Token: 0x0400225C RID: 8796
		Disconnect,
		// Token: 0x0400225D RID: 8797
		Event,
		// Token: 0x0400225E RID: 8798
		Ack,
		// Token: 0x0400225F RID: 8799
		Error,
		// Token: 0x04002260 RID: 8800
		BinaryEvent,
		// Token: 0x04002261 RID: 8801
		BinaryAck
	}
}
