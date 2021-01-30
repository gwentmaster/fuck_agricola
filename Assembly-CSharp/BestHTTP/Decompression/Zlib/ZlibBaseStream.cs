using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using BestHTTP.Decompression.Crc;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200060D RID: 1549
	internal class ZlibBaseStream : Stream
	{
		// Token: 0x17000771 RID: 1905
		// (get) Token: 0x06003897 RID: 14487 RVA: 0x00119DE6 File Offset: 0x00117FE6
		internal int Crc32
		{
			get
			{
				if (this.crc == null)
				{
					return 0;
				}
				return this.crc.Crc32Result;
			}
		}

		// Token: 0x06003898 RID: 14488 RVA: 0x00119DFD File Offset: 0x00117FFD
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen) : this(stream, compressionMode, level, flavor, leaveOpen, 15)
		{
		}

		// Token: 0x06003899 RID: 14489 RVA: 0x00119E10 File Offset: 0x00118010
		public ZlibBaseStream(Stream stream, CompressionMode compressionMode, CompressionLevel level, ZlibStreamFlavor flavor, bool leaveOpen, int windowBits)
		{
			this._flushMode = FlushType.None;
			this._stream = stream;
			this._leaveOpen = leaveOpen;
			this._compressionMode = compressionMode;
			this._flavor = flavor;
			this._level = level;
			this.windowBitsMax = windowBits;
			if (flavor == ZlibStreamFlavor.GZIP)
			{
				this.crc = new CRC32();
			}
		}

		// Token: 0x17000772 RID: 1906
		// (get) Token: 0x0600389A RID: 14490 RVA: 0x00119E89 File Offset: 0x00118089
		protected internal bool _wantCompress
		{
			get
			{
				return this._compressionMode == CompressionMode.Compress;
			}
		}

		// Token: 0x17000773 RID: 1907
		// (get) Token: 0x0600389B RID: 14491 RVA: 0x00119E94 File Offset: 0x00118094
		private ZlibCodec z
		{
			get
			{
				if (this._z == null)
				{
					bool flag = this._flavor == ZlibStreamFlavor.ZLIB;
					this._z = new ZlibCodec();
					if (this._compressionMode == CompressionMode.Decompress)
					{
						this._z.InitializeInflate(this.windowBitsMax, flag);
					}
					else
					{
						this._z.Strategy = this.Strategy;
						this._z.InitializeDeflate(this._level, this.windowBitsMax, flag);
					}
				}
				return this._z;
			}
		}

		// Token: 0x17000774 RID: 1908
		// (get) Token: 0x0600389C RID: 14492 RVA: 0x00119F10 File Offset: 0x00118110
		private byte[] workingBuffer
		{
			get
			{
				if (this._workingBuffer == null)
				{
					this._workingBuffer = new byte[this._bufferSize];
				}
				return this._workingBuffer;
			}
		}

		// Token: 0x0600389D RID: 14493 RVA: 0x00119F34 File Offset: 0x00118134
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, count);
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				this._streamMode = ZlibBaseStream.StreamMode.Writer;
			}
			else if (this._streamMode != ZlibBaseStream.StreamMode.Writer)
			{
				throw new ZlibException("Cannot Write after Reading.");
			}
			if (count == 0)
			{
				return;
			}
			this.z.InputBuffer = buffer;
			this._z.NextIn = offset;
			this._z.AvailableBytesIn = count;
			for (;;)
			{
				this._z.OutputBuffer = this.workingBuffer;
				this._z.NextOut = 0;
				this._z.AvailableBytesOut = this._workingBuffer.Length;
				int num = this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode);
				if (num != 0 && num != 1)
				{
					break;
				}
				this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
				bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
				if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
				{
					flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
				}
				if (flag)
				{
					return;
				}
			}
			throw new ZlibException((this._wantCompress ? "de" : "in") + "flating: " + this._z.Message);
		}

		// Token: 0x0600389E RID: 14494 RVA: 0x0011A0BC File Offset: 0x001182BC
		private void finish()
		{
			if (this._z == null)
			{
				return;
			}
			if (this._streamMode == ZlibBaseStream.StreamMode.Writer)
			{
				int num;
				for (;;)
				{
					this._z.OutputBuffer = this.workingBuffer;
					this._z.NextOut = 0;
					this._z.AvailableBytesOut = this._workingBuffer.Length;
					num = (this._wantCompress ? this._z.Deflate(FlushType.Finish) : this._z.Inflate(FlushType.Finish));
					if (num != 1 && num != 0)
					{
						break;
					}
					if (this._workingBuffer.Length - this._z.AvailableBytesOut > 0)
					{
						this._stream.Write(this._workingBuffer, 0, this._workingBuffer.Length - this._z.AvailableBytesOut);
					}
					bool flag = this._z.AvailableBytesIn == 0 && this._z.AvailableBytesOut != 0;
					if (this._flavor == ZlibStreamFlavor.GZIP && !this._wantCompress)
					{
						flag = (this._z.AvailableBytesIn == 8 && this._z.AvailableBytesOut != 0);
					}
					if (flag)
					{
						goto Block_12;
					}
				}
				string text = (this._wantCompress ? "de" : "in") + "flating";
				if (this._z.Message == null)
				{
					throw new ZlibException(string.Format("{0}: (rc = {1})", text, num));
				}
				throw new ZlibException(text + ": " + this._z.Message);
				Block_12:
				this.Flush();
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					if (this._wantCompress)
					{
						int crc32Result = this.crc.Crc32Result;
						this._stream.Write(BitConverter.GetBytes(crc32Result), 0, 4);
						int value = (int)(this.crc.TotalBytesRead & (long)((ulong)-1));
						this._stream.Write(BitConverter.GetBytes(value), 0, 4);
						return;
					}
					throw new ZlibException("Writing with decompression is not supported.");
				}
			}
			else if (this._streamMode == ZlibBaseStream.StreamMode.Reader && this._flavor == ZlibStreamFlavor.GZIP)
			{
				if (this._wantCompress)
				{
					throw new ZlibException("Reading with compression is not supported.");
				}
				if (this._z.TotalBytesOut == 0L)
				{
					return;
				}
				byte[] array = new byte[8];
				if (this._z.AvailableBytesIn < 8)
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, this._z.AvailableBytesIn);
					int num2 = 8 - this._z.AvailableBytesIn;
					int num3 = this._stream.Read(array, this._z.AvailableBytesIn, num2);
					if (num2 != num3)
					{
						throw new ZlibException(string.Format("Missing or incomplete GZIP trailer. Expected 8 bytes, got {0}.", this._z.AvailableBytesIn + num3));
					}
				}
				else
				{
					Array.Copy(this._z.InputBuffer, this._z.NextIn, array, 0, array.Length);
				}
				int num4 = BitConverter.ToInt32(array, 0);
				int crc32Result2 = this.crc.Crc32Result;
				int num5 = BitConverter.ToInt32(array, 4);
				int num6 = (int)(this._z.TotalBytesOut & (long)((ulong)-1));
				if (crc32Result2 != num4)
				{
					throw new ZlibException(string.Format("Bad CRC32 in GZIP trailer. (actual({0:X8})!=expected({1:X8}))", crc32Result2, num4));
				}
				if (num6 != num5)
				{
					throw new ZlibException(string.Format("Bad size in GZIP trailer. (actual({0})!=expected({1}))", num6, num5));
				}
			}
		}

		// Token: 0x0600389F RID: 14495 RVA: 0x0011A40C File Offset: 0x0011860C
		private void end()
		{
			if (this.z == null)
			{
				return;
			}
			if (this._wantCompress)
			{
				this._z.EndDeflate();
			}
			else
			{
				this._z.EndInflate();
			}
			this._z = null;
		}

		// Token: 0x060038A0 RID: 14496 RVA: 0x0011A440 File Offset: 0x00118640
		public override void Close()
		{
			if (this._stream == null)
			{
				return;
			}
			try
			{
				this.finish();
			}
			finally
			{
				this.end();
				if (!this._leaveOpen)
				{
					this._stream.Dispose();
				}
				this._stream = null;
			}
		}

		// Token: 0x060038A1 RID: 14497 RVA: 0x0011A490 File Offset: 0x00118690
		public override void Flush()
		{
			this._stream.Flush();
		}

		// Token: 0x060038A2 RID: 14498 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060038A3 RID: 14499 RVA: 0x0011A49D File Offset: 0x0011869D
		public override void SetLength(long value)
		{
			this._stream.SetLength(value);
			this.nomoreinput = false;
		}

		// Token: 0x060038A4 RID: 14500 RVA: 0x0011A4B4 File Offset: 0x001186B4
		private string ReadZeroTerminatedString()
		{
			List<byte> list = new List<byte>();
			bool flag = false;
			while (this._stream.Read(this._buf1, 0, 1) == 1)
			{
				if (this._buf1[0] == 0)
				{
					flag = true;
				}
				else
				{
					list.Add(this._buf1[0]);
				}
				if (flag)
				{
					byte[] array = list.ToArray();
					return GZipStream.iso8859dash1.GetString(array, 0, array.Length);
				}
			}
			throw new ZlibException("Unexpected EOF reading GZIP header.");
		}

		// Token: 0x060038A5 RID: 14501 RVA: 0x0011A520 File Offset: 0x00118720
		private int _ReadAndValidateGzipHeader()
		{
			int num = 0;
			byte[] array = new byte[10];
			int num2 = this._stream.Read(array, 0, array.Length);
			if (num2 == 0)
			{
				return 0;
			}
			if (num2 != 10)
			{
				throw new ZlibException("Not a valid GZIP stream.");
			}
			if (array[0] != 31 || array[1] != 139 || array[2] != 8)
			{
				throw new ZlibException("Bad GZIP header.");
			}
			int num3 = BitConverter.ToInt32(array, 4);
			this._GzipMtime = GZipStream._unixEpoch.AddSeconds((double)num3);
			num += num2;
			if ((array[3] & 4) == 4)
			{
				num2 = this._stream.Read(array, 0, 2);
				num += num2;
				short num4 = (short)((int)array[0] + (int)array[1] * 256);
				byte[] array2 = new byte[(int)num4];
				num2 = this._stream.Read(array2, 0, array2.Length);
				if (num2 != (int)num4)
				{
					throw new ZlibException("Unexpected end-of-file reading GZIP header.");
				}
				num += num2;
			}
			if ((array[3] & 8) == 8)
			{
				this._GzipFileName = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 16) == 16)
			{
				this._GzipComment = this.ReadZeroTerminatedString();
			}
			if ((array[3] & 2) == 2)
			{
				this.Read(this._buf1, 0, 1);
			}
			return num;
		}

		// Token: 0x060038A6 RID: 14502 RVA: 0x0011A640 File Offset: 0x00118840
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._stream.CanRead)
				{
					throw new ZlibException("The stream is not readable.");
				}
				this._streamMode = ZlibBaseStream.StreamMode.Reader;
				this.z.AvailableBytesIn = 0;
				if (this._flavor == ZlibStreamFlavor.GZIP)
				{
					this._gzipHeaderByteCount = this._ReadAndValidateGzipHeader();
					if (this._gzipHeaderByteCount == 0)
					{
						return 0;
					}
				}
			}
			if (this._streamMode != ZlibBaseStream.StreamMode.Reader)
			{
				throw new ZlibException("Cannot Read after Writing.");
			}
			if (count == 0)
			{
				return 0;
			}
			if (this.nomoreinput && this._wantCompress)
			{
				return 0;
			}
			if (buffer == null)
			{
				throw new ArgumentNullException("buffer");
			}
			if (count < 0)
			{
				throw new ArgumentOutOfRangeException("count");
			}
			if (offset < buffer.GetLowerBound(0))
			{
				throw new ArgumentOutOfRangeException("offset");
			}
			if (offset + count > buffer.GetLength(0))
			{
				throw new ArgumentOutOfRangeException("count");
			}
			this._z.OutputBuffer = buffer;
			this._z.NextOut = offset;
			this._z.AvailableBytesOut = count;
			this._z.InputBuffer = this.workingBuffer;
			int num;
			for (;;)
			{
				if (this._z.AvailableBytesIn == 0 && !this.nomoreinput)
				{
					this._z.NextIn = 0;
					this._z.AvailableBytesIn = this._stream.Read(this._workingBuffer, 0, this._workingBuffer.Length);
					if (this._z.AvailableBytesIn == 0)
					{
						this.nomoreinput = true;
					}
				}
				num = (this._wantCompress ? this._z.Deflate(this._flushMode) : this._z.Inflate(this._flushMode));
				if (this.nomoreinput && num == -5)
				{
					break;
				}
				if (num != 0 && num != 1)
				{
					goto Block_20;
				}
				if (((this.nomoreinput || num == 1) && this._z.AvailableBytesOut == count) || this._z.AvailableBytesOut <= 0 || this.nomoreinput || num != 0)
				{
					goto IL_20A;
				}
			}
			return 0;
			Block_20:
			throw new ZlibException(string.Format("{0}flating:  rc={1}  msg={2}", this._wantCompress ? "de" : "in", num, this._z.Message));
			IL_20A:
			if (this._z.AvailableBytesOut > 0)
			{
				if (num == 0)
				{
					int availableBytesIn = this._z.AvailableBytesIn;
				}
				if (this.nomoreinput && this._wantCompress)
				{
					num = this._z.Deflate(FlushType.Finish);
					if (num != 0 && num != 1)
					{
						throw new ZlibException(string.Format("Deflating:  rc={0}  msg={1}", num, this._z.Message));
					}
				}
			}
			num = count - this._z.AvailableBytesOut;
			if (this.crc != null)
			{
				this.crc.SlurpBlock(buffer, offset, num);
			}
			return num;
		}

		// Token: 0x17000775 RID: 1909
		// (get) Token: 0x060038A7 RID: 14503 RVA: 0x0011A8DE File Offset: 0x00118ADE
		public override bool CanRead
		{
			get
			{
				return this._stream.CanRead;
			}
		}

		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x060038A8 RID: 14504 RVA: 0x0011A8EB File Offset: 0x00118AEB
		public override bool CanSeek
		{
			get
			{
				return this._stream.CanSeek;
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x060038A9 RID: 14505 RVA: 0x0011A8F8 File Offset: 0x00118AF8
		public override bool CanWrite
		{
			get
			{
				return this._stream.CanWrite;
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x060038AA RID: 14506 RVA: 0x0011A905 File Offset: 0x00118B05
		public override long Length
		{
			get
			{
				return this._stream.Length;
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x060038AB RID: 14507 RVA: 0x00003A58 File Offset: 0x00001C58
		// (set) Token: 0x060038AC RID: 14508 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060038AD RID: 14509 RVA: 0x0011A914 File Offset: 0x00118B14
		public static void CompressString(string s, Stream compressor)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(s);
			try
			{
				compressor.Write(bytes, 0, bytes.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x060038AE RID: 14510 RVA: 0x0011A958 File Offset: 0x00118B58
		public static void CompressBuffer(byte[] b, Stream compressor)
		{
			try
			{
				compressor.Write(b, 0, b.Length);
			}
			finally
			{
				if (compressor != null)
				{
					((IDisposable)compressor).Dispose();
				}
			}
		}

		// Token: 0x060038AF RID: 14511 RVA: 0x0011A990 File Offset: 0x00118B90
		public static string UncompressString(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			Encoding utf = Encoding.UTF8;
			string result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				memoryStream.Seek(0L, SeekOrigin.Begin);
				result = new StreamReader(memoryStream, utf).ReadToEnd();
			}
			return result;
		}

		// Token: 0x060038B0 RID: 14512 RVA: 0x0011AA20 File Offset: 0x00118C20
		public static byte[] UncompressBuffer(byte[] compressed, Stream decompressor)
		{
			byte[] array = new byte[1024];
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				try
				{
					int count;
					while ((count = decompressor.Read(array, 0, array.Length)) != 0)
					{
						memoryStream.Write(array, 0, count);
					}
				}
				finally
				{
					if (decompressor != null)
					{
						((IDisposable)decompressor).Dispose();
					}
				}
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x040024C4 RID: 9412
		protected internal ZlibCodec _z;

		// Token: 0x040024C5 RID: 9413
		protected internal ZlibBaseStream.StreamMode _streamMode = ZlibBaseStream.StreamMode.Undefined;

		// Token: 0x040024C6 RID: 9414
		protected internal FlushType _flushMode;

		// Token: 0x040024C7 RID: 9415
		protected internal ZlibStreamFlavor _flavor;

		// Token: 0x040024C8 RID: 9416
		protected internal CompressionMode _compressionMode;

		// Token: 0x040024C9 RID: 9417
		protected internal CompressionLevel _level;

		// Token: 0x040024CA RID: 9418
		protected internal bool _leaveOpen;

		// Token: 0x040024CB RID: 9419
		protected internal byte[] _workingBuffer;

		// Token: 0x040024CC RID: 9420
		protected internal int _bufferSize = 16384;

		// Token: 0x040024CD RID: 9421
		protected internal int windowBitsMax;

		// Token: 0x040024CE RID: 9422
		protected internal byte[] _buf1 = new byte[1];

		// Token: 0x040024CF RID: 9423
		protected internal Stream _stream;

		// Token: 0x040024D0 RID: 9424
		protected internal CompressionStrategy Strategy;

		// Token: 0x040024D1 RID: 9425
		private CRC32 crc;

		// Token: 0x040024D2 RID: 9426
		protected internal string _GzipFileName;

		// Token: 0x040024D3 RID: 9427
		protected internal string _GzipComment;

		// Token: 0x040024D4 RID: 9428
		protected internal DateTime _GzipMtime;

		// Token: 0x040024D5 RID: 9429
		protected internal int _gzipHeaderByteCount;

		// Token: 0x040024D6 RID: 9430
		private bool nomoreinput;

		// Token: 0x0200090B RID: 2315
		internal enum StreamMode
		{
			// Token: 0x0400306C RID: 12396
			Writer,
			// Token: 0x0400306D RID: 12397
			Reader,
			// Token: 0x0400306E RID: 12398
			Undefined
		}
	}
}
