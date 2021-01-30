using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BB RID: 955
	public abstract class HandshakeType
	{
		// Token: 0x0400186A RID: 6250
		public const byte hello_request = 0;

		// Token: 0x0400186B RID: 6251
		public const byte client_hello = 1;

		// Token: 0x0400186C RID: 6252
		public const byte server_hello = 2;

		// Token: 0x0400186D RID: 6253
		public const byte certificate = 11;

		// Token: 0x0400186E RID: 6254
		public const byte server_key_exchange = 12;

		// Token: 0x0400186F RID: 6255
		public const byte certificate_request = 13;

		// Token: 0x04001870 RID: 6256
		public const byte server_hello_done = 14;

		// Token: 0x04001871 RID: 6257
		public const byte certificate_verify = 15;

		// Token: 0x04001872 RID: 6258
		public const byte client_key_exchange = 16;

		// Token: 0x04001873 RID: 6259
		public const byte finished = 20;

		// Token: 0x04001874 RID: 6260
		public const byte certificate_url = 21;

		// Token: 0x04001875 RID: 6261
		public const byte certificate_status = 22;

		// Token: 0x04001876 RID: 6262
		public const byte hello_verify_request = 3;

		// Token: 0x04001877 RID: 6263
		public const byte supplemental_data = 23;

		// Token: 0x04001878 RID: 6264
		public const byte session_ticket = 4;
	}
}
