using System;

namespace BestHTTP.SignalR
{
	// Token: 0x020005B6 RID: 1462
	public enum ConnectionStates
	{
		// Token: 0x040022FF RID: 8959
		Initial,
		// Token: 0x04002300 RID: 8960
		Authenticating,
		// Token: 0x04002301 RID: 8961
		Negotiating,
		// Token: 0x04002302 RID: 8962
		Connecting,
		// Token: 0x04002303 RID: 8963
		Connected,
		// Token: 0x04002304 RID: 8964
		Reconnecting,
		// Token: 0x04002305 RID: 8965
		Closed
	}
}
