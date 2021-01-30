using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E7 RID: 743
	public abstract class AbstractF2mPoint : ECPointBase
	{
		// Token: 0x06001A1E RID: 6686 RVA: 0x00095A6C File Offset: 0x00093C6C
		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001A1F RID: 6687 RVA: 0x00095A79 File Offset: 0x00093C79
		protected AbstractF2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001A20 RID: 6688 RVA: 0x00096BF8 File Offset: 0x00094DF8
		protected override bool SatisfiesCurveEquation()
		{
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = curve.A;
			ECFieldElement ecfieldElement2 = curve.B;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement ecfieldElement4;
			ECFieldElement ecfieldElement5;
			if (coordinateSystem == 6)
			{
				ECFieldElement ecfieldElement3 = base.RawZCoords[0];
				bool isOne = ecfieldElement3.IsOne;
				if (rawXCoord.IsZero)
				{
					ecfieldElement4 = rawYCoord.Square();
					ecfieldElement5 = ecfieldElement2;
					if (!isOne)
					{
						ECFieldElement b = ecfieldElement3.Square();
						ecfieldElement5 = ecfieldElement5.Multiply(b);
					}
				}
				else
				{
					ECFieldElement ecfieldElement6 = rawYCoord;
					ECFieldElement ecfieldElement7 = rawXCoord.Square();
					if (isOne)
					{
						ecfieldElement4 = ecfieldElement6.Square().Add(ecfieldElement6).Add(ecfieldElement);
						ecfieldElement5 = ecfieldElement7.Square().Add(ecfieldElement2);
					}
					else
					{
						ECFieldElement ecfieldElement8 = ecfieldElement3.Square();
						ECFieldElement y = ecfieldElement8.Square();
						ecfieldElement4 = ecfieldElement6.Add(ecfieldElement3).MultiplyPlusProduct(ecfieldElement6, ecfieldElement, ecfieldElement8);
						ecfieldElement5 = ecfieldElement7.SquarePlusProduct(ecfieldElement2, y);
					}
					ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement7);
				}
			}
			else
			{
				ecfieldElement4 = rawYCoord.Add(rawXCoord).Multiply(rawYCoord);
				if (coordinateSystem != 0)
				{
					if (coordinateSystem != 1)
					{
						throw new InvalidOperationException("unsupported coordinate system");
					}
					ECFieldElement ecfieldElement9 = base.RawZCoords[0];
					if (!ecfieldElement9.IsOne)
					{
						ECFieldElement b2 = ecfieldElement9.Square();
						ECFieldElement b3 = ecfieldElement9.Multiply(b2);
						ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement9);
						ecfieldElement = ecfieldElement.Multiply(ecfieldElement9);
						ecfieldElement2 = ecfieldElement2.Multiply(b3);
					}
				}
				ecfieldElement5 = rawXCoord.Add(ecfieldElement).Multiply(rawXCoord.Square()).Add(ecfieldElement2);
			}
			return ecfieldElement4.Equals(ecfieldElement5);
		}

		// Token: 0x06001A21 RID: 6689 RVA: 0x00096D84 File Offset: 0x00094F84
		public override ECPoint ScaleX(ECFieldElement scale)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem == 5)
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement rawYCoord = base.RawYCoord;
				ECFieldElement b = rawXCoord.Multiply(scale);
				ECFieldElement y = rawYCoord.Add(rawXCoord).Divide(scale).Add(b);
				return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
			}
			if (curveCoordinateSystem != 6)
			{
				return base.ScaleX(scale);
			}
			ECFieldElement rawXCoord2 = base.RawXCoord;
			ECFieldElement rawYCoord2 = base.RawYCoord;
			ECFieldElement ecfieldElement = base.RawZCoords[0];
			ECFieldElement b2 = rawXCoord2.Multiply(scale.Square());
			ECFieldElement y2 = rawYCoord2.Add(rawXCoord2).Add(b2);
			ECFieldElement ecfieldElement2 = ecfieldElement.Multiply(scale);
			return this.Curve.CreateRawPoint(rawXCoord2, y2, new ECFieldElement[]
			{
				ecfieldElement2
			}, base.IsCompressed);
		}

		// Token: 0x06001A22 RID: 6690 RVA: 0x00096E5C File Offset: 0x0009505C
		public override ECPoint ScaleY(ECFieldElement scale)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			int curveCoordinateSystem = this.CurveCoordinateSystem;
			if (curveCoordinateSystem - 5 <= 1)
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement y = base.RawYCoord.Add(rawXCoord).Multiply(scale).Add(rawXCoord);
				return this.Curve.CreateRawPoint(rawXCoord, y, base.RawZCoords, base.IsCompressed);
			}
			return base.ScaleY(scale);
		}

		// Token: 0x06001A23 RID: 6691 RVA: 0x00095BA8 File Offset: 0x00093DA8
		public override ECPoint Subtract(ECPoint b)
		{
			if (b.IsInfinity)
			{
				return this;
			}
			return this.Add(b.Negate());
		}

		// Token: 0x06001A24 RID: 6692 RVA: 0x00096EC4 File Offset: 0x000950C4
		public virtual AbstractF2mPoint Tau()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement rawXCoord = base.RawXCoord;
			switch (coordinateSystem)
			{
			case 0:
			case 5:
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.Square(), rawYCoord.Square(), base.IsCompressed);
			}
			case 1:
			case 6:
			{
				ECFieldElement rawYCoord2 = base.RawYCoord;
				ECFieldElement ecfieldElement = base.RawZCoords[0];
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.Square(), rawYCoord2.Square(), new ECFieldElement[]
				{
					ecfieldElement.Square()
				}, base.IsCompressed);
			}
			}
			throw new InvalidOperationException("unsupported coordinate system");
		}

		// Token: 0x06001A25 RID: 6693 RVA: 0x00096F84 File Offset: 0x00095184
		public virtual AbstractF2mPoint TauPow(int pow)
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement rawXCoord = base.RawXCoord;
			switch (coordinateSystem)
			{
			case 0:
			case 5:
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord.SquarePow(pow), base.IsCompressed);
			}
			case 1:
			case 6:
			{
				ECFieldElement rawYCoord2 = base.RawYCoord;
				ECFieldElement ecfieldElement = base.RawZCoords[0];
				return (AbstractF2mPoint)curve.CreateRawPoint(rawXCoord.SquarePow(pow), rawYCoord2.SquarePow(pow), new ECFieldElement[]
				{
					ecfieldElement.SquarePow(pow)
				}, base.IsCompressed);
			}
			}
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}
}
