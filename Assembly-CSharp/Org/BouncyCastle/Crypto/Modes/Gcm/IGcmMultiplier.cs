using System;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000467 RID: 1127
	public interface IGcmMultiplier
	{
		// Token: 0x0600291D RID: 10525
		void Init(byte[] H);

		// Token: 0x0600291E RID: 10526
		void MultiplyH(byte[] x);
	}
}
