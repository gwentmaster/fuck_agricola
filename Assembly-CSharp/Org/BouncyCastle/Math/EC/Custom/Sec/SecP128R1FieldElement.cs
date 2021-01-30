using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020002FE RID: 766
	internal class SecP128R1FieldElement : ECFieldElement
	{
		// Token: 0x06001ADB RID: 6875 RVA: 0x0009ADD3 File Offset: 0x00098FD3
		public SecP128R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP128R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP128R1FieldElement", "x");
			}
			this.x = SecP128R1Field.FromBigInteger(x);
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x0009AE11 File Offset: 0x00099011
		public SecP128R1FieldElement()
		{
			this.x = Nat128.Create();
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x0009AE24 File Offset: 0x00099024
		protected internal SecP128R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x06001ADE RID: 6878 RVA: 0x0009AE33 File Offset: 0x00099033
		public override bool IsZero
		{
			get
			{
				return Nat128.IsZero(this.x);
			}
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x06001ADF RID: 6879 RVA: 0x0009AE40 File Offset: 0x00099040
		public override bool IsOne
		{
			get
			{
				return Nat128.IsOne(this.x);
			}
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x0009AE4D File Offset: 0x0009904D
		public override bool TestBitZero()
		{
			return Nat128.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x0009AE5E File Offset: 0x0009905E
		public override BigInteger ToBigInteger()
		{
			return Nat128.ToBigInteger(this.x);
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001AE2 RID: 6882 RVA: 0x0009AE6B File Offset: 0x0009906B
		public override string FieldName
		{
			get
			{
				return "SecP128R1Field";
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001AE3 RID: 6883 RVA: 0x0009AE72 File Offset: 0x00099072
		public override int FieldSize
		{
			get
			{
				return SecP128R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x0009AE80 File Offset: 0x00099080
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Add(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x0009AEB0 File Offset: 0x000990B0
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.AddOne(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0009AED8 File Offset: 0x000990D8
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Subtract(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0009AF08 File Offset: 0x00099108
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Multiply(this.x, ((SecP128R1FieldElement)b).x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0009AF38 File Offset: 0x00099138
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat128.Create();
			Mod.Invert(SecP128R1Field.P, ((SecP128R1FieldElement)b).x, z);
			SecP128R1Field.Multiply(z, this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0009AF74 File Offset: 0x00099174
		public override ECFieldElement Negate()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Negate(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0009AF9C File Offset: 0x0009919C
		public override ECFieldElement Square()
		{
			uint[] z = Nat128.Create();
			SecP128R1Field.Square(this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0009AFC4 File Offset: 0x000991C4
		public override ECFieldElement Invert()
		{
			uint[] z = Nat128.Create();
			Mod.Invert(SecP128R1Field.P, this.x, z);
			return new SecP128R1FieldElement(z);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0009AFF0 File Offset: 0x000991F0
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat128.IsZero(y) || Nat128.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat128.Create();
			SecP128R1Field.Square(y, array);
			SecP128R1Field.Multiply(array, y, array);
			uint[] array2 = Nat128.Create();
			SecP128R1Field.SquareN(array, 2, array2);
			SecP128R1Field.Multiply(array2, array, array2);
			uint[] array3 = Nat128.Create();
			SecP128R1Field.SquareN(array2, 4, array3);
			SecP128R1Field.Multiply(array3, array2, array3);
			uint[] array4 = array2;
			SecP128R1Field.SquareN(array3, 2, array4);
			SecP128R1Field.Multiply(array4, array, array4);
			uint[] z = array;
			SecP128R1Field.SquareN(array4, 10, z);
			SecP128R1Field.Multiply(z, array4, z);
			uint[] array5 = array3;
			SecP128R1Field.SquareN(z, 10, array5);
			SecP128R1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP128R1Field.Square(array5, array6);
			SecP128R1Field.Multiply(array6, y, array6);
			uint[] z2 = array6;
			SecP128R1Field.SquareN(z2, 95, z2);
			uint[] array7 = array5;
			SecP128R1Field.Square(z2, array7);
			if (!Nat128.Eq(y, array7))
			{
				return null;
			}
			return new SecP128R1FieldElement(z2);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0009B0E5 File Offset: 0x000992E5
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP128R1FieldElement);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0009B0E5 File Offset: 0x000992E5
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP128R1FieldElement);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0009B0F3 File Offset: 0x000992F3
		public virtual bool Equals(SecP128R1FieldElement other)
		{
			return this == other || (other != null && Nat128.Eq(this.x, other.x));
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0009B111 File Offset: 0x00099311
		public override int GetHashCode()
		{
			return SecP128R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x040015B0 RID: 5552
		public static readonly BigInteger Q = SecP128R1Curve.q;

		// Token: 0x040015B1 RID: 5553
		protected internal readonly uint[] x;
	}
}
