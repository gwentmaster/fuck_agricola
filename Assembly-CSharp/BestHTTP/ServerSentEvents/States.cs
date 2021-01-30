using System;

namespace BestHTTP.ServerSentEvents
{
	// Token: 0x020005D9 RID: 1497
	public enum States
	{
		// Token: 0x04002364 RID: 9060
		Initial,
		// Token: 0x04002365 RID: 9061
		Connecting,
		// Token: 0x04002366 RID: 9062
		Open,
		// Token: 0x04002367 RID: 9063
		Retrying,
		// Token: 0x04002368 RID: 9064
		Closing,
		// Token: 0x04002369 RID: 9065
		Closed
	}
}
