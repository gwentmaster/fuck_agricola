using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000338 RID: 824
	internal class SecT163K1Curve : AbstractF2mCurve
	{
		// Token: 0x06001E63 RID: 7779 RVA: 0x000A8618 File Offset: 0x000A6818
		public SecT163K1Curve() : base(163, 3, 6, 7)
		{
			this.m_infinity = new SecT163K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.One);
			this.m_b = this.m_a;
			this.m_order = new BigInteger(1, Hex.Decode("04000000000000000000020108A2E0CC0D99F8A5EF"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001E64 RID: 7780 RVA: 0x000A8686 File Offset: 0x000A6886
		protected override ECCurve CloneCurve()
		{
			return new SecT163K1Curve();
		}

		// Token: 0x06001E65 RID: 7781 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x06001E66 RID: 7782 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x170002E6 RID: 742
		// (get) Token: 0x06001E67 RID: 7783 RVA: 0x000A8694 File Offset: 0x000A6894
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002E7 RID: 743
		// (get) Token: 0x06001E68 RID: 7784 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06001E69 RID: 7785 RVA: 0x000A869C File Offset: 0x000A689C
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT163FieldElement(x);
		}

		// Token: 0x06001E6A RID: 7786 RVA: 0x000A86A4 File Offset: 0x000A68A4
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT163K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001E6B RID: 7787 RVA: 0x000A86AF File Offset: 0x000A68AF
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT163K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002E8 RID: 744
		// (get) Token: 0x06001E6C RID: 7788 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002E9 RID: 745
		// (get) Token: 0x06001E6D RID: 7789 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06001E6E RID: 7790 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsTrinomial
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06001E6F RID: 7791 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06001E70 RID: 7792 RVA: 0x000A85D1 File Offset: 0x000A67D1
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06001E71 RID: 7793 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x0400162E RID: 5678
		private const int SecT163K1_DEFAULT_COORDS = 6;

		// Token: 0x0400162F RID: 5679
		protected readonly SecT163K1Point m_infinity;
	}
}
