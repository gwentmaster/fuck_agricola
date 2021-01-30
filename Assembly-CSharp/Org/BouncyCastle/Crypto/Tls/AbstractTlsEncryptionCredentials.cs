using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000394 RID: 916
	public abstract class AbstractTlsEncryptionCredentials : AbstractTlsCredentials, TlsEncryptionCredentials, TlsCredentials
	{
		// Token: 0x060022A3 RID: 8867
		public abstract byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
	}
}
