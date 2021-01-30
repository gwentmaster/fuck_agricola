using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Signers
{
	// Token: 0x02000407 RID: 1031
	public class DsaSigner : IDsa
	{
		// Token: 0x0600265E RID: 9822 RVA: 0x000C11D3 File Offset: 0x000BF3D3
		public DsaSigner()
		{
			this.kCalculator = new RandomDsaKCalculator();
		}

		// Token: 0x0600265F RID: 9823 RVA: 0x000C11E6 File Offset: 0x000BF3E6
		public DsaSigner(IDsaKCalculator kCalculator)
		{
			this.kCalculator = kCalculator;
		}

		// Token: 0x1700044C RID: 1100
		// (get) Token: 0x06002660 RID: 9824 RVA: 0x000C11F5 File Offset: 0x000BF3F5
		public virtual string AlgorithmName
		{
			get
			{
				return "DSA";
			}
		}

		// Token: 0x06002661 RID: 9825 RVA: 0x000C11FC File Offset: 0x000BF3FC
		public virtual void Init(bool forSigning, ICipherParameters parameters)
		{
			SecureRandom provided = null;
			if (forSigning)
			{
				if (parameters is ParametersWithRandom)
				{
					ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
					provided = parametersWithRandom.Random;
					parameters = parametersWithRandom.Parameters;
				}
				if (!(parameters is DsaPrivateKeyParameters))
				{
					throw new InvalidKeyException("DSA private key required for signing");
				}
				this.key = (DsaPrivateKeyParameters)parameters;
			}
			else
			{
				if (!(parameters is DsaPublicKeyParameters))
				{
					throw new InvalidKeyException("DSA public key required for verification");
				}
				this.key = (DsaPublicKeyParameters)parameters;
			}
			this.random = this.InitSecureRandom(forSigning && !this.kCalculator.IsDeterministic, provided);
		}

		// Token: 0x06002662 RID: 9826 RVA: 0x000C128C File Offset: 0x000BF48C
		public virtual BigInteger[] GenerateSignature(byte[] message)
		{
			DsaParameters parameters = this.key.Parameters;
			BigInteger q = parameters.Q;
			BigInteger bigInteger = this.CalculateE(q, message);
			BigInteger x = ((DsaPrivateKeyParameters)this.key).X;
			if (this.kCalculator.IsDeterministic)
			{
				this.kCalculator.Init(q, x, message);
			}
			else
			{
				this.kCalculator.Init(q, this.random);
			}
			BigInteger bigInteger2 = this.kCalculator.NextK();
			BigInteger bigInteger3 = parameters.G.ModPow(bigInteger2, parameters.P).Mod(q);
			bigInteger2 = bigInteger2.ModInverse(q).Multiply(bigInteger.Add(x.Multiply(bigInteger3)));
			BigInteger bigInteger4 = bigInteger2.Mod(q);
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x06002663 RID: 9827 RVA: 0x000C1354 File Offset: 0x000BF554
		public virtual bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
		{
			DsaParameters parameters = this.key.Parameters;
			BigInteger q = parameters.Q;
			BigInteger bigInteger = this.CalculateE(q, message);
			if (r.SignValue <= 0 || q.CompareTo(r) <= 0)
			{
				return false;
			}
			if (s.SignValue <= 0 || q.CompareTo(s) <= 0)
			{
				return false;
			}
			BigInteger val = s.ModInverse(q);
			BigInteger bigInteger2 = bigInteger.Multiply(val).Mod(q);
			BigInteger bigInteger3 = r.Multiply(val).Mod(q);
			BigInteger p = parameters.P;
			bigInteger2 = parameters.G.ModPow(bigInteger2, p);
			bigInteger3 = ((DsaPublicKeyParameters)this.key).Y.ModPow(bigInteger3, p);
			return bigInteger2.Multiply(bigInteger3).Mod(p).Mod(q).Equals(r);
		}

		// Token: 0x06002664 RID: 9828 RVA: 0x000C1420 File Offset: 0x000BF620
		protected virtual BigInteger CalculateE(BigInteger n, byte[] message)
		{
			int length = Math.Min(message.Length, n.BitLength / 8);
			return new BigInteger(1, message, 0, length);
		}

		// Token: 0x06002665 RID: 9829 RVA: 0x000C1447 File Offset: 0x000BF647
		protected virtual SecureRandom InitSecureRandom(bool needed, SecureRandom provided)
		{
			if (!needed)
			{
				return null;
			}
			if (provided == null)
			{
				return new SecureRandom();
			}
			return provided;
		}

		// Token: 0x040019AD RID: 6573
		protected readonly IDsaKCalculator kCalculator;

		// Token: 0x040019AE RID: 6574
		protected DsaKeyParameters key;

		// Token: 0x040019AF RID: 6575
		protected SecureRandom random;
	}
}
