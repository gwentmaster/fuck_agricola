using System;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace Org.BouncyCastle.Utilities
{
	// Token: 0x02000284 RID: 644
	public abstract class BigIntegers
	{
		// Token: 0x0600155C RID: 5468 RVA: 0x00079820 File Offset: 0x00077A20
		public static byte[] AsUnsignedByteArray(BigInteger n)
		{
			return n.ToByteArrayUnsigned();
		}

		// Token: 0x0600155D RID: 5469 RVA: 0x00079828 File Offset: 0x00077A28
		public static byte[] AsUnsignedByteArray(int length, BigInteger n)
		{
			byte[] array = n.ToByteArrayUnsigned();
			if (array.Length > length)
			{
				throw new ArgumentException("standard length exceeded", "n");
			}
			if (array.Length == length)
			{
				return array;
			}
			byte[] array2 = new byte[length];
			Array.Copy(array, 0, array2, array2.Length - array.Length, array.Length);
			return array2;
		}

		// Token: 0x0600155E RID: 5470 RVA: 0x00079874 File Offset: 0x00077A74
		public static BigInteger CreateRandomInRange(BigInteger min, BigInteger max, SecureRandom random)
		{
			int num = min.CompareTo(max);
			if (num >= 0)
			{
				if (num > 0)
				{
					throw new ArgumentException("'min' may not be greater than 'max'");
				}
				return min;
			}
			else
			{
				if (min.BitLength > max.BitLength / 2)
				{
					return BigIntegers.CreateRandomInRange(BigInteger.Zero, max.Subtract(min), random).Add(min);
				}
				for (int i = 0; i < 1000; i++)
				{
					BigInteger bigInteger = new BigInteger(max.BitLength, random);
					if (bigInteger.CompareTo(min) >= 0 && bigInteger.CompareTo(max) <= 0)
					{
						return bigInteger;
					}
				}
				return new BigInteger(max.Subtract(min).BitLength - 1, random).Add(min);
			}
		}

		// Token: 0x04001394 RID: 5012
		private const int MaxIterations = 1000;
	}
}
