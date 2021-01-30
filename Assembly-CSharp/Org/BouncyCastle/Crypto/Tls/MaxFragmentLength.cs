using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C6 RID: 966
	public abstract class MaxFragmentLength
	{
		// Token: 0x060023B4 RID: 9140 RVA: 0x000B7D8B File Offset: 0x000B5F8B
		public static bool IsValid(byte maxFragmentLength)
		{
			return maxFragmentLength >= 1 && maxFragmentLength <= 4;
		}

		// Token: 0x040018AC RID: 6316
		public const byte pow2_9 = 1;

		// Token: 0x040018AD RID: 6317
		public const byte pow2_10 = 2;

		// Token: 0x040018AE RID: 6318
		public const byte pow2_11 = 3;

		// Token: 0x040018AF RID: 6319
		public const byte pow2_12 = 4;
	}
}
