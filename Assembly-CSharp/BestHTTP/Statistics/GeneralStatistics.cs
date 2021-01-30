using System;

namespace BestHTTP.Statistics
{
	// Token: 0x02000592 RID: 1426
	public struct GeneralStatistics
	{
		// Token: 0x04002246 RID: 8774
		public StatisticsQueryFlags QueryFlags;

		// Token: 0x04002247 RID: 8775
		public int Connections;

		// Token: 0x04002248 RID: 8776
		public int ActiveConnections;

		// Token: 0x04002249 RID: 8777
		public int FreeConnections;

		// Token: 0x0400224A RID: 8778
		public int RecycledConnections;

		// Token: 0x0400224B RID: 8779
		public int RequestsInQueue;

		// Token: 0x0400224C RID: 8780
		public int CacheEntityCount;

		// Token: 0x0400224D RID: 8781
		public ulong CacheSize;

		// Token: 0x0400224E RID: 8782
		public int CookieCount;

		// Token: 0x0400224F RID: 8783
		public uint CookieJarSize;
	}
}
