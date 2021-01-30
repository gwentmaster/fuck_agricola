using System;
using System.Collections;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes
{
	// Token: 0x02000461 RID: 1121
	public class OcbBlockCipher : IAeadBlockCipher
	{
		// Token: 0x060028C6 RID: 10438 RVA: 0x000C9724 File Offset: 0x000C7924
		public OcbBlockCipher(IBlockCipher hashCipher, IBlockCipher mainCipher)
		{
			if (hashCipher == null)
			{
				throw new ArgumentNullException("hashCipher");
			}
			if (hashCipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("must have a block size of " + 16, "hashCipher");
			}
			if (mainCipher == null)
			{
				throw new ArgumentNullException("mainCipher");
			}
			if (mainCipher.GetBlockSize() != 16)
			{
				throw new ArgumentException("must have a block size of " + 16, "mainCipher");
			}
			if (!hashCipher.AlgorithmName.Equals(mainCipher.AlgorithmName))
			{
				throw new ArgumentException("'hashCipher' and 'mainCipher' must be the same algorithm");
			}
			this.hashCipher = hashCipher;
			this.mainCipher = mainCipher;
		}

		// Token: 0x060028C7 RID: 10439 RVA: 0x000C97F2 File Offset: 0x000C79F2
		public virtual IBlockCipher GetUnderlyingCipher()
		{
			return this.mainCipher;
		}

		// Token: 0x170004C7 RID: 1223
		// (get) Token: 0x060028C8 RID: 10440 RVA: 0x000C97FA File Offset: 0x000C79FA
		public virtual string AlgorithmName
		{
			get
			{
				return this.mainCipher.AlgorithmName + "/OCB";
			}
		}

		// Token: 0x060028C9 RID: 10441 RVA: 0x000C9814 File Offset: 0x000C7A14
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			bool flag = this.forEncryption;
			this.forEncryption = forEncryption;
			this.macBlock = null;
			byte[] array;
			KeyParameter keyParameter;
			if (parameters is AeadParameters)
			{
				AeadParameters aeadParameters = (AeadParameters)parameters;
				array = aeadParameters.GetNonce();
				this.initialAssociatedText = aeadParameters.GetAssociatedText();
				int num = aeadParameters.MacSize;
				if (num < 64 || num > 128 || num % 8 != 0)
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
					throw new ArgumentException("invalid parameters passed to OCB");
				}
				ParametersWithIV parametersWithIV = (ParametersWithIV)parameters;
				array = parametersWithIV.GetIV();
				this.initialAssociatedText = null;
				this.macSize = 16;
				keyParameter = (KeyParameter)parametersWithIV.Parameters;
			}
			this.hashBlock = new byte[16];
			this.mainBlock = new byte[forEncryption ? 16 : (16 + this.macSize)];
			if (array == null)
			{
				array = new byte[0];
			}
			if (array.Length > 15)
			{
				throw new ArgumentException("IV must be no more than 15 bytes");
			}
			if (keyParameter != null)
			{
				this.hashCipher.Init(true, keyParameter);
				this.mainCipher.Init(forEncryption, keyParameter);
				this.KtopInput = null;
			}
			else if (flag != forEncryption)
			{
				throw new ArgumentException("cannot change encrypting state without providing key.");
			}
			this.L_Asterisk = new byte[16];
			this.hashCipher.ProcessBlock(this.L_Asterisk, 0, this.L_Asterisk, 0);
			this.L_Dollar = OcbBlockCipher.OCB_double(this.L_Asterisk);
			this.L = Platform.CreateArrayList();
			this.L.Add(OcbBlockCipher.OCB_double(this.L_Dollar));
			int num2 = this.ProcessNonce(array);
			int num3 = num2 % 8;
			int num4 = num2 / 8;
			if (num3 == 0)
			{
				Array.Copy(this.Stretch, num4, this.OffsetMAIN_0, 0, 16);
			}
			else
			{
				for (int i = 0; i < 16; i++)
				{
					uint num5 = (uint)this.Stretch[num4];
					uint num6 = (uint)this.Stretch[++num4];
					this.OffsetMAIN_0[i] = (byte)(num5 << num3 | num6 >> 8 - num3);
				}
			}
			this.hashBlockPos = 0;
			this.mainBlockPos = 0;
			this.hashBlockCount = 0L;
			this.mainBlockCount = 0L;
			this.OffsetHASH = new byte[16];
			this.Sum = new byte[16];
			Array.Copy(this.OffsetMAIN_0, 0, this.OffsetMAIN, 0, 16);
			this.Checksum = new byte[16];
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x060028CA RID: 10442 RVA: 0x000C9A9C File Offset: 0x000C7C9C
		protected virtual int ProcessNonce(byte[] N)
		{
			byte[] array = new byte[16];
			Array.Copy(N, 0, array, array.Length - N.Length, N.Length);
			array[0] = (byte)(this.macSize << 4);
			byte[] array2 = array;
			int num = 15 - N.Length;
			array2[num] |= 1;
			int result = (int)(array[15] & 63);
			byte[] array3 = array;
			int num2 = 15;
			array3[num2] &= 192;
			if (this.KtopInput == null || !Arrays.AreEqual(array, this.KtopInput))
			{
				byte[] array4 = new byte[16];
				this.KtopInput = array;
				this.hashCipher.ProcessBlock(this.KtopInput, 0, array4, 0);
				Array.Copy(array4, 0, this.Stretch, 0, 16);
				for (int i = 0; i < 8; i++)
				{
					this.Stretch[16 + i] = (array4[i] ^ array4[i + 1]);
				}
			}
			return result;
		}

		// Token: 0x060028CB RID: 10443 RVA: 0x000C8990 File Offset: 0x000C6B90
		public virtual int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x060028CC RID: 10444 RVA: 0x000C9B67 File Offset: 0x000C7D67
		public virtual byte[] GetMac()
		{
			return Arrays.Clone(this.macBlock);
		}

		// Token: 0x060028CD RID: 10445 RVA: 0x000C9B74 File Offset: 0x000C7D74
		public virtual int GetOutputSize(int len)
		{
			int num = len + this.mainBlockPos;
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

		// Token: 0x060028CE RID: 10446 RVA: 0x000C9BB0 File Offset: 0x000C7DB0
		public virtual int GetUpdateOutputSize(int len)
		{
			int num = len + this.mainBlockPos;
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

		// Token: 0x060028CF RID: 10447 RVA: 0x000C9BE8 File Offset: 0x000C7DE8
		public virtual void ProcessAadByte(byte input)
		{
			this.hashBlock[this.hashBlockPos] = input;
			int num = this.hashBlockPos + 1;
			this.hashBlockPos = num;
			if (num == this.hashBlock.Length)
			{
				this.ProcessHashBlock();
			}
		}

		// Token: 0x060028D0 RID: 10448 RVA: 0x000C9C24 File Offset: 0x000C7E24
		public virtual void ProcessAadBytes(byte[] input, int off, int len)
		{
			for (int i = 0; i < len; i++)
			{
				this.hashBlock[this.hashBlockPos] = input[off + i];
				int num = this.hashBlockPos + 1;
				this.hashBlockPos = num;
				if (num == this.hashBlock.Length)
				{
					this.ProcessHashBlock();
				}
			}
		}

		// Token: 0x060028D1 RID: 10449 RVA: 0x000C9C70 File Offset: 0x000C7E70
		public virtual int ProcessByte(byte input, byte[] output, int outOff)
		{
			this.mainBlock[this.mainBlockPos] = input;
			int num = this.mainBlockPos + 1;
			this.mainBlockPos = num;
			if (num == this.mainBlock.Length)
			{
				this.ProcessMainBlock(output, outOff);
				return 16;
			}
			return 0;
		}

		// Token: 0x060028D2 RID: 10450 RVA: 0x000C9CB4 File Offset: 0x000C7EB4
		public virtual int ProcessBytes(byte[] input, int inOff, int len, byte[] output, int outOff)
		{
			int num = 0;
			for (int i = 0; i < len; i++)
			{
				this.mainBlock[this.mainBlockPos] = input[inOff + i];
				int num2 = this.mainBlockPos + 1;
				this.mainBlockPos = num2;
				if (num2 == this.mainBlock.Length)
				{
					this.ProcessMainBlock(output, outOff + num);
					num += 16;
				}
			}
			return num;
		}

		// Token: 0x060028D3 RID: 10451 RVA: 0x000C9D10 File Offset: 0x000C7F10
		public virtual int DoFinal(byte[] output, int outOff)
		{
			byte[] array = null;
			if (!this.forEncryption)
			{
				if (this.mainBlockPos < this.macSize)
				{
					throw new InvalidCipherTextException("data too short");
				}
				this.mainBlockPos -= this.macSize;
				array = new byte[this.macSize];
				Array.Copy(this.mainBlock, this.mainBlockPos, array, 0, this.macSize);
			}
			if (this.hashBlockPos > 0)
			{
				OcbBlockCipher.OCB_extend(this.hashBlock, this.hashBlockPos);
				this.UpdateHASH(this.L_Asterisk);
			}
			if (this.mainBlockPos > 0)
			{
				if (this.forEncryption)
				{
					OcbBlockCipher.OCB_extend(this.mainBlock, this.mainBlockPos);
					OcbBlockCipher.Xor(this.Checksum, this.mainBlock);
				}
				OcbBlockCipher.Xor(this.OffsetMAIN, this.L_Asterisk);
				byte[] array2 = new byte[16];
				this.hashCipher.ProcessBlock(this.OffsetMAIN, 0, array2, 0);
				OcbBlockCipher.Xor(this.mainBlock, array2);
				Check.OutputLength(output, outOff, this.mainBlockPos, "Output buffer too short");
				Array.Copy(this.mainBlock, 0, output, outOff, this.mainBlockPos);
				if (!this.forEncryption)
				{
					OcbBlockCipher.OCB_extend(this.mainBlock, this.mainBlockPos);
					OcbBlockCipher.Xor(this.Checksum, this.mainBlock);
				}
			}
			OcbBlockCipher.Xor(this.Checksum, this.OffsetMAIN);
			OcbBlockCipher.Xor(this.Checksum, this.L_Dollar);
			this.hashCipher.ProcessBlock(this.Checksum, 0, this.Checksum, 0);
			OcbBlockCipher.Xor(this.Checksum, this.Sum);
			this.macBlock = new byte[this.macSize];
			Array.Copy(this.Checksum, 0, this.macBlock, 0, this.macSize);
			int num = this.mainBlockPos;
			if (this.forEncryption)
			{
				Check.OutputLength(output, outOff, num + this.macSize, "Output buffer too short");
				Array.Copy(this.macBlock, 0, output, outOff + num, this.macSize);
				num += this.macSize;
			}
			else if (!Arrays.ConstantTimeAreEqual(this.macBlock, array))
			{
				throw new InvalidCipherTextException("mac check in OCB failed");
			}
			this.Reset(false);
			return num;
		}

		// Token: 0x060028D4 RID: 10452 RVA: 0x000C9F3A File Offset: 0x000C813A
		public virtual void Reset()
		{
			this.Reset(true);
		}

		// Token: 0x060028D5 RID: 10453 RVA: 0x000C9F43 File Offset: 0x000C8143
		protected virtual void Clear(byte[] bs)
		{
			if (bs != null)
			{
				Array.Clear(bs, 0, bs.Length);
			}
		}

		// Token: 0x060028D6 RID: 10454 RVA: 0x000C9F54 File Offset: 0x000C8154
		protected virtual byte[] GetLSub(int n)
		{
			while (n >= this.L.Count)
			{
				this.L.Add(OcbBlockCipher.OCB_double((byte[])this.L[this.L.Count - 1]));
			}
			return (byte[])this.L[n];
		}

		// Token: 0x060028D7 RID: 10455 RVA: 0x000C9FB0 File Offset: 0x000C81B0
		protected virtual void ProcessHashBlock()
		{
			long x = this.hashBlockCount + 1L;
			this.hashBlockCount = x;
			this.UpdateHASH(this.GetLSub(OcbBlockCipher.OCB_ntz(x)));
			this.hashBlockPos = 0;
		}

		// Token: 0x060028D8 RID: 10456 RVA: 0x000C9FE8 File Offset: 0x000C81E8
		protected virtual void ProcessMainBlock(byte[] output, int outOff)
		{
			Check.DataLength(output, outOff, 16, "Output buffer too short");
			if (this.forEncryption)
			{
				OcbBlockCipher.Xor(this.Checksum, this.mainBlock);
				this.mainBlockPos = 0;
			}
			byte[] offsetMAIN = this.OffsetMAIN;
			long x = this.mainBlockCount + 1L;
			this.mainBlockCount = x;
			OcbBlockCipher.Xor(offsetMAIN, this.GetLSub(OcbBlockCipher.OCB_ntz(x)));
			OcbBlockCipher.Xor(this.mainBlock, this.OffsetMAIN);
			this.mainCipher.ProcessBlock(this.mainBlock, 0, this.mainBlock, 0);
			OcbBlockCipher.Xor(this.mainBlock, this.OffsetMAIN);
			Array.Copy(this.mainBlock, 0, output, outOff, 16);
			if (!this.forEncryption)
			{
				OcbBlockCipher.Xor(this.Checksum, this.mainBlock);
				Array.Copy(this.mainBlock, 16, this.mainBlock, 0, this.macSize);
				this.mainBlockPos = this.macSize;
			}
		}

		// Token: 0x060028D9 RID: 10457 RVA: 0x000CA0D8 File Offset: 0x000C82D8
		protected virtual void Reset(bool clearMac)
		{
			this.hashCipher.Reset();
			this.mainCipher.Reset();
			this.Clear(this.hashBlock);
			this.Clear(this.mainBlock);
			this.hashBlockPos = 0;
			this.mainBlockPos = 0;
			this.hashBlockCount = 0L;
			this.mainBlockCount = 0L;
			this.Clear(this.OffsetHASH);
			this.Clear(this.Sum);
			Array.Copy(this.OffsetMAIN_0, 0, this.OffsetMAIN, 0, 16);
			this.Clear(this.Checksum);
			if (clearMac)
			{
				this.macBlock = null;
			}
			if (this.initialAssociatedText != null)
			{
				this.ProcessAadBytes(this.initialAssociatedText, 0, this.initialAssociatedText.Length);
			}
		}

		// Token: 0x060028DA RID: 10458 RVA: 0x000CA194 File Offset: 0x000C8394
		protected virtual void UpdateHASH(byte[] LSub)
		{
			OcbBlockCipher.Xor(this.OffsetHASH, LSub);
			OcbBlockCipher.Xor(this.hashBlock, this.OffsetHASH);
			this.hashCipher.ProcessBlock(this.hashBlock, 0, this.hashBlock, 0);
			OcbBlockCipher.Xor(this.Sum, this.hashBlock);
		}

		// Token: 0x060028DB RID: 10459 RVA: 0x000CA1EC File Offset: 0x000C83EC
		protected static byte[] OCB_double(byte[] block)
		{
			byte[] array = new byte[16];
			int num = OcbBlockCipher.ShiftLeft(block, array);
			byte[] array2 = array;
			int num2 = 15;
			array2[num2] ^= (byte)(135 >> (1 - num << 3));
			return array;
		}

		// Token: 0x060028DC RID: 10460 RVA: 0x000CA226 File Offset: 0x000C8426
		protected static void OCB_extend(byte[] block, int pos)
		{
			block[pos] = 128;
			while (++pos < 16)
			{
				block[pos] = 0;
			}
		}

		// Token: 0x060028DD RID: 10461 RVA: 0x000CA240 File Offset: 0x000C8440
		protected static int OCB_ntz(long x)
		{
			if (x == 0L)
			{
				return 64;
			}
			int num = 0;
			ulong num2 = (ulong)x;
			while ((num2 & 1UL) == 0UL)
			{
				num++;
				num2 >>= 1;
			}
			return num;
		}

		// Token: 0x060028DE RID: 10462 RVA: 0x000CA268 File Offset: 0x000C8468
		protected static int ShiftLeft(byte[] block, byte[] output)
		{
			int num = 16;
			uint num2 = 0U;
			while (--num >= 0)
			{
				uint num3 = (uint)block[num];
				output[num] = (byte)(num3 << 1 | num2);
				num2 = (num3 >> 7 & 1U);
			}
			return (int)num2;
		}

		// Token: 0x060028DF RID: 10463 RVA: 0x000CA298 File Offset: 0x000C8498
		protected static void Xor(byte[] block, byte[] val)
		{
			for (int i = 15; i >= 0; i--)
			{
				int num = i;
				block[num] ^= val[i];
			}
		}

		// Token: 0x04001ACE RID: 6862
		private const int BLOCK_SIZE = 16;

		// Token: 0x04001ACF RID: 6863
		private readonly IBlockCipher hashCipher;

		// Token: 0x04001AD0 RID: 6864
		private readonly IBlockCipher mainCipher;

		// Token: 0x04001AD1 RID: 6865
		private bool forEncryption;

		// Token: 0x04001AD2 RID: 6866
		private int macSize;

		// Token: 0x04001AD3 RID: 6867
		private byte[] initialAssociatedText;

		// Token: 0x04001AD4 RID: 6868
		private IList L;

		// Token: 0x04001AD5 RID: 6869
		private byte[] L_Asterisk;

		// Token: 0x04001AD6 RID: 6870
		private byte[] L_Dollar;

		// Token: 0x04001AD7 RID: 6871
		private byte[] KtopInput;

		// Token: 0x04001AD8 RID: 6872
		private byte[] Stretch = new byte[24];

		// Token: 0x04001AD9 RID: 6873
		private byte[] OffsetMAIN_0 = new byte[16];

		// Token: 0x04001ADA RID: 6874
		private byte[] hashBlock;

		// Token: 0x04001ADB RID: 6875
		private byte[] mainBlock;

		// Token: 0x04001ADC RID: 6876
		private int hashBlockPos;

		// Token: 0x04001ADD RID: 6877
		private int mainBlockPos;

		// Token: 0x04001ADE RID: 6878
		private long hashBlockCount;

		// Token: 0x04001ADF RID: 6879
		private long mainBlockCount;

		// Token: 0x04001AE0 RID: 6880
		private byte[] OffsetHASH;

		// Token: 0x04001AE1 RID: 6881
		private byte[] Sum;

		// Token: 0x04001AE2 RID: 6882
		private byte[] OffsetMAIN = new byte[16];

		// Token: 0x04001AE3 RID: 6883
		private byte[] Checksum;

		// Token: 0x04001AE4 RID: 6884
		private byte[] macBlock;
	}
}
