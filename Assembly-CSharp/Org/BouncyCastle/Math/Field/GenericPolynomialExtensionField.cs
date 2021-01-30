using System;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D4 RID: 724
	internal class GenericPolynomialExtensionField : IPolynomialExtensionField, IExtensionField, IFiniteField
	{
		// Token: 0x060018FC RID: 6396 RVA: 0x000929B1 File Offset: 0x00090BB1
		internal GenericPolynomialExtensionField(IFiniteField subfield, IPolynomial polynomial)
		{
			this.subfield = subfield;
			this.minimalPolynomial = polynomial;
		}

		// Token: 0x170001FD RID: 509
		// (get) Token: 0x060018FD RID: 6397 RVA: 0x000929C7 File Offset: 0x00090BC7
		public virtual BigInteger Characteristic
		{
			get
			{
				return this.subfield.Characteristic;
			}
		}

		// Token: 0x170001FE RID: 510
		// (get) Token: 0x060018FE RID: 6398 RVA: 0x000929D4 File Offset: 0x00090BD4
		public virtual int Dimension
		{
			get
			{
				return this.subfield.Dimension * this.minimalPolynomial.Degree;
			}
		}

		// Token: 0x170001FF RID: 511
		// (get) Token: 0x060018FF RID: 6399 RVA: 0x000929ED File Offset: 0x00090BED
		public virtual IFiniteField Subfield
		{
			get
			{
				return this.subfield;
			}
		}

		// Token: 0x17000200 RID: 512
		// (get) Token: 0x06001900 RID: 6400 RVA: 0x000929F5 File Offset: 0x00090BF5
		public virtual int Degree
		{
			get
			{
				return this.minimalPolynomial.Degree;
			}
		}

		// Token: 0x17000201 RID: 513
		// (get) Token: 0x06001901 RID: 6401 RVA: 0x00092A02 File Offset: 0x00090C02
		public virtual IPolynomial MinimalPolynomial
		{
			get
			{
				return this.minimalPolynomial;
			}
		}

		// Token: 0x06001902 RID: 6402 RVA: 0x00092A0C File Offset: 0x00090C0C
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			GenericPolynomialExtensionField genericPolynomialExtensionField = obj as GenericPolynomialExtensionField;
			return genericPolynomialExtensionField != null && this.subfield.Equals(genericPolynomialExtensionField.subfield) && this.minimalPolynomial.Equals(genericPolynomialExtensionField.minimalPolynomial);
		}

		// Token: 0x06001903 RID: 6403 RVA: 0x00092A51 File Offset: 0x00090C51
		public override int GetHashCode()
		{
			return this.subfield.GetHashCode() ^ Integers.RotateLeft(this.minimalPolynomial.GetHashCode(), 16);
		}

		// Token: 0x04001557 RID: 5463
		protected readonly IFiniteField subfield;

		// Token: 0x04001558 RID: 5464
		protected readonly IPolynomial minimalPolynomial;
	}
}
