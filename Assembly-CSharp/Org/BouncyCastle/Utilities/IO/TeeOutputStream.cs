using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029F RID: 671
	public class TeeOutputStream : BaseOutputStream
	{
		// Token: 0x06001657 RID: 5719 RVA: 0x00080837 File Offset: 0x0007EA37
		public TeeOutputStream(Stream output, Stream tee)
		{
			this.output = output;
			this.tee = tee;
		}

		// Token: 0x06001658 RID: 5720 RVA: 0x0008084D File Offset: 0x0007EA4D
		public override void Close()
		{
			Platform.Dispose(this.output);
			Platform.Dispose(this.tee);
			base.Close();
		}

		// Token: 0x06001659 RID: 5721 RVA: 0x0008086B File Offset: 0x0007EA6B
		public override void Write(byte[] buffer, int offset, int count)
		{
			this.output.Write(buffer, offset, count);
			this.tee.Write(buffer, offset, count);
		}

		// Token: 0x0600165A RID: 5722 RVA: 0x00080889 File Offset: 0x0007EA89
		public override void WriteByte(byte b)
		{
			this.output.WriteByte(b);
			this.tee.WriteByte(b);
		}

		// Token: 0x04001505 RID: 5381
		private readonly Stream output;

		// Token: 0x04001506 RID: 5382
		private readonly Stream tee;
	}
}
