using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069D RID: 1693
	[Serializable]
	public class ApiGetBuddiesOrIgnoredResponse
	{
		// Token: 0x0400276C RID: 10092
		public bool error;

		// Token: 0x0400276D RID: 10093
		public int status;

		// Token: 0x0400276E RID: 10094
		public ApiGetBuddiesOrIgnoredResponse.Data data;

		// Token: 0x020009A7 RID: 2471
		[Serializable]
		public class Data
		{
			// Token: 0x040032B2 RID: 12978
			public int total;

			// Token: 0x040032B3 RID: 12979
			public Links _links;

			// Token: 0x040032B4 RID: 12980
			public ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored[] buddies;

			// Token: 0x040032B5 RID: 12981
			public ApiGetBuddiesOrIgnoredResponse.Data.BuddyOrIgnored[] ignored;

			// Token: 0x02000A66 RID: 2662
			[Serializable]
			public class BuddyOrIgnored
			{
				// Token: 0x040034D5 RID: 13525
				public int id;

				// Token: 0x040034D6 RID: 13526
				public string login_name;
			}
		}
	}
}
