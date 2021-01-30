using System;

namespace Org.BouncyCastle.Math.EC.Multiplier
{
	// Token: 0x020002F5 RID: 757
	public abstract class WNafUtilities
	{
		// Token: 0x06001A95 RID: 6805 RVA: 0x00099CC8 File Offset: 0x00097EC8
		public static int[] GenerateCompactNaf(BigInteger k)
		{
			if (k.BitLength >> 16 != 0)
			{
				throw new ArgumentException("must have bitlength < 2^16", "k");
			}
			if (k.SignValue == 0)
			{
				return WNafUtilities.EMPTY_INTS;
			}
			BigInteger bigInteger = k.ShiftLeft(1).Add(k);
			int bitLength = bigInteger.BitLength;
			int[] array = new int[bitLength >> 1];
			BigInteger bigInteger2 = bigInteger.Xor(k);
			int num = bitLength - 1;
			int num2 = 0;
			int num3 = 0;
			for (int i = 1; i < num; i++)
			{
				if (!bigInteger2.TestBit(i))
				{
					num3++;
				}
				else
				{
					int num4 = k.TestBit(i) ? -1 : 1;
					array[num2++] = (num4 << 16 | num3);
					num3 = 1;
					i++;
				}
			}
			array[num2++] = (65536 | num3);
			if (array.Length > num2)
			{
				array = WNafUtilities.Trim(array, num2);
			}
			return array;
		}

		// Token: 0x06001A96 RID: 6806 RVA: 0x00099D9C File Offset: 0x00097F9C
		public static int[] GenerateCompactWindowNaf(int width, BigInteger k)
		{
			if (width == 2)
			{
				return WNafUtilities.GenerateCompactNaf(k);
			}
			if (width < 2 || width > 16)
			{
				throw new ArgumentException("must be in the range [2, 16]", "width");
			}
			if (k.BitLength >> 16 != 0)
			{
				throw new ArgumentException("must have bitlength < 2^16", "k");
			}
			if (k.SignValue == 0)
			{
				return WNafUtilities.EMPTY_INTS;
			}
			int[] array = new int[k.BitLength / width + 1];
			int num = 1 << width;
			int num2 = num - 1;
			int num3 = num >> 1;
			bool flag = false;
			int num4 = 0;
			int i = 0;
			while (i <= k.BitLength)
			{
				if (k.TestBit(i) == flag)
				{
					i++;
				}
				else
				{
					k = k.ShiftRight(i);
					int num5 = k.IntValue & num2;
					if (flag)
					{
						num5++;
					}
					flag = ((num5 & num3) != 0);
					if (flag)
					{
						num5 -= num;
					}
					int num6 = (num4 > 0) ? (i - 1) : i;
					array[num4++] = (num5 << 16 | num6);
					i = width;
				}
			}
			if (array.Length > num4)
			{
				array = WNafUtilities.Trim(array, num4);
			}
			return array;
		}

		// Token: 0x06001A97 RID: 6807 RVA: 0x00099EA4 File Offset: 0x000980A4
		public static byte[] GenerateJsf(BigInteger g, BigInteger h)
		{
			byte[] array = new byte[Math.Max(g.BitLength, h.BitLength) + 1];
			BigInteger bigInteger = g;
			BigInteger bigInteger2 = h;
			int num = 0;
			int num2 = 0;
			int num3 = 0;
			int num4 = 0;
			while ((num2 | num3) != 0 || bigInteger.BitLength > num4 || bigInteger2.BitLength > num4)
			{
				int num5 = (int)(((uint)bigInteger.IntValue >> num4) + (uint)num2 & 7U);
				int num6 = (int)(((uint)bigInteger2.IntValue >> num4) + (uint)num3 & 7U);
				int num7 = num5 & 1;
				if (num7 != 0)
				{
					num7 -= (num5 & 2);
					if (num5 + num7 == 4 && (num6 & 3) == 2)
					{
						num7 = -num7;
					}
				}
				int num8 = num6 & 1;
				if (num8 != 0)
				{
					num8 -= (num6 & 2);
					if (num6 + num8 == 4 && (num5 & 3) == 2)
					{
						num8 = -num8;
					}
				}
				if (num2 << 1 == 1 + num7)
				{
					num2 ^= 1;
				}
				if (num3 << 1 == 1 + num8)
				{
					num3 ^= 1;
				}
				if (++num4 == 30)
				{
					num4 = 0;
					bigInteger = bigInteger.ShiftRight(30);
					bigInteger2 = bigInteger2.ShiftRight(30);
				}
				array[num++] = (byte)(num7 << 4 | (num8 & 15));
			}
			if (array.Length > num)
			{
				array = WNafUtilities.Trim(array, num);
			}
			return array;
		}

