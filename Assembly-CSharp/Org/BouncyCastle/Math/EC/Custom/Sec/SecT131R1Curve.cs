using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000332 RID: 818
	internal class SecT131R1Curve : AbstractF2mCurve
	{
		// Token: 0x06001DFF RID: 7679 RVA: 0x000A6D84 File Offset: 0x000A4F84
		public SecT131R1Curve() : base(131, 2, 3, 8)
		{
			this.m_infinity = new SecT131R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07A11B09A76B562144418FF3FF8C2570B8")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0217C05610884B63B9C6C7291678F9D341")));
			this.m_order = new BigInteger(1, Hex.Decode("0400000000000000023123953A9464B54D"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001E00 RID: 7680 RVA: 0x000A6E0D File Offset: 0x000A500D
		protected override ECCurve CloneCurve()
		{
			return new SecT131R1Curve();
		}

		// Token: 0x06001E01 RID: 7681 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002C9 RID: 713
		// (get) Token: 0x06001E02 RID: 7682 RVA: 0x000A6E14 File Offset: 0x000A5014
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002CA RID: 714
		// (get) Token: 0x06001E03 RID: 7683 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x06001E04 RID: 7684 RVA: 0x000A6E1C File Offset: 0x000A501C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT131FieldElement(x);
		}

		// Token: 0x06001E05 RID: 7685 RVA: 0x000A6E24 File Offset: 0x000A5024
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT131R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001E06 RID: 7686 RVA: 0x000A6E2F File Offset: 0x000A502F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT131R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002CB RID: 715
		// (get) Token: 0x06001E07 RID: 7687 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002CC RID: 716
		// (get) Token: 0x06001E08 RID: 7688 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x170002CD RID: 717
		// (get) Token: 0x06001E09 RID: 7689 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002CE RID: 718
		// (get) Token: 0x06001E0A RID: 7690 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170002CF RID: 719
		// (get) Token: 0x06001E0B RID: 7691 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002D0 RID: 720
		// (get) Token: 0x06001E0C RID: 7692 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x04001626 RID: 5670
		private const int SecT131R1_DEFAULT_COORDS = 6;

		// Token: 0x04001627 RID: 5671
		protected readonly SecT131R1Point m_infinity;
	}
}
