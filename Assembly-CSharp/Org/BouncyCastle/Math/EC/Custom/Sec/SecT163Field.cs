using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000336 RID: 822
	internal class SecT163Field
	{
		// Token: 0x06001E2F RID: 7727 RVA: 0x000A62CF File Offset: 0x000A44CF
		public static void Add(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
		}

		// Token: 0x06001E30 RID: 7728 RVA: 0x000A7B5B File Offset: 0x000A5D5B
		public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
		{
			zz[0] = (xx[0] ^ yy[0]);
			zz[1] = (xx[1] ^ yy[1]);
			zz[2] = (xx[2] ^ yy[2]);
			zz[3] = (xx[3] ^ yy[3]);
			zz[4] = (xx[4] ^ yy[4]);
			zz[5] = (xx[5] ^ yy[5]);
		}

		// Token: 0x06001E31 RID: 7729 RVA: 0x000A6323 File Offset: 0x000A4523
		public static void AddOne(ulong[] x, ulong[] z)
		{
			z[0] = (x[0] ^ 1UL);
			z[1] = x[1];
			z[2] = x[2];
		}

		// Token: 0x06001E32 RID: 7730 RVA: 0x000A7B99 File Offset: 0x000A5D99
		public static ulong[] FromBigInteger(BigInteger x)
		{
			ulong[] array = Nat192.FromBigInteger64(x);
			SecT163Field.Reduce29(array, 0);
			return array;
		}

		// Token: 0x06001E33 RID: 7731 RVA: 0x000A7BA8 File Offset: 0x000A5DA8
		public static void Invert(ulong[] x, ulong[] z)
		{
			if (Nat192.IsZero64(x))
			{
				throw new InvalidOperationException();
			}
			ulong[] array = Nat192.Create64();
			ulong[] array2 = Nat192.Create64();
			SecT163Field.Square(x, array);
			SecT163Field.SquareN(array, 1, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array2, 1, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array, 3, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array2, 3, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array, 9, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array2, 9, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array, 27, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array2, 27, array2);
			SecT163Field.Multiply(array, array2, array);
			SecT163Field.SquareN(array, 81, array2);
			SecT163Field.Multiply(array, array2, z);
		}

		// Token: 0x06001E34 RID: 7732 RVA: 0x000A7C6C File Offset: 0x000A5E6C
		public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT163Field.ImplMultiply(x, y, array);
			SecT163Field.Reduce(array, z);
		}

		// Token: 0x06001E35 RID: 7733 RVA: 0x000A7C90 File Offset: 0x000A5E90
		public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT163Field.ImplMultiply(x, y, array);
			SecT163Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001E36 RID: 7734 RVA: 0x000A7CB4 File Offset: 0x000A5EB4
		public static void Reduce(ulong[] xx, ulong[] z)
		{
			ulong num = xx[0];
			ulong num2 = xx[1];
			ulong num3 = xx[2];
			ulong num4 = xx[3];
			ulong num5 = xx[4];
			ulong num6 = xx[5];
			num3 ^= (num6 << 29 ^ num6 << 32 ^ num6 << 35 ^ num6 << 36);
			num4 ^= (num6 >> 35 ^ num6 >> 32 ^ num6 >> 29 ^ num6 >> 28);
			num2 ^= (num5 << 29 ^ num5 << 32 ^ num5 << 35 ^ num5 << 36);
			num3 ^= (num5 >> 35 ^ num5 >> 32 ^ num5 >> 29 ^ num5 >> 28);
			num ^= (num4 << 29 ^ num4 << 32 ^ num4 << 35 ^ num4 << 36);
			num2 ^= (num4 >> 35 ^ num4 >> 32 ^ num4 >> 29 ^ num4 >> 28);
			ulong num7 = num3 >> 35;
			z[0] = (num ^ num7 ^ num7 << 3 ^ num7 << 6 ^ num7 << 7);
			z[1] = num2;
			z[2] = (num3 & 34359738367UL);
		}

		// Token: 0x06001E37 RID: 7735 RVA: 0x000A7DA0 File Offset: 0x000A5FA0
		public static void Reduce29(ulong[] z, int zOff)
		{
			ulong num = z[zOff + 2];
			ulong num2 = num >> 35;
			z[zOff] ^= (num2 ^ num2 << 3 ^ num2 << 6 ^ num2 << 7);
			z[zOff + 2] = (num & 34359738367UL);
		}

		// Token: 0x06001E38 RID: 7736 RVA: 0x000A7DE0 File Offset: 0x000A5FE0
		public static void Sqrt(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat192.Create64();
			ulong num = Interleave.Unshuffle(x[0]);
			ulong num2 = Interleave.Unshuffle(x[1]);
			ulong num3 = (num & (ulong)-1) | num2 << 32;
			array[0] = (num >> 32 | (num2 & 18446744069414584320UL));
			num = Interleave.Unshuffle(x[2]);
			ulong num4 = num & (ulong)-1;
			array[1] = num >> 32;
			SecT163Field.Multiply(array, SecT163Field.ROOT_Z, z);
			z[0] ^= num3;
			z[1] ^= num4;
		}

		// Token: 0x06001E39 RID: 7737 RVA: 0x000A7E5C File Offset: 0x000A605C
		public static void Square(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT163Field.ImplSquare(x, array);
			SecT163Field.Reduce(array, z);
		}

		// Token: 0x06001E3A RID: 7738 RVA: 0x000A7E80 File Offset: 0x000A6080
		public static void SquareAddToExt(ulong[] x, ulong[] zz)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT163Field.ImplSquare(x, array);
			SecT163Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001E3B RID: 7739 RVA: 0x000A7EA4 File Offset: 0x000A60A4
		public static void SquareN(ulong[] x, int n, ulong[] z)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT163Field.ImplSquare(x, array);
			SecT163Field.Reduce(array, z);
			while (--n > 0)
			{
				SecT163Field.ImplSquare(z, array);
				SecT163Field.Reduce(array, z);
			}
		}

		// Token: 0x06001E3C RID: 7740 RVA: 0x000A7EDE File Offset: 0x000A60DE
		public static uint Trace(ulong[] x)
		{
			return (uint)(x[0] ^ x[2] >> 29) & 1U;
		}

		// Token: 0x06001E3D RID: 7741 RVA: 0x000A7EF0 File Offset: 0x000A60F0
		protected static void ImplCompactExt(ulong[] zz)
		{
			ulong num = zz[0];
			ulong num2 = zz[1];
			ulong num3 = zz[2];
			ulong num4 = zz[3];
			ulong num5 = zz[4];
			ulong num6 = zz[5];
			zz[0] = (num ^ num2 << 55);
			zz[1] = (num2 >> 9 ^ num3 << 46);
			zz[2] = (num3 >> 18 ^ num4 << 37);
			zz[3] = (num4 >> 27 ^ num5 << 28);
			zz[4] = (num5 >> 36 ^ num6 << 19);
			zz[5] = num6 >> 45;
		}

		// Token: 0x06001E3E RID: 7742 RVA: 0x000A7F5C File Offset: 0x000A615C
		protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = x[2];
			num3 = (num2 >> 46 ^ num3 << 18);
			num2 = ((num >> 55 ^ num2 << 9) & 36028797018963967UL);
			num &= 36028797018963967UL;
			ulong num4 = y[0];
			ulong num5 = y[1];
			ulong num6 = y[2];
			num6 = (num5 >> 46 ^ num6 << 18);
			num5 = ((num4 >> 55 ^ num5 << 9) & 36028797018963967UL);
			num4 &= 36028797018963967UL;
			ulong[] array = new ulong[10];
			SecT163Field.ImplMulw(num, num4, array, 0);
			SecT163Field.ImplMulw(num3, num6, array, 2);
			ulong num7 = num ^ num2 ^ num3;
			ulong num8 = num4 ^ num5 ^ num6;
			SecT163Field.ImplMulw(num7, num8, array, 4);
			ulong num9 = num2 << 1 ^ num3 << 2;
			ulong num10 = num5 << 1 ^ num6 << 2;
			SecT163Field.ImplMulw(num ^ num9, num4 ^ num10, array, 6);
			SecT163Field.ImplMulw(num7 ^ num9, num8 ^ num10, array, 8);
			ulong num11 = array[6] ^ array[8];
			ulong num12 = array[7] ^ array[9];
			ulong num13 = num11 << 1 ^ array[6];
			ulong num14 = num11 ^ num12 << 1 ^ array[7];
			ulong num15 = num12;
			ulong num16 = array[0];
			ulong num17 = array[1] ^ array[0] ^ array[4];
			ulong num18 = array[1] ^ array[5];
			ulong num19 = num16 ^ num13 ^ array[2] << 4 ^ array[2] << 1;
			ulong num20 = num17 ^ num14 ^ array[3] << 4 ^ array[3] << 1;
			ulong num21 = num18 ^ num15;
			num20 ^= num19 >> 55;
			num19 &= 36028797018963967UL;
			num21 ^= num20 >> 55;
			num20 &= 36028797018963967UL;
			num19 = (num19 >> 1 ^ (num20 & 1UL) << 54);
			num20 = (num20 >> 1 ^ (num21 & 1UL) << 54);
			num21 >>= 1;
			num19 ^= num19 << 1;
			num19 ^= num19 << 2;
			num19 ^= num19 << 4;
			num19 ^= num19 << 8;
			num19 ^= num19 << 16;
			num19 ^= num19 << 32;
			num19 &= 36028797018963967UL;
			num20 ^= num19 >> 54;
			num20 ^= num20 << 1;
			num20 ^= num20 << 2;
			num20 ^= num20 << 4;
			num20 ^= num20 << 8;
			num20 ^= num20 << 16;
			num20 ^= num20 << 32;
			num20 &= 36028797018963967UL;
			num21 ^= num20 >> 54;
			num21 ^= num21 << 1;
			num21 ^= num21 << 2;
			num21 ^= num21 << 4;
			num21 ^= num21 << 8;
			num21 ^= num21 << 16;
			num21 ^= num21 << 32;
			zz[0] = num16;
			zz[1] = (num17 ^ num19 ^ array[2]);
			zz[2] = (num18 ^ num20 ^ num19 ^ array[3]);
			zz[3] = (num21 ^ num20);
			zz[4] = (num21 ^ array[2]);
			zz[5] = array[3];
			SecT163Field.ImplCompactExt(zz);
		}

		// Token: 0x06001E3F RID: 7743 RVA: 0x000A8238 File Offset: 0x000A6438
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
			ulong num3 = array[(int)(num & 3U)];
			int num4 = 47;
			do
			{
				num = (uint)(x >> num4);
				ulong num5 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3 ^ array[(int)(num >> 6 & 7U)] << 6;
				num3 ^= num5 << num4;
				num2 ^= num5 >> -num4;
			}
			while ((num4 -= 9) > 0);
			z[zOff] = (num3 & 36028797018963967UL);
			z[zOff + 1] = (num3 >> 55 ^ num2 << 9);
		}

		// Token: 0x06001E40 RID: 7744 RVA: 0x000A82F4 File Offset: 0x000A64F4
		protected static void ImplSquare(ulong[] x, ulong[] zz)
		{
			Interleave.Expand64To128(x[0], zz, 0);
			Interleave.Expand64To128(x[1], zz, 2);
			ulong num = x[2];
			zz[4] = Interleave.Expand32to64((uint)num);
			zz[5] = (ulong)Interleave.Expand8to16((uint)(num >> 32));
		}

		// Token: 0x0400162A RID: 5674
		private const ulong M35 = 34359738367UL;

		// Token: 0x0400162B RID: 5675
		private const ulong M55 = 36028797018963967UL;

		// Token: 0x0400162C RID: 5676
		private static readonly ulong[] ROOT_Z = new ulong[]
		{
			13176245766935393968UL,
			5270498306774195053UL,
			19634136210UL
		};
	}
}
