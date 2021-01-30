using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000426 RID: 1062
	public class DsaPublicKeyParameters : DsaKeyParameters
	{
		// Token: 0x0600274C RID: 10060 RVA: 0x000C4D10 File Offset: 0x000C2F10
		public DsaPublicKeyParameters(BigInteger y, DsaParameters parameters) : base(false, parameters)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x17000470 RID: 1136
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x000C4D2F File Offset: 0x000C2F2F
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x0600274E RID: 10062 RVA: 0x000C4D38 File Offset: 0x000C2F38
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DsaPublicKeyParameters dsaPublicKeyParameters = obj as DsaPublicKeyParameters;
			return dsaPublicKeyParameters != null && this.Equals(dsaPublicKeyParameters);
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x000C4D5E File Offset: 0x000C2F5E
		protected bool Equals(DsaPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x06002750 RID: 10064 RVA: 0x000C4D7C File Offset: 0x000C2F7C
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A2E RID: 6702
		private readonly BigInteger y;
	}
}
