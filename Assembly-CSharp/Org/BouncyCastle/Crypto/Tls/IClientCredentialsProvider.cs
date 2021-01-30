using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C2 RID: 962
	public interface IClientCredentialsProvider
	{
		// Token: 0x060023AD RID: 9133
		TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
	}
}
