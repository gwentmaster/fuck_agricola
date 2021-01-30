using System;

namespace Org.BouncyCastle.Math.EC.Abc
{
	// Token: 0x02000365 RID: 869
	internal class Tnaf
	{
		// Token: 0x06002150 RID: 8528 RVA: 0x000B388C File Offset: 0x000B1A8C
		public static BigInteger Norm(sbyte mu, ZTauElement lambda)
		{
			BigInteger bigInteger = lambda.u.Multiply(lambda.u);
			BigInteger bigInteger2 = lambda.u.Multiply(lambda.v);
			BigInteger value = lambda.v.Multiply(lambda.v).ShiftLeft(1);
			BigInteger result;
			if (mu == 1)
			{
				result = bigInteger.Add(bigInteger2).Add(value);
			}
			else
			{
				if (mu != -1)
				{
					throw new ArgumentException("mu must be 1 or -1");
				}
				result = bigInteger.Subtract(bigInteger2).Add(value);
			}
			return result;
		}

		// Token: 0x06002151 RID: 8529 RVA: 0x000B390C File Offset: 0x000B1B0C
		public static SimpleBigDecimal Norm(sbyte mu, SimpleBigDecimal u, SimpleBigDecimal v)
		{
			SimpleBigDecimal simpleBigDecimal = u.Multiply(u);
			SimpleBigDecimal b = u.Multiply(v);
			SimpleBigDecimal b2 = v.Multiply(v).ShiftLeft(1);
			SimpleBigDecimal result;
			if (mu == 1)
			{
				result = simpleBigDecimal.Add(b).Add(b2);
			}
			else
			{
				if (mu != -1)
				{
					throw new ArgumentException("mu must be 1 or -1");
				}
				result = simpleBigDecimal.Subtract(b).Add(b2);
			}
			return result;
		}

		// Token: 0x06002152 RID: 8530 RVA: 0x000B396C File Offset: 0x000B1B6C
		public static ZTauElement Round(SimpleBigDecimal lambda0, SimpleBigDecimal lambda1, sbyte mu)
		{
			int scale = lambda0.Scale;
			if (lambda1.Scale != scale)
			{
				throw new ArgumentException("lambda0 and lambda1 do not have same scale");
			}
			if (mu != 1 && mu != -1)
			{
				throw new ArgumentException("mu must be 1 or -1");
			}
			BigInteger bigInteger = lambda0.Round();
			BigInteger bigInteger2 = lambda1.Round();
			SimpleBigDecimal simpleBigDecimal = lambda0.Subtract(bigInteger);
			SimpleBigDecimal simpleBigDecimal2 = lambda1.Subtract(bigInteger2);
			SimpleBigDecimal simpleBigDecimal3 = simpleBigDecimal.Add(simpleBigDecimal);
			if (mu == 1)
			{
				simpleBigDecimal3 = simpleBigDecimal3.Add(simpleBigDecimal2);
			}
			else
			{
				simpleBigDecimal3 = simpleBigDecimal3.Subtract(simpleBigDecimal2);
			}
			SimpleBigDecimal simpleBigDecimal4 = simpleBigDecimal2.Add(simpleBigDecimal2).Add(simpleBigDecimal2);
			SimpleBigDecimal b = simpleBigDecimal4.Add(simpleBigDecimal2);
			SimpleBigDecimal simpleBigDecimal5;
			SimpleBigDecimal simpleBigDecimal6;
			if (mu == 1)
			{
				simpleBigDecimal5 = simpleBigDecimal.Subtract(simpleBigDecimal4);
				simpleBigDecimal6 = simpleBigDecimal.Add(b);
			}
			else
			{
				simpleBigDecimal5 = simpleBigDecimal.Add(simpleBigDecimal4);
				simpleBigDecimal6 = simpleBigDecimal.Subtract(b);
			}
			sbyte b2 = 0;
			sbyte b3 = 0;
			if (simpleBigDecimal3.CompareTo(BigInteger.One) >= 0)
			{
				if (simpleBigDecimal5.CompareTo(Tnaf.MinusOne) < 0)
				{
					b3 = mu;
				}
				else
				{
					b2 = 1;
				}
			}
			else if (simpleBigDecimal6.CompareTo(BigInteger.Two) >= 0)
			{
				b3 = mu;
			}
			if (simpleBigDecimal3.CompareTo(Tnaf.MinusOne) < 0)
			{
				if (simpleBigDecimal5.CompareTo(BigInteger.One) >= 0)
				{
					b3 = -mu;
				}
				else
				{
					b2 = -1;
				}
			}
			else if (simpleBigDecimal6.CompareTo(Tnaf.MinusTwo) < 0)
			{
				b3 = -mu;
			}
			BigInteger u = bigInteger.Add(BigInteger.ValueOf((long)b2));
			BigInteger v = bigInteger2.Add(BigInteger.ValueOf((long)b3));
			return new ZTauElement(u, v);
		}

