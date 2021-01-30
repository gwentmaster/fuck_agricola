using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math.EC;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042C RID: 1068
	public class ECPublicKeyParameters : ECKeyParameters
	{
		// Token: 0x0600277C RID: 10108 RVA: 0x000C52F0 File Offset: 0x000C34F0
		public ECPublicKeyParameters(ECPoint q, ECDomainParameters parameters) : this("EC", q, parameters)
		{
		}

		// Token: 0x0600277D RID: 10109 RVA: 0x000C52FF File Offset: 0x000C34FF
		[Obsolete("Use version with explicit 'algorithm' parameter")]
		public ECPublicKeyParameters(ECPoint q, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", false, publicKeyParamSet)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = q.Normalize();
		}

		// Token: 0x0600277E RID: 10110 RVA: 0x000C5328 File Offset: 0x000C3528
		public ECPublicKeyParameters(string algorithm, ECPoint q, ECDomainParameters parameters) : base(algorithm, false, parameters)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = q.Normalize();
		}

		// Token: 0x0600277F RID: 10111 RVA: 0x000C534D File Offset: 0x000C354D
		public ECPublicKeyParameters(string algorithm, ECPoint q, DerObjectIdentifier publicKeyParamSet) : base(algorithm, false, publicKeyParamSet)
		{
			if (q == null)
			{
				throw new ArgumentNullException("q");
			}
			this.q = q.Normalize();
		}

		// Token: 0x1700047D RID: 1149
		// (get) Token: 0x06002780 RID: 10112 RVA: 0x000C5372 File Offset: 0x000C3572
		public ECPoint Q
		{
			get
			{
				return this.q;
			}
		}

		// Token: 0x06002781 RID: 10113 RVA: 0x000C537C File Offset: 0x000C357C
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECPublicKeyParameters ecpublicKeyParameters = obj as ECPublicKeyParameters;
			return ecpublicKeyParameters != null && this.Equals(ecpublicKeyParameters);
		}

		// Token: 0x06002782 RID: 10114 RVA: 0x000C53A2 File Offset: 0x000C35A2
		protected bool Equals(ECPublicKeyParameters other)
		{
			return this.q.Equals(other.q) && base.Equals(other);
		}

		// Token: 0x06002783 RID: 10115 RVA: 0x000C53C0 File Offset: 0x000C35C0
		public override int GetHashCode()
		{
			return this.q.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A3E RID: 6718
		private readonly ECPoint q;
	}
}
