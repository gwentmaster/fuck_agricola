using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A2 RID: 1698
	[Serializable]
	public class ApiRecentGameResponse
	{
		// Token: 0x0400277B RID: 10107
		public ApiRecentGameResponse.Data data;

		// Token: 0x0400277C RID: 10108
		public bool error;

		// Token: 0x0400277D RID: 10109
		public int status;

		// Token: 0x020009AC RID: 2476
		[Serializable]
		public class Data
		{
			// Token: 0x040032BA RID: 12986
			public ApiRecentGameResponse.Data.Game[] games;

			// Token: 0x02000A6B RID: 2667
			[Serializable]
			public class Game
			{
				// Token: 0x040034FE RID: 13566
				public string table_id;

				// Token: 0x040034FF RID: 13567
				public string date;

				// Token: 0x04003500 RID: 13568
				public string game;

				// Token: 0x04003501 RID: 13569
				public string options;

				// Token: 0x04003502 RID: 13570
				public string status;

				// Token: 0x04003503 RID: 13571
				public string variant;

				// Token: 0x04003504 RID: 13572
				public int score;

				// Token: 0x04003505 RID: 13573
				public ApiRecentGameResponse.Data.Game.OtherPlayer[] other_players;

				// Token: 0x02000A78 RID: 2680
				[Serializable]
				public class OtherPlayer
				{
					// Token: 0x04003547 RID: 13639
					public int id;

					// Token: 0x04003548 RID: 13640
					public string login_name;

					// Token: 0x04003549 RID: 13641
					public string avatar;

					// Token: 0x0400354A RID: 13642
					public int score;
				}
			}
		}
	}
}
