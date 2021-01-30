using System;
using System.IO;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x020005FB RID: 1531
	internal class DeflateStream : Stream
	{
		// Token: 0x06003825 RID: 14373 RVA: 0x001155B1 File Offset: 0x001137B1
		public DeflateStream(Stream stream, CompressionMode mode) : this(stream, mode, CompressionLevel.Default, false)
		{
		}

		// Token: 0x06003826 RID: 14374 RVA: 0x001155BD File Offset: 0x001137BD
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level) : this(stream, mode, level, false)
		{
		}

		// Token: 0x06003827 RID: 14375 RVA: 0x001155C9 File Offset: 0x001137C9
		public DeflateStream(Stream stream, CompressionMode mode, bool leaveOpen) : this(stream, mode, CompressionLevel.Default, leaveOpen)
		{
		}

		// Token: 0x06003828 RID: 14376 RVA: 0x001155D5 File Offset: 0x001137D5
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen);
		}

		// Token: 0x06003829 RID: 14377 RVA: 0x001155F9 File Offset: 0x001137F9
		public DeflateStream(Stream stream, CompressionMode mode, CompressionLevel level, bool leaveOpen, int windowBits)
		{
			this._innerStream = stream;
			this._baseStream = new ZlibBaseStream(stream, mode, level, ZlibStreamFlavor.DEFLATE, leaveOpen, windowBits);
		}

		// Token: 0x1700075A RID: 1882
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x0011561F File Offset: 0x0011381F
		// (set) Token: 0x0600382B RID: 14379 RVA: 0x0011562C File Offset: 0x0011382C
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
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream._flushMode = value;
			}
		}

		// Token: 0x1700075B RID: 1883
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x0011564D File Offset: 0x0011384D
		// (set) Token: 0x0600382D RID: 14381 RVA: 0x0011565C File Offset: 0x0011385C
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
					throw new ObjectDisposedException("DeflateStream");
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

		// Token: 0x1700075C RID: 1884
		// (get) Token: 0x0600382E RID: 14382 RVA: 0x001156C8 File Offset: 0x001138C8
		// (set) Token: 0x0600382F RID: 14383 RVA: 0x001156D5 File Offset: 0x001138D5
		public CompressionStrategy Strategy
		{
			get
			{
				return this._baseStream.Strategy;
			}
			set
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				this._baseStream.Strategy = value;
			}
		}

		// Token: 0x1700075D RID: 1885
		// (get) Token: 0x06003830 RID: 14384 RVA: 0x001156F6 File Offset: 0x001138F6
		public virtual long TotalIn
		{
			get
			{
				return this._baseStream._z.TotalBytesIn;
			}
		}

		// Token: 0x1700075E RID: 1886
		// (get) Token: 0x06003831 RID: 14385 RVA: 0x00115708 File Offset: 0x00113908
		public virtual long TotalOut
		{
			get
			{
				return this._baseStream._z.TotalBytesOut;
			}
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0011571C File Offset: 0x0011391C
		protected override void Dispose(bool disposing)
		{
			try
			{
				if (!this._disposed)
				{
					if (disposing && this._baseStream != null)
					{
						this._baseStream.Close();
					}
					this._disposed = true;
				}
			}
			finally
			{
				base.Dispose(disposing);
			}
		}

		// Token: 0x1700075F RID: 1887
		// (get) Token: 0x06003833 RID: 14387 RVA: 0x00115768 File Offset: 0x00113968
		public override bool CanRead
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanRead;
			}
		}

		// Token: 0x17000760 RID: 1888
		// (get) Token: 0x06003834 RID: 14388 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000761 RID: 1889
		// (get) Token: 0x06003835 RID: 14389 RVA: 0x0011578D File Offset: 0x0011398D
		public override bool CanWrite
		{
			get
			{
				if (this._disposed)
				{
					throw new ObjectDisposedException("DeflateStream");
				}
				return this._baseStream._stream.CanWrite;
			}
		}

		// Token: 0x06003836 RID: 14390 RVA: 0x001157B2 File Offset: 0x001139B2
		public override void Flush()
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Flush();
		}

		// Token: 0x17000762 RID: 1890
		// (get) Token: 0x06003837 RID: 14391 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000763 RID: 1891
		// (get) Token: 0x06003838 RID: 14392 RVA: 0x001157D4 File Offset: 0x001139D4
		// (set) Token: 0x06003839 RID: 14393 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Position
		{
			get
			{
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Writer)
				{
					return this._baseStream._z.TotalBytesOut;
				}
				if (this._baseStream._streamMode == ZlibBaseStream.StreamMode.Reader)
				{
					return this._baseStream._z.TotalBytesIn;
				}
				return 0L;
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x0600383A RID: 14394 RVA: 0x00115820 File Offset: 0x00113A20
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			return this._baseStream.Read(buffer, offset, count);
		}

		// Token: 0x0600383B RID: 14395 RVA: 0x00003A58 File Offset: 0x00001C58
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x0600383C RID: 14396 RVA: 0x00115843 File Offset: 0x00113A43
		public override void SetLength(long value)
		{
			this._baseStream.SetLength(value);
		}

		// Token: 0x0600383D RID: 14397 RVA: 0x00115851 File Offset: 0x00113A51
		public override void Write(byte[] buffer, int offset, int count)
		{
			if (this._disposed)
			{
				throw new ObjectDisposedException("DeflateStream");
			}
			this._baseStream.Write(buffer, offset, count);
		}

		// Token: 0x0600383E RID: 14398 RVA: 0x00115874 File Offset: 0x00113A74
		public static byte[] CompressString(string s)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressString(s, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x0600383F RID: 14399 RVA: 0x001158BC File Offset: 0x00113ABC
		public static byte[] CompressBuffer(byte[] b)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream())
			{
				Stream compressor = new DeflateStream(memoryStream, CompressionMode.Compress, CompressionLevel.BestCompression);
				ZlibBaseStream.CompressBuffer(b, compressor);
				result = memoryStream.ToArray();
			}
			return result;
		}

		// Token: 0x06003840 RID: 14400 RVA: 0x00115904 File Offset: 0x00113B04
		public static string UncompressString(byte[] compressed)
		{
			string result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressString(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x06003841 RID: 14401 RVA: 0x00115948 File Offset: 0x00113B48
		public static byte[] UncompressBuffer(byte[] compressed)
		{
			byte[] result;
			using (MemoryStream memoryStream = new MemoryStream(compressed))
			{
				Stream decompressor = new DeflateStream(memoryStream, CompressionMode.Decompress);
				result = ZlibBaseStream.UncompressBuffer(compressed, decompressor);
			}
			return result;
		}

		// Token: 0x04002420 RID: 9248
		internal ZlibBaseStream _baseStream;

		// Token: 0x04002421 RID: 9249
		internal Stream _innerStream;

		// Token: 0x04002422 RID: 9250
		private bool _disposed;
	}
}
