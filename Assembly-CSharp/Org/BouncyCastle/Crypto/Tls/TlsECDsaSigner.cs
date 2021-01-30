using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003ED RID: 1005
	public class TlsECDsaSigner : TlsDsaSigner
	{
		// Token: 0x060024C4 RID: 9412 RVA: 0x000BB844 File Offset: 0x000B9A44
		public override bool IsValidPublicKey(AsymmetricKeyParameter publicKey)
		{
			return publicKey is ECPublicKeyParameters;
		}

		// Token: 0x060024C5 RID: 9413 RVA: 0x000BB84F File Offset: 0x000B9A4F
		protected override IDsa CreateDsaImpl(byte hashAlgorithm)
		{
			return new ECDsaSigner(new HMacDsaKCalculator(TlsUtilities.CreateHash(hashAlgorithm)));
		}

		// Token: 0x17000436 RID: 1078
		// (get) Token: 0x060024C6 RID: 9414 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		protected override byte SignatureAlgorithm
		{
			get
			{
				return 3;
			}
		}
	}
}
