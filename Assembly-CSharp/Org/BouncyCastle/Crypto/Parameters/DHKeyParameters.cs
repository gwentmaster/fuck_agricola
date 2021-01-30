using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x0200041B RID: 1051
	public class DHKeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x060026FC RID: 9980 RVA: 0x000C4310 File Offset: 0x000C2510
		protected DHKeyParameters(bool isPrivate, DHParameters parameters) : this(isPrivate, parameters, PkcsObjectIdentifiers.DhKeyAgreement)
		{
		}

		// Token: 0x060026FD RID: 9981 RVA: 0x000C431F File Offset: 0x000C251F
		protected DHKeyParameters(bool isPrivate, DHParameters parameters, DerObjectIdentifier algorithmOid) : base(isPrivate)
		{
			this.parameters = parameters;
			this.algorithmOid = algorithmOid;
		}

		// Token: 0x1700045D RID: 1117
		// (get) Token: 0x060026FE RID: 9982 RVA: 0x000C4336 File Offset: 0x000C2536
		public DHParameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x060026FF RID: 9983 RVA: 0x000C433E File Offset: 0x000C253E
		public DerObjectIdentifier AlgorithmOid
		{
			get
			{
				return this.algorithmOid;
			}
		}

		// Token: 0x06002700 RID: 9984 RVA: 0x000C4348 File Offset: 0x000C2548
		public override bool Equals(object obj)
		{
			if (obj == this)
			{
				return true;
			}
			DHKeyParameters dhkeyParameters = obj as DHKeyParameters;
			return dhkeyParameters != null && this.Equals(dhkeyParameters);
		}

		// Token: 0x06002701 RID: 9985 RVA: 0x000C436E File Offset: 0x000C256E
		protected bool Equals(DHKeyParameters other)
		{
			return object.Equals(this.parameters, other.parameters) && base.Equals(other);
		}

		// Token: 0x06002702 RID: 9986 RVA: 0x000C438C File Offset: 0x000C258C
		public override int GetHashCode()
		{
			int num = base.GetHashCode();
			if (this.parameters != null)
			{
				num ^= this.parameters.GetHashCode();
			}
			return num;
		}

		// Token: 0x04001A15 RID: 6677
		private readonly DHParameters parameters;

		// Token: 0x04001A16 RID: 6678
		private readonly DerObjectIdentifier algorithmOid;
	}
}
