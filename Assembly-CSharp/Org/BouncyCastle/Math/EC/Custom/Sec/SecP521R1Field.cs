using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000327 RID: 807
	internal class SecP521R1Field
	{
		// Token: 0x06001D37 RID: 7479 RVA: 0x000A3FFC File Offset: 0x000A21FC
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			uint num = Nat.Add(16, x, y, z) + x[16] + y[16];
			if (num > 511U || (num == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num += Nat.Inc(16, z);
				num &= 511U;
			}
			z[16] = num;
		}

		// Token: 0x06001D38 RID: 7480 RVA: 0x000A4058 File Offset: 0x000A2258
		public static void AddOne(uint[] x, uint[] z)
		{
			uint num = Nat.Inc(16, x, z) + x[16];
			if (num > 511U || (num == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num += Nat.Inc(16, z);
				num &= 511U;
			}
			z[16] = num;
		}

		// Token: 0x06001D39 RID: 7481 RVA: 0x000A40AC File Offset: 0x000A22AC
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat.FromBigInteger(521, x);
			if (Nat.Eq(17, array, SecP521R1Field.P))
			{
				Nat.Zero(17, array);
			}
			return array;
		}

		// Token: 0x06001D3A RID: 7482 RVA: 0x000A40E0 File Offset: 0x000A22E0
		public static void Half(uint[] x, uint[] z)
		{
			uint num = x[16];
			uint num2 = Nat.ShiftDownBit(16, x, num, z);
			z[16] = (num >> 1 | num2 >> 23);
		}

		// Token: 0x06001D3B RID: 7483 RVA: 0x000A410C File Offset: 0x000A230C
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplMultiply(x, y, array);
			SecP521R1Field.Reduce(array, z);
		}

		// Token: 0x06001D3C RID: 7484 RVA: 0x000A4130 File Offset: 0x000A2330
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat.IsZero(17, x))
			{
				Nat.Zero(17, z);
				return;
			}
			Nat.Sub(17, SecP521R1Field.P, x, z);
		}

		// Token: 0x06001D3D RID: 7485 RVA: 0x000A4154 File Offset: 0x000A2354
		public static void Reduce(uint[] xx, uint[] z)
		{
			uint num = xx[32];
			uint num2 = Nat.ShiftDownBits(16, xx, 16, 9, num, z, 0) >> 23;
			num2 += num >> 9;
			num2 += Nat.AddTo(16, xx, z);
			if (num2 > 511U || (num2 == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num2 += Nat.Inc(16, z);
				num2 &= 511U;
			}
			z[16] = num2;
		}

		// Token: 0x06001D3E RID: 7486 RVA: 0x000A41C4 File Offset: 0x000A23C4
		public static void Reduce23(uint[] z)
		{
			uint num = z[16];
			uint num2 = Nat.AddWordTo(16, num >> 9, z) + (num & 511U);
			if (num2 > 511U || (num2 == 511U && Nat.Eq(16, z, SecP521R1Field.P)))
			{
				num2 += Nat.Inc(16, z);
				num2 &= 511U;
			}
			z[16] = num2;
		}

		// Token: 0x06001D3F RID: 7487 RVA: 0x000A4224 File Offset: 0x000A2424
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplSquare(x, array);
			SecP521R1Field.Reduce(array, z);
		}

		// Token: 0x06001D40 RID: 7488 RVA: 0x000A4248 File Offset: 0x000A2448
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat.Create(33);
			SecP521R1Field.ImplSquare(x, array);
			SecP521R1Field.Reduce(array, z);
			while (--n > 0)
			{
				SecP521R1Field.ImplSquare(z, array);
				SecP521R1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001D41 RID: 7489 RVA: 0x000A4284 File Offset: 0x000A2484
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			int num = Nat.Sub(16, x, y, z) + (int)(x[16] - y[16]);
			if (num < 0)
			{
				num += Nat.Dec(16, z);
				num &= 511;
			}
			z[16] = (uint)num;
		}

		// Token: 0x06001D42 RID: 7490 RVA: 0x000A42C4 File Offset: 0x000A24C4
		public static void Twice(uint[] x, uint[] z)
		{
			uint num = x[16];
			uint num2 = Nat.ShiftUpBit(16, x, num << 23, z) | num << 1;
			z[16] = (num2 & 511U);
		}

		// Token: 0x06001D43 RID: 7491 RVA: 0x000A42F4 File Offset: 0x000A24F4
		protected static void ImplMultiply(uint[] x, uint[] y, uint[] zz)
		{
			Nat512.Mul(x, y, zz);
			uint num = x[16];
			uint num2 = y[16];
			zz[32] = Nat.Mul31BothAdd(16, num, y, num2, x, zz, 16) + num * num2;
		}

		// Token: 0x06001D44 RID: 7492 RVA: 0x000A432C File Offset: 0x000A252C
		protected static void ImplSquare(uint[] x, uint[] zz)
		{
			Nat512.Square(x, zz);
			uint num = x[16];
			zz[32] = Nat.MulWordAddTo(16, num << 1, x, 0, zz, 16) + num * num;
		}

		// Token: 0x04001617 RID: 5655
		internal static readonly uint[] P = new uint[]
		{
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			511U
		};

		// Token: 0x04001618 RID: 5656
		private const int P16 = 511;
	}
}
