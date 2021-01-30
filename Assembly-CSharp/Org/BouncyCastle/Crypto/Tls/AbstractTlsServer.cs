using System;
using System.Collections;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000397 RID: 919
	public abstract class AbstractTlsServer : AbstractTlsPeer, TlsServer, TlsPeer
	{
		// Token: 0x060022BE RID: 8894 RVA: 0x000B5D81 File Offset: 0x000B3F81
		public AbstractTlsServer() : this(new DefaultTlsCipherFactory())
		{
		}

		// Token: 0x060022BF RID: 8895 RVA: 0x000B5D8E File Offset: 0x000B3F8E
		public AbstractTlsServer(TlsCipherFactory cipherFactory)
		{
			this.mCipherFactory = cipherFactory;
		}

		// Token: 0x170003DE RID: 990
		// (get) Token: 0x060022C0 RID: 8896 RVA: 0x0000900B File Offset: 0x0000720B
		protected virtual bool AllowEncryptThenMac
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003DF RID: 991
		// (get) Token: 0x060022C1 RID: 8897 RVA: 0x0002A062 File Offset: 0x00028262
		protected virtual bool AllowTruncatedHMac
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060022C2 RID: 8898 RVA: 0x000B5DA0 File Offset: 0x000B3FA0
		protected virtual IDictionary CheckServerExtensions()
		{
			return this.mServerExtensions = TlsExtensionsUtilities.EnsureExtensionsInitialised(this.mServerExtensions);
		}

		// Token: 0x060022C3 RID: 8899
		protected abstract int[] GetCipherSuites();

		// Token: 0x060022C4 RID: 8900 RVA: 0x000B595A File Offset: 0x000B3B5A
		protected byte[] GetCompressionMethods()
		{
			return new byte[1];
		}

		// Token: 0x170003E0 RID: 992
		// (get) Token: 0x060022C5 RID: 8901 RVA: 0x000B5DC1 File Offset: 0x000B3FC1
		protected virtual ProtocolVersion MaximumVersion
		{
			get
			{
				return ProtocolVersion.TLSv11;
			}
		}

		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x060022C6 RID: 8902 RVA: 0x000B593B File Offset: 0x000B3B3B
		protected virtual ProtocolVersion MinimumVersion
		{
			get
			{
				return ProtocolVersion.TLSv10;
			}
		}

		// Token: 0x060022C7 RID: 8903 RVA: 0x000B5DC8 File Offset: 0x000B3FC8
		protected virtual bool SupportsClientEccCapabilities(int[] namedCurves, byte[] ecPointFormats)
		{
			if (namedCurves == null)
			{
				return TlsEccUtilities.HasAnySupportedNamedCurves();
			}
			foreach (int namedCurve in namedCurves)
			{
				if (NamedCurve.IsValid(namedCurve) && (!NamedCurve.RefersToASpecificNamedCurve(namedCurve) || TlsEccUtilities.IsSupportedNamedCurve(namedCurve)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060022C8 RID: 8904 RVA: 0x000B5E0B File Offset: 0x000B400B
		public virtual void Init(TlsServerContext context)
		{
			this.mContext = context;
		}

		// Token: 0x060022C9 RID: 8905 RVA: 0x000B5E14 File Offset: 0x000B4014
		public virtual void NotifyClientVersion(ProtocolVersion clientVersion)
		{
			this.mClientVersion = clientVersion;
		}

		// Token: 0x060022CA RID: 8906 RVA: 0x000B5E1D File Offset: 0x000B401D
		public virtual void NotifyFallback(bool isFallback)
		{
			if (isFallback && this.MaximumVersion.IsLaterVersionOf(this.mClientVersion))
			{
				throw new TlsFatalAlert(86);
			}
		}

		// Token: 0x060022CB RID: 8907 RVA: 0x000B5E3D File Offset: 0x000B403D
		public virtual void NotifyOfferedCipherSuites(int[] offeredCipherSuites)
		{
			this.mOfferedCipherSuites = offeredCipherSuites;
			this.mEccCipherSuitesOffered = TlsEccUtilities.ContainsEccCipherSuites(this.mOfferedCipherSuites);
		}

		// Token: 0x060022CC RID: 8908 RVA: 0x000B5E57 File Offset: 0x000B4057
		public virtual void NotifyOfferedCompressionMethods(byte[] offeredCompressionMethods)
		{
			this.mOfferedCompressionMethods = offeredCompressionMethods;
		}

		// Token: 0x060022CD RID: 8909 RVA: 0x000B5E60 File Offset: 0x000B4060
		public virtual void ProcessClientExtensions(IDictionary clientExtensions)
		{
			this.mClientExtensions = clientExtensions;
			if (clientExtensions != null)
			{
				this.mEncryptThenMacOffered = TlsExtensionsUtilities.HasEncryptThenMacExtension(clientExtensions);
				this.mMaxFragmentLengthOffered = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions);
				if (this.mMaxFragmentLengthOffered >= 0 && !MaxFragmentLength.IsValid((byte)this.mMaxFragmentLengthOffered))
				{
					throw new TlsFatalAlert(47);
				}
				this.mTruncatedHMacOffered = TlsExtensionsUtilities.HasTruncatedHMacExtension(clientExtensions);
				this.mSupportedSignatureAlgorithms = TlsUtilities.GetSignatureAlgorithmsExtension(clientExtensions);
				if (this.mSupportedSignatureAlgorithms != null && !TlsUtilities.IsSignatureAlgorithmsExtensionAllowed(this.mClientVersion))
				{
					throw new TlsFatalAlert(47);
				}
				this.mNamedCurves = TlsEccUtilities.GetSupportedEllipticCurvesExtension(clientExtensions);
				this.mClientECPointFormats = TlsEccUtilities.GetSupportedPointFormatsExtension(clientExtensions);
			}
		}

		// Token: 0x060022CE RID: 8910 RVA: 0x000B5F00 File Offset: 0x000B4100
		public virtual ProtocolVersion GetServerVersion()
		{
			if (this.MinimumVersion.IsEqualOrEarlierVersionOf(this.mClientVersion))
			{
				ProtocolVersion maximumVersion = this.MaximumVersion;
				if (this.mClientVersion.IsEqualOrEarlierVersionOf(maximumVersion))
				{
					return this.mServerVersion = this.mClientVersion;
				}
				if (this.mClientVersion.IsLaterVersionOf(maximumVersion))
				{
					return this.mServerVersion = maximumVersion;
				}
			}
			throw new TlsFatalAlert(70);
		}

		// Token: 0x060022CF RID: 8911 RVA: 0x000B5F68 File Offset: 0x000B4168
		public virtual int GetSelectedCipherSuite()
		{
			bool flag = this.SupportsClientEccCapabilities(this.mNamedCurves, this.mClientECPointFormats);
			foreach (int num in this.GetCipherSuites())
			{
				if (Arrays.Contains(this.mOfferedCipherSuites, num) && (flag || !TlsEccUtilities.IsEccCipherSuite(num)) && TlsUtilities.IsValidCipherSuiteForVersion(num, this.mServerVersion))
				{
					return this.mSelectedCipherSuite = num;
				}
			}
			throw new TlsFatalAlert(40);
		}

		// Token: 0x060022D0 RID: 8912 RVA: 0x000B5FDC File Offset: 0x000B41DC
		public virtual byte GetSelectedCompressionMethod()
		{
			byte[] compressionMethods = this.GetCompressionMethods();
			for (int i = 0; i < compressionMethods.Length; i++)
			{
				if (Arrays.Contains(this.mOfferedCompressionMethods, compressionMethods[i]))
				{
					return this.mSelectedCompressionMethod = compressionMethods[i];
				}
			}
			throw new TlsFatalAlert(40);
		}

		// Token: 0x060022D1 RID: 8913 RVA: 0x000B6024 File Offset: 0x000B4224
		public virtual IDictionary GetServerExtensions()
		{
			if (this.mEncryptThenMacOffered && this.AllowEncryptThenMac && TlsUtilities.IsBlockCipherSuite(this.mSelectedCipherSuite))
			{
				TlsExtensionsUtilities.AddEncryptThenMacExtension(this.CheckServerExtensions());
			}
			if (this.mMaxFragmentLengthOffered >= 0 && TlsUtilities.IsValidUint8((int)this.mMaxFragmentLengthOffered) && MaxFragmentLength.IsValid((byte)this.mMaxFragmentLengthOffered))
			{
				TlsExtensionsUtilities.AddMaxFragmentLengthExtension(this.CheckServerExtensions(), (byte)this.mMaxFragmentLengthOffered);
			}
			if (this.mTruncatedHMacOffered && this.AllowTruncatedHMac)
			{
				TlsExtensionsUtilities.AddTruncatedHMacExtension(this.CheckServerExtensions());
			}
			if (this.mClientECPointFormats != null && TlsEccUtilities.IsEccCipherSuite(this.mSelectedCipherSuite))
			{
				this.mServerECPointFormats = new byte[]
				{
					0,
					1,
					2
				};
				TlsEccUtilities.AddSupportedPointFormatsExtension(this.CheckServerExtensions(), this.mServerECPointFormats);
			}
			return this.mServerExtensions;
		}

		// Token: 0x060022D2 RID: 8914 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual IList GetServerSupplementalData()
		{
			return null;
		}

		// Token: 0x060022D3 RID: 8915
		public abstract TlsCredentials GetCredentials();

		// Token: 0x060022D4 RID: 8916 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual CertificateStatus GetCertificateStatus()
		{
			return null;
		}

		// Token: 0x060022D5 RID: 8917
		public abstract TlsKeyExchange GetKeyExchange();

		// Token: 0x060022D6 RID: 8918 RVA: 0x0000301F File Offset: 0x0000121F
		public virtual CertificateRequest GetCertificateRequest()
		{
			return null;
		}

		// Token: 0x060022D7 RID: 8919 RVA: 0x000B59C3 File Offset: 0x000B3BC3
		public virtual void ProcessClientSupplementalData(IList clientSupplementalData)
		{
			if (clientSupplementalData != null)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x060022D8 RID: 8920 RVA: 0x000B57B9 File Offset: 0x000B39B9
		public virtual void NotifyClientCertificate(Certificate clientCertificate)
		{
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060022D9 RID: 8921 RVA: 0x000B60EC File Offset: 0x000B42EC
		public override TlsCompression GetCompression()
		{
			if (this.mSelectedCompressionMethod == 0)
			{
				return new TlsNullCompression();
			}
			throw new TlsFatalAlert(80);
		}

		// Token: 0x060022DA RID: 8922 RVA: 0x000B6110 File Offset: 0x000B4310
		public override TlsCipher GetCipher()
		{
			int encryptionAlgorithm = TlsUtilities.GetEncryptionAlgorithm(this.mSelectedCipherSuite);
			int macAlgorithm = TlsUtilities.GetMacAlgorithm(this.mSelectedCipherSuite);
			return this.mCipherFactory.CreateCipher(this.mContext, encryptionAlgorithm, macAlgorithm);
		}

		// Token: 0x060022DB RID: 8923 RVA: 0x000B6148 File Offset: 0x000B4348
		public virtual NewSessionTicket GetNewSessionTicket()
		{
			return new NewSessionTicket(0L, TlsUtilities.EmptyBytes);
		}

		// Token: 0x040016A5 RID: 5797
		protected TlsCipherFactory mCipherFactory;

		// Token: 0x040016A6 RID: 5798
		protected TlsServerContext mContext;

		// Token: 0x040016A7 RID: 5799
		protected ProtocolVersion mClientVersion;

		// Token: 0x040016A8 RID: 5800
		protected int[] mOfferedCipherSuites;

		// Token: 0x040016A9 RID: 5801
		protected byte[] mOfferedCompressionMethods;

		// Token: 0x040016AA RID: 5802
		protected IDictionary mClientExtensions;

		// Token: 0x040016AB RID: 5803
		protected bool mEncryptThenMacOffered;

		// Token: 0x040016AC RID: 5804
		protected short mMaxFragmentLengthOffered;

		// Token: 0x040016AD RID: 5805
		protected bool mTruncatedHMacOffered;

		// Token: 0x040016AE RID: 5806
		protected IList mSupportedSignatureAlgorithms;

		// Token: 0x040016AF RID: 5807
		protected bool mEccCipherSuitesOffered;

		// Token: 0x040016B0 RID: 5808
		protected int[] mNamedCurves;

		// Token: 0x040016B1 RID: 5809
		protected byte[] mClientECPointFormats;

		// Token: 0x040016B2 RID: 5810
		protected byte[] mServerECPointFormats;

		// Token: 0x040016B3 RID: 5811
		protected ProtocolVersion mServerVersion;

		// Token: 0x040016B4 RID: 5812
		protected int mSelectedCipherSuite;

		// Token: 0x040016B5 RID: 5813
		protected byte mSelectedCompressionMethod;

		// Token: 0x040016B6 RID: 5814
		protected IDictionary mServerExtensions;
	}
}
