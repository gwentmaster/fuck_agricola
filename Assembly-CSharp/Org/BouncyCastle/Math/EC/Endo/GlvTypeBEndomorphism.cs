using System;

namespace Org.BouncyCastle.Math.EC.Endo
{
	// Token: 0x020002FA RID: 762
	public class GlvTypeBEndomorphism : GlvEndomorphism, ECEndomorphism
	{
		// Token: 0x06001AB1 RID: 6833 RVA: 0x0009A72A File Offset: 0x0009892A
		public GlvTypeBEndomorphism(ECCurve curve, GlvTypeBParameters parameters)
		{
			this.m_curve = curve;
			this.m_parameters = parameters;
			this.m_pointMap = new ScaleXPointMap(curve.FromBigInteger(parameters.Beta));
		}

		// Token: 0x06001AB2 RID: 6834 RVA: 0x0009A758 File Offset: 0x00098958
		public virtual BigInteger[] DecomposeScalar(BigInteger k)
		{
			int bits = this.m_parameters.Bits;
			BigInteger bigInteger = this.CalculateB(k, this.m_parameters.G1, bits);
			BigInteger bigInteger2 = this.CalculateB(k, this.m_parameters.G2, bits);
			BigInteger[] v = this.m_parameters.V1;
			BigInteger[] v2 = this.m_parameters.V2;
			BigInteger bigInteger3 = k.Subtract(bigInteger.Multiply(v[0]).Add(bigInteger2.Multiply(v2[0])));
			BigInteger bigInteger4 = bigInteger.Multiply(v[1]).Add(bigInteger2.Multiply(v2[1])).Negate();
			return new BigInteger[]
			{
				bigInteger3,
				bigInteger4
			};
		}

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06001AB3 RID: 6835 RVA: 0x0009A801 File Offset: 0x00098A01
		public virtual ECPointMap PointMap
		{
			get
			{
				return this.m_pointMap;
			}
		}

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x06001AB4 RID: 6836 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual bool HasEfficientPointMap
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06001AB5 RID: 6837 RVA: 0x0009A80C File Offset: 0x00098A0C
		protected virtual BigInteger CalculateB(BigInteger k, BigInteger g, int t)
		{
			bool flag = g.SignValue < 0;
			BigInteger bigInteger = k.Multiply(g.Abs());
			bool flag2 = bigInteger.TestBit(t - 1);
			bigInteger = bigInteger.ShiftRight(t);
			if (flag2)
			{
				bigInteger = bigInteger.Add(BigInteger.One);
			}
			if (!flag)
			{
				return bigInteger;
			}
			return bigInteger.Negate();
		}

		// Token: 0x0400159E RID: 5534
		protected readonly ECCurve m_curve;

		// Token: 0x0400159F RID: 5535
		protected readonly GlvTypeBParameters m_parameters;

		// Token: 0x040015A0 RID: 5536
		protected readonly ECPointMap m_pointMap;
	}
}
