using System;
using Org.BouncyCastle.Crypto.Modes.Gcm;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x0200045E RID: 1118
	public class GcmBlockCipher : IAeadBlockCipher
	{
		// Token: 0x06002897 RID: 10391 RVA: 0x000C8915 File Offset: 0x000C6B15
		public GcmBlockCipher(IBlockCipher c) : this(c, null)
		{
		}

		// Token: 0x06002898 RID: 10392 RVA: 0x000C8920 File Offset: 0x000C6B20
		public GcmBlockCipher(IBlockCipher c, IGcmMultiplier m)
		{
			if (c.GetBlockSize() != 16)
			{
				throw new ArgumentException("cipher required with a block size of " + 16 + ".");
			}
			if (m == null)
			{
				m = new Tables8kGcmMultiplier();
			}
			this.cipher = c;
			this.multiplier = m;
		}

		// Token: 0x170004C3 RID: 1219
		// (get) Token: 0x06002899 RID: 10393 RVA: 0x000C8971 File Offset: 0x000C6B71
		public virtual string AlgorithmName
		{
			get
			{
				return this.cipher.AlgorithmName + "/GCM";
			}
		}

		// Token: 0x0600289A RID: 10394 RVA: 0x000C8988 File Offset: 0x000C6B88
		public IBlockCipher GetUnderlyingCipher()
		{
			return this.cipher;
		}

		// Token: 0x0600289B RID: 10395 RVA: 0x000C8990 File Offset: 0x000C6B90
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x0600289C RID: 10396 RVA: 0x000C8994 File Offset: 0x000C6B94
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.forEncryption = forEncryption;
			this.macBlock = null;
			KeyParameter keyParameter;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				this.nonce = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				int num = aeadParameters.MacSize;
				if (num < 32 || num > 128 || num % 8 != 0)
				{
					throw new ArgumentException("Invalid value for MAC size: " + num);
				}
				this.macSize = num / 8;
				keyParameter = aeadParameters.Key;
			}
			else
			{
				if (!(parameters is ParametersWithIV))
				{
					throw new ArgumentException("invalid parameters passed to GCM");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				this.nonce = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = 16;
				keyParameter = (KeyParameter)parametersWithIV.Parameters;
			}
			int num2 = forEncryption ? 16 : (16 + this.macSize);
			this.bufBlock = new byte[num2];
			if (this.nonce == null || this.nonce.Length < 1)
			{
				throw new ArgumentException("IV must be at least 1 byte");
			}
			if (keyParameter != null)
			{
				this.cipher.Init(true, keyParameter);
				this.H = new byte[16];
				this.cipher.ProcessBlock(this.H, 0, this.H, 0);
				this.multiplier.Init(this.H);
				this.exp = null;
			}
			else if (this.H == null)
			{
				throw new ArgumentException("Key must be specified in initial init");
			}
			this.J0 = new byte[16];
			if (this.nonce.Length == 12)
			{
				Array.Copy(this.nonce, 0, this.J0, 0, this.nonce.Length);
				this.J0[15] = 1;
			}
			else
			{
				this.gHASH(this.J0, this.nonce, this.nonce.Length);
				byte[] array = new byte[16];
				Pack.UInt64_To_BE((ulong)((long)this.nonce.Length * 8L), array, 8);
				this.gHASHBlock(this.J0, array);
			}
			this.S = new byte[16];
			this.S_at = new byte[16];
			this.S_atPre = new byte[16];
			this.atBlock = new byte[16];
			this.atBlockPos = 0;
			this.atLength = 0UL;
			this.atLengthPre = 0UL;
			this.counter = Arrays.Clone(this.J0);
			this.blocksRemaining = 4294967294U;
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x0600289D RID: 10397 RVA: 0x000C8C11 File Offset: 0x000C6E11
		public virtual byte[] GetMac()
		{
			return Arrays.Clone(this.macBlock);
		}

		// Token: 0x0600289E RID: 10398 RVA: 0x000C8C20 File Offset: 0x000C6E20
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

		// Token: 0x0600289F RID: 10399 RVA: 0x000C8C5C File Offset: 0x000C6E5C
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
			return num - num % 16;
		}

		// Token: 0x060028A0 RID: 10400 RVA: 0x000C8C94 File Offset: 0x000C6E94
		public virtual void ProcessAadByte(byte input)
		{
			this.atBlock[this.atBlockPos] = input;
			int num = this.atBlockPos + 1;
			this.atBlockPos = num;
			if (num == 16)
			{
				this.gHASHBlock(this.S_at, this.atBlock);
				this.atBlockPos = 0;
				this.atLength += 16UL;
			}
		}

		// Token: 0x060028A1 RID: 10401 RVA: 0x000C8CF0 File Offset: 0x000C6EF0
		public virtual void ProcessAadBytes(byte[] inBytes, int inOff, int len)
		{
			for (int i = 0; i < len; i++)
			{
				this.atBlock[this.atBlockPos] = inBytes[inOff + i];
				int num = this.atBlockPos + 1;
				this.atBlockPos = num;
				if (num == 16)
				{
					this.gHASHBlock(this.S_at, this.atBlock);
					this.atBlockPos = 0;
					this.atLength += 16UL;
				}
			}
		}

		// Token: 0x060028A2 RID: 10402 RVA: 0x000C8D5C File Offset: 0x000C6F5C
		private void InitCipher()
		{
			if (this.atLength > 0UL)
			{
				Array.Copy(this.S_at, 0, this.S_atPre, 0, 16);
				this.atLengthPre = this.atLength;
			}
			if (this.atBlockPos > 0)
			{
				this.gHASHPartial(this.S_atPre, this.atBlock, 0, this.atBlockPos);
				this.atLengthPre += (ulong)this.atBlockPos;
			}
			if (this.atLengthPre > 0UL)
			{
				Array.Copy(this.S_atPre, 0, this.S, 0, 16);
			}
		}

		// Token: 0x060028A3 RID: 10403 RVA: 0x000C8DEC File Offset: 0x000C6FEC
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			this.bufBlock[this.bufOff] = input;
			int num = this.bufOff + 1;
			this.bufOff = num;
			if (num == this.bufBlock.Length)
			{
				this.OutputBlock(output, outOff);
				return 16;
			}
			return 0;
		}

		// Token: 0x060028A4 RID: 10404 RVA: 0x000C8E30 File Offset: 0x000C7030
		public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			if (input.Length < inOff + len)
			{
				throw new DataLengthException("Input buffer too short");
			}
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				this.bufBlock[this.bufOff] = input[inOff + i];
				int num2 = this.bufOff + 1;
				this.bufOff = num2;
				if (num2 == this.bufBlock.Length)
				{
					this.OutputBlock(output, outOff + num);
					num += 16;
				}
			}
			return num;
		}

		// Token: 0x060028A5 RID: 10405 RVA: 0x000C8EA0 File Offset: 0x000C70A0
		private void OutputBlock(byte[] output, int offset)
		{
			Check.OutputLength(output, offset, 16, "Output buffer too short");
			if (this.totalLength == 0UL)
			{
				this.InitCipher();
			}
			this.gCTRBlock(this.bufBlock, output, offset);
			if (this.forEncryption)
			{
				this.bufOff = 0;
				return;
			}
			Array.Copy(this.bufBlock, 16, this.bufBlock, 0, this.macSize);
			this.bufOff = this.macSize;
		}

		// Token: 0x060028A6 RID: 10406 RVA: 0x000C8F10 File Offset: 0x000C7110
		public int DoFinal(byte[] output, int outOff)
		{
			if (this.totalLength == 0UL)
			{
				this.InitCipher();
			}
			int num = this.bufOff;
			if (this.forEncryption)
			{
				Check.OutputLength(output, outOff, num + this.macSize, "Output buffer too short");
			}
			else
			{
				if (num < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				num -= this.macSize;
				Check.OutputLength(output, outOff, num, "Output buffer too short");
			}
			if (num > 0)
			{
				this.gCTRPartial(this.bufBlock, 0, num, output, outOff);
			}
			this.atLength += (ulong)this.atBlockPos;
			if (this.atLength > this.atLengthPre)
			{
				if (this.atBlockPos > 0)
				{
					this.gHASHPartial(this.S_at, this.atBlock, 0, this.atBlockPos);
				}
				if (this.atLengthPre > 0UL)
				{
					GcmUtilities.Xor(this.S_at, this.S_atPre);
				}
				long pow = (long)(this.totalLength * 8UL + 127UL >> 7);
				byte[] array = new byte[16];
				if (this.exp == null)
				{
					this.exp = new Tables1kGcmExponentiator();
					this.exp.Init(this.H);
				}
				this.exp.ExponentiateX(pow, array);
				GcmUtilities.Multiply(this.S_at, array);
				GcmUtilities.Xor(this.S, this.S_at);
			}
			byte[] array2 = new byte[16];
			Pack.UInt64_To_BE(this.atLength * 8UL, array2, 0);
			Pack.UInt64_To_BE(this.totalLength * 8UL, array2, 8);
			this.gHASHBlock(this.S, array2);
			byte[] array3 = new byte[16];
			this.cipher.ProcessBlock(this.J0, 0, array3, 0);
			GcmUtilities.Xor(array3, this.S);
			int num2 = num;
			this.macBlock = new byte[this.macSize];
			Array.Copy(array3, 0, this.macBlock, 0, this.macSize);
			if (this.forEncryption)
			{
				Array.Copy(this.macBlock, 0, output, outOff + this.bufOff, this.macSize);
				num2 += this.macSize;
			}
			else
			{
				byte[] array4 = new byte[this.macSize];
				Array.Copy(this.bufBlock, num, array4, 0, this.macSize);
				if (!Arrays.ConstantTimeAreEqual(this.macBlock, array4))
				{
					throw new InvalidCipherTextException("mac check in GCM failed");
				}
			}
			this.Reset(false);
			return num2;
		}

		// Token: 0x060028A7 RID: 10407 RVA: 0x000C9152 File Offset: 0x000C7352
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x060028A8 RID: 10408 RVA: 0x000C915C File Offset: 0x000C735C
		private void Reset(bool clearMac)
		{
			this.cipher.Reset();
			this.S = new byte[16];
			this.S_at = new byte[16];
			this.S_atPre = new byte[16];
			this.atBlock = new byte[16];
			this.atBlockPos = 0;
			this.atLength = 0UL;
			this.atLengthPre = 0UL;
			this.counter = Arrays.Clone(this.J0);
			this.blocksRemaining = 4294967294U;
			this.bufOff = 0;
			this.totalLength = 0UL;
			if (this.bufBlock != null)
			{
				Arrays.Fill(this.bufBlock, 0);
			}
			if (clearMac)
			{
				this.macBlock = null;
			}
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x060028A9 RID: 10409 RVA: 0x000C9224 File Offset: 0x000C7424
		private void gCTRBlock(byte[] block, byte[] output, int outOff)
		{
			byte[] nextCounterBlock = this.GetNextCounterBlock();
			GcmUtilities.Xor(nextCounterBlock, block);
			Array.Copy(nextCounterBlock, 0, output, outOff, 16);
			this.gHASHBlock(this.S, this.forEncryption ? nextCounterBlock : block);
			this.totalLength += 16UL;
		}

		// Token: 0x060028AA RID: 10410 RVA: 0x000C9274 File Offset: 0x000C7474
		private void gCTRPartial(byte[] buf, int off, int len, byte[] output, int outOff)
		{
			byte[] nextCounterBlock = this.GetNextCounterBlock();
			GcmUtilities.Xor(nextCounterBlock, buf, off, len);
			Array.Copy(nextCounterBlock, 0, output, outOff, len);
			this.gHASHPartial(this.S, this.forEncryption ? nextCounterBlock : buf, 0, len);
			this.totalLength += (ulong)len;
		}

		// Token: 0x060028AB RID: 10411 RVA: 0x000C92C8 File Offset: 0x000C74C8
		private void gHASH(byte[] Y, byte[] b, int len)
		{
			for (int i = 0; i < len; i += 16)
			{
				int len2 = Math.Min(len - i, 16);
				this.gHASHPartial(Y, b, i, len2);
			}
		}

		// Token: 0x060028AC RID: 10412 RVA: 0x000C92F7 File Offset: 0x000C74F7
		private void gHASHBlock(byte[] Y, byte[] b)
		{
			GcmUtilities.Xor(Y, b);
			this.multiplier.MultiplyH(Y);
		}

		// Token: 0x060028AD RID: 10413 RVA: 0x000C930C File Offset: 0x000C750C
		private void gHASHPartial(byte[] Y, byte[] b, int off, int len)
		{
			GcmUtilities.Xor(Y, b, off, len);
			this.multiplier.MultiplyH(Y);
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000C9324 File Offset: 0x000C7524
		private byte[] GetNextCounterBlock()
		{
			if (this.blocksRemaining == 0U)
			{
				throw new InvalidOperationException("Attempt to process too many blocks");
			}
			this.blocksRemaining -= 1U;
			uint num = 1U;
			num += (uint)this.counter[15];
			this.counter[15] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[14];
			this.counter[14] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[13];
			this.counter[13] = (byte)num;
			num >>= 8;
			num += (uint)this.counter[12];
			this.counter[12] = (byte)num;
			byte[] array = new byte[16];
			this.cipher.ProcessBlock(this.counter, 0, array, 0);
			return array;
		}

		// Token: 0x04001AAD RID: 6829
		private const int BlockSize = 16;

		// Token: 0x04001AAE RID: 6830
		private readonly IBlockCipher cipher;

		// Token: 0x04001AAF RID: 6831
		private readonly IGcmMultiplier multiplier;

		// Token: 0x04001AB0 RID: 6832
		private IGcmExponentiator exp;

		// Token: 0x04001AB1 RID: 6833
		private bool forEncryption;

		// Token: 0x04001AB2 RID: 6834
		private int macSize;

		// Token: 0x04001AB3 RID: 6835
		private byte[] nonce;

		// Token: 0x04001AB4 RID: 6836
		private byte[] initialAssociatedText;

		// Token: 0x04001AB5 RID: 6837
		private byte[] H;

		// Token: 0x04001AB6 RID: 6838
		private byte[] J0;

		// Token: 0x04001AB7 RID: 6839
		private byte[] bufBlock;

		// Token: 0x04001AB8 RID: 6840
		private byte[] macBlock;

		// Token: 0x04001AB9 RID: 6841
		private byte[] S;

		// Token: 0x04001ABA RID: 6842
		private byte[] S_at;

		// Token: 0x04001ABB RID: 6843
		private byte[] S_atPre;

		// Token: 0x04001ABC RID: 6844
		private byte[] counter;

		// Token: 0x04001ABD RID: 6845
		private uint blocksRemaining;

		// Token: 0x04001ABE RID: 6846
		private int bufOff;

		// Token: 0x04001ABF RID: 6847
		private ulong totalLength;

		// Token: 0x04001AC0 RID: 6848
		private byte[] atBlock;

		// Token: 0x04001AC1 RID: 6849
		private int atBlockPos;

		// Token: 0x04001AC2 RID: 6850
		private ulong atLength;

		// Token: 0x04001AC3 RID: 6851
		private ulong atLengthPre;
	}
}
