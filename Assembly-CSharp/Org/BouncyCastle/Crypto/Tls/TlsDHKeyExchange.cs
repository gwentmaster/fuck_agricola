using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E5 RID: 997
	public class TlsDHKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x06002475 RID: 9333 RVA: 0x000BA6F8 File Offset: 0x000B88F8
		public TlsDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, DHParameters dhParameters) : base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 3:
				this.mTlsSigner = new TlsDssSigner();
				goto IL_5C;
			case 5:
				this.mTlsSigner = new TlsRsaSigner();
				goto IL_5C;
			case 7:
			case 9:
				this.mTlsSigner = null;
				goto IL_5C;
			}
			throw new InvalidOperationException("unsupported key exchange algorithm");
			IL_5C:
			this.mDHParameters = dhParameters;
		}

		// Token: 0x06002476 RID: 9334 RVA: 0x000BA768 File Offset: 0x000B8968
		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (this.mTlsSigner != null)
			{
				this.mTlsSigner.Init(context);
			}
		}

		// Token: 0x06002477 RID: 9335 RVA: 0x000BA785 File Offset: 0x000B8985
		public override void SkipServerCredentials()
		{
			throw new TlsFatalAlert(10);
		}

		// Token: 0x06002478 RID: 9336 RVA: 0x000BA790 File Offset: 0x000B8990
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
			if (this.mTlsSigner == null)
			{
				try
				{
					this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey((DHPublicKeyParameters)this.mServerPublicKey);
					this.mDHParameters = this.ValidateDHParameters(this.mDHAgreePublicKey.Parameters);
				}
				catch (InvalidCastException alertCause2)
				{
					throw new TlsFatalAlert(46, alertCause2);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 8);
			}
			else
			{
				if (!this.mTlsSigner.IsValidPublicKey(this.mServerPublicKey))
				{
					throw new TlsFatalAlert(46);
				}
				TlsUtilities.ValidateKeyUsage(certificateAt, 128);
			}
			base.ProcessServerCertificate(serverCertificate);
		}

		// Token: 0x17000431 RID: 1073
		// (get) Token: 0x06002479 RID: 9337 RVA: 0x000BA868 File Offset: 0x000B8A68
		public override bool RequiresServerKeyExchange
		{
			get
			{
				int mKeyExchange = this.mKeyExchange;
				return mKeyExchange == 3 || mKeyExchange == 5 || mKeyExchange == 11;
			}
		}

		// Token: 0x0600247A RID: 9338 RVA: 0x000BA88C File Offset: 0x000B8A8C
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 3 && b != 64)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x0600247B RID: 9339 RVA: 0x000BA8C5 File Offset: 0x000B8AC5
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (clientCredentials is TlsAgreementCredentials)
			{
				this.mAgreementCredentials = (TlsAgreementCredentials)clientCredentials;
				return;
			}
			if (!(clientCredentials is TlsSignerCredentials))
			{
				throw new TlsFatalAlert(80);
			}
		}

		// Token: 0x0600247C RID: 9340 RVA: 0x000BA8EC File Offset: 0x000B8AEC
		public override void GenerateClientKeyExchange(Stream output)
		{
			if (this.mAgreementCredentials == null)
			{
				this.mDHAgreePrivateKey = TlsDHUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mDHParameters, output);
			}
		}

		// Token: 0x0600247D RID: 9341 RVA: 0x00003022 File Offset: 0x00001222
		public override void ProcessClientCertificate(Certificate clientCertificate)
		{
		}

		// Token: 0x0600247E RID: 9342 RVA: 0x000BA914 File Offset: 0x000B8B14
		public override void ProcessClientKeyExchange(Stream input)
		{
			if (this.mDHAgreePublicKey != null)
			{
				return;
			}
			BigInteger y = TlsDHUtilities.ReadDHParameter(input);
			this.mDHAgreePublicKey = TlsDHUtilities.ValidateDHPublicKey(new DHPublicKeyParameters(y, this.mDHParameters));
		}

		// Token: 0x0600247F RID: 9343 RVA: 0x000BA948 File Offset: 0x000B8B48
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mAgreementCredentials != null)
			{
				return this.mAgreementCredentials.GenerateAgreement(this.mDHAgreePublicKey);
			}
			if (this.mDHAgreePrivateKey != null)
			{
				return TlsDHUtilities.CalculateDHBasicAgreement(this.mDHAgreePublicKey, this.mDHAgreePrivateKey);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x17000432 RID: 1074
		// (get) Token: 0x06002480 RID: 9344 RVA: 0x000BA985 File Offset: 0x000B8B85
		protected virtual int MinimumPrimeBits
		{
			get
			{
				return 1024;
			}
		}

		// Token: 0x06002481 RID: 9345 RVA: 0x000BA98C File Offset: 0x000B8B8C
		protected virtual DHParameters ValidateDHParameters(DHParameters parameters)
		{
			if (parameters.P.BitLength < this.MinimumPrimeBits)
			{
				throw new TlsFatalAlert(71);
			}
			return TlsDHUtilities.ValidateDHParameters(parameters);
		}

		// Token: 0x04001938 RID: 6456
		protected TlsSigner mTlsSigner;

		// Token: 0x04001939 RID: 6457
		protected DHParameters mDHParameters;

		// Token: 0x0400193A RID: 6458
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x0400193B RID: 6459
		protected TlsAgreementCredentials mAgreementCredentials;

		// Token: 0x0400193C RID: 6460
		protected DHPrivateKeyParameters mDHAgreePrivateKey;

		// Token: 0x0400193D RID: 6461
		protected DHPublicKeyParameters mDHAgreePublicKey;
	}
}
