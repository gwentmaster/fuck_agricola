using System;
using Org.BouncyCastle.Math.EC.Endo;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F1 RID: 753
	public class GlvMultiplier : AbstractECMultiplier
	{
		// Token: 0x06001A89 RID: 6793 RVA: 0x00099A91 File Offset: 0x00097C91
		public GlvMultiplier(ECCurve curve, GlvEndomorphism glvEndomorphism)
		{
			if (curve == null || curve.Order == null)
			{
				throw new ArgumentException("Need curve with known group order", "curve");
			}
			this.curve = curve;
			this.glvEndomorphism = glvEndomorphism;
		}

		// Token: 0x06001A8A RID: 6794 RVA: 0x00099AC4 File Offset: 0x00097CC4
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			if (!this.curve.Equals(p.Curve))
			{
				throw new InvalidOperationException();
			}
			BigInteger order = p.Curve.Order;
			BigInteger[] array = this.glvEndomorphism.DecomposeScalar(k.Mod(order));
			BigInteger k2 = array[0];
			BigInteger l = array[1];
			ECPointMap pointMap = this.glvEndomorphism.PointMap;
			if (this.glvEndomorphism.HasEfficientPointMap)
			{
				return ECAlgorithms.ImplShamirsTrickWNaf(p, k2, pointMap, l);
			}
			return ECAlgorithms.ImplShamirsTrickWNaf(p, k2, pointMap.Map(p), l);
		}

		// Token: 0x04001592 RID: 5522
		protected readonly ECCurve curve;

		// Token: 0x04001593 RID: 5523
		protected readonly GlvEndomorphism glvEndomorphism;
	}
}