		// Token: 0x06002153 RID: 8531 RVA: 0x000B3ADC File Offset: 0x000B1CDC
		public static SimpleBigDecimal ApproximateDivisionByN(BigInteger k, BigInteger s, BigInteger vm, sbyte a, int m, int c)
		{
			int num = (m + 5) / 2 + c;
			BigInteger val = k.ShiftRight(m - num - 2 + (int)a);
			BigInteger bigInteger = s.Multiply(val);
			BigInteger val2 = bigInteger.ShiftRight(m);
			BigInteger value = vm.Multiply(val2);
			BigInteger bigInteger2 = bigInteger.Add(value);
			BigInteger bigInteger3 = bigInteger2.ShiftRight(num - c);
			if (bigInteger2.TestBit(num - c - 1))
			{
				bigInteger3 = bigInteger3.Add(BigInteger.One);
			}
			return new SimpleBigDecimal(bigInteger3, c);
		}

		// Token: 0x06002154 RID: 8532 RVA: 0x000B3B50 File Offset: 0x000B1D50
		public static sbyte[] TauAdicNaf(sbyte mu, ZTauElement lambda)
		{
			if (mu != 1 && mu != -1)
			{
				throw new ArgumentException("mu must be 1 or -1");
			}
			int bitLength = Tnaf.Norm(mu, lambda).BitLength;
			sbyte[] array = new sbyte[(bitLength > 30) ? (bitLength + 4) : 34];
			int num = 0;
			int num2 = 0;
			BigInteger bigInteger = lambda.u;
			BigInteger bigInteger2 = lambda.v;
			while (!bigInteger.Equals(BigInteger.Zero) || !bigInteger2.Equals(BigInteger.Zero))
			{
				if (bigInteger.TestBit(0))
				{
					array[num] = (sbyte)BigInteger.Two.Subtract(bigInteger.Subtract(bigInteger2.ShiftLeft(1)).Mod(Tnaf.Four)).IntValue;
					if (array[num] == 1)
					{
						bigInteger = bigInteger.ClearBit(0);
					}
					else
					{
						bigInteger = bigInteger.Add(BigInteger.One);
					}
					num2 = num;
				}
				else
				{
					array[num] = 0;
				}
				BigInteger bigInteger3 = bigInteger;
				BigInteger bigInteger4 = bigInteger.ShiftRight(1);
				if (mu == 1)
				{
					bigInteger = bigInteger2.Add(bigInteger4);
				}
				else
				{
					bigInteger = bigInteger2.Subtract(bigInteger4);
				}
				bigInteger2 = bigInteger3.ShiftRight(1).Negate();
				num++;
			}
			num2++;
			sbyte[] array2 = new sbyte[num2];
			Array.Copy(array, 0, array2, 0, num2);
			return array2;
		}

		// Token: 0x06002155 RID: 8533 RVA: 0x000B3C7A File Offset: 0x000B1E7A
		public static AbstractF2mPoint Tau(AbstractF2mPoint p)
		{
			return p.Tau();
		}

		// Token: 0x06002156 RID: 8534 RVA: 0x000B3C84 File Offset: 0x000B1E84
		public static sbyte GetMu(AbstractF2mCurve curve)
		{
			BigInteger bigInteger = curve.A.ToBigInteger();
			sbyte result;
			if (bigInteger.SignValue == 0)
			{
				result = -1;
			}
			else
			{
				if (!bigInteger.Equals(BigInteger.One))
				{
					throw new ArgumentException("No Koblitz curve (ABC), TNAF multiplication not possible");
				}
				result = 1;
			}
			return result;
		}

		// Token: 0x06002157 RID: 8535 RVA: 0x000B3CC6 File Offset: 0x000B1EC6
		public static sbyte GetMu(ECFieldElement curveA)
		{
			return curveA.IsZero ? -1 : 1;
		}

		// Token: 0x06002158 RID: 8536 RVA: 0x000B3CD5 File Offset: 0x000B1ED5
		public static sbyte GetMu(int curveA)
		{
			return (curveA == 0) ? -1 : 1;
		}

