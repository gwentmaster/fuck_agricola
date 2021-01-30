using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x02000298 RID: 664
	public abstract class BaseInputStream : Stream
	{
		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x0600161A RID: 5658 RVA: 0x00080451 File Offset: 0x0007E651
		public sealed override bool CanRead
		{
			get
			{
				return !this.closed;
			}
		}

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x0600161B RID: 5659 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x0600161C RID: 5660 RVA: 0x0002A062 File Offset: 0x00028262
		public sealed override bool CanWrite
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600161D RID: 5661 RVA: 0x0008045C File Offset: 0x0007E65C
		public override void Close()
		{
			this.closed = true;
			base.Close();
		}

		// Token: 0x0600161E RID: 5662 RVA: 0x00003022 File Offset: 0x00001222
		public sealed override void Flush()
		{
		}

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x0600161F RID: 5663 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Length
		{
			get
			{
				throw new NotSupportedException();
			}
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x06001620 RID: 5664 RVA: 0x0007F71F File Offset: 0x0007D91F
		// (set) Token: 0x06001621 RID: 5665 RVA: 0x0007F71F File Offset: 0x0007D91F
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

		// Token: 0x06001622 RID: 5666 RVA: 0x0008046C File Offset: 0x0007E66C
		public override int Read(byte[] buffer, int offset, int count)
		{
			int i = offset;
			try
			{
				int num = offset + count;
				while (i < num)
				{
					int num2 = this.ReadByte();
					if (num2 == -1)
					{
						break;
					}
					buffer[i++] = (byte)num2;
				}
			}
			catch (IOException)
			{
				if (i == offset)
				{
					throw;
				}
			}
			return i - offset;
		}

		// Token: 0x06001623 RID: 5667 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001624 RID: 5668 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override void SetLength(long value)
		{
			throw new NotSupportedException();
		}

		// Token: 0x06001625 RID: 5669 RVA: 0x0007F71F File Offset: 0x0007D91F
		public sealed override void Write(byte[] buffer, int offset, int count)
		{
			throw new NotSupportedException();
		}

		// Token: 0x040014FE RID: 5374
		private bool closed;
	}
}
