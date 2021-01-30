using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200030A RID: 778
	internal class SecP192K1Curve : AbstractFpCurve
	{
		// Token: 0x06001B84 RID: 7044 RVA: 0x0009D7D0 File Offset: 0x0009B9D0
		public SecP192K1Curve() : base(SecP192K1Curve.q)
		{
			this.m_infinity = new SecP192K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(3L));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFE26F2FC170F69466A74DEFD8D"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x0009D842 File Offset: 0x0009BA42
		protected override ECCurve CloneCurve()
		{
			return new SecP192K1Curve();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700026B RID: 619
		// (get) Token: 0x06001B87 RID: 7047 RVA: 0x0009D849 File Offset: 0x0009BA49
		public virtual BigInteger Q
		{
			get
			{
				return SecP192K1Curve.q;
			}
		}

		// Token: 0x1700026C RID: 620
		// (get) Token: 0x06001B88 RID: 7048 RVA: 0x0009D850 File Offset: 0x0009BA50
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700026D RID: 621
		// (get) Token: 0x06001B89 RID: 7049 RVA: 0x0009D858 File Offset: 0x0009BA58
		public override int FieldSize
		{
			get
			{
				return SecP192K1Curve.q.BitLength;
			}
		}

		// Token: 0x06001B8A RID: 7050 RVA: 0x0009D864 File Offset: 0x0009BA64
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP192K1FieldElement(x);
		}

		// Token: 0x06001B8B RID: 7051 RVA: 0x0009D86C File Offset: 0x0009BA6C
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP192K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001B8C RID: 7052 RVA: 0x0009D877 File Offset: 0x0009BA77
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP192K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015CB RID: 5579
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFEE37"));

		// Token: 0x040015CC RID: 5580
		private const int SECP192K1_DEFAULT_COORDS = 2;

		// Token: 0x040015CD RID: 5581
		protected readonly SecP192K1Point m_infinity;
	}
}
