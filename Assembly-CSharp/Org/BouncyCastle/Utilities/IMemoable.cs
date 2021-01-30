using System;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000286 RID: 646
	public interface IMemoable
	{
		// Token: 0x06001565 RID: 5477
		IMemoable Copy();

		// Token: 0x06001566 RID: 5478
		void Reset(IMemoable other);
	}
}
