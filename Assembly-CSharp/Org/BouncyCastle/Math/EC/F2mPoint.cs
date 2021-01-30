using System;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002E8 RID: 744
	public class F2mPoint : AbstractF2mPoint
	{
		// Token: 0x06001A26 RID: 6694 RVA: 0x00097049 File Offset: 0x00095249
		public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x06001A27 RID: 6695 RVA: 0x00097055 File Offset: 0x00095255
		public F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
			if (x != null)
			{
				F2mFieldElement.CheckFieldElements(x, y);
				if (curve != null)
				{
					F2mFieldElement.CheckFieldElements(x, curve.A);
				}
			}
		}

		// Token: 0x06001A28 RID: 6696 RVA: 0x00097090 File Offset: 0x00095290
		internal F2mPoint(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001A29 RID: 6697 RVA: 0x0009709F File Offset: 0x0009529F
		[Obsolete("Use ECCurve.Infinity property")]
		public F2mPoint(ECCurve curve) : this(curve, null, null)
		{
		}

		// Token: 0x06001A2A RID: 6698 RVA: 0x000970AA File Offset: 0x000952AA
		protected override ECPoint Detach()
		{
			return new F2mPoint(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06001A2B RID: 6699 RVA: 0x000970C0 File Offset: 0x000952C0
		public override ECFieldElement YCoord
		{
			get
			{
				int curveCoordinateSystem = this.CurveCoordinateSystem;
				if (curveCoordinateSystem - 5 > 1)
				{
					return base.RawYCoord;
				}
				ECFieldElement rawXCoord = base.RawXCoord;
				ECFieldElement rawYCoord = base.RawYCoord;
				if (base.IsInfinity || rawXCoord.IsZero)
				{
					return rawYCoord;
				}
				ECFieldElement ecfieldElement = rawYCoord.Add(rawXCoord).Multiply(rawXCoord);
				if (6 == curveCoordinateSystem)
				{
					ECFieldElement ecfieldElement2 = base.RawZCoords[0];
					if (!ecfieldElement2.IsOne)
					{
						ecfieldElement = ecfieldElement.Divide(ecfieldElement2);
					}
				}
				return ecfieldElement;
			}
		}

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x06001A2C RID: 6700 RVA: 0x00097130 File Offset: 0x00095330
		protected internal override bool CompressionYTilde
		{
			get
			{
				ECFieldElement rawXCoord = base.RawXCoord;
				if (rawXCoord.IsZero)
				{
					return false;
				}
				ECFieldElement rawYCoord = base.RawYCoord;
				int curveCoordinateSystem = this.CurveCoordinateSystem;
				if (curveCoordinateSystem - 5 <= 1)
				{
					return rawYCoord.TestBitZero() != rawXCoord.TestBitZero();
				}
				return rawYCoord.Divide(rawXCoord).TestBitZero();
			}
		}

		// Token: 0x06001A2D RID: 6701 RVA: 0x00097180 File Offset: 0x00095380
		public override ECPoint Add(ECPoint b)
		{
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			int coordinateSystem = curve.CoordinateSystem;
			ECFieldElement rawXCoord = base.RawXCoord;
			ECFieldElement rawXCoord2 = b.RawXCoord;
			if (coordinateSystem != 0)
			{
				if (coordinateSystem != 1)
				{
					if (coordinateSystem != 6)
					{
						throw new InvalidOperationException("unsupported coordinate system");
					}
					if (rawXCoord.IsZero)
					{
						if (rawXCoord2.IsZero)
						{
							return curve.Infinity;
						}
						return b.Add(this);
					}
					else
					{
						ECFieldElement rawYCoord = base.RawYCoord;
						ECFieldElement ecfieldElement = base.RawZCoords[0];
						ECFieldElement rawYCoord2 = b.RawYCoord;
						ECFieldElement ecfieldElement2 = b.RawZCoords[0];
						bool isOne = ecfieldElement.IsOne;
						ECFieldElement ecfieldElement3 = rawXCoord2;
						ECFieldElement ecfieldElement4 = rawYCoord2;
						if (!isOne)
						{
							ecfieldElement3 = ecfieldElement3.Multiply(ecfieldElement);
							ecfieldElement4 = ecfieldElement4.Multiply(ecfieldElement);
						}
						bool isOne2 = ecfieldElement2.IsOne;
						ECFieldElement ecfieldElement5 = rawXCoord;
						ECFieldElement ecfieldElement6 = rawYCoord;
						if (!isOne2)
						{
							ecfieldElement5 = ecfieldElement5.Multiply(ecfieldElement2);
							ecfieldElement6 = ecfieldElement6.Multiply(ecfieldElement2);
						}
						ECFieldElement ecfieldElement7 = ecfieldElement6.Add(ecfieldElement4);
						ECFieldElement ecfieldElement8 = ecfieldElement5.Add(ecfieldElement3);
						if (!ecfieldElement8.IsZero)
						{
							ECFieldElement ecfieldElement10;
							ECFieldElement y;
							ECFieldElement ecfieldElement11;
							if (rawXCoord2.IsZero)
							{
								ECPoint ecpoint = this.Normalize();
								rawXCoord = ecpoint.RawXCoord;
								ECFieldElement ycoord = ecpoint.YCoord;
								ECFieldElement b2 = rawYCoord2;
								ECFieldElement ecfieldElement9 = ycoord.Add(b2).Divide(rawXCoord);
								ecfieldElement10 = ecfieldElement9.Square().Add(ecfieldElement9).Add(rawXCoord).Add(curve.A);
								if (ecfieldElement10.IsZero)
								{
									return new F2mPoint(curve, ecfieldElement10, curve.B.Sqrt(), base.IsCompressed);
								}
								y = ecfieldElement9.Multiply(rawXCoord.Add(ecfieldElement10)).Add(ecfieldElement10).Add(ycoord).Divide(ecfieldElement10).Add(ecfieldElement10);
								ecfieldElement11 = curve.FromBigInteger(BigInteger.One);
							}
							else
							{
								ecfieldElement8 = ecfieldElement8.Square();
								ECFieldElement ecfieldElement12 = ecfieldElement7.Multiply(ecfieldElement5);
								ECFieldElement ecfieldElement13 = ecfieldElement7.Multiply(ecfieldElement3);
								ecfieldElement10 = ecfieldElement12.Multiply(ecfieldElement13);
								if (ecfieldElement10.IsZero)
								{
									return new F2mPoint(curve, ecfieldElement10, curve.B.Sqrt(), base.IsCompressed);
								}
								ECFieldElement ecfieldElement14 = ecfieldElement7.Multiply(ecfieldElement8);
								if (!isOne2)
								{
									ecfieldElement14 = ecfieldElement14.Multiply(ecfieldElement2);
								}
								y = ecfieldElement13.Add(ecfieldElement8).SquarePlusProduct(ecfieldElement14, rawYCoord.Add(ecfieldElement));
								ecfieldElement11 = ecfieldElement14;
								if (!isOne)
								{
									ecfieldElement11 = ecfieldElement11.Multiply(ecfieldElement);
								}
							}
							return new F2mPoint(curve, ecfieldElement10, y, new ECFieldElement[]
							{
								ecfieldElement11
							}, base.IsCompressed);
						}
						if (ecfieldElement7.IsZero)
						{
							return this.Twice();
						}
						return curve.Infinity;
					}
				}
				else
				{
					ECFieldElement rawYCoord3 = base.RawYCoord;
					ECFieldElement ecfieldElement15 = base.RawZCoords[0];
					ECFieldElement rawYCoord4 = b.RawYCoord;
					ECFieldElement ecfieldElement16 = b.RawZCoords[0];
					bool isOne3 = ecfieldElement15.IsOne;
					ECFieldElement ecfieldElement17 = rawYCoord4;
					ECFieldElement ecfieldElement18 = rawXCoord2;
					if (!isOne3)
					{
						ecfieldElement17 = ecfieldElement17.Multiply(ecfieldElement15);
						ecfieldElement18 = ecfieldElement18.Multiply(ecfieldElement15);
					}
					bool isOne4 = ecfieldElement16.IsOne;
					ECFieldElement ecfieldElement19 = rawYCoord3;
					ECFieldElement ecfieldElement20 = rawXCoord;
					if (!isOne4)
					{
						ecfieldElement19 = ecfieldElement19.Multiply(ecfieldElement16);
						ecfieldElement20 = ecfieldElement20.Multiply(ecfieldElement16);
					}
					ECFieldElement ecfieldElement21 = ecfieldElement17.Add(ecfieldElement19);
					ECFieldElement ecfieldElement22 = ecfieldElement18.Add(ecfieldElement20);
					if (!ecfieldElement22.IsZero)
					{
						ECFieldElement ecfieldElement23 = ecfieldElement22.Square();
						ECFieldElement ecfieldElement24 = ecfieldElement23.Multiply(ecfieldElement22);
						ECFieldElement b3 = isOne3 ? ecfieldElement16 : (isOne4 ? ecfieldElement15 : ecfieldElement15.Multiply(ecfieldElement16));
						ECFieldElement ecfieldElement25 = ecfieldElement21.Add(ecfieldElement22);
						ECFieldElement ecfieldElement26 = ecfieldElement25.MultiplyPlusProduct(ecfieldElement21, ecfieldElement23, curve.A).Multiply(b3).Add(ecfieldElement24);
						ECFieldElement x = ecfieldElement22.Multiply(ecfieldElement26);
						ECFieldElement b4 = isOne4 ? ecfieldElement23 : ecfieldElement23.Multiply(ecfieldElement16);
						ECFieldElement y2 = ecfieldElement21.MultiplyPlusProduct(rawXCoord, ecfieldElement22, rawYCoord3).MultiplyPlusProduct(b4, ecfieldElement25, ecfieldElement26);
						ECFieldElement ecfieldElement27 = ecfieldElement24.Multiply(b3);
						return new F2mPoint(curve, x, y2, new ECFieldElement[]
						{
							ecfieldElement27
						}, base.IsCompressed);
					}
					if (ecfieldElement21.IsZero)
					{
						return this.Twice();
					}
					return curve.Infinity;
				}
			}
			else
			{
				ECFieldElement rawYCoord5 = base.RawYCoord;
				ECFieldElement rawYCoord6 = b.RawYCoord;
				ECFieldElement ecfieldElement28 = rawXCoord.Add(rawXCoord2);
				ECFieldElement ecfieldElement29 = rawYCoord5.Add(rawYCoord6);
				if (!ecfieldElement28.IsZero)
				{
					ECFieldElement ecfieldElement30 = ecfieldElement29.Divide(ecfieldElement28);
					ECFieldElement ecfieldElement31 = ecfieldElement30.Square().Add(ecfieldElement30).Add(ecfieldElement28).Add(curve.A);
					ECFieldElement y3 = ecfieldElement30.Multiply(rawXCoord.Add(ecfieldElement31)).Add(ecfieldElement31).Add(rawYCoord5);
					return new F2mPoint(curve, ecfieldElement31, y3, base.IsCompressed);
				}
				if (ecfieldElement29.IsZero)
				{
					return this.Twice();
				}
				return curve.Infinity;
			}
		}

		// Token: 0x06001A2E RID: 6702 RVA: 0x0009761C File Offset: 0x0009581C
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return curve.Infinity;
			}
			int coordinateSystem = curve.CoordinateSystem;
			if (coordinateSystem == 0)
			{
				ECFieldElement ecfieldElement = base.RawYCoord.Divide(rawXCoord).Add(rawXCoord);
				ECFieldElement x = ecfieldElement.Square().Add(ecfieldElement).Add(curve.A);
				ECFieldElement y = rawXCoord.SquarePlusProduct(x, ecfieldElement.AddOne());
				return new F2mPoint(curve, x, y, base.IsCompressed);
			}
			if (coordinateSystem == 1)
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				ECFieldElement ecfieldElement2 = base.RawZCoords[0];
				bool isOne = ecfieldElement2.IsOne;
				ECFieldElement ecfieldElement3 = isOne ? rawXCoord : rawXCoord.Multiply(ecfieldElement2);
				ECFieldElement b = isOne ? rawYCoord : rawYCoord.Multiply(ecfieldElement2);
				ECFieldElement ecfieldElement4 = rawXCoord.Square();
				ECFieldElement ecfieldElement5 = ecfieldElement4.Add(b);
				ECFieldElement ecfieldElement6 = ecfieldElement3;
				ECFieldElement ecfieldElement7 = ecfieldElement6.Square();
				ECFieldElement ecfieldElement8 = ecfieldElement5.Add(ecfieldElement6);
				ECFieldElement ecfieldElement9 = ecfieldElement8.MultiplyPlusProduct(ecfieldElement5, ecfieldElement7, curve.A);
				ECFieldElement x2 = ecfieldElement6.Multiply(ecfieldElement9);
				ECFieldElement y2 = ecfieldElement4.Square().MultiplyPlusProduct(ecfieldElement6, ecfieldElement9, ecfieldElement8);
				ECFieldElement ecfieldElement10 = ecfieldElement6.Multiply(ecfieldElement7);
				return new F2mPoint(curve, x2, y2, new ECFieldElement[]
				{
					ecfieldElement10
				}, base.IsCompressed);
			}
			if (coordinateSystem != 6)
			{
				throw new InvalidOperationException("unsupported coordinate system");
			}
			ECFieldElement rawYCoord2 = base.RawYCoord;
			ECFieldElement ecfieldElement11 = base.RawZCoords[0];
			bool isOne2 = ecfieldElement11.IsOne;
			ECFieldElement ecfieldElement12 = isOne2 ? rawYCoord2 : rawYCoord2.Multiply(ecfieldElement11);
			ECFieldElement ecfieldElement13 = isOne2 ? ecfieldElement11 : ecfieldElement11.Square();
			ECFieldElement a = curve.A;
			ECFieldElement ecfieldElement14 = isOne2 ? a : a.Multiply(ecfieldElement13);
			ECFieldElement ecfieldElement15 = rawYCoord2.Square().Add(ecfieldElement12).Add(ecfieldElement14);
			if (ecfieldElement15.IsZero)
			{
				return new F2mPoint(curve, ecfieldElement15, curve.B.Sqrt(), base.IsCompressed);
			}
			ECFieldElement ecfieldElement16 = ecfieldElement15.Square();
			ECFieldElement ecfieldElement17 = isOne2 ? ecfieldElement15 : ecfieldElement15.Multiply(ecfieldElement13);
			ECFieldElement b2 = curve.B;
			ECFieldElement ecfieldElement19;
			if (b2.BitLength < curve.FieldSize >> 1)
			{
				ECFieldElement ecfieldElement18 = rawYCoord2.Add(rawXCoord).Square();
				ECFieldElement b3;
				if (b2.IsOne)
				{
					b3 = ecfieldElement14.Add(ecfieldElement13).Square();
				}
				else
				{
					b3 = ecfieldElement14.SquarePlusProduct(b2, ecfieldElement13.Square());
				}
				ecfieldElement19 = ecfieldElement18.Add(ecfieldElement15).Add(ecfieldElement13).Multiply(ecfieldElement18).Add(b3).Add(ecfieldElement16);
				if (a.IsZero)
				{
					ecfieldElement19 = ecfieldElement19.Add(ecfieldElement17);
				}
				else if (!a.IsOne)
				{
					ecfieldElement19 = ecfieldElement19.Add(a.AddOne().Multiply(ecfieldElement17));
				}
			}
			else
			{
				ecfieldElement19 = (isOne2 ? rawXCoord : rawXCoord.Multiply(ecfieldElement11)).SquarePlusProduct(ecfieldElement15, ecfieldElement12).Add(ecfieldElement16).Add(ecfieldElement17);
			}
			return new F2mPoint(curve, ecfieldElement16, ecfieldElement19, new ECFieldElement[]
			{
				ecfieldElement17
			}, base.IsCompressed);
		}

		// Token: 0x06001A2F RID: 6703 RVA: 0x00097928 File Offset: 0x00095B28
		public override ECPoint TwicePlus(ECPoint b)
		{
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this.Twice();
			}
			ECCurve curve = this.Curve;
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return b;
			}
			int coordinateSystem = curve.CoordinateSystem;
			if (coordinateSystem != 6)
			{
				return this.Twice().Add(b);
			}
			ECFieldElement rawXCoord2 = b.RawXCoord;
			ECFieldElement ecfieldElement = b.RawZCoords[0];
			if (rawXCoord2.IsZero || !ecfieldElement.IsOne)
			{
				return this.Twice().Add(b);
			}
			ECFieldElement rawYCoord = base.RawYCoord;
			ECFieldElement ecfieldElement2 = base.RawZCoords[0];
			ECFieldElement rawYCoord2 = b.RawYCoord;
			ECFieldElement x = rawXCoord.Square();
			ECFieldElement b2 = rawYCoord.Square();
			ECFieldElement ecfieldElement3 = ecfieldElement2.Square();
			ECFieldElement b3 = rawYCoord.Multiply(ecfieldElement2);
			ECFieldElement b4 = curve.A.Multiply(ecfieldElement3).Add(b2).Add(b3);
			ECFieldElement ecfieldElement4 = rawYCoord2.AddOne();
			ECFieldElement ecfieldElement5 = curve.A.Add(ecfieldElement4).Multiply(ecfieldElement3).Add(b2).MultiplyPlusProduct(b4, x, ecfieldElement3);
			ECFieldElement ecfieldElement6 = rawXCoord2.Multiply(ecfieldElement3);
			ECFieldElement ecfieldElement7 = ecfieldElement6.Add(b4).Square();
			if (ecfieldElement7.IsZero)
			{
				if (ecfieldElement5.IsZero)
				{
					return b.Twice();
				}
				return curve.Infinity;
			}
			else
			{
				if (ecfieldElement5.IsZero)
				{
					return new F2mPoint(curve, ecfieldElement5, curve.B.Sqrt(), base.IsCompressed);
				}
				ECFieldElement x2 = ecfieldElement5.Square().Multiply(ecfieldElement6);
				ECFieldElement ecfieldElement8 = ecfieldElement5.Multiply(ecfieldElement7).Multiply(ecfieldElement3);
				ECFieldElement y = ecfieldElement5.Add(ecfieldElement7).Square().MultiplyPlusProduct(b4, ecfieldElement4, ecfieldElement8);
				return new F2mPoint(curve, x2, y, new ECFieldElement[]
				{
					ecfieldElement8
				}, base.IsCompressed);
			}
		}

		// Token: 0x06001A30 RID: 6704 RVA: 0x00097AF0 File Offset: 0x00095CF0
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECFieldElement rawXCoord = base.RawXCoord;
			if (rawXCoord.IsZero)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			switch (curve.CoordinateSystem)
			{
			case 0:
			{
				ECFieldElement rawYCoord = base.RawYCoord;
				return new F2mPoint(curve, rawXCoord, rawYCoord.Add(rawXCoord), base.IsCompressed);
			}
			case 1:
			{
				ECFieldElement rawYCoord2 = base.RawYCoord;
				ECFieldElement ecfieldElement = base.RawZCoords[0];
				return new F2mPoint(curve, rawXCoord, rawYCoord2.Add(rawXCoord), new ECFieldElement[]
				{
					ecfieldElement
				}, base.IsCompressed);
			}
			case 5:
			{
				ECFieldElement rawYCoord3 = base.RawYCoord;
				return new F2mPoint(curve, rawXCoord, rawYCoord3.AddOne(), base.IsCompressed);
			}
			case 6:
			{
				ECFieldElement rawYCoord4 = base.RawYCoord;
				ECFieldElement ecfieldElement2 = base.RawZCoords[0];
				return new F2mPoint(curve, rawXCoord, rawYCoord4.Add(ecfieldElement2), new ECFieldElement[]
				{
					ecfieldElement2
				}, base.IsCompressed);
			}
			}
			throw new InvalidOperationException("unsupported coordinate system");
		}
	}
}
