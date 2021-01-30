using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035E RID: 862
	internal class SecT571R1Curve : AbstractF2mCurve
	{
		// Token: 0x060020DB RID: 8411 RVA: 0x000B1E20 File Offset: 0x000B0020
		public SecT571R1Curve() : base(571, 2, 5, 10)
		{
			this.m_infinity = new SecT571R1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = SecT571R1Curve.SecT571R1_B;
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFE661CE18FF55987308059B186823851EC7DD9CA1161DE93D5174D66E8382E9BB2FE84E47"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x060020DC RID: 8412 RVA: 0x000B1E8E File Offset: 0x000B008E
		protected override ECCurve CloneCurve()
		{
			return new SecT571R1Curve();
		}

		// Token: 0x060020DD RID: 8413 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x1700039E RID: 926
		// (get) Token: 0x060020DE RID: 8414 RVA: 0x000B1E95 File Offset: 0x000B0095
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700039F RID: 927
		// (get) Token: 0x060020DF RID: 8415 RVA: 0x000B1541 File Offset: 0x000AF741
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060020E0 RID: 8416 RVA: 0x000B17F5 File Offset: 0x000AF9F5
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT571FieldElement(x);
		}

		// Token: 0x060020E1 RID: 8417 RVA: 0x000B1E9D File Offset: 0x000B009D
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT571R1Point(this, x, y, withCompression);
		}

		// Token: 0x060020E2 RID: 8418 RVA: 0x000B1EA8 File Offset: 0x000B00A8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT571R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170003A0 RID: 928
		// (get) Token: 0x060020E3 RID: 8419 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A1 RID: 929
		// (get) Token: 0x060020E4 RID: 8420 RVA: 0x000B1541 File Offset: 0x000AF741
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x170003A2 RID: 930
		// (get) Token: 0x060020E5 RID: 8421 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003A3 RID: 931
		// (get) Token: 0x060020E6 RID: 8422 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170003A4 RID: 932
		// (get) Token: 0x060020E7 RID: 8423 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x170003A5 RID: 933
		// (get) Token: 0x060020E8 RID: 8424 RVA: 0x000B1729 File Offset: 0x000AF929
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x0400165C RID: 5724
		private const int SecT571R1_DEFAULT_COORDS = 6;

		// Token: 0x0400165D RID: 5725
		protected readonly SecT571R1Point m_infinity;

		// Token: 0x0400165E RID: 5726
		internal static readonly SecT571FieldElement SecT571R1_B = new SecT571FieldElement(new BigInteger(1, Hex.Decode("02F40E7E2221F295DE297117B7F3D62F5C6A97FFCB8CEFF1CD6BA8CE4A9A18AD84FFABBD8EFA59332BE7AD6756A66E294AFD185A78FF12AA520E4DE739BACA0C7FFEFF7F2955727A")));

		// Token: 0x0400165F RID: 5727
		internal static readonly SecT571FieldElement SecT571R1_B_SQRT = (SecT571FieldElement)SecT571R1Curve.SecT571R1_B.Sqrt();
	}
}
