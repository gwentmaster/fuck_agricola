using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x0200031B RID: 795
	internal class SecP256K1Field
	{
		// Token: 0x06001C83 RID: 7299 RVA: 0x000A12BF File Offset: 0x0009F4BF
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Add(x, y, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06001C84 RID: 7300 RVA: 0x000A12EC File Offset: 0x0009F4EC
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(16, xx, yy, zz) != 0U || (zz[15] == 4294967295U && Nat.Gte(16, zz, SecP256K1Field.PExt))) && Nat.AddTo(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C85 RID: 7301 RVA: 0x000A133F File Offset: 0x0009F53F
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(8, x, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06001C86 RID: 7302 RVA: 0x000A136C File Offset: 0x0009F56C
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat256.FromBigInteger(x);
			if (array[7] == 4294967295U && Nat256.Gte(array, SecP256K1Field.P))
			{
				Nat256.SubFrom(SecP256K1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001C87 RID: 7303 RVA: 0x000A13A0 File Offset: 0x0009F5A0
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(8, x, 0U, z);
				return;
			}
			uint c = Nat256.Add(x, SecP256K1Field.P, z);
			Nat.ShiftDownBit(8, z, c);
		}

		// Token: 0x06001C88 RID: 7304 RVA: 0x000A13D8 File Offset: 0x0009F5D8
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Mul(x, y, array);
			SecP256K1Field.Reduce(array, z);
		}

		// Token: 0x06001C89 RID: 7305 RVA: 0x000A13FC File Offset: 0x0009F5FC
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat256.MulAddTo(x, y, zz) != 0U || (zz[15] == 4294967295U && Nat.Gte(16, zz, SecP256K1Field.PExt))) && Nat.AddTo(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C8A RID: 7306 RVA: 0x000A144D File Offset: 0x0009F64D
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat256.IsZero(x))
			{
				Nat256.Zero(z);
				return;
			}
			Nat256.Sub(SecP256K1Field.P, x, z);
		}

		// Token: 0x06001C8B RID: 7307 RVA: 0x000A146C File Offset: 0x0009F66C
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat256.Mul33Add(977U, xx, 8, xx, 0, z, 0);
			if (Nat256.Mul33DWordAdd(977U, y, z, 0) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06001C8C RID: 7308 RVA: 0x000A14B9 File Offset: 0x0009F6B9
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat256.Mul33WordAdd(977U, x, z, 0) != 0U) || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x06001C8D RID: 7309 RVA: 0x000A14F0 File Offset: 0x0009F6F0
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256K1Field.Reduce(array, z);
		}

		// Token: 0x06001C8E RID: 7310 RVA: 0x000A1514 File Offset: 0x0009F714
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat256.CreateExt();
			Nat256.Square(x, array);
			SecP256K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat256.Square(z, array);
				SecP256K1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001C8F RID: 7311 RVA: 0x000A154E File Offset: 0x0009F74E
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat256.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(8, 977U, z);
			}
		}

		// Token: 0x06001C90 RID: 7312 RVA: 0x000A1567 File Offset: 0x0009F767
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(16, xx, yy, zz) != 0 && Nat.SubFrom(SecP256K1Field.PExtInv.Length, SecP256K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(16, zz, SecP256K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C91 RID: 7313 RVA: 0x000A1599 File Offset: 0x0009F799
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(8, x, 0U, z) != 0U || (z[7] == 4294967295U && Nat256.Gte(z, SecP256K1Field.P)))
			{
				Nat.Add33To(8, 977U, z);
			}
		}

		// Token: 0x040015F9 RID: 5625
		internal static readonly uint[] P = new uint[]
		{
			4294966319U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015FA RID: 5626
		internal static readonly uint[] PExt = new uint[]
		{
			954529U,
			1954U,
			1U,
			0U,
			0U,
			0U,
			0U,
			0U,
			4294965342U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015FB RID: 5627
		private static readonly uint[] PExtInv = new uint[]
		{
			4294012767U,
			4294965341U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			1953U,
			2U
		};

		// Token: 0x040015FC RID: 5628
		private const uint P7 = 4294967295U;

		// Token: 0x040015FD RID: 5629
		private const uint PExt15 = 4294967295U;

		// Token: 0x040015FE RID: 5630
		private const uint PInv33 = 977U;
	}
}
