using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002C7 RID: 711
	internal abstract class Nat
	{
		// Token: 0x060017A5 RID: 6053 RVA: 0x000889CC File Offset: 0x00086BCC
		public static uint Add(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)y[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x060017A6 RID: 6054 RVA: 0x00088A00 File Offset: 0x00086C00
		public static uint Add33At(int len, uint x, uint[] z, int zPos)
		{
			ulong num = (ulong)z[zPos] + (ulong)x;
			z[zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 1] + 1UL;
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x060017A7 RID: 6055 RVA: 0x00088A48 File Offset: 0x00086C48
		public static uint Add33At(int len, uint x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)z[zOff + zPos] + (ulong)x;
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + zPos + 1] + 1UL;
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x060017A8 RID: 6056 RVA: 0x00088A9C File Offset: 0x00086C9C
		public static uint Add33To(int len, uint x, uint[] z)
		{
			ulong num = (ulong)z[0] + (ulong)x;
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)z[1] + 1UL;
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 2);
			}
			return 0U;
		}

		// Token: 0x060017A9 RID: 6057 RVA: 0x00088ADC File Offset: 0x00086CDC
		public static uint Add33To(int len, uint x, uint[] z, int zOff)
		{
			ulong num = (ulong)z[zOff] + (ulong)x;
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 1] + 1UL;
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x00088B24 File Offset: 0x00086D24
		public static uint AddBothTo(int len, uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)y[i] + (ulong)z[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00088B60 File Offset: 0x00086D60
		public static uint AddBothTo(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[xOff + i] + (ulong)y[yOff + i] + (ulong)z[zOff + i];
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x00088BA8 File Offset: 0x00086DA8
		public static uint AddDWordAt(int len, ulong x, uint[] z, int zPos)
		{
			ulong num = (ulong)z[zPos] + (x & (ulong)-1);
			z[zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 1] + (x >> 32);
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00088BF4 File Offset: 0x00086DF4
		public static uint AddDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)z[zOff + zPos] + (x & (ulong)-1);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + zPos + 1] + (x >> 32);
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 2);
			}
			return 0U;
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x00088C4C File Offset: 0x00086E4C
		public static uint AddDWordTo(int len, ulong x, uint[] z)
		{
			ulong num = (ulong)z[0] + (x & (ulong)-1);
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)z[1] + (x >> 32);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 2);
			}
			return 0U;
		}

		// Token: 0x060017AF RID: 6063 RVA: 0x00088C90 File Offset: 0x00086E90
		public static uint AddDWordTo(int len, ulong x, uint[] z, int zOff)
		{
			ulong num = (ulong)z[zOff] + (x & (ulong)-1);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 1] + (x >> 32);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x060017B0 RID: 6064 RVA: 0x00088CDC File Offset: 0x00086EDC
		public static uint AddTo(int len, uint[] x, uint[] z)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[i] + (ulong)z[i];
				z[i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x060017B1 RID: 6065 RVA: 0x00088D10 File Offset: 0x00086F10
		public static uint AddTo(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			for (int i = 0; i < len; i++)
			{
				num += (ulong)x[xOff + i] + (ulong)z[zOff + i];
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (uint)num;
		}

		// Token: 0x060017B2 RID: 6066 RVA: 0x00088D4C File Offset: 0x00086F4C
		public static uint AddWordAt(int len, uint x, uint[] z, int zPos)
		{
			ulong num = (ulong)x + (ulong)z[zPos];
			z[zPos] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 1);
			}
			return 0U;
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x00088D7C File Offset: 0x00086F7C
		public static uint AddWordAt(int len, uint x, uint[] z, int zOff, int zPos)
		{
			ulong num = (ulong)x + (ulong)z[zOff + zPos];
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, zPos + 1);
			}
			return 0U;
		}

		// Token: 0x060017B4 RID: 6068 RVA: 0x00088DB4 File Offset: 0x00086FB4
		public static uint AddWordTo(int len, uint x, uint[] z)
		{
			ulong num = (ulong)x + (ulong)z[0];
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, 1);
			}
			return 0U;
		}

		// Token: 0x060017B5 RID: 6069 RVA: 0x00088DE0 File Offset: 0x00086FE0
		public static uint AddWordTo(int len, uint x, uint[] z, int zOff)
		{
			ulong num = (ulong)x + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zOff, 1);
			}
			return 0U;
		}

		// Token: 0x060017B6 RID: 6070 RVA: 0x00088E0D File Offset: 0x0008700D
		public static void Copy(int len, uint[] x, uint[] z)
		{
			Array.Copy(x, 0, z, 0, len);
		}

		// Token: 0x060017B7 RID: 6071 RVA: 0x00088E1C File Offset: 0x0008701C
		public static uint[] Copy(int len, uint[] x)
		{
			uint[] array = new uint[len];
			Array.Copy(x, 0, array, 0, len);
			return array;
		}

		// Token: 0x060017B8 RID: 6072 RVA: 0x00088E3B File Offset: 0x0008703B
		public static uint[] Create(int len)
		{
			return new uint[len];
		}

		// Token: 0x060017B9 RID: 6073 RVA: 0x00088E43 File Offset: 0x00087043
		public static ulong[] Create64(int len)
		{
			return new ulong[len];
		}

		// Token: 0x060017BA RID: 6074 RVA: 0x00088E4C File Offset: 0x0008704C
		public static int Dec(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x060017BB RID: 6075 RVA: 0x00088E7C File Offset: 0x0008707C
		public static int Dec(int len, uint[] x, uint[] z)
		{
			int i = 0;
			while (i < len)
			{
				uint num = x[i] - 1U;
				z[i] = num;
				i++;
				if (num != 4294967295U)
				{
					while (i < len)
					{
						z[i] = x[i];
						i++;
					}
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x060017BC RID: 6076 RVA: 0x00088EB8 File Offset: 0x000870B8
		public static int DecAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x060017BD RID: 6077 RVA: 0x00088EE8 File Offset: 0x000870E8
		public static int DecAt(int len, uint[] z, int zOff, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = zOff + i;
				uint num2 = z[num] - 1U;
				z[num] = num2;
				if (num2 != 4294967295U)
				{
					return 0;
				}
			}
			return -1;
		}

		// Token: 0x060017BE RID: 6078 RVA: 0x00088F18 File Offset: 0x00087118
		public static bool Eq(int len, uint[] x, uint[] y)
		{
			for (int i = len - 1; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060017BF RID: 6079 RVA: 0x00088F40 File Offset: 0x00087140
		public static uint[] FromBigInteger(int bits, BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > bits)
			{
				throw new ArgumentException();
			}
			uint[] array = Nat.Create(bits + 31 >> 5);
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (uint)x.IntValue;
				x = x.ShiftRight(32);
			}
			return array;
		}

		// Token: 0x060017C0 RID: 6080 RVA: 0x00088F98 File Offset: 0x00087198
		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1U;
			}
			int num = bit >> 5;
			if (num < 0 || num >= x.Length)
			{
				return 0U;
			}
			int num2 = bit & 31;
			return x[num] >> num2 & 1U;
		}

		// Token: 0x060017C1 RID: 6081 RVA: 0x00088FD0 File Offset: 0x000871D0
		public static bool Gte(int len, uint[] x, uint[] y)
		{
			for (int i = len - 1; i >= 0; i--)
			{
				uint num = x[i];
				uint num2 = y[i];
				if (num < num2)
				{
					return false;
				}
				if (num > num2)
				{
					return true;
				}
			}
			return true;
		}

		// Token: 0x060017C2 RID: 6082 RVA: 0x00089000 File Offset: 0x00087200
		public static uint Inc(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x060017C3 RID: 6083 RVA: 0x00089030 File Offset: 0x00087230
		public static uint Inc(int len, uint[] x, uint[] z)
		{
			int i = 0;
			while (i < len)
			{
				uint num = x[i] + 1U;
				z[i] = num;
				i++;
				if (num != 0U)
				{
					while (i < len)
					{
						z[i] = x[i];
						i++;
					}
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x060017C4 RID: 6084 RVA: 0x0008906C File Offset: 0x0008726C
		public static uint IncAt(int len, uint[] z, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x060017C5 RID: 6085 RVA: 0x0008909C File Offset: 0x0008729C
		public static uint IncAt(int len, uint[] z, int zOff, int zPos)
		{
			for (int i = zPos; i < len; i++)
			{
				int num = zOff + i;
				uint num2 = z[num] + 1U;
				z[num] = num2;
				if (num2 != 0U)
				{
					return 0U;
				}
			}
			return 1U;
		}

		// Token: 0x060017C6 RID: 6086 RVA: 0x000890CC File Offset: 0x000872CC
		public static bool IsOne(int len, uint[] x)
		{
			if (x[0] != 1U)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060017C7 RID: 6087 RVA: 0x000890F8 File Offset: 0x000872F8
		public static bool IsZero(int len, uint[] x)
		{
			if (x[0] != 0U)
			{
				return false;
			}
			for (int i = 1; i < len; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060017C8 RID: 6088 RVA: 0x00089120 File Offset: 0x00087320
		public static void Mul(int len, uint[] x, uint[] y, uint[] zz)
		{
			zz[len] = Nat.MulWord(len, x[0], y, zz);
			for (int i = 1; i < len; i++)
			{
				zz[i + len] = Nat.MulWordAddTo(len, x[i], y, 0, zz, i);
			}
		}

		// Token: 0x060017C9 RID: 6089 RVA: 0x0008915C File Offset: 0x0008735C
		public static void Mul(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			zz[zzOff + len] = Nat.MulWord(len, x[xOff], y, yOff, zz, zzOff);
			for (int i = 1; i < len; i++)
			{
				zz[zzOff + i + len] = Nat.MulWordAddTo(len, x[xOff + i], y, yOff, zz, zzOff + i);
			}
		}

		// Token: 0x060017CA RID: 6090 RVA: 0x000891AC File Offset: 0x000873AC
		public static uint Mul31BothAdd(int len, uint a, uint[] x, uint b, uint[] y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)a;
			ulong num3 = (ulong)b;
			int num4 = 0;
			do
			{
				num += num2 * (ulong)x[num4] + num3 * (ulong)y[num4] + (ulong)z[zOff + num4];
				z[zOff + num4] = (uint)num;
				num >>= 32;
			}
			while (++num4 < len);
			return (uint)num;
		}

		// Token: 0x060017CB RID: 6091 RVA: 0x000891F8 File Offset: 0x000873F8
		public static uint MulWord(int len, uint x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[num3];
				z[num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x060017CC RID: 6092 RVA: 0x0008922C File Offset: 0x0008742C
		public static uint MulWord(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[yOff + num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x060017CD RID: 6093 RVA: 0x00089264 File Offset: 0x00087464
		public static uint MulWordAddTo(int len, uint x, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[yOff + num3] + (ulong)z[zOff + num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < len);
			return (uint)num;
		}

		// Token: 0x060017CE RID: 6094 RVA: 0x000892A8 File Offset: 0x000874A8
		public static uint MulWordDwordAddAt(int len, uint x, ulong y, uint[] z, int zPos)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			num += num2 * (ulong)((uint)y) + (ulong)z[zPos];
			z[zPos] = (uint)num;
			num >>= 32;
			num += num2 * (y >> 32) + (ulong)z[zPos + 1];
			z[zPos + 1] = (uint)num;
			num >>= 32;
			num += (ulong)z[zPos + 2];
			z[zPos + 2] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(len, z, zPos + 3);
			}
			return 0U;
		}

		// Token: 0x060017CF RID: 6095 RVA: 0x0008931C File Offset: 0x0008751C
		public static uint ShiftDownBit(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x060017D0 RID: 6096 RVA: 0x0008934C File Offset: 0x0008754C
		public static uint ShiftDownBit(int len, uint[] z, int zOff, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[zOff + num];
				z[zOff + num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x060017D1 RID: 6097 RVA: 0x00089380 File Offset: 0x00087580
		public static uint ShiftDownBit(int len, uint[] x, uint c, uint[] z)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[num];
				z[num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x060017D2 RID: 6098 RVA: 0x000893B0 File Offset: 0x000875B0
		public static uint ShiftDownBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[xOff + num];
				z[zOff + num] = (num2 >> 1 | c << 31);
				c = num2;
			}
			return c << 31;
		}

		// Token: 0x060017D3 RID: 6099 RVA: 0x000893E8 File Offset: 0x000875E8
		public static uint ShiftDownBits(int len, uint[] z, int bits, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x060017D4 RID: 6100 RVA: 0x00089420 File Offset: 0x00087620
		public static uint ShiftDownBits(int len, uint[] z, int zOff, int bits, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[zOff + num];
				z[zOff + num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x060017D5 RID: 6101 RVA: 0x00089460 File Offset: 0x00087660
		public static uint ShiftDownBits(int len, uint[] x, int bits, uint c, uint[] z)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[num];
				z[num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x060017D6 RID: 6102 RVA: 0x0008949C File Offset: 0x0008769C
		public static uint ShiftDownBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = x[xOff + num];
				z[zOff + num] = (num2 >> bits | c << -bits);
				c = num2;
			}
			return c << -bits;
		}

		// Token: 0x060017D7 RID: 6103 RVA: 0x000894DC File Offset: 0x000876DC
		public static uint ShiftDownWord(int len, uint[] z, uint c)
		{
			int num = len;
			while (--num >= 0)
			{
				uint num2 = z[num];
				z[num] = c;
				c = num2;
			}
			return c;
		}

		// Token: 0x060017D8 RID: 6104 RVA: 0x00089500 File Offset: 0x00087700
		public static uint ShiftUpBit(int len, uint[] z, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[i];
				z[i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x00089530 File Offset: 0x00087730
		public static uint ShiftUpBit(int len, uint[] z, int zOff, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[zOff + i];
				z[zOff + i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x00089564 File Offset: 0x00087764
		public static uint ShiftUpBit(int len, uint[] x, uint c, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				z[i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x00089594 File Offset: 0x00087794
		public static uint ShiftUpBit(int len, uint[] x, int xOff, uint c, uint[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[xOff + i];
				z[zOff + i] = (num << 1 | c >> 31);
				c = num;
			}
			return c >> 31;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000895CC File Offset: 0x000877CC
		public static ulong ShiftUpBit64(int len, ulong[] x, int xOff, ulong c, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = x[xOff + i];
				z[zOff + i] = (num << 1 | c >> 63);
				c = num;
			}
			return c >> 63;
		}

		// Token: 0x060017DD RID: 6109 RVA: 0x00089604 File Offset: 0x00087804
		public static uint ShiftUpBits(int len, uint[] z, int bits, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[i];
				z[i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017DE RID: 6110 RVA: 0x0008963C File Offset: 0x0008783C
		public static uint ShiftUpBits(int len, uint[] z, int zOff, int bits, uint c)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = z[zOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017DF RID: 6111 RVA: 0x0008967C File Offset: 0x0008787C
		public static ulong ShiftUpBits64(int len, ulong[] z, int zOff, int bits, ulong c)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = z[zOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x000896BC File Offset: 0x000878BC
		public static uint ShiftUpBits(int len, uint[] x, int bits, uint c, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				z[i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x000896F8 File Offset: 0x000878F8
		public static uint ShiftUpBits(int len, uint[] x, int xOff, int bits, uint c, uint[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				uint num = x[xOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x00089738 File Offset: 0x00087938
		public static ulong ShiftUpBits64(int len, ulong[] x, int xOff, int bits, ulong c, ulong[] z, int zOff)
		{
			for (int i = 0; i < len; i++)
			{
				ulong num = x[xOff + i];
				z[zOff + i] = (num << bits | c >> -bits);
				c = num;
			}
			return c >> -bits;
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x00089778 File Offset: 0x00087978
		public static void Square(int len, uint[] x, uint[] zz)
		{
			int num = len << 1;
			uint num2 = 0U;
			int num3 = len;
			int num4 = num;
			do
			{
				ulong num5 = (ulong)x[--num3];
				ulong num6 = num5 * num5;
				zz[--num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[--num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			for (int i = 1; i < len; i++)
			{
				num2 = Nat.SquareWordAdd(x, i, zz);
				Nat.AddWordAt(num, num2, zz, i << 1);
			}
			Nat.ShiftUpBit(num, zz, x[0] << 31);
		}

		// Token: 0x060017E4 RID: 6116 RVA: 0x000897F8 File Offset: 0x000879F8
		public static void Square(int len, uint[] x, int xOff, uint[] zz, int zzOff)
		{
			int num = len << 1;
			uint num2 = 0U;
			int num3 = len;
			int num4 = num;
			do
			{
				ulong num5 = (ulong)x[xOff + --num3];
				ulong num6 = num5 * num5;
				zz[zzOff + --num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[zzOff + --num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			for (int i = 1; i < len; i++)
			{
				num2 = Nat.SquareWordAdd(x, xOff, i, zz, zzOff);
				Nat.AddWordAt(num, num2, zz, zzOff, i << 1);
			}
			Nat.ShiftUpBit(num, zz, zzOff, x[xOff] << 31);
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00089888 File Offset: 0x00087A88
		public static uint SquareWordAdd(uint[] x, int xPos, uint[] z)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x[xPos];
			int num3 = 0;
			do
			{
				num += num2 * (ulong)x[num3] + (ulong)z[xPos + num3];
				z[xPos + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < xPos);
			return (uint)num;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x000898C8 File Offset: 0x00087AC8
		public static uint SquareWordAdd(uint[] x, int xOff, int xPos, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x[xOff + xPos];
			int num3 = 0;
			do
			{
				num += num2 * ((ulong)x[xOff + num3] & (ulong)-1) + ((ulong)z[xPos + zOff] & (ulong)-1);
				z[xPos + zOff] = (uint)num;
				num >>= 32;
				zOff++;
			}
			while (++num3 < xPos);
			return (uint)num;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x00089918 File Offset: 0x00087B18
		public static int Sub(int len, uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)x[i] - (ulong)y[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x0008994C File Offset: 0x00087B4C
		public static int Sub(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)x[xOff + i] - (ulong)y[yOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x0008998C File Offset: 0x00087B8C
		public static int Sub33At(int len, uint x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (ulong)x);
			z[zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zPos + 1] - 1UL);
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 2);
			}
			return 0;
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x000899D4 File Offset: 0x00087BD4
		public static int Sub33At(int len, uint x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (ulong)x);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + zPos + 1] - 1UL);
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 2);
			}
			return 0;
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00089A28 File Offset: 0x00087C28
		public static int Sub33From(int len, uint x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (ulong)x);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - 1UL);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 2);
			}
			return 0;
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00089A68 File Offset: 0x00087C68
		public static int Sub33From(int len, uint x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (ulong)x);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 1] - 1UL);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 2);
			}
			return 0;
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00089AB0 File Offset: 0x00087CB0
		public static int SubBothFrom(int len, uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[i] - (ulong)x[i] - (ulong)y[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x00089AEC File Offset: 0x00087CEC
		public static int SubBothFrom(int len, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[zOff + i] - (ulong)x[xOff + i] - (ulong)y[yOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00089B34 File Offset: 0x00087D34
		public static int SubDWordAt(int len, ulong x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (x & (ulong)-1));
			z[zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zPos + 1] - (x >> 32));
			z[zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 2);
			}
			return 0;
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00089B80 File Offset: 0x00087D80
		public static int SubDWordAt(int len, ulong x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (x & (ulong)-1));
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + zPos + 1] - (x >> 32));
			z[zOff + zPos + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 2);
			}
			return 0;
		}

		// Token: 0x060017F1 RID: 6129 RVA: 0x00089BD8 File Offset: 0x00087DD8
		public static int SubDWordFrom(int len, ulong x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (x & (ulong)-1));
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - (x >> 32));
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 2);
			}
			return 0;
		}

		// Token: 0x060017F2 RID: 6130 RVA: 0x00089C1C File Offset: 0x00087E1C
		public static int SubDWordFrom(int len, ulong x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (x & (ulong)-1));
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 1] - (x >> 32));
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 2);
			}
			return 0;
		}

		// Token: 0x060017F3 RID: 6131 RVA: 0x00089C68 File Offset: 0x00087E68
		public static int SubFrom(int len, uint[] x, uint[] z)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[i] - (ulong)x[i]);
				z[i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017F4 RID: 6132 RVA: 0x00089C9C File Offset: 0x00087E9C
		public static int SubFrom(int len, uint[] x, int xOff, uint[] z, int zOff)
		{
			long num = 0L;
			for (int i = 0; i < len; i++)
			{
				num += (long)((ulong)z[zOff + i] - (ulong)x[xOff + i]);
				z[zOff + i] = (uint)num;
				num >>= 32;
			}
			return (int)num;
		}

		// Token: 0x060017F5 RID: 6133 RVA: 0x00089CD8 File Offset: 0x00087ED8
		public static int SubWordAt(int len, uint x, uint[] z, int zPos)
		{
			long num = (long)((ulong)z[zPos] - (ulong)x);
			z[zPos] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zPos + 1);
			}
			return 0;
		}

		// Token: 0x060017F6 RID: 6134 RVA: 0x00089D08 File Offset: 0x00087F08
		public static int SubWordAt(int len, uint x, uint[] z, int zOff, int zPos)
		{
			long num = (long)((ulong)z[zOff + zPos] - (ulong)x);
			z[zOff + zPos] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, zPos + 1);
			}
			return 0;
		}

		// Token: 0x060017F7 RID: 6135 RVA: 0x00089D40 File Offset: 0x00087F40
		public static int SubWordFrom(int len, uint x, uint[] z)
		{
			long num = (long)((ulong)z[0] - (ulong)x);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, 1);
			}
			return 0;
		}

		// Token: 0x060017F8 RID: 6136 RVA: 0x00089D6C File Offset: 0x00087F6C
		public static int SubWordFrom(int len, uint x, uint[] z, int zOff)
		{
			long num = (long)((ulong)z[zOff] - (ulong)x);
			z[zOff] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				return Nat.DecAt(len, z, zOff, 1);
			}
			return 0;
		}

		// Token: 0x060017F9 RID: 6137 RVA: 0x00089D9C File Offset: 0x00087F9C
		public static BigInteger ToBigInteger(int len, uint[] x)
		{
			byte[] array = new byte[len << 2];
			for (int i = 0; i < len; i++)
			{
				uint num = x[i];
				if (num != 0U)
				{
					Pack.UInt32_To_BE(num, array, len - 1 - i << 2);
				}
			}
			return new BigInteger(1, array);
		}

		// Token: 0x060017FA RID: 6138 RVA: 0x00089DDC File Offset: 0x00087FDC
		public static void Zero(int len, uint[] z)
		{
			for (int i = 0; i < len; i++)
			{
				z[i] = 0U;
			}
		}

		// Token: 0x0400154E RID: 5454
		private const ulong M = 4294967295UL;
	}
}
