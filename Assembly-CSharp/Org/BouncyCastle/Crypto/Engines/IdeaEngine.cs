using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x0200048E RID: 1166
	public class IdeaEngine : IBlockCipher
	{
		// Token: 0x06002A7E RID: 10878 RVA: 0x000D6DE6 File Offset: 0x000D4FE6
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to IDEA init - " + Platform.GetTypeName(parameters));
			}
			this.workingKey = this.GenerateWorkingKey(forEncryption, ((KeyParameter)parameters).GetKey());
		}

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06002A7F RID: 10879 RVA: 0x000D6E1E File Offset: 0x000D501E
		public virtual string AlgorithmName
		{
			get
			{
				return "IDEA";
			}
		}

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06002A80 RID: 10880 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A81 RID: 10881 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002A82 RID: 10882 RVA: 0x000D6E28 File Offset: 0x000D5028
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("IDEA engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			this.IdeaFunc(this.workingKey, input, inOff, output, outOff);
			return 8;
		}

		// Token: 0x06002A83 RID: 10883 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002A84 RID: 10884 RVA: 0x000D6E75 File Offset: 0x000D5075
		private int BytesToWord(byte[] input, int inOff)
		{
			return ((int)input[inOff] << 8 & 65280) + (int)(input[inOff + 1] & byte.MaxValue);
		}

		// Token: 0x06002A85 RID: 10885 RVA: 0x000D6E8E File Offset: 0x000D508E
		private void WordToBytes(int word, byte[] outBytes, int outOff)
		{
			outBytes[outOff] = (byte)((uint)word >> 8);
			outBytes[outOff + 1] = (byte)word;
		}

		// Token: 0x06002A86 RID: 10886 RVA: 0x000D6EA0 File Offset: 0x000D50A0
		private int Mul(int x, int y)
		{
			if (x == 0)
			{
				x = IdeaEngine.BASE - y;
			}
			else if (y == 0)
			{
				x = IdeaEngine.BASE - x;
			}
			else
			{
				int num = x * y;
				y = (num & IdeaEngine.MASK);
				x = (int)((uint)num >> 16);
				x = y - x + ((y < x) ? 1 : 0);
			}
			return x & IdeaEngine.MASK;
		}

		// Token: 0x06002A87 RID: 10887 RVA: 0x000D6EF0 File Offset: 0x000D50F0
		private void IdeaFunc(int[] workingKey, byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = 0;
			int num2 = this.BytesToWord(input, inOff);
			int num3 = this.BytesToWord(input, inOff + 2);
			int num4 = this.BytesToWord(input, inOff + 4);
			int num5 = this.BytesToWord(input, inOff + 6);
			for (int i = 0; i < 8; i++)
			{
				num2 = this.Mul(num2, workingKey[num++]);
				num3 += workingKey[num++];
				num3 &= IdeaEngine.MASK;
				num4 += workingKey[num++];
				num4 &= IdeaEngine.MASK;
				num5 = this.Mul(num5, workingKey[num++]);
				int num6 = num3;
				int num7 = num4;
				num4 ^= num2;
				num3 ^= num5;
				num4 = this.Mul(num4, workingKey[num++]);
				num3 += num4;
				num3 &= IdeaEngine.MASK;
				num3 = this.Mul(num3, workingKey[num++]);
				num4 += num3;
				num4 &= IdeaEngine.MASK;
				num2 ^= num3;
				num5 ^= num4;
				num3 ^= num7;
				num4 ^= num6;
			}
			this.WordToBytes(this.Mul(num2, workingKey[num++]), outBytes, outOff);
			this.WordToBytes(num4 + workingKey[num++], outBytes, outOff + 2);
			this.WordToBytes(num3 + workingKey[num++], outBytes, outOff + 4);
			this.WordToBytes(this.Mul(num5, workingKey[num]), outBytes, outOff + 6);
		}

		// Token: 0x06002A88 RID: 10888 RVA: 0x000D7044 File Offset: 0x000D5244
		private int[] ExpandKey(byte[] uKey)
		{
			int[] array = new int[52];
			if (uKey.Length < 16)
			{
				byte[] array2 = new byte[16];
				Array.Copy(uKey, 0, array2, array2.Length - uKey.Length, uKey.Length);
				uKey = array2;
			}
			for (int i = 0; i < 8; i++)
			{
				array[i] = this.BytesToWord(uKey, i * 2);
			}
			for (int j = 8; j < 52; j++)
			{
				if ((j & 7) < 6)
				{
					array[j] = (((array[j - 7] & 127) << 9 | array[j - 6] >> 7) & IdeaEngine.MASK);
				}
				else if ((j & 7) == 6)
				{
					array[j] = (((array[j - 7] & 127) << 9 | array[j - 14] >> 7) & IdeaEngine.MASK);
				}
				else
				{
					array[j] = (((array[j - 15] & 127) << 9 | array[j - 14] >> 7) & IdeaEngine.MASK);
				}
			}
			return array;
		}

		// Token: 0x06002A89 RID: 10889 RVA: 0x000D710C File Offset: 0x000D530C
		private int MulInv(int x)
		{
			if (x < 2)
			{
				return x;
			}
			int num = 1;
			int num2 = IdeaEngine.BASE / x;
			int num3 = IdeaEngine.BASE % x;
			while (num3 != 1)
			{
				int num4 = x / num3;
				x %= num3;
				num = (num + num2 * num4 & IdeaEngine.MASK);
				if (x == 1)
				{
					return num;
				}
				num4 = num3 / x;
				num3 %= x;
				num2 = (num2 + num * num4 & IdeaEngine.MASK);
			}
			return 1 - num2 & IdeaEngine.MASK;
		}

		// Token: 0x06002A8A RID: 10890 RVA: 0x000D716F File Offset: 0x000D536F
		private int AddInv(int x)
		{
			return 0 - x & IdeaEngine.MASK;
		}

		// Token: 0x06002A8B RID: 10891 RVA: 0x000D717C File Offset: 0x000D537C
		private int[] InvertKey(int[] inKey)
		{
			int num = 52;
			int[] array = new int[52];
			int num2 = 0;
			int num3 = this.MulInv(inKey[num2++]);
			int num4 = this.AddInv(inKey[num2++]);
			int num5 = this.AddInv(inKey[num2++]);
			int num6 = this.MulInv(inKey[num2++]);
			array[--num] = num6;
			array[--num] = num5;
			array[--num] = num4;
			array[--num] = num3;
			for (int i = 1; i < 8; i++)
			{
				num3 = inKey[num2++];
				num4 = inKey[num2++];
				array[--num] = num4;
				array[--num] = num3;
				num3 = this.MulInv(inKey[num2++]);
				num4 = this.AddInv(inKey[num2++]);
				num5 = this.AddInv(inKey[num2++]);
				num6 = this.MulInv(inKey[num2++]);
				array[--num] = num6;
				array[--num] = num4;
				array[--num] = num5;
				array[--num] = num3;
			}
			num3 = inKey[num2++];
			num4 = inKey[num2++];
			array[--num] = num4;
			array[--num] = num3;
			num3 = this.MulInv(inKey[num2++]);
			num4 = this.AddInv(inKey[num2++]);
			num5 = this.AddInv(inKey[num2++]);
			num6 = this.MulInv(inKey[num2]);
			array[--num] = num6;
			array[--num] = num5;
			array[--num] = num4;
			array[num - 1] = num3;
			return array;
		}

		// Token: 0x06002A8C RID: 10892 RVA: 0x000D7344 File Offset: 0x000D5544
		private int[] GenerateWorkingKey(bool forEncryption, byte[] userKey)
		{
			if (forEncryption)
			{
				return this.ExpandKey(userKey);
			}
			return this.InvertKey(this.ExpandKey(userKey));
		}

		// Token: 0x04001C0D RID: 7181
		private const int BLOCK_SIZE = 8;

		// Token: 0x04001C0E RID: 7182
		private int[] workingKey;

		// Token: 0x04001C0F RID: 7183
		private static readonly int MASK = 65535;

		// Token: 0x04001C10 RID: 7184
		private static readonly int BASE = 65537;
	}
}
