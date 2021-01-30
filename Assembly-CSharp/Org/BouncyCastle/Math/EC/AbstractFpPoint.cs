using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E5 RID: 741
	public abstract class AbstractFpPoint : ECPointBase
	{
		// Token: 0x06001A06 RID: 6662 RVA: 0x00095A6C File Offset: 0x00093C6C
		protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
		}

		// Token: 0x06001A07 RID: 6663 RVA: 0x00095A79 File Offset: 0x00093C79
		protected AbstractFpPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06001A08 RID: 6664 RVA: 0x00095A88 File Offset: 0x00093C88
		protected internal override bool CompressionYTilde
		{
			get
			{
				return this.AffineYCoord.TestBitZero();
			}
		}

		// Token: 0x06001A09 RID: 6665 RVA: 0x00095A98 File Offset: 0x00093C98
		protected override bool SatisfiesCurveEquation()
		{
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement = this.Curve.A;
			ECFieldElement ecfieldElement2 = this.Curve.B;
			ECFieldElement ecfieldElement3 = rawYCoord.Square();
			switch (this.CurveCoordinateSystem)
			{
			case 0:
				break;
			case 1:
			{
				ECFieldElement ecfieldElement4 = base.RawZCoords[0];
				if (!ecfieldElement4.IsOne)
				{
					ECFieldElement b = ecfieldElement4.Square();
					ECFieldElement b2 = ecfieldElement4.Multiply(b);
					ecfieldElement3 = ecfieldElement3.Multiply(ecfieldElement4);
					ecfieldElement = ecfieldElement.Multiply(b);
					ecfieldElement2 = ecfieldElement2.Multiply(b2);
				}
				break;
			}
			case 2:
			case 3:
			case 4:
			{
				ECFieldElement ecfieldElement5 = base.RawZCoords[0];
				if (!ecfieldElement5.IsOne)
				{
					ECFieldElement ecfieldElement6 = ecfieldElement5.Square();
					ECFieldElement b3 = ecfieldElement6.Square();
					ECFieldElement b4 = ecfieldElement6.Multiply(b3);
					ecfieldElement = ecfieldElement.Multiply(b3);
					ecfieldElement2 = ecfieldElement2.Multiply(b4);
				}
				break;
			}
			default:
				throw new InvalidOperationException("unsupported coordinate system");
			}
			ECFieldElement other = rawXCoord.Square().Add(ecfieldElement).Multiply(rawXCoord).Add(ecfieldElement2);
			return ecfieldElement3.Equals(other);
		}

		// Token: 0x06001A0A RID: 6666 RVA: 0x00095BA8 File Offset: 0x00093DA8
		public override ECPoint Subtract(ECPoint b)
		{
			if (b.IsInfinity)
			{
				return this;
			}
			return this.Add(b.Negate());
		}
	}
}
