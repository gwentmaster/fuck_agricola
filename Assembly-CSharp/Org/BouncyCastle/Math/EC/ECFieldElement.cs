using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E0 RID: 736
	public abstract class ECFieldElement
	{
		// Token: 0x06001977 RID: 6519
		public abstract BigInteger ToBigInteger();

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x06001978 RID: 6520
		public abstract string FieldName { get; }

		// Token: 0x1700021F RID: 543
		// (get) Token: 0x06001979 RID: 6521
		public abstract int FieldSize { get; }

		// Token: 0x0600197A RID: 6522
		public abstract ECFieldElement Add(ECFieldElement b);

		// Token: 0x0600197B RID: 6523
		public abstract ECFieldElement AddOne();

		// Token: 0x0600197C RID: 6524
		public abstract ECFieldElement Subtract(ECFieldElement b);

		// Token: 0x0600197D RID: 6525
		public abstract ECFieldElement Multiply(ECFieldElement b);

		// Token: 0x0600197E RID: 6526
		public abstract ECFieldElement Divide(ECFieldElement b);

		// Token: 0x0600197F RID: 6527
		public abstract ECFieldElement Negate();

		// Token: 0x06001980 RID: 6528
		public abstract ECFieldElement Square();

		// Token: 0x06001981 RID: 6529
		public abstract ECFieldElement Invert();

		// Token: 0x06001982 RID: 6530
		public abstract ECFieldElement Sqrt();

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x06001983 RID: 6531 RVA: 0x0009426D File Offset: 0x0009246D
		public virtual int BitLength
		{
			get
			{
				return this.ToBigInteger().BitLength;
			}
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06001984 RID: 6532 RVA: 0x0009427A File Offset: 0x0009247A
		public virtual bool IsOne
		{
			get
			{
				return this.BitLength == 1;
			}
		}

		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06001985 RID: 6533 RVA: 0x00094285 File Offset: 0x00092485
		public virtual bool IsZero
		{
			get
			{
				return this.ToBigInteger().SignValue == 0;
			}
		}

		// Token: 0x06001986 RID: 6534 RVA: 0x00094295 File Offset: 0x00092495
		public virtual ECFieldElement MultiplyMinusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.Multiply(b).Subtract(x.Multiply(y));
		}

		// Token: 0x06001987 RID: 6535 RVA: 0x000942AA File Offset: 0x000924AA
		public virtual ECFieldElement MultiplyPlusProduct(ECFieldElement b, ECFieldElement x, ECFieldElement y)
		{
			return this.Multiply(b).Add(x.Multiply(y));
		}

		// Token: 0x06001988 RID: 6536 RVA: 0x000942BF File Offset: 0x000924BF
		public virtual ECFieldElement SquareMinusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.Square().Subtract(x.Multiply(y));
		}

		// Token: 0x06001989 RID: 6537 RVA: 0x000942D3 File Offset: 0x000924D3
		public virtual ECFieldElement SquarePlusProduct(ECFieldElement x, ECFieldElement y)
		{
			return this.Square().Add(x.Multiply(y));
		}

		// Token: 0x0600198A RID: 6538 RVA: 0x000942E8 File Offset: 0x000924E8
		public virtual ECFieldElement SquarePow(int pow)
		{
			ECFieldElement ecfieldElement = this;
			for (int i = 0; i < pow; i++)
			{
				ecfieldElement = ecfieldElement.Square();
			}
			return ecfieldElement;
		}

		// Token: 0x0600198B RID: 6539 RVA: 0x0009430B File Offset: 0x0009250B
		public virtual bool TestBitZero()
		{
			return this.ToBigInteger().TestBit(0);
		}

		// Token: 0x0600198C RID: 6540 RVA: 0x00094319 File Offset: 0x00092519
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECFieldElement);
		}

		// Token: 0x0600198D RID: 6541 RVA: 0x00094327 File Offset: 0x00092527
		public virtual bool Equals(ECFieldElement other)
		{
			return this == other || (other != null && this.ToBigInteger().Equals(other.ToBigInteger()));
		}

		// Token: 0x0600198E RID: 6542 RVA: 0x00094345 File Offset: 0x00092545
		public override int GetHashCode()
		{
			return this.ToBigInteger().GetHashCode();
		}

		// Token: 0x0600198F RID: 6543 RVA: 0x00094352 File Offset: 0x00092552
		public override string ToString()
		{
			return this.ToBigInteger().ToString(16);
		}

		// Token: 0x06001990 RID: 6544 RVA: 0x00094361 File Offset: 0x00092561
		public virtual byte[] GetEncoded()
		{
			return BigIntegers.AsUnsignedByteArray((this.FieldSize + 7) / 8, this.ToBigInteger());
		}
	}
}
