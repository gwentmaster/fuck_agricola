using System;
using Org.BouncyCastle.Crypto.Utilities;

namespace Org.BouncyCastle.Math.Raw
{
	// Token: 0x020002CD RID: 717
	internal abstract class Nat320
	{
		// Token: 0x060018D2 RID: 6354 RVA: 0x00092245 File Offset: 0x00090445
		public static void Copy64(ulong[] x, ulong[] z)
		{
			z[0] = x[0];
			z[1] = x[1];
			z[2] = x[2];
			z[3] = x[3];
			z[4] = x[4];
		}

		// Token: 0x060018D3 RID: 6355 RVA: 0x00092265 File Offset: 0x00090465
		public static ulong[] Create64()
		{
			return new ulong[5];
		}

		// Token: 0x060018D4 RID: 6356 RVA: 0x0009226D File Offset: 0x0009046D
		public static ulong[] CreateExt64()
		{
			return new ulong[10];
		}

		// Token: 0x060018D5 RID: 6357 RVA: 0x00092278 File Offset: 0x00090478
		public static bool Eq64(ulong[] x, ulong[] y)
		{
			for (int i = 4; i >= 0; i--)
			{
				if (x[i] != y[i])
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018D6 RID: 6358 RVA: 0x0009229C File Offset: 0x0009049C
		public static ulong[] FromBigInteger64(BigInteger x)
		{
			if (x.SignValue < 0 || x.BitLength > 320)
			{
				throw new ArgumentException();
			}
			ulong[] array = Nat320.Create64();
			int num = 0;
			while (x.SignValue != 0)
			{
				array[num++] = (ulong)x.LongValue;
				x = x.ShiftRight(64);
			}
			return array;
		}

		// Token: 0x060018D7 RID: 6359 RVA: 0x000922F0 File Offset: 0x000904F0
		public static bool IsOne64(ulong[] x)
		{
			if (x[0] != 1UL)
			{
				return false;
			}
			for (int i = 1; i < 5; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018D8 RID: 6360 RVA: 0x0009231C File Offset: 0x0009051C
		public static bool IsZero64(ulong[] x)
		{
			for (int i = 0; i < 5; i++)
			{
				if (x[i] != 0UL)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060018D9 RID: 6361 RVA: 0x00092340 File Offset: 0x00090540
		public static BigInteger ToBigInteger64(ulong[] x)
		{
			byte[] array = new byte[40];
			for (int i = 0; i < 5; i++)
			{
				ulong num = x[i];
				if (num != 0UL)
				{
					Pack.UInt64_To_BE(num, array, 4 - i << 3);
				}
			}
			return new BigInteger(1, array);
		}
	}
}
