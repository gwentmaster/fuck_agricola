using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000409 RID: 1033
	public class ECGost3410Signer : IDsa
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06002671 RID: 9841 RVA: 0x000C17CF File Offset: 0x000BF9CF
		public virtual string AlgorithmName
		{
			get
			{
				return "ECGOST3410";
			}
		}

		// Token: 0x06002672 RID: 9842 RVA: 0x000C17D8 File Offset: 0x000BF9D8
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
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

		// Token: 0x06002673 RID: 9843 RVA: 0x000C1858 File Offset: 0x000BFA58
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger val = new BigInteger(1, array);
			ECDomainParameters parameters = this.key.Parameters;
			BigInteger n = parameters.N;
			BigInteger d = ((ECPrivateKeyParameters)this.key).D;
			ECMultiplier ecmultiplier = this.CreateBasePointMultiplier();
			BigInteger bigInteger2;
			BigInteger bigInteger3;
			for (;;)
			{
				BigInteger bigInteger = new BigInteger(n.BitLength, this.random);
				if (bigInteger.SignValue != 0)
				{
					bigInteger2 = ecmultiplier.Multiply(parameters.G, bigInteger).Normalize().AffineXCoord.ToBigInteger().Mod(n);
					if (bigInteger2.SignValue != 0)
					{
						bigInteger3 = bigInteger.Multiply(val).Add(d.Multiply(bigInteger2)).Mod(n);
						if (bigInteger3.SignValue != 0)
						{
							break;
						}
					}
				}
			}
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x06002674 RID: 9844 RVA: 0x000C1948 File Offset: 0x000BFB48
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger bigInteger = new BigInteger(1, array);
			BigInteger n = this.key.Parameters.N;
			if (r.CompareTo(BigInteger.One) < 0 || r.CompareTo(n) >= 0)
			{
				return false;
			}
			if (s.CompareTo(BigInteger.One) < 0 || s.CompareTo(n) >= 0)
			{
				return false;
			}
			BigInteger val = bigInteger.ModInverse(n);
			BigInteger a = s.Multiply(val).Mod(n);
			BigInteger b = n.Subtract(r).Multiply(val).Mod(n);
			ECPoint g = this.key.Parameters.G;
			ECPoint q = ((ECPublicKeyParameters)this.key).Q;
			ECPoint ecpoint = ECAlgorithms.SumOfTwoMultiplies(g, a, q, b).Normalize();
			return !ecpoint.IsInfinity && ecpoint.AffineXCoord.ToBigInteger().Mod(n).Equals(r);
		}

		// Token: 0x06002675 RID: 9845 RVA: 0x000C177C File Offset: 0x000BF97C
		protected virtual ECMultiplier CreateBasePointMultiplier()
		{
			return new FixedPointCombMultiplier();
		}

		// Token: 0x040019B4 RID: 6580
		private ECKeyParameters key;

		// Token: 0x040019B5 RID: 6581
		private SecureRandom random;
	}
}
