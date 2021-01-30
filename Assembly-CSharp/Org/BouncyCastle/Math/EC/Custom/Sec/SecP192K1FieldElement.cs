using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200030C RID: 780
	internal class SecP192K1FieldElement : ECFieldElement
	{
		// Token: 0x06001B9F RID: 7071 RVA: 0x0009DBF4 File Offset: 0x0009BDF4
		public SecP192K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP192K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP192K1FieldElement", "x");
			}
			this.x = SecP192K1Field.FromBigInteger(x);
		}

		// Token: 0x06001BA0 RID: 7072 RVA: 0x0009DC32 File Offset: 0x0009BE32
		public SecP192K1FieldElement()
		{
			this.x = Nat192.Create();
		}

		// Token: 0x06001BA1 RID: 7073 RVA: 0x0009DC45 File Offset: 0x0009BE45
		protected internal SecP192K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700026E RID: 622
		// (get) Token: 0x06001BA2 RID: 7074 RVA: 0x0009DC54 File Offset: 0x0009BE54
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero(this.x);
			}
		}

		// Token: 0x1700026F RID: 623
		// (get) Token: 0x06001BA3 RID: 7075 RVA: 0x0009DC61 File Offset: 0x0009BE61
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne(this.x);
			}
		}

		// Token: 0x06001BA4 RID: 7076 RVA: 0x0009DC6E File Offset: 0x0009BE6E
		public override bool TestBitZero()
		{
			return Nat192.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001BA5 RID: 7077 RVA: 0x0009DC7F File Offset: 0x0009BE7F
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger(this.x);
		}

		// Token: 0x17000270 RID: 624
		// (get) Token: 0x06001BA6 RID: 7078 RVA: 0x0009DC8C File Offset: 0x0009BE8C
		public override string FieldName
		{
			get
			{
				return "SecP192K1Field";
			}
		}

		// Token: 0x17000271 RID: 625
		// (get) Token: 0x06001BA7 RID: 7079 RVA: 0x0009DC93 File Offset: 0x0009BE93
		public override int FieldSize
		{
			get
			{
				return SecP192K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001BA8 RID: 7080 RVA: 0x0009DCA0 File Offset: 0x0009BEA0
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Add(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BA9 RID: 7081 RVA: 0x0009DCD0 File Offset: 0x0009BED0
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.AddOne(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAA RID: 7082 RVA: 0x0009DCF8 File Offset: 0x0009BEF8
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Subtract(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAB RID: 7083 RVA: 0x0009DD28 File Offset: 0x0009BF28
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Multiply(this.x, ((SecP192K1FieldElement)b).x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x0009DD58 File Offset: 0x0009BF58
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192K1Field.P, ((SecP192K1FieldElement)b).x, z);
			SecP192K1Field.Multiply(z, this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAD RID: 7085 RVA: 0x0009DD94 File Offset: 0x0009BF94
		public override ECFieldElement Negate()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Negate(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAE RID: 7086 RVA: 0x0009DDBC File Offset: 0x0009BFBC
		public override ECFieldElement Square()
		{
			uint[] z = Nat192.Create();
			SecP192K1Field.Square(this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BAF RID: 7087 RVA: 0x0009DDE4 File Offset: 0x0009BFE4
		public override ECFieldElement Invert()
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192K1Field.P, this.x, z);
			return new SecP192K1FieldElement(z);
		}

		// Token: 0x06001BB0 RID: 7088 RVA: 0x0009DE10 File Offset: 0x0009C010
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat192.IsZero(y) || Nat192.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat192.Create();
			SecP192K1Field.Square(y, array);
			SecP192K1Field.Multiply(array, y, array);
			uint[] array2 = Nat192.Create();
			SecP192K1Field.Square(array, array2);
			SecP192K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat192.Create();
			SecP192K1Field.SquareN(array2, 3, array3);
			SecP192K1Field.Multiply(array3, array2, array3);
			uint[] array4 = array3;
			SecP192K1Field.SquareN(array3, 2, array4);
			SecP192K1Field.Multiply(array4, array, array4);
			uint[] array5 = array;
			SecP192K1Field.SquareN(array4, 8, array5);
			SecP192K1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP192K1Field.SquareN(array5, 3, array6);
			SecP192K1Field.Multiply(array6, array2, array6);
			uint[] array7 = Nat192.Create();
			SecP192K1Field.SquareN(array6, 16, array7);
			SecP192K1Field.Multiply(array7, array5, array7);
			uint[] array8 = array5;
			SecP192K1Field.SquareN(array7, 35, array8);
			SecP192K1Field.Multiply(array8, array7, array8);
			uint[] z = array7;
			SecP192K1Field.SquareN(array8, 70, z);
			SecP192K1Field.Multiply(z, array8, z);
			uint[] array9 = array8;
			SecP192K1Field.SquareN(z, 19, array9);
			SecP192K1Field.Multiply(array9, array6, array9);
			uint[] z2 = array9;
			SecP192K1Field.SquareN(z2, 20, z2);
			SecP192K1Field.Multiply(z2, array6, z2);
			SecP192K1Field.SquareN(z2, 4, z2);
			SecP192K1Field.Multiply(z2, array2, z2);
			SecP192K1Field.SquareN(z2, 6, z2);
			SecP192K1Field.Multiply(z2, array2, z2);
			SecP192K1Field.Square(z2, z2);
			uint[] array10 = array2;
			SecP192K1Field.Square(z2, array10);
			if (!Nat192.Eq(y, array10))
			{
				return null;
			}
			return new SecP192K1FieldElement(z2);
		}

		// Token: 0x06001BB1 RID: 7089 RVA: 0x0009DF91 File Offset: 0x0009C191
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP192K1FieldElement);
		}

		// Token: 0x06001BB2 RID: 7090 RVA: 0x0009DF91 File Offset: 0x0009C191
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP192K1FieldElement);
		}

		// Token: 0x06001BB3 RID: 7091 RVA: 0x0009DF9F File Offset: 0x0009C19F
		public virtual bool Equals(SecP192K1FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq(this.x, other.x));
		}

		// Token: 0x06001BB4 RID: 7092 RVA: 0x0009DFBD File Offset: 0x0009C1BD
		public override int GetHashCode()
		{
			return SecP192K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6);
		}

		// Token: 0x040015D4 RID: 5588
		public static readonly BigInteger Q = SecP192K1Curve.q;

		// Token: 0x040015D5 RID: 5589
		protected internal readonly uint[] x;
	}
}
