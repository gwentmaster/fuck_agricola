using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004C8 RID: 1224
	public interface Asn1TaggedObjectParser : IAsn1Convertible
	{
		// Token: 0x1700052F RID: 1327
		// (get) Token: 0x06002D82 RID: 11650
		int TagNo { get; }

		// Token: 0x06002D83 RID: 11651
		IAsn1Convertible GetObjectParser(int tag, bool isExplicit);
	}
}
