using System;
using System.Text;

namespace Org.BouncyCastle.Math.EC.Abc
{
	// Token: 0x02000364 RID: 868
	internal class SimpleBigDecimal
	{
		// Token: 0x06002137 RID: 8503 RVA: 0x000B34A4 File Offset: 0x000B16A4
		public static SimpleBigDecimal GetInstance(BigInteger val, int scale)
		{
			return new SimpleBigDecimal(val.ShiftLeft(scale), scale);
		}

		// Token: 0x06002138 RID: 8504 RVA: 0x000B34B3 File Offset: 0x000B16B3
		public SimpleBigDecimal(BigInteger bigInt, int scale)
		{
			if (scale < 0)
			{
				throw new ArgumentException("scale may not be negative");
			}
			this.bigInt = bigInt;
			this.scale = scale;
		}

		// Token: 0x06002139 RID: 8505 RVA: 0x000B34D8 File Offset: 0x000B16D8
		private SimpleBigDecimal(SimpleBigDecimal limBigDec)
		{
			this.bigInt = limBigDec.bigInt;
			this.scale = limBigDec.scale;
		}

		// Token: 0x0600213A RID: 8506 RVA: 0x000B34F8 File Offset: 0x000B16F8
		private void CheckScale(SimpleBigDecimal b)
		{
			if (this.scale != b.scale)
			{
				throw new ArgumentException("Only SimpleBigDecimal of same scale allowed in arithmetic operations");
			}
		}

		// Token: 0x0600213B RID: 8507 RVA: 0x000B3513 File Offset: 0x000B1713
		public SimpleBigDecimal AdjustScale(int newScale)
		{
			if (newScale < 0)
			{
				throw new ArgumentException("scale may not be negative");
			}
			if (newScale == this.scale)
			{
				return this;
			}
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(newScale - this.scale), newScale);
		}

