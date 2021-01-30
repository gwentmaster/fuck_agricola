using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A8 RID: 936
	public abstract class CipherType
	{
		// Token: 0x040017FF RID: 6143
		public const int stream = 0;

		// Token: 0x04001800 RID: 6144
		public const int block = 1;

		// Token: 0x04001801 RID: 6145
		public const int aead = 2;
	}
}
