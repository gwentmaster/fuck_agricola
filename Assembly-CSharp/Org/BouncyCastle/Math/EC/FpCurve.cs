using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002DD RID: 733
	public class FpCurve : AbstractFpCurve
	{
		// Token: 0x0600194D RID: 6477 RVA: 0x00093B3B File Offset: 0x00091D3B
		public FpCurve(BigInteger q, BigInteger a, BigInteger b) : this(q, a, b, null, null)
		{
		}

		// Token: 0x0600194E RID: 6478 RVA: 0x00093B48 File Offset: 0x00091D48
		public FpCurve(BigInteger q, BigInteger a, BigInteger b, BigInteger order, BigInteger cofactor) : base(q)
		{
			this.m_q = q;
			this.m_r = FpFieldElement.CalculateResidue(q);
			this.m_infinity = new FpPoint(this, null, null);
			this.m_a = this.FromBigInteger(a);
			this.m_b = this.FromBigInteger(b);
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_coord = 4;
		}

		// Token: 0x0600194F RID: 6479 RVA: 0x00093BAE File Offset: 0x00091DAE
		protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b) : this(q, r, a, b, null, null)
		{
		}

		// Token: 0x06001950 RID: 6480 RVA: 0x00093BC0 File Offset: 0x00091DC0
		protected FpCurve(BigInteger q, BigInteger r, ECFieldElement a, ECFieldElement b, BigInteger order, BigInteger cofactor) : base(q)
		{
			this.m_q = q;
			this.m_r = r;
			this.m_infinity = new FpPoint(this, null, null);
			this.m_a = a;
			this.m_b = b;
			this.m_order = order;
			this.m_cofactor = cofactor;
			this.m_coord = 4;
		}

		// Token: 0x06001951 RID: 6481 RVA: 0x00093C16 File Offset: 0x00091E16
		protected override ECCurve CloneCurve()
		{
			return new FpCurve(this.m_q, this.m_r, this.m_a, this.m_b, this.m_order, this.m_cofactor);
		}

		// Token: 0x06001952 RID: 6482 RVA: 0x00093C41 File Offset: 0x00091E41
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord <= 2 || coord == 4;
		}

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x06001953 RID: 6483 RVA: 0x00093C4E File Offset: 0x00091E4E
		public virtual BigInteger Q
		{
			get
			{
				return this.m_q;
			}
		}

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x06001954 RID: 6484 RVA: 0x00093C56 File Offset: 0x00091E56
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06001955 RID: 6485 RVA: 0x00093C5E File Offset: 0x00091E5E
		public override int FieldSize
		{
			get
			{
				return this.m_q.BitLength;
			}
		}

		// Token: 0x06001956 RID: 6486 RVA: 0x00093C6B File Offset: 0x00091E6B
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new FpFieldElement(this.m_q, this.m_r, x);
		}

		// Token: 0x06001957 RID: 6487 RVA: 0x00093C7F File Offset: 0x00091E7F
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new FpPoint(this, x, y, withCompression);
		}

		// Token: 0x06001958 RID: 6488 RVA: 0x00093C8A File Offset: 0x00091E8A
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new FpPoint(this, x, y, zs, withCompression);
		}

		// Token: 0x06001959 RID: 6489 RVA: 0x00093C98 File Offset: 0x00091E98
		public override ECPoint ImportPoint(ECPoint p)
		{
			if (this != p.Curve && this.CoordinateSystem == 2 && !p.IsInfinity)
			{
				int coordinateSystem = p.Curve.CoordinateSystem;
				if (coordinateSystem - 2 <= 2)
				{
					return new FpPoint(this, this.FromBigInteger(p.RawXCoord.ToBigInteger()), this.FromBigInteger(p.RawYCoord.ToBigInteger()), new ECFieldElement[]
					{
						this.FromBigInteger(p.GetZCoord(0).ToBigInteger())
					}, p.IsCompressed);
				}
			}
			return base.ImportPoint(p);
		}

		// Token: 0x0400156A RID: 5482
		private const int FP_DEFAULT_COORDS = 4;

		// Token: 0x0400156B RID: 5483
		protected readonly BigInteger m_q;

		// Token: 0x0400156C RID: 5484
		protected readonly BigInteger m_r;

		// Token: 0x0400156D RID: 5485
		protected readonly FpPoint m_infinity;
	}
}
