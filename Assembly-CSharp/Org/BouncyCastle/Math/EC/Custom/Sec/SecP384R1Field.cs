using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000323 RID: 803
	internal class SecP384R1Field
	{
		// Token: 0x06001CFB RID: 7419 RVA: 0x000A2F5F File Offset: 0x000A115F
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat.Add(12, x, y, z) != 0U || (z[11] == 4294967295U && Nat.Gte(12, z, SecP384R1Field.P)))
			{
				SecP384R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CFC RID: 7420 RVA: 0x000A2F8C File Offset: 0x000A118C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(24, xx, yy, zz) != 0U || (zz[23] == 4294967295U && Nat.Gte(24, zz, SecP384R1Field.PExt))) && Nat.AddTo(SecP384R1Field.PExtInv.Length, SecP384R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(24, zz, SecP384R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001CFD RID: 7421 RVA: 0x000A2FDF File Offset: 0x000A11DF
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(12, x, z) != 0U || (z[11] == 4294967295U && Nat.Gte(12, z, SecP384R1Field.P)))
			{
				SecP384R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001CFE RID: 7422 RVA: 0x000A3008 File Offset: 0x000A1208
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat.FromBigInteger(384, x);
			if (array[11] == 4294967295U && Nat.Gte(12, array, SecP384R1Field.P))
			{
				Nat.SubFrom(12, SecP384R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001CFF RID: 7423 RVA: 0x000A3048 File Offset: 0x000A1248
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(12, x, 0U, z);
				return;
			}
			uint c = Nat.Add(12, x, SecP384R1Field.P, z);
			Nat.ShiftDownBit(12, z, c);
		}

		// Token: 0x06001D00 RID: 7424 RVA: 0x000A3084 File Offset: 0x000A1284
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat.Create(24);
			Nat384.Mul(x, y, array);
			SecP384R1Field.Reduce(array, z);
		}

		// Token: 0x06001D01 RID: 7425 RVA: 0x000A30A8 File Offset: 0x000A12A8
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat.IsZero(12, x))
			{
				Nat.Zero(12, z);
				return;
			}
			Nat.Sub(12, SecP384R1Field.P, x, z);
		}

		// Token: 0x06001D02 RID: 7426 RVA: 0x000A30CC File Offset: 0x000A12CC
		public static void Reduce(uint[] xx, uint[] z)
		{
			long num = (long)((ulong)xx[16]);
			long num2 = (long)((ulong)xx[17]);
			long num3 = (long)((ulong)xx[18]);
			long num4 = (long)((ulong)xx[19]);
			long num5 = (long)((ulong)xx[20]);
			long num6 = (long)((ulong)xx[21]);
			long num7 = (long)((ulong)xx[22]);
			long num8 = (long)((ulong)xx[23]);
			long num9 = (long)((ulong)xx[12] + (ulong)num5 - 1UL);
			long num10 = (long)((ulong)xx[13] + (ulong)num7);
			long num11 = (long)((ulong)xx[14] + (ulong)num7 + (ulong)num8);
			long num12 = (long)((ulong)xx[15] + (ulong)num8);
			long num13 = num2 + num6;
			long num14 = num6 - num8;
			long num15 = num7 - num8;
			long num16 = num9 + num14;
			long num17 = 0L;
			num17 += (long)((ulong)xx[0] + (ulong)num16);
			z[0] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[1] + (ulong)num8 - (ulong)num9 + (ulong)num10);
			z[1] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[2] - (ulong)num6 - (ulong)num10 + (ulong)num11);
			z[2] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[3] - (ulong)num11 + (ulong)num12 + (ulong)num16);
			z[3] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[4] + (ulong)num + (ulong)num6 + (ulong)num10 - (ulong)num12 + (ulong)num16);
			z[4] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[5] - (ulong)num + (ulong)num10 + (ulong)num11 + (ulong)num13);
			z[5] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[6] + (ulong)num3 - (ulong)num2 + (ulong)num11 + (ulong)num12);
			z[6] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[7] + (ulong)num + (ulong)num4 - (ulong)num3 + (ulong)num12);
			z[7] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[8] + (ulong)num + (ulong)num2 + (ulong)num5 - (ulong)num4);
			z[8] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[9] + (ulong)num3 - (ulong)num5 + (ulong)num13);
			z[9] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[10] + (ulong)num3 + (ulong)num4 - (ulong)num14 + (ulong)num15);
			z[10] = (uint)num17;
			num17 >>= 32;
			num17 += (long)((ulong)xx[11] + (ulong)num4 + (ulong)num5 - (ulong)num15);
			z[11] = (uint)num17;
			num17 >>= 32;
			num17 += 1L;
			SecP384R1Field.Reduce32((uint)num17, z);
		}

		// Token: 0x06001D03 RID: 7427 RVA: 0x000A32E8 File Offset: 0x000A14E8
		public static void Reduce32(uint x, uint[] z)
		{
			long num = 0L;
			if (x != 0U)
			{
				long num2 = (long)((ulong)x);
				num += (long)((ulong)z[0] + (ulong)num2);
				z[0] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[1] - (ulong)num2);
				z[1] = (uint)num;
				num >>= 32;
				if (num != 0L)
				{
					num += (long)((ulong)z[2]);
					z[2] = (uint)num;
					num >>= 32;
				}
				num += (long)((ulong)z[3] + (ulong)num2);
				z[3] = (uint)num;
				num >>= 32;
				num += (long)((ulong)z[4] + (ulong)num2);
				z[4] = (uint)num;
				num >>= 32;
			}
			if ((num != 0L && Nat.IncAt(12, z, 5) != 0U) || (z[11] == 4294967295U && Nat.Gte(12, z, SecP384R1Field.P)))
			{
				SecP384R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001D04 RID: 7428 RVA: 0x000A3388 File Offset: 0x000A1588
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat.Create(24);
			Nat384.Square(x, array);
			SecP384R1Field.Reduce(array, z);
		}

		// Token: 0x06001D05 RID: 7429 RVA: 0x000A33AC File Offset: 0x000A15AC
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat.Create(24);
			Nat384.Square(x, array);
			SecP384R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat384.Square(z, array);
				SecP384R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001D06 RID: 7430 RVA: 0x000A33E8 File Offset: 0x000A15E8
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat.Sub(12, x, y, z) != 0)
			{
				SecP384R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06001D07 RID: 7431 RVA: 0x000A33FC File Offset: 0x000A15FC
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(24, xx, yy, zz) != 0 && Nat.SubFrom(SecP384R1Field.PExtInv.Length, SecP384R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(24, zz, SecP384R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001D08 RID: 7432 RVA: 0x000A342E File Offset: 0x000A162E
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(12, x, 0U, z) != 0U || (z[11] == 4294967295U && Nat.Gte(12, z, SecP384R1Field.P)))
			{
				SecP384R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001D09 RID: 7433 RVA: 0x000A3458 File Offset: 0x000A1658
		private static void AddPInvTo(uint[] z)
		{
			long num = (long)((ulong)z[0] + 1UL);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] - 1UL);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] + 1UL);
			z[3] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[4] + 1UL);
			z[4] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.IncAt(12, z, 5);
			}
		}

		// Token: 0x06001D0A RID: 7434 RVA: 0x000A34D4 File Offset: 0x000A16D4
		private static void SubPInvFrom(uint[] z)
		{
			long num = (long)((ulong)z[0] - 1UL);
			z[0] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[1] + 1UL);
			z[1] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				num += (long)((ulong)z[2]);
				z[2] = (uint)num;
				num >>= 32;
			}
			num += (long)((ulong)z[3] - 1UL);
			z[3] = (uint)num;
			num >>= 32;
			num += (long)((ulong)z[4] - 1UL);
			z[4] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.DecAt(12, z, 5);
			}
		}

		// Token: 0x0400160D RID: 5645
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			0U,
			0U,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x0400160E RID: 5646
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			4294967294U,
			0U,
			2U,
			0U,
			4294967294U,
			0U,
			2U,
			1U,
			0U,
			0U,
			0U,
			4294967294U,
			1U,
			0U,
			4294967294U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x0400160F RID: 5647
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			1U,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			1U,
			uint.MaxValue,
			4294967293U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1U,
			4294967294U,
			uint.MaxValue,
			1U,
			2U
		};

		// Token: 0x04001610 RID: 5648
		private const uint P11 = 4294967295U;

		// Token: 0x04001611 RID: 5649
		private const uint PExt23 = 4294967295U;
	}
}
