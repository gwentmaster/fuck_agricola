using System;

namespace Org.BouncyCastle.Math.EC.Abc
{
	// Token: 0x02000366 RID: 870
	internal class ZTauElement
	{
		// Token: 0x06002166 RID: 8550 RVA: 0x000B4440 File Offset: 0x000B2640
		public ZTauElement(BigInteger u, BigInteger v)
		{
			this.u = u;
			this.v = v;
		}

		// Token: 0x04001676 RID: 5750
		public readonly BigInteger u;

		// Token: 0x04001677 RID: 5751
		public readonly BigInteger v;
	}
}
