using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x02000299 RID: 665
	public abstract class BaseOutputStream : Stream
	{
		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x06001627 RID: 5671 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x06001628 RID: 5672 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x06001629 RID: 5673 RVA: 0x000804C0 File Offset: 0x0007E6C0
		public sealed override bool CanWrite
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x0600162A RID: 5674 RVA: 0x000804CB File Offset: 0x0007E6CB
		public override void Close()
		{
			this.closed = true;
			base.Close();
		}

		// Token: 0x0600162B RID: 5675 RVA: 0x00003022 File Offset: 0x00001222
		public override void Flush()
		{
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x0600162C RID: 5676 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170001DC RID: 476
		// (get) Token: 0x0600162D RID: 5677 RVA: 0x0007F71F File Offset: 0x0007D91F
		// (set) Token: 0x0600162E RID: 5678 RVA: 0x0007F71F File Offset: 0x0007D91F
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

		// Token: 0x0600162F RID: 5679 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001630 RID: 5680 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001631 RID: 5681 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001632 RID: 5682 RVA: 0x000804DC File Offset: 0x0007E6DC
		public override void Write(byte[] buffer, int offset, int count)
		{
			int num = offset + count;
			for (int i = offset; i < num; i++)
			{
				this.WriteByte(buffer[i]);
			}
		}

		// Token: 0x06001633 RID: 5683 RVA: 0x00080502 File Offset: 0x0007E702
		public virtual void Write(params byte[] buffer)
		{
			this.Write(buffer, 0, buffer.Length);
		}

		// Token: 0x040014FF RID: 5375
		private bool closed;
	}
}
