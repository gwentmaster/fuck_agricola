using System;

namespace BestHTTP
{
	// Token: 0x02000569 RID: 1385
	internal enum HTTPConnectionStates
	{
		// Token: 0x04002126 RID: 8486
		Initial,
		// Token: 0x04002127 RID: 8487
		Processing,
		// Token: 0x04002128 RID: 8488
		Redirected,
		// Token: 0x04002129 RID: 8489
		Upgraded,
		// Token: 0x0400212A RID: 8490
		WaitForProtocolShutdown,
		// Token: 0x0400212B RID: 8491
		WaitForRecycle,
		// Token: 0x0400212C RID: 8492
		Free,
		// Token: 0x0400212D RID: 8493
		AbortRequested,
		// Token: 0x0400212E RID: 8494
		TimedOut,
		// Token: 0x0400212F RID: 8495
		Closed
	}
}
