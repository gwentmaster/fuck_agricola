using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000308 RID: 776
	internal class SecP160R2FieldElement : ECFieldElement
	{
		// Token: 0x06001B64 RID: 7012 RVA: 0x0009CE60 File Offset: 0x0009B060
		public SecP160R2FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP160R2FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP160R2FieldElement", "x");
			}
			this.x = SecP160R2Field.FromBigInteger(x);
		}

		// Token: 0x06001B65 RID: 7013 RVA: 0x0009CE9E File Offset: 0x0009B09E
		public SecP160R2FieldElement()
		{
			this.x = Nat160.Create();
		}

		// Token: 0x06001B66 RID: 7014 RVA: 0x0009CEB1 File Offset: 0x0009B0B1
		protected internal SecP160R2FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000267 RID: 615
		// (get) Token: 0x06001B67 RID: 7015 RVA: 0x0009CEC0 File Offset: 0x0009B0C0
		public override bool IsZero
		{
			get
			{
				return Nat160.IsZero(this.x);
			}
		}

		// Token: 0x17000268 RID: 616
		// (get) Token: 0x06001B68 RID: 7016 RVA: 0x0009CECD File Offset: 0x0009B0CD
		public override bool IsOne
		{
			get
			{
				return Nat160.IsOne(this.x);
			}
		}

		// Token: 0x06001B69 RID: 7017 RVA: 0x0009CEDA File Offset: 0x0009B0DA
		public override bool TestBitZero()
		{
			return Nat160.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001B6A RID: 7018 RVA: 0x0009CEEB File Offset: 0x0009B0EB
		public override BigInteger ToBigInteger()
		{
			return Nat160.ToBigInteger(this.x);
		}

		// Token: 0x17000269 RID: 617
		// (get) Token: 0x06001B6B RID: 7019 RVA: 0x0009CEF8 File Offset: 0x0009B0F8
		public override string FieldName
		{
			get
			{
				return "SecP160R2Field";
			}
		}

		// Token: 0x1700026A RID: 618
		// (get) Token: 0x06001B6C RID: 7020 RVA: 0x0009CEFF File Offset: 0x0009B0FF
		public override int FieldSize
		{
			get
			{
				return SecP160R2FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x0009CF0C File Offset: 0x0009B10C
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Add(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x0009CF3C File Offset: 0x0009B13C
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.AddOne(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x0009CF64 File Offset: 0x0009B164
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Subtract(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0009CF94 File Offset: 0x0009B194
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Multiply(this.x, ((SecP160R2FieldElement)b).x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0009CFC4 File Offset: 0x0009B1C4
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R2Field.P, ((SecP160R2FieldElement)b).x, z);
			SecP160R2Field.Multiply(z, this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x0009D000 File Offset: 0x0009B200
		public override ECFieldElement Negate()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Negate(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B73 RID: 7027 RVA: 0x0009D028 File Offset: 0x0009B228
		public override ECFieldElement Square()
		{
			uint[] z = Nat160.Create();
			SecP160R2Field.Square(this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B74 RID: 7028 RVA: 0x0009D050 File Offset: 0x0009B250
		public override ECFieldElement Invert()
		{
			uint[] z = Nat160.Create();
			Mod.Invert(SecP160R2Field.P, this.x, z);
			return new SecP160R2FieldElement(z);
		}

		// Token: 0x06001B75 RID: 7029 RVA: 0x0009D07C File Offset: 0x0009B27C
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat160.IsZero(y) || Nat160.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat160.Create();
			SecP160R2Field.Square(y, array);
			SecP160R2Field.Multiply(array, y, array);
			uint[] array2 = Nat160.Create();
			SecP160R2Field.Square(array, array2);
			SecP160R2Field.Multiply(array2, y, array2);
			uint[] array3 = Nat160.Create();
			SecP160R2Field.Square(array2, array3);
			SecP160R2Field.Multiply(array3, y, array3);
			uint[] array4 = Nat160.Create();
			SecP160R2Field.SquareN(array3, 3, array4);
			SecP160R2Field.Multiply(array4, array2, array4);
			uint[] array5 = array3;
			SecP160R2Field.SquareN(array4, 7, array5);
			SecP160R2Field.Multiply(array5, array4, array5);
			uint[] array6 = array4;
			SecP160R2Field.SquareN(array5, 3, array6);
			SecP160R2Field.Multiply(array6, array2, array6);
			uint[] array7 = Nat160.Create();
			SecP160R2Field.SquareN(array6, 14, array7);
			SecP160R2Field.Multiply(array7, array5, array7);
			uint[] array8 = array5;
			SecP160R2Field.SquareN(array7, 31, array8);
			SecP160R2Field.Multiply(array8, array7, array8);
			uint[] z = array7;
			SecP160R2Field.SquareN(array8, 62, z);
			SecP160R2Field.Multiply(z, array8, z);
			uint[] array9 = array8;
			SecP160R2Field.SquareN(z, 3, array9);
			SecP160R2Field.Multiply(array9, array2, array9);
			uint[] z2 = array9;
			SecP160R2Field.SquareN(z2, 18, z2);
			SecP160R2Field.Multiply(z2, array6, z2);
			SecP160R2Field.SquareN(z2, 2, z2);
			SecP160R2Field.Multiply(z2, y, z2);
			SecP160R2Field.SquareN(z2, 3, z2);
			SecP160R2Field.Multiply(z2, array, z2);
			SecP160R2Field.SquareN(z2, 6, z2);
			SecP160R2Field.Multiply(z2, array2, z2);
			SecP160R2Field.SquareN(z2, 2, z2);
			SecP160R2Field.Multiply(z2, y, z2);
			uint[] array10 = array;
			SecP160R2Field.Square(z2, array10);
			if (!Nat160.Eq(y, array10))
			{
				return null;
			}
			return new SecP160R2FieldElement(z2);
		}

		// Token: 0x06001B76 RID: 7030 RVA: 0x0009D21D File Offset: 0x0009B41D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP160R2FieldElement);
		}

		// Token: 0x06001B77 RID: 7031 RVA: 0x0009D21D File Offset: 0x0009B41D
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP160R2FieldElement);
		}

		// Token: 0x06001B78 RID: 7032 RVA: 0x0009D22B File Offset: 0x0009B42B
		public virtual bool Equals(SecP160R2FieldElement other)
		{
			return this == other || (other != null && Nat160.Eq(this.x, other.x));
		}

		// Token: 0x06001B79 RID: 7033 RVA: 0x0009D249 File Offset: 0x0009B449
		public override int GetHashCode()
		{
			return SecP160R2FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x040015C9 RID: 5577
		public static readonly BigInteger Q = SecP160R2Curve.q;

		// Token: 0x040015CA RID: 5578
		protected internal readonly uint[] x;
	}
}
