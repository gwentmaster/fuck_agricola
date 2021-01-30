using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
	// Token: 0x02000465 RID: 1125
	internal abstract class GcmUtilities
	{
		// Token: 0x060028FB RID: 10491 RVA: 0x000CAD18 File Offset: 0x000C8F18
		private static uint[] GenerateLookup()
		{
			uint[] array = new uint[256];
			for (int i = 0; i < 256; i++)
			{
				uint num = 0U;
				for (int j = 7; j >= 0; j--)
				{
					if ((i & 1 << j) != 0)
					{
						num ^= 3774873600U >> 7 - j;
					}
				}
				array[i] = num;
			}
			return array;
		}

		// Token: 0x060028FC RID: 10492 RVA: 0x000CAD6C File Offset: 0x000C8F6C
		internal static byte[] OneAsBytes()
		{
			byte[] array = new byte[16];
			array[0] = 128;
			return array;
		}

		// Token: 0x060028FD RID: 10493 RVA: 0x000CAD7D File Offset: 0x000C8F7D
		internal static uint[] OneAsUints()
		{
			uint[] array = new uint[4];
			array[0] = 2147483648U;
			return array;
		}

		// Token: 0x060028FE RID: 10494 RVA: 0x000CAD8D File Offset: 0x000C8F8D
		internal static ulong[] OneAsUlongs()
		{
			ulong[] array = new ulong[2];
			array[0] = 9223372036854775808UL;
			return array;
		}

		// Token: 0x060028FF RID: 10495 RVA: 0x000CADA1 File Offset: 0x000C8FA1
		internal static byte[] AsBytes(uint[] x)
		{
			return Pack.UInt32_To_BE(x);
		}

		// Token: 0x06002900 RID: 10496 RVA: 0x000CADA9 File Offset: 0x000C8FA9
		internal static void AsBytes(uint[] x, byte[] z)
		{
			Pack.UInt32_To_BE(x, z, 0);
		}

		// Token: 0x06002901 RID: 10497 RVA: 0x000CADB4 File Offset: 0x000C8FB4
		internal static byte[] AsBytes(ulong[] x)
		{
			byte[] array = new byte[16];
			Pack.UInt64_To_BE(x, array, 0);
			return array;
		}

		// Token: 0x06002902 RID: 10498 RVA: 0x000CADD2 File Offset: 0x000C8FD2
		internal static void AsBytes(ulong[] x, byte[] z)
		{
			Pack.UInt64_To_BE(x, z, 0);
		}

		// Token: 0x06002903 RID: 10499 RVA: 0x000CADDC File Offset: 0x000C8FDC
		internal static uint[] AsUints(byte[] bs)
		{
			uint[] array = new uint[4];
			Pack.BE_To_UInt32(bs, 0, array);
			return array;
		}

		// Token: 0x06002904 RID: 10500 RVA: 0x000CADF9 File Offset: 0x000C8FF9
		internal static void AsUints(byte[] bs, uint[] output)
		{
			Pack.BE_To_UInt32(bs, 0, output);
		}

		// Token: 0x06002905 RID: 10501 RVA: 0x000CAE04 File Offset: 0x000C9004
		internal static ulong[] AsUlongs(byte[] x)
		{
			ulong[] array = new ulong[2];
			Pack.BE_To_UInt64(x, 0, array);
			return array;
		}

		// Token: 0x06002906 RID: 10502 RVA: 0x000CAE21 File Offset: 0x000C9021
		public static void AsUlongs(byte[] x, ulong[] z)
		{
			Pack.BE_To_UInt64(x, 0, z);
		}

		// Token: 0x06002907 RID: 10503 RVA: 0x000CAE2C File Offset: 0x000C902C
		internal static void Multiply(byte[] x, byte[] y)
		{
			uint[] x2 = GcmUtilities.AsUints(x);
			uint[] y2 = GcmUtilities.AsUints(y);
			GcmUtilities.Multiply(x2, y2);
			GcmUtilities.AsBytes(x2, x);
		}

		// Token: 0x06002908 RID: 10504 RVA: 0x000CAE54 File Offset: 0x000C9054
		internal static void Multiply(uint[] x, uint[] y)
		{
			uint num = x[0];
			uint num2 = x[1];
			uint num3 = x[2];
			uint num4 = x[3];
			uint num5 = 0U;
			uint num6 = 0U;
			uint num7 = 0U;
			uint num8 = 0U;
			for (int i = 0; i < 4; i++)
			{
				int num9 = (int)y[i];
				for (int j = 0; j < 32; j++)
				{
					uint num10 = (uint)(num9 >> 31);
					num9 <<= 1;
					num5 ^= (num & num10);
					num6 ^= (num2 & num10);
					num7 ^= (num3 & num10);
					num8 ^= (num4 & num10);
					uint num11 = (uint)((int)((int)num4 << 31) >> 8);
					num4 = (num4 >> 1 | num3 << 31);
					num3 = (num3 >> 1 | num2 << 31);
					num2 = (num2 >> 1 | num << 31);
					num = (num >> 1 ^ (num11 & 3774873600U));
				}
			}
			x[0] = num5;
			x[1] = num6;
			x[2] = num7;
			x[3] = num8;
		}

		// Token: 0x06002909 RID: 10505 RVA: 0x000CAF1C File Offset: 0x000C911C
		internal static void Multiply(ulong[] x, ulong[] y)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = 0UL;
			ulong num4 = 0UL;
			for (int i = 0; i < 2; i++)
			{
				long num5 = (long)y[i];
				for (int j = 0; j < 64; j++)
				{
					ulong num6 = (ulong)(num5 >> 63);
					num5 <<= 1;
					num3 ^= (num & num6);
					num4 ^= (num2 & num6);
					ulong num7 = num2 << 63 >> 8;
					num2 = (num2 >> 1 | num << 63);
					num = (num >> 1 ^ (num7 & 16212958658533785600UL));
				}
			}
			x[0] = num3;
			x[1] = num4;
		}

		// Token: 0x0600290A RID: 10506 RVA: 0x000CAFA4 File Offset: 0x000C91A4
		internal static void MultiplyP(uint[] x)
		{
			uint num = (uint)((int)GcmUtilities.ShiftRight(x) >> 8);
			x[0] ^= (num & 3774873600U);
		}

		// Token: 0x0600290B RID: 10507 RVA: 0x000CAFCC File Offset: 0x000C91CC
		internal static void MultiplyP(uint[] x, uint[] z)
		{
			uint num = (uint)((int)GcmUtilities.ShiftRight(x, z) >> 8);
			z[0] ^= (num & 3774873600U);
		}

		// Token: 0x0600290C RID: 10508 RVA: 0x000CAFF8 File Offset: 0x000C91F8
		internal static void MultiplyP8(uint[] x)
		{
			uint num = GcmUtilities.ShiftRightN(x, 8);
			x[0] ^= GcmUtilities.LOOKUP[(int)(num >> 24)];
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000CB024 File Offset: 0x000C9224
		internal static void MultiplyP8(uint[] x, uint[] y)
		{
			uint num = GcmUtilities.ShiftRightN(x, 8, y);
			y[0] ^= GcmUtilities.LOOKUP[(int)(num >> 24)];
		}

		// Token: 0x0600290E RID: 10510 RVA: 0x000CB050 File Offset: 0x000C9250
		internal static uint ShiftRight(uint[] x)
		{
			uint num = x[0];
			x[0] = num >> 1;
			uint num2 = num << 31;
			num = x[1];
			x[1] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[2];
			x[2] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[3];
			x[3] = (num >> 1 | num2);
			return num << 31;
		}

		// Token: 0x0600290F RID: 10511 RVA: 0x000CB0A0 File Offset: 0x000C92A0
		internal static uint ShiftRight(uint[] x, uint[] z)
		{
			uint num = x[0];
			z[0] = num >> 1;
			uint num2 = num << 31;
			num = x[1];
			z[1] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[2];
			z[2] = (num >> 1 | num2);
			num2 = num << 31;
			num = x[3];
			z[3] = (num >> 1 | num2);
			return num << 31;
		}

		// Token: 0x06002910 RID: 10512 RVA: 0x000CB0F0 File Offset: 0x000C92F0
		internal static uint ShiftRightN(uint[] x, int n)
		{
			uint num = x[0];
			int num2 = 32 - n;
			x[0] = num >> n;
			uint num3 = num << num2;
			num = x[1];
			x[1] = (num >> n | num3);
			num3 = num << num2;
			num = x[2];
			x[2] = (num >> n | num3);
			num3 = num << num2;
			num = x[3];
			x[3] = (num >> n | num3);
			return num << num2;
		}

		// Token: 0x06002911 RID: 10513 RVA: 0x000CB158 File Offset: 0x000C9358
		internal static uint ShiftRightN(uint[] x, int n, uint[] z)
		{
			uint num = x[0];
			int num2 = 32 - n;
			z[0] = num >> n;
			uint num3 = num << num2;
			num = x[1];
			z[1] = (num >> n | num3);
			num3 = num << num2;
			num = x[2];
			z[2] = (num >> n | num3);
			num3 = num << num2;
			num = x[3];
			z[3] = (num >> n | num3);
			return num << num2;
		}

		// Token: 0x06002912 RID: 10514 RVA: 0x000CB1C0 File Offset: 0x000C93C0
		internal static void Xor(byte[] x, byte[] y)
		{
			int num = 0;
			do
			{
				int num2 = num;
				x[num2] ^= y[num];
				num++;
				int num3 = num;
				x[num3] ^= y[num];
				num++;
				int num4 = num;
				x[num4] ^= y[num];
				num++;
				int num5 = num;
				x[num5] ^= y[num];
				num++;
			}
			while (num < 16);
		}

		// Token: 0x06002913 RID: 10515 RVA: 0x000CB220 File Offset: 0x000C9420
		internal static void Xor(byte[] x, byte[] y, int yOff, int yLen)
		{
			while (--yLen >= 0)
			{
				int num = yLen;
				x[num] ^= y[yOff + yLen];
			}
		}

		// Token: 0x06002914 RID: 10516 RVA: 0x000CB240 File Offset: 0x000C9440
		internal static void Xor(byte[] x, byte[] y, byte[] z)
		{
			int num = 0;
			do
			{
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
				z[num] = (x[num] ^ y[num]);
				num++;
			}
			while (num < 16);
		}

		// Token: 0x06002915 RID: 10517 RVA: 0x000CB290 File Offset: 0x000C9490
		internal static void Xor(uint[] x, uint[] y)
		{
			x[0] ^= y[0];
			x[1] ^= y[1];
			x[2] ^= y[2];
			x[3] ^= y[3];
		}

		// Token: 0x06002916 RID: 10518 RVA: 0x000CB2CA File Offset: 0x000C94CA
		internal static void Xor(uint[] x, uint[] y, uint[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
			z[3] = (x[3] ^ y[3]);
		}

		// Token: 0x06002917 RID: 10519 RVA: 0x000CB2F4 File Offset: 0x000C94F4
		internal static void Xor(ulong[] x, ulong[] y)
		{
			x[0] ^= y[0];
			x[1] ^= y[1];
		}

		// Token: 0x06002918 RID: 10520 RVA: 0x000A4BF0 File Offset: 0x000A2DF0
		internal static void Xor(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
		}

		// Token: 0x04001AF6 RID: 6902
		private const uint E1 = 3774873600U;

		// Token: 0x04001AF7 RID: 6903
		private const ulong E1L = 16212958658533785600UL;

		// Token: 0x04001AF8 RID: 6904
		private static readonly uint[] LOOKUP = GcmUtilities.GenerateLookup();
	}
}
