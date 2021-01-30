using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000402 RID: 1026
	public interface TlsSignerCredentials : TlsCredentials
	{
		// Token: 0x060025BE RID: 9662
		byte[] GenerateCertificateSignature(byte[] hash);

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060025BF RID: 9663
		SignatureAndHashAlgorithm SignatureAndHashAlgorithm { get; }
	}
}
