using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000345 RID: 837
	internal class SecT233FieldElement : ECFieldElement
	{
		// Token: 0x06001F24 RID: 7972 RVA: 0x000AB9D3 File Offset: 0x000A9BD3
		public SecT233FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 233)
			{
				throw new ArgumentException("value invalid for SecT233FieldElement", "x");
			}
			this.x = SecT233Field.FromBigInteger(x);
		}

		// Token: 0x06001F25 RID: 7973 RVA: 0x000ABA10 File Offset: 0x000A9C10
		public SecT233FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x06001F26 RID: 7974 RVA: 0x000ABA23 File Offset: 0x000A9C23
		protected internal SecT233FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x17000321 RID: 801
		// (get) Token: 0x06001F27 RID: 7975 RVA: 0x000ABA32 File Offset: 0x000A9C32
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x17000322 RID: 802
		// (get) Token: 0x06001F28 RID: 7976 RVA: 0x000ABA3F File Offset: 0x000A9C3F
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x06001F29 RID: 7977 RVA: 0x000ABA4C File Offset: 0x000A9C4C
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001F2A RID: 7978 RVA: 0x000ABA5D File Offset: 0x000A9C5D
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x17000323 RID: 803
		// (get) Token: 0x06001F2B RID: 7979 RVA: 0x000ABA6A File Offset: 0x000A9C6A
		public override string FieldName
		{
			get
			{
				return "SecT233Field";
			}
		}

		// Token: 0x17000324 RID: 804
		// (get) Token: 0x06001F2C RID: 7980 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public override int FieldSize
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x06001F2D RID: 7981 RVA: 0x000ABA78 File Offset: 0x000A9C78
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Add(this.x, ((SecT233FieldElement)b).x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F2E RID: 7982 RVA: 0x000ABAA8 File Offset: 0x000A9CA8
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.AddOne(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F2F RID: 7983 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001F30 RID: 7984 RVA: 0x000ABAD0 File Offset: 0x000A9CD0
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Multiply(this.x, ((SecT233FieldElement)b).x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F31 RID: 7985 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001F32 RID: 7986 RVA: 0x000ABB00 File Offset: 0x000A9D00
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT233FieldElement)b).x;
			ulong[] array2 = ((SecT233FieldElement)x).x;
			ulong[] y3 = ((SecT233FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT233Field.MultiplyAddToExt(array, y2, array3);
			SecT233Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT233Field.Reduce(array3, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F33 RID: 7987 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001F34 RID: 7988 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001F35 RID: 7989 RVA: 0x000ABB64 File Offset: 0x000A9D64
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Square(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F36 RID: 7990 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001F37 RID: 7991 RVA: 0x000ABB8C File Offset: 0x000A9D8C
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT233FieldElement)x).x;
			ulong[] y2 = ((SecT233FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT233Field.SquareAddToExt(array, array3);
			SecT233Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT233Field.Reduce(array3, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F38 RID: 7992 RVA: 0x000ABBE0 File Offset: 0x000A9DE0
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT233Field.SquareN(this.x, pow, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F39 RID: 7993 RVA: 0x000ABC0C File Offset: 0x000A9E0C
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Invert(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x06001F3A RID: 7994 RVA: 0x000ABC34 File Offset: 0x000A9E34
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT233Field.Sqrt(this.x, z);
			return new SecT233FieldElement(z);
		}

		// Token: 0x17000325 RID: 805
		// (get) Token: 0x06001F3B RID: 7995 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000326 RID: 806
		// (get) Token: 0x06001F3C RID: 7996 RVA: 0x000ABA71 File Offset: 0x000A9C71
		public virtual int M
		{
			get
			{
				return 233;
			}
		}

		// Token: 0x17000327 RID: 807
		// (get) Token: 0x06001F3D RID: 7997 RVA: 0x000ABC59 File Offset: 0x000A9E59
		public virtual int K1
		{
			get
			{
				return 74;
			}
		}

		// Token: 0x17000328 RID: 808
		// (get) Token: 0x06001F3E RID: 7998 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000329 RID: 809
		// (get) Token: 0x06001F3F RID: 7999 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001F40 RID: 8000 RVA: 0x000ABC5D File Offset: 0x000A9E5D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT233FieldElement);
		}

		// Token: 0x06001F41 RID: 8001 RVA: 0x000ABC5D File Offset: 0x000A9E5D
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT233FieldElement);
		}

		// Token: 0x06001F42 RID: 8002 RVA: 0x000ABC6B File Offset: 0x000A9E6B
		public virtual bool Equals(SecT233FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x06001F43 RID: 8003 RVA: 0x000ABC89 File Offset: 0x000A9E89
		public override int GetHashCode()
		{
			return 2330074 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x0400163D RID: 5693
		protected readonly ulong[] x;
	}
}
