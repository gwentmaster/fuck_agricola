using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029B RID: 667
	public class PushbackStream : FilterStream
	{
		// Token: 0x06001644 RID: 5700 RVA: 0x000805E5 File Offset: 0x0007E7E5
		public PushbackStream(Stream s) : base(s)
		{
		}

		// Token: 0x06001645 RID: 5701 RVA: 0x000805F5 File Offset: 0x0007E7F5
		public override int ReadByte()
		{
			if (this.buf != -1)
			{
				int result = this.buf;
				this.buf = -1;
				return result;
			}
			return base.ReadByte();
		}

		// Token: 0x06001646 RID: 5702 RVA: 0x00080614 File Offset: 0x0007E814
		public override int Read(byte[] buffer, int offset, int count)
		{
			if (this.buf != -1 && count > 0)
			{
				buffer[offset] = (byte)this.buf;
				this.buf = -1;
				return 1;
			}
			return base.Read(buffer, offset, count);
		}

		// Token: 0x06001647 RID: 5703 RVA: 0x0008063F File Offset: 0x0007E83F
		public virtual void Unread(int b)
		{
			if (this.buf != -1)
			{
				throw new InvalidOperationException("Can only push back one byte");
			}
			this.buf = (b & 255);
		}

		// Token: 0x04001501 RID: 5377
		private int buf = -1;
	}
}
