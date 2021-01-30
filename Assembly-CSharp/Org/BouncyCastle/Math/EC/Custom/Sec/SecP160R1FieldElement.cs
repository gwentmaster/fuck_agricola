using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000304 RID: 772
	internal class SecP160R1FieldElement : ECFieldElement
	{
		// Token: 0x06001B29 RID: 6953 RVA: 0x0009C154 File Offset: 0x0009A354
		public SecP160R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP160R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP160R1FieldElement", "x");
			}
			this.x = SecP160R1Field.FromBigInteger(x);
		}

		// Token: 0x06001B2A RID: 6954 RVA: 0x0009C192 File Offset: 0x0009A392
		public SecP160R1FieldElement()
		{
			this.x = Nat160.Create();
		}

		// Token: 0x06001B2B RID: 6955 RVA: 0x0009C1A5 File Offset: 0x0009A3A5
		protected internal SecP160R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000260 RID: 608
		// (get) Token: 0x06001B2C RID: 6956 RVA: 0x0009C1B4 File Offset: 0x0009A3B4
		public override bool IsZero
		{
			get
			{
				return Nat160.IsZero(this.x);
			}
		}

		// Token: 0x17000261 RID: 609
		// (get) Token: 0x06001B2D RID: 6957 RVA: 0x0009C1C1 File Offset: 0x0009A3C1
		public override bool IsOne
		{
			get
			{
				return Nat160.IsOne(this.x);
			}
		}

		// Token: 0x06001B2E RID: 6958 RVA: 0x0009C1CE File Offset: 0x0009A3CE
		public override bool TestBitZero()
		{
			return Nat160.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001B2F RID: 6959 RVA: 0x0009C1DF File Offset: 0x0009A3DF
		public override BigInteger ToBigInteger()
		{
			return Nat160.ToBigInteger(this.x);
		}

		// Token: 0x17000262 RID: 610
		// (get) Token: 0x06001B30 RID: 6960 RVA: 0x0009C1EC File Offset: 0x0009A3EC
		public override string FieldName
		{
			get
			{
				return "SecP160R1Field";
			}
		}

		// Token: 0x17000263 RID: 611
		// (get) Token: 0x06001B31 RID: 6961 RVA: 0x0009C1F3 File Offset: 0x0009A3F3
		public override int FieldSize
		{
			get
			{
				return SecP160R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001B32 RID: 6962 RVA: 0x0009C200 File Offset: 0x0009A400
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Add(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B33 RID: 6963 RVA: 0x0009C230 File Offset: 0x0009A430
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.AddOne(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B34 RID: 6964 RVA: 0x0009C258 File Offset: 0x0009A458
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Subtract(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B35 RID: 6965 RVA: 0x0009C288 File Offset: 0x0009A488
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Multiply(this.x, ((SecP160R1FieldElement)b).x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B36 RID: 6966 RVA: 0x0009C2B8 File Offset: 0x0009A4B8
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R1Field.P, ((SecP160R1FieldElement)b).x, z);
			SecP160R1Field.Multiply(z, this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B37 RID: 6967 RVA: 0x0009C2F4 File Offset: 0x0009A4F4
		public override ECFieldElement Negate()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Negate(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B38 RID: 6968 RVA: 0x0009C31C File Offset: 0x0009A51C
		public override ECFieldElement Square()
		{
			uint[] z = Nat160.Create();
			SecP160R1Field.Square(this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B39 RID: 6969 RVA: 0x0009C344 File Offset: 0x0009A544
		public override ECFieldElement Invert()
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R1Field.P, this.x, z);
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B3A RID: 6970 RVA: 0x0009C370 File Offset: 0x0009A570
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat160.IsZero(y) || Nat160.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat160.Create();
			SecP160R1Field.Square(y, array);
			SecP160R1Field.Multiply(array, y, array);
			uint[] array2 = Nat160.Create();
			SecP160R1Field.SquareN(array, 2, array2);
			SecP160R1Field.Multiply(array2, array, array2);
			uint[] array3 = array;
			SecP160R1Field.SquareN(array2, 4, array3);
			SecP160R1Field.Multiply(array3, array2, array3);
			uint[] array4 = array2;
			SecP160R1Field.SquareN(array3, 8, array4);
			SecP160R1Field.Multiply(array4, array3, array4);
			uint[] array5 = array3;
			SecP160R1Field.SquareN(array4, 16, array5);
			SecP160R1Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP160R1Field.SquareN(array5, 32, array6);
			SecP160R1Field.Multiply(array6, array5, array6);
			uint[] array7 = array5;
			SecP160R1Field.SquareN(array6, 64, array7);
			SecP160R1Field.Multiply(array7, array6, array7);
			uint[] array8 = array6;
			SecP160R1Field.Square(array7, array8);
			SecP160R1Field.Multiply(array8, y, array8);
			uint[] z = array8;
			SecP160R1Field.SquareN(z, 29, z);
			uint[] array9 = array7;
			SecP160R1Field.Square(z, array9);
			if (!Nat160.Eq(y, array9))
			{
				return null;
			}
			return new SecP160R1FieldElement(z);
		}

		// Token: 0x06001B3B RID: 6971 RVA: 0x0009C47C File Offset: 0x0009A67C
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP160R1FieldElement);
		}

		// Token: 0x06001B3C RID: 6972 RVA: 0x0009C47C File Offset: 0x0009A67C
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP160R1FieldElement);
		}

		// Token: 0x06001B3D RID: 6973 RVA: 0x0009C48A File Offset: 0x0009A68A
		public virtual bool Equals(SecP160R1FieldElement other)
		{
			return this == other || (other != null && Nat160.Eq(this.x, other.x));
		}

		// Token: 0x06001B3E RID: 6974 RVA: 0x0009C4A8 File Offset: 0x0009A6A8
		public override int GetHashCode()
		{
			return SecP160R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x040015BE RID: 5566
		public static readonly BigInteger Q = SecP160R1Curve.q;

		// Token: 0x040015BF RID: 5567
		protected internal readonly uint[] x;
	}
}
