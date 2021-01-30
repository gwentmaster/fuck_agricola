using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000317 RID: 791
	internal class SecP224R1Field
	{
		// Token: 0x06001C41 RID: 7233 RVA: 0x000A0233 File Offset: 0x0009E433
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Add(x, y, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001C42 RID: 7234 RVA: 0x000A0258 File Offset: 0x0009E458
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(14, xx, yy, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224R1Field.PExt))) && Nat.AddTo(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C43 RID: 7235 RVA: 0x000A02AB File Offset: 0x0009E4AB
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(7, x, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001C44 RID: 7236 RVA: 0x000A02D0 File Offset: 0x0009E4D0
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat224.FromBigInteger(x);
			if (array[6] == 4294967295U && Nat224.Gte(array, SecP224R1Field.P))
			{
				Nat224.SubFrom(SecP224R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x000A0304 File Offset: 0x0009E504
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(7, x, 0U, z);
				return;
			}
			uint c = Nat224.Add(x, SecP224R1Field.P, z);
			Nat.ShiftDownBit(7, z, c);
		}

		// Token: 0x06001C46 RID: 7238 RVA: 0x000A033C File Offset: 0x0009E53C
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Mul(x, y, array);
			SecP224R1Field.Reduce(array, z);
		}

		// Token: 0x06001C47 RID: 7239 RVA: 0x000A0360 File Offset: 0x0009E560
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat224.MulAddTo(x, y, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224R1Field.PExt))) && Nat.AddTo(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C48 RID: 7240 RVA: 0x000A03B1 File Offset: 0x0009E5B1
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat224.IsZero(x))
			{
				Nat224.Zero(z);
				return;
			}
			Nat224.Sub(SecP224R1Field.P, x, z);
		}

		// Token: 0x06001C49 RID: 7241 RVA: 0x000A03D0 File Offset: 0x0009E5D0
		public static void Reduce(uint[] xx, uint[] z)
		{
			long num = (long)((ulong)xx[10]);
			long num2 = (long)((ulong)xx[11]);
			long num3 = (long)((ulong)xx[12]);
			long num4 = (long)((ulong)xx[13]);
			long num5 = (long)((ulong)xx[7] + (ulong)num2 - 1UL);
			long num6 = (long)((ulong)xx[8] + (ulong)num3);
			long num7 = (long)((ulong)xx[9] + (ulong)num4);
			long num8 = 0L;
			num8 += (long)((ulong)xx[0] - (ulong)num5);
			long num9 = (long)((ulong)((uint)num8));
			num8 >>= 32;
			num8 += (long)((ulong)xx[1] - (ulong)num6);
			z[1] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[2] - (ulong)num7);
			z[2] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[3] + (ulong)num5 - (ulong)num);
			long num10 = (long)((ulong)((uint)num8));
			num8 >>= 32;
			num8 += (long)((ulong)xx[4] + (ulong)num6 - (ulong)num2);
			z[4] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[5] + (ulong)num7 - (ulong)num3);
			z[5] = (uint)num8;
			num8 >>= 32;
			num8 += (long)((ulong)xx[6] + (ulong)num - (ulong)num4);
			z[6] = (uint)num8;
			num8 >>= 32;
			num8 += 1L;
			num10 += num8;
			num9 -= num8;
			z[0] = (uint)num9;
			num8 = num9 >> 32;
			if (num8 != 0L)
			{
				num8 += (long)((ulong)z[1]);
				z[1] = (uint)num8;
				num8 >>= 32;
				num8 += (long)((ulong)z[2]);
				z[2] = (uint)num8;
				num10 += num8 >> 32;
			}
			z[3] = (uint)num10;
			num8 = num10 >> 32;
			if ((num8 != 0L && Nat.IncAt(7, z, 4) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x000A0554 File Offset: 0x0009E754
		public static void Reduce32(uint x, uint[] z)
		{
			long num = 0L;
			if (x != 0U)
			{
				long num2 = (long)((ulong)x);
				num += (long)((ulong)z[0] - (ulong)num2);
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
				num += (long)((ulong)z[3] + (ulong)num2);
				z[3] = (uint)num;
				num >>= 32;
			}
			if ((num != 0L && Nat.IncAt(7, z, 4) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x000A05DC File Offset: 0x0009E7DC
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224R1Field.Reduce(array, z);
		}

		// Token: 0x06001C4C RID: 7244 RVA: 0x000A0600 File Offset: 0x0009E800
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat224.Square(z, array);
				SecP224R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001C4D RID: 7245 RVA: 0x000A063A File Offset: 0x0009E83A
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Sub(x, y, z) != 0)
			{
				SecP224R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06001C4E RID: 7246 RVA: 0x000A064C File Offset: 0x0009E84C
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(14, xx, yy, zz) != 0 && Nat.SubFrom(SecP224R1Field.PExtInv.Length, SecP224R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(14, zz, SecP224R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C4F RID: 7247 RVA: 0x000A067E File Offset: 0x0009E87E
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(7, x, 0U, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224R1Field.P)))
			{
				SecP224R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001C50 RID: 7248 RVA: 0x000A06A4 File Offset: 0x0009E8A4
		private static void AddPInvTo(uint[] z)
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
				Nat.IncAt(7, z, 4);
			}
		}

		// Token: 0x06001C51 RID: 7249 RVA: 0x000A0708 File Offset: 0x0009E908
		private static void SubPInvFrom(uint[] z)
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
				Nat.DecAt(7, z, 4);
			}
		}

		// Token: 0x040015EF RID: 5615
		internal static readonly uint[] P = new uint[]
		{
			1U,
			0U,
			0U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015F0 RID: 5616
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			0U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			0U,
			2U,
			0U,
			0U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015F1 RID: 5617
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1U,
			0U,
			0U,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			1U
		};

		// Token: 0x040015F2 RID: 5618
		private const uint P6 = 4294967295U;

		// Token: 0x040015F3 RID: 5619
		private const uint PExt13 = 4294967295U;
	}
}
