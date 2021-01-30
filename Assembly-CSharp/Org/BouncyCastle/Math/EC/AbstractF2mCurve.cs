using System;
using Org.BouncyCastle.Math.EC.Abc;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002DE RID: 734
	public abstract class AbstractF2mCurve : ECCurve
	{
		// Token: 0x0600195A RID: 6490 RVA: 0x00093D22 File Offset: 0x00091F22
		public static BigInteger Inverse(int m, int[] ks, BigInteger x)
		{
			return new LongArray(x).ModInverse(m, ks).ToBigInteger();
		}

		// Token: 0x0600195B RID: 6491 RVA: 0x00093D38 File Offset: 0x00091F38
		private static IFiniteField BuildField(int m, int k1, int k2, int k3)
		{
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
				return FiniteFields.GetBinaryExtensionField(new int[]
				{
					0,
					k1,
					m
				});
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
				return FiniteFields.GetBinaryExtensionField(new int[]
				{
					0,
					k1,
					k2,
					k3,
					m
				});
			}
		}

		// Token: 0x0600195C RID: 6492 RVA: 0x00093DB1 File Offset: 0x00091FB1
		protected AbstractF2mCurve(int m, int k1, int k2, int k3) : base(AbstractF2mCurve.BuildField(m, k1, k2, k3))
		{
		}

		// Token: 0x0600195D RID: 6493 RVA: 0x00093DC3 File Offset: 0x00091FC3
		public override bool IsValidFieldElement(BigInteger x)
		{
			return x != null && x.SignValue >= 0 && x.BitLength <= this.FieldSize;
		}

		// Token: 0x0600195E RID: 6494 RVA: 0x00093DE4 File Offset: 0x00091FE4
		public override ECPoint CreatePoint(BigInteger x, BigInteger y, bool withCompression)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(x);
			ECFieldElement ecfieldElement2 = this.FromBigInteger(y);
			int coordinateSystem = this.CoordinateSystem;
			if (coordinateSystem - 5 <= 1)
			{
				if (ecfieldElement.IsZero)
				{
					if (!ecfieldElement2.Square().Equals(this.B))
					{
						throw new ArgumentException();
					}
				}
				else
				{
					ecfieldElement2 = ecfieldElement2.Divide(ecfieldElement).Add(ecfieldElement);
				}
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, withCompression);
		}

		// Token: 0x0600195F RID: 6495 RVA: 0x00093E48 File Offset: 0x00092048
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
				ECFieldElement ecfieldElement3 = this.SolveQuadradicEquation(beta);
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

		// Token: 0x06001960 RID: 6496 RVA: 0x00093EF4 File Offset: 0x000920F4
		private ECFieldElement SolveQuadradicEquation(ECFieldElement beta)
		{
			if (beta.IsZero)
			{
				return beta;
			}
			ECFieldElement ecfieldElement = this.FromBigInteger(BigInteger.Zero);
			int fieldSize = this.FieldSize;
			for (;;)
			{
				ECFieldElement b = this.FromBigInteger(BigInteger.Arbitrary(fieldSize));
				ECFieldElement ecfieldElement2 = ecfieldElement;
				ECFieldElement ecfieldElement3 = beta;
				for (int i = 1; i < fieldSize; i++)
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

		// Token: 0x06001961 RID: 6497 RVA: 0x00093F8C File Offset: 0x0009218C
		internal virtual BigInteger[] GetSi()
		{
			if (this.si == null)
			{
				lock (this)
				{
					if (this.si == null)
					{
						this.si = Tnaf.GetSi(this);
					}
				}
			}
			return this.si;
		}

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06001962 RID: 6498 RVA: 0x00093FE4 File Offset: 0x000921E4
		public virtual bool IsKoblitz
		{
			get
			{
				return this.m_order != null && this.m_cofactor != null && this.m_b.IsOne && (this.m_a.IsZero || this.m_a.IsOne);
			}
		}

		// Token: 0x0400156E RID: 5486
		private BigInteger[] si;
	}
}
