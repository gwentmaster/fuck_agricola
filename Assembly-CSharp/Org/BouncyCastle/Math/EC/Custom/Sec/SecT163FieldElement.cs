using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000337 RID: 823
	internal class SecT163FieldElement : ECFieldElement
	{
		// Token: 0x06001E43 RID: 7747 RVA: 0x000A8349 File Offset: 0x000A6549
		public SecT163FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 163)
			{
				throw new ArgumentException("value invalid for SecT163FieldElement", "x");
			}
			this.x = SecT163Field.FromBigInteger(x);
		}

		// Token: 0x06001E44 RID: 7748 RVA: 0x000A8386 File Offset: 0x000A6586
		public SecT163FieldElement()
		{
			this.x = Nat192.Create64();
		}

		// Token: 0x06001E45 RID: 7749 RVA: 0x000A8399 File Offset: 0x000A6599
		protected internal SecT163FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x170002DD RID: 733
		// (get) Token: 0x06001E46 RID: 7750 RVA: 0x000A83A8 File Offset: 0x000A65A8
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne64(this.x);
			}
		}

		// Token: 0x170002DE RID: 734
		// (get) Token: 0x06001E47 RID: 7751 RVA: 0x000A83B5 File Offset: 0x000A65B5
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero64(this.x);
			}
		}

		// Token: 0x06001E48 RID: 7752 RVA: 0x000A83C2 File Offset: 0x000A65C2
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001E49 RID: 7753 RVA: 0x000A83D3 File Offset: 0x000A65D3
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger64(this.x);
		}

		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06001E4A RID: 7754 RVA: 0x000A83E0 File Offset: 0x000A65E0
		public override string FieldName
		{
			get
			{
				return "SecT163Field";
			}
		}

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06001E4B RID: 7755 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public override int FieldSize
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x06001E4C RID: 7756 RVA: 0x000A83F0 File Offset: 0x000A65F0
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Add(this.x, ((SecT163FieldElement)b).x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E4D RID: 7757 RVA: 0x000A8420 File Offset: 0x000A6620
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.AddOne(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E4E RID: 7758 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001E4F RID: 7759 RVA: 0x000A8448 File Offset: 0x000A6648
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Multiply(this.x, ((SecT163FieldElement)b).x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E50 RID: 7760 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001E51 RID: 7761 RVA: 0x000A8478 File Offset: 0x000A6678
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT163FieldElement)b).x;
			ulong[] array2 = ((SecT163FieldElement)x).x;
			ulong[] y3 = ((SecT163FieldElement)y).x;
			ulong[] array3 = Nat192.CreateExt64();
			SecT163Field.MultiplyAddToExt(array, y2, array3);
			SecT163Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat192.Create64();
			SecT163Field.Reduce(array3, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E52 RID: 7762 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001E53 RID: 7763 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001E54 RID: 7764 RVA: 0x000A84DC File Offset: 0x000A66DC
		public override ECFieldElement Square()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Square(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E55 RID: 7765 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001E56 RID: 7766 RVA: 0x000A8504 File Offset: 0x000A6704
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT163FieldElement)x).x;
			ulong[] y2 = ((SecT163FieldElement)y).x;
			ulong[] array3 = Nat192.CreateExt64();
			SecT163Field.SquareAddToExt(array, array3);
			SecT163Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat192.Create64();
			SecT163Field.Reduce(array3, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E57 RID: 7767 RVA: 0x000A8558 File Offset: 0x000A6758
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat192.Create64();
			SecT163Field.SquareN(this.x, pow, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E58 RID: 7768 RVA: 0x000A8584 File Offset: 0x000A6784
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Invert(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x06001E59 RID: 7769 RVA: 0x000A85AC File Offset: 0x000A67AC
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat192.Create64();
			SecT163Field.Sqrt(this.x, z);
			return new SecT163FieldElement(z);
		}

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06001E5A RID: 7770 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x06001E5B RID: 7771 RVA: 0x000A83E7 File Offset: 0x000A65E7
		public virtual int M
		{
			get
			{
				return 163;
			}
		}

		// Token: 0x170002E3 RID: 739
		// (get) Token: 0x06001E5C RID: 7772 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int K1
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x170002E4 RID: 740
		// (get) Token: 0x06001E5D RID: 7773 RVA: 0x000A85D1 File Offset: 0x000A67D1
		public virtual int K2
		{
			get
			{
				return 6;
			}
		}

		// Token: 0x170002E5 RID: 741
		// (get) Token: 0x06001E5E RID: 7774 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K3
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06001E5F RID: 7775 RVA: 0x000A85D7 File Offset: 0x000A67D7
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT163FieldElement);
		}

		// Token: 0x06001E60 RID: 7776 RVA: 0x000A85D7 File Offset: 0x000A67D7
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT163FieldElement);
		}

		// Token: 0x06001E61 RID: 7777 RVA: 0x000A85E5 File Offset: 0x000A67E5
		public virtual bool Equals(SecT163FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq64(this.x, other.x));
		}

		// Token: 0x06001E62 RID: 7778 RVA: 0x000A8603 File Offset: 0x000A6803
		public override int GetHashCode()
		{
			return 163763 ^ Arrays.GetHashCode(this.x, 0, 3);
		}

		// Token: 0x0400162D RID: 5677
		protected readonly ulong[] x;
	}
}
