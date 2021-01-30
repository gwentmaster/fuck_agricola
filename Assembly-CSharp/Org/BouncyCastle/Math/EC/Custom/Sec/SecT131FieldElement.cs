using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000331 RID: 817
	internal class SecT131FieldElement : ECFieldElement
	{
		// Token: 0x06001DDF RID: 7647 RVA: 0x000A6AB7 File Offset: 0x000A4CB7
		public SecT131FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 131)
			{
				throw new ArgumentException("value invalid for SecT131FieldElement", "x");
			}
			this.x = SecT131Field.FromBigInteger(x);
		}

		// Token: 0x06001DE0 RID: 7648 RVA: 0x000A6AF4 File Offset: 0x000A4CF4
		public SecT131FieldElement()
		{
			this.x = Nat192.Create64();
		}

		// Token: 0x06001DE1 RID: 7649 RVA: 0x000A6B07 File Offset: 0x000A4D07
		protected internal SecT131FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001DE2 RID: 7650 RVA: 0x000A6B16 File Offset: 0x000A4D16
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne64(this.x);
			}
		}

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001DE3 RID: 7651 RVA: 0x000A6B23 File Offset: 0x000A4D23
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero64(this.x);
			}
		}

		// Token: 0x06001DE4 RID: 7652 RVA: 0x000A6B30 File Offset: 0x000A4D30
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001DE5 RID: 7653 RVA: 0x000A6B41 File Offset: 0x000A4D41
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger64(this.x);
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001DE6 RID: 7654 RVA: 0x000A6B4E File Offset: 0x000A4D4E
		public override string FieldName
		{
			get
			{
				return "SecT131Field";
			}
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001DE7 RID: 7655 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public override int FieldSize
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x06001DE8 RID: 7656 RVA: 0x000A6B5C File Offset: 0x000A4D5C
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Add(this.x, ((SecT131FieldElement)b).x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DE9 RID: 7657 RVA: 0x000A6B8C File Offset: 0x000A4D8C
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.AddOne(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DEA RID: 7658 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001DEB RID: 7659 RVA: 0x000A6BB4 File Offset: 0x000A4DB4
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Multiply(this.x, ((SecT131FieldElement)b).x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DEC RID: 7660 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001DED RID: 7661 RVA: 0x000A6BE4 File Offset: 0x000A4DE4
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT131FieldElement)b).x;
			ulong[] array2 = ((SecT131FieldElement)x).x;
			ulong[] y3 = ((SecT131FieldElement)y).x;
			ulong[] array3 = Nat.Create64(5);
			SecT131Field.MultiplyAddToExt(array, y2, array3);
			SecT131Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat192.Create64();
			SecT131Field.Reduce(array3, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DEE RID: 7662 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001DEF RID: 7663 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001DF0 RID: 7664 RVA: 0x000A6C48 File Offset: 0x000A4E48
		public override ECFieldElement Square()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Square(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DF1 RID: 7665 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001DF2 RID: 7666 RVA: 0x000A6C70 File Offset: 0x000A4E70
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT131FieldElement)x).x;
			ulong[] y2 = ((SecT131FieldElement)y).x;
			ulong[] array3 = Nat.Create64(5);
			SecT131Field.SquareAddToExt(array, array3);
			SecT131Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat192.Create64();
			SecT131Field.Reduce(array3, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DF3 RID: 7667 RVA: 0x000A6CC4 File Offset: 0x000A4EC4
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat192.Create64();
			SecT131Field.SquareN(this.x, pow, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DF4 RID: 7668 RVA: 0x000A6CF0 File Offset: 0x000A4EF0
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Invert(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x06001DF5 RID: 7669 RVA: 0x000A6D18 File Offset: 0x000A4F18
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat192.Create64();
			SecT131Field.Sqrt(this.x, z);
			return new SecT131FieldElement(z);
		}

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x06001DF6 RID: 7670 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x06001DF7 RID: 7671 RVA: 0x000A6B55 File Offset: 0x000A4D55
		public virtual int M
		{
			get
			{
				return 131;
			}
		}

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x06001DF8 RID: 7672 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x06001DF9 RID: 7673 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K2
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002C8 RID: 712
		// (get) Token: 0x06001DFA RID: 7674 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int K3
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06001DFB RID: 7675 RVA: 0x000A6D43 File Offset: 0x000A4F43
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT131FieldElement);
		}

		// Token: 0x06001DFC RID: 7676 RVA: 0x000A6D43 File Offset: 0x000A4F43
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT131FieldElement);
		}

		// Token: 0x06001DFD RID: 7677 RVA: 0x000A6D51 File Offset: 0x000A4F51
		public virtual bool Equals(SecT131FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq64(this.x, other.x));
		}

		// Token: 0x06001DFE RID: 7678 RVA: 0x000A6D6F File Offset: 0x000A4F6F
		public override int GetHashCode()
		{
			return 131832 ^ Arrays.GetHashCode(this.x, 0, 3);
		}

		// Token: 0x04001625 RID: 5669
		protected readonly ulong[] x;
	}
}
