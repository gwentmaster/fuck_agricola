using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x020002FD RID: 765
	internal class SecP128R1Field
	{
		// Token: 0x06001AC8 RID: 6856 RVA: 0x0009A9B8 File Offset: 0x00098BB8
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat128.Add(x, y, z) != 0U || (z[3] == 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0009A9DE File Offset: 0x00098BDE
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat256.Add(xx, yy, zz) != 0U || (zz[7] == 4294967292U && Nat256.Gte(zz, SecP128R1Field.PExt)))
			{
				Nat.AddTo(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x0009AA11 File Offset: 0x00098C11
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(4, x, z) != 0U || (z[3] == 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0009AA38 File Offset: 0x00098C38
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat128.FromBigInteger(x);
			if (array[3] == 4294967293U && Nat128.Gte(array, SecP128R1Field.P))
			{
				Nat128.SubFrom(SecP128R1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x0009AA70 File Offset: 0x00098C70
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(4, x, 0U, z);
				return;
			}
			uint c = Nat128.Add(x, SecP128R1Field.P, z);
			Nat.ShiftDownBit(4, z, c);
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0009AAA8 File Offset: 0x00098CA8
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Mul(x, y, array);
			SecP128R1Field.Reduce(array, z);
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x0009AACA File Offset: 0x00098CCA
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if (Nat128.MulAddTo(x, y, zz) != 0U || (zz[7] == 4294967292U && Nat256.Gte(zz, SecP128R1Field.PExt)))
			{
				Nat.AddTo(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0009AAFD File Offset: 0x00098CFD
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat128.IsZero(x))
			{
				Nat128.Zero(z);
				return;
			}
			Nat128.Sub(SecP128R1Field.P, x, z);
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x0009AB1C File Offset: 0x00098D1C
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong num = (ulong)xx[0];
			ulong num2 = (ulong)xx[1];
			ulong num3 = (ulong)xx[2];
			ulong num4 = (ulong)xx[3];
			ulong num5 = (ulong)xx[4];
			ulong num6 = (ulong)xx[5];
			ulong num7 = (ulong)xx[6];
			ulong num8 = (ulong)xx[7];
			num4 += num8;
			num7 += num8 << 1;
			num3 += num7;
			num6 += num7 << 1;
			num2 += num6;
			num5 += num6 << 1;
			num += num5;
			num4 += num5 << 1;
			z[0] = (uint)num;
			num2 += num >> 32;
			z[1] = (uint)num2;
			num3 += num2 >> 32;
			z[2] = (uint)num3;
			num4 += num3 >> 32;
			z[3] = (uint)num4;
			SecP128R1Field.Reduce32((uint)(num4 >> 32), z);
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0009ABC0 File Offset: 0x00098DC0
		public static void Reduce32(uint x, uint[] z)
		{
			while (x != 0U)
			{
				ulong num = (ulong)x;
				ulong num2 = (ulong)z[0] + num;
				z[0] = (uint)num2;
				num2 >>= 32;
				if (num2 != 0UL)
				{
					num2 += (ulong)z[1];
					z[1] = (uint)num2;
					num2 >>= 32;
					num2 += (ulong)z[2];
					z[2] = (uint)num2;
					num2 >>= 32;
				}
				num2 += (ulong)z[3] + (num << 1);
				z[3] = (uint)num2;
				num2 >>= 32;
				x = (uint)num2;
			}
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x0009AC24 File Offset: 0x00098E24
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Square(x, array);
			SecP128R1Field.Reduce(array, z);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x0009AC48 File Offset: 0x00098E48
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat128.CreateExt();
			Nat128.Square(x, array);
			SecP128R1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat128.Square(z, array);
				SecP128R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x0009AC82 File Offset: 0x00098E82
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat128.Sub(x, y, z) != 0)
			{
				SecP128R1Field.SubPInvFrom(z);
			}
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x0009AC94 File Offset: 0x00098E94
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0)
			{
				Nat.SubFrom(SecP128R1Field.PExtInv.Length, SecP128R1Field.PExtInv, zz);
			}
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0009ACB5 File Offset: 0x00098EB5
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(4, x, 0U, z) != 0U || (z[3] == 4294967293U && Nat128.Gte(z, SecP128R1Field.P)))
			{
				SecP128R1Field.AddPInvTo(z);
			}
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x0009ACDC File Offset: 0x00098EDC
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
			num += (long)((ulong)z[3] + 2UL);
			z[3] = (uint)num;
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x0009AD30 File Offset: 0x00098F30
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
			num += (long)((ulong)z[3] - 2UL);
			z[3] = (uint)num;
		}

		// Token: 0x040015AB RID: 5547
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			4294967293U
		};

		// Token: 0x040015AC RID: 5548
		internal static readonly uint[] PExt = new uint[]
		{
			1U,
			0U,
			0U,
			4U,
			4294967294U,
			uint.MaxValue,
			3U,
			4294967292U
		};

		// Token: 0x040015AD RID: 5549
		private static readonly uint[] PExtInv = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			4294967291U,
			1U,
			0U,
			4294967292U,
			3U
		};

		// Token: 0x040015AE RID: 5550
		private const uint P3 = 4294967293U;

		// Token: 0x040015AF RID: 5551
		private const uint PExt7 = 4294967292U;
	}
}
