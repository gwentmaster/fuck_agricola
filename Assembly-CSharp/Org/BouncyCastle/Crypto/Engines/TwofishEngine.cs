﻿using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x020004A3 RID: 1187
	public sealed class TwofishEngine : IBlockCipher
	{
		// Token: 0x06002B80 RID: 11136 RVA: 0x000DF2B4 File Offset: 0x000DD4B4
		public TwofishEngine()
		{
			int[] array = new int[2];
			int[] array2 = new int[2];
			int[] array3 = new int[2];
			for (int i = 0; i < 256; i++)
			{
				int num = (int)(TwofishEngine.P[0, i] & byte.MaxValue);
				array[0] = num;
				array2[0] = (this.Mx_X(num) & 255);
				array3[0] = (this.Mx_Y(num) & 255);
				num = (int)(TwofishEngine.P[1, i] & byte.MaxValue);
				array[1] = num;
				array2[1] = (this.Mx_X(num) & 255);
				array3[1] = (this.Mx_Y(num) & 255);
				this.gMDS0[i] = (array[1] | array2[1] << 8 | array3[1] << 16 | array3[1] << 24);
				this.gMDS1[i] = (array3[0] | array3[0] << 8 | array2[0] << 16 | array[0] << 24);
				this.gMDS2[i] = (array2[1] | array3[1] << 8 | array[1] << 16 | array3[1] << 24);
				this.gMDS3[i] = (array2[0] | array[0] << 8 | array3[0] << 16 | array2[0] << 24);
			}
		}

		// Token: 0x06002B81 RID: 11137 RVA: 0x000DF428 File Offset: 0x000DD628
		public void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to Twofish init - " + Platform.GetTypeName(parameters));
			}
			this.encrypting = forEncryption;
			this.workingKey = ((KeyParameter)parameters).GetKey();
			this.k64Cnt = this.workingKey.Length / 8;
			this.SetKey(this.workingKey);
		}

		// Token: 0x1700050D RID: 1293
		// (get) Token: 0x06002B82 RID: 11138 RVA: 0x000DF487 File Offset: 0x000DD687
		public string AlgorithmName
		{
			get
			{
				return "Twofish";
			}
		}

		// Token: 0x1700050E RID: 1294
		// (get) Token: 0x06002B83 RID: 11139 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002B84 RID: 11140 RVA: 0x000DF490 File Offset: 0x000DD690
		public int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("Twofish not initialised");
			}
			Check.DataLength(input, inOff, 16, "input buffer too short");
			Check.OutputLength(output, outOff, 16, "output buffer too short");
			if (this.encrypting)
			{
				this.EncryptBlock(input, inOff, output, outOff);
			}
			else
			{
				this.DecryptBlock(input, inOff, output, outOff);
			}
			return 16;
		}

		// Token: 0x06002B85 RID: 11141 RVA: 0x000DF4EF File Offset: 0x000DD6EF
		public void Reset()
		{
			if (this.workingKey != null)
			{
				this.SetKey(this.workingKey);
			}
		}

		// Token: 0x06002B86 RID: 11142 RVA: 0x000C8990 File Offset: 0x000C6B90
		public int GetBlockSize()
		{
			return 16;
		}

		// Token: 0x06002B87 RID: 11143 RVA: 0x000DF508 File Offset: 0x000DD708
		private void SetKey(byte[] key)
		{
			int[] array = new int[4];
			int[] array2 = new int[4];
			int[] array3 = new int[4];
			this.gSubKeys = new int[40];
			if (this.k64Cnt < 1)
			{
				throw new ArgumentException("Key size less than 64 bits");
			}
			if (this.k64Cnt > 4)
			{
				throw new ArgumentException("Key size larger than 256 bits");
			}
			for (int i = 0; i < this.k64Cnt; i++)
			{
				int num = i * 8;
				array[i] = this.BytesTo32Bits(key, num);
				array2[i] = this.BytesTo32Bits(key, num + 4);
				array3[this.k64Cnt - 1 - i] = this.RS_MDS_Encode(array[i], array2[i]);
			}
			for (int j = 0; j < 20; j++)
			{
				int num2 = j * 33686018;
				int num3 = this.F32(num2, array);
				int num4 = this.F32(num2 + 16843009, array2);
				num4 = (num4 << 8 | (int)((uint)num4 >> 24));
				num3 += num4;
				this.gSubKeys[j * 2] = num3;
				num3 += num4;
				this.gSubKeys[j * 2 + 1] = (num3 << 9 | (int)((uint)num3 >> 23));
			}
			int x = array3[0];
			int x2 = array3[1];
			int x3 = array3[2];
			int x4 = array3[3];
			this.gSBox = new int[1024];
			int k = 0;
			while (k < 256)
			{
				int num8;
				int num7;
				int num6;
				int num5 = num6 = (num7 = (num8 = k));
				switch (this.k64Cnt & 3)
				{
				case 0:
					num6 = ((int)(TwofishEngine.P[1, num6] & byte.MaxValue) ^ this.M_b0(x4));
					num5 = ((int)(TwofishEngine.P[0, num5] & byte.MaxValue) ^ this.M_b1(x4));
					num7 = ((int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x4));
					num8 = ((int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x4));
					goto IL_2B4;
				case 1:
					this.gSBox[k * 2] = this.gMDS0[(int)(TwofishEngine.P[0, num6] & byte.MaxValue) ^ this.M_b0(x)];
					this.gSBox[k * 2 + 1] = this.gMDS1[(int)(TwofishEngine.P[0, num5] & byte.MaxValue) ^ this.M_b1(x)];
					this.gSBox[k * 2 + 512] = this.gMDS2[(int)(TwofishEngine.P[1, num7] & byte.MaxValue) ^ this.M_b2(x)];
					this.gSBox[k * 2 + 513] = this.gMDS3[(int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x)];
					break;
				case 2:
					goto IL_32C;
				case 3:
					goto IL_2B4;
				}
				IL_45A:
				k++;
				continue;
				IL_32C:
				this.gSBox[k * 2] = this.gMDS0[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[0, num6] & byte.MaxValue) ^ this.M_b0(x2)] & byte.MaxValue) ^ this.M_b0(x)];
				this.gSBox[k * 2 + 1] = this.gMDS1[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[1, num5] & byte.MaxValue) ^ this.M_b1(x2)] & byte.MaxValue) ^ this.M_b1(x)];
				this.gSBox[k * 2 + 512] = this.gMDS2[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x2)] & byte.MaxValue) ^ this.M_b2(x)];
				this.gSBox[k * 2 + 513] = this.gMDS3[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[1, num8] & byte.MaxValue) ^ this.M_b3(x2)] & byte.MaxValue) ^ this.M_b3(x)];
				goto IL_45A;
				IL_2B4:
				num6 = ((int)(TwofishEngine.P[1, num6] & byte.MaxValue) ^ this.M_b0(x3));
				num5 = ((int)(TwofishEngine.P[1, num5] & byte.MaxValue) ^ this.M_b1(x3));
				num7 = ((int)(TwofishEngine.P[0, num7] & byte.MaxValue) ^ this.M_b2(x3));
				num8 = ((int)(TwofishEngine.P[0, num8] & byte.MaxValue) ^ this.M_b3(x3));
				goto IL_32C;
			}
		}

		// Token: 0x06002B88 RID: 11144 RVA: 0x000DF984 File Offset: 0x000DDB84
		private void EncryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
		{
			int num = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[0];
			int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[1];
			int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[2];
			int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[3];
			int num5 = 8;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = this.Fe32_0(num);
				int num7 = this.Fe32_3(num2);
				num3 ^= num6 + num7 + this.gSubKeys[num5++];
				num3 = (int)((uint)num3 >> 1 | (uint)((uint)num3 << 31));
				num4 = ((num4 << 1 | (int)((uint)num4 >> 31)) ^ num6 + 2 * num7 + this.gSubKeys[num5++]);
				num6 = this.Fe32_0(num3);
				num7 = this.Fe32_3(num4);
				num ^= num6 + num7 + this.gSubKeys[num5++];
				num = (int)((uint)num >> 1 | (uint)((uint)num << 31));
				num2 = ((num2 << 1 | (int)((uint)num2 >> 31)) ^ num6 + 2 * num7 + this.gSubKeys[num5++]);
			}
			this.Bits32ToBytes(num3 ^ this.gSubKeys[4], dst, dstIndex);
			this.Bits32ToBytes(num4 ^ this.gSubKeys[5], dst, dstIndex + 4);
			this.Bits32ToBytes(num ^ this.gSubKeys[6], dst, dstIndex + 8);
			this.Bits32ToBytes(num2 ^ this.gSubKeys[7], dst, dstIndex + 12);
		}

		// Token: 0x06002B89 RID: 11145 RVA: 0x000DFAF4 File Offset: 0x000DDCF4
		private void DecryptBlock(byte[] src, int srcIndex, byte[] dst, int dstIndex)
		{
			int num = this.BytesTo32Bits(src, srcIndex) ^ this.gSubKeys[4];
			int num2 = this.BytesTo32Bits(src, srcIndex + 4) ^ this.gSubKeys[5];
			int num3 = this.BytesTo32Bits(src, srcIndex + 8) ^ this.gSubKeys[6];
			int num4 = this.BytesTo32Bits(src, srcIndex + 12) ^ this.gSubKeys[7];
			int num5 = 39;
			for (int i = 0; i < 16; i += 2)
			{
				int num6 = this.Fe32_0(num);
				int num7 = this.Fe32_3(num2);
				num4 ^= num6 + 2 * num7 + this.gSubKeys[num5--];
				num3 = ((num3 << 1 | (int)((uint)num3 >> 31)) ^ num6 + num7 + this.gSubKeys[num5--]);
				num4 = (int)((uint)num4 >> 1 | (uint)((uint)num4 << 31));
				num6 = this.Fe32_0(num3);
				num7 = this.Fe32_3(num4);
				num2 ^= num6 + 2 * num7 + this.gSubKeys[num5--];
				num = ((num << 1 | (int)((uint)num >> 31)) ^ num6 + num7 + this.gSubKeys[num5--]);
				num2 = (int)((uint)num2 >> 1 | (uint)((uint)num2 << 31));
			}
			this.Bits32ToBytes(num3 ^ this.gSubKeys[0], dst, dstIndex);
			this.Bits32ToBytes(num4 ^ this.gSubKeys[1], dst, dstIndex + 4);
			this.Bits32ToBytes(num ^ this.gSubKeys[2], dst, dstIndex + 8);
			this.Bits32ToBytes(num2 ^ this.gSubKeys[3], dst, dstIndex + 12);
		}

		// Token: 0x06002B8A RID: 11146 RVA: 0x000DFC64 File Offset: 0x000DDE64
		private int F32(int x, int[] k32)
		{
			int num = this.M_b0(x);
			int num2 = this.M_b1(x);
			int num3 = this.M_b2(x);
			int num4 = this.M_b3(x);
			int x2 = k32[0];
			int x3 = k32[1];
			int x4 = k32[2];
			int x5 = k32[3];
			int result = 0;
			switch (this.k64Cnt & 3)
			{
			case 0:
				num = ((int)(TwofishEngine.P[1, num] & byte.MaxValue) ^ this.M_b0(x5));
				num2 = ((int)(TwofishEngine.P[0, num2] & byte.MaxValue) ^ this.M_b1(x5));
				num3 = ((int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x5));
				num4 = ((int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x5));
				break;
			case 1:
				return this.gMDS0[(int)(TwofishEngine.P[0, num] & byte.MaxValue) ^ this.M_b0(x2)] ^ this.gMDS1[(int)(TwofishEngine.P[0, num2] & byte.MaxValue) ^ this.M_b1(x2)] ^ this.gMDS2[(int)(TwofishEngine.P[1, num3] & byte.MaxValue) ^ this.M_b2(x2)] ^ this.gMDS3[(int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x2)];
			case 2:
				goto IL_1CF;
			case 3:
				break;
			default:
				return result;
			}
			num = ((int)(TwofishEngine.P[1, num] & byte.MaxValue) ^ this.M_b0(x4));
			num2 = ((int)(TwofishEngine.P[1, num2] & byte.MaxValue) ^ this.M_b1(x4));
			num3 = ((int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x4));
			num4 = ((int)(TwofishEngine.P[0, num4] & byte.MaxValue) ^ this.M_b3(x4));
			IL_1CF:
			result = (this.gMDS0[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[0, num] & byte.MaxValue) ^ this.M_b0(x3)] & byte.MaxValue) ^ this.M_b0(x2)] ^ this.gMDS1[(int)(TwofishEngine.P[0, (int)(TwofishEngine.P[1, num2] & byte.MaxValue) ^ this.M_b1(x3)] & byte.MaxValue) ^ this.M_b1(x2)] ^ this.gMDS2[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[0, num3] & byte.MaxValue) ^ this.M_b2(x3)] & byte.MaxValue) ^ this.M_b2(x2)] ^ this.gMDS3[(int)(TwofishEngine.P[1, (int)(TwofishEngine.P[1, num4] & byte.MaxValue) ^ this.M_b3(x3)] & byte.MaxValue) ^ this.M_b3(x2)]);
			return result;
		}

		// Token: 0x06002B8B RID: 11147 RVA: 0x000DFF38 File Offset: 0x000DE138
		private int RS_MDS_Encode(int k0, int k1)
		{
			int num = k1;
			for (int i = 0; i < 4; i++)
			{
				num = this.RS_rem(num);
			}
			num ^= k0;
			for (int j = 0; j < 4; j++)
			{
				num = this.RS_rem(num);
			}
			return num;
		}

		// Token: 0x06002B8C RID: 11148 RVA: 0x000DFF74 File Offset: 0x000DE174
		private int RS_rem(int x)
		{
			int num = (int)((uint)x >> 24 & 255U);
			int num2 = (num << 1 ^ (((num & 128) != 0) ? 333 : 0)) & 255;
			int num3 = (int)((uint)num >> 1 ^ (((num & 1) != 0) ? 166U : 0U) ^ (uint)num2);
			return x << 8 ^ num3 << 24 ^ num2 << 16 ^ num3 << 8 ^ num;
		}

		// Token: 0x06002B8D RID: 11149 RVA: 0x000DFFCF File Offset: 0x000DE1CF
		private int LFSR1(int x)
		{
			return x >> 1 ^ (((x & 1) != 0) ? 180 : 0);
		}

		// Token: 0x06002B8E RID: 11150 RVA: 0x000DFFE2 File Offset: 0x000DE1E2
		private int LFSR2(int x)
		{
			return x >> 2 ^ (((x & 2) != 0) ? 180 : 0) ^ (((x & 1) != 0) ? 90 : 0);
		}

		// Token: 0x06002B8F RID: 11151 RVA: 0x000E0000 File Offset: 0x000DE200
		private int Mx_X(int x)
		{
			return x ^ this.LFSR2(x);
		}

		// Token: 0x06002B90 RID: 11152 RVA: 0x000E000B File Offset: 0x000DE20B
		private int Mx_Y(int x)
		{
			return x ^ this.LFSR1(x) ^ this.LFSR2(x);
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x000E001E File Offset: 0x000DE21E
		private int M_b0(int x)
		{
			return x & 255;
		}

		// Token: 0x06002B92 RID: 11154 RVA: 0x000E0027 File Offset: 0x000DE227
		private int M_b1(int x)
		{
			return (int)((uint)x >> 8 & 255U);
		}

		// Token: 0x06002B93 RID: 11155 RVA: 0x000E0032 File Offset: 0x000DE232
		private int M_b2(int x)
		{
			return (int)((uint)x >> 16 & 255U);
		}

		// Token: 0x06002B94 RID: 11156 RVA: 0x000E003E File Offset: 0x000DE23E
		private int M_b3(int x)
		{
			return (int)((uint)x >> 24 & 255U);
		}

		// Token: 0x06002B95 RID: 11157 RVA: 0x000E004C File Offset: 0x000DE24C
		private int Fe32_0(int x)
		{
			return this.gSBox[2 * (x & 255)] ^ this.gSBox[(int)(1U + 2U * ((uint)x >> 8 & 255U))] ^ this.gSBox[(int)(512U + 2U * ((uint)x >> 16 & 255U))] ^ this.gSBox[(int)(513U + 2U * ((uint)x >> 24 & 255U))];
		}

		// Token: 0x06002B96 RID: 11158 RVA: 0x000E00B4 File Offset: 0x000DE2B4
		private int Fe32_3(int x)
		{
			return this.gSBox[(int)(2U * ((uint)x >> 24 & 255U))] ^ this.gSBox[1 + 2 * (x & 255)] ^ this.gSBox[(int)(512U + 2U * ((uint)x >> 8 & 255U))] ^ this.gSBox[(int)(513U + 2U * ((uint)x >> 16 & 255U))];
		}

		// Token: 0x06002B97 RID: 11159 RVA: 0x000D906D File Offset: 0x000D726D
		private int BytesTo32Bits(byte[] b, int p)
		{
			return (int)(b[p] & byte.MaxValue) | (int)(b[p + 1] & byte.MaxValue) << 8 | (int)(b[p + 2] & byte.MaxValue) << 16 | (int)(b[p + 3] & byte.MaxValue) << 24;
		}

		// Token: 0x06002B98 RID: 11160 RVA: 0x000D90A4 File Offset: 0x000D72A4
		private void Bits32ToBytes(int inData, byte[] b, int offset)
		{
			b[offset] = (byte)inData;
			b[offset + 1] = (byte)(inData >> 8);
			b[offset + 2] = (byte)(inData >> 16);
			b[offset + 3] = (byte)(inData >> 24);
		}

		// Token: 0x04001C9B RID: 7323
		private static readonly byte[,] P = new byte[,]
		{
			{
				169,
				103,
				179,
				232,
				4,
				253,
				163,
				118,
				154,
				146,
				128,
				120,
				228,
				221,
				209,
				56,
				13,
				198,
				53,
				152,
				24,
				247,
				236,
				108,
				67,
				117,
				55,
				38,
				250,
				19,
				148,
				72,
				242,
				208,
				139,
				48,
				132,
				84,
				223,
				35,
				25,
				91,
				61,
				89,
				243,
				174,
				162,
				130,
				99,
				1,
				131,
				46,
				217,
				81,
				155,
				124,
				166,
				235,
				165,
				190,
				22,
				12,
				227,
				97,
				192,
				140,
				58,
				245,
				115,
				44,
				37,
				11,
				187,
				78,
				137,
				107,
				83,
				106,
				180,
				241,
				225,
				230,
				189,
				69,
				226,
				244,
				182,
				102,
				204,
				149,
				3,
				86,
				212,
				28,
				30,
				215,
				251,
				195,
				142,
				181,
				233,
				207,
				191,
				186,
				234,
				119,
				57,
				175,
				51,
				201,
				98,
				113,
				129,
				121,
				9,
				173,
				36,
				205,
				249,
				216,
				229,
				197,
				185,
				77,
				68,
				8,
				134,
				231,
				161,
				29,
				170,
				237,
				6,
				112,
				178,
				210,
				65,
				123,
				160,
				17,
				49,
				194,
				39,
				144,
				32,
				246,
				96,
				byte.MaxValue,
				150,
				92,
				177,
				171,
				158,
				156,
				82,
				27,
				95,
				147,
				10,
				239,
				145,
				133,
				73,
				238,
				45,
				79,
				143,
				59,
				71,
				135,
				109,
				70,
				214,
				62,
				105,
				100,
				42,
				206,
				203,
				47,
				252,
				151,
				5,
				122,
				172,
				127,
				213,
				26,
				75,
				14,
				167,
				90,
				40,
				20,
				63,
				41,
				136,
				60,
				76,
				2,
				184,
				218,
				176,
				23,
				85,
				31,
				138,
				125,
				87,
				199,
				141,
				116,
				183,
				196,
				159,
				114,
				126,
				21,
				34,
				18,
				88,
				7,
				153,
				52,
				110,
				80,
				222,
				104,
				101,
				188,
				219,
				248,
				200,
				168,
				43,
				64,
				220,
				254,
				50,
				164,
				202,
				16,
				33,
				240,
				211,
				93,
				15,
				0,
				111,
				157,
				54,
				66,
				74,
				94,
				193,
				224
			},
			{
				117,
				243,
				198,
				244,
				219,
				123,
				251,
				200,
				74,
				211,
				230,
				107,
				69,
				125,
				232,
				75,
				214,
				50,
				216,
				253,
				55,
				113,
				241,
				225,
				48,
				15,
				248,
				27,
				135,
				250,
				6,
				63,
				94,
				186,
				174,
				91,
				138,
				0,
				188,
				157,
				109,
				193,
				177,
				14,
				128,
				93,
				210,
				213,
				160,
				132,
				7,
				20,
				181,
				144,
				44,
				163,
				178,
				115,
				76,
				84,
				146,
				116,
				54,
				81,
				56,
				176,
				189,
				90,
				252,
				96,
				98,
				150,
				108,
				66,
				247,
				16,
				124,
				40,
				39,
				140,
				19,
				149,
				156,
				199,
				36,
				70,
				59,
				112,
				202,
				227,
				133,
				203,
				17,
				208,
				147,
				184,
				166,
				131,
				32,
				byte.MaxValue,
				159,
				119,
				195,
				204,
				3,
				111,
				8,
				191,
				64,
				231,
				43,
				226,
				121,
				12,
				170,
				130,
				65,
				58,
				234,
				185,
				228,
				154,
				164,
				151,
				126,
				218,
				122,
				23,
				102,
				148,
				161,
				29,
				61,
				240,
				222,
				179,
				11,
				114,
				167,
				28,
				239,
				209,
				83,
				62,
				143,
				51,
				38,
				95,
				236,
				118,
				42,
				73,
				129,
				136,
				238,
				33,
				196,
				26,
				235,
				217,
				197,
				57,
				153,
				205,
				173,
				49,
				139,
				1,
				24,
				35,
				221,
				31,
				78,
				45,
				249,
				72,
				79,
				242,
				101,
				142,
				120,
				92,
				88,
				25,
				141,
				229,
				152,
				87,
				103,
				127,
				5,
				100,
				175,
				99,
				182,
				254,
				245,
				183,
				60,
				165,
				206,
				233,
				104,
				68,
				224,
				77,
				67,
				105,
				41,
				46,
				172,
				21,
				89,
				168,
				10,
				158,
				110,
				71,
				223,
				52,
				53,
				106,
				207,
				220,
				34,
				201,
				192,
				155,
				137,
				212,
				237,
				171,
				18,
				162,
				13,
				82,
				187,
				2,
				47,
				169,
				215,
				97,
				30,
				180,
				80,
				4,
				246,
				194,
				22,
				37,
				134,
				86,
				85,
				9,
				190,
				145
			}
		};

		// Token: 0x04001C9C RID: 7324
		private const int P_00 = 1;

		// Token: 0x04001C9D RID: 7325
		private const int P_01 = 0;

		// Token: 0x04001C9E RID: 7326
		private const int P_02 = 0;

		// Token: 0x04001C9F RID: 7327
		private const int P_03 = 1;

		// Token: 0x04001CA0 RID: 7328
		private const int P_04 = 1;

		// Token: 0x04001CA1 RID: 7329
		private const int P_10 = 0;

		// Token: 0x04001CA2 RID: 7330
		private const int P_11 = 0;

		// Token: 0x04001CA3 RID: 7331
		private const int P_12 = 1;

		// Token: 0x04001CA4 RID: 7332
		private const int P_13 = 1;

		// Token: 0x04001CA5 RID: 7333
		private const int P_14 = 0;

		// Token: 0x04001CA6 RID: 7334
		private const int P_20 = 1;

		// Token: 0x04001CA7 RID: 7335
		private const int P_21 = 1;

		// Token: 0x04001CA8 RID: 7336
		private const int P_22 = 0;

		// Token: 0x04001CA9 RID: 7337
		private const int P_23 = 0;

		// Token: 0x04001CAA RID: 7338
		private const int P_24 = 0;

		// Token: 0x04001CAB RID: 7339
		private const int P_30 = 0;

		// Token: 0x04001CAC RID: 7340
		private const int P_31 = 1;

		// Token: 0x04001CAD RID: 7341
		private const int P_32 = 1;

		// Token: 0x04001CAE RID: 7342
		private const int P_33 = 0;

		// Token: 0x04001CAF RID: 7343
		private const int P_34 = 1;

		// Token: 0x04001CB0 RID: 7344
		private const int GF256_FDBK = 361;

		// Token: 0x04001CB1 RID: 7345
		private const int GF256_FDBK_2 = 180;

		// Token: 0x04001CB2 RID: 7346
		private const int GF256_FDBK_4 = 90;

		// Token: 0x04001CB3 RID: 7347
		private const int RS_GF_FDBK = 333;

		// Token: 0x04001CB4 RID: 7348
		private const int ROUNDS = 16;

		// Token: 0x04001CB5 RID: 7349
		private const int MAX_ROUNDS = 16;

		// Token: 0x04001CB6 RID: 7350
		private const int BLOCK_SIZE = 16;

		// Token: 0x04001CB7 RID: 7351
		private const int MAX_KEY_BITS = 256;

		// Token: 0x04001CB8 RID: 7352
		private const int INPUT_WHITEN = 0;

		// Token: 0x04001CB9 RID: 7353
		private const int OUTPUT_WHITEN = 4;

		// Token: 0x04001CBA RID: 7354
		private const int ROUND_SUBKEYS = 8;

		// Token: 0x04001CBB RID: 7355
		private const int TOTAL_SUBKEYS = 40;

		// Token: 0x04001CBC RID: 7356
		private const int SK_STEP = 33686018;

		// Token: 0x04001CBD RID: 7357
		private const int SK_BUMP = 16843009;

		// Token: 0x04001CBE RID: 7358
		private const int SK_ROTL = 9;

		// Token: 0x04001CBF RID: 7359
		private bool encrypting;

		// Token: 0x04001CC0 RID: 7360
		private int[] gMDS0 = new int[256];

		// Token: 0x04001CC1 RID: 7361
		private int[] gMDS1 = new int[256];

		// Token: 0x04001CC2 RID: 7362
		private int[] gMDS2 = new int[256];

		// Token: 0x04001CC3 RID: 7363
		private int[] gMDS3 = new int[256];

		// Token: 0x04001CC4 RID: 7364
		private int[] gSubKeys;

		// Token: 0x04001CC5 RID: 7365
		private int[] gSBox;

		// Token: 0x04001CC6 RID: 7366
		private int k64Cnt;

		// Token: 0x04001CC7 RID: 7367
		private byte[] workingKey;
	}
}