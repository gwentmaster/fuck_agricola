using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A9 RID: 1705
	[Serializable]
	public class ApiShowcaseBannerResponse
	{
		// Token: 0x0400278B RID: 10123
		public int status;

		// Token: 0x0400278C RID: 10124
		public bool error;

		// Token: 0x0400278D RID: 10125
		public ApiShowcaseBannerResponse.Data data;

		// Token: 0x020009B1 RID: 2481
		[Serializable]
		public class Data
		{
			// Token: 0x040032C3 RID: 12995
			public ApiShowcaseProduct product;
		}
	}
}
