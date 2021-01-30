using System;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x0200028B RID: 651
	public sealed class Times
	{
		// Token: 0x0600158C RID: 5516 RVA: 0x00079C78 File Offset: 0x00077E78
		public static long NanoTime()
		{
			return DateTime.UtcNow.Ticks * Times.NanosecondsPerTick;
		}

		// Token: 0x04001397 RID: 5015
		private static long NanosecondsPerTick = 100L;
	}
}
