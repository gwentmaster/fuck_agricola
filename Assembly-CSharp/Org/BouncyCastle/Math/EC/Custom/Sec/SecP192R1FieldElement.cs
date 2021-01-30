using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000310 RID: 784
	internal class SecP192R1FieldElement : ECFieldElement
	{
		// Token: 0x06001BDC RID: 7132 RVA: 0x0009EB19 File Offset: 0x0009CD19
		public SecP192R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP192R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP192R1FieldElement", "x");
			}
			this.x = SecP192R1Field.FromBigInteger(x);
		}

		// Token: 0x06001BDD RID: 7133 RVA: 0x0009EB57 File Offset: 0x0009CD57
		public SecP192R1FieldElement()
		{
			this.x = Nat192.Create();
		}

		// Token: 0x06001BDE RID: 7134 RVA: 0x0009EB6A File Offset: 0x0009CD6A
		protected internal SecP192R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000275 RID: 629
		// (get) Token: 0x06001BDF RID: 7135 RVA: 0x0009EB79 File Offset: 0x0009CD79
		public override bool IsZero
		{
			get
			{
				return Nat192.IsZero(this.x);
			}
		}

		// Token: 0x17000276 RID: 630
		// (get) Token: 0x06001BE0 RID: 7136 RVA: 0x0009EB86 File Offset: 0x0009CD86
		public override bool IsOne
		{
			get
			{
				return Nat192.IsOne(this.x);
			}
		}

		// Token: 0x06001BE1 RID: 7137 RVA: 0x0009EB93 File Offset: 0x0009CD93
		public override bool TestBitZero()
		{
			return Nat192.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001BE2 RID: 7138 RVA: 0x0009EBA4 File Offset: 0x0009CDA4
		public override BigInteger ToBigInteger()
		{
			return Nat192.ToBigInteger(this.x);
		}

		// Token: 0x17000277 RID: 631
		// (get) Token: 0x06001BE3 RID: 7139 RVA: 0x0009EBB1 File Offset: 0x0009CDB1
		public override string FieldName
		{
			get
			{
				return "SecP192R1Field";
			}
		}

		// Token: 0x17000278 RID: 632
		// (get) Token: 0x06001BE4 RID: 7140 RVA: 0x0009EBB8 File Offset: 0x0009CDB8
		public override int FieldSize
		{
			get
			{
				return SecP192R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001BE5 RID: 7141 RVA: 0x0009EBC4 File Offset: 0x0009CDC4
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Add(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BE6 RID: 7142 RVA: 0x0009EBF4 File Offset: 0x0009CDF4
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.AddOne(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BE7 RID: 7143 RVA: 0x0009EC1C File Offset: 0x0009CE1C
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Subtract(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BE8 RID: 7144 RVA: 0x0009EC4C File Offset: 0x0009CE4C
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Multiply(this.x, ((SecP192R1FieldElement)b).x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BE9 RID: 7145 RVA: 0x0009EC7C File Offset: 0x0009CE7C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192R1Field.P, ((SecP192R1FieldElement)b).x, z);
			SecP192R1Field.Multiply(z, this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BEA RID: 7146 RVA: 0x0009ECB8 File Offset: 0x0009CEB8
		public override ECFieldElement Negate()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Negate(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BEB RID: 7147 RVA: 0x0009ECE0 File Offset: 0x0009CEE0
		public override ECFieldElement Square()
		{
			uint[] z = Nat192.Create();
			SecP192R1Field.Square(this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BEC RID: 7148 RVA: 0x0009ED08 File Offset: 0x0009CF08
		public override ECFieldElement Invert()
		{
			uint[] z = Nat192.Create();
			Mod.Invert(SecP192R1Field.P, this.x, z);
			return new SecP192R1FieldElement(z);
		}

		// Token: 0x06001BED RID: 7149 RVA: 0x0009ED34 File Offset: 0x0009CF34
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat192.IsZero(y) || Nat192.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat192.Create();
			uint[] array2 = Nat192.Create();
			SecP192R1Field.Square(y, array);
			SecP192R1Field.Multiply(array, y, array);
			SecP192R1Field.SquareN(array, 2, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 4, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 8, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 16, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 32, array2);
			SecP192R1Field.Multiply(array2, array, array2);
			SecP192R1Field.SquareN(array2, 64, array);
			SecP192R1Field.Multiply(array, array2, array);
			SecP192R1Field.SquareN(array, 62, array);
			SecP192R1Field.Square(array, array2);
			if (!Nat192.Eq(y, array2))
			{
				return null;
			}
			return new SecP192R1FieldElement(array);
		}

		// Token: 0x06001BEE RID: 7150 RVA: 0x0009EDF9 File Offset: 0x0009CFF9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP192R1FieldElement);
		}

		// Token: 0x06001BEF RID: 7151 RVA: 0x0009EDF9 File Offset: 0x0009CFF9
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP192R1FieldElement);
		}

		// Token: 0x06001BF0 RID: 7152 RVA: 0x0009EE07 File Offset: 0x0009D007
		public virtual bool Equals(SecP192R1FieldElement other)
		{
			return this == other || (other != null && Nat192.Eq(this.x, other.x));
		}

		// Token: 0x06001BF1 RID: 7153 RVA: 0x0009EE25 File Offset: 0x0009D025
		public override int GetHashCode()
		{
			return SecP192R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 6);
		}

		// Token: 0x040015DE RID: 5598
		public static readonly BigInteger Q = SecP192R1Curve.q;

		// Token: 0x040015DF RID: 5599
		protected internal readonly uint[] x;
	}
}
