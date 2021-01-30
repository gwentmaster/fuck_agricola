using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C1 RID: 961
	public abstract class KeyExchangeAlgorithm
	{
		// Token: 0x04001885 RID: 6277
		public const int NULL = 0;

		// Token: 0x04001886 RID: 6278
		public const int RSA = 1;

		// Token: 0x04001887 RID: 6279
		public const int RSA_EXPORT = 2;

		// Token: 0x04001888 RID: 6280
		public const int DHE_DSS = 3;

		// Token: 0x04001889 RID: 6281
		public const int DHE_DSS_EXPORT = 4;

		// Token: 0x0400188A RID: 6282
		public const int DHE_RSA = 5;

		// Token: 0x0400188B RID: 6283
		public const int DHE_RSA_EXPORT = 6;

		// Token: 0x0400188C RID: 6284
		public const int DH_DSS = 7;

		// Token: 0x0400188D RID: 6285
		public const int DH_DSS_EXPORT = 8;

		// Token: 0x0400188E RID: 6286
		public const int DH_RSA = 9;

		// Token: 0x0400188F RID: 6287
		public const int DH_RSA_EXPORT = 10;

		// Token: 0x04001890 RID: 6288
		public const int DH_anon = 11;

		// Token: 0x04001891 RID: 6289
		public const int DH_anon_EXPORT = 12;

		// Token: 0x04001892 RID: 6290
		public const int PSK = 13;

		// Token: 0x04001893 RID: 6291
		public const int DHE_PSK = 14;

		// Token: 0x04001894 RID: 6292
		public const int RSA_PSK = 15;

		// Token: 0x04001895 RID: 6293
		public const int ECDH_ECDSA = 16;

		// Token: 0x04001896 RID: 6294
		public const int ECDHE_ECDSA = 17;

		// Token: 0x04001897 RID: 6295
		public const int ECDH_RSA = 18;

		// Token: 0x04001898 RID: 6296
		public const int ECDHE_RSA = 19;

		// Token: 0x04001899 RID: 6297
		public const int ECDH_anon = 20;

		// Token: 0x0400189A RID: 6298
		public const int SRP = 21;

		// Token: 0x0400189B RID: 6299
		public const int SRP_DSS = 22;

		// Token: 0x0400189C RID: 6300
		public const int SRP_RSA = 23;

		// Token: 0x0400189D RID: 6301
		public const int ECDHE_PSK = 24;
	}
}
