using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069F RID: 1695
	[Serializable]
	public class ApiLeaderboardGetRankAndScoreAllPeriodResponse
	{
		// Token: 0x04002772 RID: 10098
		public bool error;

		// Token: 0x04002773 RID: 10099
		public int status;

		// Token: 0x04002774 RID: 10100
		public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data data;

		// Token: 0x020009A9 RID: 2473
		[Serializable]
		public class Data
		{
			// Token: 0x040032B7 RID: 12983
			public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User user;

			// Token: 0x02000A68 RID: 2664
			[Serializable]
			public class User
			{
				// Token: 0x040034F1 RID: 13553
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period day;

				// Token: 0x040034F2 RID: 13554
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period week;

				// Token: 0x040034F3 RID: 13555
				public ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period ever;

				// Token: 0x02000A75 RID: 2677
				[Serializable]
				public class Period
				{
					// Token: 0x04003537 RID: 13623
					public bool isNew;

					// Token: 0x04003538 RID: 13624
					public int rank;

					// Token: 0x04003539 RID: 13625
					public int score;

					// Token: 0x0400353A RID: 13626
					public string context;

					// Token: 0x0400353B RID: 13627
					public string when;
				}
			}
		}
	}
}
