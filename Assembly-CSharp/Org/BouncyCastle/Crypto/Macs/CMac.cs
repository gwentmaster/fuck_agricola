using System;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Macs
{
	// Token: 0x0200046A RID: 1130
	public class CMac : IMac
	{
		// Token: 0x06002926 RID: 10534 RVA: 0x000CB6D6 File Offset: 0x000C98D6
		public CMac(IBlockCipher cipher) : this(cipher, cipher.GetBlockSize() * 8)
		{
		}

		// Token: 0x06002927 RID: 10535 RVA: 0x000CB6E8 File Offset: 0x000C98E8
		public CMac(IBlockCipher cipher, int macSizeInBits)
		{
			if (macSizeInBits % 8 != 0)
			{
				throw new ArgumentException("MAC size must be multiple of 8");
			}
			if (macSizeInBits > cipher.GetBlockSize() * 8)
			{
				throw new ArgumentException("MAC size must be less or equal to " + cipher.GetBlockSize() * 8);
			}
			if (cipher.GetBlockSize() != 8 && cipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("Block size must be either 64 or 128 bits");
			}
			this.cipher = new CbcBlockCipher(cipher);
			this.macSize = macSizeInBits / 8;
			this.mac = new byte[cipher.GetBlockSize()];
			this.buf = new byte[cipher.GetBlockSize()];
			this.ZEROES = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170004CE RID: 1230
		// (get) Token: 0x06002928 RID: 10536 RVA: 0x000CB7A0 File Offset: 0x000C99A0
		public string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06002929 RID: 10537 RVA: 0x000CB7B0 File Offset: 0x000C99B0
		private static int ShiftLeft(byte[] block, byte[] output)
		{
			int num = block.Length;
			uint num2 = 0U;
			while (--num >= 0)
			{
				uint num3 = (uint)block[num];
				output[num] = (byte)(num3 << 1 | num2);
				num2 = (num3 >> 7 & 1U);
			}
			return (int)num2;
		}

		// Token: 0x0600292A RID: 10538 RVA: 0x000CB7E4 File Offset: 0x000C99E4
		private static byte[] DoubleLu(byte[] input)
		{
			byte[] array = new byte[input.Length];
			int num = CMac.ShiftLeft(input, array);
			int num2 = (input.Length == 16) ? 135 : 27;
			byte[] array2 = array;
			int num3 = input.Length - 1;
			array2[num3] ^= (byte)(num2 >> (1 - num << 3));
			return array;
		}

		// Token: 0x0600292B RID: 10539 RVA: 0x000CB830 File Offset: 0x000C9A30
		public void Init(ICipherParameters parameters)
		{
			if (parameters is KeyParameter)
			{
				this.cipher.Init(true, parameters);
				this.L = new byte[this.ZEROES.Length];
				this.cipher.ProcessBlock(this.ZEROES, 0, this.L, 0);
				this.Lu = CMac.DoubleLu(this.L);
				this.Lu2 = CMac.DoubleLu(this.Lu);
			}
			else if (parameters != null)
			{
				throw new ArgumentException("CMac mode only permits key to be set.", "parameters");
			}
			this.Reset();
		}

		// Token: 0x0600292C RID: 10540 RVA: 0x000CB8BC File Offset: 0x000C9ABC
		public int GetMacSize()
		{
			return this.macSize;
		}

		// Token: 0x0600292D RID: 10541 RVA: 0x000CB8C4 File Offset: 0x000C9AC4
		public void Update(byte input)
		{
			if (this.bufOff == this.buf.Length)
			{
				this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
			}
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
		}

		// Token: 0x0600292E RID: 10542 RVA: 0x000CB91C File Offset: 0x000C9B1C
		public void BlockUpdate(byte[] inBytes, int inOff, int len)
		{
			if (len < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			int blockSize = this.cipher.GetBlockSize();
			int num = blockSize - this.bufOff;
			if (len > num)
			{
				Array.Copy(inBytes, inOff, this.buf, this.bufOff, num);
				this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
				this.bufOff = 0;
				len -= num;
				inOff += num;
				while (len > blockSize)
				{
					this.cipher.ProcessBlock(inBytes, inOff, this.mac, 0);
					len -= blockSize;
					inOff += blockSize;
				}
			}
			Array.Copy(inBytes, inOff, this.buf, this.bufOff, len);
			this.bufOff += len;
		}

		// Token: 0x0600292F RID: 10543 RVA: 0x000CB9D8 File Offset: 0x000C9BD8
		public int DoFinal(byte[] outBytes, int outOff)
		{
			int blockSize = this.cipher.GetBlockSize();
			byte[] array;
			if (this.bufOff == blockSize)
			{
				array = this.Lu;
			}
			else
			{
				new ISO7816d4Padding().AddPadding(this.buf, this.bufOff);
				array = this.Lu2;
			}
			for (int i = 0; i < this.mac.Length; i++)
			{
				byte[] array2 = this.buf;
				int num = i;
				array2[num] ^= array[i];
			}
			this.cipher.ProcessBlock(this.buf, 0, this.mac, 0);
			Array.Copy(this.mac, 0, outBytes, outOff, this.macSize);
			this.Reset();
			return this.macSize;
		}

		// Token: 0x06002930 RID: 10544 RVA: 0x000CBA82 File Offset: 0x000C9C82
		public void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x04001AFC RID: 6908
		private const byte CONSTANT_128 = 135;

		// Token: 0x04001AFD RID: 6909
		private const byte CONSTANT_64 = 27;

		// Token: 0x04001AFE RID: 6910
		private byte[] ZEROES;

		// Token: 0x04001AFF RID: 6911
		private byte[] mac;

		// Token: 0x04001B00 RID: 6912
		private byte[] buf;

		// Token: 0x04001B01 RID: 6913
		private int bufOff;

		// Token: 0x04001B02 RID: 6914
		private IBlockCipher cipher;

		// Token: 0x04001B03 RID: 6915
		private int macSize;

		// Token: 0x04001B04 RID: 6916
		private byte[] L;

		// Token: 0x04001B05 RID: 6917
		private byte[] Lu;

		// Token: 0x04001B06 RID: 6918
		private byte[] Lu2;
	}
}
