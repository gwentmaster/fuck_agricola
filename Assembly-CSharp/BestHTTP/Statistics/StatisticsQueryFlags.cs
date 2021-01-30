using System;

namespace BestHTTP.Statistics
{
	// Token: 0x02000591 RID: 1425
	[Flags]
	public enum StatisticsQueryFlags : byte
	{
		// Token: 0x04002242 RID: 8770
		Connections = 1,
		// Token: 0x04002243 RID: 8771
		Cache = 2,
		// Token: 0x04002244 RID: 8772
		Cookies = 4,
		// Token: 0x04002245 RID: 8773
		All = 255
	}
}
