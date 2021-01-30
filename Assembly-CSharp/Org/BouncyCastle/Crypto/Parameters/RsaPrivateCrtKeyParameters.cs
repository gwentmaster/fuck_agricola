using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000447 RID: 1095
	public class RsaPrivateCrtKeyParameters : RsaKeyParameters
	{
		// Token: 0x060027F4 RID: 10228 RVA: 0x000C5FFC File Offset: 0x000C41FC
		public RsaPrivateCrtKeyParameters(BigInteger modulus, BigInteger publicExponent, BigInteger privateExponent, BigInteger p, BigInteger q, BigInteger dP, BigInteger dQ, BigInteger qInv) : base(true, modulus, privateExponent)
		{
			RsaPrivateCrtKeyParameters.ValidateValue(publicExponent, "publicExponent", "exponent");
			RsaPrivateCrtKeyParameters.ValidateValue(p, "p", "P value");
			RsaPrivateCrtKeyParameters.ValidateValue(q, "q", "Q value");
			RsaPrivateCrtKeyParameters.ValidateValue(dP, "dP", "DP value");
			RsaPrivateCrtKeyParameters.ValidateValue(dQ, "dQ", "DQ value");
			RsaPrivateCrtKeyParameters.ValidateValue(qInv, "qInv", "InverseQ value");
			this.e = publicExponent;
			this.p = p;
			this.q = q;
			this.dP = dP;
			this.dQ = dQ;
			this.qInv = qInv;
		}

		// Token: 0x170004A5 RID: 1189
		// (get) Token: 0x060027F5 RID: 10229 RVA: 0x000C60A6 File Offset: 0x000C42A6
		public BigInteger PublicExponent
		{
			get
			{
				return this.e;
			}
		}

		// Token: 0x170004A6 RID: 1190
		// (get) Token: 0x060027F6 RID: 10230 RVA: 0x000C60AE File Offset: 0x000C42AE
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x170004A7 RID: 1191
		// (get) Token: 0x060027F7 RID: 10231 RVA: 0x000C60B6 File Offset: 0x000C42B6
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x170004A8 RID: 1192
		// (get) Token: 0x060027F8 RID: 10232 RVA: 0x000C60BE File Offset: 0x000C42BE
		public BigInteger DP
		{
			get
			{
				return this.dP;
			}
		}

		// Token: 0x170004A9 RID: 1193
		// (get) Token: 0x060027F9 RID: 10233 RVA: 0x000C60C6 File Offset: 0x000C42C6
		public BigInteger DQ
		{
			get
			{
				return this.dQ;
			}
		}

		// Token: 0x170004AA RID: 1194
		// (get) Token: 0x060027FA RID: 10234 RVA: 0x000C60CE File Offset: 0x000C42CE
		public BigInteger QInv
		{
			get
			{
				return this.qInv;
			}
		}

		// Token: 0x060027FB RID: 10235 RVA: 0x000C60D8 File Offset: 0x000C42D8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = obj as RsaPrivateCrtKeyParameters;
			return rsaPrivateCrtKeyParameters != null && (rsaPrivateCrtKeyParameters.DP.Equals(this.dP) && rsaPrivateCrtKeyParameters.DQ.Equals(this.dQ) && rsaPrivateCrtKeyParameters.Exponent.Equals(base.Exponent) && rsaPrivateCrtKeyParameters.Modulus.Equals(base.Modulus) && rsaPrivateCrtKeyParameters.P.Equals(this.p) && rsaPrivateCrtKeyParameters.Q.Equals(this.q) && rsaPrivateCrtKeyParameters.PublicExponent.Equals(this.e)) && rsaPrivateCrtKeyParameters.QInv.Equals(this.qInv);
		}

		// Token: 0x060027FC RID: 10236 RVA: 0x000C6194 File Offset: 0x000C4394
		public override int GetHashCode()
		{
			return this.DP.GetHashCode() ^ this.DQ.GetHashCode() ^ base.Exponent.GetHashCode() ^ base.Modulus.GetHashCode() ^ this.P.GetHashCode() ^ this.Q.GetHashCode() ^ this.PublicExponent.GetHashCode() ^ this.QInv.GetHashCode();
		}

		// Token: 0x060027FD RID: 10237 RVA: 0x000C6200 File Offset: 0x000C4400
		private static void ValidateValue(BigInteger x, string name, string desc)
		{
			if (x == null)
			{
				throw new ArgumentNullException(name);
			}
			if (x.SignValue <= 0)
			{
				throw new ArgumentException("Not a valid RSA " + desc, name);
			}
		}

		// Token: 0x04001A6F RID: 6767
		private readonly BigInteger e;

		// Token: 0x04001A70 RID: 6768
		private readonly BigInteger p;

		// Token: 0x04001A71 RID: 6769
		private readonly BigInteger q;

		// Token: 0x04001A72 RID: 6770
		private readonly BigInteger dP;

		// Token: 0x04001A73 RID: 6771
		private readonly BigInteger dQ;

		// Token: 0x04001A74 RID: 6772
		private readonly BigInteger qInv;
	}
}
