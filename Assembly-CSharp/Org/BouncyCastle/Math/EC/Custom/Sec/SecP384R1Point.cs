using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000325 RID: 805
	internal class SecP384R1Point : AbstractFpPoint
	{
		// Token: 0x06001D24 RID: 7460 RVA: 0x000A3989 File Offset: 0x000A1B89
		public SecP384R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x06001D25 RID: 7461 RVA: 0x00095BCC File Offset: 0x00093DCC
		public SecP384R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x06001D26 RID: 7462 RVA: 0x00095BEE File Offset: 0x00093DEE
		internal SecP384R1Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001D27 RID: 7463 RVA: 0x000A3995 File Offset: 0x000A1B95
		protected override ECPoint Detach()
		{
			return new SecP384R1Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06001D28 RID: 7464 RVA: 0x000A39AC File Offset: 0x000A1BAC
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
			if (this == b)
			{
				return this.Twice();
			}
			ECCurve curve = this.Curve;
			SecP384R1FieldElement secP384R1FieldElement = (SecP384R1FieldElement)base.RawXCoord;
			SecP384R1FieldElement secP384R1FieldElement2 = (SecP384R1FieldElement)base.RawYCoord;
			SecP384R1FieldElement secP384R1FieldElement3 = (SecP384R1FieldElement)b.RawXCoord;
			SecP384R1FieldElement secP384R1FieldElement4 = (SecP384R1FieldElement)b.RawYCoord;
			SecP384R1FieldElement secP384R1FieldElement5 = (SecP384R1FieldElement)base.RawZCoords[0];
			SecP384R1FieldElement secP384R1FieldElement6 = (SecP384R1FieldElement)b.RawZCoords[0];
			uint[] array = Nat.Create(24);
			uint[] array2 = Nat.Create(24);
			uint[] array3 = Nat.Create(12);
			uint[] array4 = Nat.Create(12);
			bool isOne = secP384R1FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP384R1FieldElement3.x;
				array6 = secP384R1FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP384R1Field.Square(secP384R1FieldElement5.x, array6);
				array5 = array2;
				SecP384R1Field.Multiply(array6, secP384R1FieldElement3.x, array5);
				SecP384R1Field.Multiply(array6, secP384R1FieldElement5.x, array6);
				SecP384R1Field.Multiply(array6, secP384R1FieldElement4.x, array6);
			}
			bool isOne2 = secP384R1FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP384R1FieldElement.x;
				array8 = secP384R1FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP384R1Field.Square(secP384R1FieldElement6.x, array8);
				array7 = array;
				SecP384R1Field.Multiply(array8, secP384R1FieldElement.x, array7);
				SecP384R1Field.Multiply(array8, secP384R1FieldElement6.x, array8);
				SecP384R1Field.Multiply(array8, secP384R1FieldElement2.x, array8);
			}
			uint[] array9 = Nat.Create(12);
			SecP384R1Field.Subtract(array7, array5, array9);
			uint[] array10 = Nat.Create(12);
			SecP384R1Field.Subtract(array8, array6, array10);
			if (!Nat.IsZero(12, array9))
			{
				uint[] array11 = array3;
				SecP384R1Field.Square(array9, array11);
				uint[] array12 = Nat.Create(12);
				SecP384R1Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP384R1Field.Multiply(array11, array7, array13);
				SecP384R1Field.Negate(array12, array12);
				Nat384.Mul(array8, array12, array);
				SecP384R1Field.Reduce32(Nat.AddBothTo(12, array13, array13, array12), array12);
				SecP384R1FieldElement secP384R1FieldElement7 = new SecP384R1FieldElement(array4);
				SecP384R1Field.Square(array10, secP384R1FieldElement7.x);
				SecP384R1Field.Subtract(secP384R1FieldElement7.x, array12, secP384R1FieldElement7.x);
				SecP384R1FieldElement secP384R1FieldElement8 = new SecP384R1FieldElement(array12);
				SecP384R1Field.Subtract(array13, secP384R1FieldElement7.x, secP384R1FieldElement8.x);
				Nat384.Mul(secP384R1FieldElement8.x, array10, array2);
				SecP384R1Field.AddExt(array, array2, array);
				SecP384R1Field.Reduce(array, secP384R1FieldElement8.x);
				SecP384R1FieldElement secP384R1FieldElement9 = new SecP384R1FieldElement(array9);
				if (!isOne)
				{
					SecP384R1Field.Multiply(secP384R1FieldElement9.x, secP384R1FieldElement5.x, secP384R1FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP384R1Field.Multiply(secP384R1FieldElement9.x, secP384R1FieldElement6.x, secP384R1FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP384R1FieldElement9
				};
				return new SecP384R1Point(curve, secP384R1FieldElement7, secP384R1FieldElement8, zs, base.IsCompressed);
			}
			if (Nat.IsZero(12, array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06001D29 RID: 7465 RVA: 0x000A3C98 File Offset: 0x000A1E98
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP384R1FieldElement secP384R1FieldElement = (SecP384R1FieldElement)base.RawYCoord;
			if (secP384R1FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP384R1FieldElement secP384R1FieldElement2 = (SecP384R1FieldElement)base.RawXCoord;
			SecP384R1FieldElement secP384R1FieldElement3 = (SecP384R1FieldElement)base.RawZCoords[0];
			uint[] array = Nat.Create(12);
			uint[] array2 = Nat.Create(12);
			uint[] array3 = Nat.Create(12);
			SecP384R1Field.Square(secP384R1FieldElement.x, array3);
			uint[] array4 = Nat.Create(12);
			SecP384R1Field.Square(array3, array4);
			bool isOne = secP384R1FieldElement3.IsOne;
			uint[] array5 = secP384R1FieldElement3.x;
			if (!isOne)
			{
				array5 = array2;
				SecP384R1Field.Square(secP384R1FieldElement3.x, array5);
			}
			SecP384R1Field.Subtract(secP384R1FieldElement2.x, array5, array);
			uint[] array6 = array2;
			SecP384R1Field.Add(secP384R1FieldElement2.x, array5, array6);
			SecP384R1Field.Multiply(array6, array, array6);
			SecP384R1Field.Reduce32(Nat.AddBothTo(12, array6, array6, array6), array6);
			uint[] array7 = array3;
			SecP384R1Field.Multiply(array3, secP384R1FieldElement2.x, array7);
			SecP384R1Field.Reduce32(Nat.ShiftUpBits(12, array7, 2, 0U), array7);
			SecP384R1Field.Reduce32(Nat.ShiftUpBits(12, array4, 3, 0U, array), array);
			SecP384R1FieldElement secP384R1FieldElement4 = new SecP384R1FieldElement(array4);
			SecP384R1Field.Square(array6, secP384R1FieldElement4.x);
			SecP384R1Field.Subtract(secP384R1FieldElement4.x, array7, secP384R1FieldElement4.x);
			SecP384R1Field.Subtract(secP384R1FieldElement4.x, array7, secP384R1FieldElement4.x);
			SecP384R1FieldElement secP384R1FieldElement5 = new SecP384R1FieldElement(array7);
			SecP384R1Field.Subtract(array7, secP384R1FieldElement4.x, secP384R1FieldElement5.x);
			SecP384R1Field.Multiply(secP384R1FieldElement5.x, array6, secP384R1FieldElement5.x);
			SecP384R1Field.Subtract(secP384R1FieldElement5.x, array, secP384R1FieldElement5.x);
			SecP384R1FieldElement secP384R1FieldElement6 = new SecP384R1FieldElement(array6);
			SecP384R1Field.Twice(secP384R1FieldElement.x, secP384R1FieldElement6.x);
			if (!isOne)
			{
				SecP384R1Field.Multiply(secP384R1FieldElement6.x, secP384R1FieldElement3.x, secP384R1FieldElement6.x);
			}
			return new SecP384R1Point(curve, secP384R1FieldElement4, secP384R1FieldElement5, new ECFieldElement[]
			{
				secP384R1FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x06001D2A RID: 7466 RVA: 0x000A3E9C File Offset: 0x000A209C
		public override ECPoint TwicePlus(ECPoint b)
		{
			if (this == b)
			{
				return this.ThreeTimes();
			}
			if (base.IsInfinity)
			{
				return b;
			}
			if (b.IsInfinity)
			{
				return this.Twice();
			}
			if (base.RawYCoord.IsZero)
			{
				return b;
			}
			return this.Twice().Add(b);
		}

		// Token: 0x06001D2B RID: 7467 RVA: 0x0009B664 File Offset: 0x00099864
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x06001D2C RID: 7468 RVA: 0x000A3EE8 File Offset: 0x000A20E8
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP384R1Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
