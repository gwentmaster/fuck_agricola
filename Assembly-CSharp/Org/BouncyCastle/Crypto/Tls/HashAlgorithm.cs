using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BC RID: 956
	public abstract class HashAlgorithm
	{
		// Token: 0x0600239F RID: 9119 RVA: 0x000B7BF4 File Offset: 0x000B5DF4
		public static string GetName(byte hashAlgorithm)
		{
			switch (hashAlgorithm)
			{
			case 0:
				return "none";
			case 1:
				return "md5";
			case 2:
				return "sha1";
			case 3:
				return "sha224";
			case 4:
				return "sha256";
			case 5:
				return "sha384";
			case 6:
				return "sha512";
			default:
				return "UNKNOWN";
			}
		}

		// Token: 0x060023A0 RID: 9120 RVA: 0x000B7C54 File Offset: 0x000B5E54
		public static string GetText(byte hashAlgorithm)
		{
			return string.Concat(new object[]
			{
				HashAlgorithm.GetName(hashAlgorithm),
				"(",
				hashAlgorithm,
				")"
			});
		}

		// Token: 0x060023A1 RID: 9121 RVA: 0x000B7C83 File Offset: 0x000B5E83
		public static bool IsPrivate(byte hashAlgorithm)
		{
			return 224 <= hashAlgorithm && hashAlgorithm <= byte.MaxValue;
		}

		// Token: 0x04001879 RID: 6265
		public const byte none = 0;

		// Token: 0x0400187A RID: 6266
		public const byte md5 = 1;

		// Token: 0x0400187B RID: 6267
		public const byte sha1 = 2;

		// Token: 0x0400187C RID: 6268
		public const byte sha224 = 3;

		// Token: 0x0400187D RID: 6269
		public const byte sha256 = 4;

		// Token: 0x0400187E RID: 6270
		public const byte sha384 = 5;

		// Token: 0x0400187F RID: 6271
		public const byte sha512 = 6;
	}
}
