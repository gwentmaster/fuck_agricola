using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002C8 RID: 712
	internal abstract class Nat128
	{
		// Token: 0x060017FC RID: 6140 RVA: 0x00089DFC File Offset: 0x00087FFC
		public static uint Add(uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			num += (ulong)x[0] + (ulong)y[0];
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)x[1] + (ulong)y[1];
			z[1] = (uint)num;
			num >>= 32;
			num += (ulong)x[2] + (ulong)y[2];
			z[2] = (uint)num;
			num >>= 32;
			num += (ulong)x[3] + (ulong)y[3];
			z[3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x060017FD RID: 6141 RVA: 0x00089E68 File Offset: 0x00088068
		public static uint AddBothTo(uint[] x, uint[] y, uint[] z)
		{
			ulong num = 0UL;
			num += (ulong)x[0] + (ulong)y[0] + (ulong)z[0];
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)x[1] + (ulong)y[1] + (ulong)z[1];
			z[1] = (uint)num;
			num >>= 32;
			num += (ulong)x[2] + (ulong)y[2] + (ulong)z[2];
			z[2] = (uint)num;
			num >>= 32;
			num += (ulong)x[3] + (ulong)y[3] + (ulong)z[3];
			z[3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x060017FE RID: 6142 RVA: 0x00089EE8 File Offset: 0x000880E8
		public static uint AddTo(uint[] x, uint[] z)
		{
			ulong num = 0UL;
			num += (ulong)x[0] + (ulong)z[0];
			z[0] = (uint)num;
			num >>= 32;
			num += (ulong)x[1] + (ulong)z[1];
			z[1] = (uint)num;
			num >>= 32;
			num += (ulong)x[2] + (ulong)z[2];
			z[2] = (uint)num;
			num >>= 32;
			num += (ulong)x[3] + (ulong)z[3];
			z[3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x060017FF RID: 6143 RVA: 0x00089F54 File Offset: 0x00088154
		public static uint AddTo(uint[] x, int xOff, uint[] z, int zOff, uint cIn)
		{
			ulong num = (ulong)cIn;
			num += (ulong)x[xOff] + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)x[xOff + 1] + (ulong)z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += (ulong)x[xOff + 2] + (ulong)z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			num += (ulong)x[xOff + 3] + (ulong)z[zOff + 3];
			z[zOff + 3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00089FD4 File Offset: 0x000881D4
		public static uint AddToEachOther(uint[] u, int uOff, uint[] v, int vOff)
		{
			ulong num = 0UL;
			num += (ulong)u[uOff] + (ulong)v[vOff];
			u[uOff] = (uint)num;
			v[vOff] = (uint)num;
			num >>= 32;
			num += (ulong)u[uOff + 1] + (ulong)v[vOff + 1];
			u[uOff + 1] = (uint)num;
			v[vOff + 1] = (uint)num;
			num >>= 32;
			num += (ulong)u[uOff + 2] + (ulong)v[vOff + 2];
			u[uOff + 2] = (uint)num;
			v[vOff + 2] = (uint)num;
			num >>= 32;
			num += (ulong)u[uOff + 3] + (ulong)v[vOff + 3];
			u[uOff + 3] = (uint)num;
			v[vOff + 3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x0008A06A File Offset: 0x0008826A
		public static void Copy(uint[] x, uint[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x0008A084 File Offset: 0x00088284
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
		}

		// Token: 0x06001803 RID: 6147 RVA: 0x0008A092 File Offset: 0x00088292
		public static uint[] Create()
		{
			return new uint[4];
		}

		// Token: 0x06001804 RID: 6148 RVA: 0x0008A09A File Offset: 0x0008829A
		public static ulong[] Create64()
		{
			return new ulong[2];
		}

		// Token: 0x06001805 RID: 6149 RVA: 0x0008A0A2 File Offset: 0x000882A2
		public static uint[] CreateExt()
		{
			return new uint[8];
		}

		// Token: 0x06001806 RID: 6150 RVA: 0x0008A0AA File Offset: 0x000882AA
		public static ulong[] CreateExt64()
		{
			return new ulong[4];
		}

		// Token: 0x06001807 RID: 6151 RVA: 0x0008A0B2 File Offset: 0x000882B2
		public static bool Diff(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			bool flag = Nat128.Gte(x, xOff, y, yOff);
			if (flag)
			{
				Nat128.Sub(x, xOff, y, yOff, z, zOff);
				return flag;
			}
			Nat128.Sub(y, yOff, x, xOff, z, zOff);
			return flag;
		}

		// Token: 0x06001808 RID: 6152 RVA: 0x0008A0E0 File Offset: 0x000882E0
		public static bool Eq(uint[] x, uint[] y)
		{
			for (int i = 3; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001809 RID: 6153 RVA: 0x0008A104 File Offset: 0x00088304
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 1; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600180A RID: 6154 RVA: 0x0008A128 File Offset: 0x00088328
		public static uint[] FromBigInteger(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 128)
			{
				throw new ArgumentException();
			}
			uint[] array = Nat128.Create();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (uint)x.IntValue;
				x = x.ShiftRight(32);
			}
			return array;
		}

		// Token: 0x0600180B RID: 6155 RVA: 0x0008A17C File Offset: 0x0008837C
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 128)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat128.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x0600180C RID: 6156 RVA: 0x0008A1D0 File Offset: 0x000883D0
		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1U;
			}
			if ((bit & 127) != bit)
			{
				return 0U;
			}
			int num = bit >> 5;
			int num2 = bit & 31;
			return x[num] >> num2 & 1U;
		}

		// Token: 0x0600180D RID: 6157 RVA: 0x0008A204 File Offset: 0x00088404
		public static bool Gte(uint[] x, uint[] y)
		{
			for (int i = 3; i >= 0; i--)
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

		// Token: 0x0600180E RID: 6158 RVA: 0x0008A234 File Offset: 0x00088434
		public static bool Gte(uint[] x, int xOff, uint[] y, int yOff)
		{
			for (int i = 3; i >= 0; i--)
			{
				uint num = x[xOff + i];
				uint num2 = y[yOff + i];
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

		// Token: 0x0600180F RID: 6159 RVA: 0x0008A268 File Offset: 0x00088468
		public static bool IsOne(uint[] x)
		{
			if (x[0] != 1U)
			{
				return false;
			}
			for (int i = 1; i < 4; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001810 RID: 6160 RVA: 0x0008A294 File Offset: 0x00088494
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 2; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001811 RID: 6161 RVA: 0x0008A2C0 File Offset: 0x000884C0
		public static bool IsZero(uint[] x)
		{
			for (int i = 0; i < 4; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001812 RID: 6162 RVA: 0x0008A2E4 File Offset: 0x000884E4
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 2; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001813 RID: 6163 RVA: 0x0008A308 File Offset: 0x00088508
		public static void Mul(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = (ulong)y[0];
			ulong num2 = (ulong)y[1];
			ulong num3 = (ulong)y[2];
			ulong num4 = (ulong)y[3];
			ulong num5 = 0UL;
			ulong num6 = (ulong)x[0];
			num5 += num6 * num;
			zz[0] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num2;
			zz[1] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num3;
			zz[2] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num4;
			zz[3] = (uint)num5;
			num5 >>= 32;
			zz[4] = (uint)num5;
			for (int i = 1; i < 4; i++)
			{
				ulong num7 = 0UL;
				ulong num8 = (ulong)x[i];
				num7 += num8 * num + (ulong)zz[i];
				zz[i] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num2 + (ulong)zz[i + 1];
				zz[i + 1] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num3 + (ulong)zz[i + 2];
				zz[i + 2] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num4 + (ulong)zz[i + 3];
				zz[i + 3] = (uint)num7;
				num7 >>= 32;
				zz[i + 4] = (uint)num7;
			}
		}

		// Token: 0x06001814 RID: 6164 RVA: 0x0008A43C File Offset: 0x0008863C
		public static void Mul(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)y[yOff];
			ulong num2 = (ulong)y[yOff + 1];
			ulong num3 = (ulong)y[yOff + 2];
			ulong num4 = (ulong)y[yOff + 3];
			ulong num5 = 0UL;
			ulong num6 = (ulong)x[xOff];
			num5 += num6 * num;
			zz[zzOff] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num2;
			zz[zzOff + 1] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num3;
			zz[zzOff + 2] = (uint)num5;
			num5 >>= 32;
			num5 += num6 * num4;
			zz[zzOff + 3] = (uint)num5;
			num5 >>= 32;
			zz[zzOff + 4] = (uint)num5;
			for (int i = 1; i < 4; i++)
			{
				zzOff++;
				ulong num7 = 0UL;
				ulong num8 = (ulong)x[xOff + i];
				num7 += num8 * num + (ulong)zz[zzOff];
				zz[zzOff] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num2 + (ulong)zz[zzOff + 1];
				zz[zzOff + 1] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num3 + (ulong)zz[zzOff + 2];
				zz[zzOff + 2] = (uint)num7;
				num7 >>= 32;
				num7 += num8 * num4 + (ulong)zz[zzOff + 3];
				zz[zzOff + 3] = (uint)num7;
				num7 >>= 32;
				zz[zzOff + 4] = (uint)num7;
			}
		}

		// Token: 0x06001815 RID: 6165 RVA: 0x0008A598 File Offset: 0x00088798
		public static uint MulAddTo(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = (ulong)y[0];
			ulong num2 = (ulong)y[1];
			ulong num3 = (ulong)y[2];
			ulong num4 = (ulong)y[3];
			ulong num5 = 0UL;
			for (int i = 0; i < 4; i++)
			{
				ulong num6 = 0UL;
				ulong num7 = (ulong)x[i];
				num6 += num7 * num + (ulong)zz[i];
				zz[i] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num2 + (ulong)zz[i + 1];
				zz[i + 1] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num3 + (ulong)zz[i + 2];
				zz[i + 2] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num4 + (ulong)zz[i + 3];
				zz[i + 3] = (uint)num6;
				num6 >>= 32;
				num6 += num5 + (ulong)zz[i + 4];
				zz[i + 4] = (uint)num6;
				num5 = num6 >> 32;
			}
			return (uint)num5;
		}

		// Token: 0x06001816 RID: 6166 RVA: 0x0008A680 File Offset: 0x00088880
		public static uint MulAddTo(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)y[yOff];
			ulong num2 = (ulong)y[yOff + 1];
			ulong num3 = (ulong)y[yOff + 2];
			ulong num4 = (ulong)y[yOff + 3];
			ulong num5 = 0UL;
			for (int i = 0; i < 4; i++)
			{
				ulong num6 = 0UL;
				ulong num7 = (ulong)x[xOff + i];
				num6 += num7 * num + (ulong)zz[zzOff];
				zz[zzOff] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num2 + (ulong)zz[zzOff + 1];
				zz[zzOff + 1] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num3 + (ulong)zz[zzOff + 2];
				zz[zzOff + 2] = (uint)num6;
				num6 >>= 32;
				num6 += num7 * num4 + (ulong)zz[zzOff + 3];
				zz[zzOff + 3] = (uint)num6;
				num6 >>= 32;
				num6 += num5 + (ulong)zz[zzOff + 4];
				zz[zzOff + 4] = (uint)num6;
				num5 = num6 >> 32;
				zzOff++;
			}
			return (uint)num5;
		}

		// Token: 0x06001817 RID: 6167 RVA: 0x0008A780 File Offset: 0x00088980
		public static ulong Mul33Add(uint w, uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)w;
			ulong num3 = (ulong)x[xOff];
			num += num2 * num3 + (ulong)y[yOff];
			z[zOff] = (uint)num;
			num >>= 32;
			ulong num4 = (ulong)x[xOff + 1];
			num += num2 * num4 + num3 + (ulong)y[yOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			ulong num5 = (ulong)x[xOff + 2];
			num += num2 * num5 + num4 + (ulong)y[yOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			ulong num6 = (ulong)x[xOff + 3];
			num += num2 * num6 + num5 + (ulong)y[yOff + 3];
			z[zOff + 3] = (uint)num;
			num >>= 32;
			return num + num6;
		}

		// Token: 0x06001818 RID: 6168 RVA: 0x0008A82C File Offset: 0x00088A2C
		public static uint MulWordAddExt(uint x, uint[] yy, int yyOff, uint[] zz, int zzOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			num += num2 * (ulong)yy[yyOff] + (ulong)zz[zzOff];
			zz[zzOff] = (uint)num;
			num >>= 32;
			num += num2 * (ulong)yy[yyOff + 1] + (ulong)zz[zzOff + 1];
			zz[zzOff + 1] = (uint)num;
			num >>= 32;
			num += num2 * (ulong)yy[yyOff + 2] + (ulong)zz[zzOff + 2];
			zz[zzOff + 2] = (uint)num;
			num >>= 32;
			num += num2 * (ulong)yy[yyOff + 3] + (ulong)zz[zzOff + 3];
			zz[zzOff + 3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x06001819 RID: 6169 RVA: 0x0008A8BC File Offset: 0x00088ABC
		public static uint Mul33DWordAdd(uint x, ulong y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			ulong num3 = y & (ulong)-1;
			num += num2 * num3 + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			ulong num4 = y >> 32;
			num += num2 * num4 + num3 + (ulong)z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += num4 + (ulong)z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 3];
			z[zOff + 3] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600181A RID: 6170 RVA: 0x0008A938 File Offset: 0x00088B38
		public static uint Mul33WordAdd(uint x, uint y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)y;
			num += num2 * (ulong)x + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += num2 + (ulong)z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(4, z, zOff, 3);
			}
			return 0U;
		}

		// Token: 0x0600181B RID: 6171 RVA: 0x0008A99C File Offset: 0x00088B9C
		public static uint MulWordDwordAdd(uint x, ulong y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			num += num2 * y + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += num2 * (y >> 32) + (ulong)z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 2];
			z[zOff + 2] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(4, z, zOff, 3);
			}
			return 0U;
		}

		// Token: 0x0600181C RID: 6172 RVA: 0x0008AA04 File Offset: 0x00088C04
		public static uint MulWordsAdd(uint x, uint y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			ulong num3 = (ulong)y;
			num += num3 * num2 + (ulong)z[zOff];
			z[zOff] = (uint)num;
			num >>= 32;
			num += (ulong)z[zOff + 1];
			z[zOff + 1] = (uint)num;
			num >>= 32;
			if (num != 0UL)
			{
				return Nat.IncAt(4, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x0600181D RID: 6173 RVA: 0x0008AA54 File Offset: 0x00088C54
		public static uint MulWord(uint x, uint[] y, uint[] z, int zOff)
		{
			ulong num = 0UL;
			ulong num2 = (ulong)x;
			int num3 = 0;
			do
			{
				num += num2 * (ulong)y[num3];
				z[zOff + num3] = (uint)num;
				num >>= 32;
			}
			while (++num3 < 4);
			return (uint)num;
		}

		// Token: 0x0600181E RID: 6174 RVA: 0x0008AA88 File Offset: 0x00088C88
		public static void Square(uint[] x, uint[] zz)
		{
			ulong num = (ulong)x[0];
			uint num2 = 0U;
			int num3 = 3;
			int num4 = 8;
			do
			{
				ulong num5 = (ulong)x[num3--];
				ulong num6 = num5 * num5;
				zz[--num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[--num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			ulong num7 = num * num;
			ulong num8 = (ulong)((ulong)num2 << 31) | num7 >> 33;
			zz[0] = (uint)num7;
			num2 = ((uint)(num7 >> 32) & 1U);
			ulong num9 = (ulong)x[1];
			ulong num10 = (ulong)zz[2];
			num8 += num9 * num;
			uint num11 = (uint)num8;
			zz[1] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num10 += num8 >> 32;
			ulong num12 = (ulong)x[2];
			ulong num13 = (ulong)zz[3];
			ulong num14 = (ulong)zz[4];
			num10 += num12 * num;
			num11 = (uint)num10;
			zz[2] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num13 += (num10 >> 32) + num12 * num9;
			num14 += num13 >> 32;
			num13 &= (ulong)-1;
			ulong num15 = (ulong)x[3];
			ulong num16 = (ulong)zz[5];
			ulong num17 = (ulong)zz[6];
			num13 += num15 * num;
			num11 = (uint)num13;
			zz[3] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num14 += (num13 >> 32) + num15 * num9;
			num16 += (num14 >> 32) + num15 * num12;
			num17 += num16 >> 32;
			num11 = (uint)num14;
			zz[4] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num16;
			zz[5] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num17;
			zz[6] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = zz[7] + (uint)(num17 >> 32);
			zz[7] = (num11 << 1 | num2);
		}

		// Token: 0x0600181F RID: 6175 RVA: 0x0008AC10 File Offset: 0x00088E10
		public static void Square(uint[] x, int xOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)x[xOff];
			uint num2 = 0U;
			int num3 = 3;
			int num4 = 8;
			do
			{
				ulong num5 = (ulong)x[xOff + num3--];
				ulong num6 = num5 * num5;
				zz[zzOff + --num4] = (num2 << 31 | (uint)(num6 >> 33));
				zz[zzOff + --num4] = (uint)(num6 >> 1);
				num2 = (uint)num6;
			}
			while (num3 > 0);
			ulong num7 = num * num;
			ulong num8 = (ulong)((ulong)num2 << 31) | num7 >> 33;
			zz[zzOff] = (uint)num7;
			num2 = ((uint)(num7 >> 32) & 1U);
			ulong num9 = (ulong)x[xOff + 1];
			ulong num10 = (ulong)zz[zzOff + 2];
			num8 += num9 * num;
			uint num11 = (uint)num8;
			zz[zzOff + 1] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num10 += num8 >> 32;
			ulong num12 = (ulong)x[xOff + 2];
			ulong num13 = (ulong)zz[zzOff + 3];
			ulong num14 = (ulong)zz[zzOff + 4];
			num10 += num12 * num;
			num11 = (uint)num10;
			zz[zzOff + 2] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num13 += (num10 >> 32) + num12 * num9;
			num14 += num13 >> 32;
			num13 &= (ulong)-1;
			ulong num15 = (ulong)x[xOff + 3];
			ulong num16 = (ulong)zz[zzOff + 5];
			ulong num17 = (ulong)zz[zzOff + 6];
			num13 += num15 * num;
			num11 = (uint)num13;
			zz[zzOff + 3] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num14 += (num13 >> 32) + num15 * num9;
			num16 += (num14 >> 32) + num15 * num12;
			num17 += num16 >> 32;
			num11 = (uint)num14;
			zz[zzOff + 4] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num16;
			zz[zzOff + 5] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num17;
			zz[zzOff + 6] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = zz[zzOff + 7] + (uint)(num17 >> 32);
			zz[zzOff + 7] = (num11 << 1 | num2);
		}

		// Token: 0x06001820 RID: 6176 RVA: 0x0008ADC0 File Offset: 0x00088FC0
		public static int Sub(uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			num += (long)((ulong)x[0] - (ulong)y[0]);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[1] - (ulong)y[1]);
			z[1] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[2] - (ulong)y[2]);
			z[2] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[3] - (ulong)y[3]);
			z[3] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001821 RID: 6177 RVA: 0x0008AE2C File Offset: 0x0008902C
		public static int Sub(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			long num = 0L;
			num += (long)((ulong)x[xOff] - (ulong)y[yOff]);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[xOff + 1] - (ulong)y[yOff + 1]);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[xOff + 2] - (ulong)y[yOff + 2]);
			z[zOff + 2] = (uint)num;
			num >>= 32;
			num += (long)((ulong)x[xOff + 3] - (ulong)y[yOff + 3]);
			z[zOff + 3] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001822 RID: 6178 RVA: 0x0008AEB0 File Offset: 0x000890B0
		public static int SubBothFrom(uint[] x, uint[] y, uint[] z)
		{
			long num = 0L;
			num += (long)((ulong)z[0] - (ulong)x[0] - (ulong)y[0]);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - (ulong)x[1] - (ulong)y[1]);
			z[1] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[2] - (ulong)x[2] - (ulong)y[2]);
			z[2] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[3] - (ulong)x[3] - (ulong)y[3]);
			z[3] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001823 RID: 6179 RVA: 0x0008AF30 File Offset: 0x00089130
		public static int SubFrom(uint[] x, uint[] z)
		{
			long num = 0L;
			num += (long)((ulong)z[0] - (ulong)x[0]);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - (ulong)x[1]);
			z[1] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[2] - (ulong)x[2]);
			z[2] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[3] - (ulong)x[3]);
			z[3] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001824 RID: 6180 RVA: 0x0008AF9C File Offset: 0x0008919C
		public static int SubFrom(uint[] x, int xOff, uint[] z, int zOff)
		{
			long num = 0L;
			num += (long)((ulong)z[zOff] - (ulong)x[xOff]);
			z[zOff] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 1] - (ulong)x[xOff + 1]);
			z[zOff + 1] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 2] - (ulong)x[xOff + 2]);
			z[zOff + 2] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[zOff + 3] - (ulong)x[xOff + 3]);
			z[zOff + 3] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001825 RID: 6181 RVA: 0x0008B018 File Offset: 0x00089218
		public static BigInteger ToBigInteger(uint[] x)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 4; i++)
			{
				uint num = x[i];
				if (num != 0U)
				{
					Pack.UInt32_To_BE(num, array, 3 - i << 2);
				}
			}
			return new BigInteger(1, array);
		}

		// Token: 0x06001826 RID: 6182 RVA: 0x0008B054 File Offset: 0x00089254
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[16];
			for (int i = 0; i < 2; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 1 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}

		// Token: 0x06001827 RID: 6183 RVA: 0x0008B08F File Offset: 0x0008928F
		public static void Zero(uint[] z)
		{
			z[0] = 0U;
			z[1] = 0U;
			z[2] = 0U;
			z[3] = 0U;
		}

		// Token: 0x0400154F RID: 5455
		private const ulong M = 4294967295UL;
	}
}
