using System;
using System.Collections;
using System.IO;
using Org.BouncyCastle.Crypto.Prng;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F8 RID: 1016
	public abstract class TlsProtocol
	{
		// Token: 0x06002545 RID: 9541 RVA: 0x000BC9E6 File Offset: 0x000BABE6
		public TlsProtocol(Stream stream, SecureRandom secureRandom) : this(stream, stream, secureRandom)
		{
		}

		// Token: 0x06002546 RID: 9542 RVA: 0x000BC9F4 File Offset: 0x000BABF4
		public TlsProtocol(Stream input, Stream output, SecureRandom secureRandom)
		{
			this.mApplicationDataQueue = new ByteQueue();
			this.mAlertQueue = new ByteQueue(2);
			this.mHandshakeQueue = new ByteQueue();
			this.mAppDataSplitEnabled = true;
			this.mBlocking = true;
			base..ctor();
			this.mRecordStream = new RecordStream(this, input, output);
			this.mSecureRandom = secureRandom;
		}

		// Token: 0x06002547 RID: 9543 RVA: 0x000BCA50 File Offset: 0x000BAC50
		public TlsProtocol(SecureRandom secureRandom)
		{
			this.mApplicationDataQueue = new ByteQueue();
			this.mAlertQueue = new ByteQueue(2);
			this.mHandshakeQueue = new ByteQueue();
			this.mAppDataSplitEnabled = true;
			this.mBlocking = true;
			base..ctor();
			this.mBlocking = false;
			this.mInputBuffers = new ByteQueueStream();
			this.mOutputBuffer = new ByteQueueStream();
			this.mRecordStream = new RecordStream(this, this.mInputBuffers, this.mOutputBuffer);
			this.mSecureRandom = secureRandom;
		}

		// Token: 0x1700043B RID: 1083
		// (get) Token: 0x06002548 RID: 9544
		protected abstract TlsContext Context { get; }

		// Token: 0x1700043C RID: 1084
		// (get) Token: 0x06002549 RID: 9545
		internal abstract AbstractTlsContext ContextAdmin { get; }

		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x0600254A RID: 9546
		protected abstract TlsPeer Peer { get; }

		// Token: 0x0600254B RID: 9547 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void HandleChangeCipherSpecMessage()
		{
		}

		// Token: 0x0600254C RID: 9548
		protected abstract void HandleHandshakeMessage(byte type, byte[] buf);

		// Token: 0x0600254D RID: 9549 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void HandleWarningMessage(byte description)
		{
		}

		// Token: 0x0600254E RID: 9550 RVA: 0x000BCAD4 File Offset: 0x000BACD4
		protected virtual void ApplyMaxFragmentLengthExtension()
		{
			if (this.mSecurityParameters.maxFragmentLength >= 0)
			{
				if (!MaxFragmentLength.IsValid((byte)this.mSecurityParameters.maxFragmentLength))
				{
					throw new TlsFatalAlert(80);
				}
				int plaintextLimit = 1 << (int)(8 + this.mSecurityParameters.maxFragmentLength);
				this.mRecordStream.SetPlaintextLimit(plaintextLimit);
			}
		}

		// Token: 0x0600254F RID: 9551 RVA: 0x000BCB29 File Offset: 0x000BAD29
		protected virtual void CheckReceivedChangeCipherSpec(bool expected)
		{
			if (expected != this.mReceivedChangeCipherSpec)
			{
				throw new TlsFatalAlert(10);
			}
		}

		// Token: 0x06002550 RID: 9552 RVA: 0x000BCB3C File Offset: 0x000BAD3C
		protected virtual void CleanupHandshake()
		{
			if (this.mExpectedVerifyData != null)
			{
				Arrays.Fill(this.mExpectedVerifyData, 0);
				this.mExpectedVerifyData = null;
			}
			this.mSecurityParameters.Clear();
			this.mPeerCertificate = null;
			this.mOfferedCipherSuites = null;
			this.mOfferedCompressionMethods = null;
			this.mClientExtensions = null;
			this.mServerExtensions = null;
			this.mResumedSession = false;
			this.mReceivedChangeCipherSpec = false;
			this.mSecureRenegotiation = false;
			this.mAllowCertificateStatus = false;
			this.mExpectSessionTicket = false;
		}

		// Token: 0x06002551 RID: 9553 RVA: 0x000BCBB5 File Offset: 0x000BADB5
		protected virtual void BlockForHandshake()
		{
			if (this.mBlocking)
			{
				while (this.mConnectionState != 16)
				{
					bool flag = this.mClosed;
					this.SafeReadRecord();
				}
			}
		}

		// Token: 0x06002552 RID: 9554 RVA: 0x000BCBDC File Offset: 0x000BADDC
		protected virtual void CompleteHandshake()
		{
			try
			{
				this.mRecordStream.FinaliseHandshake();
				this.mAppDataSplitEnabled = !TlsUtilities.IsTlsV11(this.Context);
				if (!this.mAppDataReady)
				{
					this.mAppDataReady = true;
					if (this.mBlocking)
					{
						this.mTlsStream = new TlsStream(this);
					}
				}
				if (this.mTlsSession != null)
				{
					if (this.mSessionParameters == null)
					{
						this.mSessionParameters = new SessionParameters.Builder().SetCipherSuite(this.mSecurityParameters.CipherSuite).SetCompressionAlgorithm(this.mSecurityParameters.CompressionAlgorithm).SetMasterSecret(this.mSecurityParameters.MasterSecret).SetPeerCertificate(this.mPeerCertificate).SetPskIdentity(this.mSecurityParameters.PskIdentity).SetSrpIdentity(this.mSecurityParameters.SrpIdentity).SetServerExtensions(this.mServerExtensions).Build();
						this.mTlsSession = new TlsSessionImpl(this.mTlsSession.SessionID, this.mSessionParameters);
					}
					this.ContextAdmin.SetResumableSession(this.mTlsSession);
				}
				this.Peer.NotifyHandshakeComplete();
			}
			finally
			{
				this.CleanupHandshake();
			}
		}

		// Token: 0x06002553 RID: 9555 RVA: 0x000BCD1C File Offset: 0x000BAF1C
		protected internal void ProcessRecord(byte protocol, byte[] buf, int offset, int len)
		{
			switch (protocol)
			{
			case 20:
				this.ProcessChangeCipherSpec(buf, offset, len);
				return;
			case 21:
				this.mAlertQueue.AddData(buf, offset, len);
				this.ProcessAlert();
				return;
			case 22:
				this.mHandshakeQueue.AddData(buf, offset, len);
				this.ProcessHandshake();
				return;
			case 23:
				if (!this.mAppDataReady)
				{
					throw new TlsFatalAlert(10);
				}
				this.mApplicationDataQueue.AddData(buf, offset, len);
				this.ProcessApplicationData();
				return;
			case 24:
				if (!this.mAppDataReady)
				{
					throw new TlsFatalAlert(10);
				}
				return;
			default:
				return;
			}
		}

		// Token: 0x06002554 RID: 9556 RVA: 0x000BCDB8 File Offset: 0x000BAFB8
		private void ProcessHandshake()
		{
			bool flag;
			do
			{
				flag = false;
				if (this.mHandshakeQueue.Available >= 4)
				{
					byte[] array = new byte[4];
					this.mHandshakeQueue.Read(array, 0, 4, 0);
					byte b = TlsUtilities.ReadUint8(array, 0);
					int num = TlsUtilities.ReadUint24(array, 1);
					if (this.mHandshakeQueue.Available >= num + 4)
					{
						byte[] array2 = this.mHandshakeQueue.RemoveData(num, 4);
						this.CheckReceivedChangeCipherSpec(this.mConnectionState == 16 || b == 20);
						if (b != 0)
						{
							TlsContext context = this.Context;
							if (b == 20 && this.mExpectedVerifyData == null && context.SecurityParameters.MasterSecret != null)
							{
								this.mExpectedVerifyData = this.CreateVerifyData(!context.IsServer);
							}
							this.mRecordStream.UpdateHandshakeData(array, 0, 4);
							this.mRecordStream.UpdateHandshakeData(array2, 0, num);
						}
						this.HandleHandshakeMessage(b, array2);
						flag = true;
					}
				}
			}
			while (flag);
		}

		// Token: 0x06002555 RID: 9557 RVA: 0x00003022 File Offset: 0x00001222
		private void ProcessApplicationData()
		{
		}

		// Token: 0x06002556 RID: 9558 RVA: 0x000BCEA8 File Offset: 0x000BB0A8
		private void ProcessAlert()
		{
			while (this.mAlertQueue.Available >= 2)
			{
				byte[] array = this.mAlertQueue.RemoveData(2, 0);
				byte b = array[0];
				byte b2 = array[1];
				this.Peer.NotifyAlertReceived(b, b2);
				if (b == 2)
				{
					this.InvalidateSession();
					this.mFailedWithError = true;
					this.mClosed = true;
					this.mRecordStream.SafeClose();
					throw new IOException(TlsProtocol.TLS_ERROR_MESSAGE);
				}
				if (b2 == 0)
				{
					this.HandleClose(false);
				}
				this.HandleWarningMessage(b2);
			}
		}

		// Token: 0x06002557 RID: 9559 RVA: 0x000BCF2C File Offset: 0x000BB12C
		private void ProcessChangeCipherSpec(byte[] buf, int off, int len)
		{
			for (int i = 0; i < len; i++)
			{
				if (TlsUtilities.ReadUint8(buf, off + i) != 1)
				{
					throw new TlsFatalAlert(50);
				}
				if (this.mReceivedChangeCipherSpec || this.mAlertQueue.Available > 0 || this.mHandshakeQueue.Available > 0)
				{
					throw new TlsFatalAlert(10);
				}
				this.mRecordStream.ReceivedReadCipherSpec();
				this.mReceivedChangeCipherSpec = true;
				this.HandleChangeCipherSpecMessage();
			}
		}

		// Token: 0x06002558 RID: 9560 RVA: 0x000BCF9D File Offset: 0x000BB19D
		protected internal virtual int ApplicationDataAvailable()
		{
			return this.mApplicationDataQueue.Available;
		}

		// Token: 0x06002559 RID: 9561 RVA: 0x000BCFAC File Offset: 0x000BB1AC
		protected internal virtual int ReadApplicationData(byte[] buf, int offset, int len)
		{
			if (len < 1)
			{
				return 0;
			}
			while (this.mApplicationDataQueue.Available == 0)
			{
				if (this.mClosed)
				{
					if (this.mFailedWithError)
					{
						throw new IOException(TlsProtocol.TLS_ERROR_MESSAGE);
					}
					return 0;
				}
				else
				{
					this.SafeReadRecord();
				}
			}
			len = Math.Min(len, this.mApplicationDataQueue.Available);
			this.mApplicationDataQueue.RemoveData(buf, offset, len, 0);
			return len;
		}

		// Token: 0x0600255A RID: 9562 RVA: 0x000BD018 File Offset: 0x000BB218
		protected virtual void SafeReadRecord()
		{
			try
			{
				if (!this.mRecordStream.ReadRecord())
				{
					throw new EndOfStreamException();
				}
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				if (!this.mClosed)
				{
					this.FailWithError(2, tlsFatalAlert.AlertDescription, "Failed to read record", tlsFatalAlert);
				}
				throw tlsFatalAlert;
			}
			catch (Exception ex)
			{
				if (!this.mClosed)
				{
					this.FailWithError(2, 80, "Failed to read record", ex);
				}
				throw ex;
			}
		}

		// Token: 0x0600255B RID: 9563 RVA: 0x000BD094 File Offset: 0x000BB294
		protected virtual void SafeWriteRecord(byte type, byte[] buf, int offset, int len)
		{
			try
			{
				this.mRecordStream.WriteRecord(type, buf, offset, len);
			}
			catch (TlsFatalAlert tlsFatalAlert)
			{
				if (!this.mClosed)
				{
					this.FailWithError(2, tlsFatalAlert.AlertDescription, "Failed to write record", tlsFatalAlert);
				}
				throw tlsFatalAlert;
			}
			catch (Exception ex)
			{
				if (!this.mClosed)
				{
					this.FailWithError(2, 80, "Failed to write record", ex);
				}
				throw ex;
			}
		}

		// Token: 0x0600255C RID: 9564 RVA: 0x000BD10C File Offset: 0x000BB30C
		protected internal virtual void WriteData(byte[] buf, int offset, int len)
		{
			if (!this.mClosed)
			{
				while (len > 0)
				{
					if (this.mAppDataSplitEnabled)
					{
						switch (this.mAppDataSplitMode)
						{
						case 1:
							this.SafeWriteRecord(23, TlsUtilities.EmptyBytes, 0, 0);
							goto IL_94;
						case 2:
							this.mAppDataSplitEnabled = false;
							this.SafeWriteRecord(23, TlsUtilities.EmptyBytes, 0, 0);
							goto IL_94;
						}
						this.SafeWriteRecord(23, buf, offset, 1);
						offset++;
						len--;
					}
					IL_94:
					if (len > 0)
					{
						int num = Math.Min(len, this.mRecordStream.GetPlaintextLimit());
						this.SafeWriteRecord(23, buf, offset, num);
						offset += num;
						len -= num;
					}
				}
				return;
			}
			if (this.mFailedWithError)
			{
				throw new IOException(TlsProtocol.TLS_ERROR_MESSAGE);
			}
			throw new IOException("Sorry, connection has been closed, you cannot write more data");
		}

		// Token: 0x0600255D RID: 9565 RVA: 0x000BD1DF File Offset: 0x000BB3DF
		protected virtual void SetAppDataSplitMode(int appDataSplitMode)
		{
			if (appDataSplitMode < 0 || appDataSplitMode > 2)
			{
				throw new ArgumentException("Illegal appDataSplitMode mode: " + appDataSplitMode, "appDataSplitMode");
			}
			this.mAppDataSplitMode = appDataSplitMode;
		}

		// Token: 0x0600255E RID: 9566 RVA: 0x000BD210 File Offset: 0x000BB410
		protected virtual void WriteHandshakeMessage(byte[] buf, int off, int len)
		{
			while (len > 0)
			{
				int num = Math.Min(len, this.mRecordStream.GetPlaintextLimit());
				this.SafeWriteRecord(22, buf, off, num);
				off += num;
				len -= num;
			}
		}

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600255F RID: 9567 RVA: 0x000BD24A File Offset: 0x000BB44A
		public virtual Stream Stream
		{
			get
			{
				if (!this.mBlocking)
				{
					throw new InvalidOperationException("Cannot use Stream in non-blocking mode! Use OfferInput()/OfferOutput() instead.");
				}
				return this.mTlsStream;
			}
		}

		// Token: 0x06002560 RID: 9568 RVA: 0x000BD268 File Offset: 0x000BB468
		public virtual void OfferInput(byte[] input)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use OfferInput() in blocking mode! Use Stream instead.");
			}
			if (this.mClosed)
			{
				throw new IOException("Connection is closed, cannot accept any more input");
			}
			this.mInputBuffers.Write(input);
			while (this.mInputBuffers.Available >= 5)
			{
				byte[] buf = new byte[5];
				this.mInputBuffers.Peek(buf);
				int num = TlsUtilities.ReadUint16(buf, 3) + 5;
				if (this.mInputBuffers.Available < num)
				{
					break;
				}
				this.SafeReadRecord();
			}
		}

		// Token: 0x06002561 RID: 9569 RVA: 0x000BD2EB File Offset: 0x000BB4EB
		public virtual int GetAvailableInputBytes()
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use GetAvailableInputBytes() in blocking mode! Use ApplicationDataAvailable() instead.");
			}
			return this.ApplicationDataAvailable();
		}

		// Token: 0x06002562 RID: 9570 RVA: 0x000BD306 File Offset: 0x000BB506
		public virtual int ReadInput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use ReadInput() in blocking mode! Use Stream instead.");
			}
			return this.ReadApplicationData(buffer, offset, Math.Min(length, this.ApplicationDataAvailable()));
		}

		// Token: 0x06002563 RID: 9571 RVA: 0x000BD32F File Offset: 0x000BB52F
		public virtual void OfferOutput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use OfferOutput() in blocking mode! Use Stream instead.");
			}
			if (!this.mAppDataReady)
			{
				throw new IOException("Application data cannot be sent until the handshake is complete!");
			}
			this.WriteData(buffer, offset, length);
		}

		// Token: 0x06002564 RID: 9572 RVA: 0x000BD362 File Offset: 0x000BB562
		public virtual int GetAvailableOutputBytes()
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use GetAvailableOutputBytes() in blocking mode! Use Stream instead.");
			}
			return this.mOutputBuffer.Available;
		}

		// Token: 0x06002565 RID: 9573 RVA: 0x000BD382 File Offset: 0x000BB582
		public virtual int ReadOutput(byte[] buffer, int offset, int length)
		{
			if (this.mBlocking)
			{
				throw new InvalidOperationException("Cannot use ReadOutput() in blocking mode! Use Stream instead.");
			}
			return this.mOutputBuffer.Read(buffer, offset, length);
		}

		// Token: 0x06002566 RID: 9574 RVA: 0x000BD3A8 File Offset: 0x000BB5A8
		protected virtual void FailWithError(byte alertLevel, byte alertDescription, string message, Exception cause)
		{
			if (!this.mClosed)
			{
				this.mClosed = true;
				if (alertLevel == 2)
				{
					this.InvalidateSession();
					this.mFailedWithError = true;
				}
				this.RaiseAlert(alertLevel, alertDescription, message, cause);
				this.mRecordStream.SafeClose();
				if (alertLevel != 2)
				{
					return;
				}
			}
			throw new IOException(TlsProtocol.TLS_ERROR_MESSAGE);
		}

		// Token: 0x06002567 RID: 9575 RVA: 0x000BD400 File Offset: 0x000BB600
		protected virtual void InvalidateSession()
		{
			if (this.mSessionParameters != null)
			{
				this.mSessionParameters.Clear();
				this.mSessionParameters = null;
			}
			if (this.mTlsSession != null)
			{
				this.mTlsSession.Invalidate();
				this.mTlsSession = null;
			}
		}

		// Token: 0x06002568 RID: 9576 RVA: 0x000BD438 File Offset: 0x000BB638
		protected virtual void ProcessFinishedMessage(MemoryStream buf)
		{
			if (this.mExpectedVerifyData == null)
			{
				throw new TlsFatalAlert(80);
			}
			byte[] b = TlsUtilities.ReadFully(this.mExpectedVerifyData.Length, buf);
			TlsProtocol.AssertEmpty(buf);
			if (!Arrays.ConstantTimeAreEqual(this.mExpectedVerifyData, b))
			{
				throw new TlsFatalAlert(51);
			}
		}

		// Token: 0x06002569 RID: 9577 RVA: 0x000BD480 File Offset: 0x000BB680
		protected virtual void RaiseAlert(byte alertLevel, byte alertDescription, string message, Exception cause)
		{
			this.Peer.NotifyAlertRaised(alertLevel, alertDescription, message, cause);
			byte[] buf = new byte[]
			{
				alertLevel,
				alertDescription
			};
			this.SafeWriteRecord(21, buf, 0, 2);
		}

		// Token: 0x0600256A RID: 9578 RVA: 0x000BD4B7 File Offset: 0x000BB6B7
		protected virtual void RaiseWarning(byte alertDescription, string message)
		{
			this.RaiseAlert(1, alertDescription, message, null);
		}

		// Token: 0x0600256B RID: 9579 RVA: 0x000BD4C4 File Offset: 0x000BB6C4
		protected virtual void SendCertificateMessage(Certificate certificate)
		{
			if (certificate == null)
			{
				certificate = Certificate.EmptyChain;
			}
			if (certificate.IsEmpty && !this.Context.IsServer)
			{
				ProtocolVersion serverVersion = this.Context.ServerVersion;
				if (serverVersion.IsSsl)
				{
					string message = serverVersion.ToString() + " client didn't provide credentials";
					this.RaiseWarning(41, message);
					return;
				}
			}
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(11);
			certificate.Encode(handshakeMessage);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x0600256C RID: 9580 RVA: 0x000BD538 File Offset: 0x000BB738
		protected virtual void SendChangeCipherSpecMessage()
		{
			byte[] array = new byte[]
			{
				1
			};
			this.SafeWriteRecord(20, array, 0, array.Length);
			this.mRecordStream.SentWriteCipherSpec();
		}

		// Token: 0x0600256D RID: 9581 RVA: 0x000BD568 File Offset: 0x000BB768
		protected virtual void SendFinishedMessage()
		{
			byte[] array = this.CreateVerifyData(this.Context.IsServer);
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(20, array.Length);
			handshakeMessage.Write(array, 0, array.Length);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x0600256E RID: 9582 RVA: 0x000BD5A2 File Offset: 0x000BB7A2
		protected virtual void SendSupplementalDataMessage(IList supplementalData)
		{
			TlsProtocol.HandshakeMessage handshakeMessage = new TlsProtocol.HandshakeMessage(23);
			TlsProtocol.WriteSupplementalData(handshakeMessage, supplementalData);
			handshakeMessage.WriteToRecordStream(this);
		}

		// Token: 0x0600256F RID: 9583 RVA: 0x000BD5B8 File Offset: 0x000BB7B8
		protected virtual byte[] CreateVerifyData(bool isServer)
		{
			TlsContext context = this.Context;
			string asciiLabel = isServer ? "server finished" : "client finished";
			byte[] sslSender = isServer ? TlsUtilities.SSL_SERVER : TlsUtilities.SSL_CLIENT;
			byte[] currentPrfHash = TlsProtocol.GetCurrentPrfHash(context, this.mRecordStream.HandshakeHash, sslSender);
			return TlsUtilities.CalculateVerifyData(context, asciiLabel, currentPrfHash);
		}

		// Token: 0x06002570 RID: 9584 RVA: 0x000BD605 File Offset: 0x000BB805
		public virtual void Close()
		{
			this.HandleClose(true);
		}

		// Token: 0x06002571 RID: 9585 RVA: 0x000BD60E File Offset: 0x000BB80E
		protected virtual void HandleClose(bool user_canceled)
		{
			if (!this.mClosed)
			{
				if (user_canceled && !this.mAppDataReady)
				{
					this.RaiseWarning(90, "User canceled handshake");
				}
				this.FailWithError(1, 0, "Connection closed", null);
			}
		}

		// Token: 0x06002572 RID: 9586 RVA: 0x000BD642 File Offset: 0x000BB842
		protected internal virtual void Flush()
		{
			this.mRecordStream.Flush();
		}

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06002573 RID: 9587 RVA: 0x000BD64F File Offset: 0x000BB84F
		public virtual bool IsClosed
		{
			get
			{
				return this.mClosed;
			}
		}

		// Token: 0x06002574 RID: 9588 RVA: 0x000BD65C File Offset: 0x000BB85C
		protected virtual short ProcessMaxFragmentLengthExtension(IDictionary clientExtensions, IDictionary serverExtensions, byte alertDescription)
		{
			short maxFragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension(serverExtensions);
			if (maxFragmentLengthExtension >= 0 && (!MaxFragmentLength.IsValid((byte)maxFragmentLengthExtension) || (!this.mResumedSession && maxFragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension(clientExtensions))))
			{
				throw new TlsFatalAlert(alertDescription);
			}
			return maxFragmentLengthExtension;
		}

		// Token: 0x06002575 RID: 9589 RVA: 0x000BD696 File Offset: 0x000BB896
		protected virtual void RefuseRenegotiation()
		{
			if (TlsUtilities.IsSsl(this.Context))
			{
				throw new TlsFatalAlert(40);
			}
			this.RaiseWarning(100, "Renegotiation not supported");
		}

		// Token: 0x06002576 RID: 9590 RVA: 0x000BD6BA File Offset: 0x000BB8BA
		protected internal static void AssertEmpty(MemoryStream buf)
		{
			if (buf.Position < buf.Length)
			{
				throw new TlsFatalAlert(50);
			}
		}

		// Token: 0x06002577 RID: 9591 RVA: 0x000BD6D4 File Offset: 0x000BB8D4
		protected internal static byte[] CreateRandomBlock(bool useGmtUnixTime, IRandomGenerator randomGenerator)
		{
			byte[] array = new byte[32];
			randomGenerator.NextBytes(array);
			if (useGmtUnixTime)
			{
				TlsUtilities.WriteGmtUnixTime(array, 0);
			}
			return array;
		}

		// Token: 0x06002578 RID: 9592 RVA: 0x000BD6FB File Offset: 0x000BB8FB
		protected internal static byte[] CreateRenegotiationInfo(byte[] renegotiated_connection)
		{
			return TlsUtilities.EncodeOpaque8(renegotiated_connection);
		}

		// Token: 0x06002579 RID: 9593 RVA: 0x000BD704 File Offset: 0x000BB904
		protected internal static void EstablishMasterSecret(TlsContext context, TlsKeyExchange keyExchange)
		{
			byte[] array = keyExchange.GeneratePremasterSecret();
			try
			{
				context.SecurityParameters.masterSecret = TlsUtilities.CalculateMasterSecret(context, array);
			}
			finally
			{
				if (array != null)
				{
					Arrays.Fill(array, 0);
				}
			}
		}

		// Token: 0x0600257A RID: 9594 RVA: 0x000BD748 File Offset: 0x000BB948
		protected internal static byte[] GetCurrentPrfHash(TlsContext context, TlsHandshakeHash handshakeHash, byte[] sslSender)
		{
			IDigest digest = handshakeHash.ForkPrfHash();
			if (sslSender != null && TlsUtilities.IsSsl(context))
			{
				digest.BlockUpdate(sslSender, 0, sslSender.Length);
			}
			return DigestUtilities.DoFinal(digest);
		}

		// Token: 0x0600257B RID: 9595 RVA: 0x000BD778 File Offset: 0x000BB978
		protected internal static IDictionary ReadExtensions(MemoryStream input)
		{
			if (input.Position >= input.Length)
			{
				return null;
			}
			byte[] buffer = TlsUtilities.ReadOpaque16(input);
			TlsProtocol.AssertEmpty(input);
			MemoryStream memoryStream = new MemoryStream(buffer, false);
			IDictionary dictionary = Platform.CreateHashtable();
			while (memoryStream.Position < memoryStream.Length)
			{
				int num = TlsUtilities.ReadUint16(memoryStream);
				byte[] value = TlsUtilities.ReadOpaque16(memoryStream);
				if (dictionary.Contains(num))
				{
					throw new TlsFatalAlert(47);
				}
				dictionary.Add(num, value);
			}
			return dictionary;
		}

		// Token: 0x0600257C RID: 9596 RVA: 0x000BD7F0 File Offset: 0x000BB9F0
		protected internal static IList ReadSupplementalDataMessage(MemoryStream input)
		{
			byte[] buffer = TlsUtilities.ReadOpaque24(input);
			TlsProtocol.AssertEmpty(input);
			MemoryStream memoryStream = new MemoryStream(buffer, false);
			IList list = Platform.CreateArrayList();
			while (memoryStream.Position < memoryStream.Length)
			{
				int dataType = TlsUtilities.ReadUint16(memoryStream);
				byte[] data = TlsUtilities.ReadOpaque16(memoryStream);
				list.Add(new SupplementalDataEntry(dataType, data));
			}
			return list;
		}

		// Token: 0x0600257D RID: 9597 RVA: 0x000BD843 File Offset: 0x000BBA43
		protected internal static void WriteExtensions(Stream output, IDictionary extensions)
		{
			MemoryStream memoryStream = new MemoryStream();
			TlsProtocol.WriteSelectedExtensions(memoryStream, extensions, true);
			TlsProtocol.WriteSelectedExtensions(memoryStream, extensions, false);
			TlsUtilities.WriteOpaque16(memoryStream.ToArray(), output);
		}

		// Token: 0x0600257E RID: 9598 RVA: 0x000BD868 File Offset: 0x000BBA68
		protected internal static void WriteSelectedExtensions(Stream output, IDictionary extensions, bool selectEmpty)
		{
			foreach (object obj in extensions.Keys)
			{
				int num = (int)obj;
				byte[] array = (byte[])extensions[num];
				if (selectEmpty == (array.Length == 0))
				{
					TlsUtilities.CheckUint16(num);
					TlsUtilities.WriteUint16(num, output);
					TlsUtilities.WriteOpaque16(array, output);
				}
			}
		}

		// Token: 0x0600257F RID: 9599 RVA: 0x000BD8E8 File Offset: 0x000BBAE8
		protected internal static void WriteSupplementalData(Stream output, IList supplementalData)
		{
			MemoryStream memoryStream = new MemoryStream();
			foreach (object obj in supplementalData)
			{
				SupplementalDataEntry supplementalDataEntry = (SupplementalDataEntry)obj;
				int dataType = supplementalDataEntry.DataType;
				TlsUtilities.CheckUint16(dataType);
				TlsUtilities.WriteUint16(dataType, memoryStream);
				TlsUtilities.WriteOpaque16(supplementalDataEntry.Data, memoryStream);
			}
			TlsUtilities.WriteOpaque24(memoryStream.ToArray(), output);
		}

		// Token: 0x06002580 RID: 9600 RVA: 0x000BD964 File Offset: 0x000BBB64
		protected internal static int GetPrfAlgorithm(TlsContext context, int ciphersuite)
		{
			bool flag = TlsUtilities.IsTlsV12(context);
			if (ciphersuite <= 197)
			{
				if (ciphersuite - 59 > 5 && ciphersuite - 103 > 4)
				{
					switch (ciphersuite)
					{
					case 156:
					case 158:
					case 160:
					case 162:
					case 164:
					case 168:
					case 170:
					case 172:
					case 186:
					case 187:
					case 188:
					case 189:
					case 190:
					case 191:
					case 192:
					case 193:
					case 194:
					case 195:
					case 196:
					case 197:
						break;
					case 157:
					case 159:
					case 161:
					case 163:
					case 165:
					case 169:
					case 171:
					case 173:
						goto IL_357;
					case 166:
					case 167:
					case 174:
					case 176:
					case 178:
					case 180:
					case 182:
					case 184:
						goto IL_36B;
					case 175:
					case 177:
					case 179:
					case 181:
					case 183:
					case 185:
						goto IL_364;
					default:
						goto IL_36B;
					}
				}
			}
			else if (ciphersuite <= 52398)
			{
				switch (ciphersuite)
				{
				case 49187:
				case 49189:
				case 49191:
				case 49193:
				case 49195:
				case 49197:
				case 49199:
				case 49201:
				case 49266:
				case 49268:
				case 49270:
				case 49272:
				case 49274:
				case 49276:
				case 49278:
				case 49280:
				case 49282:
				case 49284:
				case 49286:
				case 49288:
				case 49290:
				case 49292:
				case 49294:
				case 49296:
				case 49298:
				case 49308:
				case 49309:
				case 49310:
				case 49311:
				case 49312:
				case 49313:
				case 49314:
				case 49315:
				case 49316:
				case 49317:
				case 49318:
				case 49319:
				case 49320:
				case 49321:
				case 49322:
				case 49323:
				case 49324:
				case 49325:
				case 49326:
				case 49327:
					break;
				case 49188:
				case 49190:
				case 49192:
				case 49194:
				case 49196:
				case 49198:
				case 49200:
				case 49202:
				case 49267:
				case 49269:
				case 49271:
				case 49273:
				case 49275:
				case 49277:
				case 49279:
				case 49281:
				case 49283:
				case 49285:
				case 49287:
				case 49289:
				case 49291:
				case 49293:
				case 49295:
				case 49297:
				case 49299:
					goto IL_357;
				case 49203:
				case 49204:
				case 49205:
				case 49206:
				case 49207:
				case 49209:
				case 49210:
				case 49212:
				case 49213:
				case 49214:
				case 49215:
				case 49216:
				case 49217:
				case 49218:
				case 49219:
				case 49220:
				case 49221:
				case 49222:
				case 49223:
				case 49224:
				case 49225:
				case 49226:
				case 49227:
				case 49228:
				case 49229:
				case 49230:
				case 49231:
				case 49232:
				case 49233:
				case 49234:
				case 49235:
				case 49236:
				case 49237:
				case 49238:
				case 49239:
				case 49240:
				case 49241:
				case 49242:
				case 49243:
				case 49244:
				case 49245:
				case 49246:
				case 49247:
				case 49248:
				case 49249:
				case 49250:
				case 49251:
				case 49252:
				case 49253:
				case 49254:
				case 49255:
				case 49256:
				case 49257:
				case 49258:
				case 49259:
				case 49260:
				case 49261:
				case 49262:
				case 49263:
				case 49264:
				case 49265:
				case 49300:
				case 49302:
				case 49304:
				case 49306:
					goto IL_36B;
				case 49208:
				case 49211:
				case 49301:
				case 49303:
				case 49305:
				case 49307:
					goto IL_364;
				default:
					if (ciphersuite - 52392 > 6)
					{
						goto IL_36B;
					}
					break;
				}
			}
			else if (ciphersuite - 65280 > 5 && ciphersuite - 65296 > 5)
			{
				goto IL_36B;
			}
			if (flag)
			{
				return 1;
			}
			throw new TlsFatalAlert(47);
			IL_357:
			if (flag)
			{
				return 2;
			}
			throw new TlsFatalAlert(47);
			IL_364:
			if (flag)
			{
				return 2;
			}
			return 0;
			IL_36B:
			if (flag)
			{
				return 1;
			}
			return 0;
		}

		// Token: 0x04001964 RID: 6500
		private static readonly string TLS_ERROR_MESSAGE = "Internal TLS error, this could be an attack";

		// Token: 0x04001965 RID: 6501
		protected const short CS_START = 0;

		// Token: 0x04001966 RID: 6502
		protected const short CS_CLIENT_HELLO = 1;

		// Token: 0x04001967 RID: 6503
		protected const short CS_SERVER_HELLO = 2;

		// Token: 0x04001968 RID: 6504
		protected const short CS_SERVER_SUPPLEMENTAL_DATA = 3;

		// Token: 0x04001969 RID: 6505
		protected const short CS_SERVER_CERTIFICATE = 4;

		// Token: 0x0400196A RID: 6506
		protected const short CS_CERTIFICATE_STATUS = 5;

		// Token: 0x0400196B RID: 6507
		protected const short CS_SERVER_KEY_EXCHANGE = 6;

		// Token: 0x0400196C RID: 6508
		protected const short CS_CERTIFICATE_REQUEST = 7;

		// Token: 0x0400196D RID: 6509
		protected const short CS_SERVER_HELLO_DONE = 8;

		// Token: 0x0400196E RID: 6510
		protected const short CS_CLIENT_SUPPLEMENTAL_DATA = 9;

		// Token: 0x0400196F RID: 6511
		protected const short CS_CLIENT_CERTIFICATE = 10;

		// Token: 0x04001970 RID: 6512
		protected const short CS_CLIENT_KEY_EXCHANGE = 11;

		// Token: 0x04001971 RID: 6513
		protected const short CS_CERTIFICATE_VERIFY = 12;

		// Token: 0x04001972 RID: 6514
		protected const short CS_CLIENT_FINISHED = 13;

		// Token: 0x04001973 RID: 6515
		protected const short CS_SERVER_SESSION_TICKET = 14;

		// Token: 0x04001974 RID: 6516
		protected const short CS_SERVER_FINISHED = 15;

		// Token: 0x04001975 RID: 6517
		protected const short CS_END = 16;

		// Token: 0x04001976 RID: 6518
		protected const short ADS_MODE_1_Nsub1 = 0;

		// Token: 0x04001977 RID: 6519
		protected const short ADS_MODE_0_N = 1;

		// Token: 0x04001978 RID: 6520
		protected const short ADS_MODE_0_N_FIRSTONLY = 2;

		// Token: 0x04001979 RID: 6521
		private ByteQueue mApplicationDataQueue;

		// Token: 0x0400197A RID: 6522
		private ByteQueue mAlertQueue;

		// Token: 0x0400197B RID: 6523
		private ByteQueue mHandshakeQueue;

		// Token: 0x0400197C RID: 6524
		internal RecordStream mRecordStream;

		// Token: 0x0400197D RID: 6525
		protected SecureRandom mSecureRandom;

		// Token: 0x0400197E RID: 6526
		private TlsStream mTlsStream;

		// Token: 0x0400197F RID: 6527
		private volatile bool mClosed;

		// Token: 0x04001980 RID: 6528
		private volatile bool mFailedWithError;

		// Token: 0x04001981 RID: 6529
		private volatile bool mAppDataReady;

		// Token: 0x04001982 RID: 6530
		private volatile bool mAppDataSplitEnabled;

		// Token: 0x04001983 RID: 6531
		private volatile int mAppDataSplitMode;

		// Token: 0x04001984 RID: 6532
		private byte[] mExpectedVerifyData;

		// Token: 0x04001985 RID: 6533
		protected TlsSession mTlsSession;

		// Token: 0x04001986 RID: 6534
		protected SessionParameters mSessionParameters;

		// Token: 0x04001987 RID: 6535
		protected SecurityParameters mSecurityParameters;

		// Token: 0x04001988 RID: 6536
		protected Certificate mPeerCertificate;

		// Token: 0x04001989 RID: 6537
		protected int[] mOfferedCipherSuites;

		// Token: 0x0400198A RID: 6538
		protected byte[] mOfferedCompressionMethods;

		// Token: 0x0400198B RID: 6539
		protected IDictionary mClientExtensions;

		// Token: 0x0400198C RID: 6540
		protected IDictionary mServerExtensions;

		// Token: 0x0400198D RID: 6541
		protected short mConnectionState;

		// Token: 0x0400198E RID: 6542
		protected bool mResumedSession;

		// Token: 0x0400198F RID: 6543
		protected bool mReceivedChangeCipherSpec;

		// Token: 0x04001990 RID: 6544
		protected bool mSecureRenegotiation;

		// Token: 0x04001991 RID: 6545
		protected bool mAllowCertificateStatus;

		// Token: 0x04001992 RID: 6546
		protected bool mExpectSessionTicket;

		// Token: 0x04001993 RID: 6547
		protected bool mBlocking;

		// Token: 0x04001994 RID: 6548
		protected ByteQueueStream mInputBuffers;

		// Token: 0x04001995 RID: 6549
		protected ByteQueueStream mOutputBuffer;

		// Token: 0x02000886 RID: 2182
		internal class HandshakeMessage : MemoryStream
		{
			// Token: 0x0600456B RID: 17771 RVA: 0x00144AEB File Offset: 0x00142CEB
			internal HandshakeMessage(byte handshakeType) : this(handshakeType, 60)
			{
			}

			// Token: 0x0600456C RID: 17772 RVA: 0x00144AF6 File Offset: 0x00142CF6
			internal HandshakeMessage(byte handshakeType, int length) : base(length + 4)
			{
				TlsUtilities.WriteUint8(handshakeType, this);
				TlsUtilities.WriteUint24(0, this);
			}

			// Token: 0x0600456D RID: 17773 RVA: 0x00080502 File Offset: 0x0007E702
			internal void Write(byte[] data)
			{
				this.Write(data, 0, data.Length);
			}

			// Token: 0x0600456E RID: 17774 RVA: 0x00144B10 File Offset: 0x00142D10
			internal void WriteToRecordStream(TlsProtocol protocol)
			{
				long num = this.Length - 4L;
				TlsUtilities.CheckUint24(num);
				this.Position = 1L;
				TlsUtilities.WriteUint24((int)num, this);
				byte[] buffer = this.GetBuffer();
				int len = (int)this.Length;
				protocol.WriteHandshakeMessage(buffer, 0, len);
				Platform.Dispose(this);
			}
		}
	}
}
