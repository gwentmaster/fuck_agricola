using System;
using System.IO;
using System.Text;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020005FC RID: 1532
	internal class GZipStream : Stream
	{
		// Token: 0x17000764 RID: 1892
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x0011598C File Offset: 0x00113B8C
		// (set) Token: 0x06003843 RID: 14403 RVA: 0x00115994 File Offset: 0x00113B94
		public string Comment
		{
			get
			{
				return this._Comment;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._Comment = value;
			}
		}

		// Token: 0x17000765 RID: 1893
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x001159B0 File Offset: 0x00113BB0
		// (set) Token: 0x06003845 RID: 14405 RVA: 0x001159B8 File Offset: 0x00113BB8
		public string FileName
		{
			get
			{
				return this._FileName;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._FileName = value;
				if (this._FileName == null)
				{
					return;
				}
				if (this._FileName.IndexOf("/") != -1)
				{
					this._FileName = this._FileName.Replace("/", "\\");
				}
				if (this._FileName.EndsWith("\\"))
				{
					throw new Exception("Illegal filename");
				}
				if (this._FileName.IndexOf("\\") != -1)
				{
					this._FileName = Path.GetFileName(this._FileName);
				}
			}
		}

		// Token: 0x17000766 RID: 1894
		// (get) Token: 0x06003846 RID: 14406 RVA: 0x00115A57 File Offset: 0x00113C57
		public int Crc32
		{
			get
			{
				return this._Crc32;
			}
		}

		// Token: 0x06003847 RID: 14407 RVA: 0x00115A5F File Offset: 0x00113C5F
		public GZipStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06003848 RID: 14408 RVA: 0x00115A6B File Offset: 0x00113C6B
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06003849 RID: 14409 RVA: 0x00115A77 File Offset: 0x00113C77
		public GZipStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x0600384A RID: 14410 RVA: 0x00115A83 File Offset: 0x00113C83
		public GZipStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.GZIP, leaveOpen);
		}

		// Token: 0x17000767 RID: 1895
		// (get) Token: 0x0600384B RID: 14411 RVA: 0x00115AA0 File Offset: 0x00113CA0
		// (set) Token: 0x0600384C RID: 14412 RVA: 0x00115AAD File Offset: 0x00113CAD
		public virtual FlushType FlushMode
		{
			get
			{
				return this._baseStream._flushMode;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x17000768 RID: 1896
		// (get) Token: 0x0600384D RID: 14413 RVA: 0x00115ACE File Offset: 0x00113CCE
		// (set) Token: 0x0600384E RID: 14414 RVA: 0x00115ADC File Offset: 0x00113CDC
		public int BufferSize
		{
			get
			{
				return this._baseStream._bufferSize;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				if (this._baseStream._workingBuffer != null)
				{
					throw new ZlibException("The working buffer is already set.");
				}
				if (value < 1024)
				{
					throw new ZlibException(string.Format("Don't be silly. {0} bytes?? Use a bigger buffer, at least {1}.", value, 1024));
				}
				this._baseStream._bufferSize = value;
			}
		}

		// Token: 0x17000769 RID: 1897
		// (get) Token: 0x0600384F RID: 14415 RVA: 0x00115B48 File Offset: 0x00113D48
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700076A RID: 1898
		// (get) Token: 0x06003850 RID: 14416 RVA: 0x00115B5A File Offset: 0x00113D5A
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06003851 RID: 14417 RVA: 0x00115B6C File Offset: 0x00113D6C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
						this._Crc32 = this._baseStream.Crc32;
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700076B RID: 1899
		// (get) Token: 0x06003852 RID: 14418 RVA: 0x00115BCC File Offset: 0x00113DCC
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x1700076C RID: 1900
		// (get) Token: 0x06003853 RID: 14419 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700076D RID: 1901
		// (get) Token: 0x06003854 RID: 14420 RVA: 0x00115BF1 File Offset: 0x00113DF1
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("GZipStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06003855 RID: 14421 RVA: 0x00115C16 File Offset: 0x00113E16
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x1700076E RID: 1902
		// (get) Token: 0x06003856 RID: 14422 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x1700076F RID: 1903
		// (get) Token: 0x06003857 RID: 14423 RVA: 0x00115C38 File Offset: 0x00113E38
		// (set) Token: 0x06003858 RID: 14424 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut + (long)this._headerByteCount;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn + (long)this._baseStream._gzipHeaderByteCount;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x06003859 RID: 14425 RVA: 0x00115C9C File Offset: 0x00113E9C
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			int result = this._baseStream.Read(buffer, offset, count);
			if (!this._firstReadDone)
			{
				this._firstReadDone = true;
				this.FileName = this._baseStream._GzipFileName;
				this.Comment = this._baseStream._GzipComment;
			}
			return result;
		}

		// Token: 0x0600385A RID: 14426 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600385B RID: 14427 RVA: 0x00115CFB File Offset: 0x00113EFB
		public override void SetLength(long value)
		{
			this._baseStream.SetLength(value);
		}

		// Token: 0x0600385C RID: 14428 RVA: 0x00115D0C File Offset: 0x00113F0C
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("GZipStream");
			}
			if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Undefined)
			{
				if (!this._baseStream._wantCompress)
				{
					throw new InvalidOperationException();
				}
				this._headerByteCount = this.EmitHeader();
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600385D RID: 14429 RVA: 0x00115D6C File Offset: 0x00113F6C
		private int EmitHeader()
		{
			byte[] array = (this.Comment == null) ? null : GZipStream.iso8859dash1.GetBytes(this.Comment);
			byte[] array2 = (this.FileName == null) ? null : GZipStream.iso8859dash1.GetBytes(this.FileName);
			int num = (this.Comment == null) ? 0 : (array.Length + 1);
			int num2 = (this.FileName == null) ? 0 : (array2.Length + 1);
			byte[] array3 = new byte[10 + num + num2];
			int num3 = 0;
			array3[num3++] = 31;
			array3[num3++] = 139;
			array3[num3++] = 8;
			byte b = 0;
			if (this.Comment != null)
			{
				b ^= 16;
			}
			if (this.FileName != null)
			{
				b ^= 8;
			}
			array3[num3++] = b;
			if (this.LastModified == null)
			{
				this.LastModified = new DateTime?(DateTime.Now);
			}
			Array.Copy(BitConverter.GetBytes((int)(this.LastModified.Value - GZipStream._unixEpoch).TotalSeconds), 0, array3, num3, 4);
			num3 += 4;
			array3[num3++] = 0;
			array3[num3++] = byte.MaxValue;
			if (num2 != 0)
			{
				Array.Copy(array2, 0, array3, num3, num2 - 1);
				num3 += num2 - 1;
				array3[num3++] = 0;
			}
			if (num != 0)
			{
				Array.Copy(array, 0, array3, num3, num - 1);
				num3 += num - 1;
				array3[num3++] = 0;
			}
			this._baseStream._stream.Write(array3, 0, array3.Length);
			return array3.Length;
		}

		// Token: 0x0600385E RID: 14430 RVA: 0x00115F08 File Offset: 0x00114108
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600385F RID: 14431 RVA: 0x00115F50 File Offset: 0x00114150
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new GZipStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003860 RID: 14432 RVA: 0x00115F98 File Offset: 0x00114198
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06003861 RID: 14433 RVA: 0x00115FDC File Offset: 0x001141DC
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new GZipStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04002423 RID: 9251
		public DateTime? LastModified;

		// Token: 0x04002424 RID: 9252
		private int _headerByteCount;

		// Token: 0x04002425 RID: 9253
		internal ZlibBaseStream _baseStream;

		// Token: 0x04002426 RID: 9254
		private bool _disposed;

		// Token: 0x04002427 RID: 9255
		private bool _firstReadDone;

		// Token: 0x04002428 RID: 9256
		private string _FileName;

		// Token: 0x04002429 RID: 9257
		private string _Comment;

		// Token: 0x0400242A RID: 9258
		private int _Crc32;

		// Token: 0x0400242B RID: 9259
		internal static readonly DateTime _unixEpoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

		// Token: 0x0400242C RID: 9260
		internal static readonly Encoding iso8859dash1 = Encoding.GetEncoding("iso-8859-1");
	}
}
