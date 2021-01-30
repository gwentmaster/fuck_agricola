using System;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004D1 RID: 1233
	[Serializable]
	public class Asn1ParsingException : InvalidOperationException
	{
		// Token: 0x06002DBC RID: 11708 RVA: 0x000F016C File Offset: 0x000EE36C
		public Asn1ParsingException()
		{
		}

		// Token: 0x06002DBD RID: 11709 RVA: 0x000F0174 File Offset: 0x000EE374
		public Asn1ParsingException(string message) : base(message)
		{
		}

		// Token: 0x06002DBE RID: 11710 RVA: 0x000F017D File Offset: 0x000EE37D
		public Asn1ParsingException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
