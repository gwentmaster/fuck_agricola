using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200034E RID: 846
	internal class SecT283Field
	{
		// Token: 0x06001FC2 RID: 8130 RVA: 0x000A62EF File Offset: 0x000A44EF
		public static void Add(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
			z[3] = (x[3] ^ y[3]);
			z[4] = (x[4] ^ y[4]);
		}

		// Token: 0x06001FC3 RID: 8131 RVA: 0x000ADBD4 File Offset: 0x000ABDD4
		public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
		{
			zz[0] = (xx[0] ^ yy[0]);
			zz[1] = (xx[1] ^ yy[1]);
			zz[2] = (xx[2] ^ yy[2]);
			zz[3] = (xx[3] ^ yy[3]);
			zz[4] = (xx[4] ^ yy[4]);
			zz[5] = (xx[5] ^ yy[5]);
			zz[6] = (xx[6] ^ yy[6]);
			zz[7] = (xx[7] ^ yy[7]);
			zz[8] = (xx[8] ^ yy[8]);
		}

		// Token: 0x06001FC4 RID: 8132 RVA: 0x000ADC3B File Offset: 0x000ABE3B
		public static void AddOne(ulong[] x, ulong[] z)
		{
			z[0] = (x[0] ^ 1UL);
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
		}

		// Token: 0x06001FC5 RID: 8133 RVA: 0x000ADC5E File Offset: 0x000ABE5E
		public static ulong[] FromBigInteger(BigInteger x)
		{
			ulong[] array = Nat320.FromBigInteger64(x);
			SecT283Field.Reduce37(array, 0);
			return array;
		}

		// Token: 0x06001FC6 RID: 8134 RVA: 0x000ADC70 File Offset: 0x000ABE70
		public static void Invert(ulong[] x, ulong[] z)
		{
			if (Nat320.IsZero64(x))
			{
				throw new InvalidOperationException();
			}
			ulong[] array = Nat320.Create64();
			ulong[] array2 = Nat320.Create64();
			SecT283Field.Square(x, array);
			SecT283Field.Multiply(array, x, array);
			SecT283Field.SquareN(array, 2, array2);
			SecT283Field.Multiply(array2, array, array2);
			SecT283Field.SquareN(array2, 4, array);
			SecT283Field.Multiply(array, array2, array);
			SecT283Field.SquareN(array, 8, array2);
			SecT283Field.Multiply(array2, array, array2);
			SecT283Field.Square(array2, array2);
			SecT283Field.Multiply(array2, x, array2);
			SecT283Field.SquareN(array2, 17, array);
			SecT283Field.Multiply(array, array2, array);
			SecT283Field.Square(array, array);
			SecT283Field.Multiply(array, x, array);
			SecT283Field.SquareN(array, 35, array2);
			SecT283Field.Multiply(array2, array, array2);
			SecT283Field.SquareN(array2, 70, array);
			SecT283Field.Multiply(array, array2, array);
			SecT283Field.Square(array, array);
			SecT283Field.Multiply(array, x, array);
			SecT283Field.SquareN(array, 141, array2);
			SecT283Field.Multiply(array2, array, array2);
			SecT283Field.Square(array2, z);
		}

		// Token: 0x06001FC7 RID: 8135 RVA: 0x000ADD54 File Offset: 0x000ABF54
		public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
		{
			ulong[] array = Nat320.CreateExt64();
			SecT283Field.ImplMultiply(x, y, array);
			SecT283Field.Reduce(array, z);
		}

		// Token: 0x06001FC8 RID: 8136 RVA: 0x000ADD78 File Offset: 0x000ABF78
		public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = Nat320.CreateExt64();
			SecT283Field.ImplMultiply(x, y, array);
			SecT283Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001FC9 RID: 8137 RVA: 0x000ADD9C File Offset: 0x000ABF9C
		public static void Reduce(ulong[] xx, ulong[] z)
		{
			ulong num = xx[0];
			ulong num2 = xx[1];
			ulong num3 = xx[2];
			ulong num4 = xx[3];
			ulong num5 = xx[4];
			ulong num6 = xx[5];
			ulong num7 = xx[6];
			ulong num8 = xx[7];
			ulong num9 = xx[8];
			num4 ^= (num9 << 37 ^ num9 << 42 ^ num9 << 44 ^ num9 << 49);
			num5 ^= (num9 >> 27 ^ num9 >> 22 ^ num9 >> 20 ^ num9 >> 15);
			num3 ^= (num8 << 37 ^ num8 << 42 ^ num8 << 44 ^ num8 << 49);
			num4 ^= (num8 >> 27 ^ num8 >> 22 ^ num8 >> 20 ^ num8 >> 15);
			num2 ^= (num7 << 37 ^ num7 << 42 ^ num7 << 44 ^ num7 << 49);
			num3 ^= (num7 >> 27 ^ num7 >> 22 ^ num7 >> 20 ^ num7 >> 15);
			num ^= (num6 << 37 ^ num6 << 42 ^ num6 << 44 ^ num6 << 49);
			num2 ^= (num6 >> 27 ^ num6 >> 22 ^ num6 >> 20 ^ num6 >> 15);
			ulong num10 = num5 >> 27;
			z[0] = (num ^ num10 ^ num10 << 5 ^ num10 << 7 ^ num10 << 12);
			z[1] = num2;
			z[2] = num3;
			z[3] = num4;
			z[4] = (num5 & 134217727UL);
		}

		// Token: 0x06001FCA RID: 8138 RVA: 0x000ADEDC File Offset: 0x000AC0DC
		public static void Reduce37(ulong[] z, int zOff)
		{
			ulong num = z[zOff + 4];
			ulong num2 = num >> 27;
			z[zOff] ^= (num2 ^ num2 << 5 ^ num2 << 7 ^ num2 << 12);
			z[zOff + 4] = (num & 134217727UL);
		}

		// Token: 0x06001FCB RID: 8139 RVA: 0x000ADF1C File Offset: 0x000AC11C
		public static void Sqrt(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat320.Create64();
			ulong num = Interleave.Unshuffle(x[0]);
			ulong num2 = Interleave.Unshuffle(x[1]);
			ulong num3 = (num & (ulong)-1) | num2 << 32;
			array[0] = (num >> 32 | (num2 & 18446744069414584320UL));
			num = Interleave.Unshuffle(x[2]);
			num2 = Interleave.Unshuffle(x[3]);
			ulong num4 = (num & (ulong)-1) | num2 << 32;
			array[1] = (num >> 32 | (num2 & 18446744069414584320UL));
			num = Interleave.Unshuffle(x[4]);
			ulong num5 = num & (ulong)-1;
			array[2] = num >> 32;
			SecT283Field.Multiply(array, SecT283Field.ROOT_Z, z);
			z[0] ^= num3;
			z[1] ^= num4;
			z[2] ^= num5;
		}

		// Token: 0x06001FCC RID: 8140 RVA: 0x000ADFD4 File Offset: 0x000AC1D4
		public static void Square(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat.Create64(9);
			SecT283Field.ImplSquare(x, array);
			SecT283Field.Reduce(array, z);
		}

		// Token: 0x06001FCD RID: 8141 RVA: 0x000ADFF8 File Offset: 0x000AC1F8
		public static void SquareAddToExt(ulong[] x, ulong[] zz)
		{
			ulong[] array = Nat.Create64(9);
			SecT283Field.ImplSquare(x, array);
			SecT283Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001FCE RID: 8142 RVA: 0x000AE01C File Offset: 0x000AC21C
		public static void SquareN(ulong[] x, int n, ulong[] z)
		{
			ulong[] array = Nat.Create64(9);
			SecT283Field.ImplSquare(x, array);
			SecT283Field.Reduce(array, z);
			while (--n > 0)
			{
				SecT283Field.ImplSquare(z, array);
				SecT283Field.Reduce(array, z);
			}
		}

		// Token: 0x06001FCF RID: 8143 RVA: 0x000AE058 File Offset: 0x000AC258
		public static uint Trace(ulong[] x)
		{
			return (uint)(x[0] ^ x[4] >> 15) & 1U;
		}

		// Token: 0x06001FD0 RID: 8144 RVA: 0x000AE068 File Offset: 0x000AC268
		protected static void ImplCompactExt(ulong[] zz)
		{
			ulong num = zz[0];
			ulong num2 = zz[1];
			ulong num3 = zz[2];
			ulong num4 = zz[3];
			ulong num5 = zz[4];
			ulong num6 = zz[5];
			ulong num7 = zz[6];
			ulong num8 = zz[7];
			ulong num9 = zz[8];
			ulong num10 = zz[9];
			zz[0] = (num ^ num2 << 57);
			zz[1] = (num2 >> 7 ^ num3 << 50);
			zz[2] = (num3 >> 14 ^ num4 << 43);
			zz[3] = (num4 >> 21 ^ num5 << 36);
			zz[4] = (num5 >> 28 ^ num6 << 29);
			zz[5] = (num6 >> 35 ^ num7 << 22);
			zz[6] = (num7 >> 42 ^ num8 << 15);
			zz[7] = (num8 >> 49 ^ num9 << 8);
			zz[8] = (num9 >> 56 ^ num10 << 1);
			zz[9] = num10 >> 63;
		}

		// Token: 0x06001FD1 RID: 8145 RVA: 0x000AE120 File Offset: 0x000AC320
		protected static void ImplExpand(ulong[] x, ulong[] z)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = x[2];
			ulong num4 = x[3];
			ulong num5 = x[4];
			z[0] = (num & 144115188075855871UL);
			z[1] = ((num >> 57 ^ num2 << 7) & 144115188075855871UL);
			z[2] = ((num2 >> 50 ^ num3 << 14) & 144115188075855871UL);
			z[3] = ((num3 >> 43 ^ num4 << 21) & 144115188075855871UL);
			z[4] = (num4 >> 36 ^ num5 << 28);
		}

		// Token: 0x06001FD2 RID: 8146 RVA: 0x000AE1A0 File Offset: 0x000AC3A0
		protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = new ulong[5];
			ulong[] array2 = new ulong[5];
			SecT283Field.ImplExpand(x, array);
			SecT283Field.ImplExpand(y, array2);
			ulong[] array3 = new ulong[26];
			SecT283Field.ImplMulw(array[0], array2[0], array3, 0);
			SecT283Field.ImplMulw(array[1], array2[1], array3, 2);
			SecT283Field.ImplMulw(array[2], array2[2], array3, 4);
			SecT283Field.ImplMulw(array[3], array2[3], array3, 6);
			SecT283Field.ImplMulw(array[4], array2[4], array3, 8);
			ulong num = array[0] ^ array[1];
			ulong num2 = array2[0] ^ array2[1];
			ulong num3 = array[0] ^ array[2];
			ulong num4 = array2[0] ^ array2[2];
			ulong num5 = array[2] ^ array[4];
			ulong num6 = array2[2] ^ array2[4];
			ulong num7 = array[3] ^ array[4];
			ulong num8 = array2[3] ^ array2[4];
			SecT283Field.ImplMulw(num3 ^ array[3], num4 ^ array2[3], array3, 18);
			SecT283Field.ImplMulw(num5 ^ array[1], num6 ^ array2[1], array3, 20);
			ulong num9 = num ^ num7;
			ulong num10 = num2 ^ num8;
			ulong x2 = num9 ^ array[2];
			ulong y2 = num10 ^ array2[2];
			SecT283Field.ImplMulw(num9, num10, array3, 22);
			SecT283Field.ImplMulw(x2, y2, array3, 24);
			SecT283Field.ImplMulw(num, num2, array3, 10);
			SecT283Field.ImplMulw(num3, num4, array3, 12);
			SecT283Field.ImplMulw(num5, num6, array3, 14);
			SecT283Field.ImplMulw(num7, num8, array3, 16);
			zz[0] = array3[0];
			zz[9] = array3[9];
			ulong num11 = array3[0] ^ array3[1];
			ulong num12 = num11 ^ array3[2];
			ulong num13 = num12 ^ array3[10];
			zz[1] = num13;
			ulong num14 = array3[3] ^ array3[4];
			ulong num15 = array3[11] ^ array3[12];
			ulong num16 = num14 ^ num15;
			ulong num17 = num12 ^ num16;
			zz[2] = num17;
			ulong num18 = num11 ^ num14;
			ulong num19 = array3[5] ^ array3[6];
			ulong num20 = num18 ^ num19 ^ array3[8];
			ulong num21 = array3[13] ^ array3[14];
			ulong num22 = num20 ^ num21;
			ulong num23 = array3[18] ^ array3[22] ^ array3[24];
			ulong num24 = num22 ^ num23;
			zz[3] = num24;
			ulong num25 = array3[7] ^ array3[8] ^ array3[9];
			ulong num26 = num25 ^ array3[17];
			zz[8] = num26;
			ulong num27 = num25 ^ num19;
			ulong num28 = array3[15] ^ array3[16];
			ulong num29 = num27 ^ num28;
			zz[7] = num29;
			ulong num30 = num29 ^ num13;
			ulong num31 = array3[19] ^ array3[20];
			ulong num32 = array3[25] ^ array3[24];
			ulong num33 = array3[18] ^ array3[23];
			ulong num34 = num31 ^ num32;
			ulong num35 = num34 ^ num33 ^ num30;
			zz[4] = num35;
			ulong num36 = num17 ^ num26;
			ulong num37 = num34 ^ num36;
			ulong num38 = array3[21] ^ array3[22];
			ulong num39 = num37 ^ num38;
			zz[5] = num39;
			ulong num40 = num20 ^ array3[0] ^ array3[9] ^ num21 ^ array3[21] ^ array3[23] ^ array3[25];
			zz[6] = num40;
			SecT283Field.ImplCompactExt(zz);
		}

		// Token: 0x06001FD3 RID: 8147 RVA: 0x000AE414 File Offset: 0x000AC614
		protected static void ImplMulw(ulong x, ulong y, ulong[] z, int zOff)
		{
			ulong[] array = new ulong[8];
			array[1] = y;
			array[2] = array[1] << 1;
			array[3] = (array[2] ^ y);
			array[4] = array[2] << 1;
			array[5] = (array[4] ^ y);
			array[6] = array[3] << 1;
			array[7] = (array[6] ^ y);
			uint num = (uint)x;
			ulong num2 = 0UL;
			ulong num3 = array[(int)(num & 7U)];
			int num4 = 48;
			do
			{
				num = (uint)(x >> num4);
				ulong num5 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3 ^ array[(int)(num >> 6 & 7U)] << 6;
				num3 ^= num5 << num4;
				num2 ^= num5 >> -num4;
			}
			while ((num4 -= 9) > 0);
			num2 ^= (x & 72198606942111744UL & y << 7 >> 63) >> 8;
			z[zOff] = (num3 & 144115188075855871UL);
			z[zOff + 1] = (num3 >> 57 ^ num2 << 7);
		}

		// Token: 0x06001FD4 RID: 8148 RVA: 0x000AE4E8 File Offset: 0x000AC6E8
		protected static void ImplSquare(ulong[] x, ulong[] zz)
		{
			for (int i = 0; i < 4; i++)
			{
				Interleave.Expand64To128(x[i], zz, i << 1);
			}
			zz[8] = Interleave.Expand32to64((uint)x[4]);
		}

		// Token: 0x04001647 RID: 5703
		private const ulong M27 = 134217727UL;

		// Token: 0x04001648 RID: 5704
		private const ulong M57 = 144115188075855871UL;

		// Token: 0x04001649 RID: 5705
		private static readonly ulong[] ROOT_Z = new ulong[]
		{
			878416384462358536UL,
			3513665537849438403UL,
			9369774767598502668UL,
			585610922974906400UL,
			34087042UL
		};
	}
}
