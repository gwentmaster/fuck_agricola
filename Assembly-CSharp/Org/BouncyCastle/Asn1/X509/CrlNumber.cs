using System;
using Org.BouncyCastle.Math;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x02000523 RID: 1315
	public class CrlNumber : DerInteger
	{
		// Token: 0x06003001 RID: 12289 RVA: 0x000F677E File Offset: 0x000F497E
		public CrlNumber(BigInteger number) : base(number)
		{
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06003002 RID: 12290 RVA: 0x000F6787 File Offset: 0x000F4987
		public BigInteger Number
		{
			get
			{
				return base.PositiveValue;
			}
		}

		// Token: 0x06003003 RID: 12291 RVA: 0x000F678F File Offset: 0x000F498F
		public override string ToString()
		{
			return "CRLNumber: " + this.Number;
		}
	}
}
