using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036C RID: 876
	public abstract class BufferedCipherBase : IBufferedCipher
	{
		// Token: 0x170003B8 RID: 952
		// (get) Token: 0x06002198 RID: 8600
		public abstract string AlgorithmName { get; }

		// Token: 0x06002199 RID: 8601
		public abstract void Init(bool forEncryption, ICipherParameters parameters);

		// Token: 0x0600219A RID: 8602
		public abstract int GetBlockSize();

		// Token: 0x0600219B RID: 8603
		public abstract int GetOutputSize(int inputLen);

		// Token: 0x0600219C RID: 8604
		public abstract int GetUpdateOutputSize(int inputLen);

		// Token: 0x0600219D RID: 8605
		public abstract byte[] ProcessByte(byte input);

		// Token: 0x0600219E RID: 8606 RVA: 0x000B4D1C File Offset: 0x000B2F1C
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			byte[] array = this.ProcessByte(input);
			if (array == null)
			{
				return 0;
			}
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x0600219F RID: 8607 RVA: 0x000B4D56 File Offset: 0x000B2F56
		public virtual byte[] ProcessBytes(byte[] input)
		{
			return this.ProcessBytes(input, 0, input.Length);
		}

		// Token: 0x060021A0 RID: 8608
		public abstract byte[] ProcessBytes(byte[] input, int inOff, int length);

		// Token: 0x060021A1 RID: 8609 RVA: 0x000B4D63 File Offset: 0x000B2F63
		public virtual int ProcessBytes(byte[] input, byte[] output, int outOff)
		{
			return this.ProcessBytes(input, 0, input.Length, output, outOff);
		}

		// Token: 0x060021A2 RID: 8610 RVA: 0x000B4D74 File Offset: 0x000B2F74
		public virtual int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			byte[] array = this.ProcessBytes(input, inOff, length);
			if (array == null)
			{
				return 0;
			}
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x060021A3 RID: 8611
		public abstract byte[] DoFinal();

		// Token: 0x060021A4 RID: 8612 RVA: 0x000B4DB4 File Offset: 0x000B2FB4
		public virtual byte[] DoFinal(byte[] input)
		{
			return this.DoFinal(input, 0, input.Length);
		}

		// Token: 0x060021A5 RID: 8613
		public abstract byte[] DoFinal(byte[] input, int inOff, int length);

		// Token: 0x060021A6 RID: 8614 RVA: 0x000B4DC4 File Offset: 0x000B2FC4
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] array = this.DoFinal();
			if (outOff + array.Length > output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			array.CopyTo(output, outOff);
			return array.Length;
		}

		// Token: 0x060021A7 RID: 8615 RVA: 0x000B4DF8 File Offset: 0x000B2FF8
		public virtual int DoFinal(byte[] input, byte[] output, int outOff)
		{
			return this.DoFinal(input, 0, input.Length, output, outOff);
		}

		// Token: 0x060021A8 RID: 8616 RVA: 0x000B4E08 File Offset: 0x000B3008
		public virtual int DoFinal(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			int num = this.ProcessBytes(input, inOff, length, output, outOff);
			return num + this.DoFinal(output, outOff + num);
		}

		// Token: 0x060021A9 RID: 8617
		public abstract void Reset();

		// Token: 0x04001683 RID: 5763
		protected static readonly byte[] EmptyBuffer = new byte[0];
	}
}
