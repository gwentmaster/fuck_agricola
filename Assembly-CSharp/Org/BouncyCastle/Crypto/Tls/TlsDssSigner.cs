using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003EA RID: 1002
	public class TlsDssSigner : TlsDsaSigner
	{
		// Token: 0x060024AC RID: 9388 RVA: 0x000BB298 File Offset: 0x000B9498
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is DsaPublicKeyParameters;
		}

		// Token: 0x060024AD RID: 9389 RVA: 0x000BB2A3 File Offset: 0x000B94A3
		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new DsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}

		// Token: 0x17000434 RID: 1076
		// (get) Token: 0x060024AE RID: 9390 RVA: 0x000A5319 File Offset: 0x000A3519
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 2;
			}
		}
	}
}
