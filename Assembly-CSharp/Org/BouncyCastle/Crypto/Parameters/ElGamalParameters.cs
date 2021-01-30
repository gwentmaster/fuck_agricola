using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042F RID: 1071
	public class ElGamalParameters : ICipherParameters
	{
		// Token: 0x0600278C RID: 10124 RVA: 0x000C5497 File Offset: 0x000C3697
		public ElGamalParameters(BigInteger p, BigInteger g) : this(p, g, 0)
		{
		}

		// Token: 0x0600278D RID: 10125 RVA: 0x000C54A2 File Offset: 0x000C36A2
		public ElGamalParameters(BigInteger p, BigInteger g, int l)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			this.p = p;
			this.g = g;
			this.l = l;
		}

		// Token: 0x17000480 RID: 1152
		// (get) Token: 0x0600278E RID: 10126 RVA: 0x000C54DB File Offset: 0x000C36DB
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000481 RID: 1153
		// (get) Token: 0x0600278F RID: 10127 RVA: 0x000C54E3 File Offset: 0x000C36E3
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x17000482 RID: 1154
		// (get) Token: 0x06002790 RID: 10128 RVA: 0x000C54EB File Offset: 0x000C36EB
		public int L
		{
			get
			{
				return this.l;
			}
		}

		// Token: 0x06002791 RID: 10129 RVA: 0x000C54F4 File Offset: 0x000C36F4
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ElGamalParameters elGamalParameters = obj as ElGamalParameters;
			return elGamalParameters != null && this.Equals(elGamalParameters);
		}

		// Token: 0x06002792 RID: 10130 RVA: 0x000C551A File Offset: 0x000C371A
		protected bool Equals(ElGamalParameters other)
		{
			return this.p.Equals(other.p) && this.g.Equals(other.g) && this.l == other.l;
		}

		// Token: 0x06002793 RID: 10131 RVA: 0x000C5552 File Offset: 0x000C3752
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.g.GetHashCode() ^ this.l;
		}

		// Token: 0x04001A41 RID: 6721
		private readonly BigInteger p;

		// Token: 0x04001A42 RID: 6722
		private readonly BigInteger g;

		// Token: 0x04001A43 RID: 6723
		private readonly int l;
	}
}
