using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x020002F8 RID: 760
	public interface ECEndomorphism
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06001AAE RID: 6830
		ECPointMap PointMap { get; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06001AAF RID: 6831
		bool HasEfficientPointMap { get; }
	}
}
