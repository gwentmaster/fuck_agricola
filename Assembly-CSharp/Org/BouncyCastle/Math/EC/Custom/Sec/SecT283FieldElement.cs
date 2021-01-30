using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200034F RID: 847
	internal class SecT283FieldElement : ECFieldElement
	{
		// Token: 0x06001FD7 RID: 8151 RVA: 0x000AE531 File Offset: 0x000AC731
		public SecT283FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 283)
			{
				throw new ArgumentException("value invalid for SecT283FieldElement", "x");
			}
			this.x = SecT283Field.FromBigInteger(x);
		}

		// Token: 0x06001FD8 RID: 8152 RVA: 0x000AE56E File Offset: 0x000AC76E
		public SecT283FieldElement()
		{
			this.x = Nat320.Create64();
		}

		// Token: 0x06001FD9 RID: 8153 RVA: 0x000AE581 File Offset: 0x000AC781
		protected internal SecT283FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x17000351 RID: 849
		// (get) Token: 0x06001FDA RID: 8154 RVA: 0x000AE590 File Offset: 0x000AC790
		public override bool IsOne
		{
			get
			{
				return Nat320.IsOne64(this.x);
			}
		}

		// Token: 0x17000352 RID: 850
		// (get) Token: 0x06001FDB RID: 8155 RVA: 0x000AE59D File Offset: 0x000AC79D
		public override bool IsZero
		{
			get
			{
				return Nat320.IsZero64(this.x);
			}
		}

		// Token: 0x06001FDC RID: 8156 RVA: 0x000AE5AA File Offset: 0x000AC7AA
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001FDD RID: 8157 RVA: 0x000AE5BB File Offset: 0x000AC7BB
		public override BigInteger ToBigInteger()
		{
			return Nat320.ToBigInteger64(this.x);
		}

		// Token: 0x17000353 RID: 851
		// (get) Token: 0x06001FDE RID: 8158 RVA: 0x000AE5C8 File Offset: 0x000AC7C8
		public override string FieldName
		{
			get
			{
				return "SecT283Field";
			}
		}

		// Token: 0x17000354 RID: 852
		// (get) Token: 0x06001FDF RID: 8159 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public override int FieldSize
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x06001FE0 RID: 8160 RVA: 0x000AE5D8 File Offset: 0x000AC7D8
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Add(this.x, ((SecT283FieldElement)b).x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FE1 RID: 8161 RVA: 0x000AE608 File Offset: 0x000AC808
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.AddOne(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FE2 RID: 8162 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001FE3 RID: 8163 RVA: 0x000AE630 File Offset: 0x000AC830
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Multiply(this.x, ((SecT283FieldElement)b).x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FE4 RID: 8164 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001FE5 RID: 8165 RVA: 0x000AE660 File Offset: 0x000AC860
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT283FieldElement)b).x;
			ulong[] array2 = ((SecT283FieldElement)x).x;
			ulong[] y3 = ((SecT283FieldElement)y).x;
			ulong[] array3 = Nat.Create64(9);
			SecT283Field.MultiplyAddToExt(array, y2, array3);
			SecT283Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat320.Create64();
			SecT283Field.Reduce(array3, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FE6 RID: 8166 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001FE7 RID: 8167 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001FE8 RID: 8168 RVA: 0x000AE6C4 File Offset: 0x000AC8C4
		public override ECFieldElement Square()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Square(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FE9 RID: 8169 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001FEA RID: 8170 RVA: 0x000AE6EC File Offset: 0x000AC8EC
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT283FieldElement)x).x;
			ulong[] y2 = ((SecT283FieldElement)y).x;
			ulong[] array3 = Nat.Create64(9);
			SecT283Field.SquareAddToExt(array, array3);
			SecT283Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat320.Create64();
			SecT283Field.Reduce(array3, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FEB RID: 8171 RVA: 0x000AE740 File Offset: 0x000AC940
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat320.Create64();
			SecT283Field.SquareN(this.x, pow, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FEC RID: 8172 RVA: 0x000AE76C File Offset: 0x000AC96C
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Invert(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x06001FED RID: 8173 RVA: 0x000AE794 File Offset: 0x000AC994
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat320.Create64();
			SecT283Field.Sqrt(this.x, z);
			return new SecT283FieldElement(z);
		}

		// Token: 0x17000355 RID: 853
		// (get) Token: 0x06001FEE RID: 8174 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000356 RID: 854
		// (get) Token: 0x06001FEF RID: 8175 RVA: 0x000AE5CF File Offset: 0x000AC7CF
		public virtual int M
		{
			get
			{
				return 283;
			}
		}

		// Token: 0x17000357 RID: 855
		// (get) Token: 0x06001FF0 RID: 8176 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K1
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000358 RID: 856
		// (get) Token: 0x06001FF1 RID: 8177 RVA: 0x000A85D4 File Offset: 0x000A67D4
		public virtual int K2
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x17000359 RID: 857
		// (get) Token: 0x06001FF2 RID: 8178 RVA: 0x000AE7BC File Offset: 0x000AC9BC
		public virtual int K3
		{
			get
			{
				return 12;
			}
		}

		// Token: 0x06001FF3 RID: 8179 RVA: 0x000AE7C0 File Offset: 0x000AC9C0
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT283FieldElement);
		}

		// Token: 0x06001FF4 RID: 8180 RVA: 0x000AE7C0 File Offset: 0x000AC9C0
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT283FieldElement);
		}

		// Token: 0x06001FF5 RID: 8181 RVA: 0x000AE7CE File Offset: 0x000AC9CE
		public virtual bool Equals(SecT283FieldElement other)
		{
			return this == other || (other != null && Nat320.Eq64(this.x, other.x));
		}

		// Token: 0x06001FF6 RID: 8182 RVA: 0x000AE7EC File Offset: 0x000AC9EC
		public override int GetHashCode()
		{
			return 2831275 ^ Arrays.GetHashCode(this.x, 0, 5);
		}

		// Token: 0x0400164A RID: 5706
		protected readonly ulong[] x;
	}
}
