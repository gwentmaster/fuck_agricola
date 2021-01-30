using System;
using Org.BouncyCastle.Math.EC.Abc;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F6 RID: 758
	public class WTauNafMultiplier : AbstractECMultiplier
	{
		// Token: 0x06001AA6 RID: 6822 RVA: 0x0009A530 File Offset: 0x00098730
		protected override ECPoint MultiplyPositive(ECPoint point, BigInteger k)
		{
			if (!(point is AbstractF2mPoint))
			{
				throw new ArgumentException("Only AbstractF2mPoint can be used in WTauNafMultiplier");
			}
			AbstractF2mPoint abstractF2mPoint = (AbstractF2mPoint)point;
			AbstractF2mCurve abstractF2mCurve = (AbstractF2mCurve)abstractF2mPoint.Curve;
			int fieldSize = abstractF2mCurve.FieldSize;
			sbyte b = (sbyte)abstractF2mCurve.A.ToBigInteger().IntValue;
			sbyte mu = Tnaf.GetMu((int)b);
			BigInteger[] si = abstractF2mCurve.GetSi();
			ZTauElement lambda = Tnaf.PartModReduction(k, fieldSize, b, si, mu, 10);
			return this.MultiplyWTnaf(abstractF2mPoint, lambda, abstractF2mCurve.GetPreCompInfo(abstractF2mPoint, WTauNafMultiplier.PRECOMP_NAME), b, mu);
		}

		// Token: 0x06001AA7 RID: 6823 RVA: 0x0009A5B4 File Offset: 0x000987B4
		private AbstractF2mPoint MultiplyWTnaf(AbstractF2mPoint p, ZTauElement lambda, PreCompInfo preCompInfo, sbyte a, sbyte mu)
		{
			ZTauElement[] alpha = (a == 0) ? Tnaf.Alpha0 : Tnaf.Alpha1;
			BigInteger tw = Tnaf.GetTw(mu, 4);
			sbyte[] u = Tnaf.TauAdicWNaf(mu, lambda, 4, BigInteger.ValueOf(16L), tw, alpha);
			return WTauNafMultiplier.MultiplyFromWTnaf(p, u, preCompInfo);
		}

		// Token: 0x06001AA8 RID: 6824 RVA: 0x0009A5F8 File Offset: 0x000987F8
		private static AbstractF2mPoint MultiplyFromWTnaf(AbstractF2mPoint p, sbyte[] u, PreCompInfo preCompInfo)
		{
			AbstractF2mCurve abstractF2mCurve = (AbstractF2mCurve)p.Curve;
			sbyte a = (sbyte)abstractF2mCurve.A.ToBigInteger().IntValue;
			AbstractF2mPoint[] preComp;
			if (preCompInfo == null || !(preCompInfo is WTauNafPreCompInfo))
			{
				preComp = Tnaf.GetPreComp(p, a);
				WTauNafPreCompInfo wtauNafPreCompInfo = new WTauNafPreCompInfo();
				wtauNafPreCompInfo.PreComp = preComp;
				abstractF2mCurve.SetPreCompInfo(p, WTauNafMultiplier.PRECOMP_NAME, wtauNafPreCompInfo);
			}
			else
			{
				preComp = ((WTauNafPreCompInfo)preCompInfo).PreComp;
			}
			AbstractF2mPoint[] array = new AbstractF2mPoint[preComp.Length];
			for (int i = 0; i < preComp.Length; i++)
			{
				array[i] = (AbstractF2mPoint)preComp[i].Negate();
			}
			AbstractF2mPoint abstractF2mPoint = (AbstractF2mPoint)p.Curve.Infinity;
			int num = 0;
			for (int j = u.Length - 1; j >= 0; j--)
			{
				num++;
				int num2 = (int)u[j];
				if (num2 != 0)
				{
					abstractF2mPoint = abstractF2mPoint.TauPow(num);
					num = 0;
					ECPoint b = (num2 > 0) ? preComp[num2 >> 1] : array[-num2 >> 1];
					abstractF2mPoint = (AbstractF2mPoint)abstractF2mPoint.Add(b);
				}
			}
			if (num > 0)
			{
				abstractF2mPoint = abstractF2mPoint.TauPow(num);
			}
			return abstractF2mPoint;
		}

		// Token: 0x0400159C RID: 5532
		internal static readonly string PRECOMP_NAME = "bc_wtnaf";
	}
}
