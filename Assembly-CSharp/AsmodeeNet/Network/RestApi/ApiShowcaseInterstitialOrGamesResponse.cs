using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006AA RID: 1706
	[Serializable]
	public class ApiShowcaseInterstitialOrGamesResponse
	{
		// Token: 0x0400278E RID: 10126
		public int status;

		// Token: 0x0400278F RID: 10127
		public bool error;

		// Token: 0x04002790 RID: 10128
		public ApiShowcaseInterstitialOrGamesResponse.Data data;

		// Token: 0x020009B2 RID: 2482
		[Serializable]
		public class Data
		{
			// Token: 0x040032C4 RID: 12996
			public ApiShowcaseProduct[] products;
		}
	}
}
