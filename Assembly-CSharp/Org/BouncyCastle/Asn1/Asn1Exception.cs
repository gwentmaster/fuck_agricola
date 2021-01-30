using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
	// Token: 0x020004CB RID: 1227
	[Serializable]
	public class Asn1Exception : IOException
	{
		// Token: 0x06002D94 RID: 11668 RVA: 0x00080662 File Offset: 0x0007E862
		public Asn1Exception()
		{
		}

		// Token: 0x06002D95 RID: 11669 RVA: 0x0008066A File Offset: 0x0007E86A
		public Asn1Exception(string message) : base(message)
		{
		}

		// Token: 0x06002D96 RID: 11670 RVA: 0x00080673 File Offset: 0x0007E873
		public Asn1Exception(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
