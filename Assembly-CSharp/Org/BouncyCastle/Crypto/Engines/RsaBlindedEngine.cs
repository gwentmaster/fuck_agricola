using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000499 RID: 1177
	public class RsaBlindedEngine : IAsymmetricBlockCipher
	{
		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06002AF8 RID: 11000 RVA: 0x000DA0DF File Offset: 0x000D82DF
		public virtual string AlgorithmName
		{
			get
			{
				return "RSA";
			}
		}

		// Token: 0x06002AF9 RID: 11001 RVA: 0x000DA0E8 File Offset: 0x000D82E8
		public virtual void Init(bool forEncryption, ICipherParameters param)
		{
			this.core.Init(forEncryption, param);
			if (param is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)param;
				this.key = (RsaKeyParameters)parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
				return;
			}
			this.key = (RsaKeyParameters)param;
			this.random = new SecureRandom();
		}

		// Token: 0x06002AFA RID: 11002 RVA: 0x000DA146 File Offset: 0x000D8346
		public virtual int GetInputBlockSize()
		{
			return this.core.GetInputBlockSize();
		}

		// Token: 0x06002AFB RID: 11003 RVA: 0x000DA153 File Offset: 0x000D8353
		public virtual int GetOutputBlockSize()
		{
			return this.core.GetOutputBlockSize();
		}

		// Token: 0x06002AFC RID: 11004 RVA: 0x000DA160 File Offset: 0x000D8360
		public virtual byte[] ProcessBlock(byte[] inBuf, int inOff, int inLen)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("RSA engine not initialised");
			}
			BigInteger bigInteger = this.core.ConvertInput(inBuf, inOff, inLen);
			BigInteger bigInteger4;
			if (this.key is RsaPrivateCrtKeyParameters)
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)this.key;
				BigInteger publicExponent = rsaPrivateCrtKeyParameters.PublicExponent;
				if (publicExponent != null)
				{
					BigInteger modulus = rsaPrivateCrtKeyParameters.Modulus;
					BigInteger bigInteger2 = BigIntegers.CreateRandomInRange(BigInteger.One, modulus.Subtract(BigInteger.One), this.random);
					BigInteger input = bigInteger2.ModPow(publicExponent, modulus).Multiply(bigInteger).Mod(modulus);
					BigInteger bigInteger3 = this.core.ProcessBlock(input);
					BigInteger val = bigInteger2.ModInverse(modulus);
					bigInteger4 = bigInteger3.Multiply(val).Mod(modulus);
					if (!bigInteger.Equals(bigInteger4.ModPow(publicExponent, modulus)))
					{
						throw new InvalidOperationException("RSA engine faulty decryption/signing detected");
					}
				}
				else
				{
					bigInteger4 = this.core.ProcessBlock(bigInteger);
				}
			}
			else
			{
				bigInteger4 = this.core.ProcessBlock(bigInteger);
			}
			return this.core.ConvertOutput(bigInteger4);
		}

		// Token: 0x04001C50 RID: 7248
		private readonly RsaCoreEngine core = new RsaCoreEngine();

		// Token: 0x04001C51 RID: 7249
		private RsaKeyParameters key;

		// Token: 0x04001C52 RID: 7250
		private SecureRandom random;
	}
}
