using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E3 RID: 739
	public abstract class ECPoint
	{
		// Token: 0x060019D4 RID: 6612 RVA: 0x000953B4 File Offset: 0x000935B4
		protected static ECFieldElement[] GetInitialZCoords(ECCurve curve)
		{
			int num = (curve == null) ? 0 : curve.CoordinateSystem;
			if (num == 0 || num == 5)
			{
				return ECPoint.EMPTY_ZS;
			}
			ECFieldElement ecfieldElement = curve.FromBigInteger(BigInteger.One);
			switch (num)
			{
			case 1:
			case 2:
			case 6:
				return new ECFieldElement[]
				{
					ecfieldElement
				};
			case 3:
				return new ECFieldElement[]
				{
					ecfieldElement,
					ecfieldElement,
					ecfieldElement
				};
			case 4:
				return new ECFieldElement[]
				{
					ecfieldElement,
					curve.A
				};
			}
			throw new ArgumentException("unknown coordinate system");
		}

		// Token: 0x060019D5 RID: 6613 RVA: 0x00095445 File Offset: 0x00093645
		protected ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : this(curve, x, y, ECPoint.GetInitialZCoords(curve), withCompression)
		{
		}

		// Token: 0x060019D6 RID: 6614 RVA: 0x00095458 File Offset: 0x00093658
		internal ECPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression)
		{
			this.m_curve = curve;
			this.m_x = x;
			this.m_y = y;
			this.m_zs = zs;
			this.m_withCompression = withCompression;
		}

		// Token: 0x060019D7 RID: 6615 RVA: 0x00095488 File Offset: 0x00093688
		protected internal bool SatisfiesCofactor()
		{
			BigInteger cofactor = this.Curve.Cofactor;
			return cofactor == null || cofactor.Equals(BigInteger.One) || !ECAlgorithms.ReferenceMultiply(this, cofactor).IsInfinity;
		}

		// Token: 0x060019D8 RID: 6616
		protected abstract bool SatisfiesCurveEquation();

		// Token: 0x060019D9 RID: 6617 RVA: 0x000954C2 File Offset: 0x000936C2
		public ECPoint GetDetachedPoint()
		{
			return this.Normalize().Detach();
		}

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x060019DA RID: 6618 RVA: 0x000954CF File Offset: 0x000936CF
		public virtual ECCurve Curve
		{
			get
			{
				return this.m_curve;
			}
		}

		// Token: 0x060019DB RID: 6619
		protected abstract ECPoint Detach();

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x060019DC RID: 6620 RVA: 0x000954D7 File Offset: 0x000936D7
		protected virtual int CurveCoordinateSystem
		{
			get
			{
				if (this.m_curve != null)
				{
					return this.m_curve.CoordinateSystem;
				}
				return 0;
			}
		}

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x060019DD RID: 6621 RVA: 0x000954EE File Offset: 0x000936EE
		[Obsolete("Use AffineXCoord, or Normalize() and XCoord, instead")]
		public virtual ECFieldElement X
		{
			get
			{
				return this.Normalize().XCoord;
			}
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x060019DE RID: 6622 RVA: 0x000954FB File Offset: 0x000936FB
		[Obsolete("Use AffineYCoord, or Normalize() and YCoord, instead")]
		public virtual ECFieldElement Y
		{
			get
			{
				return this.Normalize().YCoord;
			}
		}

		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060019DF RID: 6623 RVA: 0x00095508 File Offset: 0x00093708
		public virtual ECFieldElement AffineXCoord
		{
			get
			{
				this.CheckNormalized();
				return this.XCoord;
			}
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060019E0 RID: 6624 RVA: 0x00095516 File Offset: 0x00093716
		public virtual ECFieldElement AffineYCoord
		{
			get
			{
				this.CheckNormalized();
				return this.YCoord;
			}
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060019E1 RID: 6625 RVA: 0x00095524 File Offset: 0x00093724
		public virtual ECFieldElement XCoord
		{
			get
			{
				return this.m_x;
			}
		}

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060019E2 RID: 6626 RVA: 0x0009552C File Offset: 0x0009372C
		public virtual ECFieldElement YCoord
		{
			get
			{
				return this.m_y;
			}
		}

		// Token: 0x060019E3 RID: 6627 RVA: 0x00095534 File Offset: 0x00093734
		public virtual ECFieldElement GetZCoord(int index)
		{
			if (index >= 0 && index < this.m_zs.Length)
			{
				return this.m_zs[index];
			}
			return null;
		}

		// Token: 0x060019E4 RID: 6628 RVA: 0x00095550 File Offset: 0x00093750
		public virtual ECFieldElement[] GetZCoords()
		{
			int num = this.m_zs.Length;
			if (num == 0)
			{
				return this.m_zs;
			}
			ECFieldElement[] array = new ECFieldElement[num];
			Array.Copy(this.m_zs, 0, array, 0, num);
			return array;
		}

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060019E5 RID: 6629 RVA: 0x00095524 File Offset: 0x00093724
		protected internal ECFieldElement RawXCoord
		{
			get
			{
				return this.m_x;
			}
		}

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060019E6 RID: 6630 RVA: 0x0009552C File Offset: 0x0009372C
		protected internal ECFieldElement RawYCoord
		{
			get
			{
				return this.m_y;
			}
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060019E7 RID: 6631 RVA: 0x00095587 File Offset: 0x00093787
		protected internal ECFieldElement[] RawZCoords
		{
			get
			{
				return this.m_zs;
			}
		}

		// Token: 0x060019E8 RID: 6632 RVA: 0x0009558F File Offset: 0x0009378F
		protected virtual void CheckNormalized()
		{
			if (!this.IsNormalized())
			{
				throw new InvalidOperationException("point not in normal form");
			}
		}

		// Token: 0x060019E9 RID: 6633 RVA: 0x000955A4 File Offset: 0x000937A4
		public virtual bool IsNormalized()
		{
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			return curveCoordinateSystem == 0 || curveCoordinateSystem == 5 || this.IsInfinity || this.RawZCoords[0].IsOne;
		}

		// Token: 0x060019EA RID: 6634 RVA: 0x000955D8 File Offset: 0x000937D8
		public virtual ECPoint Normalize()
		{
			if (this.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem == 0 || curveCoordinateSystem == 5)
			{
				return this;
			}
			ECFieldElement ecfieldElement = this.RawZCoords[0];
			if (ecfieldElement.IsOne)
			{
				return this;
			}
			return this.Normalize(ecfieldElement.Invert());
		}

		// Token: 0x060019EB RID: 6635 RVA: 0x00095620 File Offset: 0x00093820
		internal virtual ECPoint Normalize(ECFieldElement zInv)
		{
			switch (this.CurveCoordinateSystem)
			{
			case 1:
			case 6:
				return this.CreateScaledPoint(zInv, zInv);
			case 2:
			case 3:
			case 4:
			{
				ECFieldElement ecfieldElement = zInv.Square();
				ECFieldElement sy = ecfieldElement.Multiply(zInv);
				return this.CreateScaledPoint(ecfieldElement, sy);
			}
			}
			throw new InvalidOperationException("not a projective coordinate system");
		}

		// Token: 0x060019EC RID: 6636 RVA: 0x00095681 File Offset: 0x00093881
		protected virtual ECPoint CreateScaledPoint(ECFieldElement sx, ECFieldElement sy)
		{
			return this.Curve.CreateRawPoint(this.RawXCoord.Multiply(sx), this.RawYCoord.Multiply(sy), this.IsCompressed);
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x060019ED RID: 6637 RVA: 0x000956AC File Offset: 0x000938AC
		public bool IsInfinity
		{
			get
			{
				return this.m_x == null && this.m_y == null;
			}
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x060019EE RID: 6638 RVA: 0x000956C1 File Offset: 0x000938C1
		public bool IsCompressed
		{
			get
			{
				return this.m_withCompression;
			}
		}

		// Token: 0x060019EF RID: 6639 RVA: 0x000956C9 File Offset: 0x000938C9
		public bool IsValid()
		{
			if (this.IsInfinity)
			{
				return true;
			}
			if (this.Curve != null)
			{
				if (!this.SatisfiesCurveEquation())
				{
					return false;
				}
				if (!this.SatisfiesCofactor())
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x060019F0 RID: 6640 RVA: 0x000956F2 File Offset: 0x000938F2
		public virtual ECPoint ScaleX(ECFieldElement scale)
		{
			if (!this.IsInfinity)
			{
				return this.Curve.CreateRawPoint(this.RawXCoord.Multiply(scale), this.RawYCoord, this.RawZCoords, this.IsCompressed);
			}
			return this;
		}

		// Token: 0x060019F1 RID: 6641 RVA: 0x00095727 File Offset: 0x00093927
		public virtual ECPoint ScaleY(ECFieldElement scale)
		{
			if (!this.IsInfinity)
			{
				return this.Curve.CreateRawPoint(this.RawXCoord, this.RawYCoord.Multiply(scale), this.RawZCoords, this.IsCompressed);
			}
			return this;
		}

		// Token: 0x060019F2 RID: 6642 RVA: 0x0009575C File Offset: 0x0009395C
		public override bool Equals(object obj)
		{
			return this.Equals(obj as ECPoint);
		}

		// Token: 0x060019F3 RID: 6643 RVA: 0x0009576C File Offset: 0x0009396C
		public virtual bool Equals(ECPoint other)
		{
			if (this == other)
			{
				return true;
			}
			if (other == null)
			{
				return false;
			}
			ECCurve curve = this.Curve;
			ECCurve curve2 = other.Curve;
			bool flag = curve == null;
			bool flag2 = curve2 == null;
			bool isInfinity = this.IsInfinity;
			bool isInfinity2 = other.IsInfinity;
			if (isInfinity || isInfinity2)
			{
				return isInfinity && isInfinity2 && (flag || flag2 || curve.Equals(curve2));
			}
			ECPoint ecpoint = this;
			ECPoint ecpoint2 = other;
			if (!flag || !flag2)
			{
				if (flag)
				{
					ecpoint2 = ecpoint2.Normalize();
				}
				else if (flag2)
				{
					ecpoint = ecpoint.Normalize();
				}
				else
				{
					if (!curve.Equals(curve2))
					{
						return false;
					}
					ECPoint[] array = new ECPoint[]
					{
						this,
						curve.ImportPoint(ecpoint2)
					};
					curve.NormalizeAll(array);
					ecpoint = array[0];
					ecpoint2 = array[1];
				}
			}
			return ecpoint.XCoord.Equals(ecpoint2.XCoord) && ecpoint.YCoord.Equals(ecpoint2.YCoord);
		}

		// Token: 0x060019F4 RID: 6644 RVA: 0x00095854 File Offset: 0x00093A54
		public override int GetHashCode()
		{
			ECCurve curve = this.Curve;
			int num = (curve == null) ? 0 : (~curve.GetHashCode());
			if (!this.IsInfinity)
			{
				ECPoint ecpoint = this.Normalize();
				num ^= ecpoint.XCoord.GetHashCode() * 17;
				num ^= ecpoint.YCoord.GetHashCode() * 257;
			}
			return num;
		}

		// Token: 0x060019F5 RID: 6645 RVA: 0x000958AC File Offset: 0x00093AAC
		public override string ToString()
		{
			if (this.IsInfinity)
			{
				return "INF";
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append('(');
			stringBuilder.Append(this.RawXCoord);
			stringBuilder.Append(',');
			stringBuilder.Append(this.RawYCoord);
			for (int i = 0; i < this.m_zs.Length; i++)
			{
				stringBuilder.Append(',');
				stringBuilder.Append(this.m_zs[i]);
			}
			stringBuilder.Append(')');
			return stringBuilder.ToString();
		}

		// Token: 0x060019F6 RID: 6646 RVA: 0x00095933 File Offset: 0x00093B33
		public virtual byte[] GetEncoded()
		{
			return this.GetEncoded(this.m_withCompression);
		}

		// Token: 0x060019F7 RID: 6647
		public abstract byte[] GetEncoded(bool compressed);

		// Token: 0x1700023D RID: 573
		// (get) Token: 0x060019F8 RID: 6648
		protected internal abstract bool CompressionYTilde { get; }

		// Token: 0x060019F9 RID: 6649
		public abstract ECPoint Add(ECPoint b);

		// Token: 0x060019FA RID: 6650
		public abstract ECPoint Subtract(ECPoint b);

		// Token: 0x060019FB RID: 6651
		public abstract ECPoint Negate();

		// Token: 0x060019FC RID: 6652 RVA: 0x00095944 File Offset: 0x00093B44
		public virtual ECPoint TimesPow2(int e)
		{
			if (e < 0)
			{
				throw new ArgumentException("cannot be negative", "e");
			}
			ECPoint ecpoint = this;
			while (--e >= 0)
			{
				ecpoint = ecpoint.Twice();
			}
			return ecpoint;
		}

		// Token: 0x060019FD RID: 6653
		public abstract ECPoint Twice();

		// Token: 0x060019FE RID: 6654
		public abstract ECPoint Multiply(BigInteger b);

		// Token: 0x060019FF RID: 6655 RVA: 0x0009597A File Offset: 0x00093B7A
		public virtual ECPoint TwicePlus(ECPoint b)
		{
			return this.Twice().Add(b);
		}

		// Token: 0x06001A00 RID: 6656 RVA: 0x00095988 File Offset: 0x00093B88
		public virtual ECPoint ThreeTimes()
		{
			return this.TwicePlus(this);
		}

		// Token: 0x0400157F RID: 5503
		protected static ECFieldElement[] EMPTY_ZS = new ECFieldElement[0];

		// Token: 0x04001580 RID: 5504
		protected internal readonly ECCurve m_curve;

		// Token: 0x04001581 RID: 5505
		protected internal readonly ECFieldElement m_x;

		// Token: 0x04001582 RID: 5506
		protected internal readonly ECFieldElement m_y;

		// Token: 0x04001583 RID: 5507
		protected internal readonly ECFieldElement[] m_zs;

		// Token: 0x04001584 RID: 5508
		protected internal readonly bool m_withCompression;

		// Token: 0x04001585 RID: 5509
		protected internal IDictionary m_preCompTable;
	}
}
