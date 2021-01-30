using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000464 RID: 1124
	public class SicBlockCipher : IBlockCipher
	{
		// Token: 0x060028F3 RID: 10483 RVA: 0x000CAB10 File Offset: 0x000C8D10
		public SicBlockCipher(IBlockCipher cipher)
		{
			this.cipher = cipher;
			this.blockSize = cipher.GetBlockSize();
			this.counter = new byte[this.blockSize];
			this.counterOut = new byte[this.blockSize];
			this.IV = new byte[this.blockSize];
		}

		// Token: 0x060028F4 RID: 10484 RVA: 0x000CAB69 File Offset: 0x000C8D69
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x060028F5 RID: 10485 RVA: 0x000CAB74 File Offset: 0x000C8D74
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			ParametersWithIV parametersWithIV = parameters as ParametersWithIV;
			if (parametersWithIV == null)
			{
				throw new ArgumentException("CTR/SIC mode requires ParametersWithIV", "parameters");
			}
			this.IV = Arrays.Clone(parametersWithIV.GetIV());
			if (this.blockSize < this.IV.Length)
			{
				throw new ArgumentException("CTR/SIC mode requires IV no greater than: " + this.blockSize + " bytes.");
			}
			int num = Math.Min(8, this.blockSize / 2);
			if (this.blockSize - this.IV.Length > num)
			{
				throw new ArgumentException("CTR/SIC mode requires IV of at least: " + (this.blockSize - num) + " bytes.");
			}
			if (parametersWithIV.Parameters != null)
			{
				this.cipher.Init(true, parametersWithIV.Parameters);
			}
			this.Reset();
		}

		// Token: 0x170004CC RID: 1228
		// (get) Token: 0x060028F6 RID: 10486 RVA: 0x000CAC3F File Offset: 0x000C8E3F
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/SIC";
			}
		}

		// Token: 0x170004CD RID: 1229
		// (get) Token: 0x060028F7 RID: 10487 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060028F8 RID: 10488 RVA: 0x000CAC56 File Offset: 0x000C8E56
		public virtual int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x060028F9 RID: 10489 RVA: 0x000CAC64 File Offset: 0x000C8E64
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			this.cipher.ProcessBlock(this.counter, 0, this.counterOut, 0);
			for (int i = 0; i < this.counterOut.Length; i++)
			{
				output[outOff + i] = (this.counterOut[i] ^ input[inOff + i]);
			}
			int num = this.counter.Length;
			while (--num >= 0)
			{
				byte[] array = this.counter;
				int num2 = num;
				byte b = array[num2] + 1;
				array[num2] = b;
				if (b != 0)
				{
					break;
				}
			}
			return this.counter.Length;
		}

		// Token: 0x060028FA RID: 10490 RVA: 0x000CACE3 File Offset: 0x000C8EE3
		public virtual void Reset()
		{
			Arrays.Fill(this.counter, 0);
			Array.Copy(this.IV, 0, this.counter, 0, this.IV.Length);
			this.cipher.Reset();
		}

		// Token: 0x04001AF1 RID: 6897
		private readonly IBlockCipher cipher;

		// Token: 0x04001AF2 RID: 6898
		private readonly int blockSize;

		// Token: 0x04001AF3 RID: 6899
		private readonly byte[] counter;

		// Token: 0x04001AF4 RID: 6900
		private readonly byte[] counterOut;

		// Token: 0x04001AF5 RID: 6901
		private byte[] IV;
	}
}
