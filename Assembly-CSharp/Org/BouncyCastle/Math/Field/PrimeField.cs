using System;

namespace Org.BouncyCastle.Math.Field
{
	// Token: 0x020002D9 RID: 729
	internal class PrimeField : IFiniteField
	{
		// Token: 0x0600190B RID: 6411 RVA: 0x00092A71 File Offset: 0x00090C71
		internal PrimeField(BigInteger characteristic)
		{
			this.characteristic = characteristic;
		}

		// Token: 0x17000208 RID: 520
		// (get) Token: 0x0600190C RID: 6412 RVA: 0x00092A80 File Offset: 0x00090C80
		public virtual BigInteger Characteristic
		{
			get
			{
				return this.characteristic;
			}
		}

		// Token: 0x17000209 RID: 521
		// (get) Token: 0x0600190D RID: 6413 RVA: 0x0000900B File Offset: 0x0000720B
		public virtual int Dimension
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x0600190E RID: 6414 RVA: 0x00092A88 File Offset: 0x00090C88
		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			PrimeField primeField = obj as PrimeField;
			return primeField != null && this.characteristic.Equals(primeField.characteristic);
		}

		// Token: 0x0600190F RID: 6415 RVA: 0x00092AB8 File Offset: 0x00090CB8
		public override int GetHashCode()
		{
			return this.characteristic.GetHashCode();
		}

		// Token: 0x04001559 RID: 5465
		protected readonly BigInteger characteristic;
	}
}
