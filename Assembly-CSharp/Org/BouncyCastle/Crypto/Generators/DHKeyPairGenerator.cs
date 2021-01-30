using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000476 RID: 1142
	public class DHKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x0600299B RID: 10651 RVA: 0x000CE55A File Offset: 0x000CC75A
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.param = (DHKeyGenerationParameters)parameters;
		}

		// Token: 0x0600299C RID: 10652 RVA: 0x000CE568 File Offset: 0x000CC768
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			DHParameters parameters = this.param.Parameters;
			BigInteger x = instance.CalculatePrivate(parameters, this.param.Random);
			return new AsymmetricCipherKeyPair(new DHPublicKeyParameters(instance.CalculatePublic(parameters, x), parameters), new DHPrivateKeyParameters(x, parameters));
		}

		// Token: 0x04001B5F RID: 7007
		private DHKeyGenerationParameters param;
	}
}
