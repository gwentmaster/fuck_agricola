using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004EA RID: 1258
	public class DerSequenceParser : Asn1SequenceParser, IAsn1Convertible
	{
		// Token: 0x06002E53 RID: 11859 RVA: 0x000F199C File Offset: 0x000EFB9C
		internal DerSequenceParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E54 RID: 11860 RVA: 0x000F19AB File Offset: 0x000EFBAB
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06002E55 RID: 11861 RVA: 0x000F19B8 File Offset: 0x000EFBB8
		public Asn1Object ToAsn1Object()
		{
			return new DerSequence(this._parser.ReadVector());
		}

		// Token: 0x04001E24 RID: 7716
		private readonly Asn1StreamParser _parser;
	}
}
