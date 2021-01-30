using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000455 RID: 1109
	public class Asn1VerifierFactory : IVerifierFactory
	{
		// Token: 0x06002849 RID: 10313 RVA: 0x000C7094 File Offset: 0x000C5294
		public Asn1VerifierFactory(string algorithm, AsymmetricKeyParameter publicKey)
		{
			DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
			this.publicKey = publicKey;
			this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
		}

		// Token: 0x0600284A RID: 10314 RVA: 0x000C70C2 File Offset: 0x000C52C2
		public Asn1VerifierFactory(AlgorithmIdentifier algorithm, AsymmetricKeyParameter publicKey)
		{
			this.publicKey = publicKey;
			this.algID = algorithm;
		}

		// Token: 0x170004BA RID: 1210
		// (get) Token: 0x0600284B RID: 10315 RVA: 0x000C70D8 File Offset: 0x000C52D8
		public object AlgorithmDetails
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x0600284C RID: 10316 RVA: 0x000C70E0 File Offset: 0x000C52E0
		public IStreamCalculator CreateCalculator()
		{
			ISigner signer = SignerUtilities.GetSigner(X509Utilities.GetSignatureName(this.algID));
			signer.Init(false, this.publicKey);
			return new VerifierCalculator(signer);
		}

		// Token: 0x04001A84 RID: 6788
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04001A85 RID: 6789
		private readonly AsymmetricKeyParameter publicKey;
	}
}
