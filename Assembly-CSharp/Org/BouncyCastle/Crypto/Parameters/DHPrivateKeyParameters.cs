using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041D RID: 1053
	public class DHPrivateKeyParameters : DHKeyParameters
	{
		// Token: 0x06002714 RID: 10004 RVA: 0x000C4640 File Offset: 0x000C2840
		public DHPrivateKeyParameters(BigInteger x, DHParameters parameters) : base(true, parameters)
		{
			this.x = x;
		}

		// Token: 0x06002715 RID: 10005 RVA: 0x000C4651 File Offset: 0x000C2851
		public DHPrivateKeyParameters(BigInteger x, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(true, parameters, algorithmOid)
		{
			this.x = x;
		}

		// Token: 0x17000466 RID: 1126
		// (get) Token: 0x06002716 RID: 10006 RVA: 0x000C4663 File Offset: 0x000C2863
		public BigInteger X
		{
			get
			{
				return this.x;
			}
		}

		// Token: 0x06002717 RID: 10007 RVA: 0x000C466C File Offset: 0x000C286C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHPrivateKeyParameters dhprivateKeyParameters = obj as DHPrivateKeyParameters;
			return dhprivateKeyParameters != null && this.Equals(dhprivateKeyParameters);
		}

		// Token: 0x06002718 RID: 10008 RVA: 0x000C4692 File Offset: 0x000C2892
		protected bool Equals(DHPrivateKeyParameters other)
		{
			return this.x.Equals(other.x) && base.Equals(other);
		}

		// Token: 0x06002719 RID: 10009 RVA: 0x000C46B0 File Offset: 0x000C28B0
		public override int GetHashCode()
		{
			return this.x.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A1F RID: 6687
		private readonly BigInteger x;
	}
}
