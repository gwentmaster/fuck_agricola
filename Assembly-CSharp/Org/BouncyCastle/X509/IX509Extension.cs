using System;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Utilities.Collections;

namespace Org.BouncyCastle.X509
{
	// Token: 0x02000279 RID: 633
	public interface IX509Extension
	{
		// Token: 0x060014B4 RID: 5300
		ISet GetCriticalExtensionOids();

		// Token: 0x060014B5 RID: 5301
		ISet GetNonCriticalExtensionOids();

		// Token: 0x060014B6 RID: 5302
		[Obsolete("Use version taking a DerObjectIdentifier instead")]
		Asn1OctetString GetExtensionValue(string oid);

		// Token: 0x060014B7 RID: 5303
		Asn1OctetString GetExtensionValue(DerObjectIdentifier oid);
	}
}
