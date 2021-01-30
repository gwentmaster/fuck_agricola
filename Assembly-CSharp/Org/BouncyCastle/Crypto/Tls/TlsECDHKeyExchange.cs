using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003EB RID: 1003
	public class TlsECDHKeyExchange : AbstractTlsKeyExchange
	{
		// Token: 0x060024B0 RID: 9392 RVA: 0x000BB2C0 File Offset: 0x000B94C0
		public TlsECDHKeyExchange(int keyExchange, IList supportedSignatureAlgorithms, int[] namedCurves, byte[] clientECPointFormats, byte[] serverECPointFormats) : base(keyExchange, supportedSignatureAlgorithms)
		{
			switch (keyExchange)
			{
			case 16:
			case 18:
			case 20:
				this.mTlsSigner = null;
				break;
			case 17:
				this.mTlsSigner = new TlsECDsaSigner();
				break;
			case 19:
				this.mTlsSigner = new TlsRsaSigner();
				break;
			default:
				throw new InvalidOperationException("unsupported key exchange algorithm");
			}
			this.mNamedCurves = namedCurves;
			this.mClientECPointFormats = clientECPointFormats;
			this.mServerECPointFormats = serverECPointFormats;
		}

		// Token: 0x060024B1 RID: 9393 RVA: 0x000BB339 File Offset: 0x000B9539
		public override void Init(TlsContext context)
		{
			base.Init(context);
			if (this.mTlsSigner != null)
			{
				this.mTlsSigner.Init(context);
			}
		}

		// Token: 0x060024B2 RID: 9394 RVA: 0x000BB356 File Offset: 0x000B9556
		public override void SkipServerCredentials()
		{
			if (this.mKeyExchange != 20)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060024B3 RID: 9395 RVA: 0x000BB36C File Offset: 0x000B956C
		public override void ProcessServerCertificate(Certificate serverCertificate)
		{
			if (this.mKeyExchange == 20)
			{
				throw new TlsFatalAlert(10);
			}
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
					this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey((ECPublicKeyParameters)this.mServerPublicKey);
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

		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060024B4 RID: 9396 RVA: 0x000BB440 File Offset: 0x000B9640
		public override bool RequiresServerKeyExchange
		{
			get
			{
				int mKeyExchange = this.mKeyExchange;
				return mKeyExchange == 17 || mKeyExchange - 19 <= 1;
			}
		}

		// Token: 0x060024B5 RID: 9397 RVA: 0x000BB464 File Offset: 0x000B9664
		public override byte[] GenerateServerKeyExchange()
		{
			if (!this.RequiresServerKeyExchange)
			{
				return null;
			}
			MemoryStream memoryStream = new MemoryStream();
			this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralServerKeyExchange(this.mContext.SecureRandom, this.mNamedCurves, this.mClientECPointFormats, memoryStream);
			return memoryStream.ToArray();
		}

		// Token: 0x060024B6 RID: 9398 RVA: 0x000BB4AC File Offset: 0x000B96AC
		public override void ProcessServerKeyExchange(Stream input)
		{
			if (!this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
			ECDomainParameters curve_params = TlsEccUtilities.ReadECParameters(this.mNamedCurves, this.mClientECPointFormats, input);
			byte[] encoding = TlsUtilities.ReadOpaque8(input);
			this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mClientECPointFormats, curve_params, encoding));
		}

		// Token: 0x060024B7 RID: 9399 RVA: 0x000BB4FC File Offset: 0x000B96FC
		public override void ValidateCertificateRequest(CertificateRequest certificateRequest)
		{
			foreach (byte b in certificateRequest.CertificateTypes)
			{
				if (b - 1 > 1 && b - 64 > 2)
				{
					throw new TlsFatalAlert(47);
				}
			}
		}

		// Token: 0x060024B8 RID: 9400 RVA: 0x000BB537 File Offset: 0x000B9737
		public override void ProcessClientCredentials(TlsCredentials clientCredentials)
		{
			if (this.mKeyExchange == 20)
			{
				throw new TlsFatalAlert(80);
			}
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

		// Token: 0x060024B9 RID: 9401 RVA: 0x000BB570 File Offset: 0x000B9770
		public override void GenerateClientKeyExchange(Stream output)
		{
			if (this.mAgreementCredentials == null)
			{
				this.mECAgreePrivateKey = TlsEccUtilities.GenerateEphemeralClientKeyExchange(this.mContext.SecureRandom, this.mServerECPointFormats, this.mECAgreePublicKey.Parameters, output);
			}
		}

		// Token: 0x060024BA RID: 9402 RVA: 0x000BB5A2 File Offset: 0x000B97A2
		public override void ProcessClientCertificate(Certificate clientCertificate)
		{
			if (this.mKeyExchange == 20)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060024BB RID: 9403 RVA: 0x000BB5B8 File Offset: 0x000B97B8
		public override void ProcessClientKeyExchange(Stream input)
		{
			if (this.mECAgreePublicKey != null)
			{
				return;
			}
			byte[] encoding = TlsUtilities.ReadOpaque8(input);
			ECDomainParameters parameters = this.mECAgreePrivateKey.Parameters;
			this.mECAgreePublicKey = TlsEccUtilities.ValidateECPublicKey(TlsEccUtilities.DeserializeECPublicKey(this.mServerECPointFormats, parameters, encoding));
		}

		// Token: 0x060024BC RID: 9404 RVA: 0x000BB5F9 File Offset: 0x000B97F9
		public override byte[] GeneratePremasterSecret()
		{
			if (this.mAgreementCredentials != null)
			{
				return this.mAgreementCredentials.GenerateAgreement(this.mECAgreePublicKey);
			}
			if (this.mECAgreePrivateKey != null)
			{
				return TlsEccUtilities.CalculateECDHBasicAgreement(this.mECAgreePublicKey, this.mECAgreePrivateKey);
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x04001950 RID: 6480
		protected TlsSigner mTlsSigner;

		// Token: 0x04001951 RID: 6481
		protected int[] mNamedCurves;

		// Token: 0x04001952 RID: 6482
		protected byte[] mClientECPointFormats;

		// Token: 0x04001953 RID: 6483
		protected byte[] mServerECPointFormats;

		// Token: 0x04001954 RID: 6484
		protected AsymmetricKeyParameter mServerPublicKey;

		// Token: 0x04001955 RID: 6485
		protected TlsAgreementCredentials mAgreementCredentials;

		// Token: 0x04001956 RID: 6486
		protected ECPrivateKeyParameters mECAgreePrivateKey;

		// Token: 0x04001957 RID: 6487
		protected ECPublicKeyParameters mECAgreePublicKey;
	}
}
