using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B8 RID: 952
	public abstract class ExporterLabel
	{
		// Token: 0x04001840 RID: 6208
		public const string client_finished = "client finished";

		// Token: 0x04001841 RID: 6209
		public const string server_finished = "server finished";

		// Token: 0x04001842 RID: 6210
		public const string master_secret = "master secret";

		// Token: 0x04001843 RID: 6211
		public const string key_expansion = "key expansion";

		// Token: 0x04001844 RID: 6212
		public const string client_EAP_encryption = "client EAP encryption";

		// Token: 0x04001845 RID: 6213
		public const string ttls_keying_material = "ttls keying material";

		// Token: 0x04001846 RID: 6214
		public const string ttls_challenge = "ttls challenge";

		// Token: 0x04001847 RID: 6215
		public const string dtls_srtp = "EXTRACTOR-dtls_srtp";

		// Token: 0x04001848 RID: 6216
		public static readonly string extended_master_secret = "extended master secret";
	}
}
