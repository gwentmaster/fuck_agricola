using System;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045C RID: 1116
	public class CtsBlockCipher : BufferedBlockCipher
	{
		// Token: 0x0600287E RID: 10366 RVA: 0x000C7E60 File Offset: 0x000C6060
		public CtsBlockCipher(IBlockCipher cipher)
		{
			if (cipher is OfbBlockCipher || cipher is CfbBlockCipher)
			{
				throw new ArgumentException("CtsBlockCipher can only accept ECB, or CBC ciphers");
			}
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.buf = new byte[this.blockSize * 2];
			this.bufOff = 0;
		}

		// Token: 0x0600287F RID: 10367 RVA: 0x000C7EBC File Offset: 0x000C60BC
		public override int GetUpdateOutputSize(int length)
		{
			int num = length + this.bufOff;
			int num2 = num % this.buf.Length;
			if (num2 == 0)
			{
				return num - this.buf.Length;
			}
			return num - num2;
		}

		// Token: 0x06002880 RID: 10368 RVA: 0x000B495A File Offset: 0x000B2B5A
		public override int GetOutputSize(int length)
		{
			return length + this.bufOff;
		}

		// Token: 0x06002881 RID: 10369 RVA: 0x000C7EF0 File Offset: 0x000C60F0
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			int result = 0;
			if (this.bufOff == this.buf.Length)
			{
				result = this.cipher.ProcessBlock(this.buf, 0, output, outOff);
				Array.Copy(this.buf, this.blockSize, this.buf, 0, this.blockSize);
				this.bufOff = this.blockSize;
			}
			byte[] buf = this.buf;
			int bufOff = this.bufOff;
			this.bufOff = bufOff + 1;
			buf[bufOff] = input;
			return result;
		}

		// Token: 0x06002882 RID: 10370 RVA: 0x000C7F68 File Offset: 0x000C6168
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (length < 0)
			{
				throw new ArgumentException("Can't have a negative input outLength!");
			}
			int num = this.GetBlockSize();
			int updateOutputSize = this.GetUpdateOutputSize(length);
			if (updateOutputSize > 0 && outOff + updateOutputSize > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			int num2 = 0;
			int num3 = this.buf.Length - this.bufOff;
			if (length > num3)
			{
				Array.Copy(input, inOff, this.buf, this.bufOff, num3);
				num2 += this.cipher.ProcessBlock(this.buf, 0, output, outOff);
				Array.Copy(this.buf, num, this.buf, 0, num);
				this.bufOff = num;
				length -= num3;
				inOff += num3;
				while (length > num)
				{
					Array.Copy(input, inOff, this.buf, this.bufOff, num);
					num2 += this.cipher.ProcessBlock(this.buf, 0, output, outOff + num2);
					Array.Copy(this.buf, num, this.buf, 0, num);
					length -= num;
					inOff += num;
				}
			}
			Array.Copy(input, inOff, this.buf, this.bufOff, length);
			this.bufOff += length;
			return num2;
		}

		// Token: 0x06002883 RID: 10371 RVA: 0x000C8090 File Offset: 0x000C6290
		public override int DoFinal(byte[] output, int outOff)
		{
			if (this.bufOff + outOff > output.Length)
			{
				throw new DataLengthException("output buffer too small in doFinal");
			}
			int num = this.cipher.GetBlockSize();
			int length = this.bufOff - num;
			byte[] array = new byte[num];
			if (this.forEncryption)
			{
				this.cipher.ProcessBlock(this.buf, 0, array, 0);
				if (this.bufOff < num)
				{
					throw new DataLengthException("need at least one block of input for CTS");
				}
				for (int num2 = this.bufOff; num2 != this.buf.Length; num2++)
				{
					this.buf[num2] = array[num2 - num];
				}
				for (int num3 = num; num3 != this.bufOff; num3++)
				{
					byte[] buf = this.buf;
					int num4 = num3;
					buf[num4] ^= array[num3 - num];
				}
				((this.cipher is CbcBlockCipher) ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock(this.buf, num, output, outOff);
				Array.Copy(array, 0, output, outOff + num, length);
			}
			else
			{
				byte[] array2 = new byte[num];
				((this.cipher is CbcBlockCipher) ? ((CbcBlockCipher)this.cipher).GetUnderlyingCipher() : this.cipher).ProcessBlock(this.buf, 0, array, 0);
				for (int num5 = num; num5 != this.bufOff; num5++)
				{
					array2[num5 - num] = (array[num5 - num] ^ this.buf[num5]);
				}
				Array.Copy(this.buf, num, array, 0, length);
				this.cipher.ProcessBlock(array, 0, output, outOff);
				Array.Copy(array2, 0, output, outOff + num, length);
			}
			int bufOff = this.bufOff;
			this.Reset();
			return bufOff;
		}

		// Token: 0x04001AA0 RID: 6816
		private readonly int blockSize;
	}
}
