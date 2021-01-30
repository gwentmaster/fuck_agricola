using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000699 RID: 1689
	[Serializable]
	public class ApiGetAchievementListResponse
	{
		// Token: 0x04002757 RID: 10071
		public bool error;

		// Token: 0x04002758 RID: 10072
		public int status;

		// Token: 0x04002759 RID: 10073
		public ApiGetAchievementListResponse.Data data;

		// Token: 0x020009A3 RID: 2467
		[Serializable]
		public class Data
		{
			// Token: 0x040032A8 RID: 12968
			public JsonAchievement[] achievements;

			// Token: 0x040032A9 RID: 12969
			public int total;

			// Token: 0x040032AA RID: 12970
			public Links _links;
		}
	}
}
