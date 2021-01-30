using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B4 RID: 948
	public abstract class ECBasisType
	{
		// Token: 0x06002393 RID: 9107 RVA: 0x000B7BC0 File Offset: 0x000B5DC0
		public static bool IsValid(byte ecBasisType)
		{
			return ecBasisType >= 1 && ecBasisType <= 2;
		}

		// Token: 0x0400181F RID: 6175
		public const byte ec_basis_trinomial = 1;

		// Token: 0x04001820 RID: 6176
		public const byte ec_basis_pentanomial = 2;
	}
}
