using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Djb
{
	// Token: 0x02000361 RID: 865
	internal class Curve25519Field
	{
		// Token: 0x060020FE RID: 8446 RVA: 0x000B25B4 File Offset: 0x000B07B4
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			Nat256.Add(x, y, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x060020FF RID: 8447 RVA: 0x000B25D3 File Offset: 0x000B07D3
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			Nat.Add(16, xx, yy, zz);
			if (Nat.Gte(16, zz, Curve25519Field.PExt))
			{
				Curve25519Field.SubPExtFrom(zz);
			}
		}

		// Token: 0x06002100 RID: 8448 RVA: 0x000B25F6 File Offset: 0x000B07F6
		public static void AddOne(uint[] x, uint[] z)
		{
			Nat.Inc(8, x, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002101 RID: 8449 RVA: 0x000B2618 File Offset: 0x000B0818
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat256.FromBigInteger(x);
			while (Nat256.Gte(array, Curve25519Field.P))
			{
				Nat256.SubFrom(Curve25519Field.P, array);
			}
			return array;
		}

		// Token: 0x06002102 RID: 8450 RVA: 0x000B2648 File Offset: 0x000B0848
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(8, x, 0U, z);
				return;
			}
			Nat256.Add(x, Curve25519Field.P, z);
			Nat.ShiftDownBit(8, z, 0U);
		}

		// Token: 0x06002103 RID: 8451 RVA: 0x000B2674 File Offset: 0x000B0874
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Mul(x, y, array);
			Curve25519Field.Reduce(array, z);
		}

		// Token: 0x06002104 RID: 8452 RVA: 0x000B2696 File Offset: 0x000B0896
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			Nat256.MulAddTo(x, y, zz);
			if (Nat.Gte(16, zz, Curve25519Field.PExt))
			{
				Curve25519Field.SubPExtFrom(zz);
			}
		}

		// Token: 0x06002105 RID: 8453 RVA: 0x000B26B7 File Offset: 0x000B08B7
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat256.IsZero(x))
			{
				Nat256.Zero(z);
				return;
			}
			Nat256.Sub(Curve25519Field.P, x, z);
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000B26D8 File Offset: 0x000B08D8
		public static void Reduce(uint[] xx, uint[] z)
		{
			uint num = xx[7];
			Nat.ShiftUpBit(8, xx, 8, num, z, 0);
			uint num2 = Nat256.MulByWordAddTo(19U, xx, z) << 1;
			uint num3 = z[7];
			num2 += (num3 >> 31) - (num >> 31);
			num3 &= 2147483647U;
			num3 += Nat.AddWordTo(7, num2 * 19U, z);
			z[7] = num3;
			if (num3 >= 2147483647U && Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002107 RID: 8455 RVA: 0x000B2748 File Offset: 0x000B0948
		public static void Reduce27(uint x, uint[] z)
		{
			uint num = z[7];
			uint num2 = x << 1 | num >> 31;
			num &= 2147483647U;
			num += Nat.AddWordTo(7, num2 * 19U, z);
			z[7] = num;
			if (num >= 2147483647U && Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x06002108 RID: 8456 RVA: 0x000B2798 File Offset: 0x000B0998
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			Curve25519Field.Reduce(array, z);
		}

		// Token: 0x06002109 RID: 8457 RVA: 0x000B27BC File Offset: 0x000B09BC
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			Curve25519Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat256.Square(z, array);
				Curve25519Field.Reduce(array, z);
			}
		}

		// Token: 0x0600210A RID: 8458 RVA: 0x000B27F6 File Offset: 0x000B09F6
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Sub(x, y, z) != 0)
			{
				Curve25519Field.AddPTo(z);
			}
		}

		// Token: 0x0600210B RID: 8459 RVA: 0x000B2809 File Offset: 0x000B0A09
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(16, xx, yy, zz) != 0)
			{
				Curve25519Field.AddPExtTo(zz);
			}
		}

		// Token: 0x0600210C RID: 8460 RVA: 0x000B281E File Offset: 0x000B0A1E
		public static void Twice(uint[] x, uint[] z)
		{
			Nat.ShiftUpBit(8, x, 0U, z);
			if (Nat256.Gte(z, Curve25519Field.P))
			{
				Curve25519Field.SubPFrom(z);
			}
		}

		// Token: 0x0600210D RID: 8461 RVA: 0x000B2840 File Offset: 0x000B0A40
		private static uint AddPTo(uint[] z)
		{
			long num = (long)((ulong)z[0] - 19UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(7, z, 1);
			}
			num += (long)((ulong)z[7] + (ulong)int.MinValue);
			z[7] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600210E RID: 8462 RVA: 0x000B2888 File Offset: 0x000B0A88
		private static uint AddPExtTo(uint[] zz)
		{
			long num = (long)((ulong)zz[0] + (ulong)Curve25519Field.PExt[0]);
			zz[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(8, zz, 1));
			}
			num += (long)((ulong)zz[8] - 19UL);
			zz[8] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(15, zz, 9);
			}
			num += (long)((ulong)zz[15] + (ulong)(Curve25519Field.PExt[15] + 1U));
			zz[15] = (uint)num;
			num >>= 32;
			return (uint)num;
		}

		// Token: 0x0600210F RID: 8463 RVA: 0x000B2900 File Offset: 0x000B0B00
		private static int SubPFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] + 19UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(7, z, 1));
			}
			num += (long)((ulong)z[7] - (ulong)int.MinValue);
			z[7] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x06002110 RID: 8464 RVA: 0x000B2948 File Offset: 0x000B0B48
		private static int SubPExtFrom(uint[] zz)
		{
			long num = (long)((ulong)zz[0] - (ulong)Curve25519Field.PExt[0]);
			zz[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)Nat.DecAt(8, zz, 1);
			}
			num += (long)((ulong)zz[8] + 19UL);
			zz[8] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num = (long)((ulong)Nat.IncAt(15, zz, 9));
			}
			num += (long)((ulong)zz[15] - (ulong)(Curve25519Field.PExt[15] + 1U));
			zz[15] = (uint)num;
			num >>= 32;
			return (int)num;
		}

		// Token: 0x04001663 RID: 5731
		internal static readonly uint[] P = new uint[]
		{
			4294967277U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			2147483647U
		};

		// Token: 0x04001664 RID: 5732
		private const uint P7 = 2147483647U;

		// Token: 0x04001665 RID: 5733
		private static readonly uint[] PExt = new uint[]
		{
			361U,
			0U,
			0U,
			0U,
			0U,
			0U,
			0U,
			0U,
			4294967277U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1073741823U
		};

		// Token: 0x04001666 RID: 5734
		private const uint PInv = 19U;
	}
}
