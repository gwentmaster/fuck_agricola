using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000313 RID: 787
	internal class SecP224K1Field
	{
		// Token: 0x06001C06 RID: 7174 RVA: 0x0009F477 File Offset: 0x0009D677
		public static void Add(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Add(x, y, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x06001C07 RID: 7175 RVA: 0x0009F4A4 File Offset: 0x0009D6A4
		public static void AddExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if ((Nat.Add(14, xx, yy, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224K1Field.PExt))) && Nat.AddTo(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C08 RID: 7176 RVA: 0x0009F4F7 File Offset: 0x0009D6F7
		public static void AddOne(uint[] x, uint[] z)
		{
			if (Nat.Inc(7, x, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x06001C09 RID: 7177 RVA: 0x0009F524 File Offset: 0x0009D724
		public static uint[] FromBigInteger(BigInteger x)
		{
			uint[] array = Nat224.FromBigInteger(x);
			if (array[6] == 4294967295U && Nat224.Gte(array, SecP224K1Field.P))
			{
				Nat224.SubFrom(SecP224K1Field.P, array);
			}
			return array;
		}

		// Token: 0x06001C0A RID: 7178 RVA: 0x0009F558 File Offset: 0x0009D758
		public static void Half(uint[] x, uint[] z)
		{
			if ((x[0] & 1U) == 0U)
			{
				Nat.ShiftDownBit(7, x, 0U, z);
				return;
			}
			uint c = Nat224.Add(x, SecP224K1Field.P, z);
			Nat.ShiftDownBit(7, z, c);
		}

		// Token: 0x06001C0B RID: 7179 RVA: 0x0009F590 File Offset: 0x0009D790
		public static void Multiply(uint[] x, uint[] y, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Mul(x, y, array);
			SecP224K1Field.Reduce(array, z);
		}

		// Token: 0x06001C0C RID: 7180 RVA: 0x0009F5B4 File Offset: 0x0009D7B4
		public static void MultiplyAddToExt(uint[] x, uint[] y, uint[] zz)
		{
			if ((Nat224.MulAddTo(x, y, zz) != 0U || (zz[13] == 4294967295U && Nat.Gte(14, zz, SecP224K1Field.PExt))) && Nat.AddTo(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0U)
			{
				Nat.IncAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C0D RID: 7181 RVA: 0x0009F605 File Offset: 0x0009D805
		public static void Negate(uint[] x, uint[] z)
		{
			if (Nat224.IsZero(x))
			{
				Nat224.Zero(z);
				return;
			}
			Nat224.Sub(SecP224K1Field.P, x, z);
		}

		// Token: 0x06001C0E RID: 7182 RVA: 0x0009F624 File Offset: 0x0009D824
		public static void Reduce(uint[] xx, uint[] z)
		{
			ulong y = Nat224.Mul33Add(6803U, xx, 7, xx, 0, z, 0);
			if (Nat224.Mul33DWordAdd(6803U, y, z, 0) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x06001C0F RID: 7183 RVA: 0x0009F671 File Offset: 0x0009D871
		public static void Reduce32(uint x, uint[] z)
		{
			if ((x != 0U && Nat224.Mul33WordAdd(6803U, x, z, 0) != 0U) || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x06001C10 RID: 7184 RVA: 0x0009F6A8 File Offset: 0x0009D8A8
		public static void Square(uint[] x, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224K1Field.Reduce(array, z);
		}

		// Token: 0x06001C11 RID: 7185 RVA: 0x0009F6CC File Offset: 0x0009D8CC
		public static void SquareN(uint[] x, int n, uint[] z)
		{
			uint[] array = Nat224.CreateExt();
			Nat224.Square(x, array);
			SecP224K1Field.Reduce(array, z);
			while (--n > 0)
			{
				Nat224.Square(z, array);
				SecP224K1Field.Reduce(array, z);
			}
		}

		// Token: 0x06001C12 RID: 7186 RVA: 0x0009F706 File Offset: 0x0009D906
		public static void Subtract(uint[] x, uint[] y, uint[] z)
		{
			if (Nat224.Sub(x, y, z) != 0)
			{
				Nat.Sub33From(7, 6803U, z);
			}
		}

		// Token: 0x06001C13 RID: 7187 RVA: 0x0009F71F File Offset: 0x0009D91F
		public static void SubtractExt(uint[] xx, uint[] yy, uint[] zz)
		{
			if (Nat.Sub(14, xx, yy, zz) != 0 && Nat.SubFrom(SecP224K1Field.PExtInv.Length, SecP224K1Field.PExtInv, zz) != 0)
			{
				Nat.DecAt(14, zz, SecP224K1Field.PExtInv.Length);
			}
		}

		// Token: 0x06001C14 RID: 7188 RVA: 0x0009F751 File Offset: 0x0009D951
		public static void Twice(uint[] x, uint[] z)
		{
			if (Nat.ShiftUpBit(7, x, 0U, z) != 0U || (z[6] == 4294967295U && Nat224.Gte(z, SecP224K1Field.P)))
			{
				Nat.Add33To(7, 6803U, z);
			}
		}

		// Token: 0x040015E3 RID: 5603
		internal static readonly uint[] P = new uint[]
		{
			4294960493U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015E4 RID: 5604
		internal static readonly uint[] PExt = new uint[]
		{
			46280809U,
			13606U,
			1U,
			0U,
			0U,
			0U,
			0U,
			4294953690U,
			4294967293U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue
		};

		// Token: 0x040015E5 RID: 5605
		private static readonly uint[] PExtInv = new uint[]
		{
			4248686487U,
			4294953689U,
			4294967294U,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			uint.MaxValue,
			13605U,
			2U
		};

		// Token: 0x040015E6 RID: 5606
		private const uint P6 = 4294967295U;

		// Token: 0x040015E7 RID: 5607
		private const uint PExt13 = 4294967295U;

		// Token: 0x040015E8 RID: 5608
		private const uint PInv33 = 6803U;
	}
}
