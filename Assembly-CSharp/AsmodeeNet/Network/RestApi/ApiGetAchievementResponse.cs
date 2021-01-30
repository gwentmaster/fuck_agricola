using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200069A RID: 1690
	[Serializable]
	public class ApiGetAchievementResponse
	{
		// Token: 0x0400275A RID: 10074
		public bool error;

		// Token: 0x0400275B RID: 10075
		public int status;

		// Token: 0x0400275C RID: 10076
		public ApiGetAchievementResponse.Data data;

		// Token: 0x020009A4 RID: 2468
		[Serializable]
		public class Data
		{
			// Token: 0x040032AB RID: 12971
			public JsonAchievement achievement;
		}
	}
}
