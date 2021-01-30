using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000320 RID: 800
	internal class SecP256R1FieldElement : ECFieldElement
	{
		// Token: 0x06001CD1 RID: 7377 RVA: 0x000A25EB File Offset: 0x000A07EB
		public SecP256R1FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.CompareTo(SecP256R1FieldElement.Q) >= 0)
			{
				throw new ArgumentException("value invalid for SecP256R1FieldElement", "x");
			}
			this.x = SecP256R1Field.FromBigInteger(x);
		}

		// Token: 0x06001CD2 RID: 7378 RVA: 0x000A2629 File Offset: 0x000A0829
		public SecP256R1FieldElement()
		{
			this.x = Nat256.Create();
		}

		// Token: 0x06001CD3 RID: 7379 RVA: 0x000A263C File Offset: 0x000A083C
		protected internal SecP256R1FieldElement(uint[] x)
		{
			this.x = x;
		}

		// Token: 0x17000291 RID: 657
		// (get) Token: 0x06001CD4 RID: 7380 RVA: 0x000A264B File Offset: 0x000A084B
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero(this.x);
			}
		}

		// Token: 0x17000292 RID: 658
		// (get) Token: 0x06001CD5 RID: 7381 RVA: 0x000A2658 File Offset: 0x000A0858
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne(this.x);
			}
		}

		// Token: 0x06001CD6 RID: 7382 RVA: 0x000A2665 File Offset: 0x000A0865
		public override bool TestBitZero()
		{
			return Nat256.GetBit(this.x, 0) == 1U;
		}

		// Token: 0x06001CD7 RID: 7383 RVA: 0x000A2676 File Offset: 0x000A0876
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger(this.x);
		}

		// Token: 0x17000293 RID: 659
		// (get) Token: 0x06001CD8 RID: 7384 RVA: 0x000A2683 File Offset: 0x000A0883
		public override string FieldName
		{
			get
			{
				return "SecP256R1Field";
			}
		}

		// Token: 0x17000294 RID: 660
		// (get) Token: 0x06001CD9 RID: 7385 RVA: 0x000A268A File Offset: 0x000A088A
		public override int FieldSize
		{
			get
			{
				return SecP256R1FieldElement.Q.BitLength;
			}
		}

		// Token: 0x06001CDA RID: 7386 RVA: 0x000A2698 File Offset: 0x000A0898
		public override ECFieldElement Add(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Add(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CDB RID: 7387 RVA: 0x000A26C8 File Offset: 0x000A08C8
		public override ECFieldElement AddOne()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.AddOne(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CDC RID: 7388 RVA: 0x000A26F0 File Offset: 0x000A08F0
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Subtract(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CDD RID: 7389 RVA: 0x000A2720 File Offset: 0x000A0920
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Multiply(this.x, ((SecP256R1FieldElement)b).x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CDE RID: 7390 RVA: 0x000A2750 File Offset: 0x000A0950
		public override ECFieldElement Divide(ECFieldElement b)
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256R1Field.P, ((SecP256R1FieldElement)b).x, z);
			SecP256R1Field.Multiply(z, this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CDF RID: 7391 RVA: 0x000A278C File Offset: 0x000A098C
		public override ECFieldElement Negate()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Negate(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CE0 RID: 7392 RVA: 0x000A27B4 File Offset: 0x000A09B4
		public override ECFieldElement Square()
		{
			uint[] z = Nat256.Create();
			SecP256R1Field.Square(this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CE1 RID: 7393 RVA: 0x000A27DC File Offset: 0x000A09DC
		public override ECFieldElement Invert()
		{
			uint[] z = Nat256.Create();
			Mod.Invert(SecP256R1Field.P, this.x, z);
			return new SecP256R1FieldElement(z);
		}

		// Token: 0x06001CE2 RID: 7394 RVA: 0x000A2808 File Offset: 0x000A0A08
		public override ECFieldElement Sqrt()
		{
			uint[] y = this.x;
			if (Nat256.IsZero(y) || Nat256.IsOne(y))
			{
				return this;
			}
			uint[] array = Nat256.Create();
			uint[] array2 = Nat256.Create();
			SecP256R1Field.Square(y, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 2, array2);
			SecP256R1Field.Multiply(array2, array, array2);
			SecP256R1Field.SquareN(array2, 4, array);
			SecP256R1Field.Multiply(array, array2, array);
			SecP256R1Field.SquareN(array, 8, array2);
			SecP256R1Field.Multiply(array2, array, array2);
			SecP256R1Field.SquareN(array2, 16, array);
			SecP256R1Field.Multiply(array, array2, array);
			SecP256R1Field.SquareN(array, 32, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 96, array);
			SecP256R1Field.Multiply(array, y, array);
			SecP256R1Field.SquareN(array, 94, array);
			SecP256R1Field.Multiply(array, array, array2);
			if (!Nat256.Eq(y, array2))
			{
				return null;
			}
			return new SecP256R1FieldElement(array);
		}

		// Token: 0x06001CE3 RID: 7395 RVA: 0x000A28CE File Offset: 0x000A0ACE
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecP256R1FieldElement);
		}

		// Token: 0x06001CE4 RID: 7396 RVA: 0x000A28CE File Offset: 0x000A0ACE
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecP256R1FieldElement);
		}

		// Token: 0x06001CE5 RID: 7397 RVA: 0x000A28DC File Offset: 0x000A0ADC
		public virtual bool Equals(SecP256R1FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq(this.x, other.x));
		}

		// Token: 0x06001CE6 RID: 7398 RVA: 0x000A28FA File Offset: 0x000A0AFA
		public override int GetHashCode()
		{
			return SecP256R1FieldElement.Q.GetHashCode() ^ Arrays.GetHashCode(this.x, 0, 8);
		}

		// Token: 0x04001608 RID: 5640
		public static readonly BigInteger Q = SecP256R1Curve.q;

		// Token: 0x04001609 RID: 5641
		protected internal readonly uint[] x;
	}
}
