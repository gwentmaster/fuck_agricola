using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000478 RID: 1144
	public class DsaKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060029A3 RID: 10659 RVA: 0x000CE7D0 File Offset: 0x000CC9D0
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters == null)
			{
				throw new ArgumentNullException("parameters");
			}
			this.param = (DsaKeyGenerationParameters)parameters;
		}

		// Token: 0x060029A4 RID: 10660 RVA: 0x000CE7EC File Offset: 0x000CC9EC
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			DsaParameters parameters = this.param.Parameters;
			BigInteger x = DsaKeyPairGenerator.GeneratePrivateKey(parameters.Q, this.param.Random);
			return new AsymmetricCipherKeyPair(new DsaPublicKeyParameters(DsaKeyPairGenerator.CalculatePublicKey(parameters.P, parameters.G, x), parameters), new DsaPrivateKeyParameters(x, parameters));
		}

		// Token: 0x060029A5 RID: 10661 RVA: 0x000CE840 File Offset: 0x000CCA40
		private static BigInteger GeneratePrivateKey(BigInteger q, SecureRandom random)
		{
			int num = q.BitLength >> 2;
			BigInteger bigInteger;
			do
			{
				bigInteger = BigIntegers.CreateRandomInRange(DsaKeyPairGenerator.One, q.Subtract(DsaKeyPairGenerator.One), random);
			}
			while (WNafUtilities.GetNafWeight(bigInteger) < num);
			return bigInteger;
		}

		// Token: 0x060029A6 RID: 10662 RVA: 0x000CE877 File Offset: 0x000CCA77
		private static BigInteger CalculatePublicKey(BigInteger p, BigInteger g, BigInteger x)
		{
			return g.ModPow(x, p);
		}

		// Token: 0x04001B64 RID: 7012
		private static readonly BigInteger One = BigInteger.One;

		// Token: 0x04001B65 RID: 7013
		private DsaKeyGenerationParameters param;
	}
}
