using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002CF RID: 719
	internal abstract class Nat448
	{
		// Token: 0x060018DE RID: 6366 RVA: 0x000924AA File Offset: 0x000906AA
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
			z[5] = x[5];
			z[6] = x[6];
		}

		// Token: 0x060018DF RID: 6367 RVA: 0x000924D6 File Offset: 0x000906D6
		public static ulong[] Create64()
		{
			return new ulong[7];
		}

		// Token: 0x060018E0 RID: 6368 RVA: 0x000924DE File Offset: 0x000906DE
		public static ulong[] CreateExt64()
		{
			return new ulong[14];
		}

		// Token: 0x060018E1 RID: 6369 RVA: 0x000924E8 File Offset: 0x000906E8
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 6; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018E2 RID: 6370 RVA: 0x0009250C File Offset: 0x0009070C
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 448)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat448.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x060018E3 RID: 6371 RVA: 0x00092560 File Offset: 0x00090760
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 7; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018E4 RID: 6372 RVA: 0x0009258C File Offset: 0x0009078C
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 7; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018E5 RID: 6373 RVA: 0x000925B0 File Offset: 0x000907B0
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[56];
			for (int i = 0; i < 7; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 6 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
