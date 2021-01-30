using System;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000593 RID: 1427
	public enum TransportEventTypes
	{
		// Token: 0x04002251 RID: 8785
		Unknown = -1,
		// Token: 0x04002252 RID: 8786
		Open,
		// Token: 0x04002253 RID: 8787
		Close,
		// Token: 0x04002254 RID: 8788
		Ping,
		// Token: 0x04002255 RID: 8789
		Pong,
		// Token: 0x04002256 RID: 8790
		Message,
		// Token: 0x04002257 RID: 8791
		Upgrade,
		// Token: 0x04002258 RID: 8792
		Noop
	}
}
