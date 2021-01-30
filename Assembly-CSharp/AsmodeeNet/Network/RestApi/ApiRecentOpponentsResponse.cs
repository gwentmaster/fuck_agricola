using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A3 RID: 1699
	[Serializable]
	public class ApiRecentOpponentsResponse
	{
		// Token: 0x0400277E RID: 10110
		public ApiRecentOpponentsResponse.Data data;

		// Token: 0x0400277F RID: 10111
		public bool error;

		// Token: 0x04002780 RID: 10112
		public int status;

		// Token: 0x020009AD RID: 2477
		[Serializable]
		public class Data
		{
			// Token: 0x040032BB RID: 12987
			public ApiRecentOpponentsResponse.Data.Opponent[] opponents;

			// Token: 0x02000A6C RID: 2668
			[Serializable]
			public class Opponent
			{
				// Token: 0x04003506 RID: 13574
				public int id;

				// Token: 0x04003507 RID: 13575
				public string login_name;

				// Token: 0x04003508 RID: 13576
				public string avatar;

				// Token: 0x04003509 RID: 13577
				public string last_game_date;

				// Token: 0x0400350A RID: 13578
				public ApiRecentOpponentsResponse.Data.Opponent.Game[] games;

				// Token: 0x02000A79 RID: 2681
				[Serializable]
				public class Game
				{
					// Token: 0x0400354B RID: 13643
					public string table_id;

					// Token: 0x0400354C RID: 13644
					public string game;

					// Token: 0x0400354D RID: 13645
					public string date;

					// Token: 0x0400354E RID: 13646
					public string status;

					// Token: 0x0400354F RID: 13647
					public int other_score;

					// Token: 0x04003550 RID: 13648
					public int score;
				}
			}
		}
	}
}
