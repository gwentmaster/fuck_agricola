using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002D1 RID: 721
	internal abstract class Nat576
	{
		// Token: 0x060018EA RID: 6378 RVA: 0x0009271A File Offset: 0x0009091A
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
			z[5] = x[5];
			z[6] = x[6];
			z[7] = x[7];
			z[8] = x[8];
		}

		// Token: 0x060018EB RID: 6379 RVA: 0x00092752 File Offset: 0x00090952
		public static ulong[] Create64()
		{
			return new ulong[9];
		}

		// Token: 0x060018EC RID: 6380 RVA: 0x0009275B File Offset: 0x0009095B
		public static ulong[] CreateExt64()
		{
			return new ulong[18];
		}

		// Token: 0x060018ED RID: 6381 RVA: 0x00092764 File Offset: 0x00090964
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 8; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018EE RID: 6382 RVA: 0x00092788 File Offset: 0x00090988
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 576)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat576.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x060018EF RID: 6383 RVA: 0x000927DC File Offset: 0x000909DC
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 9; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018F0 RID: 6384 RVA: 0x00092808 File Offset: 0x00090A08
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 9; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018F1 RID: 6385 RVA: 0x0009282C File Offset: 0x00090A2C
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[72];
			for (int i = 0; i < 9; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 8 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
