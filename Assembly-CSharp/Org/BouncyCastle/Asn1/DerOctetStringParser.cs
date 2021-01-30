using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004E9 RID: 1257
	public class DerOctetStringParser : Asn1OctetStringParser, IAsn1Convertible
	{
		// Token: 0x06002E50 RID: 11856 RVA: 0x000F1938 File Offset: 0x000EFB38
		internal DerOctetStringParser(DefiniteLengthInputStream stream)
		{
			this.stream = stream;
		}

		// Token: 0x06002E51 RID: 11857 RVA: 0x000F1947 File Offset: 0x000EFB47
		public Stream GetOctetStream()
		{
			return this.stream;
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x000F1950 File Offset: 0x000EFB50
		public Asn1Object ToAsn1Object()
		{
			Asn1Object result;
			try
			{
				result = new DerOctetString(this.stream.ToArray());
			}
			catch (IOException ex)
			{
				throw new InvalidOperationException("IOException converting stream to byte array: " + ex.Message, ex);
			}
			return result;
		}

		// Token: 0x04001E23 RID: 7715
		private readonly DefiniteLengthInputStream stream;
	}
}
