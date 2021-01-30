using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000698 RID: 1688
	[Serializable]
	public class ApiFetchRankResponse
	{
		// Token: 0x04002754 RID: 10068
		public bool error;

		// Token: 0x04002755 RID: 10069
		public int status;

		// Token: 0x04002756 RID: 10070
		public ApiFetchRankResponse.Data data;

		// Token: 0x020009A2 RID: 2466
		[Serializable]
		public class Data
		{
			// Token: 0x040032A7 RID: 12967
			public ApiFetchRankResponse.Data.User user;

			// Token: 0x02000A64 RID: 2660
			[Serializable]
			public class User
			{
				// Token: 0x040034C7 RID: 13511
				public int id;

				// Token: 0x040034C8 RID: 13512
				public string name;

				// Token: 0x040034C9 RID: 13513
				public int nbgames;

				// Token: 0x040034CA RID: 13514
				public int rank;

				// Token: 0x040034CB RID: 13515
				public int karma;

				// Token: 0x040034CC RID: 13516
				public int score;

				// Token: 0x040034CD RID: 13517
				public string ranking;
			}
		}
	}
}
