using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x0200040C RID: 1036
	public class Gost3410Signer : IDsa
	{
		// Token: 0x17000451 RID: 1105
		// (get) Token: 0x06002684 RID: 9860 RVA: 0x000C1EC9 File Offset: 0x000C00C9
		public virtual string AlgorithmName
		{
			get
			{
				return "GOST3410";
			}
		}

		// Token: 0x06002685 RID: 9861 RVA: 0x000C1ED0 File Offset: 0x000C00D0
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
				if (!(parameters is Gost3410PrivateKeyParameters))
				{
					throw new InvalidKeyException("GOST3410 private key required for signing");
				}
				this.key = (Gost3410PrivateKeyParameters)parameters;
				return;
			}
			else
			{
				if (!(parameters is Gost3410PublicKeyParameters))
				{
					throw new InvalidKeyException("GOST3410 public key required for signing");
				}
				this.key = (Gost3410PublicKeyParameters)parameters;
				return;
			}
		}

		// Token: 0x06002686 RID: 9862 RVA: 0x000C1F50 File Offset: 0x000C0150
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger val = new BigInteger(1, array);
			Gost3410Parameters parameters = this.key.Parameters;
			BigInteger bigInteger;
			do
			{
				bigInteger = new BigInteger(parameters.Q.BitLength, this.random);
			}
			while (bigInteger.CompareTo(parameters.Q) >= 0);
			BigInteger bigInteger2 = parameters.A.ModPow(bigInteger, parameters.P).Mod(parameters.Q);
			BigInteger bigInteger3 = bigInteger.Multiply(val).Add(((Gost3410PrivateKeyParameters)this.key).X.Multiply(bigInteger2)).Mod(parameters.Q);
			return new BigInteger[]
			{
				bigInteger2,
				bigInteger3
			};
		}

		// Token: 0x06002687 RID: 9863 RVA: 0x000C2020 File Offset: 0x000C0220
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			byte[] array = new byte[message.Length];
			for (int num = 0; num != array.Length; num++)
			{
				array[num] = message[array.Length - 1 - num];
			}
			BigInteger bigInteger = new BigInteger(1, array);
			Gost3410Parameters parameters = this.key.Parameters;
			if (r.SignValue < 0 || parameters.Q.CompareTo(r) <= 0)
			{
				return false;
			}
			if (s.SignValue < 0 || parameters.Q.CompareTo(s) <= 0)
			{
				return false;
			}
			BigInteger val = bigInteger.ModPow(parameters.Q.Subtract(BigInteger.Two), parameters.Q);
			BigInteger bigInteger2 = s.Multiply(val).Mod(parameters.Q);
			BigInteger bigInteger3 = parameters.Q.Subtract(r).Multiply(val).Mod(parameters.Q);
			bigInteger2 = parameters.A.ModPow(bigInteger2, parameters.P);
			bigInteger3 = ((Gost3410PublicKeyParameters)this.key).Y.ModPow(bigInteger3, parameters.P);
			return bigInteger2.Multiply(bigInteger3).Mod(parameters.P).Mod(parameters.Q).Equals(r);
		}

		// Token: 0x040019BC RID: 6588
		private Gost3410KeyParameters key;

		// Token: 0x040019BD RID: 6589
		private SecureRandom random;
	}
}
