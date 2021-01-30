using System;
using System.Collections;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000458 RID: 1112
	public class Asn1VerifierFactoryProvider : IVerifierFactoryProvider
	{
		// Token: 0x06002853 RID: 10323 RVA: 0x000C7180 File Offset: 0x000C5380
		public Asn1VerifierFactoryProvider(AsymmetricKeyParameter publicKey)
		{
			this.publicKey = publicKey;
		}

		// Token: 0x06002854 RID: 10324 RVA: 0x000C718F File Offset: 0x000C538F
		public IVerifierFactory CreateVerifierFactory(object algorithmDetails)
		{
			return new Asn1VerifierFactory((AlgorithmIdentifier)algorithmDetails, this.publicKey);
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000C701A File Offset: 0x000C521A
		public IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04001A89 RID: 6793
		private readonly AsymmetricKeyParameter publicKey;
	}
}
