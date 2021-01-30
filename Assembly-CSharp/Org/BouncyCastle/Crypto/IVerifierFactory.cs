using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000385 RID: 901
	public interface IVerifierFactory
	{
		// Token: 0x170003C8 RID: 968
		// (get) Token: 0x06002223 RID: 8739
		object AlgorithmDetails { get; }

		// Token: 0x06002224 RID: 8740
		IStreamCalculator CreateCalculator();
	}
}
