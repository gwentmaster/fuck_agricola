using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000434 RID: 1076
	public class Gost3410PrivateKeyParameters : Gost3410KeyParameters
	{
		// Token: 0x060027AC RID: 10156 RVA: 0x000C580C File Offset: 0x000C3A0C
		public Gost3410PrivateKeyParameters(BigInteger x, Gost3410Parameters parameters) : base(true, parameters)
		{
			if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo(base.Parameters.Q) >= 0)
			{
				throw new ArgumentException("Invalid x for GOST3410 private key", "x");
			}
			this.x = x;
		}

		// Token: 0x060027AD RID: 10157 RVA: 0x000C5864 File Offset: 0x000C3A64
		public Gost3410PrivateKeyParameters(BigInteger x, DerObjectIdentifier publicKeyParamSet) : base(true, publicKeyParamSet)
		{
			if (x.SignValue < 1 || x.BitLength > 256 || x.CompareTo(base.Parameters.Q) >= 0)
			{
				throw new ArgumentException("Invalid x for GOST3410 private key", "x");
			}
			this.x = x;
		}

		// Token: 0x1700048B RID: 1163
		// (get) Token: 0x060027AE RID: 10158 RVA: 0x000C58BA File Offset: 0x000C3ABA
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x04001A4C RID: 6732
		private readonly BigInteger x;
	}
}
