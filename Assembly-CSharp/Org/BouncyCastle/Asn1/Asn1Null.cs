using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CD RID: 1229
	public abstract class Asn1Null : Asn1Object
	{
		// Token: 0x06002DA5 RID: 11685 RVA: 0x000EFEFC File Offset: 0x000EE0FC
		internal Asn1Null()
		{
		}

		// Token: 0x06002DA6 RID: 11686 RVA: 0x000EFF04 File Offset: 0x000EE104
		public override string ToString()
		{
			return "NULL";
		}
	}
}
