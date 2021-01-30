using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002C9 RID: 713
	internal abstract class Nat160
	{
		// Token: 0x06001829 RID: 6185 RVA: 0x0008B0A4 File Offset: 0x000892A4
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
			num += (ulong)x[4] + (ulong)y[4];
			z[4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600182A RID: 6186 RVA: 0x0008B124 File Offset: 0x00089324
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
			num += (ulong)x[4] + (ulong)y[4] + (ulong)z[4];
			z[4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600182B RID: 6187 RVA: 0x0008B1C0 File Offset: 0x000893C0
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
			num += (ulong)x[4] + (ulong)z[4];
			z[4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600182C RID: 6188 RVA: 0x0008B240 File Offset: 0x00089440
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
			num += (ulong)x[xOff + 4] + (ulong)z[zOff + 4];
			z[zOff + 4] = (uint)num;
			num >>= 32;
			num += (ulong)x[xOff + 5] + (ulong)z[zOff + 5];
			return (uint)num;
		}

		// Token: 0x0600182D RID: 6189 RVA: 0x0008B2EC File Offset: 0x000894EC
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
			num += (ulong)u[uOff + 4] + (ulong)v[vOff + 4];
			u[uOff + 4] = (uint)num;
			v[vOff + 4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600182E RID: 6190 RVA: 0x0008B3A5 File Offset: 0x000895A5
		public static void Copy(uint[] x, uint[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
		}

		// Token: 0x0600182F RID: 6191 RVA: 0x0008B3C5 File Offset: 0x000895C5
		public static uint[] Create()
		{
			return new uint[5];
		}

		// Token: 0x06001830 RID: 6192 RVA: 0x0008B3CD File Offset: 0x000895CD
		public static uint[] CreateExt()
		{
			return new uint[10];
		}

		// Token: 0x06001831 RID: 6193 RVA: 0x0008B3D6 File Offset: 0x000895D6
		public static bool Diff(uint[] x, int xOff, uint[] y, int yOff, uint[] z, int zOff)
		{
			bool flag = Nat160.Gte(x, xOff, y, yOff);
			if (flag)
			{
				Nat160.Sub(x, xOff, y, yOff, z, zOff);
				return flag;
			}
			Nat160.Sub(y, yOff, x, xOff, z, zOff);
			return flag;
		}

		// Token: 0x06001832 RID: 6194 RVA: 0x0008B404 File Offset: 0x00089604
		public static bool Eq(uint[] x, uint[] y)
		{
			for (int i = 4; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001833 RID: 6195 RVA: 0x0008B428 File Offset: 0x00089628
		public static uint[] FromBigInteger(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 160)
			{
				throw new ArgumentException();
			}
			uint[] array = Nat160.Create();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (uint)x.IntValue;
				x = x.ShiftRight(32);
			}
			return array;
		}

		// Token: 0x06001834 RID: 6196 RVA: 0x0008B47C File Offset: 0x0008967C
		public static uint GetBit(uint[] x, int bit)
		{
			if (bit == 0)
			{
				return x[0] & 1U;
			}
			int num = bit >> 5;
			if (num < 0 || num >= 5)
			{
				return 0U;
			}
			int num2 = bit & 31;
			return x[num] >> num2 & 1U;
		}

		// Token: 0x06001835 RID: 6197 RVA: 0x0008B4B0 File Offset: 0x000896B0
		public static bool Gte(uint[] x, uint[] y)
		{
			for (int i = 4; i >= 0; i--)
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

		// Token: 0x06001836 RID: 6198 RVA: 0x0008B4E0 File Offset: 0x000896E0
		public static bool Gte(uint[] x, int xOff, uint[] y, int yOff)
		{
			for (int i = 4; i >= 0; i--)
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

		// Token: 0x06001837 RID: 6199 RVA: 0x0008B514 File Offset: 0x00089714
		public static bool IsOne(uint[] x)
		{
			if (x[0] != 1U)
			{
				return false;
			}
			for (int i = 1; i < 5; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001838 RID: 6200 RVA: 0x0008B540 File Offset: 0x00089740
		public static bool IsZero(uint[] x)
		{
			for (int i = 0; i < 5; i++)
			{
				if (x[i] != 0U)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06001839 RID: 6201 RVA: 0x0008B564 File Offset: 0x00089764
		public static void Mul(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = (ulong)y[0];
			ulong num2 = (ulong)y[1];
			ulong num3 = (ulong)y[2];
			ulong num4 = (ulong)y[3];
			ulong num5 = (ulong)y[4];
			ulong num6 = 0UL;
			ulong num7 = (ulong)x[0];
			num6 += num7 * num;
			zz[0] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num2;
			zz[1] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num3;
			zz[2] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num4;
			zz[3] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num5;
			zz[4] = (uint)num6;
			num6 >>= 32;
			zz[5] = (uint)num6;
			for (int i = 1; i < 5; i++)
			{
				ulong num8 = 0UL;
				ulong num9 = (ulong)x[i];
				num8 += num9 * num + (ulong)zz[i];
				zz[i] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num2 + (ulong)zz[i + 1];
				zz[i + 1] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num3 + (ulong)zz[i + 2];
				zz[i + 2] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num4 + (ulong)zz[i + 3];
				zz[i + 3] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num5 + (ulong)zz[i + 4];
				zz[i + 4] = (uint)num8;
				num8 >>= 32;
				zz[i + 5] = (uint)num8;
			}
		}

		// Token: 0x0600183A RID: 6202 RVA: 0x0008B6D8 File Offset: 0x000898D8
		public static void Mul(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)y[yOff];
			ulong num2 = (ulong)y[yOff + 1];
			ulong num3 = (ulong)y[yOff + 2];
			ulong num4 = (ulong)y[yOff + 3];
			ulong num5 = (ulong)y[yOff + 4];
			ulong num6 = 0UL;
			ulong num7 = (ulong)x[xOff];
			num6 += num7 * num;
			zz[zzOff] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num2;
			zz[zzOff + 1] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num3;
			zz[zzOff + 2] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num4;
			zz[zzOff + 3] = (uint)num6;
			num6 >>= 32;
			num6 += num7 * num5;
			zz[zzOff + 4] = (uint)num6;
			num6 >>= 32;
			zz[zzOff + 5] = (uint)num6;
			for (int i = 1; i < 5; i++)
			{
				zzOff++;
				ulong num8 = 0UL;
				ulong num9 = (ulong)x[xOff + i];
				num8 += num9 * num + (ulong)zz[zzOff];
				zz[zzOff] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num2 + (ulong)zz[zzOff + 1];
				zz[zzOff + 1] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num3 + (ulong)zz[zzOff + 2];
				zz[zzOff + 2] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num4 + (ulong)zz[zzOff + 3];
				zz[zzOff + 3] = (uint)num8;
				num8 >>= 32;
				num8 += num9 * num5 + (ulong)zz[zzOff + 4];
				zz[zzOff + 4] = (uint)num8;
				num8 >>= 32;
				zz[zzOff + 5] = (uint)num8;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x0008B87C File Offset: 0x00089A7C
		public static uint MulAddTo(uint[] x, uint[] y, uint[] zz)
		{
			ulong num = (ulong)y[0];
			ulong num2 = (ulong)y[1];
			ulong num3 = (ulong)y[2];
			ulong num4 = (ulong)y[3];
			ulong num5 = (ulong)y[4];
			ulong num6 = 0UL;
			for (int i = 0; i < 5; i++)
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
				num7 += num8 * num5 + (ulong)zz[i + 4];
				zz[i + 4] = (uint)num7;
				num7 >>= 32;
				num7 += num6 + (ulong)zz[i + 5];
				zz[i + 5] = (uint)num7;
				num6 = num7 >> 32;
			}
			return (uint)num6;
		}

		// Token: 0x0600183C RID: 6204 RVA: 0x0008B98C File Offset: 0x00089B8C
		public static uint MulAddTo(uint[] x, int xOff, uint[] y, int yOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)y[yOff];
			ulong num2 = (ulong)y[yOff + 1];
			ulong num3 = (ulong)y[yOff + 2];
			ulong num4 = (ulong)y[yOff + 3];
			ulong num5 = (ulong)y[yOff + 4];
			ulong num6 = 0UL;
			for (int i = 0; i < 5; i++)
			{
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
				num7 += num8 * num5 + (ulong)zz[zzOff + 4];
				zz[zzOff + 4] = (uint)num7;
				num7 >>= 32;
				num7 += num6 + (ulong)zz[zzOff + 5];
				zz[zzOff + 5] = (uint)num7;
				num6 = num7 >> 32;
				zzOff++;
			}
			return (uint)num6;
		}

		// Token: 0x0600183D RID: 6205 RVA: 0x0008BAB8 File Offset: 0x00089CB8
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
			ulong num7 = (ulong)x[xOff + 4];
			num += num2 * num7 + num6 + (ulong)y[yOff + 4];
			z[zOff + 4] = (uint)num;
			num >>= 32;
			return num + num7;
		}

		// Token: 0x0600183E RID: 6206 RVA: 0x0008BB8C File Offset: 0x00089D8C
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
			num += num2 * (ulong)yy[yyOff + 4] + (ulong)zz[zzOff + 4];
			zz[zzOff + 4] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600183F RID: 6207 RVA: 0x0008BC3C File Offset: 0x00089E3C
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
			if (num != 0UL)
			{
				return Nat.IncAt(5, z, zOff, 4);
			}
			return 0U;
		}

		// Token: 0x06001840 RID: 6208 RVA: 0x0008BCC4 File Offset: 0x00089EC4
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
				return Nat.IncAt(5, z, zOff, 3);
			}
			return 0U;
		}

		// Token: 0x06001841 RID: 6209 RVA: 0x0008BD28 File Offset: 0x00089F28
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
				return Nat.IncAt(5, z, zOff, 3);
			}
			return 0U;
		}

		// Token: 0x06001842 RID: 6210 RVA: 0x0008BD90 File Offset: 0x00089F90
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
				return Nat.IncAt(5, z, zOff, 2);
			}
			return 0U;
		}

		// Token: 0x06001843 RID: 6211 RVA: 0x0008BDE0 File Offset: 0x00089FE0
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
			while (++num3 < 5);
			return (uint)num;
		}

		// Token: 0x06001844 RID: 6212 RVA: 0x0008BE14 File Offset: 0x0008A014
		public static void Square(uint[] x, uint[] zz)
		{
			ulong num = (ulong)x[0];
			uint num2 = 0U;
			int num3 = 4;
			int num4 = 10;
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
			num14 &= (ulong)-1;
			num17 += num16 >> 32;
			num16 &= (ulong)-1;
			ulong num18 = (ulong)x[4];
			ulong num19 = (ulong)zz[7];
			ulong num20 = (ulong)zz[8];
			num14 += num18 * num;
			num11 = (uint)num14;
			zz[4] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num16 += (num14 >> 32) + num18 * num9;
			num17 += (num16 >> 32) + num18 * num12;
			num19 += (num17 >> 32) + num18 * num15;
			num20 += num19 >> 32;
			num11 = (uint)num16;
			zz[5] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num17;
			zz[6] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num19;
			zz[7] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num20;
			zz[8] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = zz[9] + (uint)(num20 >> 32);
			zz[9] = (num11 << 1 | num2);
		}

		// Token: 0x06001845 RID: 6213 RVA: 0x0008C024 File Offset: 0x0008A224
		public static void Square(uint[] x, int xOff, uint[] zz, int zzOff)
		{
			ulong num = (ulong)x[xOff];
			uint num2 = 0U;
			int num3 = 4;
			int num4 = 10;
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
			num14 &= (ulong)-1;
			num17 += num16 >> 32;
			num16 &= (ulong)-1;
			ulong num18 = (ulong)x[xOff + 4];
			ulong num19 = (ulong)zz[zzOff + 7];
			ulong num20 = (ulong)zz[zzOff + 8];
			num14 += num18 * num;
			num11 = (uint)num14;
			zz[zzOff + 4] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num16 += (num14 >> 32) + num18 * num9;
			num17 += (num16 >> 32) + num18 * num12;
			num19 += (num17 >> 32) + num18 * num15;
			num20 += num19 >> 32;
			num11 = (uint)num16;
			zz[zzOff + 5] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num17;
			zz[zzOff + 6] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num19;
			zz[zzOff + 7] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = (uint)num20;
			zz[zzOff + 8] = (num11 << 1 | num2);
			num2 = num11 >> 31;
			num11 = zz[zzOff + 9] + (uint)(num20 >> 32);
			zz[zzOff + 9] = (num11 << 1 | num2);
		}

		// Token: 0x06001846 RID: 6214 RVA: 0x0008C264 File Offset: 0x0008A464
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
			num += (long)((ulong)x[4] - (ulong)y[4]);
			z[4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001847 RID: 6215 RVA: 0x0008C2E4 File Offset: 0x0008A4E4
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
			num += (long)((ulong)x[xOff + 4] - (ulong)y[yOff + 4]);
			z[zOff + 4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001848 RID: 6216 RVA: 0x0008C388 File Offset: 0x0008A588
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
			num += (long)((ulong)z[4] - (ulong)x[4] - (ulong)y[4]);
			z[4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06001849 RID: 6217 RVA: 0x0008C424 File Offset: 0x0008A624
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
			num += (long)((ulong)z[4] - (ulong)x[4]);
			z[4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x0600184A RID: 6218 RVA: 0x0008C4A4 File Offset: 0x0008A6A4
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
			num += (long)((ulong)z[zOff + 4] - (ulong)x[xOff + 4]);
			z[zOff + 4] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x0600184B RID: 6219 RVA: 0x0008C53C File Offset: 0x0008A73C
		public static BigInteger ToBigInteger(uint[] x)
		{
			byte[] array = new byte[20];
			for (int i = 0; i < 5; i++)
			{
				uint num = x[i];
				if (num != 0U)
				{
					Pack.UInt32_To_BE(num, array, 4 - i << 2);
				}
			}
			return new BigInteger(1, array);
		}

		// Token: 0x0600184C RID: 6220 RVA: 0x0008C577 File Offset: 0x0008A777
		public static void Zero(uint[] z)
		{
			z[0] = 0U;
			z[1] = 0U;
			z[2] = 0U;
			z[3] = 0U;
			z[4] = 0U;
		}

		// Token: 0x04001550 RID: 5456
		private const ulong M = 4294967295UL;
	}
}
