using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003E0 RID: 992
	internal class TlsClientContextImpl : AbstractTlsContext, TlsClientContext, TlsContext
	{
		// Token: 0x06002457 RID: 9303 RVA: 0x000B99FB File Offset: 0x000B7BFB
		internal TlsClientContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
		{
		}

		// Token: 0x17000424 RID: 1060
		// (get) Token: 0x06002458 RID: 9304 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool IsServer
		{
			get
			{
				return false;
			}
		}
	}
}
