using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000709 RID: 1801
	public struct CROSSPROMO_DISPLAYED
	{
		// Token: 0x040028DA RID: 10458
		public string crosspromo_session_id;

		// Token: 0x040028DB RID: 10459
		public string api_version;

		// Token: 0x040028DC RID: 10460
		public string product_id;

		// Token: 0x040028DD RID: 10461
		public string product_name;

		// Token: 0x020009F2 RID: 2546
		public enum crosspromo_type
		{
			// Token: 0x040033AB RID: 13227
			interstitial,
			// Token: 0x040033AC RID: 13228
			banner
		}
	}
}
