using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000462 RID: 1122
	public class OfbBlockCipher : IBlockCipher
	{
		// Token: 0x060028E0 RID: 10464 RVA: 0x000CA2C4 File Offset: 0x000C84C4
		public OfbBlockCipher(IBlockCipher cipher, int blockSize)
		{
			this.cipher = cipher;
			this.blockSize = blockSize / 8;
			this.IV = new byte[cipher.GetBlockSize()];
			this.ofbV = new byte[cipher.GetBlockSize()];
			this.ofbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x060028E1 RID: 10465 RVA: 0x000CA31A File Offset: 0x000C851A
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060028E2 RID: 10466 RVA: 0x000CA324 File Offset: 0x000C8524
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithIV)
			{
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				byte[] iv = parametersWithIV.GetIV();
				if (iv.Length < this.IV.Length)
				{
					Array.Copy(iv, 0, this.IV, this.IV.Length - iv.Length, iv.Length);
					for (int i = 0; i < this.IV.Length - iv.Length; i++)
					{
						this.IV[i] = 0;
					}
				}
				else
				{
					Array.Copy(iv, 0, this.IV, 0, this.IV.Length);
				}
				parameters = parametersWithIV.Parameters;
			}
			this.Reset();
			if (parameters != null)
			{
				this.cipher.Init(true, parameters);
			}
		}

		// Token: 0x170004C8 RID: 1224
		// (get) Token: 0x060028E3 RID: 10467 RVA: 0x000CA3C6 File Offset: 0x000C85C6
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/OFB" + this.blockSize * 8;
			}
		}

		// Token: 0x170004C9 RID: 1225
		// (get) Token: 0x060028E4 RID: 10468 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028E5 RID: 10469 RVA: 0x000CA3EA File Offset: 0x000C85EA
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x060028E6 RID: 10470 RVA: 0x000CA3F4 File Offset: 0x000C85F4
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				output[outOff + i] = (this.ofbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.ofbV, this.blockSize, this.ofbV, 0, this.ofbV.Length - this.blockSize);
			Array.Copy(this.ofbOutV, 0, this.ofbV, this.ofbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x060028E7 RID: 10471 RVA: 0x000CA4C6 File Offset: 0x000C86C6
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.ofbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x04001AE5 RID: 6885
		private byte[] IV;

		// Token: 0x04001AE6 RID: 6886
		private byte[] ofbV;

		// Token: 0x04001AE7 RID: 6887
		private byte[] ofbOutV;

		// Token: 0x04001AE8 RID: 6888
		private readonly int blockSize;

		// Token: 0x04001AE9 RID: 6889
		private readonly IBlockCipher cipher;
	}
}
