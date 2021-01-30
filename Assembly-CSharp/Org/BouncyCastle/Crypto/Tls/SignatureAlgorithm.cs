using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D3 RID: 979
	public abstract class SignatureAlgorithm
	{
		// Token: 0x04001910 RID: 6416
		public const byte anonymous = 0;

		// Token: 0x04001911 RID: 6417
		public const byte rsa = 1;

		// Token: 0x04001912 RID: 6418
		public const byte dsa = 2;

		// Token: 0x04001913 RID: 6419
		public const byte ecdsa = 3;
	}
}
