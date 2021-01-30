using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000491 RID: 1169
	public class RC2Engine : IBlockCipher
	{
		// Token: 0x06002AA5 RID: 10917 RVA: 0x000D7CF4 File Offset: 0x000D5EF4
		private int[] GenerateWorkingKey(byte[] key, int bits)
		{
			int[] array = new int[128];
			for (int num = 0; num != key.Length; num++)
			{
				array[num] = (int)(key[num] & byte.MaxValue);
			}
			int num2 = key.Length;
			int num4;
			if (num2 < 128)
			{
				int num3 = 0;
				num4 = array[num2 - 1];
				do
				{
					num4 = (int)(RC2Engine.piTable[num4 + array[num3++] & 255] & byte.MaxValue);
					array[num2++] = num4;
				}
				while (num2 < 128);
			}
			num2 = bits + 7 >> 3;
			num4 = (int)(RC2Engine.piTable[array[128 - num2] & 255 >> (7 & -bits)] & byte.MaxValue);
			array[128 - num2] = num4;
			for (int i = 128 - num2 - 1; i >= 0; i--)
			{
				num4 = (int)(RC2Engine.piTable[num4 ^ array[i + num2]] & byte.MaxValue);
				array[i] = num4;
			}
			int[] array2 = new int[64];
			for (int num5 = 0; num5 != array2.Length; num5++)
			{
				array2[num5] = array[2 * num5] + (array[2 * num5 + 1] << 8);
			}
			return array2;
		}

		// Token: 0x06002AA6 RID: 10918 RVA: 0x000D7E08 File Offset: 0x000D6008
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			this.encrypting = forEncryption;
			if (parameters is RC2Parameters)
			{
				RC2Parameters rc2Parameters = (RC2Parameters)parameters;
				this.workingKey = this.GenerateWorkingKey(rc2Parameters.GetKey(), rc2Parameters.EffectiveKeyBits);
				return;
			}
			if (parameters is KeyParameter)
			{
				byte[] key = ((KeyParameter)parameters).GetKey();
				this.workingKey = this.GenerateWorkingKey(key, key.Length * 8);
				return;
			}
			throw new ArgumentException("invalid parameter passed to RC2 init - " + Platform.GetTypeName(parameters));
		}

		// Token: 0x06002AA7 RID: 10919 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x170004F4 RID: 1268
		// (get) Token: 0x06002AA8 RID: 10920 RVA: 0x000D7E80 File Offset: 0x000D6080
		public virtual string AlgorithmName
		{
			get
			{
				return "RC2";
			}
		}

		// Token: 0x170004F5 RID: 1269
		// (get) Token: 0x06002AA9 RID: 10921 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002AAA RID: 10922 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002AAB RID: 10923 RVA: 0x000D7E88 File Offset: 0x000D6088
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("RC2 engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			if (this.encrypting)
			{
				this.EncryptBlock(input, inOff, output, outOff);
			}
			else
			{
				this.DecryptBlock(input, inOff, output, outOff);
			}
			return 8;
		}

		// Token: 0x06002AAC RID: 10924 RVA: 0x000D7EE4 File Offset: 0x000D60E4
		private int RotateWordLeft(int x, int y)
		{
			x &= 65535;
			return x << y | x >> 16 - y;
		}

		// Token: 0x06002AAD RID: 10925 RVA: 0x000D7F00 File Offset: 0x000D6100
		private void EncryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = ((int)(input[inOff + 7] & byte.MaxValue) << 8) + (int)(input[inOff + 6] & byte.MaxValue);
			int num2 = ((int)(input[inOff + 5] & byte.MaxValue) << 8) + (int)(input[inOff + 4] & byte.MaxValue);
			int num3 = ((int)(input[inOff + 3] & byte.MaxValue) << 8) + (int)(input[inOff + 2] & byte.MaxValue);
			int num4 = ((int)(input[inOff + 1] & byte.MaxValue) << 8) + (int)(input[inOff] & byte.MaxValue);
			for (int i = 0; i <= 16; i += 4)
			{
				num4 = this.RotateWordLeft(num4 + (num3 & ~num) + (num2 & num) + this.workingKey[i], 1);
				num3 = this.RotateWordLeft(num3 + (num2 & ~num4) + (num & num4) + this.workingKey[i + 1], 2);
				num2 = this.RotateWordLeft(num2 + (num & ~num3) + (num4 & num3) + this.workingKey[i + 2], 3);
				num = this.RotateWordLeft(num + (num4 & ~num2) + (num3 & num2) + this.workingKey[i + 3], 5);
			}
			num4 += this.workingKey[num & 63];
			num3 += this.workingKey[num4 & 63];
			num2 += this.workingKey[num3 & 63];
			num += this.workingKey[num2 & 63];
			for (int j = 20; j <= 40; j += 4)
			{
				num4 = this.RotateWordLeft(num4 + (num3 & ~num) + (num2 & num) + this.workingKey[j], 1);
				num3 = this.RotateWordLeft(num3 + (num2 & ~num4) + (num & num4) + this.workingKey[j + 1], 2);
				num2 = this.RotateWordLeft(num2 + (num & ~num3) + (num4 & num3) + this.workingKey[j + 2], 3);
				num = this.RotateWordLeft(num + (num4 & ~num2) + (num3 & num2) + this.workingKey[j + 3], 5);
			}
			num4 += this.workingKey[num & 63];
			num3 += this.workingKey[num4 & 63];
			num2 += this.workingKey[num3 & 63];
			num += this.workingKey[num2 & 63];
			for (int k = 44; k < 64; k += 4)
			{
				num4 = this.RotateWordLeft(num4 + (num3 & ~num) + (num2 & num) + this.workingKey[k], 1);
				num3 = this.RotateWordLeft(num3 + (num2 & ~num4) + (num & num4) + this.workingKey[k + 1], 2);
				num2 = this.RotateWordLeft(num2 + (num & ~num3) + (num4 & num3) + this.workingKey[k + 2], 3);
				num = this.RotateWordLeft(num + (num4 & ~num2) + (num3 & num2) + this.workingKey[k + 3], 5);
			}
			outBytes[outOff] = (byte)num4;
			outBytes[outOff + 1] = (byte)(num4 >> 8);
			outBytes[outOff + 2] = (byte)num3;
			outBytes[outOff + 3] = (byte)(num3 >> 8);
			outBytes[outOff + 4] = (byte)num2;
			outBytes[outOff + 5] = (byte)(num2 >> 8);
			outBytes[outOff + 6] = (byte)num;
			outBytes[outOff + 7] = (byte)(num >> 8);
		}

		// Token: 0x06002AAE RID: 10926 RVA: 0x000D81CC File Offset: 0x000D63CC
		private void DecryptBlock(byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			int num = ((int)(input[inOff + 7] & byte.MaxValue) << 8) + (int)(input[inOff + 6] & byte.MaxValue);
			int num2 = ((int)(input[inOff + 5] & byte.MaxValue) << 8) + (int)(input[inOff + 4] & byte.MaxValue);
			int num3 = ((int)(input[inOff + 3] & byte.MaxValue) << 8) + (int)(input[inOff + 2] & byte.MaxValue);
			int num4 = ((int)(input[inOff + 1] & byte.MaxValue) << 8) + (int)(input[inOff] & byte.MaxValue);
			for (int i = 60; i >= 44; i -= 4)
			{
				num = this.RotateWordLeft(num, 11) - ((num4 & ~num2) + (num3 & num2) + this.workingKey[i + 3]);
				num2 = this.RotateWordLeft(num2, 13) - ((num & ~num3) + (num4 & num3) + this.workingKey[i + 2]);
				num3 = this.RotateWordLeft(num3, 14) - ((num2 & ~num4) + (num & num4) + this.workingKey[i + 1]);
				num4 = this.RotateWordLeft(num4, 15) - ((num3 & ~num) + (num2 & num) + this.workingKey[i]);
			}
			num -= this.workingKey[num2 & 63];
			num2 -= this.workingKey[num3 & 63];
			num3 -= this.workingKey[num4 & 63];
			num4 -= this.workingKey[num & 63];
			for (int j = 40; j >= 20; j -= 4)
			{
				num = this.RotateWordLeft(num, 11) - ((num4 & ~num2) + (num3 & num2) + this.workingKey[j + 3]);
				num2 = this.RotateWordLeft(num2, 13) - ((num & ~num3) + (num4 & num3) + this.workingKey[j + 2]);
				num3 = this.RotateWordLeft(num3, 14) - ((num2 & ~num4) + (num & num4) + this.workingKey[j + 1]);
				num4 = this.RotateWordLeft(num4, 15) - ((num3 & ~num) + (num2 & num) + this.workingKey[j]);
			}
			num -= this.workingKey[num2 & 63];
			num2 -= this.workingKey[num3 & 63];
			num3 -= this.workingKey[num4 & 63];
			num4 -= this.workingKey[num & 63];
			for (int k = 16; k >= 0; k -= 4)
			{
				num = this.RotateWordLeft(num, 11) - ((num4 & ~num2) + (num3 & num2) + this.workingKey[k + 3]);
				num2 = this.RotateWordLeft(num2, 13) - ((num & ~num3) + (num4 & num3) + this.workingKey[k + 2]);
				num3 = this.RotateWordLeft(num3, 14) - ((num2 & ~num4) + (num & num4) + this.workingKey[k + 1]);
				num4 = this.RotateWordLeft(num4, 15) - ((num3 & ~num) + (num2 & num) + this.workingKey[k]);
			}
			outBytes[outOff] = (byte)num4;
			outBytes[outOff + 1] = (byte)(num4 >> 8);
			outBytes[outOff + 2] = (byte)num3;
			outBytes[outOff + 3] = (byte)(num3 >> 8);
			outBytes[outOff + 4] = (byte)num2;
			outBytes[outOff + 5] = (byte)(num2 >> 8);
			outBytes[outOff + 6] = (byte)num;
			outBytes[outOff + 7] = (byte)(num >> 8);
		}

		// Token: 0x04001C22 RID: 7202
		private static readonly byte[] piTable = new byte[]
		{
			217,
			120,
			249,
			196,
			25,
			221,
			181,
			237,
			40,
			233,
			253,
			121,
			74,
			160,
			216,
			157,
			198,
			126,
			55,
			131,
			43,
			118,
			83,
			142,
			98,
			76,
			100,
			136,
			68,
			139,
			251,
			162,
			23,
			154,
			89,
			245,
			135,
			179,
			79,
			19,
			97,
			69,
			109,
			141,
			9,
			129,
			125,
			50,
			189,
			143,
			64,
			235,
			134,
			183,
			123,
			11,
			240,
			149,
			33,
			34,
			92,
			107,
			78,
			130,
			84,
			214,
			101,
			147,
			206,
			96,
			178,
			28,
			115,
			86,
			192,
			20,
			167,
			140,
			241,
			220,
			18,
			117,
			202,
			31,
			59,
			190,
			228,
			209,
			66,
			61,
			212,
			48,
			163,
			60,
			182,
			38,
			111,
			191,
			14,
			218,
			70,
			105,
			7,
			87,
			39,
			242,
			29,
			155,
			188,
			148,
			67,
			3,
			248,
			17,
			199,
			246,
			144,
			239,
			62,
			231,
			6,
			195,
			213,
			47,
			200,
			102,
			30,
			215,
			8,
			232,
			234,
			222,
			128,
			82,
			238,
			247,
			132,
			170,
			114,
			172,
			53,
			77,
			106,
			42,
			150,
			26,
			210,
			113,
			90,
			21,
			73,
			116,
			75,
			159,
			208,
			94,
			4,
			24,
			164,
			236,
			194,
			224,
			65,
			110,
			15,
			81,
			203,
			204,
			36,
			145,
			175,
			80,
			161,
			244,
			112,
			57,
			153,
			124,
			58,
			133,
			35,
			184,
			180,
			122,
			252,
			2,
			54,
			91,
			37,
			85,
			151,
			49,
			45,
			93,
			250,
			152,
			227,
			138,
			146,
			174,
			5,
			223,
			41,
			16,
			103,
			108,
			186,
			201,
			211,
			0,
			230,
			207,
			225,
			158,
			168,
			44,
			99,
			22,
			1,
			63,
			88,
			226,
			137,
			169,
			13,
			56,
			52,
			27,
			171,
			51,
			byte.MaxValue,
			176,
			187,
			72,
			12,
			95,
			185,
			177,
			205,
			46,
			197,
			243,
			219,
			71,
			229,
			165,
			156,
			119,
			10,
			166,
			32,
			104,
			254,
			127,
			193,
			173
		};

		// Token: 0x04001C23 RID: 7203
		private const int BLOCK_SIZE = 8;

		// Token: 0x04001C24 RID: 7204
		private int[] workingKey;

		// Token: 0x04001C25 RID: 7205
		private bool encrypting;
	}
}