		// Token: 0x06002159 RID: 8537 RVA: 0x000B3CE0 File Offset: 0x000B1EE0
		public static BigInteger[] GetLucas(sbyte mu, int k, bool doV)
		{
			if (mu != 1 && mu != -1)
			{
				throw new ArgumentException("mu must be 1 or -1");
			}
			BigInteger bigInteger;
			BigInteger bigInteger2;
			if (doV)
			{
				bigInteger = BigInteger.Two;
				bigInteger2 = BigInteger.ValueOf((long)mu);
			}
			else
			{
				bigInteger = BigInteger.Zero;
				bigInteger2 = BigInteger.One;
			}
			for (int i = 1; i < k; i++)
			{
				BigInteger bigInteger3;
				if (mu == 1)
				{
					bigInteger3 = bigInteger2;
				}
				else
				{
					bigInteger3 = bigInteger2.Negate();
				}
				BigInteger bigInteger4 = bigInteger3.Subtract(bigInteger.ShiftLeft(1));
				bigInteger = bigInteger2;
				bigInteger2 = bigInteger4;
			}
			return new BigInteger[]
			{
				bigInteger,
				bigInteger2
			};
		}

		// Token: 0x0600215A RID: 8538 RVA: 0x000B3D5C File Offset: 0x000B1F5C
		public static BigInteger GetTw(sbyte mu, int w)
		{
			if (w != 4)
			{
				BigInteger[] lucas = Tnaf.GetLucas(mu, w, false);
				BigInteger m = BigInteger.Zero.SetBit(w);
				BigInteger val = lucas[1].ModInverse(m);
				return BigInteger.Two.Multiply(lucas[0]).Multiply(val).Mod(m);
			}
			if (mu == 1)
			{
				return BigInteger.ValueOf(6L);
			}
			return BigInteger.ValueOf(10L);
		}

		// Token: 0x0600215B RID: 8539 RVA: 0x000B3DBC File Offset: 0x000B1FBC
		public static BigInteger[] GetSi(AbstractF2mCurve curve)
		{
			if (!curve.IsKoblitz)
			{
				throw new ArgumentException("si is defined for Koblitz curves only");
			}
			int fieldSize = curve.FieldSize;
			int intValue = curve.A.ToBigInteger().IntValue;
			sbyte mu = Tnaf.GetMu(intValue);
			int shiftsForCofactor = Tnaf.GetShiftsForCofactor(curve.Cofactor);
			int k = fieldSize + 3 - intValue;
			BigInteger[] lucas = Tnaf.GetLucas(mu, k, false);
			if (mu == 1)
			{
				lucas[0] = lucas[0].Negate();
				lucas[1] = lucas[1].Negate();
			}
			BigInteger bigInteger = BigInteger.One.Add(lucas[1]).ShiftRight(shiftsForCofactor);
			BigInteger bigInteger2 = BigInteger.One.Add(lucas[0]).ShiftRight(shiftsForCofactor).Negate();
			return new BigInteger[]
			{
				bigInteger,
				bigInteger2
			};
		}

		// Token: 0x0600215C RID: 8540 RVA: 0x000B3E74 File Offset: 0x000B2074
		public static BigInteger[] GetSi(int fieldSize, int curveA, BigInteger cofactor)
		{
			sbyte mu = Tnaf.GetMu(curveA);
			int shiftsForCofactor = Tnaf.GetShiftsForCofactor(cofactor);
			int k = fieldSize + 3 - curveA;
			BigInteger[] lucas = Tnaf.GetLucas(mu, k, false);
			if (mu == 1)
			{
				lucas[0] = lucas[0].Negate();
				lucas[1] = lucas[1].Negate();
			}
			BigInteger bigInteger = BigInteger.One.Add(lucas[1]).ShiftRight(shiftsForCofactor);
			BigInteger bigInteger2 = BigInteger.One.Add(lucas[0]).ShiftRight(shiftsForCofactor).Negate();
			return new BigInteger[]
			{
				bigInteger,
				bigInteger2
			};
		}

		// Token: 0x0600215D RID: 8541 RVA: 0x000B3EF4 File Offset: 0x000B20F4
		protected static int GetShiftsForCofactor(BigInteger h)
		{
			if (h != null && h.BitLength < 4)
			{
				int intValue = h.IntValue;
				if (intValue == 2)
				{
					return 1;
				}
				if (intValue == 4)
				{
					return 2;
				}
			}
			throw new ArgumentException("h (Cofactor) must be 2 or 4");
		}

