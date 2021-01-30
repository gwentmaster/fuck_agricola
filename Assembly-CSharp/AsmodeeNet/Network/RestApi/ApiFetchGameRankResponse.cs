using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000697 RID: 1687
	[Serializable]
	public class ApiFetchGameRankResponse
	{
		// Token: 0x04002751 RID: 10065
		public bool error;

		// Token: 0x04002752 RID: 10066
		public int status;

		// Token: 0x04002753 RID: 10067
		public ApiFetchGameRankResponse.Data data;

		// Token: 0x020009A1 RID: 2465
		[Serializable]
		public class Data
		{
			// Token: 0x040032A4 RID: 12964
			public ApiFetchGameRankResponse.Data.Rank[] ranks;

			// Token: 0x040032A5 RID: 12965
			public int total;

			// Token: 0x040032A6 RID: 12966
			public Links _links;

			// Token: 0x02000A63 RID: 2659
			[Serializable]
			public class Rank
			{
				// Token: 0x040034C1 RID: 13505
				public int id;

				// Token: 0x040034C2 RID: 13506
				public string name;

				// Token: 0x040034C3 RID: 13507
				public int nbgames;

				// Token: 0x040034C4 RID: 13508
				public int karma;

				// Token: 0x040034C5 RID: 13509
				public int score;

				// Token: 0x040034C6 RID: 13510
				public int rank;
			}
		}
	}
}
