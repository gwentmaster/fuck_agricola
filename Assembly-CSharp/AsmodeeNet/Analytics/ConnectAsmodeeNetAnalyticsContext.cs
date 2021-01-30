using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072C RID: 1836
	public class ConnectAsmodeeNetAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008EC RID: 2284
		// (get) Token: 0x06003FFA RID: 16378 RVA: 0x00138C0B File Offset: 0x00136E0B
		// (set) Token: 0x06003FFB RID: 16379 RVA: 0x00138C13 File Offset: 0x00136E13
		public string SigninSessionId { get; private set; }

		// Token: 0x170008ED RID: 2285
		// (get) Token: 0x06003FFC RID: 16380 RVA: 0x00138C1C File Offset: 0x00136E1C
		// (set) Token: 0x06003FFD RID: 16381 RVA: 0x00138C24 File Offset: 0x00136E24
		public string ConnectPath { get; private set; }

		// Token: 0x06003FFE RID: 16382 RVA: 0x00138C30 File Offset: 0x00136E30
		public ConnectAsmodeeNetAnalyticsContext(string connectPath)
		{
			this.SigninSessionId = Guid.NewGuid().ToString();
			this.ConnectPath = connectPath;
		}
	}
}
