using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000318 RID: 792
	internal class SecP224R1FieldElement : ECFieldElement
	{
		// Token: 0x06001C54 RID: 7252 RVA: 0x000A07BD File Offset: 0x0009E9BD
		public SecP224R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP224R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP224R1FieldElement", "x");
			}
			this.x = SecP224R1Field.FromBigInteger(x);
		}

		// Token: 0x06001C55 RID: 7253 RVA: 0x000A07FB File Offset: 0x0009E9FB
		public SecP224R1FieldElement()
		{
			this.x = Nat224.Create();
		}

		// Token: 0x06001C56 RID: 7254 RVA: 0x000A080E File Offset: 0x0009EA0E
		protected internal SecP224R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000283 RID: 643
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x000A081D File Offset: 0x0009EA1D
		public override bool IsZero
		{
			get
			{
				return Nat224.IsZero(this.x);
			}
		}

		// Token: 0x17000284 RID: 644
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x000A082A File Offset: 0x0009EA2A
		public override bool IsOne
		{
			get
			{
				return Nat224.IsOne(this.x);
			}
		}

		// Token: 0x06001C59 RID: 7257 RVA: 0x000A0837 File Offset: 0x0009EA37
		public override bool TestBitZero()
		{
			return Nat224.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001C5A RID: 7258 RVA: 0x000A0848 File Offset: 0x0009EA48
		public override BigInteger ToBigInteger()
		{
			return Nat224.ToBigInteger(this.x);
		}

		// Token: 0x17000285 RID: 645
		// (get) Token: 0x06001C5B RID: 7259 RVA: 0x000A0855 File Offset: 0x0009EA55
		public override string FieldName
		{
			get
			{
				return "SecP224R1Field";
			}
		}

		// Token: 0x17000286 RID: 646
		// (get) Token: 0x06001C5C RID: 7260 RVA: 0x000A085C File Offset: 0x0009EA5C
		public override int FieldSize
		{
			get
			{
				return SecP224R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x000A0868 File Offset: 0x0009EA68
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Add(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x000A0898 File Offset: 0x0009EA98
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.AddOne(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x000A08C0 File Offset: 0x0009EAC0
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Subtract(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C60 RID: 7264 RVA: 0x000A08F0 File Offset: 0x0009EAF0
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Multiply(this.x, ((SecP224R1FieldElement)b).x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C61 RID: 7265 RVA: 0x000A0920 File Offset: 0x0009EB20
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224R1Field.P, ((SecP224R1FieldElement)b).x, z);
			SecP224R1Field.Multiply(z, this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C62 RID: 7266 RVA: 0x000A095C File Offset: 0x0009EB5C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Negate(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C63 RID: 7267 RVA: 0x000A0984 File Offset: 0x0009EB84
		public override ECFieldElement Square()
		{
			uint[] z = Nat224.Create();
			SecP224R1Field.Square(this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C64 RID: 7268 RVA: 0x000A09AC File Offset: 0x0009EBAC
		public override ECFieldElement Invert()
		{
			uint[] z = Nat224.Create();
			Mod.Invert(SecP224R1Field.P, this.x, z);
			return new SecP224R1FieldElement(z);
		}

		// Token: 0x06001C65 RID: 7269 RVA: 0x000A09D8 File Offset: 0x0009EBD8
		public override ECFieldElement Sqrt()
		{
			uint[] array = this.x;
			if (Nat224.IsZero(array) || Nat224.IsOne(array))
			{
				return this;
			}
			uint[] array2 = Nat224.Create();
			SecP224R1Field.Negate(array, array2);
			uint[] array3 = Mod.Random(SecP224R1Field.P);
			uint[] t = Nat224.Create();
			if (!SecP224R1FieldElement.IsSquare(array))
			{
				return null;
			}
			while (!SecP224R1FieldElement.TrySqrt(array2, array3, t))
			{
				SecP224R1Field.AddOne(array3, array3);
			}
			SecP224R1Field.Square(t, array3);
			if (!Nat224.Eq(array, array3))
			{
				return null;
			}
			return new SecP224R1FieldElement(t);
		}

		// Token: 0x06001C66 RID: 7270 RVA: 0x000A0A4F File Offset: 0x0009EC4F
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP224R1FieldElement);
		}

		// Token: 0x06001C67 RID: 7271 RVA: 0x000A0A4F File Offset: 0x0009EC4F
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP224R1FieldElement);
		}

		// Token: 0x06001C68 RID: 7272 RVA: 0x000A0A5D File Offset: 0x0009EC5D
		public virtual bool Equals(SecP224R1FieldElement other)
		{
			return this == other || (other != null && Nat224.Eq(this.x, other.x));
		}

		// Token: 0x06001C69 RID: 7273 RVA: 0x000A0A7B File Offset: 0x0009EC7B
		public override int GetHashCode()
		{
			return SecP224R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x06001C6A RID: 7274 RVA: 0x000A0A98 File Offset: 0x0009EC98
		private static bool IsSquare(uint[] x)
		{
			uint[] z = Nat224.Create();
			uint[] array = Nat224.Create();
			Nat224.Copy(x, z);
			for (int i = 0; i < 7; i++)
			{
				Nat224.Copy(z, array);
				SecP224R1Field.SquareN(z, 1 << i, z);
				SecP224R1Field.Multiply(z, array, z);
			}
			SecP224R1Field.SquareN(z, 95, z);
			return Nat224.IsOne(z);
		}

		// Token: 0x06001C6B RID: 7275 RVA: 0x000A0AF0 File Offset: 0x0009ECF0
		private static void RM(uint[] nc, uint[] d0, uint[] e0, uint[] d1, uint[] e1, uint[] f1, uint[] t)
		{
			SecP224R1Field.Multiply(e1, e0, t);
			SecP224R1Field.Multiply(t, nc, t);
			SecP224R1Field.Multiply(d1, d0, f1);
			SecP224R1Field.Add(f1, t, f1);
			SecP224R1Field.Multiply(d1, e0, t);
			Nat224.Copy(f1, d1);
			SecP224R1Field.Multiply(e1, d0, e1);
			SecP224R1Field.Add(e1, t, e1);
			SecP224R1Field.Square(e1, f1);
			SecP224R1Field.Multiply(f1, nc, f1);
		}

		// Token: 0x06001C6C RID: 7276 RVA: 0x000A0B60 File Offset: 0x0009ED60
		private static void RP(uint[] nc, uint[] d1, uint[] e1, uint[] f1, uint[] t)
		{
			Nat224.Copy(nc, f1);
			uint[] array = Nat224.Create();
			uint[] array2 = Nat224.Create();
			for (int i = 0; i < 7; i++)
			{
				Nat224.Copy(d1, array);
				Nat224.Copy(e1, array2);
				int num = 1 << i;
				while (--num >= 0)
				{
					SecP224R1FieldElement.RS(d1, e1, f1, t);
				}
				SecP224R1FieldElement.RM(nc, array, array2, d1, e1, f1, t);
			}
		}

		// Token: 0x06001C6D RID: 7277 RVA: 0x000A0BC2 File Offset: 0x0009EDC2
		private static void RS(uint[] d, uint[] e, uint[] f, uint[] t)
		{
			SecP224R1Field.Multiply(e, d, e);
			SecP224R1Field.Twice(e, e);
			SecP224R1Field.Square(d, t);
			SecP224R1Field.Add(f, t, d);
			SecP224R1Field.Multiply(f, t, f);
			SecP224R1Field.Reduce32(Nat.ShiftUpBits(7, f, 2, 0U), f);
		}

		// Token: 0x06001C6E RID: 7278 RVA: 0x000A0BFC File Offset: 0x0009EDFC
		private static bool TrySqrt(uint[] nc, uint[] r, uint[] t)
		{
			uint[] array = Nat224.Create();
			Nat224.Copy(r, array);
			uint[] array2 = Nat224.Create();
			array2[0] = 1U;
			uint[] array3 = Nat224.Create();
			SecP224R1FieldElement.RP(nc, array, array2, array3, t);
			uint[] array4 = Nat224.Create();
			uint[] z = Nat224.Create();
			for (int i = 1; i < 96; i++)
			{
				Nat224.Copy(array, array4);
				Nat224.Copy(array2, z);
				SecP224R1FieldElement.RS(array, array2, array3, t);
				if (Nat224.IsZero(array))
				{
					Mod.Invert(SecP224R1Field.P, z, t);
					SecP224R1Field.Multiply(t, array4, t);
					return true;
				}
			}
			return false;
		}

		// Token: 0x040015F4 RID: 5620
		public static readonly BigInteger Q = SecP224R1Curve.q;

		// Token: 0x040015F5 RID: 5621
		protected internal readonly uint[] x;
	}
}
