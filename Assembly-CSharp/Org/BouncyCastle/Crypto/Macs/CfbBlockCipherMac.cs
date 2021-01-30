using System;
using Org.BouncyCastle.Crypto.Paddings;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046D RID: 1133
	public class CfbBlockCipherMac : IMac
	{
		// Token: 0x06002944 RID: 10564 RVA: 0x000CBF6C File Offset: 0x000CA16C
		public CfbBlockCipherMac(IBlockCipher cipher) : this(cipher, 8, cipher.GetBlockSize() * 8 / 2, null)
		{
		}

		// Token: 0x06002945 RID: 10565 RVA: 0x000CBF81 File Offset: 0x000CA181
		public CfbBlockCipherMac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, 8, cipher.GetBlockSize() * 8 / 2, padding)
		{
		}

		// Token: 0x06002946 RID: 10566 RVA: 0x000CBF96 File Offset: 0x000CA196
		public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits) : this(cipher, cfbBitSize, macSizeInBits, null)
		{
		}

		// Token: 0x06002947 RID: 10567 RVA: 0x000CBFA4 File Offset: 0x000CA1A4
		public CfbBlockCipherMac(IBlockCipher cipher, int cfbBitSize, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			this.mac = new byte[cipher.GetBlockSize()];
			this.cipher = new MacCFBBlockCipher(cipher, cfbBitSize);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.Buffer = new byte[this.cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170004D2 RID: 1234
		// (get) Token: 0x06002948 RID: 10568 RVA: 0x000CC013 File Offset: 0x000CA213
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06002949 RID: 10569 RVA: 0x000CC020 File Offset: 0x000CA220
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x0600294A RID: 10570 RVA: 0x000CC035 File Offset: 0x000CA235
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600294B RID: 10571 RVA: 0x000CC040 File Offset: 0x000CA240
		public void Update(byte input)
		{
			if (this.bufOff == this.Buffer.Length)
			{
				this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] buffer = this.Buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			buffer[num] = input;
		}

		// Token: 0x0600294C RID: 10572 RVA: 0x000CC098 File Offset: 0x000CA298
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = 0;
			int num2 = blockSize - this.bufOff;
			if (len > num2)
			{
				Array.Copy(input, inOff, this.Buffer, this.bufOff, num2);
				num += this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num2;
				inOff += num2;
				while (len > blockSize)
				{
					num += this.cipher.ProcessBlock(input, inOff, this.mac, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(input, inOff, this.Buffer, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600294D RID: 10573 RVA: 0x000CC15C File Offset: 0x000CA35C
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] buffer = this.Buffer;
					int num = this.bufOff;
					this.bufOff = num + 1;
					buffer[num] = 0;
				}
			}
			else
			{
				this.padding.AddPadding(this.Buffer, this.bufOff);
			}
			this.cipher.ProcessBlock(this.Buffer, 0, this.mac, 0);
			this.cipher.GetMacBlock(this.mac);
			Array.Copy(this.mac, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x0600294E RID: 10574 RVA: 0x000CC206 File Offset: 0x000CA406
		public void Reset()
		{
			Array.Clear(this.Buffer, 0, this.Buffer.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x04001B11 RID: 6929
		private byte[] mac;

		// Token: 0x04001B12 RID: 6930
		private byte[] Buffer;

		// Token: 0x04001B13 RID: 6931
		private int bufOff;

		// Token: 0x04001B14 RID: 6932
		private MacCFBBlockCipher cipher;

		// Token: 0x04001B15 RID: 6933
		private IBlockCipherPadding padding;

		// Token: 0x04001B16 RID: 6934
		private int macSize;
	}
}
