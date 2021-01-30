using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200031F RID: 799
	internal class SecP256R1Field
	{
		// Token: 0x06001CBE RID: 7358 RVA: 0x000A2017 File Offset: 0x000A0217
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Add(x, y, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256R1Field.P)))
			{
				SecP256R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CBF RID: 7359 RVA: 0x000A203C File Offset: 0x000A023C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Add(16, xx, yy, zz) != 0U || (zz[15] >= 4294967294U && Nat.Gte(16, zz, SecP256R1Field.PExt)))
			{
				Nat.SubFrom(16, SecP256R1Field.PExt, zz);
			}
		}

		// Token: 0x06001CC0 RID: 7360 RVA: 0x000A206F File Offset: 0x000A026F
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(8, x, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256R1Field.P)))
			{
				SecP256R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CC1 RID: 7361 RVA: 0x000A2094 File Offset: 0x000A0294
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat256.FromBigInteger(x);
			if (array[7] == 4294967295U && Nat256.Gte(array, SecP256R1Field.P))
			{
				Nat256.SubFrom(SecP256R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001CC2 RID: 7362 RVA: 0x000A20C8 File Offset: 0x000A02C8
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(8, x, 0U, z);
				return;
			}
			uint c = Nat256.Add(x, SecP256R1Field.P, z);
			Nat.ShiftDownBit(8, z, c);
		}

		// Token: 0x06001CC3 RID: 7363 RVA: 0x000A2100 File Offset: 0x000A0300
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Mul(x, y, array);
			SecP256R1Field.Reduce(array, z);
		}

		// Token: 0x06001CC4 RID: 7364 RVA: 0x000A2122 File Offset: 0x000A0322
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if (Nat256.MulAddTo(x, y, zz) != 0U || (zz[15] >= 4294967294U && Nat.Gte(16, zz, SecP256R1Field.PExt)))
			{
				Nat.SubFrom(16, SecP256R1Field.PExt, zz);
			}
		}

		// Token: 0x06001CC5 RID: 7365 RVA: 0x000A2153 File Offset: 0x000A0353
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat256.IsZero(x))
			{
				Nat256.Zero(z);
				return;
			}
			Nat256.Sub(SecP256R1Field.P, x, z);
		}

		// Token: 0x06001CC6 RID: 7366 RVA: 0x000A2174 File Offset: 0x000A0374
		public static void Reduce(uint[] xx, uint[] z)
		{
			long num = (long)((ulong)xx[8]);
			long num2 = (long)((ulong)xx[9]);
			long num3 = (long)((ulong)xx[10]);
			long num4 = (long)((ulong)xx[11]);
			long num5 = (long)((ulong)xx[12]);
			long num6 = (long)((ulong)xx[13]);
			long num7 = (long)((ulong)xx[14]);
			long num8 = (long)((ulong)xx[15]);
			num -= 6L;
			long num9 = num + num2;
			long num10 = num2 + num3;
			long num11 = num3 + num4 - num8;
			long num12 = num4 + num5;
			long num13 = num5 + num6;
			long num14 = num6 + num7;
			long num15 = num7 + num8;
			long num16 = num14 - num9;
			long num17 = 0L;
			num17 += (long)((ulong)xx[0] - (ulong)num12 - (ulong)num16);
			z[0] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[1] + (ulong)num10 - (ulong)num13 - (ulong)num15);
			z[1] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[2] + (ulong)num11 - (ulong)num14);
			z[2] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[3] + (ulong)((ulong)num12 << 1) + (ulong)num16 - (ulong)num15);
			z[3] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[4] + (ulong)((ulong)num13 << 1) + (ulong)num7 - (ulong)num10);
			z[4] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[5] + (ulong)((ulong)num14 << 1) - (ulong)num11);
			z[5] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[6] + (ulong)((ulong)num15 << 1) + (ulong)num16);
			z[6] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[7] + (ulong)((ulong)num8 << 1) + (ulong)num - (ulong)num11 - (ulong)num13);
			z[7] = (uint)num17;
			num17 >>= 32;
			num17 += 6L;
			SecP256R1Field.Reduce32((uint)num17, z);
		}

		// Token: 0x06001CC7 RID: 7367 RVA: 0x000A22FC File Offset: 0x000A04FC
		public static void Reduce32(uint x, uint[] z)
		{
			long num = 0L;
			if (x != 0U)
			{
				long num2 = (long)((ulong)x);
				num += (long)((ulong)z[0] + (ulong)num2);
				z[0] = (uint)num;
				num >>= 32;
				if (num != 0L)
				{
					num += (long)((ulong)z[1]);
					z[1] = (uint)num;
					num >>= 32;
					num += (long)((ulong)z[2]);
					z[2] = (uint)num;
					num >>= 32;
				}
				num += (long)((ulong)z[3] - (ulong)num2);
				z[3] = (uint)num;
				num >>= 32;
				if (num != 0L)
				{
					num += (long)((ulong)z[4]);
					z[4] = (uint)num;
					num >>= 32;
					num += (long)((ulong)z[5]);
					z[5] = (uint)num;
					num >>= 32;
				}
				num += (long)((ulong)z[6] - (ulong)num2);
				z[6] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[7] + (ulong)num2);
				z[7] = (uint)num;
				num >>= 32;
			}
			if (num != 0L || (z[7] == 4294967295U && Nat256.Gte(z, SecP256R1Field.P)))
			{
				SecP256R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CC8 RID: 7368 RVA: 0x000A23C8 File Offset: 0x000A05C8
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256R1Field.Reduce(array, z);
		}

		// Token: 0x06001CC9 RID: 7369 RVA: 0x000A23EC File Offset: 0x000A05EC
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat256.Square(z, array);
				SecP256R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001CCA RID: 7370 RVA: 0x000A2426 File Offset: 0x000A0626
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Sub(x, y, z) != 0)
			{
				SecP256R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06001CCB RID: 7371 RVA: 0x000A2438 File Offset: 0x000A0638
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(16, xx, yy, zz) != 0)
			{
				Nat.AddTo(16, SecP256R1Field.PExt, zz);
			}
		}

		// Token: 0x06001CCC RID: 7372 RVA: 0x000A2454 File Offset: 0x000A0654
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(8, x, 0U, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256R1Field.P)))
			{
				SecP256R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CCD RID: 7373 RVA: 0x000A247C File Offset: 0x000A067C
		private static void AddPInvTo(uint[] z)
		{
			long num = (long)((ulong)z[0] + 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] - 1UL);
			z[3] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[4]);
				z[4] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[5]);
				z[5] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[6] - 1UL);
			z[6] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[7] + 1UL);
			z[7] = (uint)num;
		}

		// Token: 0x06001CCE RID: 7374 RVA: 0x000A251C File Offset: 0x000A071C
		private static void SubPInvFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] - 1UL);
			z[0] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[1]);
				z[1] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] + 1UL);
			z[3] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[4]);
				z[4] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[5]);
				z[5] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[6] + 1UL);
			z[6] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[7] - 1UL);
			z[7] = (uint)num;
		}

		// Token: 0x04001604 RID: 5636
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			0U,
			0U,
			0U,
			1U,
			uint.MaxValue
		};

		// Token: 0x04001605 RID: 5637
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			0U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			4294967294U,
			1U,
			4294967294U,
			1U,
			4294967294U,
			1U,
			1U,
			4294967294U,
			2U,
			4294967294U
		};

		// Token: 0x04001606 RID: 5638
		internal const uint P7 = 4294967295U;

		// Token: 0x04001607 RID: 5639
		internal const uint PExt15 = 4294967294U;
	}
}
