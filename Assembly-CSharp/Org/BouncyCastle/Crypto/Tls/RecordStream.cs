using System;
using System.IO;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003CD RID: 973
	internal class RecordStream
	{
		// Token: 0x060023D7 RID: 9175 RVA: 0x000B8250 File Offset: 0x000B6450
		internal RecordStream(TlsProtocol handler, Stream input, Stream output)
		{
			this.mHandler = handler;
			this.mInput = input;
			this.mOutput = output;
			this.mReadCompression = new TlsNullCompression();
			this.mWriteCompression = this.mReadCompression;
		}

		// Token: 0x060023D8 RID: 9176 RVA: 0x000B82A1 File Offset: 0x000B64A1
		internal virtual void Init(TlsContext context)
		{
			this.mReadCipher = new TlsNullCipher(context);
			this.mWriteCipher = this.mReadCipher;
			this.mHandshakeHash = new DeferredHash();
			this.mHandshakeHash.Init(context);
			this.SetPlaintextLimit(16384);
		}

		// Token: 0x060023D9 RID: 9177 RVA: 0x000B82DD File Offset: 0x000B64DD
		internal virtual int GetPlaintextLimit()
		{
			return this.mPlaintextLimit;
		}

		// Token: 0x060023DA RID: 9178 RVA: 0x000B82E5 File Offset: 0x000B64E5
		internal virtual void SetPlaintextLimit(int plaintextLimit)
		{
			this.mPlaintextLimit = plaintextLimit;
			this.mCompressedLimit = this.mPlaintextLimit + 1024;
			this.mCiphertextLimit = this.mCompressedLimit + 1024;
		}

		// Token: 0x17000402 RID: 1026
		// (get) Token: 0x060023DB RID: 9179 RVA: 0x000B8312 File Offset: 0x000B6512
		// (set) Token: 0x060023DC RID: 9180 RVA: 0x000B831A File Offset: 0x000B651A
		internal virtual ProtocolVersion ReadVersion
		{
			get
			{
				return this.mReadVersion;
			}
			set
			{
				this.mReadVersion = value;
			}
		}

		// Token: 0x060023DD RID: 9181 RVA: 0x000B8323 File Offset: 0x000B6523
		internal virtual void SetWriteVersion(ProtocolVersion writeVersion)
		{
			this.mWriteVersion = writeVersion;
		}

		// Token: 0x060023DE RID: 9182 RVA: 0x000B832C File Offset: 0x000B652C
		internal virtual void SetRestrictReadVersion(bool enabled)
		{
			this.mRestrictReadVersion = enabled;
		}

		// Token: 0x060023DF RID: 9183 RVA: 0x000B8335 File Offset: 0x000B6535
		internal virtual void SetPendingConnectionState(TlsCompression tlsCompression, TlsCipher tlsCipher)
		{
			this.mPendingCompression = tlsCompression;
			this.mPendingCipher = tlsCipher;
		}

		// Token: 0x060023E0 RID: 9184 RVA: 0x000B8345 File Offset: 0x000B6545
		internal virtual void SentWriteCipherSpec()
		{
			if (this.mPendingCompression == null || this.mPendingCipher == null)
			{
				throw new TlsFatalAlert(40);
			}
			this.mWriteCompression = this.mPendingCompression;
			this.mWriteCipher = this.mPendingCipher;
			this.mWriteSeqNo = 0L;
		}

		// Token: 0x060023E1 RID: 9185 RVA: 0x000B837F File Offset: 0x000B657F
		internal virtual void ReceivedReadCipherSpec()
		{
			if (this.mPendingCompression == null || this.mPendingCipher == null)
			{
				throw new TlsFatalAlert(40);
			}
			this.mReadCompression = this.mPendingCompression;
			this.mReadCipher = this.mPendingCipher;
			this.mReadSeqNo = 0L;
		}

		// Token: 0x060023E2 RID: 9186 RVA: 0x000B83BC File Offset: 0x000B65BC
		internal virtual void FinaliseHandshake()
		{
			if (this.mReadCompression != this.mPendingCompression || this.mWriteCompression != this.mPendingCompression || this.mReadCipher != this.mPendingCipher || this.mWriteCipher != this.mPendingCipher)
			{
				throw new TlsFatalAlert(40);
			}
			this.mPendingCompression = null;
			this.mPendingCipher = null;
		}

		// Token: 0x060023E3 RID: 9187 RVA: 0x000B8418 File Offset: 0x000B6618
		internal virtual bool ReadRecord()
		{
			byte[] array = TlsUtilities.ReadAllOrNothing(5, this.mInput);
			if (array == null)
			{
				return false;
			}
			byte b = TlsUtilities.ReadUint8(array, 0);
			RecordStream.CheckType(b, 10);
			if (!this.mRestrictReadVersion)
			{
				if (((long)TlsUtilities.ReadVersionRaw(array, 1) & (long)((ulong)-256)) != 768L)
				{
					throw new TlsFatalAlert(47);
				}
			}
			else
			{
				ProtocolVersion protocolVersion = TlsUtilities.ReadVersion(array, 1);
				if (this.mReadVersion == null)
				{
					this.mReadVersion = protocolVersion;
				}
				else if (!protocolVersion.Equals(this.mReadVersion))
				{
					throw new TlsFatalAlert(47);
				}
			}
			int len = TlsUtilities.ReadUint16(array, 3);
			byte[] array2 = this.DecodeAndVerify(b, this.mInput, len);
			this.mHandler.ProcessRecord(b, array2, 0, array2.Length);
			return true;
		}

		// Token: 0x060023E4 RID: 9188 RVA: 0x000B84CC File Offset: 0x000B66CC
		internal virtual byte[] DecodeAndVerify(byte type, Stream input, int len)
		{
			RecordStream.CheckLength(len, this.mCiphertextLimit, 22);
			byte[] array = TlsUtilities.ReadFully(len, input);
			TlsCipher tlsCipher = this.mReadCipher;
			long num = this.mReadSeqNo;
			this.mReadSeqNo = num + 1L;
			byte[] array2 = tlsCipher.DecodeCiphertext(num, type, array, 0, array.Length);
			RecordStream.CheckLength(array2.Length, this.mCompressedLimit, 22);
			Stream stream = this.mReadCompression.Decompress(this.mBuffer);
			if (stream != this.mBuffer)
			{
				stream.Write(array2, 0, array2.Length);
				stream.Flush();
				array2 = this.GetBufferContents();
			}
			RecordStream.CheckLength(array2.Length, this.mPlaintextLimit, 30);
			if (array2.Length < 1 && type != 23)
			{
				throw new TlsFatalAlert(47);
			}
			return array2;
		}

		// Token: 0x060023E5 RID: 9189 RVA: 0x000B857C File Offset: 0x000B677C
		internal virtual void WriteRecord(byte type, byte[] plaintext, int plaintextOffset, int plaintextLength)
		{
			if (this.mWriteVersion == null)
			{
				return;
			}
			RecordStream.CheckType(type, 80);
			RecordStream.CheckLength(plaintextLength, this.mPlaintextLimit, 80);
			if (plaintextLength < 1 && type != 23)
			{
				throw new TlsFatalAlert(80);
			}
			if (type == 22)
			{
				this.UpdateHandshakeData(plaintext, plaintextOffset, plaintextLength);
			}
			Stream stream = this.mWriteCompression.Compress(this.mBuffer);
			byte[] array;
			if (stream == this.mBuffer)
			{
				TlsCipher tlsCipher = this.mWriteCipher;
				long num = this.mWriteSeqNo;
				this.mWriteSeqNo = num + 1L;
				array = tlsCipher.EncodePlaintext(num, type, plaintext, plaintextOffset, plaintextLength);
			}
			else
			{
				stream.Write(plaintext, plaintextOffset, plaintextLength);
				stream.Flush();
				byte[] bufferContents = this.GetBufferContents();
				RecordStream.CheckLength(bufferContents.Length, plaintextLength + 1024, 80);
				TlsCipher tlsCipher2 = this.mWriteCipher;
				long num = this.mWriteSeqNo;
				this.mWriteSeqNo = num + 1L;
				array = tlsCipher2.EncodePlaintext(num, type, bufferContents, 0, bufferContents.Length);
			}
			RecordStream.CheckLength(array.Length, this.mCiphertextLimit, 80);
			byte[] array2 = new byte[array.Length + 5];
			TlsUtilities.WriteUint8(type, array2, 0);
			TlsUtilities.WriteVersion(this.mWriteVersion, array2, 1);
			TlsUtilities.WriteUint16(array.Length, array2, 3);
			Array.Copy(array, 0, array2, 5, array.Length);
			this.mOutput.Write(array2, 0, array2.Length);
			this.mOutput.Flush();
		}

		// Token: 0x060023E6 RID: 9190 RVA: 0x000B86BC File Offset: 0x000B68BC
		internal virtual void NotifyHelloComplete()
		{
			this.mHandshakeHash = this.mHandshakeHash.NotifyPrfDetermined();
		}

		// Token: 0x17000403 RID: 1027
		// (get) Token: 0x060023E7 RID: 9191 RVA: 0x000B86CF File Offset: 0x000B68CF
		internal virtual TlsHandshakeHash HandshakeHash
		{
			get
			{
				return this.mHandshakeHash;
			}
		}

		// Token: 0x060023E8 RID: 9192 RVA: 0x000B86D7 File Offset: 0x000B68D7
		internal virtual TlsHandshakeHash PrepareToFinish()
		{
			TlsHandshakeHash result = this.mHandshakeHash;
			this.mHandshakeHash = this.mHandshakeHash.StopTracking();
			return result;
		}

		// Token: 0x060023E9 RID: 9193 RVA: 0x000B86F0 File Offset: 0x000B68F0
		internal virtual void UpdateHandshakeData(byte[] message, int offset, int len)
		{
			this.mHandshakeHash.BlockUpdate(message, offset, len);
		}

		// Token: 0x060023EA RID: 9194 RVA: 0x000B8700 File Offset: 0x000B6900
		internal virtual void SafeClose()
		{
			try
			{
				Platform.Dispose(this.mInput);
			}
			catch (IOException)
			{
			}
			try
			{
				Platform.Dispose(this.mOutput);
			}
			catch (IOException)
			{
			}
		}

		// Token: 0x060023EB RID: 9195 RVA: 0x000B874C File Offset: 0x000B694C
		internal virtual void Flush()
		{
			this.mOutput.Flush();
		}

		// Token: 0x060023EC RID: 9196 RVA: 0x000B8759 File Offset: 0x000B6959
		private byte[] GetBufferContents()
		{
			byte[] result = this.mBuffer.ToArray();
			this.mBuffer.SetLength(0L);
			return result;
		}

		// Token: 0x060023ED RID: 9197 RVA: 0x000B8773 File Offset: 0x000B6973
		private static void CheckType(byte type, byte alertDescription)
		{
			if (type - 20 > 4)
			{
				throw new TlsFatalAlert(alertDescription);
			}
		}

		// Token: 0x060023EE RID: 9198 RVA: 0x000B8783 File Offset: 0x000B6983
		private static void CheckLength(int length, int limit, byte alertDescription)
		{
			if (length > limit)
			{
				throw new TlsFatalAlert(alertDescription);
			}
		}

		// Token: 0x040018DE RID: 6366
		private const int DEFAULT_PLAINTEXT_LIMIT = 16384;

		// Token: 0x040018DF RID: 6367
		internal const int TLS_HEADER_SIZE = 5;

		// Token: 0x040018E0 RID: 6368
		internal const int TLS_HEADER_TYPE_OFFSET = 0;

		// Token: 0x040018E1 RID: 6369
		internal const int TLS_HEADER_VERSION_OFFSET = 1;

		// Token: 0x040018E2 RID: 6370
		internal const int TLS_HEADER_LENGTH_OFFSET = 3;

		// Token: 0x040018E3 RID: 6371
		private TlsProtocol mHandler;

		// Token: 0x040018E4 RID: 6372
		private Stream mInput;

		// Token: 0x040018E5 RID: 6373
		private Stream mOutput;

		// Token: 0x040018E6 RID: 6374
		private TlsCompression mPendingCompression;

		// Token: 0x040018E7 RID: 6375
		private TlsCompression mReadCompression;

		// Token: 0x040018E8 RID: 6376
		private TlsCompression mWriteCompression;

		// Token: 0x040018E9 RID: 6377
		private TlsCipher mPendingCipher;

		// Token: 0x040018EA RID: 6378
		private TlsCipher mReadCipher;

		// Token: 0x040018EB RID: 6379
		private TlsCipher mWriteCipher;

		// Token: 0x040018EC RID: 6380
		private long mReadSeqNo;

		// Token: 0x040018ED RID: 6381
		private long mWriteSeqNo;

		// Token: 0x040018EE RID: 6382
		private MemoryStream mBuffer = new MemoryStream();

		// Token: 0x040018EF RID: 6383
		private TlsHandshakeHash mHandshakeHash;

		// Token: 0x040018F0 RID: 6384
		private ProtocolVersion mReadVersion;

		// Token: 0x040018F1 RID: 6385
		private ProtocolVersion mWriteVersion;

		// Token: 0x040018F2 RID: 6386
		private bool mRestrictReadVersion = true;

		// Token: 0x040018F3 RID: 6387
		private int mPlaintextLimit;

		// Token: 0x040018F4 RID: 6388
		private int mCompressedLimit;

		// Token: 0x040018F5 RID: 6389
		private int mCiphertextLimit;
	}
}
