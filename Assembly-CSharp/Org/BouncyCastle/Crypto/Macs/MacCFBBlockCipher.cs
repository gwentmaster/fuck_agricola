using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046C RID: 1132
	internal class MacCFBBlockCipher : IBlockCipher
	{
		// Token: 0x0600293C RID: 10556 RVA: 0x000CBD64 File Offset: 0x000C9F64
		public MacCFBBlockCipher(IBlockCipher cipher, int bitBlockSize)
		{
			this.cipher = cipher;
			this.blockSize = bitBlockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.cfbV = new byte[cipher.GetBlockSize()];
			this.cfbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x0600293D RID: 10557 RVA: 0x000CBDBC File Offset: 0x000C9FBC
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length < this.IV.Length)
				{
					Array.Copy(iv, 0, this.IV, this.IV.Length - iv.Length, iv.Length);
				}
				else
				{
					Array.Copy(iv, 0, this.IV, 0, this.IV.Length);
				}
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x170004D0 RID: 1232
		// (get) Token: 0x0600293E RID: 10558 RVA: 0x000CBE39 File Offset: 0x000CA039
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/CFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170004D1 RID: 1233
		// (get) Token: 0x0600293F RID: 10559 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002940 RID: 10560 RVA: 0x000CBE5D File Offset: 0x000CA05D
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x06002941 RID: 10561 RVA: 0x000CBE68 File Offset: 0x000CA068
		public int ProcessBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
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

		// Token: 0x06002942 RID: 10562 RVA: 0x000CBF36 File Offset: 0x000CA136
		public void Reset()
		{
			this.IV.CopyTo(this.cfbV, 0);
			this.cipher.Reset();
		}

		// Token: 0x06002943 RID: 10563 RVA: 0x000CBF55 File Offset: 0x000CA155
		public void GetMacBlock(byte[] mac)
		{
			this.cipher.ProcessBlock(this.cfbV, 0, mac, 0);
		}

		// Token: 0x04001B0C RID: 6924
		private byte[] IV;

		// Token: 0x04001B0D RID: 6925
		private byte[] cfbV;

		// Token: 0x04001B0E RID: 6926
		private byte[] cfbOutV;

		// Token: 0x04001B0F RID: 6927
		private readonly int blockSize;

		// Token: 0x04001B10 RID: 6928
		private readonly IBlockCipher cipher;
	}
}
