using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200049A RID: 1178
	internal class RsaCoreEngine
	{
		// Token: 0x06002AFE RID: 11006 RVA: 0x000DA278 File Offset: 0x000D8478
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			if (!(parameters is RsaKeyParameters))
			{
				throw new InvalidKeyException("Not an RSA key");
			}
			this.key = (RsaKeyParameters)parameters;
			this.forEncryption = forEncryption;
			this.bitSize = this.key.Modulus.BitLength;
		}

		// Token: 0x06002AFF RID: 11007 RVA: 0x000DA2D6 File Offset: 0x000D84D6
		public virtual int GetInputBlockSize()
		{
			if (this.forEncryption)
			{
				return (this.bitSize - 1) / 8;
			}
			return (this.bitSize + 7) / 8;
		}

		// Token: 0x06002B00 RID: 11008 RVA: 0x000DA2F5 File Offset: 0x000D84F5
		public virtual int GetOutputBlockSize()
		{
			if (this.forEncryption)
			{
				return (this.bitSize + 7) / 8;
			}
			return (this.bitSize - 1) / 8;
		}

		// Token: 0x06002B01 RID: 11009 RVA: 0x000DA314 File Offset: 0x000D8514
		public virtual BigInteger ConvertInput(byte[] inBuf, int inOff, int inLen)
		{
			int num = (this.bitSize + 7) / 8;
			if (inLen > num)
			{
				throw new DataLengthException("input too large for RSA cipher.");
			}
			BigInteger bigInteger = new BigInteger(1, inBuf, inOff, inLen);
			if (bigInteger.CompareTo(this.key.Modulus) >= 0)
			{
				throw new DataLengthException("input too large for RSA cipher.");
			}
			return bigInteger;
		}

		// Token: 0x06002B02 RID: 11010 RVA: 0x000DA364 File Offset: 0x000D8564
		public virtual byte[] ConvertOutput(BigInteger result)
		{
			byte[] array = result.ToByteArrayUnsigned();
			if (this.forEncryption)
			{
				int outputBlockSize = this.GetOutputBlockSize();
				if (array.Length < outputBlockSize)
				{
					byte[] array2 = new byte[outputBlockSize];
					array.CopyTo(array2, array2.Length - array.Length);
					array = array2;
				}
			}
			return array;
		}

		// Token: 0x06002B03 RID: 11011 RVA: 0x000DA3A8 File Offset: 0x000D85A8
		public virtual BigInteger ProcessBlock(BigInteger input)
		{
			if (this.key is RsaPrivateCrtKeyParameters)
			{
				RsaPrivateCrtKeyParameters rsaPrivateCrtKeyParameters = (RsaPrivateCrtKeyParameters)this.key;
				BigInteger p = rsaPrivateCrtKeyParameters.P;
				BigInteger q = rsaPrivateCrtKeyParameters.Q;
				BigInteger dp = rsaPrivateCrtKeyParameters.DP;
				BigInteger dq = rsaPrivateCrtKeyParameters.DQ;
				BigInteger qinv = rsaPrivateCrtKeyParameters.QInv;
				BigInteger bigInteger = input.Remainder(p).ModPow(dp, p);
				BigInteger bigInteger2 = input.Remainder(q).ModPow(dq, q);
				return bigInteger.Subtract(bigInteger2).Multiply(qinv).Mod(p).Multiply(q).Add(bigInteger2);
			}
			return input.ModPow(this.key.Exponent, this.key.Modulus);
		}

		// Token: 0x04001C53 RID: 7251
		private RsaKeyParameters key;

		// Token: 0x04001C54 RID: 7252
		private bool forEncryption;

		// Token: 0x04001C55 RID: 7253
		private int bitSize;
	}
}
