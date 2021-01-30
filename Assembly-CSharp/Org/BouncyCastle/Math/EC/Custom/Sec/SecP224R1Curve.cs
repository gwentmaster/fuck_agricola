using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000316 RID: 790
	internal class SecP224R1Curve : AbstractFpCurve
	{
		// Token: 0x06001C37 RID: 7223 RVA: 0x000A0154 File Offset: 0x0009E354
		public SecP224R1Curve() : base(SecP224R1Curve.q)
		{
			this.m_infinity = new SecP224R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFFFFFFFFFE")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B4050A850C04B3ABF54132565044B0B7D7BFD8BA270B39432355FFB4")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFF16A2E0B8F03E13DD29455C5C2A3D"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001C38 RID: 7224 RVA: 0x000A01DA File Offset: 0x0009E3DA
		protected override ECCurve CloneCurve()
		{
			return new SecP224R1Curve();
		}

		// Token: 0x06001C39 RID: 7225 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000280 RID: 640
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x000A01E1 File Offset: 0x0009E3E1
		public virtual BigInteger Q
		{
			get
			{
				return SecP224R1Curve.q;
			}
		}

		// Token: 0x17000281 RID: 641
		// (get) Token: 0x06001C3B RID: 7227 RVA: 0x000A01E8 File Offset: 0x0009E3E8
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000282 RID: 642
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x000A01F0 File Offset: 0x0009E3F0
		public override int FieldSize
		{
			get
			{
				return SecP224R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001C3D RID: 7229 RVA: 0x000A01FC File Offset: 0x0009E3FC
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP224R1FieldElement(x);
		}

		// Token: 0x06001C3E RID: 7230 RVA: 0x000A0204 File Offset: 0x0009E404
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP224R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001C3F RID: 7231 RVA: 0x000A020F File Offset: 0x0009E40F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP224R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015EC RID: 5612
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF000000000000000000000001"));

		// Token: 0x040015ED RID: 5613
		private const int SecP224R1_DEFAULT_COORDS = 2;

		// Token: 0x040015EE RID: 5614
		protected readonly SecP224R1Point m_infinity;
	}
}
