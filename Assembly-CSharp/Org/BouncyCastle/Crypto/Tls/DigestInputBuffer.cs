using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B2 RID: 946
	internal class DigestInputBuffer : MemoryStream
	{
		// Token: 0x0600238C RID: 9100 RVA: 0x000B7B25 File Offset: 0x000B5D25
		internal void UpdateDigest(IDigest d)
		{
			this.WriteTo(new DigestInputBuffer.DigStream(d));
		}

		// Token: 0x02000882 RID: 2178
		private class DigStream : BaseOutputStream
		{
			// Token: 0x06004559 RID: 17753 RVA: 0x00144956 File Offset: 0x00142B56
			internal DigStream(IDigest d)
			{
				this.d = d;
			}

			// Token: 0x0600455A RID: 17754 RVA: 0x00144965 File Offset: 0x00142B65
			public override void WriteByte(byte b)
			{
				this.d.Update(b);
			}

			// Token: 0x0600455B RID: 17755 RVA: 0x00144973 File Offset: 0x00142B73
			public override void Write(byte[] buf, int off, int len)
			{
				this.d.BlockUpdate(buf, off, len);
			}

			// Token: 0x04002F5E RID: 12126
			private readonly IDigest d;
		}
	}
}
