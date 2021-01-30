using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x020002F9 RID: 761
	public interface GlvEndomorphism : ECEndomorphism
	{
		// Token: 0x06001AB0 RID: 6832
		BigInteger[] DecomposeScalar(BigInteger k);
	}
}
