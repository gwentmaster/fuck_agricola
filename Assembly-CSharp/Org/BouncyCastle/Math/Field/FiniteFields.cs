using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D2 RID: 722
	public abstract class FiniteFields
	{
		// Token: 0x060018F3 RID: 6387 RVA: 0x00092868 File Offset: 0x00090A68
		public static IPolynomialExtensionField GetBinaryExtensionField(int[] exponents)
		{
			if (exponents[0] != 0)
			{
				throw new ArgumentException("Irreducible polynomials in GF(2) must have constant term", "exponents");
			}
			for (int i = 1; i < exponents.Length; i++)
			{
				if (exponents[i] <= exponents[i - 1])
				{
					throw new ArgumentException("Polynomial exponents must be montonically increasing", "exponents");
				}
			}
			return new GenericPolynomialExtensionField(FiniteFields.GF_2, new GF2Polynomial(exponents));
		}

		// Token: 0x060018F4 RID: 6388 RVA: 0x000928C4 File Offset: 0x00090AC4
		public static IFiniteField GetPrimeField(BigInteger characteristic)
		{
			int bitLength = characteristic.BitLength;
			if (characteristic.SignValue <= 0 || bitLength < 2)
			{
				throw new ArgumentException("Must be >= 2", "characteristic");
			}
			if (bitLength < 3)
			{
				int intValue = characteristic.IntValue;
				if (intValue == 2)
				{
					return FiniteFields.GF_2;
				}
				if (intValue == 3)
				{
					return FiniteFields.GF_3;
				}
			}
			return new PrimeField(characteristic);
		}

		// Token: 0x04001554 RID: 5460
		internal static readonly IFiniteField GF_2 = new PrimeField(BigInteger.ValueOf(2L));

		// Token: 0x04001555 RID: 5461
		internal static readonly IFiniteField GF_3 = new PrimeField(BigInteger.ValueOf(3L));
	}
}
