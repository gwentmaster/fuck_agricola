using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Parameters
{
	// Token: 0x02000429 RID: 1065
	public class ECKeyGenerationParameters : KeyGenerationParameters
	{
		// Token: 0x06002764 RID: 10084 RVA: 0x000C4FEB File Offset: 0x000C31EB
		public ECKeyGenerationParameters(ECDomainParameters domainParameters, SecureRandom random) : base(random, domainParameters.N.BitLength)
		{
			this.domainParams = domainParameters;
		}

		// Token: 0x06002765 RID: 10085 RVA: 0x000C5006 File Offset: 0x000C3206
		public ECKeyGenerationParameters(DerObjectIdentifier publicKeyParamSet, SecureRandom random) : this(ECKeyParameters.LookupParameters(publicKeyParamSet), random)
		{
			this.publicKeyParamSet = publicKeyParamSet;
		}

		// Token: 0x17000477 RID: 1143
		// (get) Token: 0x06002766 RID: 10086 RVA: 0x000C501C File Offset: 0x000C321C
		public ECDomainParameters DomainParameters
		{
			get
			{
				return this.domainParams;
			}
		}

		// Token: 0x17000478 RID: 1144
		// (get) Token: 0x06002767 RID: 10087 RVA: 0x000C5024 File Offset: 0x000C3224
		public DerObjectIdentifier PublicKeyParamSet
		{
			get
			{
				return this.publicKeyParamSet;
			}
		}

		// Token: 0x04001A37 RID: 6711
		private readonly ECDomainParameters domainParams;

		// Token: 0x04001A38 RID: 6712
		private readonly DerObjectIdentifier publicKeyParamSet;
	}
}