		// Token: 0x0600215E RID: 8542 RVA: 0x000B3F2C File Offset: 0x000B212C
		public static ZTauElement PartModReduction(BigInteger k, int m, sbyte a, BigInteger[] s, sbyte mu, sbyte c)
		{
			BigInteger bigInteger;
			if (mu == 1)
			{
				bigInteger = s[0].Add(s[1]);
			}
			else
			{
				bigInteger = s[0].Subtract(s[1]);
			}
			BigInteger vm = Tnaf.GetLucas(mu, m, true)[1];
			SimpleBigDecimal lambda = Tnaf.ApproximateDivisionByN(k, s[0], vm, a, m, (int)c);
			SimpleBigDecimal lambda2 = Tnaf.ApproximateDivisionByN(k, s[1], vm, a, m, (int)c);
			ZTauElement ztauElement = Tnaf.Round(lambda, lambda2, mu);
			BigInteger u = k.Subtract(bigInteger.Multiply(ztauElement.u)).Subtract(BigInteger.ValueOf(2L).Multiply(s[1]).Multiply(ztauElement.v));
			BigInteger v = s[1].Multiply(ztauElement.u).Subtract(s[0].Multiply(ztauElement.v));
			return new ZTauElement(u, v);
		}

		// Token: 0x0600215F RID: 8543 RVA: 0x000B3FE8 File Offset: 0x000B21E8
		public static AbstractF2mPoint MultiplyRTnaf(AbstractF2mPoint p, BigInteger k)
		{
			AbstractF2mCurve abstractF2mCurve = (AbstractF2mCurve)p.Curve;
			int fieldSize = abstractF2mCurve.FieldSize;
			int intValue = abstractF2mCurve.A.ToBigInteger().IntValue;
			sbyte mu = Tnaf.GetMu(intValue);
			BigInteger[] si = abstractF2mCurve.GetSi();
			ZTauElement lambda = Tnaf.PartModReduction(k, fieldSize, (sbyte)intValue, si, mu, 10);
			return Tnaf.MultiplyTnaf(p, lambda);
		}

		// Token: 0x06002160 RID: 8544 RVA: 0x000B403C File Offset: 0x000B223C
		public static AbstractF2mPoint MultiplyTnaf(AbstractF2mPoint p, ZTauElement lambda)
		{
			sbyte[] u = Tnaf.TauAdicNaf(Tnaf.GetMu(((AbstractF2mCurve)p.Curve).A), lambda);
			return Tnaf.MultiplyFromTnaf(p, u);
		}

		// Token: 0x06002161 RID: 8545 RVA: 0x000B406C File Offset: 0x000B226C
		public static AbstractF2mPoint MultiplyFromTnaf(AbstractF2mPoint p, sbyte[] u)
		{
			AbstractF2mPoint abstractF2mPoint = (AbstractF2mPoint)p.Curve.Infinity;
			AbstractF2mPoint abstractF2mPoint2 = (AbstractF2mPoint)p.Negate();
			int num = 0;
			for (int i = u.Length - 1; i >= 0; i--)
			{
				num++;
				sbyte b = u[i];
				if (b != 0)
				{
					abstractF2mPoint = abstractF2mPoint.TauPow(num);
					num = 0;
					ECPoint b2 = (b > 0) ? p : abstractF2mPoint2;
					abstractF2mPoint = (AbstractF2mPoint)abstractF2mPoint.Add(b2);
				}
			}
			if (num > 0)
			{
				abstractF2mPoint = abstractF2mPoint.TauPow(num);
			}
			return abstractF2mPoint;
		}

		// Token: 0x06002162 RID: 8546 RVA: 0x000B40E8 File Offset: 0x000B22E8
		public static sbyte[] TauAdicWNaf(sbyte mu, ZTauElement lambda, sbyte width, BigInteger pow2w, BigInteger tw, ZTauElement[] alpha)
		{
			if (mu != 1 && mu != -1)
			{
				throw new ArgumentException("mu must be 1 or -1");
			}
			int bitLength = Tnaf.Norm(mu, lambda).BitLength;
			sbyte[] array = new sbyte[(bitLength > 30) ? (bitLength + 4 + (int)width) : ((int)(34 + width))];
			BigInteger value = pow2w.ShiftRight(1);
			BigInteger bigInteger = lambda.u;
			BigInteger bigInteger2 = lambda.v;
			int num = 0;
			while (!bigInteger.Equals(BigInteger.Zero) || !bigInteger2.Equals(BigInteger.Zero))
			{
				if (bigInteger.TestBit(0))
				{
					BigInteger bigInteger3 = bigInteger.Add(bigInteger2.Multiply(tw)).Mod(pow2w);
					sbyte b;
					if (bigInteger3.CompareTo(value) >= 0)
					{
						b = (sbyte)bigInteger3.Subtract(pow2w).IntValue;
					}
					else
					{
						b = (sbyte)bigInteger3.IntValue;
					}
					array[num] = b;
					bool flag = true;
					if (b < 0)
					{
						flag = false;
						b = -b;
					}
					if (flag)
					{
						bigInteger = bigInteger.Subtract(alpha[(int)b].u);
						bigInteger2 = bigInteger2.Subtract(alpha[(int)b].v);
					}
					else
					{
						bigInteger = bigInteger.Add(alpha[(int)b].u);
						bigInteger2 = bigInteger2.Add(alpha[(int)b].v);
					}
				}
				else
				{
					array[num] = 0;
				}
				BigInteger bigInteger4 = bigInteger;
				if (mu == 1)
				{
					bigInteger = bigInteger2.Add(bigInteger.ShiftRight(1));
				}
				else
				{
					bigInteger = bigInteger2.Subtract(bigInteger.ShiftRight(1));
				}
				bigInteger2 = bigInteger4.ShiftRight(1).Negate();
				num++;
			}
			return array;
		}

