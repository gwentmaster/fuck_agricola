using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Engines
{
	// Token: 0x02000489 RID: 1161
	public class DesEngine : IBlockCipher
	{
		// Token: 0x06002A3F RID: 10815 RVA: 0x000D5611 File Offset: 0x000D3811
		public virtual int[] GetWorkingKey()
		{
			return this.workingKey;
		}

		// Token: 0x06002A40 RID: 10816 RVA: 0x000D5619 File Offset: 0x000D3819
		public virtual void Init(bool forEncryption, ICipherParameters parameters)
		{
			if (!(parameters is KeyParameter))
			{
				throw new ArgumentException("invalid parameter passed to DES init - " + Platform.GetTypeName(parameters));
			}
			this.workingKey = DesEngine.GenerateWorkingKey(forEncryption, ((KeyParameter)parameters).GetKey());
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x06002A41 RID: 10817 RVA: 0x000D5650 File Offset: 0x000D3850
		public virtual string AlgorithmName
		{
			get
			{
				return "DES";
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x06002A42 RID: 10818 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsPartialBlockOkay
		{
			get
			{
				return false;
			}
		}

		// Token: 0x06002A43 RID: 10819 RVA: 0x000A6D40 File Offset: 0x000A4F40
		public virtual int GetBlockSize()
		{
			return 8;
		}

		// Token: 0x06002A44 RID: 10820 RVA: 0x000D5658 File Offset: 0x000D3858
		public virtual int ProcessBlock(byte[] input, int inOff, byte[] output, int outOff)
		{
			if (this.workingKey == null)
			{
				throw new InvalidOperationException("DES engine not initialised");
			}
			Check.DataLength(input, inOff, 8, "input buffer too short");
			Check.OutputLength(output, outOff, 8, "output buffer too short");
			DesEngine.DesFunc(this.workingKey, input, inOff, output, outOff);
			return 8;
		}

		// Token: 0x06002A45 RID: 10821 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Reset()
		{
		}

		// Token: 0x06002A46 RID: 10822 RVA: 0x000D56A4 File Offset: 0x000D38A4
		protected static int[] GenerateWorkingKey(bool encrypting, byte[] key)
		{
			int[] array = new int[32];
			bool[] array2 = new bool[56];
			bool[] array3 = new bool[56];
			for (int i = 0; i < 56; i++)
			{
				int num = (int)DesEngine.pc1[i];
				array2[i] = (((short)key[(int)((uint)num >> 3)] & DesEngine.bytebit[num & 7]) != 0);
			}
			for (int j = 0; j < 16; j++)
			{
				int num2;
				if (encrypting)
				{
					num2 = j << 1;
				}
				else
				{
					num2 = 15 - j << 1;
				}
				int num3 = num2 + 1;
				array[num2] = (array[num3] = 0);
				for (int k = 0; k < 28; k++)
				{
					int num4 = k + (int)DesEngine.totrot[j];
					if (num4 < 28)
					{
						array3[k] = array2[num4];
					}
					else
					{
						array3[k] = array2[num4 - 28];
					}
				}
				for (int l = 28; l < 56; l++)
				{
					int num4 = l + (int)DesEngine.totrot[j];
					if (num4 < 56)
					{
						array3[l] = array2[num4];
					}
					else
					{
						array3[l] = array2[num4 - 28];
					}
				}
				for (int m = 0; m < 24; m++)
				{
					if (array3[(int)DesEngine.pc2[m]])
					{
						array[num2] |= DesEngine.bigbyte[m];
					}
					if (array3[(int)DesEngine.pc2[m + 24]])
					{
						array[num3] |= DesEngine.bigbyte[m];
					}
				}
			}
			for (int num5 = 0; num5 != 32; num5 += 2)
			{
				int num6 = array[num5];
				int num7 = array[num5 + 1];
				array[num5] = ((num6 & 16515072) << 6 | (num6 & 4032) << 10 | (int)((uint)(num7 & 16515072) >> 10) | (int)((uint)(num7 & 4032) >> 6));
				array[num5 + 1] = ((num6 & 258048) << 12 | (num6 & 63) << 16 | (int)((uint)(num7 & 258048) >> 4) | (num7 & 63));
			}
			return array;
		}

		// Token: 0x06002A47 RID: 10823 RVA: 0x000D5878 File Offset: 0x000D3A78
		internal static void DesFunc(int[] wKey, byte[] input, int inOff, byte[] outBytes, int outOff)
		{
			uint num = Pack.BE_To_UInt32(input, inOff);
			uint num2 = Pack.BE_To_UInt32(input, inOff + 4);
			uint num3 = (num >> 4 ^ num2) & 252645135U;
			num2 ^= num3;
			num ^= num3 << 4;
			num3 = ((num >> 16 ^ num2) & 65535U);
			num2 ^= num3;
			num ^= num3 << 16;
			num3 = ((num2 >> 2 ^ num) & 858993459U);
			num ^= num3;
			num2 ^= num3 << 2;
			num3 = ((num2 >> 8 ^ num) & 16711935U);
			num ^= num3;
			num2 ^= num3 << 8;
			num2 = (num2 << 1 | num2 >> 31);
			num3 = ((num ^ num2) & 2863311530U);
			num ^= num3;
			num2 ^= num3;
			num = (num << 1 | num >> 31);
			for (int i = 0; i < 8; i++)
			{
				num3 = (num2 << 28 | num2 >> 4);
				num3 ^= (uint)wKey[i * 4];
				uint num4 = DesEngine.SP7[(int)(num3 & 63U)];
				num4 |= DesEngine.SP5[(int)(num3 >> 8 & 63U)];
				num4 |= DesEngine.SP3[(int)(num3 >> 16 & 63U)];
				num4 |= DesEngine.SP1[(int)(num3 >> 24 & 63U)];
				num3 = (num2 ^ (uint)wKey[i * 4 + 1]);
				num4 |= DesEngine.SP8[(int)(num3 & 63U)];
				num4 |= DesEngine.SP6[(int)(num3 >> 8 & 63U)];
				num4 |= DesEngine.SP4[(int)(num3 >> 16 & 63U)];
				num4 |= DesEngine.SP2[(int)(num3 >> 24 & 63U)];
				num ^= num4;
				num3 = (num << 28 | num >> 4);
				num3 ^= (uint)wKey[i * 4 + 2];
				num4 = DesEngine.SP7[(int)(num3 & 63U)];
				num4 |= DesEngine.SP5[(int)(num3 >> 8 & 63U)];
				num4 |= DesEngine.SP3[(int)(num3 >> 16 & 63U)];
				num4 |= DesEngine.SP1[(int)(num3 >> 24 & 63U)];
				num3 = (num ^ (uint)wKey[i * 4 + 3]);
				num4 |= DesEngine.SP8[(int)(num3 & 63U)];
				num4 |= DesEngine.SP6[(int)(num3 >> 8 & 63U)];
				num4 |= DesEngine.SP4[(int)(num3 >> 16 & 63U)];
				num4 |= DesEngine.SP2[(int)(num3 >> 24 & 63U)];
				num2 ^= num4;
			}
			num2 = (num2 << 31 | num2 >> 1);
			num3 = ((num ^ num2) & 2863311530U);
			num ^= num3;
			num2 ^= num3;
			num = (num << 31 | num >> 1);
			num3 = ((num >> 8 ^ num2) & 16711935U);
			num2 ^= num3;
			num ^= num3 << 8;
			num3 = ((num >> 2 ^ num2) & 858993459U);
			num2 ^= num3;
			num ^= num3 << 2;
			num3 = ((num2 >> 16 ^ num) & 65535U);
			num ^= num3;
			num2 ^= num3 << 16;
			num3 = ((num2 >> 4 ^ num) & 252645135U);
			num ^= num3;
			num2 ^= num3 << 4;
			Pack.UInt32_To_BE(num2, outBytes, outOff);
			Pack.UInt32_To_BE(num, outBytes, outOff + 4);
		}

		// Token: 0x04001BDD RID: 7133
		internal const int BLOCK_SIZE = 8;

		// Token: 0x04001BDE RID: 7134
		private int[] workingKey;

		// Token: 0x04001BDF RID: 7135
		private static readonly short[] bytebit = new short[]
		{
			128,
			64,
			32,
			16,
			8,
			4,
			2,
			1
		};

		// Token: 0x04001BE0 RID: 7136
		private static readonly int[] bigbyte = new int[]
		{
			8388608,
			4194304,
			2097152,
			1048576,
			524288,
			262144,
			131072,
			65536,
			32768,
			16384,
			8192,
			4096,
			2048,
			1024,
			512,
			256,
			128,
			64,
			32,
			16,
			8,
			4,
			2,
			1
		};

		// Token: 0x04001BE1 RID: 7137
		private static readonly byte[] pc1 = new byte[]
		{
			56,
			48,
			40,
			32,
			24,
			16,
			8,
			0,
			57,
			49,
			41,
			33,
			25,
			17,
			9,
			1,
			58,
			50,
			42,
			34,
			26,
			18,
			10,
			2,
			59,
			51,
			43,
			35,
			62,
			54,
			46,
			38,
			30,
			22,
			14,
			6,
			61,
			53,
			45,
			37,
			29,
			21,
			13,
			5,
			60,
			52,
			44,
			36,
			28,
			20,
			12,
			4,
			27,
			19,
			11,
			3
		};

		// Token: 0x04001BE2 RID: 7138
		private static readonly byte[] totrot = new byte[]
		{
			1,
			2,
			4,
			6,
			8,
			10,
			12,
			14,
			15,
			17,
			19,
			21,
			23,
			25,
			27,
			28
		};

		// Token: 0x04001BE3 RID: 7139
		private static readonly byte[] pc2 = new byte[]
		{
			13,
			16,
			10,
			23,
			0,
			4,
			2,
			27,
			14,
			5,
			20,
			9,
			22,
			18,
			11,
			3,
			25,
			7,
			15,
			6,
			26,
			19,
			12,
			1,
			40,
			51,
			30,
			36,
			46,
			54,
			29,
			39,
			50,
			44,
			32,
			47,
			43,
			48,
			38,
			55,
			33,
			52,
			45,
			41,
			49,
			35,
			28,
			31
		};

		// Token: 0x04001BE4 RID: 7140
		private static readonly uint[] SP1 = new uint[]
		{
			16843776U,
			0U,
			65536U,
			16843780U,
			16842756U,
			66564U,
			4U,
			65536U,
			1024U,
			16843776U,
			16843780U,
			1024U,
			16778244U,
			16842756U,
			16777216U,
			4U,
			1028U,
			16778240U,
			16778240U,
			66560U,
			66560U,
			16842752U,
			16842752U,
			16778244U,
			65540U,
			16777220U,
			16777220U,
			65540U,
			0U,
			1028U,
			66564U,
			16777216U,
			65536U,
			16843780U,
			4U,
			16842752U,
			16843776U,
			16777216U,
			16777216U,
			1024U,
			16842756U,
			65536U,
			66560U,
			16777220U,
			1024U,
			4U,
			16778244U,
			66564U,
			16843780U,
			65540U,
			16842752U,
			16778244U,
			16777220U,
			1028U,
			66564U,
			16843776U,
			1028U,
			16778240U,
			16778240U,
			0U,
			65540U,
			66560U,
			0U,
			16842756U
		};

		// Token: 0x04001BE5 RID: 7141
		private static readonly uint[] SP2 = new uint[]
		{
			2148565024U,
			2147516416U,
			32768U,
			1081376U,
			1048576U,
			32U,
			2148532256U,
			2147516448U,
			2147483680U,
			2148565024U,
			2148564992U,
			2147483648U,
			2147516416U,
			1048576U,
			32U,
			2148532256U,
			1081344U,
			1048608U,
			2147516448U,
			0U,
			2147483648U,
			32768U,
			1081376U,
			2148532224U,
			1048608U,
			2147483680U,
			0U,
			1081344U,
			32800U,
			2148564992U,
			2148532224U,
			32800U,
			0U,
			1081376U,
			2148532256U,
			1048576U,
			2147516448U,
			2148532224U,
			2148564992U,
			32768U,
			2148532224U,
			2147516416U,
			32U,
			2148565024U,
			1081376U,
			32U,
			32768U,
			2147483648U,
			32800U,
			2148564992U,
			1048576U,
			2147483680U,
			1048608U,
			2147516448U,
			2147483680U,
			1048608U,
			1081344U,
			0U,
			2147516416U,
			32800U,
			2147483648U,
			2148532256U,
			2148565024U,
			1081344U
		};

		// Token: 0x04001BE6 RID: 7142
		private static readonly uint[] SP3 = new uint[]
		{
			520U,
			134349312U,
			0U,
			134348808U,
			134218240U,
			0U,
			131592U,
			134218240U,
			131080U,
			134217736U,
			134217736U,
			131072U,
			134349320U,
			131080U,
			134348800U,
			520U,
			134217728U,
			8U,
			134349312U,
			512U,
			131584U,
			134348800U,
			134348808U,
			131592U,
			134218248U,
			131584U,
			131072U,
			134218248U,
			8U,
			134349320U,
			512U,
			134217728U,
			134349312U,
			134217728U,
			131080U,
			520U,
			131072U,
			134349312U,
			134218240U,
			0U,
			512U,
			131080U,
			134349320U,
			134218240U,
			134217736U,
			512U,
			0U,
			134348808U,
			134218248U,
			131072U,
			134217728U,
			134349320U,
			8U,
			131592U,
			131584U,
			134217736U,
			134348800U,
			134218248U,
			520U,
			134348800U,
			131592U,
			8U,
			134348808U,
			131584U
		};

		// Token: 0x04001BE7 RID: 7143
		private static readonly uint[] SP4 = new uint[]
		{
			8396801U,
			8321U,
			8321U,
			128U,
			8396928U,
			8388737U,
			8388609U,
			8193U,
			0U,
			8396800U,
			8396800U,
			8396929U,
			129U,
			0U,
			8388736U,
			8388609U,
			1U,
			8192U,
			8388608U,
			8396801U,
			128U,
			8388608U,
			8193U,
			8320U,
			8388737U,
			1U,
			8320U,
			8388736U,
			8192U,
			8396928U,
			8396929U,
			129U,
			8388736U,
			8388609U,
			8396800U,
			8396929U,
			129U,
			0U,
			0U,
			8396800U,
			8320U,
			8388736U,
			8388737U,
			1U,
			8396801U,
			8321U,
			8321U,
			128U,
			8396929U,
			129U,
			1U,
			8192U,
			8388609U,
			8193U,
			8396928U,
			8388737U,
			8193U,
			8320U,
			8388608U,
			8396801U,
			128U,
			8388608U,
			8192U,
			8396928U
		};

		// Token: 0x04001BE8 RID: 7144
		private static readonly uint[] SP5 = new uint[]
		{
			256U,
			34078976U,
			34078720U,
			1107296512U,
			524288U,
			256U,
			1073741824U,
			34078720U,
			1074266368U,
			524288U,
			33554688U,
			1074266368U,
			1107296512U,
			1107820544U,
			524544U,
			1073741824U,
			33554432U,
			1074266112U,
			1074266112U,
			0U,
			1073742080U,
			1107820800U,
			1107820800U,
			33554688U,
			1107820544U,
			1073742080U,
			0U,
			1107296256U,
			34078976U,
			33554432U,
			1107296256U,
			524544U,
			524288U,
			1107296512U,
			256U,
			33554432U,
			1073741824U,
			34078720U,
			1107296512U,
			1074266368U,
			33554688U,
			1073741824U,
			1107820544U,
			34078976U,
			1074266368U,
			256U,
			33554432U,
			1107820544U,
			1107820800U,
			524544U,
			1107296256U,
			1107820800U,
			34078720U,
			0U,
			1074266112U,
			1107296256U,
			524544U,
			33554688U,
			1073742080U,
			524288U,
			0U,
			1074266112U,
			34078976U,
			1073742080U
		};

		// Token: 0x04001BE9 RID: 7145
		private static readonly uint[] SP6 = new uint[]
		{
			536870928U,
			541065216U,
			16384U,
			541081616U,
			541065216U,
			16U,
			541081616U,
			4194304U,
			536887296U,
			4210704U,
			4194304U,
			536870928U,
			4194320U,
			536887296U,
			536870912U,
			16400U,
			0U,
			4194320U,
			536887312U,
			16384U,
			4210688U,
			536887312U,
			16U,
			541065232U,
			541065232U,
			0U,
			4210704U,
			541081600U,
			16400U,
			4210688U,
			541081600U,
			536870912U,
			536887296U,
			16U,
			541065232U,
			4210688U,
			541081616U,
			4194304U,
			16400U,
			536870928U,
			4194304U,
			536887296U,
			536870912U,
			16400U,
			536870928U,
			541081616U,
			4210688U,
			541065216U,
			4210704U,
			541081600U,
			0U,
			541065232U,
			16U,
			16384U,
			541065216U,
			4210704U,
			16384U,
			4194320U,
			536887312U,
			0U,
			541081600U,
			536870912U,
			4194320U,
			536887312U
		};

		// Token: 0x04001BEA RID: 7146
		private static readonly uint[] SP7 = new uint[]
		{
			2097152U,
			69206018U,
			67110914U,
			0U,
			2048U,
			67110914U,
			2099202U,
			69208064U,
			69208066U,
			2097152U,
			0U,
			67108866U,
			2U,
			67108864U,
			69206018U,
			2050U,
			67110912U,
			2099202U,
			2097154U,
			67110912U,
			67108866U,
			69206016U,
			69208064U,
			2097154U,
			69206016U,
			2048U,
			2050U,
			69208066U,
			2099200U,
			2U,
			67108864U,
			2099200U,
			67108864U,
			2099200U,
			2097152U,
			67110914U,
			67110914U,
			69206018U,
			69206018U,
			2U,
			2097154U,
			67108864U,
			67110912U,
			2097152U,
			69208064U,
			2050U,
			2099202U,
			69208064U,
			2050U,
			67108866U,
			69208066U,
			69206016U,
			2099200U,
			0U,
			2U,
			69208066U,
			0U,
			2099202U,
			69206016U,
			2048U,
			67108866U,
			67110912U,
			2048U,
			2097154U
		};

		// Token: 0x04001BEB RID: 7147
		private static readonly uint[] SP8 = new uint[]
		{
			268439616U,
			4096U,
			262144U,
			268701760U,
			268435456U,
			268439616U,
			64U,
			268435456U,
			262208U,
			268697600U,
			268701760U,
			266240U,
			268701696U,
			266304U,
			4096U,
			64U,
			268697600U,
			268435520U,
			268439552U,
			4160U,
			266240U,
			262208U,
			268697664U,
			268701696U,
			4160U,
			0U,
			0U,
			268697664U,
			268435520U,
			268439552U,
			266304U,
			262144U,
			266304U,
			262144U,
			268701696U,
			4096U,
			64U,
			268697664U,
			4096U,
			266304U,
			268439552U,
			64U,
			268435520U,
			268697600U,
			268697664U,
			268435456U,
			262144U,
			268439616U,
			0U,
			268701760U,
			262208U,
			268435520U,
			268697600U,
			268439552U,
			268439616U,
			0U,
			268701760U,
			266240U,
			266240U,
			4160U,
			4160U,
			262208U,
			268435456U,
			268701696U
		};
	}
}
