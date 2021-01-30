using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000352 RID: 850
	internal class SecT283R1Curve : AbstractF2mCurve
	{
		// Token: 0x06002010 RID: 8208 RVA: 0x000AEEB0 File Offset: 0x000AD0B0
		public SecT283R1Curve() : base(283, 5, 7, 12)
		{
			this.m_infinity = new SecT283R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("027B680AC8B8596DA5A4AF8A19A0303FCA97FD7645309FA2A581485AF6263E313B79A2F5")));
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEF90399660FC938A90165B042A7CEFADB307"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06002011 RID: 8209 RVA: 0x000AEF2F File Offset: 0x000AD12F
		protected override ECCurve CloneCurve()
		{
			return new SecT283R1Curve();
		}

		// Token: 0x06002012 RID: 8210 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x17000364 RID: 868
		// (get) Token: 0x06002013 RID: 8211 RVA: 0x000AEF36 File Offset: 0x000AD136
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000365 RID: 869
		// (get) Token: 0x06002014 RID: 8212 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x06002015 RID: 8213 RVA: 0x000AE889 File Offset: 0x000ACA89
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT283FieldElement(x);
		}

		// Token: 0x06002016 RID: 8214 RVA: 0x000AEF3E File Offset: 0x000AD13E
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT283R1Point(this, x, y, withCompression);
		}

		// Token: 0x06002017 RID: 8215 RVA: 0x000AEF49 File Offset: 0x000AD149
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT283R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000366 RID: 870
		// (get) Token: 0x06002018 RID: 8216 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000367 RID: 871
		// (get) Token: 0x06002019 RID: 8217 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x17000368 RID: 872
		// (get) Token: 0x0600201A RID: 8218 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000369 RID: 873
		// (get) Token: 0x0600201B RID: 8219 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x1700036A RID: 874
		// (get) Token: 0x0600201C RID: 8220 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x1700036B RID: 875
		// (get) Token: 0x0600201D RID: 8221 RVA: 0x000AE7BC File Offset: 0x000AC9BC
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x0400164D RID: 5709
		private const int SecT283R1_DEFAULT_COORDS = 6;

		// Token: 0x0400164E RID: 5710
		protected readonly SecT283R1Point m_infinity;
	}
}
