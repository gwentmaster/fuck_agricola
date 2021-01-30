using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003F6 RID: 1014
	public class TlsNullCompression : TlsCompression
	{
		// Token: 0x0600253B RID: 9531 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public virtual Stream Compress(Stream output)
		{
			return output;
		}

		// Token: 0x0600253C RID: 9532 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public virtual Stream Decompress(Stream output)
		{
			return output;
		}
	}
}
