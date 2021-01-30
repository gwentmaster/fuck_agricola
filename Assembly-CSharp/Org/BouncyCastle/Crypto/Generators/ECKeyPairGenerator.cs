using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Sec;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.EC;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000479 RID: 1145
	public class ECKeyPairGenerator : IAsymmetricCipherKeyPairGenerator
	{
		// Token: 0x060029A9 RID: 10665 RVA: 0x000CE88D File Offset: 0x000CCA8D
		public ECKeyPairGenerator() : this("EC")
		{
		}

		// Token: 0x060029AA RID: 10666 RVA: 0x000CE89A File Offset: 0x000CCA9A
		public ECKeyPairGenerator(string algorithm)
		{
			if (algorithm == null)
			{
				throw new ArgumentNullException("algorithm");
			}
			this.algorithm = ECKeyParameters.VerifyAlgorithmName(algorithm);
		}

		// Token: 0x060029AB RID: 10667 RVA: 0x000CE8BC File Offset: 0x000CCABC
		public void Init(KeyGenerationParameters parameters)
		{
			if (parameters is ECKeyGenerationParameters)
			{
				ECKeyGenerationParameters eckeyGenerationParameters = (ECKeyGenerationParameters)parameters;
				this.publicKeyParamSet = eckeyGenerationParameters.PublicKeyParamSet;
				this.parameters = eckeyGenerationParameters.DomainParameters;
			}
			else
			{
				int strength = parameters.Strength;
				DerObjectIdentifier oid;
				if (strength <= 239)
				{
					if (strength == 192)
					{
						oid = X9ObjectIdentifiers.Prime192v1;
						goto IL_AA;
					}
					if (strength == 224)
					{
						oid = SecObjectIdentifiers.SecP224r1;
						goto IL_AA;
					}
					if (strength == 239)
					{
						oid = X9ObjectIdentifiers.Prime239v1;
						goto IL_AA;
					}
				}
				else
				{
					if (strength == 256)
					{
						oid = X9ObjectIdentifiers.Prime256v1;
						goto IL_AA;
					}
					if (strength == 384)
					{
						oid = SecObjectIdentifiers.SecP384r1;
						goto IL_AA;
					}
					if (strength == 521)
					{
						oid = SecObjectIdentifiers.SecP521r1;
						goto IL_AA;
					}
				}
				throw new InvalidParameterException("unknown key size.");
				IL_AA:
				X9ECParameters x9ECParameters = ECKeyPairGenerator.FindECCurveByOid(oid);
				this.publicKeyParamSet = oid;
				this.parameters = new ECDomainParameters(x9ECParameters.Curve, x9ECParameters.G, x9ECParameters.N, x9ECParameters.H, x9ECParameters.GetSeed());
			}
			this.random = parameters.Random;
			if (this.random == null)
			{
				this.random = new SecureRandom();
			}
		}

		// Token: 0x060029AC RID: 10668 RVA: 0x000CE9CC File Offset: 0x000CCBCC
		public AsymmetricCipherKeyPair GenerateKeyPair()
		{
			BigInteger n = this.parameters.N;
			int num = n.BitLength >> 2;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(n.BitLength, this.random);
			}
			while (bigInteger.CompareTo(BigInteger.Two) < 0 || bigInteger.CompareTo(n) >= 0 || WNafUtilities.GetNafWeight(bigInteger) < num);
			ECPoint q = this.CreateBasePointMultiplier().Multiply(this.parameters.G, bigInteger);
			if (this.publicKeyParamSet != null)
			{
				return new AsymmetricCipherKeyPair(new ECPublicKeyParameters(this.algorithm, q, this.publicKeyParamSet), new ECPrivateKeyParameters(this.algorithm, bigInteger, this.publicKeyParamSet));
			}
			return new AsymmetricCipherKeyPair(new ECPublicKeyParameters(this.algorithm, q, this.parameters), new ECPrivateKeyParameters(this.algorithm, bigInteger, this.parameters));
		}

		// Token: 0x060029AD RID: 10669 RVA: 0x000C177C File Offset: 0x000BF97C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x060029AE RID: 10670 RVA: 0x000CEA94 File Offset: 0x000CCC94
		internal static X9ECParameters FindECCurveByOid(DerObjectIdentifier oid)
		{
			X9ECParameters byOid = CustomNamedCurves.GetByOid(oid);
			if (byOid == null)
			{
				byOid = ECNamedCurveTable.GetByOid(oid);
			}
			return byOid;
		}

		// Token: 0x060029AF RID: 10671 RVA: 0x000CEAB4 File Offset: 0x000CCCB4
		internal static ECPublicKeyParameters GetCorrespondingPublicKey(ECPrivateKeyParameters privKey)
		{
			ECDomainParameters ecdomainParameters = privKey.Parameters;
			ECPoint q = new FixedPointCombMultiplier().Multiply(ecdomainParameters.G, privKey.D);
			if (privKey.PublicKeyParamSet != null)
			{
				return new ECPublicKeyParameters(privKey.AlgorithmName, q, privKey.PublicKeyParamSet);
			}
			return new ECPublicKeyParameters(privKey.AlgorithmName, q, ecdomainParameters);
		}

		// Token: 0x04001B66 RID: 7014
		private readonly string algorithm;

		// Token: 0x04001B67 RID: 7015
		private ECDomainParameters parameters;

		// Token: 0x04001B68 RID: 7016
		private DerObjectIdentifier publicKeyParamSet;

		// Token: 0x04001B69 RID: 7017
		private SecureRandom random;
	}
}
