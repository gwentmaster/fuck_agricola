using System;
using System.Collections;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Operators
{
	// Token: 0x02000452 RID: 1106
	public class Asn1SignatureFactory : ISignatureFactory
	{
		// Token: 0x0600283E RID: 10302 RVA: 0x000C6F7A File Offset: 0x000C517A
		public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey) : this(algorithm, privateKey, null)
		{
		}

		// Token: 0x0600283F RID: 10303 RVA: 0x000C6F88 File Offset: 0x000C5188
		public Asn1SignatureFactory(string algorithm, AsymmetricKeyParameter privateKey, SecureRandom random)
		{
			DerObjectIdentifier algorithmOid = X509Utilities.GetAlgorithmOid(algorithm);
			this.algorithm = algorithm;
			this.privateKey = privateKey;
			this.random = random;
			this.algID = X509Utilities.GetSigAlgID(algorithmOid, algorithm);
		}

		// Token: 0x170004B7 RID: 1207
		// (get) Token: 0x06002840 RID: 10304 RVA: 0x000C6FC4 File Offset: 0x000C51C4
		public object AlgorithmDetails
		{
			get
			{
				return this.algID;
			}
		}

		// Token: 0x06002841 RID: 10305 RVA: 0x000C6FCC File Offset: 0x000C51CC
		public IStreamCalculator CreateCalculator()
		{
			ISigner signer = SignerUtilities.GetSigner(this.algorithm);
			if (this.random != null)
			{
				signer.Init(true, new ParametersWithRandom(this.privateKey, this.random));
			}
			else
			{
				signer.Init(true, this.privateKey);
			}
			return new SigCalculator(signer);
		}

		// Token: 0x170004B8 RID: 1208
		// (get) Token: 0x06002842 RID: 10306 RVA: 0x000C701A File Offset: 0x000C521A
		public static IEnumerable SignatureAlgNames
		{
			get
			{
				return X509Utilities.GetAlgNames();
			}
		}

		// Token: 0x04001A7D RID: 6781
		private readonly AlgorithmIdentifier algID;

		// Token: 0x04001A7E RID: 6782
		private readonly string algorithm;

		// Token: 0x04001A7F RID: 6783
		private readonly AsymmetricKeyParameter privateKey;

		// Token: 0x04001A80 RID: 6784
		private readonly SecureRandom random;
	}
}
