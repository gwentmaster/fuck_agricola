using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029E RID: 670
	public class TeeInputStream : BaseInputStream
	{
		// Token: 0x06001653 RID: 5715 RVA: 0x000807A6 File Offset: 0x0007E9A6
		public TeeInputStream(Stream input, Stream tee)
		{
			this.input = input;
			this.tee = tee;
		}

		// Token: 0x06001654 RID: 5716 RVA: 0x000807BC File Offset: 0x0007E9BC
		public override void Close()
		{
			Platform.Dispose(this.input);
			Platform.Dispose(this.tee);
			base.Close();
		}

		// Token: 0x06001655 RID: 5717 RVA: 0x000807DC File Offset: 0x0007E9DC
		public override int Read(byte[] buf, int off, int len)
		{
			int num = this.input.Read(buf, off, len);
			if (num > 0)
			{
				this.tee.Write(buf, off, num);
			}
			return num;
		}

		// Token: 0x06001656 RID: 5718 RVA: 0x0008080C File Offset: 0x0007EA0C
		public override int ReadByte()
		{
			int num = this.input.ReadByte();
			if (num >= 0)
			{
				this.tee.WriteByte((byte)num);
			}
			return num;
		}

		// Token: 0x04001503 RID: 5379
		private readonly Stream input;

		// Token: 0x04001504 RID: 5380
		private readonly Stream tee;
	}
}
