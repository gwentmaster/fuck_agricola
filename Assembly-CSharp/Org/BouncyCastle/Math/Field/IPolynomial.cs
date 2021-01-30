using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D7 RID: 727
	public interface IPolynomial
	{
		// Token: 0x17000206 RID: 518
		// (get) Token: 0x06001908 RID: 6408
		int Degree { get; }

		// Token: 0x06001909 RID: 6409
		int[] GetExponentsPresent();
	}
}
