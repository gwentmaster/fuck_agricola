using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000334 RID: 820
	internal class SecT131R2Curve : AbstractF2mCurve
	{
		// Token: 0x06001E17 RID: 7703 RVA: 0x000A7474 File Offset: 0x000A5674
		public SecT131R2Curve() : base(131, 2, 3, 8)
		{
			this.m_infinity = new SecT131R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("03E5A88919D7CAFCBF415F07C2176573B2")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("04B8266A46C55657AC734CE38F018F2192")));
			this.m_order = new BigInteger(1, Hex.Decode("0400000000000000016954A233049BA98F"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001E18 RID: 7704 RVA: 0x000A74FD File Offset: 0x000A56FD
		protected override ECCurve CloneCurve()
		{
			return new SecT131R2Curve();
		}

		// Token: 0x06001E19 RID: 7705 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002D3 RID: 723
		// (get) Token: 0x06001E1A RID: 7706 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x06001E1B RID: 7707 RVA: 0x000A6E1C File Offset: 0x000A501C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT131FieldElement(x);
		}

		// Token: 0x06001E1C RID: 7708 RVA: 0x000A7504 File Offset: 0x000A5704
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT131R2Point(this, x, y, withCompression);
		}

		// Token: 0x06001E1D RID: 7709 RVA: 0x000A750F File Offset: 0x000A570F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT131R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002D4 RID: 724
		// (get) Token: 0x06001E1E RID: 7710 RVA: 0x000A751C File Offset: 0x000A571C
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002D5 RID: 725
		// (get) Token: 0x06001E1F RID: 7711 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002D6 RID: 726
		// (get) Token: 0x06001E20 RID: 7712 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x170002D7 RID: 727
		// (get) Token: 0x06001E21 RID: 7713 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002D8 RID: 728
		// (get) Token: 0x06001E22 RID: 7714 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x06001E23 RID: 7715 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06001E24 RID: 7716 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x04001628 RID: 5672
		private const int SecT131R2_DEFAULT_COORDS = 6;

		// Token: 0x04001629 RID: 5673
		protected readonly SecT131R2Point m_infinity;
	}
}
