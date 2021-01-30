using System;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC.Multiplier;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Generators
{
	// Token: 0x02000475 RID: 1141
	internal class DHKeyGeneratorHelper
	{
		// Token: 0x06002997 RID: 10647 RVA: 0x00003425 File Offset: 0x00001625
		private DHKeyGeneratorHelper()
		{
		}

		// Token: 0x06002998 RID: 10648 RVA: 0x000CE4A0 File Offset: 0x000CC6A0
		internal BigInteger CalculatePrivate(DHParameters dhParams, SecureRandom random)
		{
			int l = dhParams.L;
			if (l != 0)
			{
				int num = l >> 2;
				BigInteger bigInteger;
				do
				{
					bigInteger = new BigInteger(l, random).SetBit(l - 1);
				}
				while (WNafUtilities.GetNafWeight(bigInteger) < num);
				return bigInteger;
			}
			BigInteger min = BigInteger.Two;
			int m = dhParams.M;
			if (m != 0)
			{
				min = BigInteger.One.ShiftLeft(m - 1);
			}
			BigInteger bigInteger2 = dhParams.Q;
			if (bigInteger2 == null)
			{
				bigInteger2 = dhParams.P;
			}
			BigInteger bigInteger3 = bigInteger2.Subtract(BigInteger.Two);
			int num2 = bigInteger3.BitLength >> 2;
			BigInteger bigInteger4;
			do
			{
				bigInteger4 = BigIntegers.CreateRandomInRange(min, bigInteger3, random);
			}
			while (WNafUtilities.GetNafWeight(bigInteger4) < num2);
			return bigInteger4;
		}

		// Token: 0x06002999 RID: 10649 RVA: 0x000CE53A File Offset: 0x000CC73A
		internal BigInteger CalculatePublic(DHParameters dhParams, BigInteger x)
		{
			return dhParams.G.ModPow(x, dhParams.P);
		}

		// Token: 0x04001B5E RID: 7006
		internal static readonly DHKeyGeneratorHelper Instance = new DHKeyGeneratorHelper();
	}
}
