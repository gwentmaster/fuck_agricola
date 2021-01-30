using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000300 RID: 768
	internal class SecP160K1Curve : AbstractFpCurve
	{
		// Token: 0x06001AFB RID: 6907 RVA: 0x0009B6C0 File Offset: 0x000998C0
		public SecP160K1Curve() : base(SecP160K1Curve.q)
		{
			this.m_infinity = new SecP160K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(7L));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000001B8FA16DFAB9ACA16B6B3"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0009B732 File Offset: 0x00099932
		protected override ECCurve CloneCurve()
		{
			return new SecP160K1Curve();
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001AFE RID: 6910 RVA: 0x0009B739 File Offset: 0x00099939
		public virtual BigInteger Q
		{
			get
			{
				return SecP160K1Curve.q;
			}
		}

		// Token: 0x1700025B RID: 603
		// (get) Token: 0x06001AFF RID: 6911 RVA: 0x0009B740 File Offset: 0x00099940
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700025C RID: 604
		// (get) Token: 0x06001B00 RID: 6912 RVA: 0x0009B748 File Offset: 0x00099948
		public override int FieldSize
		{
			get
			{
				return SecP160K1Curve.q.BitLength;
			}
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0009B754 File Offset: 0x00099954
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R2FieldElement(x);
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0009B75C File Offset: 0x0009995C
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0009B767 File Offset: 0x00099967
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015B2 RID: 5554
		public static readonly BigInteger q = SecP160R2Curve.q;

		// Token: 0x040015B3 RID: 5555
		private const int SECP160K1_DEFAULT_COORDS = 2;

		// Token: 0x040015B4 RID: 5556
		protected readonly SecP160K1Point m_infinity;
	}
}
