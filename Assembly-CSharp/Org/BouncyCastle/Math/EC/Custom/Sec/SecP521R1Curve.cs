using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000326 RID: 806
	internal class SecP521R1Curve : AbstractFpCurve
	{
		// Token: 0x06001D2D RID: 7469 RVA: 0x000A3F1C File Offset: 0x000A211C
		public SecP521R1Curve() : base(SecP521R1Curve.q)
		{
			this.m_infinity = new SecP521R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0051953EB9618E1C9A1F929A21A0B68540EEA2DA725B99B315F3B8B489918EF109E156193951EC7E937B1652C0BD3BB1BF073573DF883D2C34F1EF451FD46B503F00")));
			this.m_order = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFA51868783BF2F966B7FCC0148F709A5D03BB5C9B8899C47AEBB6FB71E91386409"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001D2E RID: 7470 RVA: 0x000A3FA2 File Offset: 0x000A21A2
		protected override ECCurve CloneCurve()
		{
			return new SecP521R1Curve();
		}

		// Token: 0x06001D2F RID: 7471 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700029C RID: 668
		// (get) Token: 0x06001D30 RID: 7472 RVA: 0x000A3FA9 File Offset: 0x000A21A9
		public virtual BigInteger Q
		{
			get
			{
				return SecP521R1Curve.q;
			}
		}

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x06001D31 RID: 7473 RVA: 0x000A3FB0 File Offset: 0x000A21B0
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x06001D32 RID: 7474 RVA: 0x000A3FB8 File Offset: 0x000A21B8
		public override int FieldSize
		{
			get
			{
				return SecP521R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001D33 RID: 7475 RVA: 0x000A3FC4 File Offset: 0x000A21C4
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP521R1FieldElement(x);
		}

		// Token: 0x06001D34 RID: 7476 RVA: 0x000A3FCC File Offset: 0x000A21CC
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP521R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001D35 RID: 7477 RVA: 0x000A3FD7 File Offset: 0x000A21D7
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP521R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x04001614 RID: 5652
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("01FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x04001615 RID: 5653
		private const int SecP521R1_DEFAULT_COORDS = 2;

		// Token: 0x04001616 RID: 5654
		protected readonly SecP521R1Point m_infinity;
	}
}
