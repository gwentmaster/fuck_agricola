using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A1 RID: 1697
	[Serializable]
	public class ApiLeaderboardRequestGivenGameResponse
	{
		// Token: 0x04002778 RID: 10104
		public bool error;

		// Token: 0x04002779 RID: 10105
		public int status;

		// Token: 0x0400277A RID: 10106
		public ApiLeaderboardRequestGivenGameResponse.Data data;

		// Token: 0x020009AB RID: 2475
		[Serializable]
		public class Data
		{
			// Token: 0x040032B9 RID: 12985
			public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard leaderboard;

			// Token: 0x02000A6A RID: 2666
			[Serializable]
			public class Leaderboard
			{
				// Token: 0x040034F8 RID: 13560
				public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Links _links;

				// Token: 0x040034F9 RID: 13561
				public int total;

				// Token: 0x040034FA RID: 13562
				public string id;

				// Token: 0x040034FB RID: 13563
				public string game;

				// Token: 0x040034FC RID: 13564
				public string period;

				// Token: 0x040034FD RID: 13565
				public ApiLeaderboardRequestGivenGameResponse.Data.Leaderboard.Player[] players;

				// Token: 0x02000A76 RID: 2678
				[Serializable]
				public class Links
				{
					// Token: 0x0400353C RID: 13628
					public string first;

					// Token: 0x0400353D RID: 13629
					public string last;

					// Token: 0x0400353E RID: 13630
					public string next;

					// Token: 0x0400353F RID: 13631
					public string prev;
				}

				// Token: 0x02000A77 RID: 2679
				[Serializable]
				public class Player
				{
					// Token: 0x04003540 RID: 13632
					public int rank;

					// Token: 0x04003541 RID: 13633
					public int id;

					// Token: 0x04003542 RID: 13634
					public int score;

					// Token: 0x04003543 RID: 13635
					public string context;

					// Token: 0x04003544 RID: 13636
					public string name;

					// Token: 0x04003545 RID: 13637
					public string avatar;

					// Token: 0x04003546 RID: 13638
					public string when;
				}
			}
		}
	}
}
