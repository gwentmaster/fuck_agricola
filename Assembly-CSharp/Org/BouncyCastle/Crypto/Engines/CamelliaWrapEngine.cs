using System;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000482 RID: 1154
	public class CamelliaWrapEngine : Rfc3394WrapEngine
	{
		// Token: 0x06002A05 RID: 10757 RVA: 0x000D2B78 File Offset: 0x000D0D78
		public CamelliaWrapEngine() : base(new CamelliaEngine())
		{
		}
	}
}
