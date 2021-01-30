using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004EC RID: 1260
	public class DerSetParser : Asn1SetParser, IAsn1Convertible
	{
		// Token: 0x06002E5B RID: 11867 RVA: 0x000F1A24 File Offset: 0x000EFC24
		internal DerSetParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E5C RID: 11868 RVA: 0x000F1A33 File Offset: 0x000EFC33
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06002E5D RID: 11869 RVA: 0x000F1A40 File Offset: 0x000EFC40
		public Asn1Object ToAsn1Object()
		{
			return new DerSet(this._parser.ReadVector(), false);
		}

		// Token: 0x04001E26 RID: 7718
		private readonly Asn1StreamParser _parser;
	}
}
