using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003DA RID: 986
	public interface TlsAuthentication
	{
		// Token: 0x06002434 RID: 9268
		void NotifyServerCertificate(Certificate serverCertificate);

		// Token: 0x06002435 RID: 9269
		TlsCredentials GetClientCredentials(TlsContext context, CertificateRequest certificateRequest);
	}
}
