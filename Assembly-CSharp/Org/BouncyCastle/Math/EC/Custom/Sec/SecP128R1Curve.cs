using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020002FC RID: 764
	internal class SecP128R1Curve : AbstractFpCurve
	{
		// Token: 0x06001ABE RID: 6846 RVA: 0x0009A8D0 File Offset: 0x00098AD0
		public SecP128R1Curve() : base(SecP128R1Curve.q)
		{
			this.m_infinity = new SecP128R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("E87579C11079F43DD824993C2CEE5ED3")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFE0000000075A30D1B9038A115"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x0009A956 File Offset: 0x00098B56
		protected override ECCurve CloneCurve()
		{
			return new SecP128R1Curve();
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001AC1 RID: 6849 RVA: 0x0009A966 File Offset: 0x00098B66
		public virtual BigInteger Q
		{
			get
			{
				return SecP128R1Curve.q;
			}
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001AC2 RID: 6850 RVA: 0x0009A96D File Offset: 0x00098B6D
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000255 RID: 597
		// (get) Token: 0x06001AC3 RID: 6851 RVA: 0x0009A975 File Offset: 0x00098B75
		public override int FieldSize
		{
			get
			{
				return SecP128R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x0009A981 File Offset: 0x00098B81
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP128R1FieldElement(x);
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0009A989 File Offset: 0x00098B89
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP128R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x0009A994 File Offset: 0x00098B94
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP128R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015A8 RID: 5544
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFDFFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x040015A9 RID: 5545
		private const int SecP128R1_DEFAULT_COORDS = 2;

		// Token: 0x040015AA RID: 5546
		protected readonly SecP128R1Point m_infinity;
	}
}
