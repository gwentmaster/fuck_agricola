using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045F RID: 1119
	public class GOfbBlockCipher : IBlockCipher
	{
		// Token: 0x060028AF RID: 10415 RVA: 0x000C93DC File Offset: 0x000C75DC
		public GOfbBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			if (this.blockSize != 8)
			{
				throw new ArgumentException("GCTR only for 64 bit block ciphers");
			}
			this.IV = new byte[cipher.GetBlockSize()];
			this.ofbV = new byte[cipher.GetBlockSize()];
			this.ofbOutV = new byte[cipher.GetBlockSize()];
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000C9450 File Offset: 0x000C7650
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000C9458 File Offset: 0x000C7658
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.firstStep = true;
			this.N3 = 0;
			this.N4 = 0;
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

		// Token: 0x170004C4 RID: 1220
		// (get) Token: 0x060028B2 RID: 10418 RVA: 0x000C950F File Offset: 0x000C770F
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/GCTR";
			}
		}

		// Token: 0x170004C5 RID: 1221
		// (get) Token: 0x060028B3 RID: 10419 RVA: 0x0000900B File Offset: 0x0000720B
		public bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000C9526 File Offset: 0x000C7726
		public int GetBlockSize()
		{
			return this.blockSize;
		}

		// Token: 0x060028B5 RID: 10421 RVA: 0x000C9530 File Offset: 0x000C7730
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
			if (this.firstStep)
			{
				this.firstStep = false;
				this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
				this.N3 = this.bytesToint(this.ofbOutV, 0);
				this.N4 = this.bytesToint(this.ofbOutV, 4);
			}
			this.N3 += 16843009;
			this.N4 += 16843012;
			this.intTobytes(this.N3, this.ofbV, 0);
			this.intTobytes(this.N4, this.ofbV, 4);
			this.cipher.ProcessBlock(this.ofbV, 0, this.ofbOutV, 0);
			for (int i = 0; i < this.blockSize; i++)
			{
				output[outOff + i] = (this.ofbOutV[i] ^ input[inOff + i]);
			}
			Array.Copy(this.ofbV, this.blockSize, this.ofbV, 0, this.ofbV.Length - this.blockSize);
			Array.Copy(this.ofbOutV, 0, this.ofbV, this.ofbV.Length - this.blockSize, this.blockSize);
			return this.blockSize;
		}

		// Token: 0x060028B6 RID: 10422 RVA: 0x000C969B File Offset: 0x000C789B
		public void Reset()
		{
			Array.Copy(this.IV, 0, this.ofbV, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x060028B7 RID: 10423 RVA: 0x000C96C3 File Offset: 0x000C78C3
		private int bytesToint(byte[] inBytes, int inOff)
		{
			return (int)((long)((long)inBytes[inOff + 3] << 24) & (long)((ulong)-16777216)) + ((int)inBytes[inOff + 2] << 16 & 16711680) + ((int)inBytes[inOff + 1] << 8 & 65280) + (int)(inBytes[inOff] & byte.MaxValue);
		}

		// Token: 0x060028B8 RID: 10424 RVA: 0x000C96FD File Offset: 0x000C78FD
		private void intTobytes(int num, byte[] outBytes, int outOff)
		{
			outBytes[outOff + 3] = (byte)(num >> 24);
			outBytes[outOff + 2] = (byte)(num >> 16);
			outBytes[outOff + 1] = (byte)(num >> 8);
			outBytes[outOff] = (byte)num;
		}

		// Token: 0x04001AC4 RID: 6852
		private byte[] IV;

		// Token: 0x04001AC5 RID: 6853
		private byte[] ofbV;

		// Token: 0x04001AC6 RID: 6854
		private byte[] ofbOutV;

		// Token: 0x04001AC7 RID: 6855
		private readonly int blockSize;

		// Token: 0x04001AC8 RID: 6856
		private readonly IBlockCipher cipher;

		// Token: 0x04001AC9 RID: 6857
		private bool firstStep = true;

		// Token: 0x04001ACA RID: 6858
		private int N3;

		// Token: 0x04001ACB RID: 6859
		private int N4;

		// Token: 0x04001ACC RID: 6860
		private const int C1 = 16843012;

		// Token: 0x04001ACD RID: 6861
		private const int C2 = 16843009;
	}
}
