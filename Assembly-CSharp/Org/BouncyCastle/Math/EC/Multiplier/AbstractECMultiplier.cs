using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002EC RID: 748
	public abstract class AbstractECMultiplier : ECMultiplier
	{
		// Token: 0x06001A78 RID: 6776 RVA: 0x00099800 File Offset: 0x00097A00
		public virtual ECPoint Multiply(ECPoint p, BigInteger k)
		{
			int signValue = k.SignValue;
			if (signValue == 0 || p.IsInfinity)
			{
				return p.Curve.Infinity;
			}
			ECPoint ecpoint = this.MultiplyPositive(p, k.Abs());
			return ECAlgorithms.ValidatePoint((signValue > 0) ? ecpoint : ecpoint.Negate());
		}

		// Token: 0x06001A79 RID: 6777
		protected abstract ECPoint MultiplyPositive(ECPoint p, BigInteger k);
	}
}
