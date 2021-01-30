using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003A9 RID: 937
	public abstract class ClientCertificateType
	{
		// Token: 0x04001802 RID: 6146
		public const byte rsa_sign = 1;

		// Token: 0x04001803 RID: 6147
		public const byte dss_sign = 2;

		// Token: 0x04001804 RID: 6148
		public const byte rsa_fixed_dh = 3;

		// Token: 0x04001805 RID: 6149
		public const byte dss_fixed_dh = 4;

		// Token: 0x04001806 RID: 6150
		public const byte rsa_ephemeral_dh_RESERVED = 5;

		// Token: 0x04001807 RID: 6151
		public const byte dss_ephemeral_dh_RESERVED = 6;

		// Token: 0x04001808 RID: 6152
		public const byte fortezza_dms_RESERVED = 20;

		// Token: 0x04001809 RID: 6153
		public const byte ecdsa_sign = 64;

		// Token: 0x0400180A RID: 6154
		public const byte rsa_fixed_ecdh = 65;

		// Token: 0x0400180B RID: 6155
		public const byte ecdsa_fixed_ecdh = 66;
	}
}
