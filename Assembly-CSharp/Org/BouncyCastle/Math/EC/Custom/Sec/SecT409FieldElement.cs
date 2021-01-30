using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000355 RID: 853
	internal class SecT409FieldElement : ECFieldElement
	{
		// Token: 0x0600203C RID: 8252 RVA: 0x000AFD86 File Offset: 0x000ADF86
		public SecT409FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 409)
			{
				throw new ArgumentException("value invalid for SecT409FieldElement", "x");
			}
			this.x = SecT409Field.FromBigInteger(x);
		}

		// Token: 0x0600203D RID: 8253 RVA: 0x000AFDC3 File Offset: 0x000ADFC3
		public SecT409FieldElement()
		{
			this.x = Nat448.Create64();
		}

		// Token: 0x0600203E RID: 8254 RVA: 0x000AFDD6 File Offset: 0x000ADFD6
		protected internal SecT409FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700036E RID: 878
		// (get) Token: 0x0600203F RID: 8255 RVA: 0x000AFDE5 File Offset: 0x000ADFE5
		public override bool IsOne
		{
			get
			{
				return Nat448.IsOne64(this.x);
			}
		}

		// Token: 0x1700036F RID: 879
		// (get) Token: 0x06002040 RID: 8256 RVA: 0x000AFDF2 File Offset: 0x000ADFF2
		public override bool IsZero
		{
			get
			{
				return Nat448.IsZero64(this.x);
			}
		}

		// Token: 0x06002041 RID: 8257 RVA: 0x000AFDFF File Offset: 0x000ADFFF
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06002042 RID: 8258 RVA: 0x000AFE10 File Offset: 0x000AE010
		public override BigInteger ToBigInteger()
		{
			return Nat448.ToBigInteger64(this.x);
		}

		// Token: 0x17000370 RID: 880
		// (get) Token: 0x06002043 RID: 8259 RVA: 0x000AFE1D File Offset: 0x000AE01D
		public override string FieldName
		{
			get
			{
				return "SecT409Field";
			}
		}

		// Token: 0x17000371 RID: 881
		// (get) Token: 0x06002044 RID: 8260 RVA: 0x000AFE24 File Offset: 0x000AE024
		public override int FieldSize
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x06002045 RID: 8261 RVA: 0x000AFE2C File Offset: 0x000AE02C
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Add(this.x, ((SecT409FieldElement)b).x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002046 RID: 8262 RVA: 0x000AFE5C File Offset: 0x000AE05C
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.AddOne(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002047 RID: 8263 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06002048 RID: 8264 RVA: 0x000AFE84 File Offset: 0x000AE084
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Multiply(this.x, ((SecT409FieldElement)b).x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002049 RID: 8265 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x0600204A RID: 8266 RVA: 0x000AFEB4 File Offset: 0x000AE0B4
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT409FieldElement)b).x;
			ulong[] array2 = ((SecT409FieldElement)x).x;
			ulong[] y3 = ((SecT409FieldElement)y).x;
			ulong[] array3 = Nat.Create64(13);
			SecT409Field.MultiplyAddToExt(array, y2, array3);
			SecT409Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat448.Create64();
			SecT409Field.Reduce(array3, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x0600204B RID: 8267 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x0600204C RID: 8268 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x0600204D RID: 8269 RVA: 0x000AFF18 File Offset: 0x000AE118
		public override ECFieldElement Square()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Square(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x0600204E RID: 8270 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x0600204F RID: 8271 RVA: 0x000AFF40 File Offset: 0x000AE140
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT409FieldElement)x).x;
			ulong[] y2 = ((SecT409FieldElement)y).x;
			ulong[] array3 = Nat.Create64(13);
			SecT409Field.SquareAddToExt(array, array3);
			SecT409Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat448.Create64();
			SecT409Field.Reduce(array3, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002050 RID: 8272 RVA: 0x000AFF94 File Offset: 0x000AE194
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat448.Create64();
			SecT409Field.SquareN(this.x, pow, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002051 RID: 8273 RVA: 0x000AFFC0 File Offset: 0x000AE1C0
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Invert(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x06002052 RID: 8274 RVA: 0x000AFFE8 File Offset: 0x000AE1E8
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat448.Create64();
			SecT409Field.Sqrt(this.x, z);
			return new SecT409FieldElement(z);
		}

		// Token: 0x17000372 RID: 882
		// (get) Token: 0x06002053 RID: 8275 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000373 RID: 883
		// (get) Token: 0x06002054 RID: 8276 RVA: 0x000AFE24 File Offset: 0x000AE024
		public virtual int M
		{
			get
			{
				return 409;
			}
		}

		// Token: 0x17000374 RID: 884
		// (get) Token: 0x06002055 RID: 8277 RVA: 0x000B000D File Offset: 0x000AE20D
		public virtual int K1
		{
			get
			{
				return 87;
			}
		}

		// Token: 0x17000375 RID: 885
		// (get) Token: 0x06002056 RID: 8278 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x17000376 RID: 886
		// (get) Token: 0x06002057 RID: 8279 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06002058 RID: 8280 RVA: 0x000B0011 File Offset: 0x000AE211
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT409FieldElement);
		}

		// Token: 0x06002059 RID: 8281 RVA: 0x000B0011 File Offset: 0x000AE211
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT409FieldElement);
		}

		// Token: 0x0600205A RID: 8282 RVA: 0x000B001F File Offset: 0x000AE21F
		public virtual bool Equals(SecT409FieldElement other)
		{
			return this == other || (other != null && Nat448.Eq64(this.x, other.x));
		}

		// Token: 0x0600205B RID: 8283 RVA: 0x000B003D File Offset: 0x000AE23D
		public override int GetHashCode()
		{
			return 4090087 ^ Arrays.GetHashCode(this.x, 0, 7);
		}

		// Token: 0x04001651 RID: 5713
		protected ulong[] x;
	}
}
