using System;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E2 RID: 994
	public interface TlsCompression
	{
		// Token: 0x06002468 RID: 9320
		Stream Compress(Stream output);

		// Token: 0x06002469 RID: 9321
		Stream Decompress(Stream output);
	}
}
