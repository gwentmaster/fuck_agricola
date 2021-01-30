using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004DA RID: 1242
	public class BerSequenceParser : Asn1SequenceParser, IAsn1Convertible
	{
		// Token: 0x06002E00 RID: 11776 RVA: 0x000F0C60 File Offset: 0x000EEE60
		internal BerSequenceParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E01 RID: 11777 RVA: 0x000F0C6F File Offset: 0x000EEE6F
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06002E02 RID: 11778 RVA: 0x000F0C7C File Offset: 0x000EEE7C
		public Asn1Object ToAsn1Object()
		{
			return new BerSequence(this._parser.ReadVector());
		}

		// Token: 0x04001E0C RID: 7692
		private readonly Asn1StreamParser _parser;
	}
}
