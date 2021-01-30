using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036B RID: 875
	public class BufferedBlockCipher : BufferedCipherBase
	{
		// Token: 0x06002189 RID: 8585 RVA: 0x000B48A4 File Offset: 0x000B2AA4
		protected BufferedBlockCipher()
		{
		}

		// Token: 0x0600218A RID: 8586 RVA: 0x000B48AC File Offset: 0x000B2AAC
		public BufferedBlockCipher(IBlockCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			this.cipher = cipher;
			this.buf = new byte[cipher.GetBlockSize()];
			this.bufOff = 0;
		}

		// Token: 0x170003B7 RID: 951
		// (get) Token: 0x0600218B RID: 8587 RVA: 0x000B48E1 File Offset: 0x000B2AE1
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x0600218C RID: 8588 RVA: 0x000B48F0 File Offset: 0x000B2AF0
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			ParametersWithRandom parametersWithRandom = parameters as ParametersWithRandom;
			if (parametersWithRandom != null)
			{
				parameters = parametersWithRandom.Parameters;
			}
			this.Reset();
			this.cipher.Init(forEncryption, parameters);
		}

		// Token: 0x0600218D RID: 8589 RVA: 0x000B4929 File Offset: 0x000B2B29
		public override int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x0600218E RID: 8590 RVA: 0x000B4938 File Offset: 0x000B2B38
		public override int GetUpdateOutputSize(int length)
		{
			int num = length + this.bufOff;
			int num2 = num % this.buf.Length;
			return num - num2;
		}

		// Token: 0x0600218F RID: 8591 RVA: 0x000B495A File Offset: 0x000B2B5A
		public override int GetOutputSize(int length)
		{
			return length + this.bufOff;
		}

		// Token: 0x06002190 RID: 8592 RVA: 0x000B4964 File Offset: 0x000B2B64
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			byte[] array = this.buf;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
			if (this.bufOff != this.buf.Length)
			{
				return 0;
			}
			if (outOff + this.buf.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			this.bufOff = 0;
			return this.cipher.ProcessBlock(this.buf, 0, output, outOff);
		}

		// Token: 0x06002191 RID: 8593 RVA: 0x000B49D4 File Offset: 0x000B2BD4
		public override byte[] ProcessByte(byte input)
		{
			int updateOutputSize = this.GetUpdateOutputSize(1);
			byte[] array = (updateOutputSize > 0) ? new byte[updateOutputSize] : null;
			int num = this.ProcessByte(input, array, 0);
			if (updateOutputSize > 0 && num < updateOutputSize)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06002192 RID: 8594 RVA: 0x000B4A20 File Offset: 0x000B2C20
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (length < 1)
			{
				return null;
			}
			int updateOutputSize = this.GetUpdateOutputSize(length);
			byte[] array = (updateOutputSize > 0) ? new byte[updateOutputSize] : null;
			int num = this.ProcessBytes(input, inOff, length, array, 0);
			if (updateOutputSize > 0 && num < updateOutputSize)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x06002193 RID: 8595 RVA: 0x000B4A80 File Offset: 0x000B2C80
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (length >= 1)
			{
				int blockSize = this.GetBlockSize();
				int updateOutputSize = this.GetUpdateOutputSize(length);
				if (updateOutputSize > 0)
				{
					Check.OutputLength(output, outOff, updateOutputSize, "output buffer too short");
				}
				int num = 0;
				int num2 = this.buf.Length - this.bufOff;
				if (length > num2)
				{
					Array.Copy(input, inOff, this.buf, this.bufOff, num2);
					num += this.cipher.ProcessBlock(this.buf, 0, output, outOff);
					this.bufOff = 0;
					length -= num2;
					inOff += num2;
					while (length > this.buf.Length)
					{
						num += this.cipher.ProcessBlock(input, inOff, output, outOff + num);
						length -= blockSize;
						inOff += blockSize;
					}
				}
				Array.Copy(input, inOff, this.buf, this.bufOff, length);
				this.bufOff += length;
				if (this.bufOff == this.buf.Length)
				{
					num += this.cipher.ProcessBlock(this.buf, 0, output, outOff + num);
					this.bufOff = 0;
				}
				return num;
			}
			if (length < 0)
			{
				throw new ArgumentException("Can't have a negative input length!");
			}
			return 0;
		}

		// Token: 0x06002194 RID: 8596 RVA: 0x000B4B9C File Offset: 0x000B2D9C
		public override byte[] DoFinal()
		{
			byte[] array = BufferedCipherBase.EmptyBuffer;
			int outputSize = this.GetOutputSize(0);
			if (outputSize > 0)
			{
				array = new byte[outputSize];
				int num = this.DoFinal(array, 0);
				if (num < array.Length)
				{
					byte[] array2 = new byte[num];
					Array.Copy(array, 0, array2, 0, num);
					array = array2;
				}
			}
			else
			{
				this.Reset();
			}
			return array;
		}

		// Token: 0x06002195 RID: 8597 RVA: 0x000B4BF0 File Offset: 0x000B2DF0
		public override byte[] DoFinal(byte[] input, int inOff, int inLen)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			int outputSize = this.GetOutputSize(inLen);
			byte[] array = BufferedCipherBase.EmptyBuffer;
			if (outputSize > 0)
			{
				array = new byte[outputSize];
				int num = (inLen > 0) ? this.ProcessBytes(input, inOff, inLen, array, 0) : 0;
				num += this.DoFinal(array, num);
				if (num < array.Length)
				{
					byte[] array2 = new byte[num];
					Array.Copy(array, 0, array2, 0, num);
					array = array2;
				}
			}
			else
			{
				this.Reset();
			}
			return array;
		}

		// Token: 0x06002196 RID: 8598 RVA: 0x000B4C64 File Offset: 0x000B2E64
		public override int DoFinal(byte[] output, int outOff)
		{
			int result;
			try
			{
				if (this.bufOff != 0)
				{
					Check.DataLength(!this.cipher.IsPartialBlockOkay, "data not block size aligned");
					Check.OutputLength(output, outOff, this.bufOff, "output buffer too short for DoFinal()");
					this.cipher.ProcessBlock(this.buf, 0, this.buf, 0);
					Array.Copy(this.buf, 0, output, outOff, this.bufOff);
				}
				result = this.bufOff;
			}
			finally
			{
				this.Reset();
			}
			return result;
		}

		// Token: 0x06002197 RID: 8599 RVA: 0x000B4CF4 File Offset: 0x000B2EF4
		public override void Reset()
		{
			Array.Clear(this.buf, 0, this.buf.Length);
			this.bufOff = 0;
			this.cipher.Reset();
		}

		// Token: 0x0400167F RID: 5759
		internal byte[] buf;

		// Token: 0x04001680 RID: 5760
		internal int bufOff;

		// Token: 0x04001681 RID: 5761
		internal bool forEncryption;

		// Token: 0x04001682 RID: 5762
		internal IBlockCipher cipher;
	}
}
