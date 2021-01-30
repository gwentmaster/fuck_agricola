using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039E RID: 926
	public class ByteQueueStream : Stream
	{
		// Token: 0x060022FB RID: 8955 RVA: 0x000B660F File Offset: 0x000B480F
		public ByteQueueStream()
		{
			this.buffer = new ByteQueue();
		}

		// Token: 0x170003E4 RID: 996
		// (get) Token: 0x060022FC RID: 8956 RVA: 0x000B6622 File Offset: 0x000B4822
		public virtual int Available
		{
			get
			{
				return this.buffer.Available;
			}
		}

		// Token: 0x170003E5 RID: 997
		// (get) Token: 0x060022FD RID: 8957 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool CanRead
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170003E6 RID: 998
		// (get) Token: 0x060022FE RID: 8958 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170003E7 RID: 999
		// (get) Token: 0x060022FF RID: 8959 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002300 RID: 8960 RVA: 0x00003022 File Offset: 0x00001222
		public override void Flush()
		{
		}

		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x06002301 RID: 8961 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x06002302 RID: 8962 RVA: 0x000B6630 File Offset: 0x000B4830
		public virtual int Peek(byte[] buf)
		{
			int num = Math.Min(this.buffer.Available, buf.Length);
			this.buffer.Read(buf, 0, num, 0);
			return num;
		}

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x06002303 RID: 8963 RVA: 0x0007F71F File Offset: 0x0007D91F
		// (set) Token: 0x06002304 RID: 8964 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Position
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

		// Token: 0x06002305 RID: 8965 RVA: 0x000B6661 File Offset: 0x000B4861
		public virtual int Read(byte[] buf)
		{
			return this.Read(buf, 0, buf.Length);
		}

		// Token: 0x06002306 RID: 8966 RVA: 0x000B6670 File Offset: 0x000B4870
		public override int Read(byte[] buf, int off, int len)
		{
			int num = Math.Min(this.buffer.Available, len);
			this.buffer.RemoveData(buf, off, num, 0);
			return num;
		}

		// Token: 0x06002307 RID: 8967 RVA: 0x000B669F File Offset: 0x000B489F
		public override int ReadByte()
		{
			if (this.buffer.Available == 0)
			{
				return -1;
			}
			return (int)(this.buffer.RemoveData(1, 0)[0] & byte.MaxValue);
		}

		// Token: 0x06002308 RID: 8968 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06002309 RID: 8969 RVA: 0x0007F71F File Offset: 0x0007D91F
		public override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x0600230A RID: 8970 RVA: 0x000B66C8 File Offset: 0x000B48C8
		public virtual int Skip(int n)
		{
			int num = Math.Min(this.buffer.Available, n);
			this.buffer.RemoveData(num);
			return num;
		}

		// Token: 0x0600230B RID: 8971 RVA: 0x000B66F4 File Offset: 0x000B48F4
		public virtual void Write(byte[] buf)
		{
			this.buffer.AddData(buf, 0, buf.Length);
		}

		// Token: 0x0600230C RID: 8972 RVA: 0x000B6706 File Offset: 0x000B4906
		public override void Write(byte[] buf, int off, int len)
		{
			this.buffer.AddData(buf, off, len);
		}

		// Token: 0x0600230D RID: 8973 RVA: 0x000B6716 File Offset: 0x000B4916
		public override void WriteByte(byte b)
		{
			this.buffer.AddData(new byte[]
			{
				b
			}, 0, 1);
		}

		// Token: 0x040016DD RID: 5853
		private readonly ByteQueue buffer;
	}
}
