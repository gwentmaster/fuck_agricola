using System;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200047F RID: 1151
	public class AesWrapEngine : Rfc3394WrapEngine
	{
		// Token: 0x060029E2 RID: 10722 RVA: 0x000D1500 File Offset: 0x000CF700
		public AesWrapEngine() : base(new AesEngine())
		{
		}
	}
}
