using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000307 RID: 775
	internal class SecP160R2Field
	{
		// Token: 0x06001B53 RID: 6995 RVA: 0x0009CB07 File Offset: 0x0009AD07
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Add(x, y, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06001B54 RID: 6996 RVA: 0x0009CB34 File Offset: 0x0009AD34
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(10, xx, yy, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R2Field.PExt))) && Nat.AddTo(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B55 RID: 6997 RVA: 0x0009CB87 File Offset: 0x0009AD87
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(5, x, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06001B56 RID: 6998 RVA: 0x0009CBB4 File Offset: 0x0009ADB4
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat160.FromBigInteger(x);
			if (array[4] == 4294967295U && Nat160.Gte(array, SecP160R2Field.P))
			{
				Nat160.SubFrom(SecP160R2Field.P, array);
			}
			return array;
		}

		// Token: 0x06001B57 RID: 6999 RVA: 0x0009CBE8 File Offset: 0x0009ADE8
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(5, x, 0U, z);
				return;
			}
			uint c = Nat160.Add(x, SecP160R2Field.P, z);
			Nat.ShiftDownBit(5, z, c);
		}

		// Token: 0x06001B58 RID: 7000 RVA: 0x0009CC20 File Offset: 0x0009AE20
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Mul(x, y, array);
			SecP160R2Field.Reduce(array, z);
		}

		// Token: 0x06001B59 RID: 7001 RVA: 0x0009CC44 File Offset: 0x0009AE44
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat160.MulAddTo(x, y, zz) != 0U || (zz[9] == 4294967295U && Nat.Gte(10, zz, SecP160R2Field.PExt))) && Nat.AddTo(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B5A RID: 7002 RVA: 0x0009CC95 File Offset: 0x0009AE95
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat160.IsZero(x))
			{
				Nat160.Zero(z);
				return;
			}
			Nat160.Sub(SecP160R2Field.P, x, z);
		}

		// Token: 0x06001B5B RID: 7003 RVA: 0x0009CCB4 File Offset: 0x0009AEB4
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat160.Mul33Add(21389U, xx, 5, xx, 0, z, 0);
			if (Nat160.Mul33DWordAdd(21389U, y, z, 0) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06001B5C RID: 7004 RVA: 0x0009CD01 File Offset: 0x0009AF01
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat160.Mul33WordAdd(21389U, x, z, 0) != 0U) || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x06001B5D RID: 7005 RVA: 0x0009CD38 File Offset: 0x0009AF38
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R2Field.Reduce(array, z);
		}

		// Token: 0x06001B5E RID: 7006 RVA: 0x0009CD5C File Offset: 0x0009AF5C
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat160.CreateExt();
			Nat160.Square(x, array);
			SecP160R2Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat160.Square(z, array);
				SecP160R2Field.Reduce(array, z);
			}
		}

		// Token: 0x06001B5F RID: 7007 RVA: 0x0009CD96 File Offset: 0x0009AF96
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat160.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(5, 21389U, z);
			}
		}

		// Token: 0x06001B60 RID: 7008 RVA: 0x0009CDAF File Offset: 0x0009AFAF
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(10, xx, yy, zz) != 0 && Nat.SubFrom(SecP160R2Field.PExtInv.Length, SecP160R2Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(10, zz, SecP160R2Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B61 RID: 7009 RVA: 0x0009CDE1 File Offset: 0x0009AFE1
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(5, x, 0U, z) != 0U || (z[4] == 4294967295U && Nat160.Gte(z, SecP160R2Field.P)))
			{
				Nat.Add33To(5, 21389U, z);
			}
		}

		// Token: 0x040015C3 RID: 5571
		internal static readonly uint[] P = new uint[]
		{
			4294945907U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015C4 RID: 5572
		internal static readonly uint[] PExt = new uint[]
		{
			457489321U,
			42778U,
			1U,
			0U,
			0U,
			4294924518U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015C5 RID: 5573
		private static readonly uint[] PExtInv = new uint[]
		{
			3837477975U,
			4294924517U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			42777U,
			2U
		};

		// Token: 0x040015C6 RID: 5574
		private const uint P4 = 4294967295U;

		// Token: 0x040015C7 RID: 5575
		private const uint PExt9 = 4294967295U;

		// Token: 0x040015C8 RID: 5576
		private const uint PInv33 = 21389U;
	}
}
