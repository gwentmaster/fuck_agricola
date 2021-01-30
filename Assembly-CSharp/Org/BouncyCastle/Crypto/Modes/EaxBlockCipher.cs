using System;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045D RID: 1117
	public class EaxBlockCipher : IAeadBlockCipher
	{
		// Token: 0x06002884 RID: 10372 RVA: 0x000C8240 File Offset: 0x000C6440
		public EaxBlockCipher(IBlockCipher cipher)
		{
			this.blockSize = cipher.GetBlockSize();
			this.mac = new CMac(cipher);
			this.macBlock = new byte[this.blockSize];
			this.associatedTextMac = new byte[this.mac.GetMacSize()];
			this.nonceMac = new byte[this.mac.GetMacSize()];
			this.cipher = new SicBlockCipher(cipher);
		}

		// Token: 0x170004C2 RID: 1218
		// (get) Token: 0x06002885 RID: 10373 RVA: 0x000C82B4 File Offset: 0x000C64B4
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.GetUnderlyingCipher().AlgorithmName + "/EAX";
			}
		}

		// Token: 0x06002886 RID: 10374 RVA: 0x000C82D0 File Offset: 0x000C64D0
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x06002887 RID: 10375 RVA: 0x000C82D8 File Offset: 0x000C64D8
		public virtual int GetBlockSize()
		{
			return this.cipher.GetBlockSize();
		}

		// Token: 0x06002888 RID: 10376 RVA: 0x000C82E8 File Offset: 0x000C64E8
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			byte[] array;
			ICipherParameters parameters2;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				array = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				this.macSize = aeadParameters.MacSize / 8;
				parameters2 = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to EAX");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				array = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = this.mac.GetMacSize() / 2;
				parameters2 = parametersWithIV.Parameters;
			}
			this.bufBlock = new byte[forEncryption ? this.blockSize : (this.blockSize + this.macSize)];
			byte[] array2 = new byte[this.blockSize];
			this.mac.Init(parameters2);
			array2[this.blockSize - 1] = 0;
			this.mac.BlockUpdate(array2, 0, this.blockSize);
			this.mac.BlockUpdate(array, 0, array.Length);
			this.mac.DoFinal(this.nonceMac, 0);
			this.cipher.Init(true, new ParametersWithIV(null, this.nonceMac));
			this.Reset();
		}

		// Token: 0x06002889 RID: 10377 RVA: 0x000C8414 File Offset: 0x000C6614
		private void InitCipher()
		{
			if (this.cipherInitialized)
			{
				return;
			}
			this.cipherInitialized = true;
			this.mac.DoFinal(this.associatedTextMac, 0);
			byte[] array = new byte[this.blockSize];
			array[this.blockSize - 1] = 2;
			this.mac.BlockUpdate(array, 0, this.blockSize);
		}

		// Token: 0x0600288A RID: 10378 RVA: 0x000C8470 File Offset: 0x000C6670
		private void CalculateMac()
		{
			byte[] array = new byte[this.blockSize];
			this.mac.DoFinal(array, 0);
			for (int i = 0; i < this.macBlock.Length; i++)
			{
				this.macBlock[i] = (this.nonceMac[i] ^ this.associatedTextMac[i] ^ array[i]);
			}
		}

		// Token: 0x0600288B RID: 10379 RVA: 0x000C84C8 File Offset: 0x000C66C8
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x0600288C RID: 10380 RVA: 0x000C84D4 File Offset: 0x000C66D4
		private void Reset(bool clearMac)
		{
			this.cipher.Reset();
			this.mac.Reset();
			this.bufOff = 0;
			Array.Clear(this.bufBlock, 0, this.bufBlock.Length);
			if (clearMac)
			{
				Array.Clear(this.macBlock, 0, this.macBlock.Length);
			}
			byte[] array = new byte[this.blockSize];
			array[this.blockSize - 1] = 1;
			this.mac.BlockUpdate(array, 0, this.blockSize);
			this.cipherInitialized = false;
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x000C8577 File Offset: 0x000C6777
		public virtual void ProcessAadByte(byte input)
		{
			if (this.cipherInitialized)
			{
				throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
			}
			this.mac.Update(input);
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x000C8598 File Offset: 0x000C6798
		public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
		{
			if (this.cipherInitialized)
			{
				throw new InvalidOperationException("AAD data cannot be added after encryption/decryption processing has begun.");
			}
			this.mac.BlockUpdate(inBytes, inOff, len);
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x000C85BB File Offset: 0x000C67BB
		public virtual int ProcessByte(byte input, byte[] outBytes, int outOff)
		{
			this.InitCipher();
			return this.Process(input, outBytes, outOff);
		}

		// Token: 0x06002890 RID: 10384 RVA: 0x000C85CC File Offset: 0x000C67CC
		public virtual int ProcessBytes(byte[] inBytes, int inOff, int len, byte[] outBytes, int outOff)
		{
			this.InitCipher();
			int num = 0;
			for (int num2 = 0; num2 != len; num2++)
			{
				num += this.Process(inBytes[inOff + num2], outBytes, outOff + num);
			}
			return num;
		}

		// Token: 0x06002891 RID: 10385 RVA: 0x000C8604 File Offset: 0x000C6804
		public virtual int DoFinal(byte[] outBytes, int outOff)
		{
			this.InitCipher();
			int num = this.bufOff;
			byte[] array = new byte[this.bufBlock.Length];
			this.bufOff = 0;
			if (this.forEncryption)
			{
				Check.OutputLength(outBytes, outOff, num + this.macSize, "Output buffer too short");
				this.cipher.ProcessBlock(this.bufBlock, 0, array, 0);
				Array.Copy(array, 0, outBytes, outOff, num);
				this.mac.BlockUpdate(array, 0, num);
				this.CalculateMac();
				Array.Copy(this.macBlock, 0, outBytes, outOff + num, this.macSize);
				this.Reset(false);
				return num + this.macSize;
			}
			if (num < this.macSize)
			{
				throw new InvalidCipherTextException("data too short");
			}
			Check.OutputLength(outBytes, outOff, num - this.macSize, "Output buffer too short");
			if (num > this.macSize)
			{
				this.mac.BlockUpdate(this.bufBlock, 0, num - this.macSize);
				this.cipher.ProcessBlock(this.bufBlock, 0, array, 0);
				Array.Copy(array, 0, outBytes, outOff, num - this.macSize);
			}
			this.CalculateMac();
			if (!this.VerifyMac(this.bufBlock, num - this.macSize))
			{
				throw new InvalidCipherTextException("mac check in EAX failed");
			}
			this.Reset(false);
			return num - this.macSize;
		}

		// Token: 0x06002892 RID: 10386 RVA: 0x000C8750 File Offset: 0x000C6950
		public virtual byte[] GetMac()
		{
			byte[] array = new byte[this.macSize];
			Array.Copy(this.macBlock, 0, array, 0, this.macSize);
			return array;
		}

		// Token: 0x06002893 RID: 10387 RVA: 0x000C8780 File Offset: 0x000C6980
		public virtual int GetUpdateOutputSize(int len)
		{
			int num = len + this.bufOff;
			if (!this.forEncryption)
			{
				if (num < this.macSize)
				{
					return 0;
				}
				num -= this.macSize;
			}
			return num - num % this.blockSize;
		}

		// Token: 0x06002894 RID: 10388 RVA: 0x000C87BC File Offset: 0x000C69BC
		public virtual int GetOutputSize(int len)
		{
			int num = len + this.bufOff;
			if (this.forEncryption)
			{
				return num + this.macSize;
			}
			if (num >= this.macSize)
			{
				return num - this.macSize;
			}
			return 0;
		}

		// Token: 0x06002895 RID: 10389 RVA: 0x000C87F8 File Offset: 0x000C69F8
		private int Process(byte b, byte[] outBytes, int outOff)
		{
			byte[] array = this.bufBlock;
			int num = this.bufOff;
			this.bufOff = num + 1;
			array[num] = b;
			if (this.bufOff == this.bufBlock.Length)
			{
				Check.OutputLength(outBytes, outOff, this.blockSize, "Output buffer is too short");
				int result;
				if (this.forEncryption)
				{
					result = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
					this.mac.BlockUpdate(outBytes, outOff, this.blockSize);
				}
				else
				{
					this.mac.BlockUpdate(this.bufBlock, 0, this.blockSize);
					result = this.cipher.ProcessBlock(this.bufBlock, 0, outBytes, outOff);
				}
				this.bufOff = 0;
				if (!this.forEncryption)
				{
					Array.Copy(this.bufBlock, this.blockSize, this.bufBlock, 0, this.macSize);
					this.bufOff = this.macSize;
				}
				return result;
			}
			return 0;
		}

		// Token: 0x06002896 RID: 10390 RVA: 0x000C88E0 File Offset: 0x000C6AE0
		private bool VerifyMac(byte[] mac, int off)
		{
			int num = 0;
			for (int i = 0; i < this.macSize; i++)
			{
				num |= (int)(this.macBlock[i] ^ mac[off + i]);
			}
			return num == 0;
		}

		// Token: 0x04001AA1 RID: 6817
		private SicBlockCipher cipher;

		// Token: 0x04001AA2 RID: 6818
		private bool forEncryption;

		// Token: 0x04001AA3 RID: 6819
		private int blockSize;

		// Token: 0x04001AA4 RID: 6820
		private IMac mac;

		// Token: 0x04001AA5 RID: 6821
		private byte[] nonceMac;

		// Token: 0x04001AA6 RID: 6822
		private byte[] associatedTextMac;

		// Token: 0x04001AA7 RID: 6823
		private byte[] macBlock;

		// Token: 0x04001AA8 RID: 6824
		private int macSize;

		// Token: 0x04001AA9 RID: 6825
		private byte[] bufBlock;

		// Token: 0x04001AAA RID: 6826
		private int bufOff;

		// Token: 0x04001AAB RID: 6827
		private bool cipherInitialized;

		// Token: 0x04001AAC RID: 6828
		private byte[] initialAssociatedText;

		// Token: 0x02000887 RID: 2183
		private enum Tag : byte
		{
			// Token: 0x04002F68 RID: 12136
			N,
			// Token: 0x04002F69 RID: 12137
			H,
			// Token: 0x04002F6A RID: 12138
			C
		}
	}
}
