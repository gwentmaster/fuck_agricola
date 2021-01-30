using System;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046B RID: 1131
	public class CbcBlockCipherMac : IMac
	{
		// Token: 0x06002931 RID: 10545 RVA: 0x000CBAAA File Offset: 0x000C9CAA
		public CbcBlockCipherMac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8 / 2, null)
		{
		}

		// Token: 0x06002932 RID: 10546 RVA: 0x000CBABE File Offset: 0x000C9CBE
		public CbcBlockCipherMac(IBlockCipher cipher, IBlockCipherPadding padding) : this(cipher, cipher.GetBlockSize() * 8 / 2, padding)
		{
		}

		// Token: 0x06002933 RID: 10547 RVA: 0x000CBAD2 File Offset: 0x000C9CD2
		public CbcBlockCipherMac(IBlockCipher cipher, int macSizeInBits) : this(cipher, macSizeInBits, null)
		{
		}

		// Token: 0x06002934 RID: 10548 RVA: 0x000CBAE0 File Offset: 0x000C9CE0
		public CbcBlockCipherMac(IBlockCipher cipher, int macSizeInBits, IBlockCipherPadding padding)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.padding = padding;
			this.macSize = macSizeInBits / 8;
			this.buf = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170004CF RID: 1231
		// (get) Token: 0x06002935 RID: 10549 RVA: 0x000CBB37 File Offset: 0x000C9D37
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06002936 RID: 10550 RVA: 0x000CBB44 File Offset: 0x000C9D44
		public void Init(ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(true, parameters);
		}

		// Token: 0x06002937 RID: 10551 RVA: 0x000CBB59 File Offset: 0x000C9D59
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x06002938 RID: 10552 RVA: 0x000CBB64 File Offset: 0x000C9D64
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x06002939 RID: 10553 RVA: 0x000CBBBC File Offset: 0x000C9DBC
		public void BlockUpdate(byte[] input, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = blockSize - this.bufOff;
			if (len > num)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num);
				this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
				this.bufOff = 0;
				len -= num;
				inOff += num;
				while (len > blockSize)
				{
					this.cipher.ProcessBlock(input, inOff, this.buf, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600293A RID: 10554 RVA: 0x000CBC78 File Offset: 0x000C9E78
		public int DoFinal(byte[] output, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			if (this.padding == null)
			{
				while (this.bufOff < blockSize)
				{
					byte[] array = this.buf;
					int num = this.bufOff;
					this.bufOff = num + 1;
					array[num] = 0;
				}
			}
			else
			{
				if (this.bufOff == blockSize)
				{
					this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
					this.bufOff = 0;
				}
				this.padding.AddPadding(this.buf, this.bufOff);
			}
			this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
			Array.Copy(this.buf, 0, output, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x0600293B RID: 10555 RVA: 0x000CBD3B File Offset: 0x000C9F3B
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x04001B07 RID: 6919
		private byte[] buf;

		// Token: 0x04001B08 RID: 6920
		private int bufOff;

		// Token: 0x04001B09 RID: 6921
		private IBlockCipher cipher;

		// Token: 0x04001B0A RID: 6922
		private IBlockCipherPadding padding;

		// Token: 0x04001B0B RID: 6923
		private int macSize;
	}
}