		// Token: 0x06001A98 RID: 6808 RVA: 0x00099FD8 File Offset: 0x000981D8
		public static byte[] GenerateNaf(BigInteger k)
		{
			if (k.SignValue == 0)
			{
				return WNafUtilities.EMPTY_BYTES;
			}
			BigInteger bigInteger = k.ShiftLeft(1).Add(k);
			int num = bigInteger.BitLength - 1;
			byte[] array = new byte[num];
			BigInteger bigInteger2 = bigInteger.Xor(k);
			for (int i = 1; i < num; i++)
			{
				if (bigInteger2.TestBit(i))
				{
					array[i - 1] = (byte)(k.TestBit(i) ? -1 : 1);
					i++;
				}
			}
			array[num - 1] = 1;
			return array;
		}

		// Token: 0x06001A99 RID: 6809 RVA: 0x0009A04C File Offset: 0x0009824C
		public static byte[] GenerateWindowNaf(int width, BigInteger k)
		{
			if (width == 2)
			{
				return WNafUtilities.GenerateNaf(k);
			}
			if (width < 2 || width > 8)
			{
				throw new ArgumentException("must be in the range [2, 8]", "width");
			}
			if (k.SignValue == 0)
			{
				return WNafUtilities.EMPTY_BYTES;
			}
			byte[] array = new byte[k.BitLength + 1];
			int num = 1 << width;
			int num2 = num - 1;
			int num3 = num >> 1;
			bool flag = false;
			int num4 = 0;
			int i = 0;
			while (i <= k.BitLength)
			{
				if (k.TestBit(i) == flag)
				{
					i++;
				}
				else
				{
					k = k.ShiftRight(i);
					int num5 = k.IntValue & num2;
					if (flag)
					{
						num5++;
					}
					flag = ((num5 & num3) != 0);
					if (flag)
					{
						num5 -= num;
					}
					num4 += ((num4 > 0) ? (i - 1) : i);
					array[num4++] = (byte)num5;
					i = width;
				}
			}
			if (array.Length > num4)
			{
				array = WNafUtilities.Trim(array, num4);
			}
			return array;
		}

		// Token: 0x06001A9A RID: 6810 RVA: 0x0009A133 File Offset: 0x00098333
		public static int GetNafWeight(BigInteger k)
		{
			if (k.SignValue == 0)
			{
				return 0;
			}
			return k.ShiftLeft(1).Add(k).Xor(k).BitCount;
		}

		// Token: 0x06001A9B RID: 6811 RVA: 0x0009A157 File Offset: 0x00098357
		public static WNafPreCompInfo GetWNafPreCompInfo(ECPoint p)
		{
			return WNafUtilities.GetWNafPreCompInfo(p.Curve.GetPreCompInfo(p, WNafUtilities.PRECOMP_NAME));
		}

		// Token: 0x06001A9C RID: 6812 RVA: 0x0009A16F File Offset: 0x0009836F
		public static WNafPreCompInfo GetWNafPreCompInfo(PreCompInfo preCompInfo)
		{
			if (preCompInfo != null && preCompInfo is WNafPreCompInfo)
			{
				return (WNafPreCompInfo)preCompInfo;
			}
			return new WNafPreCompInfo();
		}

		// Token: 0x06001A9D RID: 6813 RVA: 0x0009A188 File Offset: 0x00098388
		public static int GetWindowSize(int bits)
		{
			return WNafUtilities.GetWindowSize(bits, WNafUtilities.DEFAULT_WINDOW_SIZE_CUTOFFS);
		}

		// Token: 0x06001A9E RID: 6814 RVA: 0x0009A198 File Offset: 0x00098398
		public static int GetWindowSize(int bits, int[] windowSizeCutoffs)
		{
			int num = 0;
			while (num < windowSizeCutoffs.Length && bits >= windowSizeCutoffs[num])
			{
				num++;
			}
			return num + 2;
		}

		// Token: 0x06001A9F RID: 6815 RVA: 0x0009A1BC File Offset: 0x000983BC
		public static ECPoint MapPointWithPrecomp(ECPoint p, int width, bool includeNegated, ECPointMap pointMap)
		{
			ECCurve curve = p.Curve;
			WNafPreCompInfo wnafPreCompInfo = WNafUtilities.Precompute(p, width, includeNegated);
			ECPoint ecpoint = pointMap.Map(p);
			WNafPreCompInfo wnafPreCompInfo2 = WNafUtilities.GetWNafPreCompInfo(curve.GetPreCompInfo(ecpoint, WNafUtilities.PRECOMP_NAME));
			ECPoint twice = wnafPreCompInfo.Twice;
			if (twice != null)
			{
				ECPoint twice2 = pointMap.Map(twice);
				wnafPreCompInfo2.Twice = twice2;
			}
			ECPoint[] preComp = wnafPreCompInfo.PreComp;
			ECPoint[] array = new ECPoint[preComp.Length];
			for (int i = 0; i < preComp.Length; i++)
			{
				array[i] = pointMap.Map(preComp[i]);
			}
			wnafPreCompInfo2.PreComp = array;
			if (includeNegated)
			{
				ECPoint[] array2 = new ECPoint[array.Length];
				for (int j = 0; j < array2.Length; j++)
				{
					array2[j] = array[j].Negate();
				}
				wnafPreCompInfo2.PreCompNeg = array2;
			}
			curve.SetPreCompInfo(ecpoint, WNafUtilities.PRECOMP_NAME, wnafPreCompInfo2);
			return ecpoint;
		}

