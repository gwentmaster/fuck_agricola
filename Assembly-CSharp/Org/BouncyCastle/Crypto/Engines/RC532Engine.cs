using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000494 RID: 1172
	public class RC532Engine : IBlockCipher
	{
		// Token: 0x06002AC1 RID: 10945 RVA: 0x000D8D33 File Offset: 0x000D6F33
		public RC532Engine()
		{
			this._noRounds = 12;
		}

		// Token: 0x170004F8 RID: 1272
		// (get) Token: 0x06002AC2 RID: 10946 RVA: 0x000D8D43 File Offset: 0x000D6F43
		public virtual string AlgorithmName
		{
			get
			{
				return "RC5-32";
			}
		}

		// Token: 0x170004F9 RID: 1273
		// (get) Token: 0x06002AC3 RID: 10947 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002AC4 RID: 10948 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002AC5 RID: 10949 RVA: 0x000D8D4C File Offset: 0x000D6F4C
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (typeof(RC5Parameters).IsInstanceOfType(parameters))
			{
				RC5Parameters rc5Parameters = (RC5Parameters)parameters;
				this._noRounds = rc5Parameters.Rounds;
				this.SetKey(rc5Parameters.GetKey());
			}
			else
			{
				if (!typeof(KeyParameter).IsInstanceOfType(parameters))
				{
					throw new ArgumentException("invalid parameter passed to RC532 init - " + Platform.GetTypeName(parameters));
				}
				KeyParameter keyParameter = (KeyParameter)parameters;
				this.SetKey(keyParameter.GetKey());
			}
			this.forEncryption = forEncryption;
		}

		// Token: 0x06002AC6 RID: 10950 RVA: 0x000D8DD0 File Offset: 0x000D6FD0
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (!this.forEncryption)
			{
				return this.DecryptBlock(input, inOff, output, outOff);
			}
			return this.EncryptBlock(input, inOff, output, outOff);
		}

		// Token: 0x06002AC7 RID: 10951 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002AC8 RID: 10952 RVA: 0x000D8DF4 File Offset: 0x000D6FF4
		private void SetKey(byte[] key)
		{
			int[] array = new int[(key.Length + 3) / 4];
			for (int num = 0; num != key.Length; num++)
			{
				array[num / 4] += (int)(key[num] & byte.MaxValue) << 8 * (num % 4);
			}
			this._S = new int[2 * (this._noRounds + 1)];
			this._S[0] = RC532Engine.P32;
			for (int i = 1; i < this._S.Length; i++)
			{
				this._S[i] = this._S[i - 1] + RC532Engine.Q32;
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
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			int num6 = 0;
			for (int j = 0; j < num2; j++)
			{
				num3 = (this._S[num5] = this.RotateLeft(this._S[num5] + num3 + num4, 3));
				num4 = (array[num6] = this.RotateLeft(array[num6] + num3 + num4, num3 + num4));
				num5 = (num5 + 1) % this._S.Length;
				num6 = (num6 + 1) % array.Length;
			}
		}

		// Token: 0x06002AC9 RID: 10953 RVA: 0x000D8F28 File Offset: 0x000D7128
		private int EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = this.BytesToWord(input, inOff) + this._S[0];
			int num2 = this.BytesToWord(input, inOff + 4) + this._S[1];
			for (int i = 1; i <= this._noRounds; i++)
			{
				num = this.RotateLeft(num ^ num2, num2) + this._S[2 * i];
				num2 = this.RotateLeft(num2 ^ num, num) + this._S[2 * i + 1];
			}
			this.WordToBytes(num, outBytes, outOff);
			this.WordToBytes(num2, outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x06002ACA RID: 10954 RVA: 0x000D8FB4 File Offset: 0x000D71B4
		private int DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = this.BytesToWord(input, inOff);
			int num2 = this.BytesToWord(input, inOff + 4);
			for (int i = this._noRounds; i >= 1; i--)
			{
				num2 = (this.RotateRight(num2 - this._S[2 * i + 1], num) ^ num);
				num = (this.RotateRight(num - this._S[2 * i], num2) ^ num2);
			}
			this.WordToBytes(num - this._S[0], outBytes, outOff);
			this.WordToBytes(num2 - this._S[1], outBytes, outOff + 4);
			return 8;
		}

		// Token: 0x06002ACB RID: 10955 RVA: 0x000D903D File Offset: 0x000D723D
		private int RotateLeft(int x, int y)
		{
			return x << y | (int)((uint)x >> 32 - (y & 31));
		}

		// Token: 0x06002ACC RID: 10956 RVA: 0x000D9055 File Offset: 0x000D7255
		private int RotateRight(int x, int y)
		{
			return (int)((uint)x >> y | (uint)((uint)x << 32 - (y & 31)));
		}

		// Token: 0x06002ACD RID: 10957 RVA: 0x000D906D File Offset: 0x000D726D
		private int BytesToWord(byte[] src, int srcOff)
		{
			return (int)(src[srcOff] & byte.MaxValue) | (int)(src[srcOff + 1] & byte.MaxValue) << 8 | (int)(src[srcOff + 2] & byte.MaxValue) << 16 | (int)(src[srcOff + 3] & byte.MaxValue) << 24;
		}

		// Token: 0x06002ACE RID: 10958 RVA: 0x000D90A4 File Offset: 0x000D72A4
		private void WordToBytes(int word, byte[] dst, int dstOff)
		{
			dst[dstOff] = (byte)word;
			dst[dstOff + 1] = (byte)(word >> 8);
			dst[dstOff + 2] = (byte)(word >> 16);
			dst[dstOff + 3] = (byte)(word >> 24);
		}

		// Token: 0x04001C34 RID: 7220
		private int _noRounds;

		// Token: 0x04001C35 RID: 7221
		private int[] _S;

		// Token: 0x04001C36 RID: 7222
		private static readonly int P32 = -1209970333;

		// Token: 0x04001C37 RID: 7223
		private static readonly int Q32 = -1640531527;

		// Token: 0x04001C38 RID: 7224
		private bool forEncryption;
	}
}
