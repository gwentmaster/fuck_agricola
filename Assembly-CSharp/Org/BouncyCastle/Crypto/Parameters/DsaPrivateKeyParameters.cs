using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000425 RID: 1061
	public class DsaPrivateKeyParameters : DsaKeyParameters
	{
		// Token: 0x06002747 RID: 10055 RVA: 0x000C4C8E File Offset: 0x000C2E8E
		public DsaPrivateKeyParameters(BigInteger x, DsaParameters parameters) : base(true, parameters)
		{
			if (x == null)
			{
				throw new ArgumentNullException("x");
			}
			this.x = x;
		}

		// Token: 0x1700046F RID: 1135
		// (get) Token: 0x06002748 RID: 10056 RVA: 0x000C4CAD File Offset: 0x000C2EAD
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x06002749 RID: 10057 RVA: 0x000C4CB8 File Offset: 0x000C2EB8
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaPrivateKeyParameters dsaPrivateKeyParameters = obj as DsaPrivateKeyParameters;
			return dsaPrivateKeyParameters != null && this.Equals(dsaPrivateKeyParameters);
		}

		// Token: 0x0600274A RID: 10058 RVA: 0x000C4CDE File Offset: 0x000C2EDE
		protected bool Equals(DsaPrivateKeyParameters other)
		{
			return this.x.Equals(other.x) && base.Equals(other);
		}

		// Token: 0x0600274B RID: 10059 RVA: 0x000C4CFC File Offset: 0x000C2EFC
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A2D RID: 6701
		private readonly BigInteger x;
	}
}
