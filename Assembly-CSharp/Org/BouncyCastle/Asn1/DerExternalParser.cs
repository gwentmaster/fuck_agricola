using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E7 RID: 1255
	public class DerExternalParser : Asn1Encodable
	{
		// Token: 0x06002E47 RID: 11847 RVA: 0x000F17E2 File Offset: 0x000EF9E2
		public DerExternalParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002E48 RID: 11848 RVA: 0x000F17F1 File Offset: 0x000EF9F1
		public IAsn1Convertible ReadObject()
		{
			return this._parser.ReadObject();
		}

		// Token: 0x06002E49 RID: 11849 RVA: 0x000F17FE File Offset: 0x000EF9FE
		public override Asn1Object ToAsn1Object()
		{
			return new DerExternal(this._parser.ReadVector());
		}

		// Token: 0x04001E1F RID: 7711
		private readonly Asn1StreamParser _parser;
	}
}