		// Token: 0x06001AA0 RID: 6816 RVA: 0x0009A298 File Offset: 0x00098498
		public static WNafPreCompInfo Precompute(ECPoint p, int width, bool includeNegated)
		{
			ECCurve curve = p.Curve;
			WNafPreCompInfo wnafPreCompInfo = WNafUtilities.GetWNafPreCompInfo(curve.GetPreCompInfo(p, WNafUtilities.PRECOMP_NAME));
			int num = 0;
			int num2 = 1 << Math.Max(0, width - 2);
			ECPoint[] array = wnafPreCompInfo.PreComp;
			if (array == null)
			{
				array = WNafUtilities.EMPTY_POINTS;
			}
			else
			{
				num = array.Length;
			}
			if (num < num2)
			{
				array = WNafUtilities.ResizeTable(array, num2);
				if (num2 == 1)
				{
					array[0] = p.Normalize();
				}
				else
				{
					int i = num;
					if (i == 0)
					{
						array[0] = p;
						i = 1;
					}
					ECFieldElement ecfieldElement = null;
					if (num2 == 2)
					{
						array[1] = p.ThreeTimes();
					}
					else
					{
						ECPoint ecpoint = wnafPreCompInfo.Twice;
						ECPoint ecpoint2 = array[i - 1];
						if (ecpoint == null)
						{
							ecpoint = array[0].Twice();
							wnafPreCompInfo.Twice = ecpoint;
							if (ECAlgorithms.IsFpCurve(curve) && curve.FieldSize >= 64)
							{
								int coordinateSystem = curve.CoordinateSystem;
								if (coordinateSystem - 2 <= 2)
								{
									ecfieldElement = ecpoint.GetZCoord(0);
									ecpoint = curve.CreatePoint(ecpoint.XCoord.ToBigInteger(), ecpoint.YCoord.ToBigInteger());
									ECFieldElement ecfieldElement2 = ecfieldElement.Square();
									ECFieldElement scale = ecfieldElement2.Multiply(ecfieldElement);
									ecpoint2 = ecpoint2.ScaleX(ecfieldElement2).ScaleY(scale);
									if (num == 0)
									{
										array[0] = ecpoint2;
									}
								}
							}
						}
						while (i < num2)
						{
							ecpoint2 = (array[i++] = ecpoint2.Add(ecpoint));
						}
					}
					curve.NormalizeAll(array, num, num2 - num, ecfieldElement);
				}
			}
			wnafPreCompInfo.PreComp = array;
			if (includeNegated)
			{
				ECPoint[] array2 = wnafPreCompInfo.PreCompNeg;
				int j;
				if (array2 == null)
				{
					j = 0;
					array2 = new ECPoint[num2];
				}
				else
				{
					j = array2.Length;
					if (j < num2)
					{
						array2 = WNafUtilities.ResizeTable(array2, num2);
					}
				}
				while (j < num2)
				{
					array2[j] = array[j].Negate();
					j++;
				}
				wnafPreCompInfo.PreCompNeg = array2;
			}
			curve.SetPreCompInfo(p, WNafUtilities.PRECOMP_NAME, wnafPreCompInfo);
			return wnafPreCompInfo;
		}

		// Token: 0x06001AA1 RID: 6817 RVA: 0x0009A474 File Offset: 0x00098674
		private static byte[] Trim(byte[] a, int length)
		{
			byte[] array = new byte[length];
			Array.Copy(a, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06001AA2 RID: 6818 RVA: 0x0009A498 File Offset: 0x00098698
		private static int[] Trim(int[] a, int length)
		{
			int[] array = new int[length];
			Array.Copy(a, 0, array, 0, array.Length);
			return array;
		}

		// Token: 0x06001AA3 RID: 6819 RVA: 0x0009A4BC File Offset: 0x000986BC
		private static ECPoint[] ResizeTable(ECPoint[] a, int length)
		{
			ECPoint[] array = new ECPoint[length];
			Array.Copy(a, 0, array, 0, a.Length);
			return array;
		}

		// Token: 0x04001597 RID: 5527
		public static readonly string PRECOMP_NAME = "bc_wnaf";

		// Token: 0x04001598 RID: 5528
		private static readonly int[] DEFAULT_WINDOW_SIZE_CUTOFFS = new int[]
		{
			13,
			41,
			121,
			337,
			897,
			2305
		};

		// Token: 0x04001599 RID: 5529
		private static readonly byte[] EMPTY_BYTES = new byte[0];

		// Token: 0x0400159A RID: 5530
		private static readonly int[] EMPTY_INTS = new int[0];

		// Token: 0x0400159B RID: 5531
		private static readonly ECPoint[] EMPTY_POINTS = new ECPoint[0];
	}
}
