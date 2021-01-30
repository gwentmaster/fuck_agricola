using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D3 RID: 723
	internal class GF2Polynomial : IPolynomial
	{
		// Token: 0x060018F7 RID: 6391 RVA: 0x00092940 File Offset: 0x00090B40
		internal GF2Polynomial(int[] exponents)
		{
			this.exponents = Arrays.Clone(exponents);
		}

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x060018F8 RID: 6392 RVA: 0x00092954 File Offset: 0x00090B54
		public virtual int Degree
		{
			get
			{
				return this.exponents[this.exponents.Length - 1];
			}
		}

		// Token: 0x060018F9 RID: 6393 RVA: 0x00092967 File Offset: 0x00090B67
		public virtual int[] GetExponentsPresent()
		{
			return Arrays.Clone(this.exponents);
		}

		// Token: 0x060018FA RID: 6394 RVA: 0x00092974 File Offset: 0x00090B74
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			GF2Polynomial gf2Polynomial = obj as GF2Polynomial;
			return gf2Polynomial != null && Arrays.AreEqual(this.exponents, gf2Polynomial.exponents);
		}

		// Token: 0x060018FB RID: 6395 RVA: 0x000929A4 File Offset: 0x00090BA4
		public override int GetHashCode()
		{
			return Arrays.GetHashCode(this.exponents);
		}

		// Token: 0x04001556 RID: 5462
		protected readonly int[] exponents;
	}
}
