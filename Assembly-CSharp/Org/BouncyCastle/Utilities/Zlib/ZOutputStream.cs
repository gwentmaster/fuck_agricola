using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.Zlib
{
	// Token: 0x02000294 RID: 660
	public class ZOutputStream : Stream
	{
		// Token: 0x060015DB RID: 5595 RVA: 0x0007F46B File Offset: 0x0007D66B
		private static ZStream GetDefaultZStream(bool nowrap)
		{
			ZStream zstream = new ZStream();
			zstream.inflateInit(nowrap);
			return zstream;
		}

		// Token: 0x060015DC RID: 5596 RVA: 0x0007F47A File Offset: 0x0007D67A
		public ZOutputStream(Stream output) : this(output, false)
		{
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0007F484 File Offset: 0x0007D684
		public ZOutputStream(Stream output, bool nowrap) : this(output, ZOutputStream.GetDefaultZStream(nowrap))
		{
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0007F494 File Offset: 0x0007D694
		public ZOutputStream(Stream output, ZStream z)
		{
			this.buf = new byte[512];
			this.buf1 = new byte[1];
			base..ctor();
			if (z == null)
			{
				z = new ZStream();
			}
			if (z.istate == null && z.dstate == null)
			{
				z.inflateInit();
			}
			this.output = output;
			this.compress = (z.istate == null);
			this.z = z;
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0007F501 File Offset: 0x0007D701
		public ZOutputStream(Stream output, int level) : this(output, level, false)
		{
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0007F50C File Offset: 0x0007D70C
		public ZOutputStream(Stream output, int level, bool nowrap)
		{
			this.buf = new byte[512];
			this.buf1 = new byte[1];
			base..ctor();
			this.output = output;
			this.compress = true;
			this.z = new ZStream();
			this.z.deflateInit(level, nowrap);
		}

		// Token: 0x170001CB RID: 459
		// (get) Token: 0x060015E1 RID: 5601 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CC RID: 460
		// (get) Token: 0x060015E2 RID: 5602 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001CD RID: 461
		// (get) Token: 0x060015E3 RID: 5603 RVA: 0x0007F562 File Offset: 0x0007D762
		public sealed override bool CanWrite
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x060015E4 RID: 5604 RVA: 0x0007F56D File Offset: 0x0007D76D
		public override void Close()
		{
			if (this.closed)
			{
				return;
			}
			this.DoClose();
			base.Close();
		}

		// Token: 0x060015E5 RID: 5605 RVA: 0x0007F584 File Offset: 0x0007D784
		private void DoClose()
		{
			try
			{
				this.Finish();
			}
			catch (IOException)
			{
			}
			finally
			{
				this.closed = true;
				this.End();
				Platform.Dispose(this.output);
				this.output = null;
			}
		}

		// Token: 0x060015E6 RID: 5606 RVA: 0x0007F5D8 File Offset: 0x0007D7D8
		public virtual void End()
		{
			if (this.z == null)
			{
				return;
			}
			if (this.compress)
			{
				this.z.deflateEnd();
			}
			else
			{
				this.z.inflateEnd();
			}
			this.z.free();
			this.z = null;
		}

		// Token: 0x060015E7 RID: 5607 RVA: 0x0007F618 File Offset: 0x0007D818
		public virtual void Finish()
		{
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = this.buf.Length;
				int num = this.compress ? this.z.deflate(4) : this.z.inflate(4);
				if (num != 1 && num != 0)
				{
					break;
				}
				int num2 = this.buf.Length - this.z.avail_out;
				if (num2 > 0)
				{
					this.output.Write(this.buf, 0, num2);
				}
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					goto Block_6;
				}
			}
			throw new IOException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
			Block_6:
			this.Flush();
		}

		// Token: 0x060015E8 RID: 5608 RVA: 0x0007F701 File Offset: 0x0007D901
		public override void Flush()
		{
			this.output.Flush();
		}

		// Token: 0x170001CE RID: 462
		// (get) Token: 0x060015E9 RID: 5609 RVA: 0x0007F70E File Offset: 0x0007D90E
		// (set) Token: 0x060015EA RID: 5610 RVA: 0x0007F716 File Offset: 0x0007D916
		public virtual int FlushMode
		{
			get
			{
				return this.flushLevel;
			}
			set
			{
				this.flushLevel = value;
			}
		}

		// Token: 0x170001CF RID: 463
		// (get) Token: 0x060015EB RID: 5611 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170001D0 RID: 464
		// (get) Token: 0x060015EC RID: 5612 RVA: 0x0007F71F File Offset: 0x0007D91F
		// (set) Token: 0x060015ED RID: 5613 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Position
		{
			get
			{
				throw new NotSupportedException();
			}
			set
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x060015EE RID: 5614 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060015EF RID: 5615 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x060015F0 RID: 5616 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060015F1 RID: 5617 RVA: 0x0007F726 File Offset: 0x0007D926
		public virtual long TotalIn
		{
			get
			{
				return this.z.total_in;
			}
		}

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060015F2 RID: 5618 RVA: 0x0007F733 File Offset: 0x0007D933
		public virtual long TotalOut
		{
			get
			{
				return this.z.total_out;
			}
		}

		// Token: 0x060015F3 RID: 5619 RVA: 0x0007F740 File Offset: 0x0007D940
		public override void Write(byte[] b, int off, int len)
		{
			if (len == 0)
			{
				return;
			}
			this.z.next_in = b;
			this.z.next_in_index = off;
			this.z.avail_in = len;
			for (;;)
			{
				this.z.next_out = this.buf;
				this.z.next_out_index = 0;
				this.z.avail_out = this.buf.Length;
				if ((this.compress ? this.z.deflate(this.flushLevel) : this.z.inflate(this.flushLevel)) != 0)
				{
					break;
				}
				this.output.Write(this.buf, 0, this.buf.Length - this.z.avail_out);
				if (this.z.avail_in <= 0 && this.z.avail_out != 0)
				{
					return;
				}
			}
			throw new IOException((this.compress ? "de" : "in") + "flating: " + this.z.msg);
		}

		// Token: 0x060015F4 RID: 5620 RVA: 0x0007F849 File Offset: 0x0007DA49
		public override void WriteByte(byte b)
		{
			this.buf1[0] = b;
			this.Write(this.buf1, 0, 1);
		}

		// Token: 0x040014BE RID: 5310
		private const int BufferSize = 512;

		// Token: 0x040014BF RID: 5311
		protected ZStream z;

		// Token: 0x040014C0 RID: 5312
		protected int flushLevel;

		// Token: 0x040014C1 RID: 5313
		protected byte[] buf;

		// Token: 0x040014C2 RID: 5314
		protected byte[] buf1;

		// Token: 0x040014C3 RID: 5315
		protected bool compress;

		// Token: 0x040014C4 RID: 5316
		protected Stream output;

		// Token: 0x040014C5 RID: 5317
		protected bool closed;
	}
}
