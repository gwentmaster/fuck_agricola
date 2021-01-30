using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200032B RID: 811
	internal class SecT113FieldElement : ECFieldElement
	{
		// Token: 0x06001D79 RID: 7545 RVA: 0x000A508C File Offset: 0x000A328C
		public SecT113FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 113)
			{
				throw new ArgumentException("value invalid for SecT113FieldElement", "x");
			}
			this.x = SecT113Field.FromBigInteger(x);
		}

		// Token: 0x06001D7A RID: 7546 RVA: 0x000A50C6 File Offset: 0x000A32C6
		public SecT113FieldElement()
		{
			this.x = Nat128.Create64();
		}

		// Token: 0x06001D7B RID: 7547 RVA: 0x000A50D9 File Offset: 0x000A32D9
		protected internal SecT113FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001D7C RID: 7548 RVA: 0x000A50E8 File Offset: 0x000A32E8
		public override bool IsOne
		{
			get
			{
				return Nat128.IsOne64(this.x);
			}
		}

		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001D7D RID: 7549 RVA: 0x000A50F5 File Offset: 0x000A32F5
		public override bool IsZero
		{
			get
			{
				return Nat128.IsZero64(this.x);
			}
		}

		// Token: 0x06001D7E RID: 7550 RVA: 0x000A5102 File Offset: 0x000A3302
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001D7F RID: 7551 RVA: 0x000A5113 File Offset: 0x000A3313
		public override BigInteger ToBigInteger()
		{
			return Nat128.ToBigInteger64(this.x);
		}

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001D80 RID: 7552 RVA: 0x000A5120 File Offset: 0x000A3320
		public override string FieldName
		{
			get
			{
				return "SecT113Field";
			}
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001D81 RID: 7553 RVA: 0x000A5127 File Offset: 0x000A3327
		public override int FieldSize
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x06001D82 RID: 7554 RVA: 0x000A512C File Offset: 0x000A332C
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Add(this.x, ((SecT113FieldElement)b).x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D83 RID: 7555 RVA: 0x000A515C File Offset: 0x000A335C
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.AddOne(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D84 RID: 7556 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001D85 RID: 7557 RVA: 0x000A5184 File Offset: 0x000A3384
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Multiply(this.x, ((SecT113FieldElement)b).x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D86 RID: 7558 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001D87 RID: 7559 RVA: 0x000A51B4 File Offset: 0x000A33B4
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT113FieldElement)b).x;
			ulong[] array2 = ((SecT113FieldElement)x).x;
			ulong[] y3 = ((SecT113FieldElement)y).x;
			ulong[] array3 = Nat128.CreateExt64();
			SecT113Field.MultiplyAddToExt(array, y2, array3);
			SecT113Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat128.Create64();
			SecT113Field.Reduce(array3, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D88 RID: 7560 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001D89 RID: 7561 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001D8A RID: 7562 RVA: 0x000A5224 File Offset: 0x000A3424
		public override ECFieldElement Square()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Square(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D8B RID: 7563 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001D8C RID: 7564 RVA: 0x000A524C File Offset: 0x000A344C
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT113FieldElement)x).x;
			ulong[] y2 = ((SecT113FieldElement)y).x;
			ulong[] array3 = Nat128.CreateExt64();
			SecT113Field.SquareAddToExt(array, array3);
			SecT113Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat128.Create64();
			SecT113Field.Reduce(array3, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D8D RID: 7565 RVA: 0x000A52A0 File Offset: 0x000A34A0
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat128.Create64();
			SecT113Field.SquareN(this.x, pow, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D8E RID: 7566 RVA: 0x000A52CC File Offset: 0x000A34CC
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Invert(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x06001D8F RID: 7567 RVA: 0x000A52F4 File Offset: 0x000A34F4
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat128.Create64();
			SecT113Field.Sqrt(this.x, z);
			return new SecT113FieldElement(z);
		}

		// Token: 0x170002A7 RID: 679
		// (get) Token: 0x06001D90 RID: 7568 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170002A8 RID: 680
		// (get) Token: 0x06001D91 RID: 7569 RVA: 0x000A5127 File Offset: 0x000A3327
		public virtual int M
		{
			get
			{
				return 113;
			}
		}

		// Token: 0x170002A9 RID: 681
		// (get) Token: 0x06001D92 RID: 7570 RVA: 0x000A531C File Offset: 0x000A351C
		public virtual int K1
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170002AA RID: 682
		// (get) Token: 0x06001D93 RID: 7571 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170002AB RID: 683
		// (get) Token: 0x06001D94 RID: 7572 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001D95 RID: 7573 RVA: 0x000A5320 File Offset: 0x000A3520
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT113FieldElement);
		}

		// Token: 0x06001D96 RID: 7574 RVA: 0x000A5320 File Offset: 0x000A3520
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT113FieldElement);
		}

		// Token: 0x06001D97 RID: 7575 RVA: 0x000A532E File Offset: 0x000A352E
		public virtual bool Equals(SecT113FieldElement other)
		{
			return this == other || (other != null && Nat128.Eq64(this.x, other.x));
		}

		// Token: 0x06001D98 RID: 7576 RVA: 0x000A534C File Offset: 0x000A354C
		public override int GetHashCode()
		{
			return 113009 ^ Arrays.GetHashCode(this.x, 0, 2);
		}

		// Token: 0x0400161D RID: 5661
		protected internal readonly ulong[] x;
	}
}
