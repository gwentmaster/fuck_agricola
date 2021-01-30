using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003AD RID: 941
	public abstract class ContentType
	{
		// Token: 0x04001813 RID: 6163
		public const byte change_cipher_spec = 20;

		// Token: 0x04001814 RID: 6164
		public const byte alert = 21;

		// Token: 0x04001815 RID: 6165
		public const byte handshake = 22;

		// Token: 0x04001816 RID: 6166
		public const byte application_data = 23;

		// Token: 0x04001817 RID: 6167
		public const byte heartbeat = 24;
	}
}
