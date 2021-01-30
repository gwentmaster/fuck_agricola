﻿using System;
using Org.BouncyCastle.Math.Raw;

namespace Org.BouncyCastle.Math.EC.Custom.Sec
{
	// Token: 0x02000309 RID: 777
	internal class SecP160R2Point : AbstractFpPoint
	{
		// Token: 0x06001B7B RID: 7035 RVA: 0x0009D26F File Offset: 0x0009B46F
		public SecP160R2Point(ECCurve curve, ECFieldElement x, ECFieldElement y) : this(curve, x, y, false)
		{
		}

		// Token: 0x06001B7C RID: 7036 RVA: 0x00095BCC File Offset: 0x00093DCC
		public SecP160R2Point(ECCurve curve, ECFieldElement x, ECFieldElement y, bool withCompression) : base(curve, x, y, withCompression)
		{
			if (x == null != (y == null))
			{
				throw new ArgumentException("Exactly one of the field elements is null");
			}
		}

		// Token: 0x06001B7D RID: 7037 RVA: 0x00095BEE File Offset: 0x00093DEE
		internal SecP160R2Point(ECCurve curve, ECFieldElement x, ECFieldElement y, ECFieldElement[] zs, bool withCompression) : base(curve, x, y, zs, withCompression)
		{
		}

		// Token: 0x06001B7E RID: 7038 RVA: 0x0009D27B File Offset: 0x0009B47B
		protected override ECPoint Detach()
		{
			return new SecP160R2Point(null, this.AffineXCoord, this.AffineYCoord);
		}

		// Token: 0x06001B7F RID: 7039 RVA: 0x0009D290 File Offset: 0x0009B490
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
			SecP160R2FieldElement secP160R2FieldElement = (SecP160R2FieldElement)base.RawXCoord;
			SecP160R2FieldElement secP160R2FieldElement2 = (SecP160R2FieldElement)base.RawYCoord;
			SecP160R2FieldElement secP160R2FieldElement3 = (SecP160R2FieldElement)b.RawXCoord;
			SecP160R2FieldElement secP160R2FieldElement4 = (SecP160R2FieldElement)b.RawYCoord;
			SecP160R2FieldElement secP160R2FieldElement5 = (SecP160R2FieldElement)base.RawZCoords[0];
			SecP160R2FieldElement secP160R2FieldElement6 = (SecP160R2FieldElement)b.RawZCoords[0];
			uint[] array = Nat160.CreateExt();
			uint[] array2 = Nat160.Create();
			uint[] array3 = Nat160.Create();
			uint[] array4 = Nat160.Create();
			bool isOne = secP160R2FieldElement5.IsOne;
			uint[] array5;
			uint[] array6;
			if (isOne)
			{
				array5 = secP160R2FieldElement3.x;
				array6 = secP160R2FieldElement4.x;
			}
			else
			{
				array6 = array3;
				SecP160R2Field.Square(secP160R2FieldElement5.x, array6);
				array5 = array2;
				SecP160R2Field.Multiply(array6, secP160R2FieldElement3.x, array5);
				SecP160R2Field.Multiply(array6, secP160R2FieldElement5.x, array6);
				SecP160R2Field.Multiply(array6, secP160R2FieldElement4.x, array6);
			}
			bool isOne2 = secP160R2FieldElement6.IsOne;
			uint[] array7;
			uint[] array8;
			if (isOne2)
			{
				array7 = secP160R2FieldElement.x;
				array8 = secP160R2FieldElement2.x;
			}
			else
			{
				array8 = array4;
				SecP160R2Field.Square(secP160R2FieldElement6.x, array8);
				array7 = array;
				SecP160R2Field.Multiply(array8, secP160R2FieldElement.x, array7);
				SecP160R2Field.Multiply(array8, secP160R2FieldElement6.x, array8);
				SecP160R2Field.Multiply(array8, secP160R2FieldElement2.x, array8);
			}
			uint[] array9 = Nat160.Create();
			SecP160R2Field.Subtract(array7, array5, array9);
			uint[] array10 = array2;
			SecP160R2Field.Subtract(array8, array6, array10);
			if (!Nat160.IsZero(array9))
			{
				uint[] array11 = array3;
				SecP160R2Field.Square(array9, array11);
				uint[] array12 = Nat160.Create();
				SecP160R2Field.Multiply(array11, array9, array12);
				uint[] array13 = array3;
				SecP160R2Field.Multiply(array11, array7, array13);
				SecP160R2Field.Negate(array12, array12);
				Nat160.Mul(array8, array12, array);
				SecP160R2Field.Reduce32(Nat160.AddBothTo(array13, array13, array12), array12);
				SecP160R2FieldElement secP160R2FieldElement7 = new SecP160R2FieldElement(array4);
				SecP160R2Field.Square(array10, secP160R2FieldElement7.x);
				SecP160R2Field.Subtract(secP160R2FieldElement7.x, array12, secP160R2FieldElement7.x);
				SecP160R2FieldElement secP160R2FieldElement8 = new SecP160R2FieldElement(array12);
				SecP160R2Field.Subtract(array13, secP160R2FieldElement7.x, secP160R2FieldElement8.x);
				SecP160R2Field.MultiplyAddToExt(secP160R2FieldElement8.x, array10, array);
				SecP160R2Field.Reduce(array, secP160R2FieldElement8.x);
				SecP160R2FieldElement secP160R2FieldElement9 = new SecP160R2FieldElement(array9);
				if (!isOne)
				{
					SecP160R2Field.Multiply(secP160R2FieldElement9.x, secP160R2FieldElement5.x, secP160R2FieldElement9.x);
				}
				if (!isOne2)
				{
					SecP160R2Field.Multiply(secP160R2FieldElement9.x, secP160R2FieldElement6.x, secP160R2FieldElement9.x);
				}
				ECFieldElement[] zs = new ECFieldElement[]
				{
					secP160R2FieldElement9
				};
				return new SecP160R2Point(curve, secP160R2FieldElement7, secP160R2FieldElement8, zs, base.IsCompressed);
			}
			if (Nat160.IsZero(array10))
			{
				return this.Twice();
			}
			return curve.Infinity;
		}

