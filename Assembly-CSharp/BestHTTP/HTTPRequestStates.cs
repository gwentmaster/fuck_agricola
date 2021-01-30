using System;

namespace BestHTTP
{
	// Token: 0x02000570 RID: 1392
	public enum HTTPRequestStates
	{
		// Token: 0x04002162 RID: 8546
		Initial,
		// Token: 0x04002163 RID: 8547
		Queued,
		// Token: 0x04002164 RID: 8548
		Processing,
		// Token: 0x04002165 RID: 8549
		Finished,
		// Token: 0x04002166 RID: 8550
		Error,
		// Token: 0x04002167 RID: 8551
		Aborted,
		// Token: 0x04002168 RID: 8552
		ConnectionTimedOut,
		// Token: 0x04002169 RID: 8553
		TimedOut
	}
}
