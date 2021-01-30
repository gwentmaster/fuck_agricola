using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041E RID: 1054
	public class DHPublicKeyParameters : DHKeyParameters
	{
		// Token: 0x0600271A RID: 10010 RVA: 0x000C46C4 File Offset: 0x000C28C4
		public DHPublicKeyParameters(BigInteger y, DHParameters parameters) : base(false, parameters)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x0600271B RID: 10011 RVA: 0x000C46E3 File Offset: 0x000C28E3
		public DHPublicKeyParameters(BigInteger y, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(false, parameters, algorithmOid)
		{
			if (y == null)
			{
				throw new ArgumentNullException("y");
			}
			this.y = y;
		}

		// Token: 0x17000467 RID: 1127
		// (get) Token: 0x0600271C RID: 10012 RVA: 0x000C4703 File Offset: 0x000C2903
		public BigInteger Y
		{
			get
			{
				return this.y;
			}
		}

		// Token: 0x0600271D RID: 10013 RVA: 0x000C470C File Offset: 0x000C290C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHPublicKeyParameters dhpublicKeyParameters = obj as DHPublicKeyParameters;
			return dhpublicKeyParameters != null && this.Equals(dhpublicKeyParameters);
		}

		// Token: 0x0600271E RID: 10014 RVA: 0x000C4732 File Offset: 0x000C2932
		protected bool Equals(DHPublicKeyParameters other)
		{
			return this.y.Equals(other.y) && base.Equals(other);
		}

		// Token: 0x0600271F RID: 10015 RVA: 0x000C4750 File Offset: 0x000C2950
		public override int GetHashCode()
		{
			return this.y.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A20 RID: 6688
		private readonly BigInteger y;
	}
}
