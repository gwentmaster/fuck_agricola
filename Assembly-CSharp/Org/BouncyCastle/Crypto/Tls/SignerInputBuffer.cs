using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003D5 RID: 981
	internal class SignerInputBuffer : MemoryStream
	{
		// Token: 0x0600241E RID: 9246 RVA: 0x000B8D2C File Offset: 0x000B6F2C
		internal void UpdateSigner(ISigner s)
		{
			this.WriteTo(new SignerInputBuffer.SigStream(s));
		}

		// Token: 0x02000884 RID: 2180
		private class SigStream : BaseOutputStream
		{
			// Token: 0x06004566 RID: 17766 RVA: 0x00144AA6 File Offset: 0x00142CA6
			internal SigStream(ISigner s)
			{
				this.s = s;
			}

			// Token: 0x06004567 RID: 17767 RVA: 0x00144AB5 File Offset: 0x00142CB5
			public override void WriteByte(byte b)
			{
				this.s.Update(b);
			}

			// Token: 0x06004568 RID: 17768 RVA: 0x00144AC3 File Offset: 0x00142CC3
			public override void Write(byte[] buf, int off, int len)
			{
				this.s.BlockUpdate(buf, off, len);
			}

			// Token: 0x04002F66 RID: 12134
			private readonly ISigner s;
		}
	}
}
