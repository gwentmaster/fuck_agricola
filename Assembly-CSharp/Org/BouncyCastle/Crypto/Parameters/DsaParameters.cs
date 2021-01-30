using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000424 RID: 1060
	public class DsaParameters : ICipherParameters
	{
		// Token: 0x0600273E RID: 10046 RVA: 0x000C4B7F File Offset: 0x000C2D7F
		public DsaParameters(BigInteger p, BigInteger q, BigInteger g) : this(p, q, g, null)
		{
		}

		// Token: 0x0600273F RID: 10047 RVA: 0x000C4B8C File Offset: 0x000C2D8C
		public DsaParameters(BigInteger p, BigInteger q, BigInteger g, DsaValidationParameters parameters)
		{
			if (p == null)
			{
				throw new ArgumentNullException("p");
			}
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			if (g == null)
			{
				throw new ArgumentNullException("g");
			}
			this.p = p;
			this.q = q;
			this.g = g;
			this.validation = parameters;
		}

		// Token: 0x1700046B RID: 1131
		// (get) Token: 0x06002740 RID: 10048 RVA: 0x000C4BE6 File Offset: 0x000C2DE6
		public BigInteger P
		{
			get
			{
				return this.p;
			}
		}

		// Token: 0x1700046C RID: 1132
		// (get) Token: 0x06002741 RID: 10049 RVA: 0x000C4BEE File Offset: 0x000C2DEE
		public BigInteger Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x1700046D RID: 1133
		// (get) Token: 0x06002742 RID: 10050 RVA: 0x000C4BF6 File Offset: 0x000C2DF6
		public BigInteger G
		{
			get
			{
				return this.g;
			}
		}

		// Token: 0x1700046E RID: 1134
		// (get) Token: 0x06002743 RID: 10051 RVA: 0x000C4BFE File Offset: 0x000C2DFE
		public DsaValidationParameters ValidationParameters
		{
			get
			{
				return this.validation;
			}
		}

		// Token: 0x06002744 RID: 10052 RVA: 0x000C4C08 File Offset: 0x000C2E08
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaParameters dsaParameters = obj as DsaParameters;
			return dsaParameters != null && this.Equals(dsaParameters);
		}

		// Token: 0x06002745 RID: 10053 RVA: 0x000C4C2E File Offset: 0x000C2E2E
		protected bool Equals(DsaParameters other)
		{
			return this.p.Equals(other.p) && this.q.Equals(other.q) && this.g.Equals(other.g);
		}

		// Token: 0x06002746 RID: 10054 RVA: 0x000C4C69 File Offset: 0x000C2E69
		public override int GetHashCode()
		{
			return this.p.GetHashCode() ^ this.q.GetHashCode() ^ this.g.GetHashCode();
		}

		// Token: 0x04001A29 RID: 6697
		private readonly BigInteger p;

		// Token: 0x04001A2A RID: 6698
		private readonly BigInteger q;

		// Token: 0x04001A2B RID: 6699
		private readonly BigInteger g;

		// Token: 0x04001A2C RID: 6700
		private readonly DsaValidationParameters validation;
	}
}
