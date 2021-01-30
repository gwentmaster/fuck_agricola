using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000374 RID: 884
	public interface IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060021DA RID: 8666
		void Init(KeyGenerationParameters parameters);

		// Token: 0x060021DB RID: 8667
		AsymmetricCipherKeyPair GenerateKeyPair();
	}
}
