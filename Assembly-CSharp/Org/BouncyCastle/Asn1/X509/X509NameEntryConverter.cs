using System;
using Org.BouncyCastle.Utilities.Encoders;

namespace Org.BouncyCastle.Asn1.X509
{
	// Token: 0x0200053A RID: 1338
	public abstract class X509NameEntryConverter
	{
		// Token: 0x060030EB RID: 12523 RVA: 0x000FA6CE File Offset: 0x000F88CE
		protected Asn1Object ConvertHexEncoded(string hexString, int offset)
		{
			return Asn1Object.FromByteArray(Hex.Decode(hexString.Substring(offset)));
		}

		// Token: 0x060030EC RID: 12524 RVA: 0x000FA6E1 File Offset: 0x000F88E1
		protected bool CanBePrintable(string str)
		{
			return DerPrintableString.IsPrintableString(str);
		}

		// Token: 0x060030ED RID: 12525
		public abstract Asn1Object GetConvertedValue(DerObjectIdentifier oid, string value);
	}
}
