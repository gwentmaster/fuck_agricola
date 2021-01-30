using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000330 RID: 816
	internal class SecT131Field
	{
		// Token: 0x06001DCB RID: 7627 RVA: 0x000A62CF File Offset: 0x000A44CF
		public static void Add(ulong[] x, ulong[] y, ulong[] z)
		{
			z[0] = (x[0] ^ y[0]);
			z[1] = (x[1] ^ y[1]);
			z[2] = (x[2] ^ y[2]);
		}

		// Token: 0x06001DCC RID: 7628 RVA: 0x000A62EF File Offset: 0x000A44EF
		public static void AddExt(ulong[] xx, ulong[] yy, ulong[] zz)
		{
			zz[0] = (xx[0] ^ yy[0]);
			zz[1] = (xx[1] ^ yy[1]);
			zz[2] = (xx[2] ^ yy[2]);
			zz[3] = (xx[3] ^ yy[3]);
			zz[4] = (xx[4] ^ yy[4]);
		}

		// Token: 0x06001DCD RID: 7629 RVA: 0x000A6323 File Offset: 0x000A4523
		public static void AddOne(ulong[] x, ulong[] z)
		{
			z[0] = (x[0] ^ 1UL);
			z[1] = x[1];
			z[2] = x[2];
		}

		// Token: 0x06001DCE RID: 7630 RVA: 0x000A633A File Offset: 0x000A453A
		public static ulong[] FromBigInteger(BigInteger x)
		{
			ulong[] array = Nat192.FromBigInteger64(x);
			SecT131Field.Reduce61(array, 0);
			return array;
		}

		// Token: 0x06001DCF RID: 7631 RVA: 0x000A634C File Offset: 0x000A454C
		public static void Invert(ulong[] x, ulong[] z)
		{
			if (Nat192.IsZero64(x))
			{
				throw new InvalidOperationException();
			}
			ulong[] array = Nat192.Create64();
			ulong[] array2 = Nat192.Create64();
			SecT131Field.Square(x, array);
			SecT131Field.Multiply(array, x, array);
			SecT131Field.SquareN(array, 2, array2);
			SecT131Field.Multiply(array2, array, array2);
			SecT131Field.SquareN(array2, 4, array);
			SecT131Field.Multiply(array, array2, array);
			SecT131Field.SquareN(array, 8, array2);
			SecT131Field.Multiply(array2, array, array2);
			SecT131Field.SquareN(array2, 16, array);
			SecT131Field.Multiply(array, array2, array);
			SecT131Field.SquareN(array, 32, array2);
			SecT131Field.Multiply(array2, array, array2);
			SecT131Field.Square(array2, array2);
			SecT131Field.Multiply(array2, x, array2);
			SecT131Field.SquareN(array2, 65, array);
			SecT131Field.Multiply(array, array2, array);
			SecT131Field.Square(array, z);
		}

		// Token: 0x06001DD0 RID: 7632 RVA: 0x000A63FC File Offset: 0x000A45FC
		public static void Multiply(ulong[] x, ulong[] y, ulong[] z)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT131Field.ImplMultiply(x, y, array);
			SecT131Field.Reduce(array, z);
		}

		// Token: 0x06001DD1 RID: 7633 RVA: 0x000A6420 File Offset: 0x000A4620
		public static void MultiplyAddToExt(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong[] array = Nat192.CreateExt64();
			SecT131Field.ImplMultiply(x, y, array);
			SecT131Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001DD2 RID: 7634 RVA: 0x000A6444 File Offset: 0x000A4644
		public static void Reduce(ulong[] xx, ulong[] z)
		{
			ulong num = xx[0];
			ulong num2 = xx[1];
			ulong num3 = xx[2];
			ulong num4 = xx[3];
			ulong num5 = xx[4];
			num2 ^= (num5 << 61 ^ num5 << 63);
			num3 ^= (num5 >> 3 ^ num5 >> 1 ^ num5 ^ num5 << 5);
			num4 ^= num5 >> 59;
			num ^= (num4 << 61 ^ num4 << 63);
			num2 ^= (num4 >> 3 ^ num4 >> 1 ^ num4 ^ num4 << 5);
			num3 ^= num4 >> 59;
			ulong num6 = num3 >> 3;
			z[0] = (num ^ num6 ^ num6 << 2 ^ num6 << 3 ^ num6 << 8);
			z[1] = (num2 ^ num6 >> 56);
			z[2] = (num3 & 7UL);
		}

		// Token: 0x06001DD3 RID: 7635 RVA: 0x000A64E0 File Offset: 0x000A46E0
		public static void Reduce61(ulong[] z, int zOff)
		{
			ulong num = z[zOff + 2];
			ulong num2 = num >> 3;
			z[zOff] ^= (num2 ^ num2 << 2 ^ num2 << 3 ^ num2 << 8);
			z[zOff + 1] ^= num2 >> 56;
			z[zOff + 2] = (num & 7UL);
		}

		// Token: 0x06001DD4 RID: 7636 RVA: 0x000A652C File Offset: 0x000A472C
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
			SecT131Field.Multiply(array, SecT131Field.ROOT_Z, z);
			z[0] ^= num3;
			z[1] ^= num4;
		}

		// Token: 0x06001DD5 RID: 7637 RVA: 0x000A65A8 File Offset: 0x000A47A8
		public static void Square(ulong[] x, ulong[] z)
		{
			ulong[] array = Nat.Create64(5);
			SecT131Field.ImplSquare(x, array);
			SecT131Field.Reduce(array, z);
		}

		// Token: 0x06001DD6 RID: 7638 RVA: 0x000A65CC File Offset: 0x000A47CC
		public static void SquareAddToExt(ulong[] x, ulong[] zz)
		{
			ulong[] array = Nat.Create64(5);
			SecT131Field.ImplSquare(x, array);
			SecT131Field.AddExt(zz, array, zz);
		}

		// Token: 0x06001DD7 RID: 7639 RVA: 0x000A65F0 File Offset: 0x000A47F0
		public static void SquareN(ulong[] x, int n, ulong[] z)
		{
			ulong[] array = Nat.Create64(5);
			SecT131Field.ImplSquare(x, array);
			SecT131Field.Reduce(array, z);
			while (--n > 0)
			{
				SecT131Field.ImplSquare(z, array);
				SecT131Field.Reduce(array, z);
			}
		}

		// Token: 0x06001DD8 RID: 7640 RVA: 0x000A662B File Offset: 0x000A482B
		public static uint Trace(ulong[] x)
		{
			return (uint)(x[0] ^ x[1] >> 59 ^ x[2] >> 1) & 1U;
		}

		// Token: 0x06001DD9 RID: 7641 RVA: 0x000A6640 File Offset: 0x000A4840
		protected static void ImplCompactExt(ulong[] zz)
		{
			ulong num = zz[0];
			ulong num2 = zz[1];
			ulong num3 = zz[2];
			ulong num4 = zz[3];
			ulong num5 = zz[4];
			ulong num6 = zz[5];
			zz[0] = (num ^ num2 << 44);
			zz[1] = (num2 >> 20 ^ num3 << 24);
			zz[2] = (num3 >> 40 ^ num4 << 4 ^ num5 << 48);
			zz[3] = (num4 >> 60 ^ num6 << 28 ^ num5 >> 16);
			zz[4] = num6 >> 36;
			zz[5] = 0UL;
		}

		// Token: 0x06001DDA RID: 7642 RVA: 0x000A66B0 File Offset: 0x000A48B0
		protected static void ImplMultiply(ulong[] x, ulong[] y, ulong[] zz)
		{
			ulong num = x[0];
			ulong num2 = x[1];
			ulong num3 = x[2];
			num3 = ((num2 >> 24 ^ num3 << 40) & 17592186044415UL);
			num2 = ((num >> 44 ^ num2 << 20) & 17592186044415UL);
			num &= 17592186044415UL;
			ulong num4 = y[0];
			ulong num5 = y[1];
			ulong num6 = y[2];
			num6 = ((num5 >> 24 ^ num6 << 40) & 17592186044415UL);
			num5 = ((num4 >> 44 ^ num5 << 20) & 17592186044415UL);
			num4 &= 17592186044415UL;
			ulong[] array = new ulong[10];
			SecT131Field.ImplMulw(num, num4, array, 0);
			SecT131Field.ImplMulw(num3, num6, array, 2);
			ulong num7 = num ^ num2 ^ num3;
			ulong num8 = num4 ^ num5 ^ num6;
			SecT131Field.ImplMulw(num7, num8, array, 4);
			ulong num9 = num2 << 1 ^ num3 << 2;
			ulong num10 = num5 << 1 ^ num6 << 2;
			SecT131Field.ImplMulw(num ^ num9, num4 ^ num10, array, 6);
			SecT131Field.ImplMulw(num7 ^ num9, num8 ^ num10, array, 8);
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
			num20 ^= num19 >> 44;
			num19 &= 17592186044415UL;
			num21 ^= num20 >> 44;
			num20 &= 17592186044415UL;
			num19 = (num19 >> 1 ^ (num20 & 1UL) << 43);
			num20 = (num20 >> 1 ^ (num21 & 1UL) << 43);
			num21 >>= 1;
			num19 ^= num19 << 1;
			num19 ^= num19 << 2;
			num19 ^= num19 << 4;
			num19 ^= num19 << 8;
			num19 ^= num19 << 16;
			num19 ^= num19 << 32;
			num19 &= 17592186044415UL;
			num20 ^= num19 >> 43;
			num20 ^= num20 << 1;
			num20 ^= num20 << 2;
			num20 ^= num20 << 4;
			num20 ^= num20 << 8;
			num20 ^= num20 << 16;
			num20 ^= num20 << 32;
			num20 &= 17592186044415UL;
			num21 ^= num20 >> 43;
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
			SecT131Field.ImplCompactExt(zz);
		}

		// Token: 0x06001DDB RID: 7643 RVA: 0x000A69A0 File Offset: 0x000A4BA0
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
			ulong num3 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3 ^ array[(int)(num >> 6 & 7U)] << 6;
			int num4 = 33;
			do
			{
				num = (uint)(x >> num4);
				ulong num5 = array[(int)(num & 7U)] ^ array[(int)(num >> 3 & 7U)] << 3 ^ array[(int)(num >> 6 & 7U)] << 6 ^ array[(int)(num >> 9 & 7U)] << 9;
				num3 ^= num5 << num4;
				num2 ^= num5 >> -num4;
			}
			while ((num4 -= 12) > 0);
			z[zOff] = (num3 & 17592186044415UL);
			z[zOff + 1] = (num3 >> 44 ^ num2 << 20);
		}

		// Token: 0x06001DDC RID: 7644 RVA: 0x000A6A7C File Offset: 0x000A4C7C
		protected static void ImplSquare(ulong[] x, ulong[] zz)
		{
			Interleave.Expand64To128(x[0], zz, 0);
			Interleave.Expand64To128(x[1], zz, 2);
			zz[4] = (ulong)Interleave.Expand8to16((uint)x[2]);
		}

		// Token: 0x04001622 RID: 5666
		private const ulong M03 = 7UL;

		// Token: 0x04001623 RID: 5667
		private const ulong M44 = 17592186044415UL;

		// Token: 0x04001624 RID: 5668
		private static readonly ulong[] ROOT_Z = new ulong[]
		{
			2791191049453778211UL,
			2791191049453778402UL,
			6UL
		};
	}
}
