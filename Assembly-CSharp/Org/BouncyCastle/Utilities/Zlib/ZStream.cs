using System;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000295 RID: 661
	public sealed class ZStream
	{
		// Token: 0x060015F5 RID: 5621 RVA: 0x0007F862 File Offset: 0x0007DA62
		public int inflateInit()
		{
			return this.inflateInit(15);
		}

		// Token: 0x060015F6 RID: 5622 RVA: 0x0007F86C File Offset: 0x0007DA6C
		public int inflateInit(bool nowrap)
		{
			return this.inflateInit(15, nowrap);
		}

		// Token: 0x060015F7 RID: 5623 RVA: 0x0007F877 File Offset: 0x0007DA77
		public int inflateInit(int w)
		{
			return this.inflateInit(w, false);
		}

		// Token: 0x060015F8 RID: 5624 RVA: 0x0007F881 File Offset: 0x0007DA81
		public int inflateInit(int w, bool nowrap)
		{
			this.istate = new Inflate();
			return this.istate.inflateInit(this, nowrap ? (-w) : w);
		}

		// Token: 0x060015F9 RID: 5625 RVA: 0x0007F8A2 File Offset: 0x0007DAA2
		public int inflate(int f)
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflate(this, f);
		}

		// Token: 0x060015FA RID: 5626 RVA: 0x0007F8BC File Offset: 0x0007DABC
		public int inflateEnd()
		{
			if (this.istate == null)
			{
				return -2;
			}
			int result = this.istate.inflateEnd(this);
			this.istate = null;
			return result;
		}

		// Token: 0x060015FB RID: 5627 RVA: 0x0007F8DC File Offset: 0x0007DADC
		public int inflateSync()
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflateSync(this);
		}

		// Token: 0x060015FC RID: 5628 RVA: 0x0007F8F5 File Offset: 0x0007DAF5
		public int inflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this.istate == null)
			{
				return -2;
			}
			return this.istate.inflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x060015FD RID: 5629 RVA: 0x0007F910 File Offset: 0x0007DB10
		public int deflateInit(int level)
		{
			return this.deflateInit(level, 15);
		}

		// Token: 0x060015FE RID: 5630 RVA: 0x0007F91B File Offset: 0x0007DB1B
		public int deflateInit(int level, bool nowrap)
		{
			return this.deflateInit(level, 15, nowrap);
		}

		// Token: 0x060015FF RID: 5631 RVA: 0x0007F927 File Offset: 0x0007DB27
		public int deflateInit(int level, int bits)
		{
			return this.deflateInit(level, bits, false);
		}

		// Token: 0x06001600 RID: 5632 RVA: 0x0007F932 File Offset: 0x0007DB32
		public int deflateInit(int level, int bits, bool nowrap)
		{
			this.dstate = new Deflate();
			return this.dstate.deflateInit(this, level, nowrap ? (-bits) : bits);
		}

		// Token: 0x06001601 RID: 5633 RVA: 0x0007F954 File Offset: 0x0007DB54
		public int deflate(int flush)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflate(this, flush);
		}

		// Token: 0x06001602 RID: 5634 RVA: 0x0007F96E File Offset: 0x0007DB6E
		public int deflateEnd()
		{
			if (this.dstate == null)
			{
				return -2;
			}
			int result = this.dstate.deflateEnd();
			this.dstate = null;
			return result;
		}

		// Token: 0x06001603 RID: 5635 RVA: 0x0007F98D File Offset: 0x0007DB8D
		public int deflateParams(int level, int strategy)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflateParams(this, level, strategy);
		}

		// Token: 0x06001604 RID: 5636 RVA: 0x0007F9A8 File Offset: 0x0007DBA8
		public int deflateSetDictionary(byte[] dictionary, int dictLength)
		{
			if (this.dstate == null)
			{
				return -2;
			}
			return this.dstate.deflateSetDictionary(this, dictionary, dictLength);
		}

		// Token: 0x06001605 RID: 5637 RVA: 0x0007F9C4 File Offset: 0x0007DBC4
		internal void flush_pending()
		{
			int pending = this.dstate.pending;
			if (pending > this.avail_out)
			{
				pending = this.avail_out;
			}
			if (pending == 0)
			{
				return;
			}
			if (this.dstate.pending_buf.Length > this.dstate.pending_out && this.next_out.Length > this.next_out_index && this.dstate.pending_buf.Length >= this.dstate.pending_out + pending)
			{
				int num = this.next_out.Length;
				int num2 = this.next_out_index + pending;
			}
			Array.Copy(this.dstate.pending_buf, this.dstate.pending_out, this.next_out, this.next_out_index, pending);
			this.next_out_index += pending;
			this.dstate.pending_out += pending;
			this.total_out += (long)pending;
			this.avail_out -= pending;
			this.dstate.pending -= pending;
			if (this.dstate.pending == 0)
			{
				this.dstate.pending_out = 0;
			}
		}

		// Token: 0x06001606 RID: 5638 RVA: 0x0007FADC File Offset: 0x0007DCDC
		internal int read_buf(byte[] buf, int start, int size)
		{
			int num = this.avail_in;
			if (num > size)
			{
				num = size;
			}
			if (num == 0)
			{
				return 0;
			}
			this.avail_in -= num;
			if (this.dstate.noheader == 0)
			{
				this.adler = this._adler.adler32(this.adler, this.next_in, this.next_in_index, num);
			}
			Array.Copy(this.next_in, this.next_in_index, buf, start, num);
			this.next_in_index += num;
			this.total_in += (long)num;
			return num;
		}

		// Token: 0x06001607 RID: 5639 RVA: 0x0007FB6C File Offset: 0x0007DD6C
		public void free()
		{
			this.next_in = null;
			this.next_out = null;
			this.msg = null;
			this._adler = null;
		}

		// Token: 0x040014C6 RID: 5318
		private const int MAX_WBITS = 15;

		// Token: 0x040014C7 RID: 5319
		private const int DEF_WBITS = 15;

		// Token: 0x040014C8 RID: 5320
		private const int Z_NO_FLUSH = 0;

		// Token: 0x040014C9 RID: 5321
		private const int Z_PARTIAL_FLUSH = 1;

		// Token: 0x040014CA RID: 5322
		private const int Z_SYNC_FLUSH = 2;

		// Token: 0x040014CB RID: 5323
		private const int Z_FULL_FLUSH = 3;

		// Token: 0x040014CC RID: 5324
		private const int Z_FINISH = 4;

		// Token: 0x040014CD RID: 5325
		private const int MAX_MEM_LEVEL = 9;

		// Token: 0x040014CE RID: 5326
		private const int Z_OK = 0;

		// Token: 0x040014CF RID: 5327
		private const int Z_STREAM_END = 1;

		// Token: 0x040014D0 RID: 5328
		private const int Z_NEED_DICT = 2;

		// Token: 0x040014D1 RID: 5329
		private const int Z_ERRNO = -1;

		// Token: 0x040014D2 RID: 5330
		private const int Z_STREAM_ERROR = -2;

		// Token: 0x040014D3 RID: 5331
		private const int Z_DATA_ERROR = -3;

		// Token: 0x040014D4 RID: 5332
		private const int Z_MEM_ERROR = -4;

		// Token: 0x040014D5 RID: 5333
		private const int Z_BUF_ERROR = -5;

		// Token: 0x040014D6 RID: 5334
		private const int Z_VERSION_ERROR = -6;

		// Token: 0x040014D7 RID: 5335
		public byte[] next_in;

		// Token: 0x040014D8 RID: 5336
		public int next_in_index;

		// Token: 0x040014D9 RID: 5337
		public int avail_in;

		// Token: 0x040014DA RID: 5338
		public long total_in;

		// Token: 0x040014DB RID: 5339
		public byte[] next_out;

		// Token: 0x040014DC RID: 5340
		public int next_out_index;

		// Token: 0x040014DD RID: 5341
		public int avail_out;

		// Token: 0x040014DE RID: 5342
		public long total_out;

		// Token: 0x040014DF RID: 5343
		public string msg;

		// Token: 0x040014E0 RID: 5344
		internal Deflate dstate;

		// Token: 0x040014E1 RID: 5345
		internal Inflate istate;

		// Token: 0x040014E2 RID: 5346
		internal int data_type;

		// Token: 0x040014E3 RID: 5347
		public long adler;

		// Token: 0x040014E4 RID: 5348
		internal Adler32 _adler = new Adler32();
	}
}
