using System;
using Org.BouncyCastle.Math.Field;

namespace Org.BouncyCastle.Math.EC
{
	// Token: 0x020002DC RID: 732
	public abstract class AbstractFpCurve : ECCurve
	{
		// Token: 0x0600194A RID: 6474 RVA: 0x00093AA0 File Offset: 0x00091CA0
		protected AbstractFpCurve(BigInteger q) : base(FiniteFields.GetPrimeField(q))
		{
		}

		// Token: 0x0600194B RID: 6475 RVA: 0x00093AAE File Offset: 0x00091CAE
		public override bool IsValidFieldElement(BigInteger x)
		{
			return x != null && x.SignValue >= 0 && x.CompareTo(this.Field.Characteristic) < 0;
		}

		// Token: 0x0600194C RID: 6476 RVA: 0x00093AD4 File Offset: 0x00091CD4
		protected override ECPoint DecompressPoint(int yTilde, BigInteger X1)
		{
			ECFieldElement ecfieldElement = this.FromBigInteger(X1);
			ECFieldElement ecfieldElement2 = ecfieldElement.Square().Add(this.A).Multiply(ecfieldElement).Add(this.B).Sqrt();
			if (ecfieldElement2 == null)
			{
				throw new ArgumentException("Invalid point compression");
			}
			if (ecfieldElement2.TestBitZero() != (yTilde == 1))
			{
				ecfieldElement2 = ecfieldElement2.Negate();
			}
			return this.CreateRawPoint(ecfieldElement, ecfieldElement2, true);
		}
	}
}
