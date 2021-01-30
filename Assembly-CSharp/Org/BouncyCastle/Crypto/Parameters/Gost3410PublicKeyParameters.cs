using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000435 RID: 1077
	public class Gost3410PublicKeyParameters : Gost3410KeyParameters
	{
		// Token: 0x060027AF RID: 10159 RVA: 0x000C58C2 File Offset: 0x000C3AC2
		public Gost3410PublicKeyParameters(BigInteger y, Gost3410Parameters parameters) : base(false, parameters)
		{
			if (y.SignValue < 1 || y.CompareTo(base.Parameters.P) >= 0)
			{
				throw new ArgumentException("Invalid y for GOST3410 public key", "y");
			}
			this.y = y;
		}

		// Token: 0x060027B0 RID: 10160 RVA: 0x000C5900 File Offset: 0x000C3B00
		public Gost3410PublicKeyParameters(BigInteger y, DerObjectIdentifier publicKeyParamSet) : base(false, publicKeyParamSet)
		{
			if (y.SignValue < 1 || y.CompareTo(base.Parameters.P) >= 0)
			{
				throw new ArgumentException("Invalid y for GOST3410 public key", "y");
			}
			this.y = y;
		}

		// Token: 0x1700048C RID: 1164
		// (get) Token: 0x060027B1 RID: 10161 RVA: 0x000C593E File Offset: 0x000C3B3E
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x04001A4D RID: 6733
		private readonly BigInteger y;
	}
}
