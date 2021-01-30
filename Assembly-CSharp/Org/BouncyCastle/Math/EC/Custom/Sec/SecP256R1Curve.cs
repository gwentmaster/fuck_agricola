using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200031E RID: 798
	internal class SecP256R1Curve : AbstractFpCurve
	{
		// Token: 0x06001CB4 RID: 7348 RVA: 0x000A1F38 File Offset: 0x000A0138
		public SecP256R1Curve() : base(SecP256R1Curve.q)
		{
			this.m_infinity = new SecP256R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFC")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("5AC635D8AA3A93E7B3EBBD55769886BC651D06B0CC53B0F63BCE3C3E27D2604B")));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFF00000000FFFFFFFFFFFFFFFFBCE6FAADA7179E84F3B9CAC2FC632551"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001CB5 RID: 7349 RVA: 0x000A1FBE File Offset: 0x000A01BE
		protected override ECCurve CloneCurve()
		{
			return new SecP256R1Curve();
		}

		// Token: 0x06001CB6 RID: 7350 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001CB7 RID: 7351 RVA: 0x000A1FC5 File Offset: 0x000A01C5
		public virtual BigInteger Q
		{
			get
			{
				return SecP256R1Curve.q;
			}
		}

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001CB8 RID: 7352 RVA: 0x000A1FCC File Offset: 0x000A01CC
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001CB9 RID: 7353 RVA: 0x000A1FD4 File Offset: 0x000A01D4
		public override int FieldSize
		{
			get
			{
				return SecP256R1Curve.q.BitLength;
			}
		}

		// Token: 0x06001CBA RID: 7354 RVA: 0x000A1FE0 File Offset: 0x000A01E0
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP256R1FieldElement(x);
		}

		// Token: 0x06001CBB RID: 7355 RVA: 0x000A1FE8 File Offset: 0x000A01E8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP256R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001CBC RID: 7356 RVA: 0x000A1FF3 File Offset: 0x000A01F3
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP256R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x04001601 RID: 5633
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFF00000001000000000000000000000000FFFFFFFFFFFFFFFFFFFFFFFF"));

		// Token: 0x04001602 RID: 5634
		private const int SecP256R1_DEFAULT_COORDS = 2;

		// Token: 0x04001603 RID: 5635
		protected readonly SecP256R1Point m_infinity;
	}
}
