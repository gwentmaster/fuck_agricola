using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200032C RID: 812
	internal class SecT113R1Curve : AbstractF2mCurve
	{
		// Token: 0x06001D99 RID: 7577 RVA: 0x000A5364 File Offset: 0x000A3564
		public SecT113R1Curve() : base(113, 9, 0, 0)
		{
			this.m_infinity = new SecT113R1Point(this, null, null);
			this.m_a = this.FromBigInteger(new BigInteger(1, Hex.Decode("003088250CA6E7C7FE649CE85820F7")));
			this.m_b = this.FromBigInteger(new BigInteger(1, Hex.Decode("00E8BEE4D3E2260744188BE0E9C723")));
			this.m_order = new BigInteger(1, Hex.Decode("0100000000000000D9CCEC8A39E56F"));
			this.m_cofactor = BigInteger.Two;
			this.m_coord = 6;
		}

		// Token: 0x06001D9A RID: 7578 RVA: 0x000A53EB File Offset: 0x000A35EB
		protected override ECCurve CloneCurve()
		{
			return new SecT113R1Curve();
		}

		// Token: 0x06001D9B RID: 7579 RVA: 0x000A53F2 File Offset: 0x000A35F2
		public override bool SupportsCoordinateSystem(int coord)
		{
			return coord == 6;
		}

		// Token: 0x170002AC RID: 684
		// (get) Token: 0x06001D9C RID: 7580 RVA: 0x000A53FB File Offset: 0x000A35FB
		public override ECPoint Infinity
		{
			get
			{
				return this.m_infinity;
			}
		}

		// Token: 0x170002AD RID: 685
		// (get) Token: 0x06001D9D RID: 7581 RVA: 0x000A5127 File Offset: 0x000A3327
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x06001D9E RID: 7582 RVA: 0x000A5403 File Offset: 0x000A3603
		public override ECFieldElement FromBigInteger(BigInteger x)
		{
			return new SecT113FieldElement(x);
		}

		// Token: 0x06001D9F RID: 7583 RVA: 0x000A540B File Offset: 0x000A360B
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, bool withCompression)
		{
			return new SecT113R1Point(this, x, y, withCompression);
		}

		// Token: 0x06001DA0 RID: 7584 RVA: 0x000A5416 File Offset: 0x000A3616
		protected internal override ECPoint CreateRawPoint(ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			return new SecT113R1Point(this, x, y, zs, withCompression);
		}

		// Token: 0x170002AE RID: 686
		// (get) Token: 0x06001DA1 RID: 7585 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsKoblitz
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06001DA2 RID: 7586 RVA: 0x000A5424 File Offset: 0x000A3624
		protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(X1);
			ECFieldElement ecfieldElement2 = null;
			if (ecfieldElement.IsZero)
			{
				ecfieldElement2 = this.B.Sqrt();
			}
			else
			{
				ECFieldElement beta = ecfieldElement.Square().Invert().Multiply(this.B).Add(this.A).Add(ecfieldElement);
				ECFieldElement ecfieldElement3 = this.SolveQuadraticEquation(beta);
				if (ecfieldElement3 != null)
				{
					if (ecfieldElement3.TestBitZero() != (yTilde == 1))
					{
						ecfieldElement3 = ecfieldElement3.AddOne();
					}
					int coordinateSystem = this.CoordinateSystem;
					if (coordinateSystem - 5 <= 1)
					{
						ecfieldElement2 = ecfieldElement3.Add(ecfieldElement);
					}
					else
					{
						ecfieldElement2 = ecfieldElement3.Multiply(ecfieldElement);
					}
				}
			}
			if (ecfieldElement2 == null)
			{
				throw new ArgumentException("Invalid point compression");
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, true);
		}

		// Token: 0x06001DA3 RID: 7587 RVA: 0x000A54D0 File Offset: 0x000A36D0
		private ECFieldElement SolveQuadraticEquation(ECFieldElement beta)
		{
			if (beta.IsZero)
			{
				return beta;
			}
			ECFieldElement ecfieldElement = this.FromBigInteger(BigInteger.Zero);
			Random random = new Random();
			for (;;)
			{
				ECFieldElement b = this.FromBigInteger(new BigInteger(113, random));
				ECFieldElement ecfieldElement2 = ecfieldElement;
				ECFieldElement ecfieldElement3 = beta;
				for (int i = 1; i < 113; i++)
				{
					ECFieldElement ecfieldElement4 = ecfieldElement3.Square();
					ecfieldElement2 = ecfieldElement2.Square().Add(ecfieldElement4.Multiply(b));
					ecfieldElement3 = ecfieldElement4.Add(beta);
				}
				if (!ecfieldElement3.IsZero)
				{
					break;
				}
				ECFieldElement ecfieldElement5 = ecfieldElement2.Square().Add(ecfieldElement2);
				if (!ecfieldElement5.IsZero)
				{
					return ecfieldElement2;
				}
			}
			return null;
		}

		// Token: 0x170002AF RID: 687
		// (get) Token: 0x06001DA4 RID: 7588 RVA: 0x000A5127 File Offset: 0x000A3327
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001DA5 RID: 7589 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsTrinomial
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001DA6 RID: 7590 RVA: 0x000A531C File Offset: 0x000A351C
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001DA7 RID: 7591 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001DA8 RID: 7592 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x0400161E RID: 5662
		private const int SecT113R1_DEFAULT_COORDS = 6;

		// Token: 0x0400161F RID: 5663
		protected readonly SecT113R1Point m_infinity;
	}
}
