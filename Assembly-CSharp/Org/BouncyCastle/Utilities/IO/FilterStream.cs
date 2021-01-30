using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029A RID: 666
	public class FilterStream : Stream
	{
		// Token: 0x06001635 RID: 5685 RVA: 0x0008050F File Offset: 0x0007E70F
		public FilterStream(Stream s)
		{
			this.s = s;
		}

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x06001636 RID: 5686 RVA: 0x0008051E File Offset: 0x0007E71E
		public override bool CanRead
		{
			get
			{
				return this.s.CanRead;
			}
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x06001637 RID: 5687 RVA: 0x0008052B File Offset: 0x0007E72B
		public override bool CanSeek
		{
			get
			{
				return this.s.CanSeek;
			}
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x06001638 RID: 5688 RVA: 0x00080538 File Offset: 0x0007E738
		public override bool CanWrite
		{
			get
			{
				return this.s.CanWrite;
			}
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x06001639 RID: 5689 RVA: 0x00080545 File Offset: 0x0007E745
		public override long Length
		{
			get
			{
				return this.s.Length;
			}
		}

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x0600163A RID: 5690 RVA: 0x00080552 File Offset: 0x0007E752
		// (set) Token: 0x0600163B RID: 5691 RVA: 0x0008055F File Offset: 0x0007E75F
		public override long Position
		{
			get
			{
				return this.s.Position;
			}
			set
			{
				this.s.Position = value;
			}
		}

		// Token: 0x0600163C RID: 5692 RVA: 0x0008056D File Offset: 0x0007E76D
		public override void Close()
		{
			Platform.Dispose(this.s);
			base.Close();
		}

		// Token: 0x0600163D RID: 5693 RVA: 0x00080580 File Offset: 0x0007E780
		public override void Flush()
		{
			this.s.Flush();
		}

		// Token: 0x0600163E RID: 5694 RVA: 0x0008058D File Offset: 0x0007E78D
		public override long Seek(long offset, SeekOrigin origin)
		{
			return this.s.Seek(offset, origin);
		}

		// Token: 0x0600163F RID: 5695 RVA: 0x0008059C File Offset: 0x0007E79C
		public override void SetLength(long value)
		{
			this.s.SetLength(value);
		}

		// Token: 0x06001640 RID: 5696 RVA: 0x000805AA File Offset: 0x0007E7AA
		public override int Read(byte[] buffer, int offset, int count)
		{
			return this.s.Read(buffer, offset, count);
		}

		// Token: 0x06001641 RID: 5697 RVA: 0x000805BA File Offset: 0x0007E7BA
		public override int ReadByte()
		{
			return this.s.ReadByte();
		}

		// Token: 0x06001642 RID: 5698 RVA: 0x000805C7 File Offset: 0x0007E7C7
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.s.Write(buffer, offset, count);
		}

		// Token: 0x06001643 RID: 5699 RVA: 0x000805D7 File Offset: 0x0007E7D7
		public override void WriteByte(byte value)
		{
			this.s.WriteByte(value);
		}

		// Token: 0x04001500 RID: 5376
		protected readonly Stream s;
	}
}
