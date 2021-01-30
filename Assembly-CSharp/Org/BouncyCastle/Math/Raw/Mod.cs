using System;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002C6 RID: 710
	internal abstract class Mod
	{
		// Token: 0x0600179C RID: 6044 RVA: 0x0008870C File Offset: 0x0008690C
		public static void Invert(uint[] p, uint[] x, uint[] z)
		{
			int num = p.Length;
			if (Nat.IsZero(num, x))
			{
				throw new ArgumentException("cannot be 0", "x");
			}
			if (Nat.IsOne(num, x))
			{
				Array.Copy(x, 0, z, 0, num);
				return;
			}
			uint[] array = Nat.Copy(num, x);
			uint[] array2 = Nat.Create(num);
			array2[0] = 1U;
			int num2 = 0;
			if ((array[0] & 1U) == 0U)
			{
				Mod.InversionStep(p, array, num, array2, ref num2);
			}
			if (Nat.IsOne(num, array))
			{
				Mod.InversionResult(p, num2, array2, z);
				return;
			}
			uint[] array3 = Nat.Copy(num, p);
			uint[] array4 = Nat.Create(num);
			int num3 = 0;
			int num4 = num;
			for (;;)
			{
				if (array[num4 - 1] != 0U || array3[num4 - 1] != 0U)
				{
					if (Nat.Gte(num, array, array3))
					{
						Nat.SubFrom(num, array3, array);
						num2 += Nat.SubFrom(num, array4, array2) - num3;
						Mod.InversionStep(p, array, num4, array2, ref num2);
						if (Nat.IsOne(num, array))
						{
							break;
						}
					}
					else
					{
						Nat.SubFrom(num, array, array3);
						num3 += Nat.SubFrom(num, array2, array4) - num2;
						Mod.InversionStep(p, array3, num4, array4, ref num3);
						if (Nat.IsOne(num, array3))
						{
							goto Block_8;
						}
					}
				}
				else
				{
					num4--;
				}
			}
			Mod.InversionResult(p, num2, array2, z);
			return;
			Block_8:
			Mod.InversionResult(p, num3, array4, z);
		}

		// Token: 0x0600179D RID: 6045 RVA: 0x00088838 File Offset: 0x00086A38
		public static uint[] Random(uint[] p)
		{
			int num = p.Length;
			uint[] array = Nat.Create(num);
			uint num2 = p[num - 1];
			num2 |= num2 >> 1;
			num2 |= num2 >> 2;
			num2 |= num2 >> 4;
			num2 |= num2 >> 8;
			num2 |= num2 >> 16;
			do
			{
				byte[] array2 = new byte[num << 2];
				Mod.RandomSource.NextBytes(array2);
				Pack.BE_To_UInt32(array2, 0, array);
				array[num - 1] &= num2;
			}
			while (Nat.Gte(num, array, p));
			return array;
		}

		// Token: 0x0600179E RID: 6046 RVA: 0x000888AC File Offset: 0x00086AAC
		public static void Add(uint[] p, uint[] x, uint[] y, uint[] z)
		{
			int len = p.Length;
			if (Nat.Add(len, x, y, z) != 0U)
			{
				Nat.SubFrom(len, p, z);
			}
		}

		// Token: 0x0600179F RID: 6047 RVA: 0x000888D4 File Offset: 0x00086AD4
		public static void Subtract(uint[] p, uint[] x, uint[] y, uint[] z)
		{
			int len = p.Length;
			if (Nat.Sub(len, x, y, z) != 0)
			{
				Nat.AddTo(len, p, z);
			}
		}

		// Token: 0x060017A0 RID: 6048 RVA: 0x000888F9 File Offset: 0x00086AF9
		private static void InversionResult(uint[] p, int ac, uint[] a, uint[] z)
		{
			if (ac < 0)
			{
				Nat.Add(p.Length, a, p, z);
				return;
			}
			Array.Copy(a, 0, z, 0, p.Length);
		}

		// Token: 0x060017A1 RID: 6049 RVA: 0x00088918 File Offset: 0x00086B18
		private static void InversionStep(uint[] p, uint[] u, int uLen, uint[] x, ref int xc)
		{
			int len = p.Length;
			int num = 0;
			while (u[0] == 0U)
			{
				Nat.ShiftDownWord(uLen, u, 0U);
				num += 32;
			}
			int trailingZeroes = Mod.GetTrailingZeroes(u[0]);
			if (trailingZeroes > 0)
			{
				Nat.ShiftDownBits(uLen, u, trailingZeroes, 0U);
				num += trailingZeroes;
			}
			for (int i = 0; i < num; i++)
			{
				if ((x[0] & 1U) != 0U)
				{
					if (xc < 0)
					{
						xc += (int)Nat.AddTo(len, p, x);
					}
					else
					{
						xc += Nat.SubFrom(len, p, x);
					}
				}
				Nat.ShiftDownBit(len, x, (uint)xc);
			}
		}

		// Token: 0x060017A2 RID: 6050 RVA: 0x000889A0 File Offset: 0x00086BA0
		private static int GetTrailingZeroes(uint x)
		{
			int num = 0;
			while ((x & 1U) == 0U)
			{
				x >>= 1;
				num++;
			}
			return num;
		}

		// Token: 0x0400154D RID: 5453
		private static readonly SecureRandom RandomSource = new SecureRandom();
	}
}
