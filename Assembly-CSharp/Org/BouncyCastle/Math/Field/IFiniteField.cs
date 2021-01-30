using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D6 RID: 726
	public interface IFiniteField
	{
		// Token: 0x17000204 RID: 516
		// (get) Token: 0x06001906 RID: 6406
		BigInteger Characteristic { get; }

		// Token: 0x17000205 RID: 517
		// (get) Token: 0x06001907 RID: 6407
		int Dimension { get; }
	}
}
