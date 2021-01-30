using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004DD RID: 1245
	public class BerTaggedObjectParser : Asn1TaggedObjectParser, IAsn1Convertible
	{
		// Token: 0x06002E08 RID: 11784 RVA: 0x000F0CE1 File Offset: 0x000EEEE1
		[Obsolete]
		internal BerTaggedObjectParser(int baseTag, int tagNumber, Stream contentStream) : this((baseTag & 32) != 0, tagNumber, new Asn1StreamParser(contentStream))
		{
		}

		// Token: 0x06002E09 RID: 11785 RVA: 0x000F0CF7 File Offset: 0x000EEEF7
		internal BerTaggedObjectParser(bool constructed, int tagNumber, Asn1StreamParser parser)
		{
			this._constructed = constructed;
			this._tagNumber = tagNumber;
			this._parser = parser;
		}

		// Token: 0x1700053D RID: 1341
		// (get) Token: 0x06002E0A RID: 11786 RVA: 0x000F0D14 File Offset: 0x000EEF14
		public bool IsConstructed
		{
			get
			{
				return this._constructed;
			}
		}

		// Token: 0x1700053E RID: 1342
		// (get) Token: 0x06002E0B RID: 11787 RVA: 0x000F0D1C File Offset: 0x000EEF1C
		public int TagNo
		{
			get
			{
				return this._tagNumber;
			}
		}

		// Token: 0x06002E0C RID: 11788 RVA: 0x000F0D24 File Offset: 0x000EEF24
		public IAsn1Convertible GetObjectParser(int tag, bool isExplicit)
		{
			if (!isExplicit)
			{
				return this._parser.ReadImplicit(this._constructed, tag);
			}
			if (!this._constructed)
			{
				throw new IOException("Explicit tags must be constructed (see X.690 8.14.2)");
			}
			return this._parser.ReadObject();
		}

		// Token: 0x06002E0D RID: 11789 RVA: 0x000F0D5C File Offset: 0x000EEF5C
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = this._parser.ReadTaggedObject(this._constructed, this._tagNumber);
			}
			catch (IOException ex)
			{
				throw new Asn1ParsingException(ex.Message);
			}
			return result;
		}

		// Token: 0x04001E0E RID: 7694
		private bool _constructed;

		// Token: 0x04001E0F RID: 7695
		private int _tagNumber;

		// Token: 0x04001E10 RID: 7696
		private Asn1StreamParser _parser;
	}
}
