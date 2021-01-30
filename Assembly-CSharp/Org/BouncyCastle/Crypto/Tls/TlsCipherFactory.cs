using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003DD RID: 989
	public interface TlsCipherFactory
	{
		// Token: 0x06002442 RID: 9282
		TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm);
	}
}
