﻿using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F0 RID: 752
	public class FixedPointUtilities
	{
		// Token: 0x06001A84 RID: 6788 RVA: 0x00099944 File Offset: 0x00097B44
		public static int GetCombSize(ECCurve c)
		{
			BigInteger order = c.Order;
			if (order != null)
			{
				return order.BitLength;
			}
			return c.FieldSize + 1;
		}

		// Token: 0x06001A85 RID: 6789 RVA: 0x0009996A File Offset: 0x00097B6A
		public static FixedPointPreCompInfo GetFixedPointPreCompInfo(PreCompInfo preCompInfo)
		{
			if (preCompInfo != null && preCompInfo is FixedPointPreCompInfo)
			{
				return (FixedPointPreCompInfo)preCompInfo;
			}
			return new FixedPointPreCompInfo();
		}

		// Token: 0x06001A86 RID: 6790 RVA: 0x00099984 File Offset: 0x00097B84
		public static FixedPointPreCompInfo Precompute(ECPoint p, int minWidth)
		{
			ECCurve curve = p.Curve;
			int num = 1 << minWidth;
			FixedPointPreCompInfo fixedPointPreCompInfo = FixedPointUtilities.GetFixedPointPreCompInfo(curve.GetPreCompInfo(p, FixedPointUtilities.PRECOMP_NAME));
			ECPoint[] array = fixedPointPreCompInfo.PreComp;
			if (array == null || array.Length < num)
			{
				int e = (FixedPointUtilities.GetCombSize(curve) + minWidth - 1) / minWidth;
				ECPoint[] array2 = new ECPoint[minWidth];
				array2[0] = p;
				for (int i = 1; i < minWidth; i++)
				{
					array2[i] = array2[i - 1].TimesPow2(e);
				}
				curve.NormalizeAll(array2);
				array = new ECPoint[num];
				array[0] = curve.Infinity;
				for (int j = minWidth - 1; j >= 0; j--)
				{
					ECPoint b = array2[j];
					int num2 = 1 << j;
					for (int k = num2; k < num; k += num2 << 1)
					{
						array[k] = array[k - num2].Add(b);
					}
				}
				curve.NormalizeAll(array);
				fixedPointPreCompInfo.PreComp = array;
				fixedPointPreCompInfo.Width = minWidth;
				curve.SetPreCompInfo(p, FixedPointUtilities.PRECOMP_NAME, fixedPointPreCompInfo);
			}
			return fixedPointPreCompInfo;
		}

		// Token: 0x04001591 RID: 5521
		public static readonly string PRECOMP_NAME = "bc_fixed_point";
	}
}
