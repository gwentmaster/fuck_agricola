using System;

namespace BestHTTP.Decompression.Zlib
{
	// Token: 0x0200060E RID: 1550
	internal sealed class ZlibCodec
	{
		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x060038B1 RID: 14513 RVA: 0x0011AA98 File Offset: 0x00118C98
		public int Adler32
		{
			get
			{
				return (int)this._Adler32;
			}
		}

		// Token: 0x060038B2 RID: 14514 RVA: 0x0011AAA0 File Offset: 0x00118CA0
		public ZlibCodec()
		{
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x0011AAB8 File Offset: 0x00118CB8
		public ZlibCodec(CompressionMode mode)
		{
			if (mode == CompressionMode.Compress)
			{
				if (this.InitializeDeflate() != 0)
				{
					throw new ZlibException("Cannot initialize for deflate.");
				}
			}
			else
			{
				if (mode != CompressionMode.Decompress)
				{
					throw new ZlibException("Invalid ZlibStreamFlavor.");
				}
				if (this.InitializeInflate() != 0)
				{
					throw new ZlibException("Cannot initialize for inflate.");
				}
			}
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x0011AB12 File Offset: 0x00118D12
		public int InitializeInflate()
		{
			return this.InitializeInflate(this.WindowBits);
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0011AB20 File Offset: 0x00118D20
		public int InitializeInflate(bool expectRfc1950Header)
		{
			return this.InitializeInflate(this.WindowBits, expectRfc1950Header);
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0011AB2F File Offset: 0x00118D2F
		public int InitializeInflate(int windowBits)
		{
			this.WindowBits = windowBits;
			return this.InitializeInflate(windowBits, true);
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0011AB40 File Offset: 0x00118D40
		public int InitializeInflate(int windowBits, bool expectRfc1950Header)
		{
			this.WindowBits = windowBits;
			if (this.dstate != null)
			{
				throw new ZlibException("You may not call InitializeInflate() after calling InitializeDeflate().");
			}
			this.istate = new InflateManager(expectRfc1950Header);
			return this.istate.Initialize(this, windowBits);
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0011AB75 File Offset: 0x00118D75
		public int Inflate(FlushType flush)
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Inflate(flush);
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0011AB96 File Offset: 0x00118D96
		public int EndInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			int result = this.istate.End();
			this.istate = null;
			return result;
		}

		// Token: 0x060038BA RID: 14522 RVA: 0x0011ABBD File Offset: 0x00118DBD
		public int SyncInflate()
		{
			if (this.istate == null)
			{
				throw new ZlibException("No Inflate State!");
			}
			return this.istate.Sync();
		}

		// Token: 0x060038BB RID: 14523 RVA: 0x0011ABDD File Offset: 0x00118DDD
		public int InitializeDeflate()
		{
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060038BC RID: 14524 RVA: 0x0011ABE6 File Offset: 0x00118DE6
		public int InitializeDeflate(CompressionLevel level)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060038BD RID: 14525 RVA: 0x0011ABF6 File Offset: 0x00118DF6
		public int InitializeDeflate(CompressionLevel level, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060038BE RID: 14526 RVA: 0x0011AC06 File Offset: 0x00118E06
		public int InitializeDeflate(CompressionLevel level, int bits)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(true);
		}

		// Token: 0x060038BF RID: 14527 RVA: 0x0011AC1D File Offset: 0x00118E1D
		public int InitializeDeflate(CompressionLevel level, int bits, bool wantRfc1950Header)
		{
			this.CompressLevel = level;
			this.WindowBits = bits;
			return this._InternalInitializeDeflate(wantRfc1950Header);
		}

		// Token: 0x060038C0 RID: 14528 RVA: 0x0011AC34 File Offset: 0x00118E34
		private int _InternalInitializeDeflate(bool wantRfc1950Header)
		{
			if (this.istate != null)
			{
				throw new ZlibException("You may not call InitializeDeflate() after calling InitializeInflate().");
			}
			this.dstate = new DeflateManager();
			this.dstate.WantRfc1950HeaderBytes = wantRfc1950Header;
			return this.dstate.Initialize(this, this.CompressLevel, this.WindowBits, this.Strategy);
		}

		// Token: 0x060038C1 RID: 14529 RVA: 0x0011AC89 File Offset: 0x00118E89
		public int Deflate(FlushType flush)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.Deflate(flush);
		}

		// Token: 0x060038C2 RID: 14530 RVA: 0x0011ACAA File Offset: 0x00118EAA
		public int EndDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate = null;
			return 0;
		}

		// Token: 0x060038C3 RID: 14531 RVA: 0x0011ACC7 File Offset: 0x00118EC7
		public void ResetDeflate()
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			this.dstate.Reset();
		}

		// Token: 0x060038C4 RID: 14532 RVA: 0x0011ACE7 File Offset: 0x00118EE7
		public int SetDeflateParams(CompressionLevel level, CompressionStrategy strategy)
		{
			if (this.dstate == null)
			{
				throw new ZlibException("No Deflate State!");
			}
			return this.dstate.SetParams(level, strategy);
		}

		// Token: 0x060038C5 RID: 14533 RVA: 0x0011AD09 File Offset: 0x00118F09
		public int SetDictionary(byte[] dictionary)
		{
			if (this.istate != null)
			{
				return this.istate.SetDictionary(dictionary);
			}
			if (this.dstate != null)
			{
				return this.dstate.SetDictionary(dictionary);
			}
			throw new ZlibException("No Inflate or Deflate state!");
		}

		// Token: 0x060038C6 RID: 14534 RVA: 0x0011AD40 File Offset: 0x00118F40
		internal void flush_pending()
		{
			int num = this.dstate.pendingCount;
			if (num > this.AvailableBytesOut)
			{
				num = this.AvailableBytesOut;
			}
			if (num == 0)
			{
				return;
			}
			if (this.dstate.pending.Length <= this.dstate.nextPending || this.OutputBuffer.Length <= this.NextOut || this.dstate.pending.Length < this.dstate.nextPending + num || this.OutputBuffer.Length < this.NextOut + num)
			{
				throw new ZlibException(string.Format("Invalid State. (pending.Length={0}, pendingCount={1})", this.dstate.pending.Length, this.dstate.pendingCount));
			}
			Array.Copy(this.dstate.pending, this.dstate.nextPending, this.OutputBuffer, this.NextOut, num);
			this.NextOut += num;
			this.dstate.nextPending += num;
			this.TotalBytesOut += (long)num;
			this.AvailableBytesOut -= num;
			this.dstate.pendingCount -= num;
			if (this.dstate.pendingCount == 0)
			{
				this.dstate.nextPending = 0;
			}
		}

		// Token: 0x060038C7 RID: 14535 RVA: 0x0011AE8C File Offset: 0x0011908C
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.AvailableBytesIn;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.AvailableBytesIn -= num;
			if (this.dstate.WantRfc1950HeaderBytes)
			{
				this._Adler32 = Adler.Adler32(this._Adler32, this.InputBuffer, this.NextIn, num);
			}
			Array.Copy(this.InputBuffer, this.NextIn, buf, start, num);
			this.NextIn += num;
			this.TotalBytesIn += (long)num;
			return num;
		}

		// Token: 0x040024D7 RID: 9431
		public byte[] InputBuffer;

		// Token: 0x040024D8 RID: 9432
		public int NextIn;

		// Token: 0x040024D9 RID: 9433
		public int AvailableBytesIn;

		// Token: 0x040024DA RID: 9434
		public long TotalBytesIn;

		// Token: 0x040024DB RID: 9435
		public byte[] OutputBuffer;

		// Token: 0x040024DC RID: 9436
		public int NextOut;

		// Token: 0x040024DD RID: 9437
		public int AvailableBytesOut;

		// Token: 0x040024DE RID: 9438
		public long TotalBytesOut;

		// Token: 0x040024DF RID: 9439
		public string Message;

		// Token: 0x040024E0 RID: 9440
		internal DeflateManager dstate;

		// Token: 0x040024E1 RID: 9441
		internal InflateManager istate;

		// Token: 0x040024E2 RID: 9442
		internal uint _Adler32;

		// Token: 0x040024E3 RID: 9443
		public CompressionLevel CompressLevel = CompressionLevel.Default;

		// Token: 0x040024E4 RID: 9444
		public int WindowBits = 15;

		// Token: 0x040024E5 RID: 9445
		public CompressionStrategy Strategy;
	}
}