		// Token: 0x06001B80 RID: 7040 RVA: 0x0009D558 File Offset: 0x0009B758
		public override ECPoint Twice()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			ECCurve curve = this.Curve;
			SecP160R2FieldElement secP160R2FieldElement = (SecP160R2FieldElement)base.RawYCoord;
			if (secP160R2FieldElement.IsZero)
			{
				return curve.Infinity;
			}
			SecP160R2FieldElement secP160R2FieldElement2 = (SecP160R2FieldElement)base.RawXCoord;
			SecP160R2FieldElement secP160R2FieldElement3 = (SecP160R2FieldElement)base.RawZCoords[0];
			uint[] array = Nat160.Create();
			uint[] array2 = Nat160.Create();
			uint[] array3 = Nat160.Create();
			SecP160R2Field.Square(secP160R2FieldElement.x, array3);
			uint[] array4 = Nat160.Create();
			SecP160R2Field.Square(array3, array4);
			bool isOne = secP160R2FieldElement3.IsOne;
			uint[] array5 = secP160R2FieldElement3.x;
			if (!isOne)
			{
				array5 = array2;
				SecP160R2Field.Square(secP160R2FieldElement3.x, array5);
			}
			SecP160R2Field.Subtract(secP160R2FieldElement2.x, array5, array);
			uint[] array6 = array2;
			SecP160R2Field.Add(secP160R2FieldElement2.x, array5, array6);
			SecP160R2Field.Multiply(array6, array, array6);
			SecP160R2Field.Reduce32(Nat160.AddBothTo(array6, array6, array6), array6);
			uint[] array7 = array3;
			SecP160R2Field.Multiply(array3, secP160R2FieldElement2.x, array7);
			SecP160R2Field.Reduce32(Nat.ShiftUpBits(5, array7, 2, 0U), array7);
			SecP160R2Field.Reduce32(Nat.ShiftUpBits(5, array4, 3, 0U, array), array);
			SecP160R2FieldElement secP160R2FieldElement4 = new SecP160R2FieldElement(array4);
			SecP160R2Field.Square(array6, secP160R2FieldElement4.x);
			SecP160R2Field.Subtract(secP160R2FieldElement4.x, array7, secP160R2FieldElement4.x);
			SecP160R2Field.Subtract(secP160R2FieldElement4.x, array7, secP160R2FieldElement4.x);
			SecP160R2FieldElement secP160R2FieldElement5 = new SecP160R2FieldElement(array7);
			SecP160R2Field.Subtract(array7, secP160R2FieldElement4.x, secP160R2FieldElement5.x);
			SecP160R2Field.Multiply(secP160R2FieldElement5.x, array6, secP160R2FieldElement5.x);
			SecP160R2Field.Subtract(secP160R2FieldElement5.x, array, secP160R2FieldElement5.x);
			SecP160R2FieldElement secP160R2FieldElement6 = new SecP160R2FieldElement(array6);
			SecP160R2Field.Twice(secP160R2FieldElement.x, secP160R2FieldElement6.x);
			if (!isOne)
			{
				SecP160R2Field.Multiply(secP160R2FieldElement6.x, secP160R2FieldElement3.x, secP160R2FieldElement6.x);
			}
			return new SecP160R2Point(curve, secP160R2FieldElement4, secP160R2FieldElement5, new ECFieldElement[]
			{
				secP160R2FieldElement6
			}, base.IsCompressed);
		}

		// Token: 0x06001B81 RID: 7041 RVA: 0x0009D750 File Offset: 0x0009B950
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

		// Token: 0x06001B82 RID: 7042 RVA: 0x0009B664 File Offset: 0x00099864
		public override ECPoint ThreeTimes()
		{
			if (base.IsInfinity || base.RawYCoord.IsZero)
			{
				return this;
			}
			return this.Twice().Add(this);
		}

		// Token: 0x06001B83 RID: 7043 RVA: 0x0009D79C File Offset: 0x0009B99C
		public override ECPoint Negate()
		{
			if (base.IsInfinity)
			{
				return this;
			}
			return new SecP160R2Point(this.Curve, base.RawXCoord, base.RawYCoord.Negate(), base.RawZCoords, base.IsCompressed);
		}
	}
}
