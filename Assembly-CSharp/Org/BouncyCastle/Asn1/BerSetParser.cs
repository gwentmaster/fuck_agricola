using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004DC RID: 1244
	public class BerSetParser : Asn1SetParser, IAsn1Convertible
	{
		// Token: 0x06002E05 RID: 11781 RVA: 0x000F0CB2 File Offset: 0x000EEEB2
		internal BerSetParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E06 RID: 11782 RVA: 0x000F0CC1 File Offset: 0x000EEEC1
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06002E07 RID: 11783 RVA: 0x000F0CCE File Offset: 0x000EEECE
		public Asn1Object ToAsn1Object()
		{
			return new BerSet(this._parser.ReadVector(), false);
		}

		// Token: 0x04001E0D RID: 7693
		private readonly Asn1StreamParser _parser;
	}
}