		// Token: 0x06002163 RID: 8547 RVA: 0x000B425C File Offset: 0x000B245C
		public static AbstractF2mPoint[] GetPreComp(AbstractF2mPoint p, sbyte a)
		{
			sbyte[][] array = (a == 0) ? Tnaf.Alpha0Tnaf : Tnaf.Alpha1Tnaf;
			AbstractF2mPoint[] array2 = new AbstractF2mPoint[(uint)(array.Length + 1) >> 1];
			array2[0] = p;
			uint num = (uint)array.Length;
			for (uint num2 = 3U; num2 < num; num2 += 2U)
			{
				array2[(int)(num2 >> 1)] = Tnaf.MultiplyFromTnaf(p, array[(int)num2]);
			}
			ECCurve curve = p.Curve;
			ECPoint[] points = array2;
			curve.NormalizeAll(points);
			return array2;
		}

		// Token: 0x06002165 RID: 8549 RVA: 0x000B42BC File Offset: 0x000B24BC
		// Note: this type is marked as 'beforefieldinit'.
		static Tnaf()
		{
			ZTauElement[] array = new ZTauElement[9];
			array[1] = new ZTauElement(BigInteger.One, BigInteger.Zero);
			array[3] = new ZTauElement(Tnaf.MinusThree, Tnaf.MinusOne);
			array[5] = new ZTauElement(Tnaf.MinusOne, Tnaf.MinusOne);
			array[7] = new ZTauElement(BigInteger.One, Tnaf.MinusOne);
			Tnaf.Alpha0 = array;
			Tnaf.Alpha0Tnaf = new sbyte[][]
			{
				default(sbyte[]),
				new sbyte[]
				{
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					-1,
					0,
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					1,
					0,
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					-1,
					0,
					0,
					1
				}
			};
			ZTauElement[] array2 = new ZTauElement[9];
			array2[1] = new ZTauElement(BigInteger.One, BigInteger.Zero);
			array2[3] = new ZTauElement(Tnaf.MinusThree, BigInteger.One);
			array2[5] = new ZTauElement(Tnaf.MinusOne, BigInteger.One);
			array2[7] = new ZTauElement(BigInteger.One, BigInteger.One);
			Tnaf.Alpha1 = array2;
			Tnaf.Alpha1Tnaf = new sbyte[][]
			{
				default(sbyte[]),
				new sbyte[]
				{
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					-1,
					0,
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					1,
					0,
					1
				},
				default(sbyte[]),
				new sbyte[]
				{
					-1,
					0,
					0,
					-1
				}
			};
		}

		// Token: 0x0400166C RID: 5740
		private static readonly BigInteger MinusOne = BigInteger.One.Negate();

		// Token: 0x0400166D RID: 5741
		private static readonly BigInteger MinusTwo = BigInteger.Two.Negate();

		// Token: 0x0400166E RID: 5742
		private static readonly BigInteger MinusThree = BigInteger.Three.Negate();

		// Token: 0x0400166F RID: 5743
		private static readonly BigInteger Four = BigInteger.ValueOf(4L);

		// Token: 0x04001670 RID: 5744
		public const sbyte Width = 4;

		// Token: 0x04001671 RID: 5745
		public const sbyte Pow2Width = 16;

		// Token: 0x04001672 RID: 5746
		public static readonly ZTauElement[] Alpha0;

		// Token: 0x04001673 RID: 5747
		public static readonly sbyte[][] Alpha0Tnaf;

		// Token: 0x04001674 RID: 5748
		public static readonly ZTauElement[] Alpha1;

		// Token: 0x04001675 RID: 5749
		public static readonly sbyte[][] Alpha1Tnaf;
	}
}
