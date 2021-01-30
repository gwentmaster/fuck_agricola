using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E1 RID: 993
	public class TlsClientProtocol : TlsProtocol
	{
		// Token: 0x06002459 RID: 9305 RVA: 0x000B9A05 File Offset: 0x000B7C05
		public TlsClientProtocol(Stream stream, SecureRandom secureRandom) : base(stream, secureRandom)
		{
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x000B9A0F File Offset: 0x000B7C0F
		public TlsClientProtocol(Stream input, Stream output, SecureRandom secureRandom) : base(input, output, secureRandom)
		{
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x000B9A1A File Offset: 0x000B7C1A
		public TlsClientProtocol(SecureRandom secureRandom) : base(secureRandom)
		{
		}

		// Token: 0x0600245C RID: 9308 RVA: 0x000B9A24 File Offset: 0x000B7C24
		public virtual void Connect(TlsClient tlsClient)
		{
			if (tlsClient == null)
			{
				throw new ArgumentNullException("tlsClient");
			}
			if (this.mTlsClient != null)
			{
				throw new InvalidOperationException("'Connect' can only be called once");
			}
			this.mTlsClient = tlsClient;
			this.mSecurityParameters = new SecurityParameters();
			this.mSecurityParameters.entity = 1;
			this.mTlsClientContext = new TlsClientContextImpl(this.mSecureRandom, this.mSecurityParameters);
			this.mSecurityParameters.clientRandom = TlsProtocol.CreateRandomBlock(tlsClient.ShouldUseGmtUnixTime(), this.mTlsClientContext.NonceRandomGenerator);
			this.mTlsClient.Init(this.mTlsClientContext);
			this.mRecordStream.Init(this.mTlsClientContext);
			TlsSession sessionToResume = tlsClient.GetSessionToResume();
			if (sessionToResume != null && sessionToResume.IsResumable)
			{
				SessionParameters sessionParameters = sessionToResume.ExportSessionParameters();
				if (sessionParameters != null)
				{
					this.mTlsSession = sessionToResume;
					this.mSessionParameters = sessionParameters;
				}
			}
			this.SendClientHelloMessage();
			this.mConnectionState = 1;
			this.BlockForHandshake();
		}

		// Token: 0x0600245D RID: 9309 RVA: 0x000B9B07 File Offset: 0x000B7D07
		protected override void CleanupHandshake()
		{
			base.CleanupHandshake();
			this.mSelectedSessionID = null;
			this.mKeyExchange = null;
			this.mAuthentication = null;
			this.mCertificateStatus = null;
			this.mCertificateRequest = null;
		}

		// Token: 0x17000425 RID: 1061
		// (get) Token: 0x0600245E RID: 9310 RVA: 0x000B9B32 File Offset: 0x000B7D32
		protected override TlsContext Context
		{
			get
			{
				return this.mTlsClientContext;
			}
		}

		// Token: 0x17000426 RID: 1062
		// (get) Token: 0x0600245F RID: 9311 RVA: 0x000B9B32 File Offset: 0x000B7D32
		internal override AbstractTlsContext ContextAdmin
		{
			get
			{
				return this.mTlsClientContext;
			}
		}

		// Token: 0x17000427 RID: 1063
		// (get) Token: 0x06002460 RID: 9312 RVA: 0x000B9B3A File Offset: 0x000B7D3A
		protected override TlsPeer Peer
		{
			get
			{
				return this.mTlsClient;
			}
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x000B9B44 File Offset: 0x000B7D44
		protected override void HandleHandshakeMessage(byte type, byte[] data)
		{
			MemoryStream memoryStream = new MemoryStream(data, false);
			if (!this.mResumedSession)
			{
				switch (type)
				{
				case 0:
					TlsProtocol.AssertEmpty(memoryStream);
					if (this.mConnectionState == 16)
					{
						this.RefuseRenegotiation();
						return;
					}
					return;
				case 2:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 1)
					{
						throw new TlsFatalAlert(10);
					}
					this.ReceiveServerHelloMessage(memoryStream);
					this.mConnectionState = 2;
					this.mRecordStream.NotifyHelloComplete();
					this.ApplyMaxFragmentLengthExtension();
					if (this.mResumedSession)
					{
						this.mSecurityParameters.masterSecret = Arrays.Clone(this.mSessionParameters.MasterSecret);
						this.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
						this.SendChangeCipherSpecMessage();
						return;
					}
					this.InvalidateSession();
					if (this.mSelectedSessionID.Length != 0)
					{
						this.mTlsSession = new TlsSessionImpl(this.mSelectedSessionID, null);
						return;
					}
					return;
				}
				case 4:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 13)
					{
						throw new TlsFatalAlert(10);
					}
					if (!this.mExpectSessionTicket)
					{
						throw new TlsFatalAlert(10);
					}
					this.InvalidateSession();
					this.ReceiveNewSessionTicketMessage(memoryStream);
					this.mConnectionState = 14;
					return;
				}
				case 11:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 1)
					{
						if (this.mConnectionState == 2)
						{
							this.HandleSupplementalData(null);
						}
						this.mPeerCertificate = Certificate.Parse(memoryStream);
						TlsProtocol.AssertEmpty(memoryStream);
						if (this.mPeerCertificate == null || this.mPeerCertificate.IsEmpty)
						{
							this.mAllowCertificateStatus = false;
						}
						this.mKeyExchange.ProcessServerCertificate(this.mPeerCertificate);
						this.mAuthentication = this.mTlsClient.GetAuthentication();
						this.mAuthentication.NotifyServerCertificate(this.mPeerCertificate);
						this.mConnectionState = 4;
						return;
					}
					throw new TlsFatalAlert(10);
				}
				case 12:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 3)
					{
						if (this.mConnectionState < 3)
						{
							this.HandleSupplementalData(null);
						}
						if (this.mConnectionState < 4)
						{
							this.mKeyExchange.SkipServerCredentials();
							this.mAuthentication = null;
						}
						this.mKeyExchange.ProcessServerKeyExchange(memoryStream);
						TlsProtocol.AssertEmpty(memoryStream);
						this.mConnectionState = 6;
						return;
					}
					throw new TlsFatalAlert(10);
				}
				case 13:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 4 > 2)
					{
						throw new TlsFatalAlert(10);
					}
					if (this.mConnectionState != 6)
					{
						this.mKeyExchange.SkipServerKeyExchange();
					}
					if (this.mAuthentication == null)
					{
						throw new TlsFatalAlert(40);
					}
					this.mCertificateRequest = CertificateRequest.Parse(this.Context, memoryStream);
					TlsProtocol.AssertEmpty(memoryStream);
					this.mKeyExchange.ValidateCertificateRequest(this.mCertificateRequest);
					TlsUtilities.TrackHashAlgorithms(this.mRecordStream.HandshakeHash, this.mCertificateRequest.SupportedSignatureAlgorithms);
					this.mConnectionState = 7;
					return;
				}
				case 14:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 2 <= 5)
					{
						if (this.mConnectionState < 3)
						{
							this.HandleSupplementalData(null);
						}
						if (this.mConnectionState < 4)
						{
							this.mKeyExchange.SkipServerCredentials();
							this.mAuthentication = null;
						}
						if (this.mConnectionState < 6)
						{
							this.mKeyExchange.SkipServerKeyExchange();
						}
						TlsProtocol.AssertEmpty(memoryStream);
						this.mConnectionState = 8;
						this.mRecordStream.HandshakeHash.SealHashAlgorithms();
						IList clientSupplementalData = this.mTlsClient.GetClientSupplementalData();
						if (clientSupplementalData != null)
						{
							this.SendSupplementalDataMessage(clientSupplementalData);
						}
						this.mConnectionState = 9;
						TlsCredentials tlsCredentials = null;
						if (this.mCertificateRequest == null)
						{
							this.mKeyExchange.SkipClientCredentials();
						}
						else
						{
							tlsCredentials = this.mAuthentication.GetClientCredentials(this.Context, this.mCertificateRequest);
							if (tlsCredentials == null)
							{
								this.mKeyExchange.SkipClientCredentials();
								this.SendCertificateMessage(Certificate.EmptyChain);
							}
							else
							{
								this.mKeyExchange.ProcessClientCredentials(tlsCredentials);
								this.SendCertificateMessage(tlsCredentials.Certificate);
							}
						}
						this.mConnectionState = 10;
						this.SendClientKeyExchangeMessage();
						this.mConnectionState = 11;
						TlsHandshakeHash tlsHandshakeHash = this.mRecordStream.PrepareToFinish();
						this.mSecurityParameters.sessionHash = TlsProtocol.GetCurrentPrfHash(this.Context, tlsHandshakeHash, null);
						TlsProtocol.EstablishMasterSecret(this.Context, this.mKeyExchange);
						this.mRecordStream.SetPendingConnectionState(this.Peer.GetCompression(), this.Peer.GetCipher());
						if (tlsCredentials != null && tlsCredentials is TlsSignerCredentials)
						{
							TlsSignerCredentials tlsSignerCredentials = (TlsSignerCredentials)tlsCredentials;
							SignatureAndHashAlgorithm signatureAndHashAlgorithm = TlsUtilities.GetSignatureAndHashAlgorithm(this.Context, tlsSignerCredentials);
							byte[] hash;
							if (signatureAndHashAlgorithm == null)
							{
								hash = this.mSecurityParameters.SessionHash;
							}
							else
							{
								hash = tlsHandshakeHash.GetFinalHash(signatureAndHashAlgorithm.Hash);
							}
							byte[] signature = tlsSignerCredentials.GenerateCertificateSignature(hash);
							DigitallySigned certificateVerify = new DigitallySigned(signatureAndHashAlgorithm, signature);
							this.SendCertificateVerifyMessage(certificateVerify);
							this.mConnectionState = 12;
						}
						this.SendChangeCipherSpecMessage();
						this.SendFinishedMessage();
						this.mConnectionState = 13;
						return;
					}
					throw new TlsFatalAlert(40);
				}
				case 20:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState - 13 > 1)
					{
						throw new TlsFatalAlert(10);
					}
					if (this.mConnectionState == 13 && this.mExpectSessionTicket)
					{
						throw new TlsFatalAlert(10);
					}
					this.ProcessFinishedMessage(memoryStream);
					this.mConnectionState = 15;
					this.mConnectionState = 16;
					this.CompleteHandshake();
					return;
				}
				case 22:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState != 4)
					{
						throw new TlsFatalAlert(10);
					}
					if (!this.mAllowCertificateStatus)
					{
						throw new TlsFatalAlert(10);
					}
					this.mCertificateStatus = CertificateStatus.Parse(memoryStream);
					TlsProtocol.AssertEmpty(memoryStream);
					this.mConnectionState = 5;
					return;
				}
				case 23:
				{
					short mConnectionState = this.mConnectionState;
					if (mConnectionState == 2)
					{
						this.HandleSupplementalData(TlsProtocol.ReadSupplementalDataMessage(memoryStream));
						return;
					}
					throw new TlsFatalAlert(10);
				}
				}
				throw new TlsFatalAlert(10);
			}
			if (type != 20 || this.mConnectionState != 2)
			{
				throw new TlsFatalAlert(10);
			}
			this.ProcessFinishedMessage(memoryStream);
			this.mConnectionState = 15;
			this.SendFinishedMessage();
			this.mConnectionState = 13;
			this.mConnectionState = 16;
			this.CompleteHandshake();
		}

		// Token: 0x06002462 RID: 9314 RVA: 0x000BA110 File Offset: 0x000B8310
		protected virtual void HandleSupplementalData(IList serverSupplementalData)
		{
			this.mTlsClient.ProcessServerSupplementalData(serverSupplementalData);
			this.mConnectionState = 3;
			this.mKeyExchange = this.mTlsClient.GetKeyExchange();
			this.mKeyExchange.Init(this.Context);
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x000BA148 File Offset: 0x000B8348
		protected virtual void ReceiveNewSessionTicketMessage(MemoryStream buf)
		{
			NewSessionTicket newSessionTicket = NewSessionTicket.Parse(buf);
			TlsProtocol.AssertEmpty(buf);
			this.mTlsClient.NotifyNewSessionTicket(newSessionTicket);
		}

		// Token: 0x06002464 RID: 9316 RVA: 0x000BA170 File Offset: 0x000B8370
		protected virtual void ReceiveServerHelloMessage(MemoryStream buf)
		{
			ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(buf);
			if (protocolVersion.IsDtls)
			{
				throw new TlsFatalAlert(47);
			}
			if (!protocolVersion.Equals(this.mRecordStream.ReadVersion))
			{
				throw new TlsFatalAlert(47);
			}
			ProtocolVersion clientVersion = this.Context.ClientVersion;
			if (!protocolVersion.IsEqualOrEarlierVersionOf(clientVersion))
			{
				throw new TlsFatalAlert(47);
			}
			this.mRecordStream.SetWriteVersion(protocolVersion);
			this.ContextAdmin.SetServerVersion(protocolVersion);
			this.mTlsClient.NotifyServerVersion(protocolVersion);
			this.mSecurityParameters.serverRandom = TlsUtilities.ReadFully(32, buf);
			this.mSelectedSessionID = TlsUtilities.ReadOpaque8(buf);
			if (this.mSelectedSessionID.Length > 32)
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySessionID(this.mSelectedSessionID);
			this.mResumedSession = (this.mSelectedSessionID.Length != 0 && this.mTlsSession != null && Arrays.AreEqual(this.mSelectedSessionID, this.mTlsSession.SessionID));
			int num = TlsUtilities.ReadUint16(buf);
			if (!Arrays.Contains(this.mOfferedCipherSuites, num) || num == 0 || CipherSuite.IsScsv(num) || !TlsUtilities.IsValidCipherSuiteForVersion(num, this.Context.ServerVersion))
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySelectedCipherSuite(num);
			byte b = TlsUtilities.ReadUint8(buf);
			if (!Arrays.Contains(this.mOfferedCompressionMethods, b))
			{
				throw new TlsFatalAlert(47);
			}
			this.mTlsClient.NotifySelectedCompressionMethod(b);
			this.mServerExtensions = TlsProtocol.ReadExtensions(buf);
			if (this.mServerExtensions != null)
			{
				foreach (object obj in this.mServerExtensions.Keys)
				{
					int num2 = (int)obj;
					if (num2 != 65281)
					{
						if (TlsUtilities.GetExtensionData(this.mClientExtensions, num2) == null)
						{
							throw new TlsFatalAlert(110);
						}
						bool mResumedSession = this.mResumedSession;
					}
				}
			}
			byte[] extensionData = TlsUtilities.GetExtensionData(this.mServerExtensions, 65281);
			if (extensionData != null)
			{
				this.mSecureRenegotiation = true;
				if (!Arrays.ConstantTimeAreEqual(extensionData, TlsProtocol.CreateRenegotiationInfo(TlsUtilities.EmptyBytes)))
				{
					throw new TlsFatalAlert(40);
				}
			}
			this.mTlsClient.NotifySecureRenegotiation(this.mSecureRenegotiation);
			IDictionary dictionary = this.mClientExtensions;
			IDictionary dictionary2 = this.mServerExtensions;
			if (this.mResumedSession)
			{
				if (num != this.mSessionParameters.CipherSuite || b != this.mSessionParameters.CompressionAlgorithm)
				{
					throw new TlsFatalAlert(47);
				}
				dictionary = null;
				dictionary2 = this.mSessionParameters.ReadServerExtensions();
			}
			this.mSecurityParameters.cipherSuite = num;
			this.mSecurityParameters.compressionAlgorithm = b;
			if (dictionary2 != null)
			{
				bool flag = TlsExtensionsUtilities.HasEncryptThenMacExtension(dictionary2);
				if (flag && !TlsUtilities.IsBlockCipherSuite(num))
				{
					throw new TlsFatalAlert(47);
				}
				this.mSecurityParameters.encryptThenMac = flag;
				this.mSecurityParameters.extendedMasterSecret = TlsExtensionsUtilities.HasExtendedMasterSecretExtension(dictionary2);
				this.mSecurityParameters.maxFragmentLength = this.ProcessMaxFragmentLengthExtension(dictionary, dictionary2, 47);
				this.mSecurityParameters.truncatedHMac = TlsExtensionsUtilities.HasTruncatedHMacExtension(dictionary2);
				this.mAllowCertificateStatus = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(dictionary2, 5, 47));
				this.mExpectSessionTicket = (!this.mResumedSession && TlsUtilities.HasExpectedEmptyExtensionData(dictionary2, 35, 47));
			}
			if (dictionary != null)
			{
				this.mTlsClient.ProcessServerExtensions(dictionary2);
			}
			this.mSecurityParameters.prfAlgorithm = TlsProtocol.GetPrfAlgorithm(this.Context, this.mSecurityParameters.CipherSuite);
			this.mSecurityParameters.verifyDataLength = 12;
		}

		// Token: 0x06002465 RID: 9317 RVA: 0x000BA4EC File Offset: 0x000B86EC
		protected virtual void SendCertificateVerifyMessage(DigitallySigned certificateVerify)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(15);
			certificateVerify.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002466 RID: 9318 RVA: 0x000BA510 File Offset: 0x000B8710
		protected virtual void SendClientHelloMessage()
		{
			this.mRecordStream.SetWriteVersion(this.mTlsClient.ClientHelloRecordLayerVersion);
			ProtocolVersion clientVersion = this.mTlsClient.ClientVersion;
			if (clientVersion.IsDtls)
			{
				throw new TlsFatalAlert(80);
			}
			this.ContextAdmin.SetClientVersion(clientVersion);
			byte[] array = TlsUtilities.EmptyBytes;
			if (this.mTlsSession != null)
			{
				array = this.mTlsSession.SessionID;
				if (array == null || array.Length > 32)
				{
					array = TlsUtilities.EmptyBytes;
				}
			}
			bool isFallback = this.mTlsClient.IsFallback;
			this.mOfferedCipherSuites = this.mTlsClient.GetCipherSuites();
			this.mOfferedCompressionMethods = this.mTlsClient.GetCompressionMethods();
			if (array.Length != 0 && this.mSessionParameters != null && (!Arrays.Contains(this.mOfferedCipherSuites, this.mSessionParameters.CipherSuite) || !Arrays.Contains(this.mOfferedCompressionMethods, this.mSessionParameters.CompressionAlgorithm)))
			{
				array = TlsUtilities.EmptyBytes;
			}
			this.mClientExtensions = this.mTlsClient.GetClientExtensions();
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(1);
			TlsUtilities.WriteVersion(clientVersion, handshakeMessage);
			handshakeMessage.Write(this.mSecurityParameters.ClientRandom);
			TlsUtilities.WriteOpaque8(array, handshakeMessage);
			byte[] extensionData = TlsUtilities.GetExtensionData(this.mClientExtensions, 65281);
			bool flag = extensionData == null;
			bool flag2 = !Arrays.Contains(this.mOfferedCipherSuites, 255);
			if (flag && flag2)
			{
				this.mOfferedCipherSuites = Arrays.Append(this.mOfferedCipherSuites, 255);
			}
			if (isFallback && !Arrays.Contains(this.mOfferedCipherSuites, 22016))
			{
				this.mOfferedCipherSuites = Arrays.Append(this.mOfferedCipherSuites, 22016);
			}
			TlsUtilities.WriteUint16ArrayWithUint16Length(this.mOfferedCipherSuites, handshakeMessage);
			TlsUtilities.WriteUint8ArrayWithUint8Length(this.mOfferedCompressionMethods, handshakeMessage);
			if (this.mClientExtensions != null)
			{
				TlsProtocol.WriteExtensions(handshakeMessage, this.mClientExtensions);
			}
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x06002467 RID: 9319 RVA: 0x000BA6D0 File Offset: 0x000B88D0
		protected virtual void SendClientKeyExchangeMessage()
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(16);
			this.mKeyExchange.GenerateClientKeyExchange(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x04001931 RID: 6449
		protected TlsClient mTlsClient;

		// Token: 0x04001932 RID: 6450
		internal TlsClientContextImpl mTlsClientContext;

		// Token: 0x04001933 RID: 6451
		protected byte[] mSelectedSessionID;

		// Token: 0x04001934 RID: 6452
		protected TlsKeyExchange mKeyExchange;

		// Token: 0x04001935 RID: 6453
		protected TlsAuthentication mAuthentication;

		// Token: 0x04001936 RID: 6454
		protected CertificateStatus mCertificateStatus;

		// Token: 0x04001937 RID: 6455
		protected CertificateRequest mCertificateRequest;
	}
}
