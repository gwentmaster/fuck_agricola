using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C5 RID: 965
	public abstract class MacAlgorithm
	{
		// Token: 0x040018A4 RID: 6308
		public const int cls_null = 0;

		// Token: 0x040018A5 RID: 6309
		public const int md5 = 1;

		// Token: 0x040018A6 RID: 6310
		public const int sha = 2;

		// Token: 0x040018A7 RID: 6311
		public const int hmac_md5 = 1;

		// Token: 0x040018A8 RID: 6312
		public const int hmac_sha1 = 2;

		// Token: 0x040018A9 RID: 6313
		public const int hmac_sha256 = 3;

		// Token: 0x040018AA RID: 6314
		public const int hmac_sha384 = 4;

		// Token: 0x040018AB RID: 6315
		public const int hmac_sha512 = 5;
	}
}
