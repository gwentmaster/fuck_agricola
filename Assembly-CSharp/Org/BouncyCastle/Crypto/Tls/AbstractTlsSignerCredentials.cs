using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000399 RID: 921
	public abstract class AbstractTlsSignerCredentials : AbstractTlsCredentials, TlsSignerCredentials, TlsCredentials
	{
		// Token: 0x060022E7 RID: 8935
		public abstract byte[] GenerateCertificateSignature(byte[] hash);

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x060022E8 RID: 8936 RVA: 0x000B618A File Offset: 0x000B438A
		public virtual SignatureAndHashAlgorithm SignatureAndHashAlgorithm
		{
			get
			{
				throw new InvalidOperationException("TlsSignerCredentials implementation does not support (D)TLS 1.2+");
			}
		}
	}
}
