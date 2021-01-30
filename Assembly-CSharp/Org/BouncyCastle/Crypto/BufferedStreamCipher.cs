using System;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200036E RID: 878
	public class BufferedStreamCipher : BufferedCipherBase
	{
		// Token: 0x060021B7 RID: 8631 RVA: 0x000B4F6E File Offset: 0x000B316E
		public BufferedStreamCipher(IStreamCipher cipher)
		{
			if (cipher == null)
			{
				throw new ArgumentNullException("cipher");
			}
			this.cipher = cipher;
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060021B8 RID: 8632 RVA: 0x000B4F8B File Offset: 0x000B318B
		public override string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName;
			}
		}

		// Token: 0x060021B9 RID: 8633 RVA: 0x000B4F98 File Offset: 0x000B3198
		public override void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (parameters is ParametersWithRandom)
			{
				parameters = ((ParametersWithRandom)parameters).Parameters;
			}
			this.cipher.Init(forEncryption, parameters);
		}

		// Token: 0x060021BA RID: 8634 RVA: 0x0002A062 File Offset: 0x00028262
		public override int GetBlockSize()
		{
			return 0;
		}

		// Token: 0x060021BB RID: 8635 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public override int GetOutputSize(int inputLen)
		{
			return inputLen;
		}

		// Token: 0x060021BC RID: 8636 RVA: 0x00055DC8 File Offset: 0x00053FC8
		public override int GetUpdateOutputSize(int inputLen)
		{
			return inputLen;
		}

		// Token: 0x060021BD RID: 8637 RVA: 0x000B4FBC File Offset: 0x000B31BC
		public override byte[] ProcessByte(byte input)
		{
			return new byte[]
			{
				this.cipher.ReturnByte(input)
			};
		}

		// Token: 0x060021BE RID: 8638 RVA: 0x000B4FD3 File Offset: 0x000B31D3
		public override int ProcessByte(byte input, byte[] output, int outOff)
		{
			if (outOff >= output.Length)
			{
				throw new DataLengthException("output buffer too short");
			}
			output[outOff] = this.cipher.ReturnByte(input);
			return 1;
		}

		// Token: 0x060021BF RID: 8639 RVA: 0x000B4FF8 File Offset: 0x000B31F8
		public override byte[] ProcessBytes(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return null;
			}
			byte[] array = new byte[length];
			this.cipher.ProcessBytes(input, inOff, length, array, 0);
			return array;
		}

		// Token: 0x060021C0 RID: 8640 RVA: 0x000B5023 File Offset: 0x000B3223
		public override int ProcessBytes(byte[] input, int inOff, int length, byte[] output, int outOff)
		{
			if (length < 1)
			{
				return 0;
			}
			if (length > 0)
			{
				this.cipher.ProcessBytes(input, inOff, length, output, outOff);
			}
			return length;
		}

		// Token: 0x060021C1 RID: 8641 RVA: 0x000B5042 File Offset: 0x000B3242
		public override byte[] DoFinal()
		{
			this.Reset();
			return BufferedCipherBase.EmptyBuffer;
		}

		// Token: 0x060021C2 RID: 8642 RVA: 0x000B504F File Offset: 0x000B324F
		public override byte[] DoFinal(byte[] input, int inOff, int length)
		{
			if (length < 1)
			{
				return BufferedCipherBase.EmptyBuffer;
			}
			byte[] result = this.ProcessBytes(input, inOff, length);
			this.Reset();
			return result;
		}

		// Token: 0x060021C3 RID: 8643 RVA: 0x000B506A File Offset: 0x000B326A
		public override void Reset()
		{
			this.cipher.Reset();
		}

		// Token: 0x04001687 RID: 5767
		private readonly IStreamCipher cipher;
	}
}
