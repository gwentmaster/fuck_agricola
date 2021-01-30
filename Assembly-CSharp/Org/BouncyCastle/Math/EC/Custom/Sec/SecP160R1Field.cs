using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000303 RID: 771
	internal class SecP160R1Field
	{
		// Token: 0x06001B18 RID: 6936 RVA: 0x0009BD83 File Offset: 0x00099F83
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Add(x, y, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x06001B19 RID: 6937 RVA: 0x0009BDB0 File Offset: 0x00099FB0
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(10, xx, yy, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R1Field.PExt))) && Nat.AddTo(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B1A RID: 6938 RVA: 0x0009BE03 File Offset: 0x0009A003
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(5, x, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x06001B1B RID: 6939 RVA: 0x0009BE30 File Offset: 0x0009A030
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat160.FromBigInteger(x);
			if (array[4] == 4294967295U && Nat160.Gte(array, SecP160R1Field.P))
			{
				Nat160.SubFrom(SecP160R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001B1C RID: 6940 RVA: 0x0009BE64 File Offset: 0x0009A064
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(5, x, 0U, z);
				return;
			}
			uint c = Nat160.Add(x, SecP160R1Field.P, z);
			Nat.ShiftDownBit(5, z, c);
		}

		// Token: 0x06001B1D RID: 6941 RVA: 0x0009BE9C File Offset: 0x0009A09C
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Mul(x, y, array);
			SecP160R1Field.Reduce(array, z);
		}

		// Token: 0x06001B1E RID: 6942 RVA: 0x0009BEC0 File Offset: 0x0009A0C0
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat160.MulAddTo(x, y, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R1Field.PExt))) && Nat.AddTo(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B1F RID: 6943 RVA: 0x0009BF11 File Offset: 0x0009A111
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat160.IsZero(x))
			{
				Nat160.Zero(z);
				return;
			}
			Nat160.Sub(SecP160R1Field.P, x, z);
		}

		// Token: 0x06001B20 RID: 6944 RVA: 0x0009BF30 File Offset: 0x0009A130
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[5];
			ulong num2 = (ulong)xx[6];
			ulong num3 = (ulong)xx[7];
			ulong num4 = (ulong)xx[8];
			ulong num5 = (ulong)xx[9];
			ulong num6 = 0UL;
			num6 += (ulong)xx[0] + num + (num << 31);
			z[0] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[1] + num2 + (num2 << 31);
			z[1] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[2] + num3 + (num3 << 31);
			z[2] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[3] + num4 + (num4 << 31);
			z[3] = (uint)num6;
			num6 >>= 32;
			num6 += (ulong)xx[4] + num5 + (num5 << 31);
			z[4] = (uint)num6;
			num6 >>= 32;
			SecP160R1Field.Reduce32((uint)num6, z);
		}

		// Token: 0x06001B21 RID: 6945 RVA: 0x0009BFF8 File Offset: 0x0009A1F8
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat160.MulWordsAdd(2147483649U, x, z, 0) != 0U) || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x06001B22 RID: 6946 RVA: 0x0009C02C File Offset: 0x0009A22C
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R1Field.Reduce(array, z);
		}

		// Token: 0x06001B23 RID: 6947 RVA: 0x0009C050 File Offset: 0x0009A250
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat160.Square(z, array);
				SecP160R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001B24 RID: 6948 RVA: 0x0009C08A File Offset: 0x0009A28A
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Sub(x, y, z) != 0)
			{
				Nat.SubWordFrom(5, 2147483649U, z);
			}
		}

		// Token: 0x06001B25 RID: 6949 RVA: 0x0009C0A3 File Offset: 0x0009A2A3
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0 && Nat.SubFrom(SecP160R1Field.PExtInv.Length, SecP160R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(10, zz, SecP160R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B26 RID: 6950 RVA: 0x0009C0D5 File Offset: 0x0009A2D5
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(5, x, 0U, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R1Field.P)))
			{
				Nat.AddWordTo(5, 2147483649U, z);
			}
		}

		// Token: 0x040015B8 RID: 5560
		internal static readonly uint[] P = new uint[]
		{
			2147483647U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015B9 RID: 5561
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			1073741825U,
			0U,
			0U,
			0U,
			4294967294U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015BA RID: 5562
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			3221225470U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1U,
			1U
		};

		// Token: 0x040015BB RID: 5563
		private const uint P4 = 4294967295U;

		// Token: 0x040015BC RID: 5564
		private const uint PExt9 = 4294967295U;

		// Token: 0x040015BD RID: 5565
		private const uint PInv = 2147483649U;
	}
}
