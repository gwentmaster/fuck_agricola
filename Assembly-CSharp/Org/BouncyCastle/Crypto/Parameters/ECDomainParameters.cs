using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000428 RID: 1064
	public class ECDomainParameters
	{
		// Token: 0x06002759 RID: 10073 RVA: 0x000C4E67 File Offset: 0x000C3067
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n) : this(curve, g, n, BigInteger.One)
		{
		}

		// Token: 0x0600275A RID: 10074 RVA: 0x000C4E77 File Offset: 0x000C3077
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h) : this(curve, g, n, h, null)
		{
		}

		// Token: 0x0600275B RID: 10075 RVA: 0x000C4E88 File Offset: 0x000C3088
		public ECDomainParameters(ECCurve curve, ECPoint g, BigInteger n, BigInteger h, byte[] seed)
		{
			if (curve == null)
			{
				throw new ArgumentNullException("curve");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (n == null)
			{
				throw new ArgumentNullException("n");
			}
			if (h == null)
			{
				throw new ArgumentNullException("h");
			}
			this.curve = curve;
			this.g = g.Normalize();
			this.n = n;
			this.h = h;
			this.seed = Arrays.Clone(seed);
		}

		// Token: 0x17000473 RID: 1139
		// (get) Token: 0x0600275C RID: 10076 RVA: 0x000C4F03 File Offset: 0x000C3103
		public ECCurve Curve
		{
			get
			{
				return this.curve;
			}
		}

		// Token: 0x17000474 RID: 1140
		// (get) Token: 0x0600275D RID: 10077 RVA: 0x000C4F0B File Offset: 0x000C310B
		public ECPoint G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000475 RID: 1141
		// (get) Token: 0x0600275E RID: 10078 RVA: 0x000C4F13 File Offset: 0x000C3113
		public BigInteger N
		{
			get
			{
				return this.n;
			}
		}

		// Token: 0x17000476 RID: 1142
		// (get) Token: 0x0600275F RID: 10079 RVA: 0x000C4F1B File Offset: 0x000C311B
		public BigInteger H
		{
			get
			{
				return this.h;
			}
		}

		// Token: 0x06002760 RID: 10080 RVA: 0x000C4F23 File Offset: 0x000C3123
		public byte[] GetSeed()
		{
			return Arrays.Clone(this.seed);
		}

		// Token: 0x06002761 RID: 10081 RVA: 0x000C4F30 File Offset: 0x000C3130
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECDomainParameters ecdomainParameters = obj as ECDomainParameters;
			return ecdomainParameters != null && this.Equals(ecdomainParameters);
		}

		// Token: 0x06002762 RID: 10082 RVA: 0x000C4F58 File Offset: 0x000C3158
		protected virtual bool Equals(ECDomainParameters other)
		{
			return this.curve.Equals(other.curve) && this.g.Equals(other.g) && this.n.Equals(other.n) && this.h.Equals(other.h);
		}

		// Token: 0x06002763 RID: 10083 RVA: 0x000C4FB1 File Offset: 0x000C31B1
		public override int GetHashCode()
		{
			return ((this.curve.GetHashCode() * 37 ^ this.g.GetHashCode()) * 37 ^ this.n.GetHashCode()) * 37 ^ this.h.GetHashCode();
		}

		// Token: 0x04001A32 RID: 6706
		internal ECCurve curve;

		// Token: 0x04001A33 RID: 6707
		internal byte[] seed;

		// Token: 0x04001A34 RID: 6708
		internal ECPoint g;

		// Token: 0x04001A35 RID: 6709
		internal BigInteger n;

		// Token: 0x04001A36 RID: 6710
		internal BigInteger h;
	}
}
