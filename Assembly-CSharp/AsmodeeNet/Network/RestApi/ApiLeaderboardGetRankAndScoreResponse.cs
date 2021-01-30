using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A0 RID: 1696
	[Serializable]
	public class ApiLeaderboardGetRankAndScoreResponse
	{
		// Token: 0x04002775 RID: 10101
		public bool error;

		// Token: 0x04002776 RID: 10102
		public int status;

		// Token: 0x04002777 RID: 10103
		public ApiLeaderboardGetRankAndScoreResponse.Data data;

		// Token: 0x020009AA RID: 2474
		[Serializable]
		public class Data
		{
			// Token: 0x040032B8 RID: 12984
			public ApiLeaderboardGetRankAndScoreResponse.Data.User user;

			// Token: 0x02000A69 RID: 2665
			[Serializable]
			public class User
			{
				// Token: 0x040034F4 RID: 13556
				public int rank;

				// Token: 0x040034F5 RID: 13557
				public int score;

				// Token: 0x040034F6 RID: 13558
				public string context;

				// Token: 0x040034F7 RID: 13559
				public string when;
			}
		}
	}
}
