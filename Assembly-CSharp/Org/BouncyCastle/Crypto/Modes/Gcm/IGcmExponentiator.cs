using System;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000466 RID: 1126
	public interface IGcmExponentiator
	{
		// Token: 0x0600291B RID: 10523
		void Init(byte[] x);

		// Token: 0x0600291C RID: 10524
		void ExponentiateX(long pow, byte[] output);
	}
}
