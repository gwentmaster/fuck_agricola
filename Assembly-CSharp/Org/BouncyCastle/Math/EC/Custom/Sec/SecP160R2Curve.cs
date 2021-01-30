using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000306 RID: 774
	internal class SecP160R2Curve : AbstractFpCurve
	{
		// Token: 0x06001B49 RID: 6985 RVA: 0x0009CA30 File Offset: 0x0009AC30
		public SecP160R2Curve() : base(SecP160R2Curve.q)
		{
			this.m_infinity = new SecP160R2Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC70")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B4E134D3FB59EB8BAB57274904664D5AF50388BA")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000000000351EE786A818F3A1A16B"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001B4A RID: 6986 RVA: 0x0009CAB6 File Offset: 0x0009ACB6
		protected override ECCurve CloneCurve()
		{
			return new SecP160R2Curve();
		}

		// Token: 0x06001B4B RID: 6987 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000264 RID: 612
		// (get) Token: 0x06001B4C RID: 6988 RVA: 0x0009CABD File Offset: 0x0009ACBD
		public virtual BigInteger Q
		{
			get
			{
				return SecP160R2Curve.q;
			}
		}

		// Token: 0x17000265 RID: 613
		// (get) Token: 0x06001B4D RID: 6989 RVA: 0x0009CAC4 File Offset: 0x0009ACC4
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000266 RID: 614
		// (get) Token: 0x06001B4E RID: 6990 RVA: 0x0009CACC File Offset: 0x0009ACCC
		public override int FieldSize
		{
			get
			{
				return SecP160R2Curve.q.BitLength;
			}
		}

		// Token: 0x06001B4F RID: 6991 RVA: 0x0009B754 File Offset: 0x00099954
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP160R2FieldElement(x);
		}

		// Token: 0x06001B50 RID: 6992 RVA: 0x0009CAD8 File Offset: 0x0009ACD8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP160R2Point(this, x, y, withCompression);
		}

		// Token: 0x06001B51 RID: 6993 RVA: 0x0009CAE3 File Offset: 0x0009ACE3
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP160R2Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015C0 RID: 5568
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFAC73"));

		// Token: 0x040015C1 RID: 5569
		private const int SecP160R2_DEFAULT_COORDS = 2;

		// Token: 0x040015C2 RID: 5570
		protected readonly SecP160R2Point m_infinity;
	}
}
