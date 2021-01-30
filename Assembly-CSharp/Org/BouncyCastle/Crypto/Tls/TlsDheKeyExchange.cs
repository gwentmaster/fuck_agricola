using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E8 RID: 1000
	public class TlsDheKeyExchange : TlsDHKeyExchange
	{
		// Token: 0x0600249E RID: 9374 RVA: 0x000BAFB2 File Offset: 0x000B91B2
		public TlsDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters) : base(keyExchange, supportedSignatureAlgorithms, dhParameters)
		{
		}

		// Token: 0x0600249F RID: 9375 RVA: 0x000BAFBD File Offset: 0x000B91BD
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsSignerCredentials)serverCredentials;
		}

		// Token: 0x060024A0 RID: 9376 RVA: 0x000BAFE8 File Offset: 0x000B91E8
		public override byte[] GenerateServerKeyExchange()
		{
			if (this.mDHParameters == null)
			{
				throw new TlsFatalAlert(80);
			}
			DigestInputBuffer digestInputBuffer = new DigestInputBuffer();
			this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mDHParameters, digestInputBuffer);
			SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(this.mContext, this.mServerCredentials);
			IDigest digest = TlsUtilities.CreateHash(signatureAndHashAlgorithm);
			SecurityParameters securityParameters = this.mContext.SecurityParameters;
			digest.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			digest.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			digestInputBuffer.UpdateDigest(digest);
			byte[] hash = DigestUtilities.DoFinal(digest);
			byte[] signature = this.mServerCredentials.GenerateCertificateSignature(hash);
			new DigitallySigned(signatureAndHashAlgorithm, signature).Encode(digestInputBuffer);
			return digestInputBuffer.ToArray();
		}

		// Token: 0x060024A1 RID: 9377 RVA: 0x000BB0A8 File Offset: 0x000B92A8
		public override void ProcessServerKeyExchange(Stream input)
		{
			SecurityParameters securityParameters = this.mContext.SecurityParameters;
			SignerInputBuffer signerInputBuffer = new SignerInputBuffer();
			ServerDHParams serverDHParams = ServerDHParams.Parse(new TeeInputStream(input, signerInputBuffer));
			DigitallySigned digitallySigned = this.ParseSignature(input);
			ISigner signer = this.InitVerifyer(this.mTlsSigner, digitallySigned.Algorithm, securityParameters);
			signerInputBuffer.UpdateSigner(signer);
			if (!signer.VerifySignature(digitallySigned.Signature))
			{
				throw new TlsFatalAlert(51);
			}
			this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey(serverDHParams.PublicKey);
			this.mDHParameters = this.ValidateDHParameters(this.mDHAgreePublicKey.Parameters);
		}

		// Token: 0x060024A2 RID: 9378 RVA: 0x000BB138 File Offset: 0x000B9338
		protected virtual ISigner InitVerifyer(TlsSigner tlsSigner, SignatureAndHashAlgorithm algorithm, SecurityParameters securityParameters)
		{
			ISigner signer = tlsSigner.CreateVerifyer(algorithm, this.mServerPublicKey);
			signer.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			signer.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			return signer;
		}

		// Token: 0x0400194F RID: 6479
		protected TlsSignerCredentials mServerCredentials;
	}
}
