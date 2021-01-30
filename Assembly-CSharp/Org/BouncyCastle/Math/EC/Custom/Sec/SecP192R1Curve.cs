using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200030E RID: 782
	internal class SecP192R1Curve : AbstractFpCurve
	{
		// Token: 0x06001BBF RID: 7103 RVA: 0x0009E508 File Offset: 0x0009C708
		public SecP192R1Curve() : base(SecP192R1Curve.q)
		{
			this.m_infinity = new SecP192R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("64210519E59C80E70FA7E9AB72243049FEB8DEECC146B9B1")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFF99DEF836146BC9B1B4D22831"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001BC0 RID: 7104 RVA: 0x0009E58E File Offset: 0x0009C78E
		protected override ECCurve CloneCurve()
		{
			return new SecP192R1Curve();
		}

		// Token: 0x06001BC1 RID: 7105 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000272 RID: 626
		// (get) Token: 0x06001BC2 RID: 7106 RVA: 0x0009E595 File Offset: 0x0009C795
		public virtual BigInteger Q
		{
			get
			{
				return SecP192R1Curve.q;
			}
		}

		// Token: 0x17000273 RID: 627
		// (get) Token: 0x06001BC3 RID: 7107 RVA: 0x0009E59C File Offset: 0x0009C79C
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000274 RID: 628
		// (get) Token: 0x06001BC4 RID: 7108 RVA: 0x0009E5A4 File Offset: 0x0009C7A4
		public override int FieldSize
		{
			get
			{
				return SecP192R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001BC5 RID: 7109 RVA: 0x0009E5B0 File Offset: 0x0009C7B0
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP192R1FieldElement(x);
		}

		// Token: 0x06001BC6 RID: 7110 RVA: 0x0009E5B8 File Offset: 0x0009C7B8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP192R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001BC7 RID: 7111 RVA: 0x0009E5C3 File Offset: 0x0009C7C3
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP192R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015D6 RID: 5590
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFFFFFFFFFF"));

		// Token: 0x040015D7 RID: 5591
		private const int SecP192R1_DEFAULT_COORDS = 2;

		// Token: 0x040015D8 RID: 5592
		protected readonly SecP192R1Point m_infinity;
	}
}
