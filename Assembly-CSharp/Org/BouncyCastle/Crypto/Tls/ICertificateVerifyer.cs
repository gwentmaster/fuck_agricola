using System;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C0 RID: 960
	public interface ICertificateVerifyer
	{
		// Token: 0x060023AB RID: 9131
		bool IsValid(Uri targetUri, X509CertificateStructure[] certs);
	}
}
