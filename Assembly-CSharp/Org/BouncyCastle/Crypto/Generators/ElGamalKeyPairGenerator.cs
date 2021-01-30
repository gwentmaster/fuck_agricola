using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x0200047A RID: 1146
	public class ElGamalKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060029B0 RID: 10672 RVA: 0x000CEB07 File Offset: 0x000CCD07
		public void Init(KeyGenerationParameters parameters)
		{
			this.param = (ElGamalKeyGenerationParameters)parameters;
		}

		// Token: 0x060029B1 RID: 10673 RVA: 0x000CEB18 File Offset: 0x000CCD18
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DHKeyGeneratorHelper instance = DHKeyGeneratorHelper.Instance;
			ElGamalParameters parameters = this.param.Parameters;
			DHParameters dhParams = new DHParameters(parameters.P, parameters.G, null, 0, parameters.L);
			BigInteger x = instance.CalculatePrivate(dhParams, this.param.Random);
			return new AsymmetricCipherKeyPair(new ElGamalPublicKeyParameters(instance.CalculatePublic(dhParams, x), parameters), new ElGamalPrivateKeyParameters(x, parameters));
		}

		// Token: 0x04001B6A RID: 7018
		private ElGamalKeyGenerationParameters param;
	}
}
