using System;
using System.Collections;
using System.Collections.Generic;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000391 RID: 913
	public abstract class AbstractTlsClient : AbstractTlsPeer, TlsClient, TlsPeer
	{
		// Token: 0x170003CF RID: 975
		// (get) Token: 0x06002276 RID: 8822 RVA: 0x000B57C2 File Offset: 0x000B39C2
		// (set) Token: 0x06002277 RID: 8823 RVA: 0x000B57CA File Offset: 0x000B39CA
		public List<string> HostNames { get; set; }

		// Token: 0x06002278 RID: 8824 RVA: 0x000B57D3 File Offset: 0x000B39D3
		public AbstractTlsClient() : this(new DefaultTlsCipherFactory())
		{
		}

		// Token: 0x06002279 RID: 8825 RVA: 0x000B57E0 File Offset: 0x000B39E0
		public AbstractTlsClient(TlsCipherFactory cipherFactory)
		{
			this.mCipherFactory = cipherFactory;
		}

		// Token: 0x0600227A RID: 8826 RVA: 0x000B57EF File Offset: 0x000B39EF
		protected virtual bool AllowUnexpectedServerExtension(int extensionType, byte[] extensionData)
		{
			if (extensionType == 10)
			{
				TlsEccUtilities.ReadSupportedEllipticCurvesExtension(extensionData);
				return true;
			}
			return false;
		}

		// Token: 0x0600227B RID: 8827 RVA: 0x000B5800 File Offset: 0x000B3A00
		protected virtual void CheckForUnexpectedServerExtension(IDictionary serverExtensions, int extensionType)
		{
			byte[] extensionData = TlsUtilities.GetExtensionData(serverExtensions, extensionType);
			if (extensionData != null && !this.AllowUnexpectedServerExtension(extensionType, extensionData))
			{
				throw new TlsFatalAlert(47);
			}
		}

		// Token: 0x0600227C RID: 8828 RVA: 0x000B582A File Offset: 0x000B3A2A
		public virtual void Init(TlsClientContext context)
		{
			this.mContext = context;
		}

		// Token: 0x0600227D RID: 8829 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual TlsSession GetSessionToResume()
		{
			return null;
		}

		// Token: 0x170003D0 RID: 976
		// (get) Token: 0x0600227E RID: 8830 RVA: 0x000B5833 File Offset: 0x000B3A33
		public virtual ProtocolVersion ClientHelloRecordLayerVersion
		{
			get
			{
				return this.ClientVersion;
			}
		}

		// Token: 0x170003D1 RID: 977
		// (get) Token: 0x0600227F RID: 8831 RVA: 0x000B583B File Offset: 0x000B3A3B
		public virtual ProtocolVersion ClientVersion
		{
			get
			{
				return ProtocolVersion.TLSv12;
			}
		}

		// Token: 0x170003D2 RID: 978
		// (get) Token: 0x06002280 RID: 8832 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsFallback
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002281 RID: 8833 RVA: 0x000B5844 File Offset: 0x000B3A44
		public virtual IDictionary GetClientExtensions()
		{
			IDictionary dictionary = null;
			if (TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mContext.ClientVersion))
			{
				this.mSupportedSignatureAlgorithms = TlsUtilities.GetDefaultSupportedSignatureAlgorithms();
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsUtilities.AddSignatureAlgorithmsExtension(dictionary, this.mSupportedSignatureAlgorithms);
			}
			if (TlsEccUtilities.ContainsEccCipherSuites(this.GetCipherSuites()))
			{
				this.mNamedCurves = new int[]
				{
					23,
					24
				};
				this.mClientECPointFormats = new byte[]
				{
					0,
					1,
					2
				};
				dictionary = TlsExtensionsUtilities.EnsureExtensionsInitialised(dictionary);
				TlsEccUtilities.AddSupportedEllipticCurvesExtension(dictionary, this.mNamedCurves);
				TlsEccUtilities.AddSupportedPointFormatsExtension(dictionary, this.mClientECPointFormats);
			}
			if (this.HostNames != null && this.HostNames.Count > 0)
			{
				List<ServerName> list = new List<ServerName>(this.HostNames.Count);
				for (int i = 0; i < this.HostNames.Count; i++)
				{
					list.Add(new ServerName(0, this.HostNames[i]));
				}
				TlsExtensionsUtilities.AddServerNameExtension(dictionary, new ServerNameList(list));
			}
			return dictionary;
		}

		// Token: 0x170003D3 RID: 979
		// (get) Token: 0x06002282 RID: 8834 RVA: 0x000B593B File Offset: 0x000B3B3B
		public virtual ProtocolVersion MinimumVersion
		{
			get
			{
				return ProtocolVersion.TLSv10;
			}
		}

		// Token: 0x06002283 RID: 8835 RVA: 0x000B5942 File Offset: 0x000B3B42
		public virtual void NotifyServerVersion(ProtocolVersion serverVersion)
		{
			if (!this.MinimumVersion.IsEqualOrEarlierVersionOf(serverVersion))
			{
				throw new TlsFatalAlert(70);
			}
		}

		// Token: 0x06002284 RID: 8836
		public abstract int[] GetCipherSuites();

		// Token: 0x06002285 RID: 8837 RVA: 0x000B595A File Offset: 0x000B3B5A
		public virtual byte[] GetCompressionMethods()
		{
			return new byte[1];
		}

		// Token: 0x06002286 RID: 8838 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void NotifySessionID(byte[] sessionID)
		{
		}

		// Token: 0x06002287 RID: 8839 RVA: 0x000B5962 File Offset: 0x000B3B62
		public virtual void NotifySelectedCipherSuite(int selectedCipherSuite)
		{
			this.mSelectedCipherSuite = selectedCipherSuite;
		}

		// Token: 0x06002288 RID: 8840 RVA: 0x000B596B File Offset: 0x000B3B6B
		public virtual void NotifySelectedCompressionMethod(byte selectedCompressionMethod)
		{
			this.mSelectedCompressionMethod = (short)selectedCompressionMethod;
		}

		// Token: 0x06002289 RID: 8841 RVA: 0x000B5974 File Offset: 0x000B3B74
		public virtual void ProcessServerExtensions(IDictionary serverExtensions)
		{
			if (serverExtensions != null)
			{
				this.CheckForUnexpectedServerExtension(serverExtensions, 13);
				this.CheckForUnexpectedServerExtension(serverExtensions, 10);
				if (TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
				{
					this.mServerECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(serverExtensions);
				}
				else
				{
					this.CheckForUnexpectedServerExtension(serverExtensions, 11);
				}
				this.CheckForUnexpectedServerExtension(serverExtensions, 21);
			}
		}

		// Token: 0x0600228A RID: 8842 RVA: 0x000B59C3 File Offset: 0x000B3BC3
		public virtual void ProcessServerSupplementalData(IList serverSupplementalData)
		{
			if (serverSupplementalData != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x0600228B RID: 8843
		public abstract TlsKeyExchange GetKeyExchange();

		// Token: 0x0600228C RID: 8844
		public abstract TlsAuthentication GetAuthentication();

		// Token: 0x0600228D RID: 8845 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual IList GetClientSupplementalData()
		{
			return null;
		}

		// Token: 0x0600228E RID: 8846 RVA: 0x000B59D0 File Offset: 0x000B3BD0
		public override TlsCompression GetCompression()
		{
			short num = this.mSelectedCompressionMethod;
			if (num == 0)
			{
				return new TlsNullCompression();
			}
			if (num != 1)
			{
				throw new TlsFatalAlert(80);
			}
			return new TlsDeflateCompression();
		}

		// Token: 0x0600228F RID: 8847 RVA: 0x000B5A00 File Offset: 0x000B3C00
		public override TlsCipher GetCipher()
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
			int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
			return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
		}

		// Token: 0x06002290 RID: 8848 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void NotifyNewSessionTicket(NewSessionTicket newSessionTicket)
		{
		}

		// Token: 0x04001691 RID: 5777
		protected TlsCipherFactory mCipherFactory;

		// Token: 0x04001692 RID: 5778
		protected TlsClientContext mContext;

		// Token: 0x04001693 RID: 5779
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x04001694 RID: 5780
		protected int[] mNamedCurves;

		// Token: 0x04001695 RID: 5781
		protected byte[] mClientECPointFormats;

		// Token: 0x04001696 RID: 5782
		protected byte[] mServerECPointFormats;

		// Token: 0x04001697 RID: 5783
		protected int mSelectedCipherSuite;

		// Token: 0x04001698 RID: 5784
		protected short mSelectedCompressionMethod;
	}
}
