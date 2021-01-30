using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039F RID: 927
	public abstract class CertChainType
	{
		// Token: 0x0600230E RID: 8974 RVA: 0x000B672F File Offset: 0x000B492F
		public static bool IsValid(byte certChainType)
		{
			return certChainType >= 0 && certChainType <= 1;
		}

		// Token: 0x040016DE RID: 5854
		public const byte individual_certs = 0;

		// Token: 0x040016DF RID: 5855
		public const byte pkipath = 1;
	}
}
