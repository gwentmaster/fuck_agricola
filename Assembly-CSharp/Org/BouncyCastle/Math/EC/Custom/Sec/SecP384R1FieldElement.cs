using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000324 RID: 804
	internal class SecP384R1FieldElement : ECFieldElement
	{
		// Token: 0x06001D0D RID: 7437 RVA: 0x000A35A2 File Offset: 0x000A17A2
		public SecP384R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP384R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP384R1FieldElement", "x");
			}
			this.x = SecP384R1Field.FromBigInteger(x);
		}

		// Token: 0x06001D0E RID: 7438 RVA: 0x000A35E0 File Offset: 0x000A17E0
		public SecP384R1FieldElement()
		{
			this.x = Nat.Create(12);
		}

		// Token: 0x06001D0F RID: 7439 RVA: 0x000A35F5 File Offset: 0x000A17F5
		protected internal SecP384R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000298 RID: 664
		// (get) Token: 0x06001D10 RID: 7440 RVA: 0x000A3604 File Offset: 0x000A1804
		public override bool IsZero
		{
			get
			{
				return Nat.IsZero(12, this.x);
			}
		}

		// Token: 0x17000299 RID: 665
		// (get) Token: 0x06001D11 RID: 7441 RVA: 0x000A3613 File Offset: 0x000A1813
		public override bool IsOne
		{
			get
			{
				return Nat.IsOne(12, this.x);
			}
		}

		// Token: 0x06001D12 RID: 7442 RVA: 0x000A3622 File Offset: 0x000A1822
		public override bool TestBitZero()
		{
			return Nat.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001D13 RID: 7443 RVA: 0x000A3633 File Offset: 0x000A1833
		public override BigInteger ToBigInteger()
		{
			return Nat.ToBigInteger(12, this.x);
		}

		// Token: 0x1700029A RID: 666
		// (get) Token: 0x06001D14 RID: 7444 RVA: 0x000A3642 File Offset: 0x000A1842
		public override string FieldName
		{
			get
			{
				return "SecP384R1Field";
			}
		}

		// Token: 0x1700029B RID: 667
		// (get) Token: 0x06001D15 RID: 7445 RVA: 0x000A3649 File Offset: 0x000A1849
		public override int FieldSize
		{
			get
			{
				return SecP384R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001D16 RID: 7446 RVA: 0x000A3658 File Offset: 0x000A1858
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Add(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D17 RID: 7447 RVA: 0x000A368C File Offset: 0x000A188C
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.AddOne(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D18 RID: 7448 RVA: 0x000A36B4 File Offset: 0x000A18B4
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Subtract(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D19 RID: 7449 RVA: 0x000A36E8 File Offset: 0x000A18E8
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Multiply(this.x, ((SecP384R1FieldElement)b).x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D1A RID: 7450 RVA: 0x000A371C File Offset: 0x000A191C
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat.Create(12);
			Mod.Invert(SecP384R1Field.P, ((SecP384R1FieldElement)b).x, z);
			SecP384R1Field.Multiply(z, this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D1B RID: 7451 RVA: 0x000A375C File Offset: 0x000A195C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Negate(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D1C RID: 7452 RVA: 0x000A3784 File Offset: 0x000A1984
		public override ECFieldElement Square()
		{
			uint[] z = Nat.Create(12);
			SecP384R1Field.Square(this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D1D RID: 7453 RVA: 0x000A37AC File Offset: 0x000A19AC
		public override ECFieldElement Invert()
		{
			uint[] z = Nat.Create(12);
			Mod.Invert(SecP384R1Field.P, this.x, z);
			return new SecP384R1FieldElement(z);
		}

		// Token: 0x06001D1E RID: 7454 RVA: 0x000A37D8 File Offset: 0x000A19D8
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat.IsZero(12, y) || Nat.IsOne(12, y))
			{
				return this;
			}
			uint[] array = Nat.Create(12);
			uint[] array2 = Nat.Create(12);
			uint[] array3 = Nat.Create(12);
			uint[] array4 = Nat.Create(12);
			SecP384R1Field.Square(y, array);
			SecP384R1Field.Multiply(array, y, array);
			SecP384R1Field.SquareN(array, 2, array2);
			SecP384R1Field.Multiply(array2, array, array2);
			SecP384R1Field.Square(array2, array2);
			SecP384R1Field.Multiply(array2, y, array2);
			SecP384R1Field.SquareN(array2, 5, array3);
			SecP384R1Field.Multiply(array3, array2, array3);
			SecP384R1Field.SquareN(array3, 5, array4);
			SecP384R1Field.Multiply(array4, array2, array4);
			SecP384R1Field.SquareN(array4, 15, array2);
			SecP384R1Field.Multiply(array2, array4, array2);
			SecP384R1Field.SquareN(array2, 2, array3);
			SecP384R1Field.Multiply(array, array3, array);
			SecP384R1Field.SquareN(array3, 28, array3);
			SecP384R1Field.Multiply(array2, array3, array2);
			SecP384R1Field.SquareN(array2, 60, array3);
			SecP384R1Field.Multiply(array3, array2, array3);
			uint[] z = array2;
			SecP384R1Field.SquareN(array3, 120, z);
			SecP384R1Field.Multiply(z, array3, z);
			SecP384R1Field.SquareN(z, 15, z);
			SecP384R1Field.Multiply(z, array4, z);
			SecP384R1Field.SquareN(z, 33, z);
			SecP384R1Field.Multiply(z, array, z);
			SecP384R1Field.SquareN(z, 64, z);
			SecP384R1Field.Multiply(z, y, z);
			SecP384R1Field.SquareN(z, 30, array);
			SecP384R1Field.Square(array, array2);
			if (!Nat.Eq(12, y, array2))
			{
				return null;
			}
			return new SecP384R1FieldElement(array);
		}

		// Token: 0x06001D1F RID: 7455 RVA: 0x000A3934 File Offset: 0x000A1B34
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP384R1FieldElement);
		}

		// Token: 0x06001D20 RID: 7456 RVA: 0x000A3934 File Offset: 0x000A1B34
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP384R1FieldElement);
		}

		// Token: 0x06001D21 RID: 7457 RVA: 0x000A3942 File Offset: 0x000A1B42
		public virtual bool Equals(SecP384R1FieldElement other)
		{
			return this == other || (other != null && Nat.Eq(12, this.x, other.x));
		}

		// Token: 0x06001D22 RID: 7458 RVA: 0x000A3962 File Offset: 0x000A1B62
		public override int GetHashCode()
		{
			return SecP384R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 12);
		}

		// Token: 0x04001612 RID: 5650
		public static readonly BigInteger Q = SecP384R1Curve.q;

		// Token: 0x04001613 RID: 5651
		protected internal readonly uint[] x;
	}
}
