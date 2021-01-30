using System;

namespace BestHTTP.Caching
{
	// Token: 0x02000615 RID: 1557
	public sealed class HTTPCacheMaintananceParams
	{
		// Token: 0x17000798 RID: 1944
		// (get) Token: 0x06003946 RID: 14662 RVA: 0x0011CEEA File Offset: 0x0011B0EA
		// (set) Token: 0x06003947 RID: 14663 RVA: 0x0011CEF2 File Offset: 0x0011B0F2
		public TimeSpan DeleteOlder { get; private set; }

		// Token: 0x17000799 RID: 1945
		// (get) Token: 0x06003948 RID: 14664 RVA: 0x0011CEFB File Offset: 0x0011B0FB
		// (set) Token: 0x06003949 RID: 14665 RVA: 0x0011CF03 File Offset: 0x0011B103
		public ulong MaxCacheSize { get; private set; }

		// Token: 0x0600394A RID: 14666 RVA: 0x0011CF0C File Offset: 0x0011B10C
		public HTTPCacheMaintananceParams(TimeSpan deleteOlder, ulong maxCacheSize)
		{
			this.DeleteOlder = deleteOlder;
			this.MaxCacheSize = maxCacheSize;
		}
	}
}
