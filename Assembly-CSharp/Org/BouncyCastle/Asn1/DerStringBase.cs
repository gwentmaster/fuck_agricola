using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x02000500 RID: 1280
	public abstract class DerStringBase : Asn1Object, IAsn1String
	{
		// Token: 0x06002F28 RID: 12072
		public abstract string GetString();

		// Token: 0x06002F29 RID: 12073 RVA: 0x000F3F1C File Offset: 0x000F211C
		public override string ToString()
		{
			return this.GetString();
		}

		// Token: 0x06002F2A RID: 12074 RVA: 0x000F3F24 File Offset: 0x000F2124
		protected override int Asn1GetHashCode()
		{
			return this.GetString().GetHashCode();
		}
	}
}
