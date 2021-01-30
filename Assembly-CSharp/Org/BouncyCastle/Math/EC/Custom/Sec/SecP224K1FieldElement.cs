using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000314 RID: 788
	internal class SecP224K1FieldElement : ECFieldElement
	{
		// Token: 0x06001C17 RID: 7191 RVA: 0x0009F7D1 File Offset: 0x0009D9D1
		public SecP224K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP224K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP224K1FieldElement", "x");
			}
			this.x = SecP224K1Field.FromBigInteger(x);
		}

		// Token: 0x06001C18 RID: 7192 RVA: 0x0009F80F File Offset: 0x0009DA0F
		public SecP224K1FieldElement()
		{
			this.x = Nat224.Create();
		}

		// Token: 0x06001C19 RID: 7193 RVA: 0x0009F822 File Offset: 0x0009DA22
		protected internal SecP224K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700027C RID: 636
		// (get) Token: 0x06001C1A RID: 7194 RVA: 0x0009F831 File Offset: 0x0009DA31
		public override bool IsZero
		{
			get
			{
				return Nat224.IsZero(this.x);
			}
		}

		// Token: 0x1700027D RID: 637
		// (get) Token: 0x06001C1B RID: 7195 RVA: 0x0009F83E File Offset: 0x0009DA3E
		public override bool IsOne
		{
			get
			{
				return Nat224.IsOne(this.x);
			}
		}

		// Token: 0x06001C1C RID: 7196 RVA: 0x0009F84B File Offset: 0x0009DA4B
		public override bool TestBitZero()
		{
			return Nat224.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001C1D RID: 7197 RVA: 0x0009F85C File Offset: 0x0009DA5C
		public override BigInteger ToBigInteger()
		{
			return Nat224.ToBigInteger(this.x);
		}

		// Token: 0x1700027E RID: 638
		// (get) Token: 0x06001C1E RID: 7198 RVA: 0x0009F869 File Offset: 0x0009DA69
		public override string FieldName
		{
			get
			{
				return "SecP224K1Field";
			}
		}

		// Token: 0x1700027F RID: 639
		// (get) Token: 0x06001C1F RID: 7199 RVA: 0x0009F870 File Offset: 0x0009DA70
		public override int FieldSize
		{
			get
			{
				return SecP224K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001C20 RID: 7200 RVA: 0x0009F87C File Offset: 0x0009DA7C
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Add(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C21 RID: 7201 RVA: 0x0009F8AC File Offset: 0x0009DAAC
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.AddOne(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C22 RID: 7202 RVA: 0x0009F8D4 File Offset: 0x0009DAD4
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Subtract(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C23 RID: 7203 RVA: 0x0009F904 File Offset: 0x0009DB04
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Multiply(this.x, ((SecP224K1FieldElement)b).x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C24 RID: 7204 RVA: 0x0009F934 File Offset: 0x0009DB34
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224K1Field.P, ((SecP224K1FieldElement)b).x, z);
			SecP224K1Field.Multiply(z, this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C25 RID: 7205 RVA: 0x0009F970 File Offset: 0x0009DB70
		public override ECFieldElement Negate()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Negate(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C26 RID: 7206 RVA: 0x0009F998 File Offset: 0x0009DB98
		public override ECFieldElement Square()
		{
			uint[] z = Nat224.Create();
			SecP224K1Field.Square(this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C27 RID: 7207 RVA: 0x0009F9C0 File Offset: 0x0009DBC0
		public override ECFieldElement Invert()
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224K1Field.P, this.x, z);
			return new SecP224K1FieldElement(z);
		}

		// Token: 0x06001C28 RID: 7208 RVA: 0x0009F9EC File Offset: 0x0009DBEC
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat224.IsZero(y) || Nat224.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat224.Create();
			SecP224K1Field.Square(y, array);
			SecP224K1Field.Multiply(array, y, array);
			uint[] array2 = array;
			SecP224K1Field.Square(array, array2);
			SecP224K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat224.Create();
			SecP224K1Field.Square(array2, array3);
			SecP224K1Field.Multiply(array3, y, array3);
			uint[] array4 = Nat224.Create();
			SecP224K1Field.SquareN(array3, 4, array4);
			SecP224K1Field.Multiply(array4, array3, array4);
			uint[] array5 = Nat224.Create();
			SecP224K1Field.SquareN(array4, 3, array5);
			SecP224K1Field.Multiply(array5, array2, array5);
			uint[] array6 = array5;
			SecP224K1Field.SquareN(array5, 8, array6);
			SecP224K1Field.Multiply(array6, array4, array6);
			uint[] array7 = array4;
			SecP224K1Field.SquareN(array6, 4, array7);
			SecP224K1Field.Multiply(array7, array3, array7);
			uint[] array8 = array3;
			SecP224K1Field.SquareN(array7, 19, array8);
			SecP224K1Field.Multiply(array8, array6, array8);
			uint[] array9 = Nat224.Create();
			SecP224K1Field.SquareN(array8, 42, array9);
			SecP224K1Field.Multiply(array9, array8, array9);
			uint[] z = array8;
			SecP224K1Field.SquareN(array9, 23, z);
			SecP224K1Field.Multiply(z, array7, z);
			uint[] array10 = array7;
			SecP224K1Field.SquareN(z, 84, array10);
			SecP224K1Field.Multiply(array10, array9, array10);
			uint[] z2 = array10;
			SecP224K1Field.SquareN(z2, 20, z2);
			SecP224K1Field.Multiply(z2, array6, z2);
			SecP224K1Field.SquareN(z2, 3, z2);
			SecP224K1Field.Multiply(z2, y, z2);
			SecP224K1Field.SquareN(z2, 2, z2);
			SecP224K1Field.Multiply(z2, y, z2);
			SecP224K1Field.SquareN(z2, 4, z2);
			SecP224K1Field.Multiply(z2, array2, z2);
			SecP224K1Field.Square(z2, z2);
			uint[] array11 = array9;
			SecP224K1Field.Square(z2, array11);
			if (Nat224.Eq(y, array11))
			{
				return new SecP224K1FieldElement(z2);
			}
			SecP224K1Field.Multiply(z2, SecP224K1FieldElement.PRECOMP_POW2, z2);
			SecP224K1Field.Square(z2, array11);
			if (Nat224.Eq(y, array11))
			{
				return new SecP224K1FieldElement(z2);
			}
			return null;
		}

		// Token: 0x06001C29 RID: 7209 RVA: 0x0009FBC5 File Offset: 0x0009DDC5
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP224K1FieldElement);
		}

		// Token: 0x06001C2A RID: 7210 RVA: 0x0009FBC5 File Offset: 0x0009DDC5
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP224K1FieldElement);
		}

		// Token: 0x06001C2B RID: 7211 RVA: 0x0009FBD3 File Offset: 0x0009DDD3
		public virtual bool Equals(SecP224K1FieldElement other)
		{
			return this == other || (other != null && Nat224.Eq(this.x, other.x));
		}

		// Token: 0x06001C2C RID: 7212 RVA: 0x0009FBF1 File Offset: 0x0009DDF1
		public override int GetHashCode()
		{
			return SecP224K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x040015E9 RID: 5609
		public static readonly BigInteger Q = SecP224K1Curve.q;

		// Token: 0x040015EA RID: 5610
		private static readonly uint[] PRECOMP_POW2 = new uint[]
		{
			868209154U,
			3707425075U,
			579297866U,
			3280018344U,
			2824165628U,
			514782679U,
			2396984652U
		};

		// Token: 0x040015EB RID: 5611
		protected internal readonly uint[] x;
	}
}
