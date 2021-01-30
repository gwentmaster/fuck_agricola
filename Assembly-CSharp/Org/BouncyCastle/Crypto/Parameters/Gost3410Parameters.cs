using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000433 RID: 1075
	public class Gost3410Parameters : ICipherParameters
	{
		// Token: 0x060027A3 RID: 10147 RVA: 0x000C56FC File Offset: 0x000C38FC
		public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a) : this(p, q, a, null)
		{
		}

		// Token: 0x060027A4 RID: 10148 RVA: 0x000C5708 File Offset: 0x000C3908
		public Gost3410Parameters(BigInteger p, BigInteger q, BigInteger a, Gost3410ValidationParameters validation)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			if (a == null)
			{
				throw new ArgumentNullException("a");
			}
			this.p = p;
			this.q = q;
			this.a = a;
			this.validation = validation;
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x060027A5 RID: 10149 RVA: 0x000C5762 File Offset: 0x000C3962
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x060027A6 RID: 10150 RVA: 0x000C576A File Offset: 0x000C396A
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x060027A7 RID: 10151 RVA: 0x000C5772 File Offset: 0x000C3972
		public BigInteger A
		{
			get
			{
				return this.a;
			}
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x060027A8 RID: 10152 RVA: 0x000C577A File Offset: 0x000C397A
		public Gost3410ValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x060027A9 RID: 10153 RVA: 0x000C5784 File Offset: 0x000C3984
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			Gost3410Parameters gost3410Parameters = obj as Gost3410Parameters;
			return gost3410Parameters != null && this.Equals(gost3410Parameters);
		}

		// Token: 0x060027AA RID: 10154 RVA: 0x000C57AA File Offset: 0x000C39AA
		protected bool Equals(Gost3410Parameters other)
		{
			return this.p.Equals(other.p) && this.q.Equals(other.q) && this.a.Equals(other.a);
		}

		// Token: 0x060027AB RID: 10155 RVA: 0x000C57E5 File Offset: 0x000C39E5
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.a.GetHashCode();
		}

		// Token: 0x04001A48 RID: 6728
		private readonly BigInteger p;

		// Token: 0x04001A49 RID: 6729
		private readonly BigInteger q;

		// Token: 0x04001A4A RID: 6730
		private readonly BigInteger a;

		// Token: 0x04001A4B RID: 6731
		private readonly Gost3410ValidationParameters validation;
	}
}
