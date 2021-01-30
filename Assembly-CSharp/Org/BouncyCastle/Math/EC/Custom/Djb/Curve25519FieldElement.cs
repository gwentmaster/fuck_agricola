using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x02000362 RID: 866
	internal class Curve25519FieldElement : ECFieldElement
	{
		// Token: 0x06002113 RID: 8467 RVA: 0x000B29EE File Offset: 0x000B0BEE
		public Curve25519FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(Curve25519FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for Curve25519FieldElement", "x");
			}
			this.x = Curve25519Field.FromBigInteger(x);
		}

		// Token: 0x06002114 RID: 8468 RVA: 0x000B2A2C File Offset: 0x000B0C2C
		public Curve25519FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x06002115 RID: 8469 RVA: 0x000B2A3F File Offset: 0x000B0C3F
		protected internal Curve25519FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06002116 RID: 8470 RVA: 0x000B2A4E File Offset: 0x000B0C4E
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06002117 RID: 8471 RVA: 0x000B2A5B File Offset: 0x000B0C5B
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x06002118 RID: 8472 RVA: 0x000B2A68 File Offset: 0x000B0C68
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06002119 RID: 8473 RVA: 0x000B2A79 File Offset: 0x000B0C79
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x170003AD RID: 941
		// (get) Token: 0x0600211A RID: 8474 RVA: 0x000B2A86 File Offset: 0x000B0C86
		public override string FieldName
		{
			get
			{
				return "Curve25519Field";
			}
		}

		// Token: 0x170003AE RID: 942
		// (get) Token: 0x0600211B RID: 8475 RVA: 0x000B2A8D File Offset: 0x000B0C8D
		public override int FieldSize
		{
			get
			{
				return Curve25519FieldElement.Q.BitLength;
			}
		}

		// Token: 0x0600211C RID: 8476 RVA: 0x000B2A9C File Offset: 0x000B0C9C
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Add(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x0600211D RID: 8477 RVA: 0x000B2ACC File Offset: 0x000B0CCC
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.AddOne(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x0600211E RID: 8478 RVA: 0x000B2AF4 File Offset: 0x000B0CF4
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Subtract(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x0600211F RID: 8479 RVA: 0x000B2B24 File Offset: 0x000B0D24
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Multiply(this.x, ((Curve25519FieldElement)b).x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002120 RID: 8480 RVA: 0x000B2B54 File Offset: 0x000B0D54
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(Curve25519Field.P, ((Curve25519FieldElement)b).x, z);
			Curve25519Field.Multiply(z, this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002121 RID: 8481 RVA: 0x000B2B90 File Offset: 0x000B0D90
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Negate(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002122 RID: 8482 RVA: 0x000B2BB8 File Offset: 0x000B0DB8
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			Curve25519Field.Square(this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002123 RID: 8483 RVA: 0x000B2BE0 File Offset: 0x000B0DE0
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(Curve25519Field.P, this.x, z);
			return new Curve25519FieldElement(z);
		}

		// Token: 0x06002124 RID: 8484 RVA: 0x000B2C0C File Offset: 0x000B0E0C
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			Curve25519Field.Square(y, array);
			Curve25519Field.Multiply(array, y, array);
			uint[] array2 = array;
			Curve25519Field.Square(array, array2);
			Curve25519Field.Multiply(array2, y, array2);
			uint[] array3 = Nat256.Create();
			Curve25519Field.Square(array2, array3);
			Curve25519Field.Multiply(array3, y, array3);
			uint[] array4 = Nat256.Create();
			Curve25519Field.SquareN(array3, 3, array4);
			Curve25519Field.Multiply(array4, array2, array4);
			uint[] array5 = array2;
			Curve25519Field.SquareN(array4, 4, array5);
			Curve25519Field.Multiply(array5, array3, array5);
			uint[] array6 = array4;
			Curve25519Field.SquareN(array5, 4, array6);
			Curve25519Field.Multiply(array6, array3, array6);
			uint[] array7 = array3;
			Curve25519Field.SquareN(array6, 15, array7);
			Curve25519Field.Multiply(array7, array6, array7);
			uint[] array8 = array6;
			Curve25519Field.SquareN(array7, 30, array8);
			Curve25519Field.Multiply(array8, array7, array8);
			uint[] array9 = array7;
			Curve25519Field.SquareN(array8, 60, array9);
			Curve25519Field.Multiply(array9, array8, array9);
			uint[] z = array8;
			Curve25519Field.SquareN(array9, 11, z);
			Curve25519Field.Multiply(z, array5, z);
			uint[] array10 = array5;
			Curve25519Field.SquareN(z, 120, array10);
			Curve25519Field.Multiply(array10, array9, array10);
			uint[] z2 = array10;
			Curve25519Field.Square(z2, z2);
			uint[] array11 = array9;
			Curve25519Field.Square(z2, array11);
			if (Nat256.Eq(y, array11))
			{
				return new Curve25519FieldElement(z2);
			}
			Curve25519Field.Multiply(z2, Curve25519FieldElement.PRECOMP_POW2, z2);
			Curve25519Field.Square(z2, array11);
			if (Nat256.Eq(y, array11))
			{
				return new Curve25519FieldElement(z2);
			}
			return null;
		}

		// Token: 0x06002125 RID: 8485 RVA: 0x000B2D8D File Offset: 0x000B0F8D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as Curve25519FieldElement);
		}

		// Token: 0x06002126 RID: 8486 RVA: 0x000B2D8D File Offset: 0x000B0F8D
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as Curve25519FieldElement);
		}

		// Token: 0x06002127 RID: 8487 RVA: 0x000B2D9B File Offset: 0x000B0F9B
		public virtual bool Equals(Curve25519FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x06002128 RID: 8488 RVA: 0x000B2DB9 File Offset: 0x000B0FB9
		public override int GetHashCode()
		{
			return Curve25519FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001667 RID: 5735
		public static readonly BigInteger Q = Curve25519.q;

		// Token: 0x04001668 RID: 5736
		private static readonly uint[] PRECOMP_POW2 = new uint[]
		{
			1242472624U,
			3303938855U,
			2905597048U,
			792926214U,
			1039914919U,
			726466713U,
			1338105611U,
			730014848U
		};

		// Token: 0x04001669 RID: 5737
		protected internal readonly uint[] x;
	}
}
