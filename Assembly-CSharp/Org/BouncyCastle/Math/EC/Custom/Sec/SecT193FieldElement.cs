using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200033F RID: 831
	internal class SecT193FieldElement : ECFieldElement
	{
		// Token: 0x06001EC0 RID: 7872 RVA: 0x000AA12F File Offset: 0x000A832F
		public SecT193FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 193)
			{
				throw new ArgumentException("value invalid for SecT193FieldElement", "x");
			}
			this.x = SecT193Field.FromBigInteger(x);
		}

		// Token: 0x06001EC1 RID: 7873 RVA: 0x000AA16C File Offset: 0x000A836C
		public SecT193FieldElement()
		{
			this.x = Nat256.Create64();
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000AA17F File Offset: 0x000A837F
		protected internal SecT193FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x17000304 RID: 772
		// (get) Token: 0x06001EC3 RID: 7875 RVA: 0x000AA18E File Offset: 0x000A838E
		public override bool IsOne
		{
			get
			{
				return Nat256.IsOne64(this.x);
			}
		}

		// Token: 0x17000305 RID: 773
		// (get) Token: 0x06001EC4 RID: 7876 RVA: 0x000AA19B File Offset: 0x000A839B
		public override bool IsZero
		{
			get
			{
				return Nat256.IsZero64(this.x);
			}
		}

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000AA1A8 File Offset: 0x000A83A8
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000AA1B9 File Offset: 0x000A83B9
		public override BigInteger ToBigInteger()
		{
			return Nat256.ToBigInteger64(this.x);
		}

		// Token: 0x17000306 RID: 774
		// (get) Token: 0x06001EC7 RID: 7879 RVA: 0x000AA1C6 File Offset: 0x000A83C6
		public override string FieldName
		{
			get
			{
				return "SecT193Field";
			}
		}

		// Token: 0x17000307 RID: 775
		// (get) Token: 0x06001EC8 RID: 7880 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public override int FieldSize
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000AA1D4 File Offset: 0x000A83D4
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Add(this.x, ((SecT193FieldElement)b).x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000AA204 File Offset: 0x000A8404
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.AddOne(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ECB RID: 7883 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x06001ECC RID: 7884 RVA: 0x000AA22C File Offset: 0x000A842C
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Multiply(this.x, ((SecT193FieldElement)b).x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ECD RID: 7885 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x06001ECE RID: 7886 RVA: 0x000AA25C File Offset: 0x000A845C
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT193FieldElement)b).x;
			ulong[] array2 = ((SecT193FieldElement)x).x;
			ulong[] y3 = ((SecT193FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT193Field.MultiplyAddToExt(array, y2, array3);
			SecT193Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat256.Create64();
			SecT193Field.Reduce(array3, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ECF RID: 7887 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x06001ED0 RID: 7888 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x06001ED1 RID: 7889 RVA: 0x000AA2C0 File Offset: 0x000A84C0
		public override ECFieldElement Square()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Square(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ED2 RID: 7890 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x06001ED3 RID: 7891 RVA: 0x000AA2E8 File Offset: 0x000A84E8
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT193FieldElement)x).x;
			ulong[] y2 = ((SecT193FieldElement)y).x;
			ulong[] array3 = Nat256.CreateExt64();
			SecT193Field.SquareAddToExt(array, array3);
			SecT193Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat256.Create64();
			SecT193Field.Reduce(array3, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ED4 RID: 7892 RVA: 0x000AA33C File Offset: 0x000A853C
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat256.Create64();
			SecT193Field.SquareN(this.x, pow, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ED5 RID: 7893 RVA: 0x000AA368 File Offset: 0x000A8568
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Invert(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x06001ED6 RID: 7894 RVA: 0x000AA390 File Offset: 0x000A8590
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat256.Create64();
			SecT193Field.Sqrt(this.x, z);
			return new SecT193FieldElement(z);
		}

		// Token: 0x17000308 RID: 776
		// (get) Token: 0x06001ED7 RID: 7895 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int Representation
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000309 RID: 777
		// (get) Token: 0x06001ED8 RID: 7896 RVA: 0x000AA1CD File Offset: 0x000A83CD
		public virtual int M
		{
			get
			{
				return 193;
			}
		}

		// Token: 0x1700030A RID: 778
		// (get) Token: 0x06001ED9 RID: 7897 RVA: 0x000AA3B5 File Offset: 0x000A85B5
		public virtual int K1
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x1700030B RID: 779
		// (get) Token: 0x06001EDA RID: 7898 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K2
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x1700030C RID: 780
		// (get) Token: 0x06001EDB RID: 7899 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual int K3
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x06001EDC RID: 7900 RVA: 0x000AA3B9 File Offset: 0x000A85B9
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT193FieldElement);
		}

		// Token: 0x06001EDD RID: 7901 RVA: 0x000AA3B9 File Offset: 0x000A85B9
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT193FieldElement);
		}

		// Token: 0x06001EDE RID: 7902 RVA: 0x000AA3C7 File Offset: 0x000A85C7
		public virtual bool Equals(SecT193FieldElement other)
		{
			return this == other || (other != null && Nat256.Eq64(this.x, other.x));
		}

		// Token: 0x06001EDF RID: 7903 RVA: 0x000AA3E5 File Offset: 0x000A85E5
		public override int GetHashCode()
		{
			return 1930015 ^ Arrays.GetHashCode(this.x, 0, 4);
		}

		// Token: 0x04001636 RID: 5686
		protected readonly ulong[] x;
	}
}
