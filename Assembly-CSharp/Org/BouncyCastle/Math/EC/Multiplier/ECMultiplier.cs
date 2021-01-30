using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002ED RID: 749
	public interface ECMultiplier
	{
		// Token: 0x06001A7B RID: 6779
		ECPoint Multiply(ECPoint p, BigInteger k);
	}
}
