using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000495 RID: 1173
	public class RC564Engine : IBlockCipher
	{
		// Token: 0x06002AD0 RID: 10960 RVA: 0x000D90DE File Offset: 0x000D72DE
		public RC564Engine()
		{
			this._noRounds = 12;
		}

		// Token: 0x170004FA RID: 1274
		// (get) Token: 0x06002AD1 RID: 10961 RVA: 0x000D90EE File Offset: 0x000D72EE
		public virtual string AlgorithmName
		{
			get
			{
				return "RC5-64";
			}
		}

		// Token: 0x170004FB RID: 1275
		// (get) Token: 0x06002AD2 RID: 10962 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002AD3 RID: 10963 RVA: 0x000D90F5 File Offset: 0x000D72F5
		public virtual int GetBlockSize()
		{
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x06002AD4 RID: 10964 RVA: 0x000D9100 File Offset: 0x000D7300
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!typeof(RC5Parameters).IsInstanceOfType(parameters))
			{
				throw new ArgumentException("invalid parameter passed to RC564 init - " + Platform.GetTypeName(parameters));
			}
			RC5Parameters rc5Parameters = (RC5Parameters)parameters;
			this.forEncryption = forEncryption;
			this._noRounds = rc5Parameters.Rounds;
			this.SetKey(rc5Parameters.GetKey());
		}

		// Token: 0x06002AD5 RID: 10965 RVA: 0x000D915B File Offset: 0x000D735B
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06002AD6 RID: 10966 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002AD7 RID: 10967 RVA: 0x000D917C File Offset: 0x000D737C
		private void SetKey(byte[] key)
		{
			long[] array = new long[(key.Length + (RC564Engine.bytesPerWord - 1)) / RC564Engine.bytesPerWord];
			for (int num = 0; num != key.Length; num++)
			{
				array[num / RC564Engine.bytesPerWord] += (long)(key[num] & byte.MaxValue) << 8 * (num % RC564Engine.bytesPerWord);
			}
			this._S = new long[2 * (this._noRounds + 1)];
			this._S[0] = RC564Engine.P64;
			for (int i = 1; i < this._S.Length; i++)
			{
				this._S[i] = this._S[i - 1] + RC564Engine.Q64;
			}
			int num2;
			if (array.Length > this._S.Length)
			{
				num2 = 3 * array.Length;
			}
			else
			{
				num2 = 3 * this._S.Length;
			}
			long num3 = 0L;
			long num4 = 0L;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < num2; j++)
			{
				num3 = (this._S[num5] = this.RotateLeft(this._S[num5] + num3 + num4, 3L));
				num4 = (array[num6] = this.RotateLeft(array[num6] + num3 + num4, num3 + num4));
				num5 = (num5 + 1) % this._S.Length;
				num6 = (num6 + 1) % array.Length;
			}
		}

		// Token: 0x06002AD8 RID: 10968 RVA: 0x000D92C4 File Offset: 0x000D74C4
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			long num = this.BytesToWord(input, inOff) + this._S[0];
			long num2 = this.BytesToWord(input, inOff + RC564Engine.bytesPerWord) + this._S[1];
			for (int i = 1; i <= this._noRounds; i++)
			{
				num = this.RotateLeft(num ^ num2, num2) + this._S[2 * i];
				num2 = this.RotateLeft(num2 ^ num, num) + this._S[2 * i + 1];
			}
			this.WordToBytes(num, outBytes, outOff);
			this.WordToBytes(num2, outBytes, outOff + RC564Engine.bytesPerWord);
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x06002AD9 RID: 10969 RVA: 0x000D935C File Offset: 0x000D755C
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			long num = this.BytesToWord(input, inOff);
			long num2 = this.BytesToWord(input, inOff + RC564Engine.bytesPerWord);
			for (int i = this._noRounds; i >= 1; i--)
			{
				num2 = (this.RotateRight(num2 - this._S[2 * i + 1], num) ^ num);
				num = (this.RotateRight(num - this._S[2 * i], num2) ^ num2);
			}
			this.WordToBytes(num - this._S[0], outBytes, outOff);
			this.WordToBytes(num2 - this._S[1], outBytes, outOff + RC564Engine.bytesPerWord);
			return 2 * RC564Engine.bytesPerWord;
		}

		// Token: 0x06002ADA RID: 10970 RVA: 0x000D93F3 File Offset: 0x000D75F3
		private long RotateLeft(long x, long y)
		{
			return x << (int)(y & (long)(RC564Engine.wordSize - 1)) | (long)((ulong)x >> (int)((long)RC564Engine.wordSize - (y & (long)(RC564Engine.wordSize - 1))));
		}

		// Token: 0x06002ADB RID: 10971 RVA: 0x000D941D File Offset: 0x000D761D
		private long RotateRight(long x, long y)
		{
			return (long)((ulong)x >> (int)(y & (long)(RC564Engine.wordSize - 1)) | (ulong)((ulong)x << (int)((long)RC564Engine.wordSize - (y & (long)(RC564Engine.wordSize - 1)))));
		}

		// Token: 0x06002ADC RID: 10972 RVA: 0x000D9448 File Offset: 0x000D7648
		private long BytesToWord(byte[] src, int srcOff)
		{
			long num = 0L;
			for (int i = RC564Engine.bytesPerWord - 1; i >= 0; i--)
			{
				num = (num << 8) + (long)(src[i + srcOff] & byte.MaxValue);
			}
			return num;
		}

		// Token: 0x06002ADD RID: 10973 RVA: 0x000D947C File Offset: 0x000D767C
		private void WordToBytes(long word, byte[] dst, int dstOff)
		{
			for (int i = 0; i < RC564Engine.bytesPerWord; i++)
			{
				dst[i + dstOff] = (byte)word;
				word = (long)((ulong)word >> 8);
			}
		}

		// Token: 0x04001C39 RID: 7225
		private static readonly int wordSize = 64;

		// Token: 0x04001C3A RID: 7226
		private static readonly int bytesPerWord = RC564Engine.wordSize / 8;

		// Token: 0x04001C3B RID: 7227
		private int _noRounds;

		// Token: 0x04001C3C RID: 7228
		private long[] _S;

		// Token: 0x04001C3D RID: 7229
		private static readonly long P64 = -5196783011329398165L;

		// Token: 0x04001C3E RID: 7230
		private static readonly long Q64 = -7046029254386353131L;

		// Token: 0x04001C3F RID: 7231
		private bool forEncryption;
	}
}
