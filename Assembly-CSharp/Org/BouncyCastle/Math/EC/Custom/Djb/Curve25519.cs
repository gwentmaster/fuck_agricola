using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x02000360 RID: 864
	internal class Curve25519 : AbstractFpCurve
	{
		// Token: 0x060020F4 RID: 8436 RVA: 0x000B24D0 File Offset: 0x000B06D0
		public Curve25519() : base(Curve25519.q)
		{
			this.m_infinity = new Curve25519Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("2AAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA984914A144")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("7B425ED097B425ED097B425ED097B425ED097B425ED097B4260B5E9C7710C864")));
			this.m_order = new BigInteger(1, Hex.Decode("1000000000000000000000000000000014DEF9DEA2F79CD65812631A5CF5D3ED"));
			this.m_cofactor = BigInteger.ValueOf(8L);
			this.m_coord = 4;
		}

		// Token: 0x060020F5 RID: 8437 RVA: 0x000B2558 File Offset: 0x000B0758
		protected override ECCurve CloneCurve()
		{
			return new Curve25519();
		}

		// Token: 0x060020F6 RID: 8438 RVA: 0x000B255F File Offset: 0x000B075F
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 4;
		}

		// Token: 0x170003A8 RID: 936
		// (get) Token: 0x060020F7 RID: 8439 RVA: 0x000B2568 File Offset: 0x000B0768
		public virtual BigInteger Q
		{
			get
			{
				return Curve25519.q;
			}
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x060020F8 RID: 8440 RVA: 0x000B256F File Offset: 0x000B076F
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x060020F9 RID: 8441 RVA: 0x000B2577 File Offset: 0x000B0777
		public override int FieldSize
		{
			get
			{
				return Curve25519.q.BitLength;
			}
		}

		// Token: 0x060020FA RID: 8442 RVA: 0x000B2583 File Offset: 0x000B0783
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new Curve25519FieldElement(x);
		}

		// Token: 0x060020FB RID: 8443 RVA: 0x000B258B File Offset: 0x000B078B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new Curve25519Point(this, x, y, withCompression);
		}

		// Token: 0x060020FC RID: 8444 RVA: 0x000B2596 File Offset: 0x000B0796
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new Curve25519Point(this, x, y, zs, withCompression);
		}

		// Token: 0x04001660 RID: 5728
		public static readonly BigInteger q = Nat256.ToBigInteger(Curve25519Field.P);

		// Token: 0x04001661 RID: 5729
		private const int Curve25519_DEFAULT_COORDS = 4;

		// Token: 0x04001662 RID: 5730
		protected readonly Curve25519Point m_infinity;
	}
}
