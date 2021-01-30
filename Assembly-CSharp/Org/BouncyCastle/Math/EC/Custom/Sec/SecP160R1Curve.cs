using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000302 RID: 770
	internal class SecP160R1Curve : AbstractFpCurve
	{
		// Token: 0x06001B0E RID: 6926 RVA: 0x0009BCA4 File Offset: 0x00099EA4
		public SecP160R1Curve() : base(SecP160R1Curve.q)
		{
			this.m_infinity = new SecP160R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("1C97BEFC54BD7A8B65ACF89F81D4D4ADC565FA45")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000001F4C8F927AED3CA752257"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001B0F RID: 6927 RVA: 0x0009BD2A File Offset: 0x00099F2A
		protected override ECCurve CloneCurve()
		{
			return new SecP160R1Curve();
		}

		// Token: 0x06001B10 RID: 6928 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700025D RID: 605
		// (get) Token: 0x06001B11 RID: 6929 RVA: 0x0009BD31 File Offset: 0x00099F31
		public virtual BigInteger Q
		{
			get
			{
				return SecP160R1Curve.q;
			}
		}

		// Token: 0x1700025E RID: 606
		// (get) Token: 0x06001B12 RID: 6930 RVA: 0x0009BD38 File Offset: 0x00099F38
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700025F RID: 607
		// (get) Token: 0x06001B13 RID: 6931 RVA: 0x0009BD40 File Offset: 0x00099F40
		public override int FieldSize
		{
			get
			{
				return SecP160R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001B14 RID: 6932 RVA: 0x0009BD4C File Offset: 0x00099F4C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R1FieldElement(x);
		}

		// Token: 0x06001B15 RID: 6933 RVA: 0x0009BD54 File Offset: 0x00099F54
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001B16 RID: 6934 RVA: 0x0009BD5F File Offset: 0x00099F5F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015B5 RID: 5557
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF7FFFFFFF"));

		// Token: 0x040015B6 RID: 5558
		private const int SecP160R1_DEFAULT_COORDS = 2;

		// Token: 0x040015B7 RID: 5559
		protected readonly SecP160R1Point m_infinity;
	}
}
