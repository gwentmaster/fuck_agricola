using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B6 RID: 950
	public abstract class ECPointFormat
	{
		// Token: 0x04001824 RID: 6180
		public const byte uncompressed = 0;

		// Token: 0x04001825 RID: 6181
		public const byte ansiX962_compressed_prime = 1;

		// Token: 0x04001826 RID: 6182
		public const byte ansiX962_compressed_char2 = 2;
	}
}
