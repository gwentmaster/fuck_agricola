using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200037F RID: 895
	public interface ISignatureFactory
	{
		// Token: 0x170003C4 RID: 964
		// (get) Token: 0x0600220E RID: 8718
		object AlgorithmDetails { get; }

		// Token: 0x0600220F RID: 8719
		IStreamCalculator CreateCalculator();
	}
}
