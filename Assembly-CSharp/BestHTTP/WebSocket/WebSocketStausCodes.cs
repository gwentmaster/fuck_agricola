using System;

namespace BestHTTP.WebSocket
{
	// Token: 0x0200058B RID: 1419
	public enum WebSocketStausCodes : uint
	{
		// Token: 0x04002212 RID: 8722
		NormalClosure = 1000U,
		// Token: 0x04002213 RID: 8723
		GoingAway,
		// Token: 0x04002214 RID: 8724
		ProtocolError,
		// Token: 0x04002215 RID: 8725
		WrongDataType,
		// Token: 0x04002216 RID: 8726
		Reserved,
		// Token: 0x04002217 RID: 8727
		NoStatusCode,
		// Token: 0x04002218 RID: 8728
		ClosedAbnormally,
		// Token: 0x04002219 RID: 8729
		DataError,
		// Token: 0x0400221A RID: 8730
		PolicyError,
		// Token: 0x0400221B RID: 8731
		TooBigMessage,
		// Token: 0x0400221C RID: 8732
		ExtensionExpected,
		// Token: 0x0400221D RID: 8733
		WrongRequest,
		// Token: 0x0400221E RID: 8734
		TLSHandshakeError = 1015U
	}
}
