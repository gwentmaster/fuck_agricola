using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003B9 RID: 953
	public abstract class ExtensionType
	{
		// Token: 0x04001849 RID: 6217
		public const int server_name = 0;

		// Token: 0x0400184A RID: 6218
		public const int max_fragment_length = 1;

		// Token: 0x0400184B RID: 6219
		public const int client_certificate_url = 2;

		// Token: 0x0400184C RID: 6220
		public const int trusted_ca_keys = 3;

		// Token: 0x0400184D RID: 6221
		public const int truncated_hmac = 4;

		// Token: 0x0400184E RID: 6222
		public const int status_request = 5;

		// Token: 0x0400184F RID: 6223
		public const int user_mapping = 6;

		// Token: 0x04001850 RID: 6224
		public const int client_authz = 7;

		// Token: 0x04001851 RID: 6225
		public const int server_authz = 8;

		// Token: 0x04001852 RID: 6226
		public const int cert_type = 9;

		// Token: 0x04001853 RID: 6227
		public const int supported_groups = 10;

		// Token: 0x04001854 RID: 6228
		public const int elliptic_curves = 10;

		// Token: 0x04001855 RID: 6229
		public const int ec_point_formats = 11;

		// Token: 0x04001856 RID: 6230
		public const int srp = 12;

		// Token: 0x04001857 RID: 6231
		public const int signature_algorithms = 13;

		// Token: 0x04001858 RID: 6232
		public const int use_srtp = 14;

		// Token: 0x04001859 RID: 6233
		public const int heartbeat = 15;

		// Token: 0x0400185A RID: 6234
		public const int application_layer_protocol_negotiation = 16;

		// Token: 0x0400185B RID: 6235
		public const int status_request_v2 = 17;

		// Token: 0x0400185C RID: 6236
		public const int signed_certificate_timestamp = 18;

		// Token: 0x0400185D RID: 6237
		public const int client_certificate_type = 19;

		// Token: 0x0400185E RID: 6238
		public const int server_certificate_type = 20;

		// Token: 0x0400185F RID: 6239
		public const int padding = 21;

		// Token: 0x04001860 RID: 6240
		public const int encrypt_then_mac = 22;

		// Token: 0x04001861 RID: 6241
		public const int extended_master_secret = 23;

		// Token: 0x04001862 RID: 6242
		public const int session_ticket = 35;

		// Token: 0x04001863 RID: 6243
		public static readonly int negotiated_ff_dhe_groups = 101;

		// Token: 0x04001864 RID: 6244
		public const int renegotiation_info = 65281;
	}
}
