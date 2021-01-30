using System;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040A RID: 1034
	public class ECNRSigner : IDsa
	{
		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06002677 RID: 9847 RVA: 0x000C1A4E File Offset: 0x000BFC4E
		public virtual string AlgorithmName
		{
			get
			{
				return "ECNR";
			}
		}

		// Token: 0x06002678 RID: 9848 RVA: 0x000C1A58 File Offset: 0x000BFC58
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			this.forSigning = forSigning;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					this.random = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				else
				{
					this.random = new SecureRandom();
				}
				if (!(parameters is ECPrivateKeyParameters))
				{
					throw new InvalidKeyException("EC private key required for signing");
				}
				this.key = (ECPrivateKeyParameters)parameters;
				return;
			}
			else
			{
				if (!(parameters is ECPublicKeyParameters))
				{
					throw new InvalidKeyException("EC public key required for verification");
				}
				this.key = (ECPublicKeyParameters)parameters;
				return;
			}
		}

		// Token: 0x06002679 RID: 9849 RVA: 0x000C1AE0 File Offset: 0x000BFCE0
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			if (!this.forSigning)
			{
				throw new InvalidOperationException("not initialised for signing");
			}
			BigInteger n = ((ECPrivateKeyParameters)this.key).Parameters.N;
			int bitLength = n.BitLength;
			BigInteger bigInteger = new BigInteger(1, message);
			int bitLength2 = bigInteger.BitLength;
			ECPrivateKeyParameters ecprivateKeyParameters = (ECPrivateKeyParameters)this.key;
			if (bitLength2 > bitLength)
			{
				throw new DataLengthException("input too large for ECNR key.");
			}
			AsymmetricCipherKeyPair asymmetricCipherKeyPair;
			BigInteger bigInteger2;
			do
			{
				ECKeyPairGenerator eckeyPairGenerator = new ECKeyPairGenerator();
				eckeyPairGenerator.Init(new ECKeyGenerationParameters(ecprivateKeyParameters.Parameters, this.random));
				asymmetricCipherKeyPair = eckeyPairGenerator.GenerateKeyPair();
				bigInteger2 = ((ECPublicKeyParameters)asymmetricCipherKeyPair.Public).Q.AffineXCoord.ToBigInteger().Add(bigInteger).Mod(n);
			}
			while (bigInteger2.SignValue == 0);
			BigInteger d = ecprivateKeyParameters.D;
			BigInteger bigInteger3 = ((ECPrivateKeyParameters)asymmetricCipherKeyPair.Private).D.Subtract(bigInteger2.Multiply(d)).Mod(n);
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x0600267A RID: 9850 RVA: 0x000C1BE0 File Offset: 0x000BFDE0
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			if (this.forSigning)
			{
				throw new InvalidOperationException("not initialised for verifying");
			}
			ECPublicKeyParameters ecpublicKeyParameters = (ECPublicKeyParameters)this.key;
			BigInteger n = ecpublicKeyParameters.Parameters.N;
			int bitLength = n.BitLength;
			BigInteger bigInteger = new BigInteger(1, message);
			if (bigInteger.BitLength > bitLength)
			{
				throw new DataLengthException("input too large for ECNR key.");
			}
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.Zero) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			ECPoint g = ecpublicKeyParameters.Parameters.G;
			ECPoint q = ecpublicKeyParameters.Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, s, q, r).Normalize();
			if (ecpoint.IsInfinity)
			{
				return false;
			}
			BigInteger n2 = ecpoint.AffineXCoord.ToBigInteger();
			return r.Subtract(n2).Mod(n).Equals(bigInteger);
		}

		// Token: 0x040019B6 RID: 6582
		private bool forSigning;

		// Token: 0x040019B7 RID: 6583
		private ECKeyParameters key;

		// Token: 0x040019B8 RID: 6584
		private SecureRandom random;
	}
}
