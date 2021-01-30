using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E2 RID: 738
	public class F2mFieldElement : ECFieldElement
	{
		// Token: 0x060019B3 RID: 6579 RVA: 0x00094E84 File Offset: 0x00093084
		public F2mFieldElement(int m, int k1, int k2, int k3, BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > m)
			{
				throw new ArgumentException("value invalid in F2m field element", "x");
			}
			if (k2 == 0 && k3 == 0)
			{
				this.representation = 2;
				this.ks = new int[]
				{
					k1
				};
			}
			else
			{
				if (k2 >= k3)
				{
					throw new ArgumentException("k2 must be smaller than k3");
				}
				if (k2 <= 0)
				{
					throw new ArgumentException("k2 must be larger than 0");
				}
				this.representation = 3;
				this.ks = new int[]
				{
					k1,
					k2,
					k3
				};
			}
			this.m = m;
			this.x = new LongArray(x);
		}

		// Token: 0x060019B4 RID: 6580 RVA: 0x00094F32 File Offset: 0x00093132
		public F2mFieldElement(int m, int k, BigInteger x) : this(m, k, 0, 0, x)
		{
		}

		// Token: 0x060019B5 RID: 6581 RVA: 0x00094F3F File Offset: 0x0009313F
		private F2mFieldElement(int m, int[] ks, LongArray x)
		{
			this.m = m;
			this.representation = ((ks.Length == 1) ? 2 : 3);
			this.ks = ks;
			this.x = x;
		}

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x060019B6 RID: 6582 RVA: 0x00094F6C File Offset: 0x0009316C
		public override int BitLength
		{
			get
			{
				return this.x.Degree();
			}
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x060019B7 RID: 6583 RVA: 0x00094F79 File Offset: 0x00093179
		public override bool IsOne
		{
			get
			{
				return this.x.IsOne();
			}
		}

		// Token: 0x17000228 RID: 552
		// (get) Token: 0x060019B8 RID: 6584 RVA: 0x00094F86 File Offset: 0x00093186
		public override bool IsZero
		{
			get
			{
				return this.x.IsZero();
			}
		}

		// Token: 0x060019B9 RID: 6585 RVA: 0x00094F93 File Offset: 0x00093193
		public override bool TestBitZero()
		{
			return this.x.TestBitZero();
		}

		// Token: 0x060019BA RID: 6586 RVA: 0x00094FA0 File Offset: 0x000931A0
		public override BigInteger ToBigInteger()
		{
			return this.x.ToBigInteger();
		}

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x060019BB RID: 6587 RVA: 0x00094FAD File Offset: 0x000931AD
		public override string FieldName
		{
			get
			{
				return "F2m";
			}
		}

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x060019BC RID: 6588 RVA: 0x00094FB4 File Offset: 0x000931B4
		public override int FieldSize
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x060019BD RID: 6589 RVA: 0x00094FBC File Offset: 0x000931BC
		public static void CheckFieldElements(ECFieldElement a, ECFieldElement b)
		{
			if (!(a is F2mFieldElement) || !(b is F2mFieldElement))
			{
				throw new ArgumentException("Field elements are not both instances of F2mFieldElement");
			}
			F2mFieldElement f2mFieldElement = (F2mFieldElement)a;
			F2mFieldElement f2mFieldElement2 = (F2mFieldElement)b;
			if (f2mFieldElement.representation != f2mFieldElement2.representation)
			{
				throw new ArgumentException("One of the F2m field elements has incorrect representation");
			}
			if (f2mFieldElement.m != f2mFieldElement2.m || !Arrays.AreEqual(f2mFieldElement.ks, f2mFieldElement2.ks))
			{
				throw new ArgumentException("Field elements are not elements of the same field F2m");
			}
		}

		// Token: 0x060019BE RID: 6590 RVA: 0x00095038 File Offset: 0x00093238
		public override ECFieldElement Add(ECFieldElement b)
		{
			LongArray longArray = this.x.Copy();
			F2mFieldElement f2mFieldElement = (F2mFieldElement)b;
			longArray.AddShiftedByWords(f2mFieldElement.x, 0);
			return new F2mFieldElement(this.m, this.ks, longArray);
		}

		// Token: 0x060019BF RID: 6591 RVA: 0x00095077 File Offset: 0x00093277
		public override ECFieldElement AddOne()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.AddOne());
		}

		// Token: 0x060019C0 RID: 6592 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060019C1 RID: 6593 RVA: 0x0009509E File Offset: 0x0009329E
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModMultiply(((F2mFieldElement)b).x, this.m, this.ks));
		}

		// Token: 0x060019C2 RID: 6594 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060019C3 RID: 6595 RVA: 0x000950E0 File Offset: 0x000932E0
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			LongArray longArray = this.x;
			LongArray longArray2 = ((F2mFieldElement)b).x;
			LongArray longArray3 = ((F2mFieldElement)x).x;
			LongArray other = ((F2mFieldElement)y).x;
			LongArray longArray4 = longArray.Multiply(longArray2, this.m, this.ks);
			LongArray other2 = longArray3.Multiply(other, this.m, this.ks);
			if (longArray4 == longArray || longArray4 == longArray2)
			{
				longArray4 = longArray4.Copy();
			}
			longArray4.AddShiftedByWords(other2, 0);
			longArray4.Reduce(this.m, this.ks);
			return new F2mFieldElement(this.m, this.ks, longArray4);
		}

		// Token: 0x060019C4 RID: 6596 RVA: 0x0009517C File Offset: 0x0009337C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			ECFieldElement b2 = b.Invert();
			return this.Multiply(b2);
		}

		// Token: 0x060019C5 RID: 6597 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060019C6 RID: 6598 RVA: 0x00095197 File Offset: 0x00093397
		public override ECFieldElement Square()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModSquare(this.m, this.ks));
		}

		// Token: 0x060019C7 RID: 6599 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060019C8 RID: 6600 RVA: 0x000951CC File Offset: 0x000933CC
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			LongArray longArray = this.x;
			LongArray longArray2 = ((F2mFieldElement)x).x;
			LongArray other = ((F2mFieldElement)y).x;
			LongArray longArray3 = longArray.Square(this.m, this.ks);
			LongArray other2 = longArray2.Multiply(other, this.m, this.ks);
			if (longArray3 == longArray)
			{
				longArray3 = longArray3.Copy();
			}
			longArray3.AddShiftedByWords(other2, 0);
			longArray3.Reduce(this.m, this.ks);
			return new F2mFieldElement(this.m, this.ks, longArray3);
		}

		// Token: 0x060019C9 RID: 6601 RVA: 0x00095254 File Offset: 0x00093454
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow >= 1)
			{
				return new F2mFieldElement(this.m, this.ks, this.x.ModSquareN(pow, this.m, this.ks));
			}
			return this;
		}

		// Token: 0x060019CA RID: 6602 RVA: 0x00095285 File Offset: 0x00093485
		public override ECFieldElement Invert()
		{
			return new F2mFieldElement(this.m, this.ks, this.x.ModInverse(this.m, this.ks));
		}

		// Token: 0x060019CB RID: 6603 RVA: 0x000952AF File Offset: 0x000934AF
		public override ECFieldElement Sqrt()
		{
			if (!this.x.IsZero() && !this.x.IsOne())
			{
				return this.SquarePow(this.m - 1);
			}
			return this;
		}

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x060019CC RID: 6604 RVA: 0x000952DB File Offset: 0x000934DB
		public int Representation
		{
			get
			{
				return this.representation;
			}
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x060019CD RID: 6605 RVA: 0x00094FB4 File Offset: 0x000931B4
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x060019CE RID: 6606 RVA: 0x000952E3 File Offset: 0x000934E3
		public int K1
		{
			get
			{
				return this.ks[0];
			}
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x060019CF RID: 6607 RVA: 0x000952ED File Offset: 0x000934ED
		public int K2
		{
			get
			{
				if (this.ks.Length < 2)
				{
					return 0;
				}
				return this.ks[1];
			}
		}

		// Token: 0x1700022F RID: 559
		// (get) Token: 0x060019D0 RID: 6608 RVA: 0x00095304 File Offset: 0x00093504
		public int K3
		{
			get
			{
				if (this.ks.Length < 3)
				{
					return 0;
				}
				return this.ks[2];
			}
		}

		// Token: 0x060019D1 RID: 6609 RVA: 0x0009531C File Offset: 0x0009351C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			F2mFieldElement f2mFieldElement = obj as F2mFieldElement;
			return f2mFieldElement != null && this.Equals(f2mFieldElement);
		}

		// Token: 0x060019D2 RID: 6610 RVA: 0x00095344 File Offset: 0x00093544
		public virtual bool Equals(F2mFieldElement other)
		{
			return this.m == other.m && this.representation == other.representation && Arrays.AreEqual(this.ks, other.ks) && this.x.Equals(other.x);
		}

		// Token: 0x060019D3 RID: 6611 RVA: 0x00095393 File Offset: 0x00093593
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ this.m ^ Arrays.GetHashCode(this.ks);
		}

		// Token: 0x04001578 RID: 5496
		public const int Gnb = 1;

		// Token: 0x04001579 RID: 5497
		public const int Tpb = 2;

		// Token: 0x0400157A RID: 5498
		public const int Ppb = 3;

		// Token: 0x0400157B RID: 5499
		private int representation;

		// Token: 0x0400157C RID: 5500
		private int m;

		// Token: 0x0400157D RID: 5501
		private int[] ks;

		// Token: 0x0400157E RID: 5502
		private LongArray x;
	}
}
