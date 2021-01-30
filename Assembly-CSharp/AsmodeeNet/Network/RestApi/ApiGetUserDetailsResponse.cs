using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069E RID: 1694
	[Serializable]
	public class ApiGetUserDetailsResponse
	{
		// Token: 0x0400276F RID: 10095
		public bool error;

		// Token: 0x04002770 RID: 10096
		public int status;

		// Token: 0x04002771 RID: 10097
		public ApiGetUserDetailsResponse.Data data;

		// Token: 0x020009A8 RID: 2472
		[Serializable]
		public class Data
		{
			// Token: 0x040032B6 RID: 12982
			public ApiGetUserDetailsResponse.Data.User user;

			// Token: 0x02000A67 RID: 2663
			[Serializable]
			public class User
			{
				// Token: 0x040034D7 RID: 13527
				public int user_id;

				// Token: 0x040034D8 RID: 13528
				public string login_name;

				// Token: 0x040034D9 RID: 13529
				public string country;

				// Token: 0x040034DA RID: 13530
				public bool email_valid;

				// Token: 0x040034DB RID: 13531
				public string language;

				// Token: 0x040034DC RID: 13532
				public string time_zone;

				// Token: 0x040034DD RID: 13533
				public int posted_msg_count;

				// Token: 0x040034DE RID: 13534
				public int last_post_id;

				// Token: 0x040034DF RID: 13535
				public bool validated;

				// Token: 0x040034E0 RID: 13536
				public string avatar;

				// Token: 0x040034E1 RID: 13537
				public long join_date_ts;

				// Token: 0x040034E2 RID: 13538
				public string join_date;

				// Token: 0x040034E3 RID: 13539
				public string join_date_cet;

				// Token: 0x040034E4 RID: 13540
				public long last_visit_ts;

				// Token: 0x040034E5 RID: 13541
				public string last_visit;

				// Token: 0x040034E6 RID: 13542
				public string last_visit_cet;

				// Token: 0x040034E7 RID: 13543
				public string zipcode;

				// Token: 0x040034E8 RID: 13544
				public string birthday;

				// Token: 0x040034E9 RID: 13545
				public string email;

				// Token: 0x040034EA RID: 13546
				public string gender;

				// Token: 0x040034EB RID: 13547
				public bool coppa;

				// Token: 0x040034EC RID: 13548
				public string name;

				// Token: 0x040034ED RID: 13549
				public string[] features;

				// Token: 0x040034EE RID: 13550
				public ApiGetUserDetailsResponse.Data.User.BoardGame[] boardgames;

				// Token: 0x040034EF RID: 13551
				public ApiGetUserDetailsResponse.Data.User.OnlineGame[] onlinegames;

				// Token: 0x040034F0 RID: 13552
				public ApiGetUserDetailsResponse.Data.User.Partners[] partners;

				// Token: 0x02000A72 RID: 2674
				[Serializable]
				public class BoardGame
				{
					// Token: 0x04003528 RID: 13608
					public string code;

					// Token: 0x04003529 RID: 13609
					public string name;

					// Token: 0x0400352A RID: 13610
					public string registered_date;

					// Token: 0x0400352B RID: 13611
					public long registered_date_ts;
				}

				// Token: 0x02000A73 RID: 2675
				[Serializable]
				public class OnlineGame
				{
					// Token: 0x0400352C RID: 13612
					public string game;

					// Token: 0x0400352D RID: 13613
					public int nbgames;

					// Token: 0x0400352E RID: 13614
					public int karma;

					// Token: 0x0400352F RID: 13615
					public float rankscore;

					// Token: 0x04003530 RID: 13616
					public int rank;

					// Token: 0x04003531 RID: 13617
					public string lastgame;

					// Token: 0x04003532 RID: 13618
					public string variant;

					// Token: 0x04003533 RID: 13619
					public long lastgame_ts;
				}

				// Token: 0x02000A74 RID: 2676
				[Serializable]
				public class Partners
				{
					// Token: 0x04003534 RID: 13620
					public int partner_id;

					// Token: 0x04003535 RID: 13621
					public string partner_user_id;

					// Token: 0x04003536 RID: 13622
					public string created_at;
				}
			}
		}
	}
}
