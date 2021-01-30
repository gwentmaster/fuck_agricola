using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200031A RID: 794
	internal class SecP256K1Curve : AbstractFpCurve
	{
		// Token: 0x06001C79 RID: 7289 RVA: 0x000A11F4 File Offset: 0x0009F3F4
		public SecP256K1Curve() : base(SecP256K1Curve.q)
		{
			this.m_infinity = new SecP256K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.ValueOf(7L));
			this.m_order = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEBAAEDCE6AF48A03BBFD25E8CD0364141"));
			this.m_cofactor = BigInteger.One;
			this.m_coord = 2;
		}

		// Token: 0x06001C7A RID: 7290 RVA: 0x000A1266 File Offset: 0x0009F466
		protected override ECCurve CloneCurve()
		{
			return new SecP256K1Curve();
		}

		// Token: 0x06001C7B RID: 7291 RVA: 0x0009A95D File Offset: 0x00098B5D
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 2;
		}

		// Token: 0x17000287 RID: 647
		// (get) Token: 0x06001C7C RID: 7292 RVA: 0x000A126D File Offset: 0x0009F46D
		public virtual BigInteger Q
		{
			get
			{
				return SecP256K1Curve.q;
			}
		}

		// Token: 0x17000288 RID: 648
		// (get) Token: 0x06001C7D RID: 7293 RVA: 0x000A1274 File Offset: 0x0009F474
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000289 RID: 649
		// (get) Token: 0x06001C7E RID: 7294 RVA: 0x000A127C File Offset: 0x0009F47C
		public override int FieldSize
		{
			get
			{
				return SecP256K1Curve.q.BitLength;
			}
		}

		// Token: 0x06001C7F RID: 7295 RVA: 0x000A1288 File Offset: 0x0009F488
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecP256K1FieldElement(x);
		}

		// Token: 0x06001C80 RID: 7296 RVA: 0x000A1290 File Offset: 0x0009F490
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecP256K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001C81 RID: 7297 RVA: 0x000A129B File Offset: 0x0009F49B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecP256K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x040015F6 RID: 5622
		public static readonly BigInteger q = new BigInteger(1, Hex.Decode("FFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFFEFFFFFC2F"));

		// Token: 0x040015F7 RID: 5623
		private const int SECP256K1_DEFAULT_COORDS = 2;

		// Token: 0x040015F8 RID: 5624
		protected readonly SecP256K1Point m_infinity;
	}
}
