using System;
using Org.BouncyCastle.Math.Raw;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200035B RID: 859
	internal class SecT571FieldElement : ECFieldElement
	{
		// Token: 0x060020A2 RID: 8354 RVA: 0x000B14A3 File Offset: 0x000AF6A3
		public SecT571FieldElement(BigInteger x)
		{
			if (x == null || x.SignValue < 0 || x.BitLength > 571)
			{
				throw new ArgumentException("value invalid for SecT571FieldElement", "x");
			}
			this.x = SecT571Field.FromBigInteger(x);
		}

		// Token: 0x060020A3 RID: 8355 RVA: 0x000B14E0 File Offset: 0x000AF6E0
		public SecT571FieldElement()
		{
			this.x = Nat576.Create64();
		}

		// Token: 0x060020A4 RID: 8356 RVA: 0x000B14F3 File Offset: 0x000AF6F3
		protected internal SecT571FieldElement(ulong[] x)
		{
			this.x = x;
		}

		// Token: 0x1700038B RID: 907
		// (get) Token: 0x060020A5 RID: 8357 RVA: 0x000B1502 File Offset: 0x000AF702
		public override bool IsOne
		{
			get
			{
				return Nat576.IsOne64(this.x);
			}
		}

		// Token: 0x1700038C RID: 908
		// (get) Token: 0x060020A6 RID: 8358 RVA: 0x000B150F File Offset: 0x000AF70F
		public override bool IsZero
		{
			get
			{
				return Nat576.IsZero64(this.x);
			}
		}

		// Token: 0x060020A7 RID: 8359 RVA: 0x000B151C File Offset: 0x000AF71C
		public override bool TestBitZero()
		{
			return (this.x[0] & 1UL) > 0UL;
		}

		// Token: 0x060020A8 RID: 8360 RVA: 0x000B152D File Offset: 0x000AF72D
		public override BigInteger ToBigInteger()
		{
			return Nat576.ToBigInteger64(this.x);
		}

		// Token: 0x1700038D RID: 909
		// (get) Token: 0x060020A9 RID: 8361 RVA: 0x000B153A File Offset: 0x000AF73A
		public override string FieldName
		{
			get
			{
				return "SecT571Field";
			}
		}

		// Token: 0x1700038E RID: 910
		// (get) Token: 0x060020AA RID: 8362 RVA: 0x000B1541 File Offset: 0x000AF741
		public override int FieldSize
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x060020AB RID: 8363 RVA: 0x000B1548 File Offset: 0x000AF748
		public override ECFieldElement Add(ECFieldElement b)
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Add(this.x, ((SecT571FieldElement)b).x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020AC RID: 8364 RVA: 0x000B1578 File Offset: 0x000AF778
		public override ECFieldElement AddOne()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.AddOne(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020AD RID: 8365 RVA: 0x00095095 File Offset: 0x00093295
		public override ECFieldElement Subtract(ECFieldElement b)
		{
			return this.Add(b);
		}

		// Token: 0x060020AE RID: 8366 RVA: 0x000B15A0 File Offset: 0x000AF7A0
		public override ECFieldElement Multiply(ECFieldElement b)
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Multiply(this.x, ((SecT571FieldElement)b).x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020AF RID: 8367 RVA: 0x000950D3 File Offset: 0x000932D3
		public override ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.MultiplyPlusProduct(b, x, y);
		}

		// Token: 0x060020B0 RID: 8368 RVA: 0x000B15D0 File Offset: 0x000AF7D0
		public override ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] y2 = ((SecT571FieldElement)b).x;
			ulong[] array2 = ((SecT571FieldElement)x).x;
			ulong[] y3 = ((SecT571FieldElement)y).x;
			ulong[] array3 = Nat576.CreateExt64();
			SecT571Field.MultiplyAddToExt(array, y2, array3);
			SecT571Field.MultiplyAddToExt(array2, y3, array3);
			ulong[] z = Nat576.Create64();
			SecT571Field.Reduce(array3, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020B1 RID: 8369 RVA: 0x000A5216 File Offset: 0x000A3416
		public override ECFieldElement Divide(ECFieldElement b)
		{
			return this.Multiply(b.Invert());
		}

		// Token: 0x060020B2 RID: 8370 RVA: 0x00035D67 File Offset: 0x00033F67
		public override ECFieldElement Negate()
		{
			return this;
		}

		// Token: 0x060020B3 RID: 8371 RVA: 0x000B1634 File Offset: 0x000AF834
		public override ECFieldElement Square()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Square(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020B4 RID: 8372 RVA: 0x000951C1 File Offset: 0x000933C1
		public override ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.SquarePlusProduct(x, y);
		}

		// Token: 0x060020B5 RID: 8373 RVA: 0x000B165C File Offset: 0x000AF85C
		public override ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			ulong[] array = this.x;
			ulong[] array2 = ((SecT571FieldElement)x).x;
			ulong[] y2 = ((SecT571FieldElement)y).x;
			ulong[] array3 = Nat576.CreateExt64();
			SecT571Field.SquareAddToExt(array, array3);
			SecT571Field.MultiplyAddToExt(array2, y2, array3);
			ulong[] z = Nat576.Create64();
			SecT571Field.Reduce(array3, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020B6 RID: 8374 RVA: 0x000B16B0 File Offset: 0x000AF8B0
		public override ECFieldElement SquarePow(int pow)
		{
			if (pow < 1)
			{
				return this;
			}
			ulong[] z = Nat576.Create64();
			SecT571Field.SquareN(this.x, pow, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020B7 RID: 8375 RVA: 0x000B16DC File Offset: 0x000AF8DC
		public override ECFieldElement Invert()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Invert(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x060020B8 RID: 8376 RVA: 0x000B1704 File Offset: 0x000AF904
		public override ECFieldElement Sqrt()
		{
			ulong[] z = Nat576.Create64();
			SecT571Field.Sqrt(this.x, z);
			return new SecT571FieldElement(z);
		}

		// Token: 0x1700038F RID: 911
		// (get) Token: 0x060020B9 RID: 8377 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		public virtual int Representation
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000390 RID: 912
		// (get) Token: 0x060020BA RID: 8378 RVA: 0x000B1541 File Offset: 0x000AF741
		public virtual int M
		{
			get
			{
				return 571;
			}
		}

		// Token: 0x17000391 RID: 913
		// (get) Token: 0x060020BB RID: 8379 RVA: 0x000A5319 File Offset: 0x000A3519
		public virtual int K1
		{
			get
			{
				return 2;
			}
		}

		// Token: 0x17000392 RID: 914
		// (get) Token: 0x060020BC RID: 8380 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		public virtual int K2
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x17000393 RID: 915
		// (get) Token: 0x060020BD RID: 8381 RVA: 0x000B1729 File Offset: 0x000AF929
		public virtual int K3
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x060020BE RID: 8382 RVA: 0x000B172D File Offset: 0x000AF92D
		public override bool Equals(object obj)
		{
			return this.Equals(obj as SecT571FieldElement);
		}

		// Token: 0x060020BF RID: 8383 RVA: 0x000B172D File Offset: 0x000AF92D
		public override bool Equals(ECFieldElement other)
		{
			return this.Equals(other as SecT571FieldElement);
		}

		// Token: 0x060020C0 RID: 8384 RVA: 0x000B173B File Offset: 0x000AF93B
		public virtual bool Equals(SecT571FieldElement other)
		{
			return this == other || (other != null && Nat576.Eq64(this.x, other.x));
		}

		// Token: 0x060020C1 RID: 8385 RVA: 0x000B1759 File Offset: 0x000AF959
		public override int GetHashCode()
		{
			return 5711052 ^ Arrays.GetHashCode(this.x, 0, 9);
		}

		// Token: 0x04001659 RID: 5721
		protected readonly ulong[] x;
	}
}
