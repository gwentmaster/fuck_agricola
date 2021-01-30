using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000395 RID: 917
	public abstract class AbstractTlsKeyExchange : TlsKeyExchange
	{
		// Token: 0x060022A5 RID: 8869 RVA: 0x000B5BF4 File Offset: 0x000B3DF4
		protected AbstractTlsKeyExchange(int keyExchange, IList supportedSignatureAlgorithms)
		{
			this.mKeyExchange = keyExchange;
			this.mSupportedSignatureAlgorithms = supportedSignatureAlgorithms;
		}

		// Token: 0x060022A6 RID: 8870 RVA: 0x000B5C0C File Offset: 0x000B3E0C
		protected virtual DigitallySigned ParseSignature(Stream input)
		{
			DigitallySigned digitallySigned = DigitallySigned.Parse(this.mContext, input);
			SignatureAndHashAlgorithm algorithm = digitallySigned.Algorithm;
			if (algorithm != null)
			{
				TlsUtilities.VerifySupportedSignatureAlgorithm(this.mSupportedSignatureAlgorithms, algorithm);
			}
			return digitallySigned;
		}

		// Token: 0x060022A7 RID: 8871 RVA: 0x000B5C3C File Offset: 0x000B3E3C
		public virtual void Init(TlsContext context)
		{
			this.mContext = context;
			ProtocolVersion clientVersion = context.ClientVersion;
			if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(clientVersion))
			{
				if (this.mSupportedSignatureAlgorithms == null)
				{
					switch (this.mKeyExchange)
					{
					case 1:
					case 5:
					case 9:
					case 15:
					case 18:
					case 19:
					case 23:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultRsaSignatureAlgorithms();
						return;
					case 3:
					case 7:
					case 22:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultDssSignatureAlgorithms();
						return;
					case 13:
					case 14:
					case 21:
					case 24:
						return;
					case 16:
					case 17:
						this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultECDsaSignatureAlgorithms();
						return;
					}
					throw new InvalidOperationException("unsupported key exchange algorithm");
				}
			}
			else if (this.mSupportedSignatureAlgorithms != null)
			{
				throw new InvalidOperationException("supported_signature_algorithms not allowed for " + clientVersion);
			}
		}

		// Token: 0x060022A8 RID: 8872
		public abstract void SkipServerCredentials();

		// Token: 0x060022A9 RID: 8873 RVA: 0x000B5D26 File Offset: 0x000B3F26
		public virtual void ProcessServerCertificate(Certificate serverCertificate)
		{
			IList list = this.mSupportedSignatureAlgorithms;
		}

		// Token: 0x060022AA RID: 8874 RVA: 0x000B5D2F File Offset: 0x000B3F2F
		public virtual void ProcessServerCredentials(TlsCredentials serverCredentials)
		{
			this.ProcessServerCertificate(serverCredentials.Certificate);
		}

		// Token: 0x170003DD RID: 989
		// (get) Token: 0x060022AB RID: 8875 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool RequiresServerKeyExchange
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060022AC RID: 8876 RVA: 0x000B5D3D File Offset: 0x000B3F3D
		public virtual byte[] GenerateServerKeyExchange()
		{
			if (this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(80);
			}
			return null;
		}

		// Token: 0x060022AD RID: 8877 RVA: 0x000B5D50 File Offset: 0x000B3F50
		public virtual void SkipServerKeyExchange()
		{
			if (this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060022AE RID: 8878 RVA: 0x000B5D62 File Offset: 0x000B3F62
		public virtual void ProcessServerKeyExchange(Stream input)
		{
			if (!this.RequiresServerKeyExchange)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060022AF RID: 8879
		public abstract void ValidateCertificateRequest(CertificateRequest certificateRequest);

		// Token: 0x060022B0 RID: 8880 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void SkipClientCredentials()
		{
		}

		// Token: 0x060022B1 RID: 8881
		public abstract void ProcessClientCredentials(TlsCredentials clientCredentials);

		// Token: 0x060022B2 RID: 8882 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void ProcessClientCertificate(Certificate clientCertificate)
		{
		}

		// Token: 0x060022B3 RID: 8883
		public abstract void GenerateClientKeyExchange(Stream output);

		// Token: 0x060022B4 RID: 8884 RVA: 0x000B57B9 File Offset: 0x000B39B9
		public virtual void ProcessClientKeyExchange(Stream input)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060022B5 RID: 8885
		public abstract byte[] GeneratePremasterSecret();

		// Token: 0x040016A2 RID: 5794
		protected readonly int mKeyExchange;

		// Token: 0x040016A3 RID: 5795
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x040016A4 RID: 5796
		protected TlsContext mContext;
	}
}
