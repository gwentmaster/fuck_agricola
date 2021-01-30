using System;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003FE RID: 1022
	internal class TlsServerContextImpl : AbstractTlsContext, TlsServerContext, TlsContext
	{
		// Token: 0x060025A9 RID: 9641 RVA: 0x000B99FB File Offset: 0x000B7BFB
		internal TlsServerContextImpl(SecureRandom secureRandom, SecurityParameters securityParameters) : base(secureRandom, securityParameters)
		{
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x060025AA RID: 9642 RVA: 0x0000900B File Offset: 0x0000720B
		public override bool IsServer
		{
			get
			{
				return true;
			}
		}
	}
}
