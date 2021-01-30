using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C8 RID: 968
	public abstract class NamedCurve
	{
		// Token: 0x060023B8 RID: 9144 RVA: 0x000B7DA0 File Offset: 0x000B5FA0
		public static bool IsValid(int namedCurve)
		{
			return (namedCurve >= 1 && namedCurve <= 28) || (namedCurve >= 65281 && namedCurve <= 65282);
		}

		// Token: 0x060023B9 RID: 9145 RVA: 0x000B7DC2 File Offset: 0x000B5FC2
		public static bool RefersToASpecificNamedCurve(int namedCurve)
		{
			return namedCurve - 65281 > 1;
		}

		// Token: 0x040018B1 RID: 6321
		public const int sect163k1 = 1;

		// Token: 0x040018B2 RID: 6322
		public const int sect163r1 = 2;

		// Token: 0x040018B3 RID: 6323
		public const int sect163r2 = 3;

		// Token: 0x040018B4 RID: 6324
		public const int sect193r1 = 4;

		// Token: 0x040018B5 RID: 6325
		public const int sect193r2 = 5;

		// Token: 0x040018B6 RID: 6326
		public const int sect233k1 = 6;

		// Token: 0x040018B7 RID: 6327
		public const int sect233r1 = 7;

		// Token: 0x040018B8 RID: 6328
		public const int sect239k1 = 8;

		// Token: 0x040018B9 RID: 6329
		public const int sect283k1 = 9;

		// Token: 0x040018BA RID: 6330
		public const int sect283r1 = 10;

		// Token: 0x040018BB RID: 6331
		public const int sect409k1 = 11;

		// Token: 0x040018BC RID: 6332
		public const int sect409r1 = 12;

		// Token: 0x040018BD RID: 6333
		public const int sect571k1 = 13;

		// Token: 0x040018BE RID: 6334
		public const int sect571r1 = 14;

		// Token: 0x040018BF RID: 6335
		public const int secp160k1 = 15;

		// Token: 0x040018C0 RID: 6336
		public const int secp160r1 = 16;

		// Token: 0x040018C1 RID: 6337
		public const int secp160r2 = 17;

		// Token: 0x040018C2 RID: 6338
		public const int secp192k1 = 18;

		// Token: 0x040018C3 RID: 6339
		public const int secp192r1 = 19;

		// Token: 0x040018C4 RID: 6340
		public const int secp224k1 = 20;

		// Token: 0x040018C5 RID: 6341
		public const int secp224r1 = 21;

		// Token: 0x040018C6 RID: 6342
		public const int secp256k1 = 22;

		// Token: 0x040018C7 RID: 6343
		public const int secp256r1 = 23;

		// Token: 0x040018C8 RID: 6344
		public const int secp384r1 = 24;

		// Token: 0x040018C9 RID: 6345
		public const int secp521r1 = 25;

		// Token: 0x040018CA RID: 6346
		public const int brainpoolP256r1 = 26;

		// Token: 0x040018CB RID: 6347
		public const int brainpoolP384r1 = 27;

		// Token: 0x040018CC RID: 6348
		public const int brainpoolP512r1 = 28;

		// Token: 0x040018CD RID: 6349
		public const int arbitrary_explicit_prime_curves = 65281;

		// Token: 0x040018CE RID: 6350
		public const int arbitrary_explicit_char2_curves = 65282;
	}
}
