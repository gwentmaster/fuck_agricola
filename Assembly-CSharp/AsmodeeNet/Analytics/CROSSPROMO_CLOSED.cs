using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200070D RID: 1805
	public struct CROSSPROMO_CLOSED
	{
		// Token: 0x040028EB RID: 10475
		public string crosspromo_session_id;

		// Token: 0x040028EC RID: 10476
		public int time_active_sec;

		// Token: 0x020009FD RID: 2557
		public enum crosspromo_type
		{
			// Token: 0x040033E2 RID: 13282
			more_games,
			// Token: 0x040033E3 RID: 13283
			interstitial,
			// Token: 0x040033E4 RID: 13284
			banner
		}
	}
}
