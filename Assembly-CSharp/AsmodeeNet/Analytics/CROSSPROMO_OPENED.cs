using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200070A RID: 1802
	public struct CROSSPROMO_OPENED
	{
		// Token: 0x040028DE RID: 10462
		public string crosspromo_session_id;

		// Token: 0x040028DF RID: 10463
		public string api_version;

		// Token: 0x040028E0 RID: 10464
		public bool is_automatic;

		// Token: 0x020009F3 RID: 2547
		public enum crosspromo_type
		{
			// Token: 0x040033AE RID: 13230
			more_games,
			// Token: 0x040033AF RID: 13231
			interstitial,
			// Token: 0x040033B0 RID: 13232
			banner
		}
	}
}
