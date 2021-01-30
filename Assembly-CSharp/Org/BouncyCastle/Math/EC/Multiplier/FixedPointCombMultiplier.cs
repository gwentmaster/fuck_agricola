﻿using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002EE RID: 750
	public class FixedPointCombMultiplier : AbstractECMultiplier
	{
		// Token: 0x06001A7C RID: 6780 RVA: 0x0009984C File Offset: 0x00097A4C
		protected override ECPoint MultiplyPositive(ECPoint p, BigInteger k)
		{
			ECCurve curve = p.Curve;
			int combSize = FixedPointUtilities.GetCombSize(curve);
			if (k.BitLength > combSize)
			{
				throw new InvalidOperationException("fixed-point comb doesn't support scalars larger than the curve order");
			}
			int widthForCombSize = this.GetWidthForCombSize(combSize);
			FixedPointPreCompInfo fixedPointPreCompInfo = FixedPointUtilities.Precompute(p, widthForCombSize);
			ECPoint[] preComp = fixedPointPreCompInfo.PreComp;
			int width = fixedPointPreCompInfo.Width;
			int num = (combSize + width - 1) / width;
			ECPoint ecpoint = curve.Infinity;
			int num2 = num * width - 1;
			for (int i = 0; i < num; i++)
			{
				int num3 = 0;
				for (int j = num2 - i; j >= 0; j -= num)
				{
					num3 <<= 1;
					if (k.TestBit(j))
					{
						num3 |= 1;
					}
				}
				ecpoint = ecpoint.TwicePlus(preComp[num3]);
			}
			return ecpoint;
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x000998FC File Offset: 0x00097AFC
		protected virtual int GetWidthForCombSize(int combSize)
		{
			if (combSize <= 257)
			{
				return 5;
			}
			return 6;
		}
	}
}
