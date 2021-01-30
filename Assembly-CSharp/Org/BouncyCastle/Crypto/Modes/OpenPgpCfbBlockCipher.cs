using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000463 RID: 1123
	public class OpenPgpCfbBlockCipher : IBlockCipher
	{
		// Token: 0x060028E8 RID: 10472 RVA: 0x000CA4F0 File Offset: 0x000C86F0
		public OpenPgpCfbBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.IV = new byte[this.blockSize];
			this.FR = new byte[this.blockSize];
			this.FRE = new byte[this.blockSize];
		}

		// Token: 0x060028E9 RID: 10473 RVA: 0x000CA549 File Offset: 0x000C8749
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x170004CA RID: 1226
		// (get) Token: 0x060028EA RID: 10474 RVA: 0x000CA551 File Offset: 0x000C8751
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/OpenPGPCFB";
			}
		}

		// Token: 0x170004CB RID: 1227
		// (get) Token: 0x060028EB RID: 10475 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028EC RID: 10476 RVA: 0x000CA568 File Offset: 0x000C8768
		public int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x060028ED RID: 10477 RVA: 0x000CA575 File Offset: 0x000C8775
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x060028EE RID: 10478 RVA: 0x000CA596 File Offset: 0x000C8796
		public void Reset()
		{
			this.count = 0;
			Array.Copy(this.IV, 0, this.FR, 0, this.FR.Length);
			this.cipher.Reset();
		}

		// Token: 0x060028EF RID: 10479 RVA: 0x000CA5C8 File Offset: 0x000C87C8
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
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
			this.cipher.Init(true, parameters);
		}

		// Token: 0x060028F0 RID: 10480 RVA: 0x000CA66E File Offset: 0x000C886E
		private byte EncryptByte(byte data, int blockOff)
		{
			return this.FRE[blockOff] ^ data;
		}

		// Token: 0x060028F1 RID: 10481 RVA: 0x000CA67C File Offset: 0x000C887C
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			if (this.count > this.blockSize)
			{
				this.FR[this.blockSize - 2] = (outBytes[outOff] = this.EncryptByte(input[inOff], this.blockSize - 2));
				this.FR[this.blockSize - 1] = (outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], this.blockSize - 1));
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int i = 2; i < this.blockSize; i++)
				{
					this.FR[i - 2] = (outBytes[outOff + i] = this.EncryptByte(input[inOff + i], i - 2));
				}
			}
			else if (this.count == 0)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int j = 0; j < this.blockSize; j++)
				{
					this.FR[j] = (outBytes[outOff + j] = this.EncryptByte(input[inOff + j], j));
				}
				this.count += this.blockSize;
			}
			else if (this.count == this.blockSize)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				outBytes[outOff] = this.EncryptByte(input[inOff], 0);
				outBytes[outOff + 1] = this.EncryptByte(input[inOff + 1], 1);
				Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
				Array.Copy(outBytes, outOff, this.FR, this.blockSize - 2, 2);
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int k = 2; k < this.blockSize; k++)
				{
					this.FR[k - 2] = (outBytes[outOff + k] = this.EncryptByte(input[inOff + k], k - 2));
				}
				this.count += this.blockSize;
			}
			return this.blockSize;
		}

		// Token: 0x060028F2 RID: 10482 RVA: 0x000CA8B8 File Offset: 0x000C8AB8
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			if (inOff + this.blockSize > input.Length)
			{
				throw new DataLengthException("input buffer too short");
			}
			if (outOff + this.blockSize > outBytes.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			if (this.count > this.blockSize)
			{
				byte b = input[inOff];
				this.FR[this.blockSize - 2] = b;
				outBytes[outOff] = this.EncryptByte(b, this.blockSize - 2);
				b = input[inOff + 1];
				this.FR[this.blockSize - 1] = b;
				outBytes[outOff + 1] = this.EncryptByte(b, this.blockSize - 1);
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int i = 2; i < this.blockSize; i++)
				{
					b = input[inOff + i];
					this.FR[i - 2] = b;
					outBytes[outOff + i] = this.EncryptByte(b, i - 2);
				}
			}
			else if (this.count == 0)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int j = 0; j < this.blockSize; j++)
				{
					this.FR[j] = input[inOff + j];
					outBytes[j] = this.EncryptByte(input[inOff + j], j);
				}
				this.count += this.blockSize;
			}
			else if (this.count == this.blockSize)
			{
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				byte b2 = input[inOff];
				byte b3 = input[inOff + 1];
				outBytes[outOff] = this.EncryptByte(b2, 0);
				outBytes[outOff + 1] = this.EncryptByte(b3, 1);
				Array.Copy(this.FR, 2, this.FR, 0, this.blockSize - 2);
				this.FR[this.blockSize - 2] = b2;
				this.FR[this.blockSize - 1] = b3;
				this.cipher.ProcessBlock(this.FR, 0, this.FRE, 0);
				for (int k = 2; k < this.blockSize; k++)
				{
					byte b4 = input[inOff + k];
					this.FR[k - 2] = b4;
					outBytes[outOff + k] = this.EncryptByte(b4, k - 2);
				}
				this.count += this.blockSize;
			}
			return this.blockSize;
		}

		// Token: 0x04001AEA RID: 6890
		private byte[] IV;

		// Token: 0x04001AEB RID: 6891
		private byte[] FR;

		// Token: 0x04001AEC RID: 6892
		private byte[] FRE;

		// Token: 0x04001AED RID: 6893
		private readonly IBlockCipher cipher;

		// Token: 0x04001AEE RID: 6894
		private readonly int blockSize;

		// Token: 0x04001AEF RID: 6895
		private int count;

		// Token: 0x04001AF0 RID: 6896
		private bool forEncryption;
	}
}
