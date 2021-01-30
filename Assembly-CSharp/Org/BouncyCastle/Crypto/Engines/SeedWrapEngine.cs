using System;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200049D RID: 1181
	public class SeedWrapEngine : Rfc3394WrapEngine
	{
		// Token: 0x06002B33 RID: 11059 RVA: 0x000DB660 File Offset: 0x000D9860
		public SeedWrapEngine() : base(new SeedEngine())
		{
		}
	}
}
