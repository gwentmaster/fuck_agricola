using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003EF RID: 1007
	public interface TlsEncryptionCredentials : TlsCredentials
	{
		// Token: 0x060024F1 RID: 9457
		byte[] DecryptPreMasterSecret(byte[] encryptedPreMasterSecret);
	}
}
