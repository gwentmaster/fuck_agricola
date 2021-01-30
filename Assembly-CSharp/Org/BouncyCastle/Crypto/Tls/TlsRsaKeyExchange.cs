using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F9 RID: 1017
	public class TlsRsaKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002582 RID: 9602 RVA: 0x000BDCEE File Offset: 0x000BBEEE
		public TlsRsaKeyExchange(IList supportedSignatureAlgorithms) : base(1, supportedSignatureAlgorithms)
		{
		}

		// Token: 0x06002583 RID: 9603 RVA: 0x000BA785 File Offset: 0x000B8985
		public override void SkipServerCredentials()
		{
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002584 RID: 9604 RVA: 0x000BDCF8 File Offset: 0x000BBEF8
		public override void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			if (!(serverCredentials is TlsEncryptionCredentials))
			{
				throw new TlsFatalAlert(80);
			}
			this.ProcessServerCertificate(serverCredentials.Certificate);
			this.mServerCredentials = (TlsEncryptionCredentials)serverCredentials;
		}

		// Token: 0x06002585 RID: 9605 RVA: 0x000BDD24 File Offset: 0x000BBF24
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (serverCertificate.IsEmpty)
			{
				throw new TlsFatalAlert(42);
			}
			X509CertificateStructure certificateAt = serverCertificate.GetCertificateAt(0);
			SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
			try
			{
				this.mServerPublicKey = PublicKeyFactory.CreateKey(subjectPublicKeyInfo);
			}
			catch (Exception alertCause)
			{
				throw new TlsFatalAlert(43, alertCause);
			}
			if (this.mServerPublicKey.IsPrivate)
			{
				throw new TlsFatalAlert(80);
			}
			this.mRsaServerPublicKey = this.ValidateRsaPublicKey((RsaKeyParameters)this.mServerPublicKey);
			TlsUtilities.ValidateKeyUsage(certificateAt, 32);
			base.ProcessServerCertificate(serverCertificate);
		}

		// Token: 0x06002586 RID: 9606 RVA: 0x000BDDB4 File Offset: 0x000BBFB4
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

		// Token: 0x06002587 RID: 9607 RVA: 0x000BB7F9 File Offset: 0x000B99F9
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x06002588 RID: 9608 RVA: 0x000BDDED File Offset: 0x000BBFED
		public override void GenerateClientKeyExchange(Stream output)
		{
			this.mPremasterSecret = TlsRsaUtilities.GenerateEncryptedPreMasterSecret(this.mContext, this.mRsaServerPublicKey, output);
		}

		// Token: 0x06002589 RID: 9609 RVA: 0x000BDE08 File Offset: 0x000BC008
		public override void ProcessClientKeyExchange(Stream input)
		{
			byte[] encryptedPreMasterSecret;
			if (TlsUtilities.IsSsl(this.mContext))
			{
				encryptedPreMasterSecret = Streams.ReadAll(input);
			}
			else
			{
				encryptedPreMasterSecret = TlsUtilities.ReadOpaque16(input);
			}
			this.mPremasterSecret = this.mServerCredentials.DecryptPreMasterSecret(encryptedPreMasterSecret);
		}

		// Token: 0x0600258A RID: 9610 RVA: 0x000BDE44 File Offset: 0x000BC044
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mPremasterSecret == null)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] result = this.mPremasterSecret;
			this.mPremasterSecret = null;
			return result;
		}

		// Token: 0x0600258B RID: 9611 RVA: 0x000BDE63 File Offset: 0x000BC063
		protected virtual RsaKeyParameters ValidateRsaPublicKey(RsaKeyParameters key)
		{
			if (!key.Exponent.IsProbablePrime(2))
			{
				throw new TlsFatalAlert(47);
			}
			return key;
		}

		// Token: 0x04001996 RID: 6550
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001997 RID: 6551
		protected RsaKeyParameters mRsaServerPublicKey;

		// Token: 0x04001998 RID: 6552
		protected TlsEncryptionCredentials mServerCredentials;

		// Token: 0x04001999 RID: 6553
		protected byte[] mPremasterSecret;
	}
}
