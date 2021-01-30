using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200070C RID: 1804
	public struct CROSSPROMO_REDIRECTED
	{
		// Token: 0x040028E8 RID: 10472
		public string crosspromo_session_id;

		// Token: 0x040028E9 RID: 10473
		public string product_id;

		// Token: 0x040028EA RID: 10474
		public string product_name;

		// Token: 0x020009FA RID: 2554
		public enum crosspromo_type
		{
			// Token: 0x040033D6 RID: 13270
			more_games,
			// Token: 0x040033D7 RID: 13271
			interstitial,
			// Token: 0x040033D8 RID: 13272
			banner
		}

		// Token: 0x020009FB RID: 2555
		public enum more_game_category
		{
			// Token: 0x040033DA RID: 13274
			featured,
			// Token: 0x040033DB RID: 13275
			family,
			// Token: 0x040033DC RID: 13276
			advanced,
			// Token: 0x040033DD RID: 13277
			tabletop
		}

		// Token: 0x020009FC RID: 2556
		public enum product_type
		{
			// Token: 0x040033DF RID: 13279
			digital,
			// Token: 0x040033E0 RID: 13280
			boardgame
		}
	}
}
