using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000390 RID: 912
	public class AbstractTlsCipherFactory : TlsCipherFactory
	{
		// Token: 0x06002274 RID: 8820 RVA: 0x000B57B9 File Offset: 0x000B39B9
		public virtual TlsCipher CreateCipher(TlsContext context, int encryptionAlgorithm, int macAlgorithm)
		{
			throw new TlsFatalAlert(80);
		}
	}
}
