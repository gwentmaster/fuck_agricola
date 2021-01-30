using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200048A RID: 1162
	public class ElGamalEngine : IAsymmetricBlockCipher
	{
		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x06002A4A RID: 10826 RVA: 0x000D5C3F File Offset: 0x000D3E3F
		public virtual string AlgorithmName
		{
			get
			{
				return "ElGamal";
			}
		}

		// Token: 0x06002A4B RID: 10827 RVA: 0x000D5C48 File Offset: 0x000D3E48
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				ParametersWithRandom parametersWithRandom = (ParametersWithRandom)parameters;
				this.key = (ElGamalKeyParameters)parametersWithRandom.Parameters;
				this.random = parametersWithRandom.Random;
			}
			else
			{
				this.key = (ElGamalKeyParameters)parameters;
				this.random = new SecureRandom();
			}
			this.forEncryption = forEncryption;
			this.bitSize = this.key.Parameters.P.BitLength;
			if (forEncryption)
			{
				if (!(this.key is ElGamalPublicKeyParameters))
				{
					throw new ArgumentException("ElGamalPublicKeyParameters are required for encryption.");
				}
			}
			else if (!(this.key is ElGamalPrivateKeyParameters))
			{
				throw new ArgumentException("ElGamalPrivateKeyParameters are required for decryption.");
			}
		}

		// Token: 0x06002A4C RID: 10828 RVA: 0x000D5CEF File Offset: 0x000D3EEF
		public virtual int GetInputBlockSize()
		{
			if (this.forEncryption)
			{
				return (this.bitSize - 1) / 8;
			}
			return 2 * ((this.bitSize + 7) / 8);
		}

		// Token: 0x06002A4D RID: 10829 RVA: 0x000D5D10 File Offset: 0x000D3F10
		public virtual int GetOutputBlockSize()
		{
			if (this.forEncryption)
			{
				return 2 * ((this.bitSize + 7) / 8);
			}
			return (this.bitSize - 1) / 8;
		}

		// Token: 0x06002A4E RID: 10830 RVA: 0x000D5D34 File Offset: 0x000D3F34
		public virtual byte[] ProcessBlock(byte[] input, int inOff, int length)
		{
			if (this.key == null)
			{
				throw new InvalidOperationException("ElGamal engine not initialised");
			}
			int num = this.forEncryption ? ((this.bitSize - 1 + 7) / 8) : this.GetInputBlockSize();
			if (length > num)
			{
				throw new DataLengthException("input too large for ElGamal cipher.\n");
			}
			BigInteger p = this.key.Parameters.P;
			byte[] array;
			if (this.key is ElGamalPrivateKeyParameters)
			{
				int num2 = length / 2;
				BigInteger bigInteger = new BigInteger(1, input, inOff, num2);
				BigInteger val = new BigInteger(1, input, inOff + num2, num2);
				ElGamalPrivateKeyParameters elGamalPrivateKeyParameters = (ElGamalPrivateKeyParameters)this.key;
				array = bigInteger.ModPow(p.Subtract(BigInteger.One).Subtract(elGamalPrivateKeyParameters.X), p).Multiply(val).Mod(p).ToByteArrayUnsigned();
			}
			else
			{
				BigInteger bigInteger2 = new BigInteger(1, input, inOff, length);
				if (bigInteger2.BitLength >= p.BitLength)
				{
					throw new DataLengthException("input too large for ElGamal cipher.\n");
				}
				ElGamalPublicKeyParameters elGamalPublicKeyParameters = (ElGamalPublicKeyParameters)this.key;
				BigInteger value = p.Subtract(BigInteger.Two);
				BigInteger bigInteger3;
				do
				{
					bigInteger3 = new BigInteger(p.BitLength, this.random);
				}
				while (bigInteger3.SignValue == 0 || bigInteger3.CompareTo(value) > 0);
				BigInteger bigInteger4 = this.key.Parameters.G.ModPow(bigInteger3, p);
				BigInteger bigInteger5 = bigInteger2.Multiply(elGamalPublicKeyParameters.Y.ModPow(bigInteger3, p)).Mod(p);
				array = new byte[this.GetOutputBlockSize()];
				byte[] array2 = bigInteger4.ToByteArrayUnsigned();
				byte[] array3 = bigInteger5.ToByteArrayUnsigned();
				array2.CopyTo(array, array.Length / 2 - array2.Length);
				array3.CopyTo(array, array.Length - array3.Length);
			}
			return array;
		}

		// Token: 0x04001BEC RID: 7148
		private ElGamalKeyParameters key;

		// Token: 0x04001BED RID: 7149
		private SecureRandom random;

		// Token: 0x04001BEE RID: 7150
		private bool forEncryption;

		// Token: 0x04001BEF RID: 7151
		private int bitSize;
	}
}
