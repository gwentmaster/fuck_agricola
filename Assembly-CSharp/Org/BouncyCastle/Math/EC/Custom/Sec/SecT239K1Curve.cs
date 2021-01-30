using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200034C RID: 844
	internal class SecT239K1Curve : AbstractF2mCurve
	{
		// Token: 0x06001FA9 RID: 8105 RVA: 0x000AD528 File Offset: 0x000AB728
		public SecT239K1Curve() : base(239, 158, 0, 0)
		{
			this.m_infinity = new SecT239K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("2000000000000000000000000000005A79FEC67CB6E91F1C1DA800E478A5"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x06001FAA RID: 8106 RVA: 0x000AD5A1 File Offset: 0x000AB7A1
		protected override ECCurve CloneCurve()
		{
			return new SecT239K1Curve();
		}

		// Token: 0x06001FAB RID: 8107 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x06001FAC RID: 8108 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x17000347 RID: 839
		// (get) Token: 0x06001FAD RID: 8109 RVA: 0x000AD5A8 File Offset: 0x000AB7A8
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000348 RID: 840
		// (get) Token: 0x06001FAE RID: 8110 RVA: 0x000AD2F5 File Offset: 0x000AB4F5
		public override int FieldSize
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x06001FAF RID: 8111 RVA: 0x000AD5B0 File Offset: 0x000AB7B0
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT239FieldElement(x);
		}

		// Token: 0x06001FB0 RID: 8112 RVA: 0x000AD5B8 File Offset: 0x000AB7B8
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT239K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001FB1 RID: 8113 RVA: 0x000AD5C3 File Offset: 0x000AB7C3
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT239K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x17000349 RID: 841
		// (get) Token: 0x06001FB2 RID: 8114 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700034A RID: 842
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x000AD2F5 File Offset: 0x000AB4F5
		public virtual int M
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x1700034B RID: 843
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700034C RID: 844
		// (get) Token: 0x06001FB5 RID: 8117 RVA: 0x000AD4DD File Offset: 0x000AB6DD
		public virtual int K1
		{
			get
			{
				return 158;
			}
		}

		// Token: 0x1700034D RID: 845
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700034E RID: 846
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x04001645 RID: 5701
		private const int SecT239K1_DEFAULT_COORDS = 6;

		// Token: 0x04001646 RID: 5702
		protected readonly SecT239K1Point m_infinity;
	}
}
