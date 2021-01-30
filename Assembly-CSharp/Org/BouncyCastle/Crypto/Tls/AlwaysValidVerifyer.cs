using System;
using Org.BouncyCastle.Asn1.X509;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039C RID: 924
	public class AlwaysValidVerifyer : ICertificateVerifyer
	{
		// Token: 0x060022F0 RID: 8944 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsValid(Uri targetUri, X509CertificateStructure[] certs)
		{
			return true;
		}
	}
}