		// Token: 0x0600213C RID: 8508 RVA: 0x000B3548 File Offset: 0x000B1748
		public SimpleBigDecimal Add(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.Add(b.bigInt), this.scale);
		}

		// Token: 0x0600213D RID: 8509 RVA: 0x000B356D File Offset: 0x000B176D
		public SimpleBigDecimal Add(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Add(b.ShiftLeft(this.scale)), this.scale);
		}

		// Token: 0x0600213E RID: 8510 RVA: 0x000B3591 File Offset: 0x000B1791
		public SimpleBigDecimal Negate()
		{
			return new SimpleBigDecimal(this.bigInt.Negate(), this.scale);
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000B35A9 File Offset: 0x000B17A9
		public SimpleBigDecimal Subtract(SimpleBigDecimal b)
		{
			return this.Add(b.Negate());
		}

		// Token: 0x06002140 RID: 8512 RVA: 0x000B35B7 File Offset: 0x000B17B7
		public SimpleBigDecimal Subtract(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Subtract(b.ShiftLeft(this.scale)), this.scale);
		}

		// Token: 0x06002141 RID: 8513 RVA: 0x000B35DB File Offset: 0x000B17DB
		public SimpleBigDecimal Multiply(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.Multiply(b.bigInt), this.scale + this.scale);
		}

		// Token: 0x06002142 RID: 8514 RVA: 0x000B3607 File Offset: 0x000B1807
		public SimpleBigDecimal Multiply(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Multiply(b), this.scale);
		}

		// Token: 0x06002143 RID: 8515 RVA: 0x000B3620 File Offset: 0x000B1820
		public SimpleBigDecimal Divide(SimpleBigDecimal b)
		{
			this.CheckScale(b);
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(this.scale).Divide(b.bigInt), this.scale);
		}

		// Token: 0x06002144 RID: 8516 RVA: 0x000B3650 File Offset: 0x000B1850
		public SimpleBigDecimal Divide(BigInteger b)
		{
			return new SimpleBigDecimal(this.bigInt.Divide(b), this.scale);
		}

		// Token: 0x06002145 RID: 8517 RVA: 0x000B3669 File Offset: 0x000B1869
		public SimpleBigDecimal ShiftLeft(int n)
		{
			return new SimpleBigDecimal(this.bigInt.ShiftLeft(n), this.scale);
		}

		// Token: 0x06002146 RID: 8518 RVA: 0x000B3682 File Offset: 0x000B1882
		public int CompareTo(SimpleBigDecimal val)
		{
			this.CheckScale(val);
			return this.bigInt.CompareTo(val.bigInt);
		}

		// Token: 0x06002147 RID: 8519 RVA: 0x000B369C File Offset: 0x000B189C
		public int CompareTo(BigInteger val)
		{
			return this.bigInt.CompareTo(val.ShiftLeft(this.scale));
		}

		// Token: 0x06002148 RID: 8520 RVA: 0x000B36B5 File Offset: 0x000B18B5
		public BigInteger Floor()
		{
			return this.bigInt.ShiftRight(this.scale);
		}

		// Token: 0x06002149 RID: 8521 RVA: 0x000B36C8 File Offset: 0x000B18C8
		public BigInteger Round()
		{
			SimpleBigDecimal simpleBigDecimal = new SimpleBigDecimal(BigInteger.One, 1);
			return this.Add(simpleBigDecimal.AdjustScale(this.scale)).Floor();
		}

		// Token: 0x170003AF RID: 943
		// (get) Token: 0x0600214A RID: 8522 RVA: 0x000B36F8 File Offset: 0x000B18F8
		public int IntValue
		{
			get
			{
				return this.Floor().IntValue;
			}
		}

		// Token: 0x170003B0 RID: 944
		// (get) Token: 0x0600214B RID: 8523 RVA: 0x000B3705 File Offset: 0x000B1905
		public long LongValue
		{
			get
			{
				return this.Floor().LongValue;
			}
		}

		// Token: 0x170003B1 RID: 945
		// (get) Token: 0x0600214C RID: 8524 RVA: 0x000B3712 File Offset: 0x000B1912
		public int Scale
		{
			get
			{
				return this.scale;
			}
		}

		// Token: 0x0600214D RID: 8525 RVA: 0x000B371C File Offset: 0x000B191C
		public override string ToString()
		{
			if (this.scale == 0)
			{
				return this.bigInt.ToString();
			}
			BigInteger bigInteger = this.Floor();
			BigInteger bigInteger2 = this.bigInt.Subtract(bigInteger.ShiftLeft(this.scale));
			if (this.bigInt.SignValue < 0)
			{
				bigInteger2 = BigInteger.One.ShiftLeft(this.scale).Subtract(bigInteger2);
			}
			if (bigInteger.SignValue == -1 && !bigInteger2.Equals(BigInteger.Zero))
			{
				bigInteger = bigInteger.Add(BigInteger.One);
			}
			string value = bigInteger.ToString();
			char[] array = new char[this.scale];
			string text = bigInteger2.ToString(2);
			int length = text.Length;
			int num = this.scale - length;
			for (int i = 0; i < num; i++)
			{
				array[i] = '0';
			}
			for (int j = 0; j < length; j++)
			{
				array[num + j] = text[j];
			}
			string value2 = new string(array);
			StringBuilder stringBuilder = new StringBuilder(value);
			stringBuilder.Append(".");
			stringBuilder.Append(value2);
			return stringBuilder.ToString();
		}

		// Token: 0x0600214E RID: 8526 RVA: 0x000B3834 File Offset: 0x000B1A34
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			SimpleBigDecimal simpleBigDecimal = obj as SimpleBigDecimal;
			return simpleBigDecimal != null && this.bigInt.Equals(simpleBigDecimal.bigInt) && this.scale == simpleBigDecimal.scale;
		}

		// Token: 0x0600214F RID: 8527 RVA: 0x000B3876 File Offset: 0x000B1A76
		public override int GetHashCode()
		{
			return this.bigInt.GetHashCode() ^ this.scale;
		}

		// Token: 0x0400166A RID: 5738
		private readonly BigInteger bigInt;

		// Token: 0x0400166B RID: 5739
		private readonly int scale;
	}
}
