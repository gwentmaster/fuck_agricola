using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200034B RID: 843
	internal class SecT239FieldElement : ECFieldElement
	{
		// Token: 0x06001F89 RID: 8073 RVA: 0x000AD257 File Offset: 0x000AB457
		public SecT239FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 239)
			{
				throw new ArgumentException("value invalid for SecT239FieldElement", "x");
			}
			this.x = SecT239Field.FromBigInteger(x);
		}

		// Token: 0x06001F8A RID: 8074 RVA: 0x000AD294 File Offset: 0x000AB494
		public SecT239FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x06001F8B RID: 8075 RVA: 0x000AD2A7 File Offset: 0x000AB4A7
		protected internal SecT239FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700033E RID: 830
		// (get) Token: 0x06001F8C RID: 8076 RVA: 0x000AD2B6 File Offset: 0x000AB4B6
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x1700033F RID: 831
		// (get) Token: 0x06001F8D RID: 8077 RVA: 0x000AD2C3 File Offset: 0x000AB4C3
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x06001F8E RID: 8078 RVA: 0x000AD2D0 File Offset: 0x000AB4D0
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001F8F RID: 8079 RVA: 0x000AD2E1 File Offset: 0x000AB4E1
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x17000340 RID: 832
		// (get) Token: 0x06001F90 RID: 8080 RVA: 0x000AD2EE File Offset: 0x000AB4EE
		public override string FieldName
		{
			get
			{
				return "SecT239Field";
			}
		}

		// Token: 0x17000341 RID: 833
		// (get) Token: 0x06001F91 RID: 8081 RVA: 0x000AD2F5 File Offset: 0x000AB4F5
		public override int FieldSize
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x06001F92 RID: 8082 RVA: 0x000AD2FC File Offset: 0x000AB4FC
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Add(this.x, ((SecT239FieldElement)b).x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F93 RID: 8083 RVA: 0x000AD32C File Offset: 0x000AB52C
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.AddOne(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F94 RID: 8084 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001F95 RID: 8085 RVA: 0x000AD354 File Offset: 0x000AB554
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Multiply(this.x, ((SecT239FieldElement)b).x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F96 RID: 8086 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001F97 RID: 8087 RVA: 0x000AD384 File Offset: 0x000AB584
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT239FieldElement)b).x;
			ulong[] array2 = ((SecT239FieldElement)x).x;
			ulong[] y3 = ((SecT239FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT239Field.MultiplyAddToExt(array, y2, array3);
			SecT239Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT239Field.Reduce(array3, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F98 RID: 8088 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001F99 RID: 8089 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001F9A RID: 8090 RVA: 0x000AD3E8 File Offset: 0x000AB5E8
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Square(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F9B RID: 8091 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001F9C RID: 8092 RVA: 0x000AD410 File Offset: 0x000AB610
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT239FieldElement)x).x;
			ulong[] y2 = ((SecT239FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT239Field.SquareAddToExt(array, array3);
			SecT239Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT239Field.Reduce(array3, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F9D RID: 8093 RVA: 0x000AD464 File Offset: 0x000AB664
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT239Field.SquareN(this.x, pow, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F9E RID: 8094 RVA: 0x000AD490 File Offset: 0x000AB690
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Invert(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x06001F9F RID: 8095 RVA: 0x000AD4B8 File Offset: 0x000AB6B8
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT239Field.Sqrt(this.x, z);
			return new SecT239FieldElement(z);
		}

		// Token: 0x17000342 RID: 834
		// (get) Token: 0x06001FA0 RID: 8096 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000343 RID: 835
		// (get) Token: 0x06001FA1 RID: 8097 RVA: 0x000AD2F5 File Offset: 0x000AB4F5
		public virtual int M
		{
			get
			{
				return 239;
			}
		}

		// Token: 0x17000344 RID: 836
		// (get) Token: 0x06001FA2 RID: 8098 RVA: 0x000AD4DD File Offset: 0x000AB6DD
		public virtual int K1
		{
			get
			{
				return 158;
			}
		}

		// Token: 0x17000345 RID: 837
		// (get) Token: 0x06001FA3 RID: 8099 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000346 RID: 838
		// (get) Token: 0x06001FA4 RID: 8100 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001FA5 RID: 8101 RVA: 0x000AD4E4 File Offset: 0x000AB6E4
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT239FieldElement);
		}

		// Token: 0x06001FA6 RID: 8102 RVA: 0x000AD4E4 File Offset: 0x000AB6E4
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT239FieldElement);
		}

		// Token: 0x06001FA7 RID: 8103 RVA: 0x000AD4F2 File Offset: 0x000AB6F2
		public virtual bool Equals(SecT239FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x06001FA8 RID: 8104 RVA: 0x000AD510 File Offset: 0x000AB710
		public override int GetHashCode()
		{
			return 23900158 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001644 RID: 5700
		protected ulong[] x;
	}
}
