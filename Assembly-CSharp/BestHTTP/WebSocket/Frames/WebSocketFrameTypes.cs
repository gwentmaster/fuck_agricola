using System;

namespace BestHTTP.WebSocket.Frames
{
	// Token: 0x0200058E RID: 1422
	public enum WebSocketFrameTypes : byte
	{
		// Token: 0x0400222E RID: 8750
		Continuation,
		// Token: 0x0400222F RID: 8751
		Text,
		// Token: 0x04002230 RID: 8752
		Binary,
		// Token: 0x04002231 RID: 8753
		ConnectionClose = 8,
		// Token: 0x04002232 RID: 8754
		Ping,
		// Token: 0x04002233 RID: 8755
		Pong
	}
}
