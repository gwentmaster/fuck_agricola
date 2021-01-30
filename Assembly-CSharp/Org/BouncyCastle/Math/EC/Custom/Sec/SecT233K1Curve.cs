using System;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000346 RID: 838
	internal class SecT233K1Curve : AbstractF2mCurve
	{
		// Token: 0x06001F44 RID: 8004 RVA: 0x000ABCA0 File Offset: 0x000A9EA0
		public SecT233K1Curve() : base(233, 74, 0, 0)
		{
			this.m_infinity = new SecT233K1Point(this, null, null);
			this.m_a = this.FromBigInteger(BigInteger.Zero);
			this.m_b = this.FromBigInteger(BigInteger.One);
			this.m_order = new BigInteger(1, Hex.Decode("8000000000000000000000000000069D5BB915BCD46EFB1AD5F173ABDF"));
			this.m_cofactor = BigInteger.ValueOf(4L);
			this.m_coord = 6;
		}

		// Token: 0x06001F45 RID: 8005 RVA: 0x000ABD16 File Offset: 0x000A9F16
		protected override ECCurve CloneCurve()
		{
			return new SecT233K1Curve();
		}

		// Token: 0x06001F46 RID: 8006 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x06001F47 RID: 8007 RVA: 0x000A868D File Offset: 0x000A688D
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			return new WTauNafMultiplier();
		}

		// Token: 0x1700032A RID: 810
		// (get) Token: 0x06001F48 RID: 8008 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x06001F49 RID: 8009 RVA: 0x000ABD1D File Offset: 0x000A9F1D
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT233FieldElement(x);
		}

		// Token: 0x06001F4A RID: 8010 RVA: 0x000ABD25 File Offset: 0x000A9F25
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT233K1Point(this, x, y, withCompression);
		}

		// Token: 0x06001F4B RID: 8011 RVA: 0x000ABD30 File Offset: 0x000A9F30
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT233K1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x1700032B RID: 811
		// (get) Token: 0x06001F4C RID: 8012 RVA: 0x000ABD3D File Offset: 0x000A9F3D
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x1700032C RID: 812
		// (get) Token: 0x06001F4D RID: 8013 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsKoblitz
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700032D RID: 813
		// (get) Token: 0x06001F4E RID: 8014 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x1700032E RID: 814
		// (get) Token: 0x06001F4F RID: 8015 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700032F RID: 815
		// (get) Token: 0x06001F50 RID: 8016 RVA: 0x000ABC59 File Offset: 0x000A9E59
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x17000330 RID: 816
		// (get) Token: 0x06001F51 RID: 8017 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000331 RID: 817
		// (get) Token: 0x06001F52 RID: 8018 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0400163E RID: 5694
		private const int SecT233K1_DEFAULT_COORDS = 6;

		// Token: 0x0400163F RID: 5695
		protected readonly SecT233K1Point m_infinity;
	}
}
