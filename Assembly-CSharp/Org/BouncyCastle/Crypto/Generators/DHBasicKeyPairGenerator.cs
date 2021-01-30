using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000474 RID: 1140
	public class DHBasicKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x06002994 RID: 10644 RVA: 0x000CE443 File Offset: 0x000CC643
		public virtual void Init(KeyGenerationParameters parameters)
		{
			this.param = (DHKeyGenerationParameters)parameters;
		}

		// Token: 0x06002995 RID: 10645 RVA: 0x000CE454 File Offset: 0x000CC654
		public virtual AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			DHParameters parameters = this.param.Parameters;
			BigInteger x = instance.CalculatePrivate(parameters, this.param.Random);
			return new AsymmetricCipherKeyPair(new DHPublicKeyParameters(instance.CalculatePublic(parameters, x), parameters), new DHPrivateKeyParameters(x, parameters));
		}

		// Token: 0x04001B5D RID: 7005
		private DHKeyGenerationParameters param;
	}
}
