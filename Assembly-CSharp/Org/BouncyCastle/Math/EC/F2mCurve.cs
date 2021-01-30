using System;
using Org.BouncyCastle.Math.EC.Multiplier;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002DF RID: 735
	public class F2mCurve : AbstractF2mCurve
	{
		// Token: 0x06001963 RID: 6499 RVA: 0x00094020 File Offset: 0x00092220
		public F2mCurve(int m, int k, BigInteger a, BigInteger b) : this(m, k, 0, 0, a, b, null, null)
		{
		}

		// Token: 0x06001964 RID: 6500 RVA: 0x0009403C File Offset: 0x0009223C
		public F2mCurve(int m, int k, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : this(m, k, 0, 0, a, b, order, cofactor)
		{
		}

		// Token: 0x06001965 RID: 6501 RVA: 0x0009405C File Offset: 0x0009225C
		public F2mCurve(int m, int k1, int k2, int k3, BigInteger a, BigInteger b) : this(m, k1, k2, k3, a, b, null, null)
		{
		}

		// Token: 0x06001966 RID: 6502 RVA: 0x0009407C File Offset: 0x0009227C
		public F2mCurve(int m, int k1, int k2, int k3, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : base(m, k1, k2, k3)
		{
			this.m = m;
			this.k1 = k1;
			this.k2 = k2;
			this.k3 = k3;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_infinity = new F2mPoint(this, null, null);
			if (k1 == 0)
			{
				throw new ArgumentException("k1 must be > 0");
			}
			if (k2 == 0)
			{
				if (k3 != 0)
				{
					throw new ArgumentException("k3 must be 0 if k2 == 0");
				}
			}
			else
			{
				if (k2 <= k1)
				{
					throw new ArgumentException("k2 must be > k1");
				}
				if (k3 <= k2)
				{
					throw new ArgumentException("k3 must be > k2");
				}
			}
			this.m_a = this.FromBigInteger(a);
			this.m_b = this.FromBigInteger(b);
			this.m_coord = 6;
		}

		// Token: 0x06001967 RID: 6503 RVA: 0x00094134 File Offset: 0x00092334
		protected F2mCurve(int m, int k1, int k2, int k3, ECFieldElement a, ECFieldElement b, BigInteger order, BigInteger cofactor) : base(m, k1, k2, k3)
		{
			this.m = m;
			this.k1 = k1;
			this.k2 = k2;
			this.k3 = k3;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_infinity = new F2mPoint(this, null, null);
			this.m_a = a;
			this.m_b = b;
			this.m_coord = 6;
		}

		// Token: 0x06001968 RID: 6504 RVA: 0x0009419E File Offset: 0x0009239E
		protected override ECCurve CloneCurve()
		{
			return new F2mCurve(this.m, this.k1, this.k2, this.k3, this.m_a, this.m_b, this.m_order, this.m_cofactor);
		}

		// Token: 0x06001969 RID: 6505 RVA: 0x000941D5 File Offset: 0x000923D5
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord <= 1 || coord == 6;
		}

		// Token: 0x0600196A RID: 6506 RVA: 0x000941E2 File Offset: 0x000923E2
		protected override ECMultiplier CreateDefaultMultiplier()
		{
			if (this.IsKoblitz)
			{
				return new WTauNafMultiplier();
			}
			return base.CreateDefaultMultiplier();
		}

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x0600196B RID: 6507 RVA: 0x000941F8 File Offset: 0x000923F8
		public override int FieldSize
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x0600196C RID: 6508 RVA: 0x00094200 File Offset: 0x00092400
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new F2mFieldElement(this.m, this.k1, this.k2, this.k3, x);
		}

		// Token: 0x0600196D RID: 6509 RVA: 0x00094220 File Offset: 0x00092420
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new F2mPoint(this, x, y, withCompression);
		}

		// Token: 0x0600196E RID: 6510 RVA: 0x0009422B File Offset: 0x0009242B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new F2mPoint(this, x, y, zs, withCompression);
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x0600196F RID: 6511 RVA: 0x00094238 File Offset: 0x00092438
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06001970 RID: 6512 RVA: 0x000941F8 File Offset: 0x000923F8
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x06001971 RID: 6513 RVA: 0x00094240 File Offset: 0x00092440
		public bool IsTrinomial()
		{
			return this.k2 == 0 && this.k3 == 0;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06001972 RID: 6514 RVA: 0x00094255 File Offset: 0x00092455
		public int K1
		{
			get
			{
				return this.k1;
			}
		}

		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06001973 RID: 6515 RVA: 0x0009425D File Offset: 0x0009245D
		public int K2
		{
			get
			{
				return this.k2;
			}
		}

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06001974 RID: 6516 RVA: 0x00094265 File Offset: 0x00092465
		public int K3
		{
			get
			{
				return this.k3;
			}
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06001975 RID: 6517 RVA: 0x00093765 File Offset: 0x00091965
		[Obsolete("Use 'Order' property instead")]
		public BigInteger N
		{
			get
			{
				return this.m_order;
			}
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x06001976 RID: 6518 RVA: 0x0009376D File Offset: 0x0009196D
		[Obsolete("Use 'Cofactor' property instead")]
		public BigInteger H
		{
			get
			{
				return this.m_cofactor;
			}
		}

		// Token: 0x0400156F RID: 5487
		private const int F2M_DEFAULT_COORDS = 6;

		// Token: 0x04001570 RID: 5488
		private readonly int m;

		// Token: 0x04001571 RID: 5489
		private readonly int k1;

		// Token: 0x04001572 RID: 5490
		private readonly int k2;

		// Token: 0x04001573 RID: 5491
		private readonly int k3;

		// Token: 0x04001574 RID: 5492
		protected readonly F2mPoint m_infinity;
	}
}
