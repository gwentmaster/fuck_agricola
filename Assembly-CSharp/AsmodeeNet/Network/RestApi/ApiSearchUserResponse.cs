using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006A8 RID: 1704
	[Serializable]
	public class ApiSearchUserResponse
	{
		// Token: 0x04002788 RID: 10120
		public bool error;

		// Token: 0x04002789 RID: 10121
		public int status;

		// Token: 0x0400278A RID: 10122
		public ApiSearchUserResponse.Data data;

		// Token: 0x020009B0 RID: 2480
		[Serializable]
		public class Data
		{
			// Token: 0x040032C0 RID: 12992
			public int total;

			// Token: 0x040032C1 RID: 12993
			public Links _links;

			// Token: 0x040032C2 RID: 12994
			public ApiSearchUserResponse.Data.User[] users;

			// Token: 0x02000A6F RID: 2671
			[Serializable]
			public class User
			{
				// Token: 0x04003510 RID: 13584
				public int user_id;

				// Token: 0x04003511 RID: 13585
				public string login_name;

				// Token: 0x04003512 RID: 13586
				public string avatar;

				// Token: 0x04003513 RID: 13587
				public string[] features;

				// Token: 0x04003514 RID: 13588
				public ApiSearchUserResponse.Data.User.BoardGame[] boardgames;

				// Token: 0x04003515 RID: 13589
				public ApiSearchUserResponse.Data.User.OnlineGame[] onlinegames;

				// Token: 0x02000A7B RID: 2683
				[Serializable]
				public class BoardGame
				{
					// Token: 0x04003553 RID: 13651
					public string code;

					// Token: 0x04003554 RID: 13652
					public string name;

					// Token: 0x04003555 RID: 13653
					public string registered_date;

					// Token: 0x04003556 RID: 13654
					public long registered_date_ts;
				}

				// Token: 0x02000A7C RID: 2684
				[Serializable]
				public class OnlineGame
				{
					// Token: 0x04003557 RID: 13655
					public string game;

					// Token: 0x04003558 RID: 13656
					public int nbgames;

					// Token: 0x04003559 RID: 13657
					public int karma;

					// Token: 0x0400355A RID: 13658
					public float rankscore;

					// Token: 0x0400355B RID: 13659
					public int rank;

					// Token: 0x0400355C RID: 13660
					public string lastgame;

					// Token: 0x0400355D RID: 13661
					public string variant;

					// Token: 0x0400355E RID: 13662
					public long lastgame_ts;
				}
			}
		}
	}
}
