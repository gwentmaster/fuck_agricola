using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000444 RID: 1092
	public class RsaBlindingParameters : ICipherParameters
	{
		// Token: 0x060027E7 RID: 10215 RVA: 0x000C5E31 File Offset: 0x000C4031
		public RsaBlindingParameters(RsaKeyParameters publicKey, BigInteger blindingFactor)
		{
			if (publicKey.IsPrivate)
			{
				throw new ArgumentException("RSA parameters should be for a public key");
			}
			this.publicKey = publicKey;
			this.blindingFactor = blindingFactor;
		}

		// Token: 0x1700049F RID: 1183
		// (get) Token: 0x060027E8 RID: 10216 RVA: 0x000C5E5A File Offset: 0x000C405A
		public RsaKeyParameters PublicKey
		{
			get
			{
				return this.publicKey;
			}
		}

		// Token: 0x170004A0 RID: 1184
		// (get) Token: 0x060027E9 RID: 10217 RVA: 0x000C5E62 File Offset: 0x000C4062
		public BigInteger BlindingFactor
		{
			get
			{
				return this.blindingFactor;
			}
		}

		// Token: 0x04001A69 RID: 6761
		private readonly RsaKeyParameters publicKey;

		// Token: 0x04001A6A RID: 6762
		private readonly BigInteger blindingFactor;
	}
}
