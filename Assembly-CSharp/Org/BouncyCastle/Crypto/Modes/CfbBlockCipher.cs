using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045B RID: 1115
	public class CfbBlockCipher : IBlockCipher
	{
		// Token: 0x06002874 RID: 10356 RVA: 0x000C7B78 File Offset: 0x000C5D78
		public CfbBlockCipher(IBlockCipher cipher, int bitBlockSize)
		{
			this.cipher = cipher;
			this.blockSize = bitBlockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.cfbV = new byte[cipher.GetBlockSize()];
			this.cfbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x000C7BCE File Offset: 0x000C5DCE
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x000C7BD8 File Offset: 0x000C5DD8
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.encrypting = forEncryption;
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				int num = this.IV.Length - iv.Length;
				Array.Copy(iv, 0, this.IV, num, iv.Length);
				Array.Clear(this.IV, 0, num);
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(true, parameters);
			}
		}

		// Token: 0x170004C0 RID: 1216
		// (get) Token: 0x06002877 RID: 10359 RVA: 0x000C7C49 File Offset: 0x000C5E49
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170004C1 RID: 1217
		// (get) Token: 0x06002878 RID: 10360 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002879 RID: 10361 RVA: 0x000C7C6D File Offset: 0x000C5E6D
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x0600287A RID: 10362 RVA: 0x000C7C75 File Offset: 0x000C5E75
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.encrypting)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x0600287B RID: 10363 RVA: 0x000C7C98 File Offset: 0x000C5E98
		public int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				outBytes[outOff + i] = (this.cfbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
			Array.Copy(outBytes, outOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x0600287C RID: 10364 RVA: 0x000C7D68 File Offset: 0x000C5F68
		public int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.cfbV, 0, this.cfbOutV, 0);
			Array.Copy(this.cfbV, this.blockSize, this.cfbV, 0, this.cfbV.Length - this.blockSize);
			Array.Copy(input, inOff, this.cfbV, this.cfbV.Length - this.blockSize, this.blockSize);
			for (int i = 0; i < this.blockSize; i++)
			{
				outBytes[outOff + i] = (this.cfbOutV[i] ^ input[inOff + i]);
			}
			return this.blockSize;
		}

		// Token: 0x0600287D RID: 10365 RVA: 0x000C7E35 File Offset: 0x000C6035
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.cfbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x04001A9A RID: 6810
		private byte[] IV;

		// Token: 0x04001A9B RID: 6811
		private byte[] cfbV;

		// Token: 0x04001A9C RID: 6812
		private byte[] cfbOutV;

		// Token: 0x04001A9D RID: 6813
		private bool encrypting;

		// Token: 0x04001A9E RID: 6814
		private readonly int blockSize;

		// Token: 0x04001A9F RID: 6815
		private readonly IBlockCipher cipher;
	}
}
