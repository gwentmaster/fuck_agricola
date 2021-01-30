using System;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000369 RID: 873
	public class BufferedAeadBlockCipher : BufferedCipherBase
	{
		// Token: 0x0600216F RID: 8559 RVA: 0x000B451B File Offset: 0x000B271B
		public BufferedAeadBlockCipher(IAeadBlockCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			this.cipher = cipher;
		}

		// Token: 0x170003B5 RID: 949
		// (get) Token: 0x06002170 RID: 8560 RVA: 0x000B4538 File Offset: 0x000B2738
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06002171 RID: 8561 RVA: 0x000B4545 File Offset: 0x000B2745
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.cipher.Init(forEncryption, parameters);
		}

		// Token: 0x06002172 RID: 8562 RVA: 0x000B4569 File Offset: 0x000B2769
		public override int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x06002173 RID: 8563 RVA: 0x000B4576 File Offset: 0x000B2776
		public override int GetUpdateOutputSize(int length)
		{
			return this.cipher.GetUpdateOutputSize(length);
		}

		// Token: 0x06002174 RID: 8564 RVA: 0x000B4584 File Offset: 0x000B2784
		public override int GetOutputSize(int length)
		{
			return this.cipher.GetOutputSize(length);
		}

		// Token: 0x06002175 RID: 8565 RVA: 0x000B4592 File Offset: 0x000B2792
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			return this.cipher.ProcessByte(input, output, outOff);
		}

		// Token: 0x06002176 RID: 8566 RVA: 0x000B45A4 File Offset: 0x000B27A4
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

		// Token: 0x06002177 RID: 8567 RVA: 0x000B45F0 File Offset: 0x000B27F0
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

		// Token: 0x06002178 RID: 8568 RVA: 0x000B464F File Offset: 0x000B284F
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			return this.cipher.ProcessBytes(input, inOff, length, output, outOff);
		}

		// Token: 0x06002179 RID: 8569 RVA: 0x000B4664 File Offset: 0x000B2864
		public override byte[] DoFinal()
		{
			byte[] array = new byte[this.GetOutputSize(0)];
			int num = this.DoFinal(array, 0);
			if (num < array.Length)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x0600217A RID: 8570 RVA: 0x000B46A4 File Offset: 0x000B28A4
		public override byte[] DoFinal(byte[] input, int inOff, int inLen)
		{
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			byte[] array = new byte[this.GetOutputSize(inLen)];
			int num = (inLen > 0) ? this.ProcessBytes(input, inOff, inLen, array, 0) : 0;
			num += this.DoFinal(array, num);
			if (num < array.Length)
			{
				byte[] array2 = new byte[num];
				Array.Copy(array, 0, array2, 0, num);
				array = array2;
			}
			return array;
		}

		// Token: 0x0600217B RID: 8571 RVA: 0x000B4704 File Offset: 0x000B2904
		public override int DoFinal(byte[] output, int outOff)
		{
			return this.cipher.DoFinal(output, outOff);
		}

		// Token: 0x0600217C RID: 8572 RVA: 0x000B4713 File Offset: 0x000B2913
		public override void Reset()
		{
			this.cipher.Reset();
		}

		// Token: 0x0400167B RID: 5755
		private readonly IAeadBlockCipher cipher;
	}
}
