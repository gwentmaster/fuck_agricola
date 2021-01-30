using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000386 RID: 902
	public interface IVerifierFactoryProvider
	{
		// Token: 0x06002225 RID: 8741
		IVerifierFactory CreateVerifierFactory(object algorithmDetails);
	}
}
