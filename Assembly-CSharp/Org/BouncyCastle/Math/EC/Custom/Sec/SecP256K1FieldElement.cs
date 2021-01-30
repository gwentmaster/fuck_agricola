using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200031C RID: 796
	internal class SecP256K1FieldElement : ECFieldElement
	{
		// Token: 0x06001C94 RID: 7316 RVA: 0x000A1619 File Offset: 0x0009F819
		public SecP256K1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP256K1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP256K1FieldElement", "x");
			}
			this.x = SecP256K1Field.FromBigInteger(x);
		}

		// Token: 0x06001C95 RID: 7317 RVA: 0x000A1657 File Offset: 0x0009F857
		public SecP256K1FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x000A166A File Offset: 0x0009F86A
		protected internal SecP256K1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001C97 RID: 7319 RVA: 0x000A1679 File Offset: 0x0009F879
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x000A1686 File Offset: 0x0009F886
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x06001C99 RID: 7321 RVA: 0x000A1693 File Offset: 0x0009F893
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001C9A RID: 7322 RVA: 0x000A16A4 File Offset: 0x0009F8A4
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x000A16B1 File Offset: 0x0009F8B1
		public override string FieldName
		{
			get
			{
				return "SecP256K1Field";
			}
		}

		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001C9C RID: 7324 RVA: 0x000A16B8 File Offset: 0x0009F8B8
		public override int FieldSize
		{
			get
			{
				return SecP256K1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001C9D RID: 7325 RVA: 0x000A16C4 File Offset: 0x0009F8C4
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Add(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001C9E RID: 7326 RVA: 0x000A16F4 File Offset: 0x0009F8F4
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.AddOne(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x000A171C File Offset: 0x0009F91C
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Subtract(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x000A174C File Offset: 0x0009F94C
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Multiply(this.x, ((SecP256K1FieldElement)b).x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x000A177C File Offset: 0x0009F97C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256K1Field.P, ((SecP256K1FieldElement)b).x, z);
			SecP256K1Field.Multiply(z, this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x000A17B8 File Offset: 0x0009F9B8
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Negate(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x000A17E0 File Offset: 0x0009F9E0
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			SecP256K1Field.Square(this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x000A1808 File Offset: 0x0009FA08
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256K1Field.P, this.x, z);
			return new SecP256K1FieldElement(z);
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x000A1834 File Offset: 0x0009FA34
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			SecP256K1Field.Square(y, array);
			SecP256K1Field.Multiply(array, y, array);
			uint[] array2 = Nat256.Create();
			SecP256K1Field.Square(array, array2);
			SecP256K1Field.Multiply(array2, y, array2);
			uint[] array3 = Nat256.Create();
			SecP256K1Field.SquareN(array2, 3, array3);
			SecP256K1Field.Multiply(array3, array2, array3);
			uint[] array4 = array3;
			SecP256K1Field.SquareN(array3, 3, array4);
			SecP256K1Field.Multiply(array4, array2, array4);
			uint[] array5 = array4;
			SecP256K1Field.SquareN(array4, 2, array5);
			SecP256K1Field.Multiply(array5, array, array5);
			uint[] array6 = Nat256.Create();
			SecP256K1Field.SquareN(array5, 11, array6);
			SecP256K1Field.Multiply(array6, array5, array6);
			uint[] array7 = array5;
			SecP256K1Field.SquareN(array6, 22, array7);
			SecP256K1Field.Multiply(array7, array6, array7);
			uint[] array8 = Nat256.Create();
			SecP256K1Field.SquareN(array7, 44, array8);
			SecP256K1Field.Multiply(array8, array7, array8);
			uint[] z = Nat256.Create();
			SecP256K1Field.SquareN(array8, 88, z);
			SecP256K1Field.Multiply(z, array8, z);
			uint[] z2 = array8;
			SecP256K1Field.SquareN(z, 44, z2);
			SecP256K1Field.Multiply(z2, array7, z2);
			uint[] array9 = array7;
			SecP256K1Field.SquareN(z2, 3, array9);
			SecP256K1Field.Multiply(array9, array2, array9);
			uint[] z3 = array9;
			SecP256K1Field.SquareN(z3, 23, z3);
			SecP256K1Field.Multiply(z3, array6, z3);
			SecP256K1Field.SquareN(z3, 6, z3);
			SecP256K1Field.Multiply(z3, array, z3);
			SecP256K1Field.SquareN(z3, 2, z3);
			uint[] array10 = array;
			SecP256K1Field.Square(z3, array10);
			if (!Nat256.Eq(y, array10))
			{
				return null;
			}
			return new SecP256K1FieldElement(z3);
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x000A19C2 File Offset: 0x0009FBC2
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP256K1FieldElement);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x000A19C2 File Offset: 0x0009FBC2
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP256K1FieldElement);
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x000A19D0 File Offset: 0x0009FBD0
		public virtual bool Equals(SecP256K1FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x000A19EE File Offset: 0x0009FBEE
		public override int GetHashCode()
		{
			return SecP256K1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x040015FF RID: 5631
		public static readonly BigInteger Q = SecP256K1Curve.q;

		// Token: 0x04001600 RID: 5632
		protected internal readonly uint[] x;
	}
}
