using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041C RID: 1052
	public class DHParameters : ICipherParameters
	{
		// Token: 0x06002703 RID: 9987 RVA: 0x000C43B7 File Offset: 0x000C25B7
		private static int GetDefaultMParam(int lParam)
		{
			if (lParam == 0)
			{
				return 160;
			}
			return Math.Min(lParam, 160);
		}

		// Token: 0x06002704 RID: 9988 RVA: 0x000C43CD File Offset: 0x000C25CD
		public DHParameters(BigInteger p, BigInteger g) : this(p, g, null, 0)
		{
		}

		// Token: 0x06002705 RID: 9989 RVA: 0x000C43D9 File Offset: 0x000C25D9
		public DHParameters(BigInteger p, BigInteger g, BigInteger q) : this(p, g, q, 0)
		{
		}

		// Token: 0x06002706 RID: 9990 RVA: 0x000C43E5 File Offset: 0x000C25E5
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int l) : this(p, g, q, DHParameters.GetDefaultMParam(l), l, null, null)
		{
		}

		// Token: 0x06002707 RID: 9991 RVA: 0x000C43FB File Offset: 0x000C25FB
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int m, int l) : this(p, g, q, m, l, null, null)
		{
		}

		// Token: 0x06002708 RID: 9992 RVA: 0x000C440C File Offset: 0x000C260C
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, BigInteger j, DHValidationParameters validation) : this(p, g, q, 160, 0, j, validation)
		{
		}

		// Token: 0x06002709 RID: 9993 RVA: 0x000C4424 File Offset: 0x000C2624
		public DHParameters(BigInteger p, BigInteger g, BigInteger q, int m, int l, BigInteger j, DHValidationParameters validation)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			if (!p.TestBit(0))
			{
				throw new ArgumentException("field must be an odd prime", "p");
			}
			if (g.CompareTo(BigInteger.Two) < 0 || g.CompareTo(p.Subtract(BigInteger.Two)) > 0)
			{
				throw new ArgumentException("generator must in the range [2, p - 2]", "g");
			}
			if (q != null && q.BitLength >= p.BitLength)
			{
				throw new ArgumentException("q too big to be a factor of (p-1)", "q");
			}
			if (m >= p.BitLength)
			{
				throw new ArgumentException("m value must be < bitlength of p", "m");
			}
			if (l != 0)
			{
				if (l >= p.BitLength)
				{
					throw new ArgumentException("when l value specified, it must be less than bitlength(p)", "l");
				}
				if (l < m)
				{
					throw new ArgumentException("when l value specified, it may not be less than m value", "l");
				}
			}
			if (j != null && j.CompareTo(BigInteger.Two) < 0)
			{
				throw new ArgumentException("subgroup factor must be >= 2", "j");
			}
			this.p = p;
			this.g = g;
			this.q = q;
			this.m = m;
			this.l = l;
			this.j = j;
			this.validation = validation;
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x0600270A RID: 9994 RVA: 0x000C4565 File Offset: 0x000C2765
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x0600270B RID: 9995 RVA: 0x000C456D File Offset: 0x000C276D
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000461 RID: 1121
		// (get) Token: 0x0600270C RID: 9996 RVA: 0x000C4575 File Offset: 0x000C2775
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x17000462 RID: 1122
		// (get) Token: 0x0600270D RID: 9997 RVA: 0x000C457D File Offset: 0x000C277D
		public BigInteger J
		{
			get
			{
				return this.j;
			}
		}

		// Token: 0x17000463 RID: 1123
		// (get) Token: 0x0600270E RID: 9998 RVA: 0x000C4585 File Offset: 0x000C2785
		public int M
		{
			get
			{
				return this.m;
			}
		}

		// Token: 0x17000464 RID: 1124
		// (get) Token: 0x0600270F RID: 9999 RVA: 0x000C458D File Offset: 0x000C278D
		public int L
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x17000465 RID: 1125
		// (get) Token: 0x06002710 RID: 10000 RVA: 0x000C4595 File Offset: 0x000C2795
		public DHValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x06002711 RID: 10001 RVA: 0x000C45A0 File Offset: 0x000C27A0
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHParameters dhparameters = obj as DHParameters;
			return dhparameters != null && this.Equals(dhparameters);
		}

		// Token: 0x06002712 RID: 10002 RVA: 0x000C45C6 File Offset: 0x000C27C6
		protected virtual bool Equals(DHParameters other)
		{
			return this.p.Equals(other.p) && this.g.Equals(other.g) && object.Equals(this.q, other.q);
		}

		// Token: 0x06002713 RID: 10003 RVA: 0x000C4604 File Offset: 0x000C2804
		public override int GetHashCode()
		{
			int num = this.p.GetHashCode() ^ this.g.GetHashCode();
			if (this.q != null)
			{
				num ^= this.q.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001A17 RID: 6679
		private const int DefaultMinimumLength = 160;

		// Token: 0x04001A18 RID: 6680
		private readonly BigInteger p;

		// Token: 0x04001A19 RID: 6681
		private readonly BigInteger g;

		// Token: 0x04001A1A RID: 6682
		private readonly BigInteger q;

		// Token: 0x04001A1B RID: 6683
		private readonly BigInteger j;

		// Token: 0x04001A1C RID: 6684
		private readonly int m;

		// Token: 0x04001A1D RID: 6685
		private readonly int l;

		// Token: 0x04001A1E RID: 6686
		private readonly DHValidationParameters validation;
	}
}
