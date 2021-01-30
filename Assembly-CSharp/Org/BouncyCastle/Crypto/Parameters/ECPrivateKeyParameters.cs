using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200042B RID: 1067
	public class ECPrivateKeyParameters : ECKeyParameters
	{
		// Token: 0x06002774 RID: 10100 RVA: 0x000C521D File Offset: 0x000C341D
		public ECPrivateKeyParameters(BigInteger d, ECDomainParameters parameters) : this("EC", d, parameters)
		{
		}

		// Token: 0x06002775 RID: 10101 RVA: 0x000C522C File Offset: 0x000C342C
		[Obsolete("Use version with explicit 'algorithm' parameter")]
		public ECPrivateKeyParameters(BigInteger d, DerObjectIdentifier publicKeyParamSet) : base("ECGOST3410", true, publicKeyParamSet)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x06002776 RID: 10102 RVA: 0x000C5250 File Offset: 0x000C3450
		public ECPrivateKeyParameters(string algorithm, BigInteger d, ECDomainParameters parameters) : base(algorithm, true, parameters)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x06002777 RID: 10103 RVA: 0x000C5270 File Offset: 0x000C3470
		public ECPrivateKeyParameters(string algorithm, BigInteger d, DerObjectIdentifier publicKeyParamSet) : base(algorithm, true, publicKeyParamSet)
		{
			if (d == null)
			{
				throw new ArgumentNullException("d");
			}
			this.d = d;
		}

		// Token: 0x1700047C RID: 1148
		// (get) Token: 0x06002778 RID: 10104 RVA: 0x000C5290 File Offset: 0x000C3490
		public BigInteger D
		{
			get
			{
				return this.d;
			}
		}

		// Token: 0x06002779 RID: 10105 RVA: 0x000C5298 File Offset: 0x000C3498
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			ECPrivateKeyParameters ecprivateKeyParameters = obj as ECPrivateKeyParameters;
			return ecprivateKeyParameters != null && this.Equals(ecprivateKeyParameters);
		}

		// Token: 0x0600277A RID: 10106 RVA: 0x000C52BE File Offset: 0x000C34BE
		protected bool Equals(ECPrivateKeyParameters other)
		{
			return this.d.Equals(other.d) && base.Equals(other);
		}

		// Token: 0x0600277B RID: 10107 RVA: 0x000C52DC File Offset: 0x000C34DC
		public override int GetHashCode()
		{
			return this.d.GetHashCode() ^ base.GetHashCode();
		}

		// Token: 0x04001A3D RID: 6717
		private readonly BigInteger d;
	}
}
