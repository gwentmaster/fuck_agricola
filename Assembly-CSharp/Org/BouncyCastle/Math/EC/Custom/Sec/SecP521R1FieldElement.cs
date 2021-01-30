using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000328 RID: 808
	internal class SecP521R1FieldElement : ECFieldElement
	{
		// Token: 0x06001D47 RID: 7495 RVA: 0x000A4375 File Offset: 0x000A2575
		public SecP521R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP521R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP521R1FieldElement", "x");
			}
			this.x = SecP521R1Field.FromBigInteger(x);
		}

		// Token: 0x06001D48 RID: 7496 RVA: 0x000A43B3 File Offset: 0x000A25B3
		public SecP521R1FieldElement()
		{
			this.x = Nat.Create(17);
		}

		// Token: 0x06001D49 RID: 7497 RVA: 0x000A43C8 File Offset: 0x000A25C8
		protected internal SecP521R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x06001D4A RID: 7498 RVA: 0x000A43D7 File Offset: 0x000A25D7
		public override bool IsZero
		{
			get
			{
				return Nat.IsZero(17, this.x);
			}
		}

		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x06001D4B RID: 7499 RVA: 0x000A43E6 File Offset: 0x000A25E6
		public override bool IsOne
		{
			get
			{
				return Nat.IsOne(17, this.x);
			}
		}

		// Token: 0x06001D4C RID: 7500 RVA: 0x000A43F5 File Offset: 0x000A25F5
		public override bool TestBitZero()
		{
			return Nat.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001D4D RID: 7501 RVA: 0x000A4406 File Offset: 0x000A2606
		public override BigInteger ToBigInteger()
		{
			return Nat.ToBigInteger(17, this.x);
		}

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06001D4E RID: 7502 RVA: 0x000A4415 File Offset: 0x000A2615
		public override string FieldName
		{
			get
			{
				return "SecP521R1Field";
			}
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001D4F RID: 7503 RVA: 0x000A441C File Offset: 0x000A261C
		public override int FieldSize
		{
			get
			{
				return SecP521R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001D50 RID: 7504 RVA: 0x000A4428 File Offset: 0x000A2628
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Add(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D51 RID: 7505 RVA: 0x000A445C File Offset: 0x000A265C
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.AddOne(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D52 RID: 7506 RVA: 0x000A4484 File Offset: 0x000A2684
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Subtract(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D53 RID: 7507 RVA: 0x000A44B8 File Offset: 0x000A26B8
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Multiply(this.x, ((SecP521R1FieldElement)b).x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D54 RID: 7508 RVA: 0x000A44EC File Offset: 0x000A26EC
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat.Create(17);
			Mod.Invert(SecP521R1Field.P, ((SecP521R1FieldElement)b).x, z);
			SecP521R1Field.Multiply(z, this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D55 RID: 7509 RVA: 0x000A452C File Offset: 0x000A272C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Negate(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D56 RID: 7510 RVA: 0x000A4554 File Offset: 0x000A2754
		public override ECFieldElement Square()
		{
			uint[] z = Nat.Create(17);
			SecP521R1Field.Square(this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D57 RID: 7511 RVA: 0x000A457C File Offset: 0x000A277C
		public override ECFieldElement Invert()
		{
			uint[] z = Nat.Create(17);
			Mod.Invert(SecP521R1Field.P, this.x, z);
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D58 RID: 7512 RVA: 0x000A45A8 File Offset: 0x000A27A8
		public override ECFieldElement Sqrt()
		{
			uint[] array = this.x;
			if (Nat.IsZero(17, array) || Nat.IsOne(17, array))
			{
				return this;
			}
			uint[] z = Nat.Create(17);
			uint[] array2 = Nat.Create(17);
			SecP521R1Field.SquareN(array, 519, z);
			SecP521R1Field.Square(z, array2);
			if (!Nat.Eq(17, array, array2))
			{
				return null;
			}
			return new SecP521R1FieldElement(z);
		}

		// Token: 0x06001D59 RID: 7513 RVA: 0x000A4608 File Offset: 0x000A2808
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP521R1FieldElement);
		}

		// Token: 0x06001D5A RID: 7514 RVA: 0x000A4608 File Offset: 0x000A2808
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP521R1FieldElement);
		}

		// Token: 0x06001D5B RID: 7515 RVA: 0x000A4616 File Offset: 0x000A2816
		public virtual bool Equals(SecP521R1FieldElement other)
		{
			return this == other || (other != null && Nat.Eq(17, this.x, other.x));
		}

		// Token: 0x06001D5C RID: 7516 RVA: 0x000A4636 File Offset: 0x000A2836
		public override int GetHashCode()
		{
			return SecP521R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 17);
		}

		// Token: 0x04001619 RID: 5657
		public static readonly BigInteger Q = SecP521R1Curve.q;

		// Token: 0x0400161A RID: 5658
		protected internal readonly uint[] x;
	}
}
