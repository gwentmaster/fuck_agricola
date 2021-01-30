using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B7 RID: 951
	public abstract class EncryptionAlgorithm
	{
		// Token: 0x04001827 RID: 6183
		public const int NULL = 0;

		// Token: 0x04001828 RID: 6184
		public const int RC4_40 = 1;

		// Token: 0x04001829 RID: 6185
		public const int RC4_128 = 2;

		// Token: 0x0400182A RID: 6186
		public const int RC2_CBC_40 = 3;

		// Token: 0x0400182B RID: 6187
		public const int IDEA_CBC = 4;

		// Token: 0x0400182C RID: 6188
		public const int DES40_CBC = 5;

		// Token: 0x0400182D RID: 6189
		public const int DES_CBC = 6;

		// Token: 0x0400182E RID: 6190
		public const int cls_3DES_EDE_CBC = 7;

		// Token: 0x0400182F RID: 6191
		public const int AES_128_CBC = 8;

		// Token: 0x04001830 RID: 6192
		public const int AES_256_CBC = 9;

		// Token: 0x04001831 RID: 6193
		public const int AES_128_GCM = 10;

		// Token: 0x04001832 RID: 6194
		public const int AES_256_GCM = 11;

		// Token: 0x04001833 RID: 6195
		public const int CAMELLIA_128_CBC = 12;

		// Token: 0x04001834 RID: 6196
		public const int CAMELLIA_256_CBC = 13;

		// Token: 0x04001835 RID: 6197
		public const int SEED_CBC = 14;

		// Token: 0x04001836 RID: 6198
		public const int AES_128_CCM = 15;

		// Token: 0x04001837 RID: 6199
		public const int AES_128_CCM_8 = 16;

		// Token: 0x04001838 RID: 6200
		public const int AES_256_CCM = 17;

		// Token: 0x04001839 RID: 6201
		public const int AES_256_CCM_8 = 18;

		// Token: 0x0400183A RID: 6202
		public const int CAMELLIA_128_GCM = 19;

		// Token: 0x0400183B RID: 6203
		public const int CAMELLIA_256_GCM = 20;

		// Token: 0x0400183C RID: 6204
		public const int CHACHA20_POLY1305 = 102;

		// Token: 0x0400183D RID: 6205
		[Obsolete]
		public const int AEAD_CHACHA20_POLY1305 = 102;

		// Token: 0x0400183E RID: 6206
		public const int AES_128_OCB_TAGLEN96 = 103;

		// Token: 0x0400183F RID: 6207
		public const int AES_256_OCB_TAGLEN96 = 104;
	}
}
