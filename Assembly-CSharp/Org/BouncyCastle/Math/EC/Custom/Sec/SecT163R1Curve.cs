using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200033A RID: 826
	internal class SecT163R1Curve : AbstractF2mCurve
	{
		// Token: 0x06001E7C RID: 7804 RVA: 0x000A8CB0 File Offset: 0x000A6EB0
		public SecT163R1Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("07B6882CAAEFA84F9554FF8428BD88E246D2782AE2")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("0713612DCDDCB40AAB946BDA29CA91F73AF958AFD9")));
			this.m_order = new BigInteger(1, Hex.Decode("03FFFFFFFFFFFFFFFFFFFF48AAB689C29CA710279B"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000A8D39 File Offset: 0x000A6F39
		protected override ECCurve CloneCurve()
		{
			return new SecT163R1Curve();
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002F0 RID: 752
		// (get) Token: 0x06001E7F RID: 7807 RVA: 0x000A8D40 File Offset: 0x000A6F40
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002F1 RID: 753
		// (get) Token: 0x06001E80 RID: 7808 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06001E81 RID: 7809 RVA: 0x000A869C File Offset: 0x000A689C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x06001E82 RID: 7810 RVA: 0x000A8D48 File Offset: 0x000A6F48
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001E83 RID: 7811 RVA: 0x000A8D53 File Offset: 0x000A6F53
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002F2 RID: 754
		// (get) Token: 0x06001E84 RID: 7812 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F3 RID: 755
		// (get) Token: 0x06001E85 RID: 7813 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x170002F4 RID: 756
		// (get) Token: 0x06001E86 RID: 7814 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002F5 RID: 757
		// (get) Token: 0x06001E87 RID: 7815 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002F6 RID: 758
		// (get) Token: 0x06001E88 RID: 7816 RVA: 0x000A85D1 File Offset: 0x000A67D1
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170002F7 RID: 759
		// (get) Token: 0x06001E89 RID: 7817 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x04001630 RID: 5680
		private const int SecT163R1_DEFAULT_COORDS = 6;

		// Token: 0x04001631 RID: 5681
		protected readonly SecT163R1Point m_infinity;
	}
}
