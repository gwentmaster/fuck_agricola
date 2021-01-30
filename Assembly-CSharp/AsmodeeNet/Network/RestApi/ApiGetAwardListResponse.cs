using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069C RID: 1692
	[Serializable]
	public class ApiGetAwardListResponse
	{
		// Token: 0x04002769 RID: 10089
		public bool error;

		// Token: 0x0400276A RID: 10090
		public int status;

		// Token: 0x0400276B RID: 10091
		public ApiGetAwardListResponse.Data data;

		// Token: 0x020009A6 RID: 2470
		[Serializable]
		public class Data
		{
			// Token: 0x040032AF RID: 12975
			public int total;

			// Token: 0x040032B0 RID: 12976
			public Links _links;

			// Token: 0x040032B1 RID: 12977
			public ApiGetAwardListResponse.Data.Award[] awards;

			// Token: 0x02000A65 RID: 2661
			[Serializable]
			public class Award
			{
				// Token: 0x040034CE RID: 13518
				public int id;

				// Token: 0x040034CF RID: 13519
				public string tag;

				// Token: 0x040034D0 RID: 13520
				public int table_id;

				// Token: 0x040034D1 RID: 13521
				public int info_id;

				// Token: 0x040034D2 RID: 13522
				public string awarded_utc;

				// Token: 0x040034D3 RID: 13523
				public string awarded_cet;

				// Token: 0x040034D4 RID: 13524
				public long awarded_ts;
			}
		}
	}
}
