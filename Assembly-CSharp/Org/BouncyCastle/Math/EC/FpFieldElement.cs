using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E1 RID: 737
	public class FpFieldElement : ECFieldElement
	{
		// Token: 0x06001992 RID: 6546 RVA: 0x00094378 File Offset: 0x00092578
		internal static BigInteger CalculateResidue(BigInteger p)
		{
			int bitLength = p.BitLength;
			if (bitLength >= 96)
			{
				if (p.ShiftRight(bitLength - 64).LongValue == -1L)
				{
					return BigInteger.One.ShiftLeft(bitLength).Subtract(p);
				}
				if ((bitLength & 7) == 0)
				{
					return BigInteger.One.ShiftLeft(bitLength << 1).Divide(p).Negate();
				}
			}
			return null;
		}

		// Token: 0x06001993 RID: 6547 RVA: 0x000943D5 File Offset: 0x000925D5
		public FpFieldElement(BigInteger q, BigInteger x) : this(q, FpFieldElement.CalculateResidue(q), x)
		{
		}

		// Token: 0x06001994 RID: 6548 RVA: 0x000943E8 File Offset: 0x000925E8
		internal FpFieldElement(BigInteger q, BigInteger r, BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(q) >= 0)
			{
				throw new ArgumentException("value invalid in Fp field element", "x");
			}
			this.q = q;
			this.r = r;
			this.x = x;
		}

		// Token: 0x06001995 RID: 6549 RVA: 0x00094436 File Offset: 0x00092636
		public override BigInteger ToBigInteger()
		{
			return this.x;
		}

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06001996 RID: 6550 RVA: 0x0009443E File Offset: 0x0009263E
		public override string FieldName
		{
			get
			{
				return "Fp";
			}
		}

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06001997 RID: 6551 RVA: 0x00094445 File Offset: 0x00092645
		public override int FieldSize
		{
			get
			{
				return this.q.BitLength;
			}
		}

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06001998 RID: 6552 RVA: 0x00094452 File Offset: 0x00092652
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x06001999 RID: 6553 RVA: 0x0009445A File Offset: 0x0009265A
		public override ECFieldElement Add(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModAdd(this.x, b.ToBigInteger()));
		}

		// Token: 0x0600199A RID: 6554 RVA: 0x00094480 File Offset: 0x00092680
		public override ECFieldElement AddOne()
		{
			BigInteger bigInteger = this.x.Add(BigInteger.One);
			if (bigInteger.CompareTo(this.q) == 0)
			{
				bigInteger = BigInteger.Zero;
			}
			return new FpFieldElement(this.q, this.r, bigInteger);
		}

		// Token: 0x0600199B RID: 6555 RVA: 0x000944C4 File Offset: 0x000926C4
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModSubtract(this.x, b.ToBigInteger()));
		}

		// Token: 0x0600199C RID: 6556 RVA: 0x000944E9 File Offset: 0x000926E9
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, b.ToBigInteger()));
		}

		// Token: 0x0600199D RID: 6557 RVA: 0x00094510 File Offset: 0x00092710
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger val = b.ToBigInteger();
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val2 = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(val);
			BigInteger n = bigInteger2.Multiply(val2);
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger3.Subtract(n)));
		}

		// Token: 0x0600199E RID: 6558 RVA: 0x00094568 File Offset: 0x00092768
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger val = b.ToBigInteger();
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val2 = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(val);
			BigInteger value = bigInteger2.Multiply(val2);
			BigInteger bigInteger4 = bigInteger3.Add(value);
			if (this.r != null && this.r.SignValue < 0 && bigInteger4.BitLength > this.q.BitLength << 1)
			{
				bigInteger4 = bigInteger4.Subtract(this.q.ShiftLeft(this.q.BitLength));
			}
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger4));
		}

		// Token: 0x0600199F RID: 6559 RVA: 0x0009460A File Offset: 0x0009280A
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.ModInverse(b.ToBigInteger())));
		}

		// Token: 0x060019A0 RID: 6560 RVA: 0x00094635 File Offset: 0x00092835
		public override ECFieldElement Negate()
		{
			if (this.x.SignValue != 0)
			{
				return new FpFieldElement(this.q, this.r, this.q.Subtract(this.x));
			}
			return this;
		}

		// Token: 0x060019A1 RID: 6561 RVA: 0x00094668 File Offset: 0x00092868
		public override ECFieldElement Square()
		{
			return new FpFieldElement(this.q, this.r, this.ModMult(this.x, this.x));
		}

		// Token: 0x060019A2 RID: 6562 RVA: 0x00094690 File Offset: 0x00092890
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger);
			BigInteger n = bigInteger2.Multiply(val);
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger3.Subtract(n)));
		}

		// Token: 0x060019A3 RID: 6563 RVA: 0x000946E0 File Offset: 0x000928E0
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			BigInteger bigInteger = this.x;
			BigInteger bigInteger2 = x.ToBigInteger();
			BigInteger val = y.ToBigInteger();
			BigInteger bigInteger3 = bigInteger.Multiply(bigInteger);
			BigInteger value = bigInteger2.Multiply(val);
			BigInteger bigInteger4 = bigInteger3.Add(value);
			if (this.r != null && this.r.SignValue < 0 && bigInteger4.BitLength > this.q.BitLength << 1)
			{
				bigInteger4 = bigInteger4.Subtract(this.q.ShiftLeft(this.q.BitLength));
			}
			return new FpFieldElement(this.q, this.r, this.ModReduce(bigInteger4));
		}

		// Token: 0x060019A4 RID: 6564 RVA: 0x00094776 File Offset: 0x00092976
		public override ECFieldElement Invert()
		{
			return new FpFieldElement(this.q, this.r, this.ModInverse(this.x));
		}

		// Token: 0x060019A5 RID: 6565 RVA: 0x00094798 File Offset: 0x00092998
		public override ECFieldElement Sqrt()
		{
			if (this.IsZero || this.IsOne)
			{
				return this;
			}
			if (!this.q.TestBit(0))
			{
				throw Platform.CreateNotImplementedException("even value of q");
			}
			if (this.q.TestBit(1))
			{
				BigInteger e = this.q.ShiftRight(2).Add(BigInteger.One);
				return this.CheckSqrt(new FpFieldElement(this.q, this.r, this.x.ModPow(e, this.q)));
			}
			if (this.q.TestBit(2))
			{
				BigInteger bigInteger = this.x.ModPow(this.q.ShiftRight(3), this.q);
				BigInteger x = this.ModMult(bigInteger, this.x);
				if (this.ModMult(x, bigInteger).Equals(BigInteger.One))
				{
					return this.CheckSqrt(new FpFieldElement(this.q, this.r, x));
				}
				BigInteger x2 = BigInteger.Two.ModPow(this.q.ShiftRight(2), this.q);
				BigInteger bigInteger2 = this.ModMult(x, x2);
				return this.CheckSqrt(new FpFieldElement(this.q, this.r, bigInteger2));
			}
			else
			{
				BigInteger bigInteger3 = this.q.ShiftRight(1);
				if (!this.x.ModPow(bigInteger3, this.q).Equals(BigInteger.One))
				{
					return null;
				}
				BigInteger bigInteger4 = this.x;
				BigInteger bigInteger5 = this.ModDouble(this.ModDouble(bigInteger4));
				BigInteger k = bigInteger3.Add(BigInteger.One);
				BigInteger obj = this.q.Subtract(BigInteger.One);
				BigInteger bigInteger8;
				for (;;)
				{
					BigInteger bigInteger6 = BigInteger.Arbitrary(this.q.BitLength);
					if (bigInteger6.CompareTo(this.q) < 0 && this.ModReduce(bigInteger6.Multiply(bigInteger6).Subtract(bigInteger5)).ModPow(bigInteger3, this.q).Equals(obj))
					{
						BigInteger[] array = this.LucasSequence(bigInteger6, bigInteger4, k);
						BigInteger bigInteger7 = array[0];
						bigInteger8 = array[1];
						if (this.ModMult(bigInteger8, bigInteger8).Equals(bigInteger5))
						{
							break;
						}
						if (!bigInteger7.Equals(BigInteger.One) && !bigInteger7.Equals(obj))
						{
							goto Block_11;
						}
					}
				}
				return new FpFieldElement(this.q, this.r, this.ModHalfAbs(bigInteger8));
				Block_11:
				return null;
			}
		}

		// Token: 0x060019A6 RID: 6566 RVA: 0x000949E0 File Offset: 0x00092BE0
		private ECFieldElement CheckSqrt(ECFieldElement z)
		{
			if (!z.Square().Equals(this))
			{
				return null;
			}
			return z;
		}

		// Token: 0x060019A7 RID: 6567 RVA: 0x000949F4 File Offset: 0x00092BF4
		private BigInteger[] LucasSequence(BigInteger P, BigInteger Q, BigInteger k)
		{
			int bitLength = k.BitLength;
			int lowestSetBit = k.GetLowestSetBit();
			BigInteger bigInteger = BigInteger.One;
			BigInteger bigInteger2 = BigInteger.Two;
			BigInteger bigInteger3 = P;
			BigInteger bigInteger4 = BigInteger.One;
			BigInteger bigInteger5 = BigInteger.One;
			for (int i = bitLength - 1; i >= lowestSetBit + 1; i--)
			{
				bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
				if (k.TestBit(i))
				{
					bigInteger5 = this.ModMult(bigInteger4, Q);
					bigInteger = this.ModMult(bigInteger, bigInteger3);
					bigInteger2 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
					bigInteger3 = this.ModReduce(bigInteger3.Multiply(bigInteger3).Subtract(bigInteger5.ShiftLeft(1)));
				}
				else
				{
					bigInteger5 = bigInteger4;
					bigInteger = this.ModReduce(bigInteger.Multiply(bigInteger2).Subtract(bigInteger4));
					bigInteger3 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
					bigInteger2 = this.ModReduce(bigInteger2.Multiply(bigInteger2).Subtract(bigInteger4.ShiftLeft(1)));
				}
			}
			bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
			bigInteger5 = this.ModMult(bigInteger4, Q);
			bigInteger = this.ModReduce(bigInteger.Multiply(bigInteger2).Subtract(bigInteger4));
			bigInteger2 = this.ModReduce(bigInteger3.Multiply(bigInteger2).Subtract(P.Multiply(bigInteger4)));
			bigInteger4 = this.ModMult(bigInteger4, bigInteger5);
			for (int j = 1; j <= lowestSetBit; j++)
			{
				bigInteger = this.ModMult(bigInteger, bigInteger2);
				bigInteger2 = this.ModReduce(bigInteger2.Multiply(bigInteger2).Subtract(bigInteger4.ShiftLeft(1)));
				bigInteger4 = this.ModMult(bigInteger4, bigInteger4);
			}
			return new BigInteger[]
			{
				bigInteger,
				bigInteger2
			};
		}

		// Token: 0x060019A8 RID: 6568 RVA: 0x00094B98 File Offset: 0x00092D98
		protected virtual BigInteger ModAdd(BigInteger x1, BigInteger x2)
		{
			BigInteger bigInteger = x1.Add(x2);
			if (bigInteger.CompareTo(this.q) >= 0)
			{
				bigInteger = bigInteger.Subtract(this.q);
			}
			return bigInteger;
		}

		// Token: 0x060019A9 RID: 6569 RVA: 0x00094BCC File Offset: 0x00092DCC
		protected virtual BigInteger ModDouble(BigInteger x)
		{
			BigInteger bigInteger = x.ShiftLeft(1);
			if (bigInteger.CompareTo(this.q) >= 0)
			{
				bigInteger = bigInteger.Subtract(this.q);
			}
			return bigInteger;
		}

		// Token: 0x060019AA RID: 6570 RVA: 0x00094BFE File Offset: 0x00092DFE
		protected virtual BigInteger ModHalf(BigInteger x)
		{
			if (x.TestBit(0))
			{
				x = this.q.Add(x);
			}
			return x.ShiftRight(1);
		}

		// Token: 0x060019AB RID: 6571 RVA: 0x00094C1E File Offset: 0x00092E1E
		protected virtual BigInteger ModHalfAbs(BigInteger x)
		{
			if (x.TestBit(0))
			{
				x = this.q.Subtract(x);
			}
			return x.ShiftRight(1);
		}

		// Token: 0x060019AC RID: 6572 RVA: 0x00094C40 File Offset: 0x00092E40
		protected virtual BigInteger ModInverse(BigInteger x)
		{
			int fieldSize = this.FieldSize;
			int len = fieldSize + 31 >> 5;
			uint[] p = Nat.FromBigInteger(fieldSize, this.q);
			uint[] array = Nat.FromBigInteger(fieldSize, x);
			uint[] z = Nat.Create(len);
			Mod.Invert(p, array, z);
			return Nat.ToBigInteger(len, z);
		}

		// Token: 0x060019AD RID: 6573 RVA: 0x00094C84 File Offset: 0x00092E84
		protected virtual BigInteger ModMult(BigInteger x1, BigInteger x2)
		{
			return this.ModReduce(x1.Multiply(x2));
		}

		// Token: 0x060019AE RID: 6574 RVA: 0x00094C94 File Offset: 0x00092E94
		protected virtual BigInteger ModReduce(BigInteger x)
		{
			if (this.r == null)
			{
				x = x.Mod(this.q);
			}
			else
			{
				bool flag = x.SignValue < 0;
				if (flag)
				{
					x = x.Abs();
				}
				int bitLength = this.q.BitLength;
				if (this.r.SignValue > 0)
				{
					BigInteger n = BigInteger.One.ShiftLeft(bitLength);
					bool flag2 = this.r.Equals(BigInteger.One);
					while (x.BitLength > bitLength + 1)
					{
						BigInteger bigInteger = x.ShiftRight(bitLength);
						BigInteger value = x.Remainder(n);
						if (!flag2)
						{
							bigInteger = bigInteger.Multiply(this.r);
						}
						x = bigInteger.Add(value);
					}
				}
				else
				{
					int num = (bitLength - 1 & 31) + 1;
					BigInteger bigInteger2 = this.r.Negate().Multiply(x.ShiftRight(bitLength - num)).ShiftRight(bitLength + num).Multiply(this.q);
					BigInteger bigInteger3 = BigInteger.One.ShiftLeft(bitLength + num);
					bigInteger2 = bigInteger2.Remainder(bigInteger3);
					x = x.Remainder(bigInteger3);
					x = x.Subtract(bigInteger2);
					if (x.SignValue < 0)
					{
						x = x.Add(bigInteger3);
					}
				}
				while (x.CompareTo(this.q) >= 0)
				{
					x = x.Subtract(this.q);
				}
				if (flag && x.SignValue != 0)
				{
					x = this.q.Subtract(x);
				}
			}
			return x;
		}

		// Token: 0x060019AF RID: 6575 RVA: 0x00094E00 File Offset: 0x00093000
		protected virtual BigInteger ModSubtract(BigInteger x1, BigInteger x2)
		{
			BigInteger bigInteger = x1.Subtract(x2);
			if (bigInteger.SignValue < 0)
			{
				bigInteger = bigInteger.Add(this.q);
			}
			return bigInteger;
		}

		// Token: 0x060019B0 RID: 6576 RVA: 0x00094E2C File Offset: 0x0009302C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			FpFieldElement fpFieldElement = obj as FpFieldElement;
			return fpFieldElement != null && this.Equals(fpFieldElement);
		}

		// Token: 0x060019B1 RID: 6577 RVA: 0x00094E52 File Offset: 0x00093052
		public virtual bool Equals(FpFieldElement other)
		{
			return this.q.Equals(other.q) && base.Equals(other);
		}

		// Token: 0x060019B2 RID: 6578 RVA: 0x00094E70 File Offset: 0x00093070
		public override int GetHashCode()
		{
			return this.q.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001575 RID: 5493
		private readonly BigInteger q;

		// Token: 0x04001576 RID: 5494
		private readonly BigInteger r;

		// Token: 0x04001577 RID: 5495
		private readonly BigInteger x;
	}
}
