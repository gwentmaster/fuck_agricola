using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003EC RID: 1004
	public class TlsECDheKeyExchange : TlsECDHKeyExchange
	{
		// Token: 0x060024BD RID: 9405 RVA: 0x000BB636 File Offset: 0x000B9836
		public TlsECDheKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms, namedCurves, clientECPointFormats, serverECPointFormats)
		{
		}

		// Token: 0x060024BE RID: 9406 RVA: 0x000BB645 File Offset: 0x000B9845
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsSignerCredentials)serverCredentials;
		}

		// Token: 0x060024BF RID: 9407 RVA: 0x000BB670 File Offset: 0x000B9870
		public override byte[] GenerateServerKeyExchange()
		{
			DigestInputBuffer digestInputBuffer = new DigestInputBuffer();
			this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, digestInputBuffer);
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

		// Token: 0x060024C0 RID: 9408 RVA: 0x000BB724 File Offset: 0x000B9924
		public override void ProcessServerKeyExchange(Stream input)
		{
			SecurityParameters securityParameters = this.mContext.SecurityParameters;
			SignerInputBuffer signerInputBuffer = new SignerInputBuffer();
			Stream input2 = new TeeInputStream(input, signerInputBuffer);
			ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters(this.mNamedCurves, this.mClientECPointFormats, input2);
			byte[] encoding = TlsUtilities.ReadOpaque8(input2);
			DigitallySigned digitallySigned = this.ParseSignature(input);
			ISigner signer = this.InitVerifyer(this.mTlsSigner, digitallySigned.Algorithm, securityParameters);
			signerInputBuffer.UpdateSigner(signer);
			if (!signer.VerifySignature(digitallySigned.Signature))
			{
				throw new TlsFatalAlert(51);
			}
			this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mClientECPointFormats, curve_params, encoding));
		}

		// Token: 0x060024C1 RID: 9409 RVA: 0x000BB7C0 File Offset: 0x000B99C0
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 1 && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x060024C2 RID: 9410 RVA: 0x000BB7F9 File Offset: 0x000B99F9
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x060024C3 RID: 9411 RVA: 0x000BB80B File Offset: 0x000B9A0B
		protected virtual ISigner InitVerifyer(TlsSigner tlsSigner, SignatureAndHashAlgorithm algorithm, SecurityParameters securityParameters)
		{
			ISigner signer = tlsSigner.CreateVerifyer(algorithm, this.mServerPublicKey);
			signer.BlockUpdate(securityParameters.clientRandom, 0, securityParameters.clientRandom.Length);
			signer.BlockUpdate(securityParameters.serverRandom, 0, securityParameters.serverRandom.Length);
			return signer;
		}

		// Token: 0x04001958 RID: 6488
		protected TlsSignerCredentials mServerCredentials;
	}
}
