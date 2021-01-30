using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CB RID: 971
	public abstract class PrfAlgorithm
	{
		// Token: 0x040018D3 RID: 6355
		public const int tls_prf_legacy = 0;

		// Token: 0x040018D4 RID: 6356
		public const int tls_prf_sha256 = 1;

		// Token: 0x040018D5 RID: 6357
		public const int tls_prf_sha384 = 2;
	}
}
