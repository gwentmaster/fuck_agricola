using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000322 RID: 802
	internal class SecP384R1Curve : AbstractFpCurve
	{
		// Token: 0x06001CF1 RID: 7409 RVA: 0x000A2E80 File Offset: 0x000A1080
		public SecP384R1Curve() : base(SecP384R1Curve.q)
		{
			this.m_infinity = new SecP384R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("B3312FA7E23EE7E4988E056BE3F82D19181D9C6EFE8141120314088F5013875AC656398D8A2ED19D2A85C8EDD3EC2AEF")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFC7634D81F4372DDF581A0DB248B0A77AECEC196ACCC52973"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001CF2 RID: 7410 RVA: 0x000A2F06 File Offset: 0x000A1106
		protected override ECCurve CloneCurve()
		{
			return new SecP384R1Curve();
		}

		// Token: 0x06001CF3 RID: 7411 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000295 RID: 661
		// (get) Token: 0x06001CF4 RID: 7412 RVA: 0x000A2F0D File Offset: 0x000A110D
		public virtual BigInteger Q
		{
			get
			{
				return SecP384R1Curve.q;
			}
		}

		// Token: 0x17000296 RID: 662
		// (get) Token: 0x06001CF5 RID: 7413 RVA: 0x000A2F14 File Offset: 0x000A1114
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000297 RID: 663
		// (get) Token: 0x06001CF6 RID: 7414 RVA: 0x000A2F1C File Offset: 0x000A111C
		public override int FieldSize
		{
			get
			{
				return SecP384R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001CF7 RID: 7415 RVA: 0x000A2F28 File Offset: 0x000A1128
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP384R1FieldElement(x);
		}

		// Token: 0x06001CF8 RID: 7416 RVA: 0x000A2F30 File Offset: 0x000A1130
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP384R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001CF9 RID: 7417 RVA: 0x000A2F3B File Offset: 0x000A113B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP384R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x0400160A RID: 5642
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFFFF0000000000000000FFFFFFFF"));

		// Token: 0x0400160B RID: 5643
		private const int SecP384R1_DEFAULT_COORDS = 2;

		// Token: 0x0400160C RID: 5644
		protected readonly SecP384R1Point m_infinity;
	}
}
