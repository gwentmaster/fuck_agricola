using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.CryptoPro;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000432 RID: 1074
	public abstract class Gost3410KeyParameters : AsymmetricKeyParameter
	{
		// Token: 0x0600279E RID: 10142 RVA: 0x000C5674 File Offset: 0x000C3874
		protected Gost3410KeyParameters(bool isPrivate, Gost3410Parameters parameters) : base(isPrivate)
		{
			this.parameters = parameters;
		}

		// Token: 0x0600279F RID: 10143 RVA: 0x000C5684 File Offset: 0x000C3884
		protected Gost3410KeyParameters(bool isPrivate, DerObjectIdentifier publicKeyParamSet) : base(isPrivate)
		{
			this.parameters = Gost3410KeyParameters.LookupParameters(publicKeyParamSet);
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000485 RID: 1157
		// (get) Token: 0x060027A0 RID: 10144 RVA: 0x000C56A0 File Offset: 0x000C38A0
		public Gost3410Parameters Parameters
		{
			get
			{
				return this.parameters;
			}
		}

		// Token: 0x17000486 RID: 1158
		// (get) Token: 0x060027A1 RID: 10145 RVA: 0x000C56A8 File Offset: 0x000C38A8
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x060027A2 RID: 10146 RVA: 0x000C56B0 File Offset: 0x000C38B0
		private static Gost3410Parameters LookupParameters(DerObjectIdentifier publicKeyParamSet)
		{
			if (publicKeyParamSet == null)
			{
				throw new ArgumentNullException("publicKeyParamSet");
			}
			Gost3410ParamSetParameters byOid = Gost3410NamedParameters.GetByOid(publicKeyParamSet);
			if (byOid == null)
			{
				throw new ArgumentException("OID is not a valid CryptoPro public key parameter set", "publicKeyParamSet");
			}
			return new Gost3410Parameters(byOid.P, byOid.Q, byOid.A);
		}

		// Token: 0x04001A46 RID: 6726
		private readonly Gost3410Parameters parameters;

		// Token: 0x04001A47 RID: 6727
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
