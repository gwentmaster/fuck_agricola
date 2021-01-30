using System;
using System.IO;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D8 RID: 1240
	public class BerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06002DFB RID: 11771 RVA: 0x000F0BD1 File Offset: 0x000EEDD1
		internal BerOctetStringParser(Asn1StreamParser parser)
		{
			this._parser = parser;
		}

		// Token: 0x06002DFC RID: 11772 RVA: 0x000F0BE0 File Offset: 0x000EEDE0
		public Stream GetOctetStream()
		{
			return new ConstructedOctetStream(this._parser);
		}

		// Token: 0x06002DFD RID: 11773 RVA: 0x000F0BF0 File Offset: 0x000EEDF0
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new BerOctetString(Streams.ReadAll(this.GetOctetStream()));
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x04001E0B RID: 7691
		private readonly Asn1StreamParser _parser;
	}
}
