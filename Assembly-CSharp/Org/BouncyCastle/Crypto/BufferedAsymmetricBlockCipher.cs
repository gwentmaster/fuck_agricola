using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036A RID: 874
	public class BufferedAsymmetricBlockCipher : BufferedCipherBase
	{
		// Token: 0x0600217D RID: 8573 RVA: 0x000B4720 File Offset: 0x000B2920
		public BufferedAsymmetricBlockCipher(IAsymmetricBlockCipher cipher)
		{
			this.cipher = cipher;
		}

		// Token: 0x0600217E RID: 8574 RVA: 0x000B472F File Offset: 0x000B292F
		internal int GetBufferPosition()
		{
			return this.bufOff;
		}

		// Token: 0x170003B6 RID: 950
		// (get) Token: 0x0600217F RID: 8575 RVA: 0x000B4737 File Offset: 0x000B2937
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x06002180 RID: 8576 RVA: 0x000B4744 File Offset: 0x000B2944
		public override int GetBlockSize()
		{
			return this.cipher.GetInputBlockSize();
		}

		// Token: 0x06002181 RID: 8577 RVA: 0x000B4751 File Offset: 0x000B2951
		public override int GetOutputSize(int length)
		{
			return this.cipher.GetOutputBlockSize();
		}

		// Token: 0x06002182 RID: 8578 RVA: 0x0002A062 File Offset: 0x00028262
		public override int GetUpdateOutputSize(int length)
		{
			return 0;
		}

		// Token: 0x06002183 RID: 8579 RVA: 0x000B475E File Offset: 0x000B295E
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.Reset();
			this.cipher.Init(forEncryption, parameters);
			this.buffer = new byte[this.cipher.GetInputBlockSize() + (forEncryption ? 1 : 0)];
			this.bufOff = 0;
		}

		// Token: 0x06002184 RID: 8580 RVA: 0x000B4798 File Offset: 0x000B2998
		public override byte[] ProcessByte(byte input)
		{
			if (this.bufOff >= this.buffer.Length)
			{
				throw new DataLengthException("attempt to process message to long for cipher");
			}
			byte[] array = this.buffer;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = input;
			return null;
		}

		// Token: 0x06002185 RID: 8581 RVA: 0x000B47DC File Offset: 0x000B29DC
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return null;
			}
			if (input == null)
			{
				throw new ArgumentNullException("input");
			}
			if (this.bufOff + length > this.buffer.Length)
			{
				throw new DataLengthException("attempt to process message to long for cipher");
			}
			Array.Copy(input, inOff, this.buffer, this.bufOff, length);
			this.bufOff += length;
			return null;
		}

		// Token: 0x06002186 RID: 8582 RVA: 0x000B483D File Offset: 0x000B2A3D
		public override byte[] DoFinal()
		{
			byte[] result = (this.bufOff > 0) ? this.cipher.ProcessBlock(this.buffer, 0, this.bufOff) : BufferedCipherBase.EmptyBuffer;
			this.Reset();
			return result;
		}

		// Token: 0x06002187 RID: 8583 RVA: 0x000B486D File Offset: 0x000B2A6D
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			this.ProcessBytes(input, inOff, length);
			return this.DoFinal();
		}

		// Token: 0x06002188 RID: 8584 RVA: 0x000B487F File Offset: 0x000B2A7F
		public override void Reset()
		{
			if (this.buffer != null)
			{
				Array.Clear(this.buffer, 0, this.buffer.Length);
				this.bufOff = 0;
			}
		}

		// Token: 0x0400167C RID: 5756
		private readonly IAsymmetricBlockCipher cipher;

		// Token: 0x0400167D RID: 5757
		private byte[] buffer;

		// Token: 0x0400167E RID: 5758
		private int bufOff;
	}
}
