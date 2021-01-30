using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000312 RID: 786
	internal class SecP224K1Curve : AbstractFpCurve
	{
		// Token: 0x06001BFC RID: 7164 RVA: 0x0009F3AC File Offset: 0x0009D5AC
		public SecP224K1Curve() : base(SecP224K1Curve.q)
		{
			this.m_infinity = new SecP224K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(5L));
			this.m_order = new BigInteger(1, Hex.Decode("010000000000000000000000000001DCE8D2EC6184CAF0A971769FB1F7"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001BFD RID: 7165 RVA: 0x0009F41E File Offset: 0x0009D61E
		protected override ECCurve CloneCurve()
		{
			return new SecP224K1Curve();
		}

		// Token: 0x06001BFE RID: 7166 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000279 RID: 633
		// (get) Token: 0x06001BFF RID: 7167 RVA: 0x0009F425 File Offset: 0x0009D625
		public virtual BigInteger Q
		{
			get
			{
				return SecP224K1Curve.q;
			}
		}

		// Token: 0x1700027A RID: 634
		// (get) Token: 0x06001C00 RID: 7168 RVA: 0x0009F42C File Offset: 0x0009D62C
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700027B RID: 635
		// (get) Token: 0x06001C01 RID: 7169 RVA: 0x0009F434 File Offset: 0x0009D634
		public override int FieldSize
		{
			get
			{
				return SecP224K1Curve.q.BitLength;
			}
		}

		// Token: 0x06001C02 RID: 7170 RVA: 0x0009F440 File Offset: 0x0009D640
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP224K1FieldElement(x);
		}

		// Token: 0x06001C03 RID: 7171 RVA: 0x0009F448 File Offset: 0x0009D648
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP224K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001C04 RID: 7172 RVA: 0x0009F453 File Offset: 0x0009D653
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP224K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015E0 RID: 5600
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFE56D"));

		// Token: 0x040015E1 RID: 5601
		private const int SECP224K1_DEFAULT_COORDS = 2;

		// Token: 0x040015E2 RID: 5602
		protected readonly SecP224K1Point m_infinity;
	}
}
