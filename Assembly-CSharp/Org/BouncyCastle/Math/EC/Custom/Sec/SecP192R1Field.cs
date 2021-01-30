using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200030F RID: 783
	internal class SecP192R1Field
	{
		// Token: 0x06001BC9 RID: 7113 RVA: 0x0009E5E7 File Offset: 0x0009C7E7
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Add(x, y, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001BCA RID: 7114 RVA: 0x0009E60C File Offset: 0x0009C80C
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(12, xx, yy, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192R1Field.PExt))) && Nat.AddTo(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001BCB RID: 7115 RVA: 0x0009E65F File Offset: 0x0009C85F
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(6, x, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001BCC RID: 7116 RVA: 0x0009E684 File Offset: 0x0009C884
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat192.FromBigInteger(x);
			if (array[5] == 4294967295U && Nat192.Gte(array, SecP192R1Field.P))
			{
				Nat192.SubFrom(SecP192R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001BCD RID: 7117 RVA: 0x0009E6B8 File Offset: 0x0009C8B8
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(6, x, 0U, z);
				return;
			}
			uint c = Nat192.Add(x, SecP192R1Field.P, z);
			Nat.ShiftDownBit(6, z, c);
		}

		// Token: 0x06001BCE RID: 7118 RVA: 0x0009E6F0 File Offset: 0x0009C8F0
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Mul(x, y, array);
			SecP192R1Field.Reduce(array, z);
		}

		// Token: 0x06001BCF RID: 7119 RVA: 0x0009E714 File Offset: 0x0009C914
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat192.MulAddTo(x, y, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192R1Field.PExt))) && Nat.AddTo(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001BD0 RID: 7120 RVA: 0x0009E765 File Offset: 0x0009C965
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat192.IsZero(x))
			{
				Nat192.Zero(z);
				return;
			}
			Nat192.Sub(SecP192R1Field.P, x, z);
		}

		// Token: 0x06001BD1 RID: 7121 RVA: 0x0009E784 File Offset: 0x0009C984
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[6];
			ulong num2 = (ulong)xx[7];
			ulong num3 = (ulong)xx[8];
			ulong num4 = (ulong)xx[9];
			ulong num5 = (ulong)xx[10];
			ulong num6 = (ulong)xx[11];
			ulong num7 = num + num5;
			ulong num8 = num2 + num6;
			ulong num9 = 0UL;
			num9 += (ulong)xx[0] + num7;
			uint num10 = (uint)num9;
			num9 >>= 32;
			num9 += (ulong)xx[1] + num8;
			z[1] = (uint)num9;
			num9 >>= 32;
			num7 += num3;
			num8 += num4;
			num9 += (ulong)xx[2] + num7;
			ulong num11 = (ulong)((uint)num9);
			num9 >>= 32;
			num9 += (ulong)xx[3] + num8;
			z[3] = (uint)num9;
			num9 >>= 32;
			num7 -= num;
			num8 -= num2;
			num9 += (ulong)xx[4] + num7;
			z[4] = (uint)num9;
			num9 >>= 32;
			num9 += (ulong)xx[5] + num8;
			z[5] = (uint)num9;
			num9 >>= 32;
			num11 += num9;
			num9 += (ulong)num10;
			z[0] = (uint)num9;
			num9 >>= 32;
			if (num9 != 0UL)
			{
				num9 += (ulong)z[1];
				z[1] = (uint)num9;
				num11 += num9 >> 32;
			}
			z[2] = (uint)num11;
			num9 = num11 >> 32;
			if ((num9 != 0UL && Nat.IncAt(6, z, 3) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001BD2 RID: 7122 RVA: 0x0009E8E0 File Offset: 0x0009CAE0
		public static void Reduce32(uint x, uint[] z)
		{
			ulong num = 0UL;
			if (x != 0U)
			{
				num += (ulong)z[0] + (ulong)x;
				z[0] = (uint)num;
				num >>= 32;
				if (num != 0UL)
				{
					num += (ulong)z[1];
					z[1] = (uint)num;
					num >>= 32;
				}
				num += (ulong)z[2] + (ulong)x;
				z[2] = (uint)num;
				num >>= 32;
			}
			if ((num != 0UL && Nat.IncAt(6, z, 3) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001BD3 RID: 7123 RVA: 0x0009E958 File Offset: 0x0009CB58
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192R1Field.Reduce(array, z);
		}

		// Token: 0x06001BD4 RID: 7124 RVA: 0x0009E97C File Offset: 0x0009CB7C
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat192.Square(z, array);
				SecP192R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001BD5 RID: 7125 RVA: 0x0009E9B6 File Offset: 0x0009CBB6
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Sub(x, y, z) != 0)
			{
				SecP192R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06001BD6 RID: 7126 RVA: 0x0009E9C8 File Offset: 0x0009CBC8
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(12, xx, yy, zz) != 0 && Nat.SubFrom(SecP192R1Field.PExtInv.Length, SecP192R1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(12, zz, SecP192R1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001BD7 RID: 7127 RVA: 0x0009E9FA File Offset: 0x0009CBFA
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(6, x, 0U, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192R1Field.P)))
			{
				SecP192R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001BD8 RID: 7128 RVA: 0x0009EA20 File Offset: 0x0009CC20
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
			}
			num += (long)((ulong)z[2] + 1UL);
			z[2] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.IncAt(6, z, 3);
			}
		}

		// Token: 0x06001BD9 RID: 7129 RVA: 0x0009EA74 File Offset: 0x0009CC74
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
			}
			num += (long)((ulong)z[2] - 1UL);
			z[2] = (uint)num;
			num >>= 32;
			if (num != 0L)
			{
				Nat.DecAt(6, z, 3);
			}
		}

		// Token: 0x040015D9 RID: 5593
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015DA RID: 5594
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			2U,
			0U,
			1U,
			0U,
			4294967294U,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015DB RID: 5595
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			4294967293U,
			uint.MaxValue,
			4294967294U,
			uint.MaxValue,
			1U,
			0U,
			2U
		};

		// Token: 0x040015DC RID: 5596
		private const uint P5 = 4294967295U;

		// Token: 0x040015DD RID: 5597
		private const uint PExt11 = 4294967295U;
	}
}
