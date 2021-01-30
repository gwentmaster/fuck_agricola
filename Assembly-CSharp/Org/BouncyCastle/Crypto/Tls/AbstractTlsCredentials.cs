using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x02000393 RID: 915
	public abstract class AbstractTlsCredentials : TlsCredentials
	{
		// Token: 0x170003DC RID: 988
		// (get) Token: 0x060022A1 RID: 8865
		public abstract Certificate Certificate { get; }
	}
}
