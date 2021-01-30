using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200030B RID: 779
	internal class SecP192K1Field
	{
		// Token: 0x06001B8E RID: 7054 RVA: 0x0009D89B File Offset: 0x0009BA9B
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Add(x, y, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06001B8F RID: 7055 RVA: 0x0009D8C8 File Offset: 0x0009BAC8
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(12, xx, yy, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192K1Field.PExt))) && Nat.AddTo(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B90 RID: 7056 RVA: 0x0009D91B File Offset: 0x0009BB1B
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(6, x, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06001B91 RID: 7057 RVA: 0x0009D948 File Offset: 0x0009BB48
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat192.FromBigInteger(x);
			if (array[5] == 4294967295U && Nat192.Gte(array, SecP192K1Field.P))
			{
				Nat192.SubFrom(SecP192K1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001B92 RID: 7058 RVA: 0x0009D97C File Offset: 0x0009BB7C
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(6, x, 0U, z);
				return;
			}
			uint c = Nat192.Add(x, SecP192K1Field.P, z);
			Nat.ShiftDownBit(6, z, c);
		}

		// Token: 0x06001B93 RID: 7059 RVA: 0x0009D9B4 File Offset: 0x0009BBB4
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Mul(x, y, array);
			SecP192K1Field.Reduce(array, z);
		}

		// Token: 0x06001B94 RID: 7060 RVA: 0x0009D9D8 File Offset: 0x0009BBD8
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat192.MulAddTo(x, y, zz) != 0U || (zz[11] == 4294967295U && Nat.Gte(12, zz, SecP192K1Field.PExt))) && Nat.AddTo(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B95 RID: 7061 RVA: 0x0009DA29 File Offset: 0x0009BC29
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat192.IsZero(x))
			{
				Nat192.Zero(z);
				return;
			}
			Nat192.Sub(SecP192K1Field.P, x, z);
		}

		// Token: 0x06001B96 RID: 7062 RVA: 0x0009DA48 File Offset: 0x0009BC48
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat192.Mul33Add(4553U, xx, 6, xx, 0, z, 0);
			if (Nat192.Mul33DWordAdd(4553U, y, z, 0) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06001B97 RID: 7063 RVA: 0x0009DA95 File Offset: 0x0009BC95
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat192.Mul33WordAdd(4553U, x, z, 0) != 0U) || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x06001B98 RID: 7064 RVA: 0x0009DACC File Offset: 0x0009BCCC
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192K1Field.Reduce(array, z);
		}

		// Token: 0x06001B99 RID: 7065 RVA: 0x0009DAF0 File Offset: 0x0009BCF0
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat192.CreateExt();
			Nat192.Square(x, array);
			SecP192K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat192.Square(z, array);
				SecP192K1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001B9A RID: 7066 RVA: 0x0009DB2A File Offset: 0x0009BD2A
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat192.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(6, 4553U, z);
			}
		}

		// Token: 0x06001B9B RID: 7067 RVA: 0x0009DB43 File Offset: 0x0009BD43
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(12, xx, yy, zz) != 0 && Nat.SubFrom(SecP192K1Field.PExtInv.Length, SecP192K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(12, zz, SecP192K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001B9C RID: 7068 RVA: 0x0009DB75 File Offset: 0x0009BD75
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(6, x, 0U, z) != 0U || (z[5] == 4294967295U && Nat192.Gte(z, SecP192K1Field.P)))
			{
				Nat.Add33To(6, 4553U, z);
			}
		}

		// Token: 0x040015CE RID: 5582
		internal static readonly uint[] P = new uint[]
		{
			4294962743U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015CF RID: 5583
		internal static readonly uint[] PExt = new uint[]
		{
			20729809U,
			9106U,
			1U,
			0U,
			0U,
			0U,
			4294958190U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015D0 RID: 5584
		private static readonly uint[] PExtInv = new uint[]
		{
			4274237487U,
			4294958189U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			9105U,
			2U
		};

		// Token: 0x040015D1 RID: 5585
		private const uint P5 = 4294967295U;

		// Token: 0x040015D2 RID: 5586
		private const uint PExt11 = 4294967295U;

		// Token: 0x040015D3 RID: 5587
		private const uint PInv33 = 4553U;
	}
}
