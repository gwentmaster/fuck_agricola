using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D2 RID: 978
	public sealed class SessionParameters
	{
		// Token: 0x0600240C RID: 9228 RVA: 0x000B8B44 File Offset: 0x000B6D44
		private SessionParameters(int cipherSuite, byte compressionAlgorithm, byte[] masterSecret, Certificate peerCertificate, byte[] pskIdentity, byte[] srpIdentity, byte[] encodedServerExtensions)
		{
			this.mCipherSuite = cipherSuite;
			this.mCompressionAlgorithm = compressionAlgorithm;
			this.mMasterSecret = Arrays.Clone(masterSecret);
			this.mPeerCertificate = peerCertificate;
			this.mPskIdentity = Arrays.Clone(pskIdentity);
			this.mSrpIdentity = Arrays.Clone(srpIdentity);
			this.mEncodedServerExtensions = encodedServerExtensions;
		}

		// Token: 0x0600240D RID: 9229 RVA: 0x000B8B9B File Offset: 0x000B6D9B
		public void Clear()
		{
			if (this.mMasterSecret != null)
			{
				Arrays.Fill(this.mMasterSecret, 0);
			}
		}

		// Token: 0x0600240E RID: 9230 RVA: 0x000B8BB1 File Offset: 0x000B6DB1
		public SessionParameters Copy()
		{
			return new SessionParameters(this.mCipherSuite, this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions);
		}

		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x0600240F RID: 9231 RVA: 0x000B8BE2 File Offset: 0x000B6DE2
		public int CipherSuite
		{
			get
			{
				return this.mCipherSuite;
			}
		}

		// Token: 0x17000414 RID: 1044
		// (get) Token: 0x06002410 RID: 9232 RVA: 0x000B8BEA File Offset: 0x000B6DEA
		public byte CompressionAlgorithm
		{
			get
			{
				return this.mCompressionAlgorithm;
			}
		}

		// Token: 0x17000415 RID: 1045
		// (get) Token: 0x06002411 RID: 9233 RVA: 0x000B8BF2 File Offset: 0x000B6DF2
		public byte[] MasterSecret
		{
			get
			{
				return this.mMasterSecret;
			}
		}

		// Token: 0x17000416 RID: 1046
		// (get) Token: 0x06002412 RID: 9234 RVA: 0x000B8BFA File Offset: 0x000B6DFA
		public Certificate PeerCertificate
		{
			get
			{
				return this.mPeerCertificate;
			}
		}

		// Token: 0x17000417 RID: 1047
		// (get) Token: 0x06002413 RID: 9235 RVA: 0x000B8C02 File Offset: 0x000B6E02
		public byte[] PskIdentity
		{
			get
			{
				return this.mPskIdentity;
			}
		}

		// Token: 0x17000418 RID: 1048
		// (get) Token: 0x06002414 RID: 9236 RVA: 0x000B8C0A File Offset: 0x000B6E0A
		public byte[] SrpIdentity
		{
			get
			{
				return this.mSrpIdentity;
			}
		}

		// Token: 0x06002415 RID: 9237 RVA: 0x000B8C12 File Offset: 0x000B6E12
		public IDictionary ReadServerExtensions()
		{
			if (this.mEncodedServerExtensions == null)
			{
				return null;
			}
			return TlsProtocol.ReadExtensions(new MemoryStream(this.mEncodedServerExtensions, false));
		}

		// Token: 0x04001909 RID: 6409
		private int mCipherSuite;

		// Token: 0x0400190A RID: 6410
		private byte mCompressionAlgorithm;

		// Token: 0x0400190B RID: 6411
		private byte[] mMasterSecret;

		// Token: 0x0400190C RID: 6412
		private Certificate mPeerCertificate;

		// Token: 0x0400190D RID: 6413
		private byte[] mPskIdentity;

		// Token: 0x0400190E RID: 6414
		private byte[] mSrpIdentity;

		// Token: 0x0400190F RID: 6415
		private byte[] mEncodedServerExtensions;

		// Token: 0x02000883 RID: 2179
		public sealed class Builder
		{
			// Token: 0x0600455D RID: 17757 RVA: 0x0014499C File Offset: 0x00142B9C
			public SessionParameters Build()
			{
				this.Validate(this.mCipherSuite >= 0, "cipherSuite");
				this.Validate(this.mCompressionAlgorithm >= 0, "compressionAlgorithm");
				this.Validate(this.mMasterSecret != null, "masterSecret");
				return new SessionParameters(this.mCipherSuite, (byte)this.mCompressionAlgorithm, this.mMasterSecret, this.mPeerCertificate, this.mPskIdentity, this.mSrpIdentity, this.mEncodedServerExtensions);
			}

			// Token: 0x0600455E RID: 17758 RVA: 0x00144A1B File Offset: 0x00142C1B
			public SessionParameters.Builder SetCipherSuite(int cipherSuite)
			{
				this.mCipherSuite = cipherSuite;
				return this;
			}

			// Token: 0x0600455F RID: 17759 RVA: 0x00144A25 File Offset: 0x00142C25
			public SessionParameters.Builder SetCompressionAlgorithm(byte compressionAlgorithm)
			{
				this.mCompressionAlgorithm = (short)compressionAlgorithm;
				return this;
			}

			// Token: 0x06004560 RID: 17760 RVA: 0x00144A2F File Offset: 0x00142C2F
			public SessionParameters.Builder SetMasterSecret(byte[] masterSecret)
			{
				this.mMasterSecret = masterSecret;
				return this;
			}

			// Token: 0x06004561 RID: 17761 RVA: 0x00144A39 File Offset: 0x00142C39
			public SessionParameters.Builder SetPeerCertificate(Certificate peerCertificate)
			{
				this.mPeerCertificate = peerCertificate;
				return this;
			}

			// Token: 0x06004562 RID: 17762 RVA: 0x00144A43 File Offset: 0x00142C43
			public SessionParameters.Builder SetPskIdentity(byte[] pskIdentity)
			{
				this.mPskIdentity = pskIdentity;
				return this;
			}

			// Token: 0x06004563 RID: 17763 RVA: 0x00144A4D File Offset: 0x00142C4D
			public SessionParameters.Builder SetSrpIdentity(byte[] srpIdentity)
			{
				this.mSrpIdentity = srpIdentity;
				return this;
			}

			// Token: 0x06004564 RID: 17764 RVA: 0x00144A58 File Offset: 0x00142C58
			public SessionParameters.Builder SetServerExtensions(IDictionary serverExtensions)
			{
				if (serverExtensions == null)
				{
					this.mEncodedServerExtensions = null;
				}
				else
				{
					MemoryStream memoryStream = new MemoryStream();
					TlsProtocol.WriteExtensions(memoryStream, serverExtensions);
					this.mEncodedServerExtensions = memoryStream.ToArray();
				}
				return this;
			}

			// Token: 0x06004565 RID: 17765 RVA: 0x00144A8B File Offset: 0x00142C8B
			private void Validate(bool condition, string parameter)
			{
				if (!condition)
				{
					throw new InvalidOperationException("Required session parameter '" + parameter + "' not configured");
				}
			}

			// Token: 0x04002F5F RID: 12127
			private int mCipherSuite = -1;

			// Token: 0x04002F60 RID: 12128
			private short mCompressionAlgorithm = -1;

			// Token: 0x04002F61 RID: 12129
			private byte[] mMasterSecret;

			// Token: 0x04002F62 RID: 12130
			private Certificate mPeerCertificate;

			// Token: 0x04002F63 RID: 12131
			private byte[] mPskIdentity;

			// Token: 0x04002F64 RID: 12132
			private byte[] mSrpIdentity;

			// Token: 0x04002F65 RID: 12133
			private byte[] mEncodedServerExtensions;
		}
	}
}
