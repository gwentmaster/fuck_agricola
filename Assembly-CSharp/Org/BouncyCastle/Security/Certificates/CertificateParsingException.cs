using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002C2 RID: 706
	[Serializable]
	public class CertificateParsingException : CertificateException
	{
		// Token: 0x06001725 RID: 5925 RVA: 0x00084825 File Offset: 0x00082A25
		public CertificateParsingException()
		{
		}

		// Token: 0x06001726 RID: 5926 RVA: 0x0008482D File Offset: 0x00082A2D
		public CertificateParsingException(string message) : base(message)
		{
		}

		// Token: 0x06001727 RID: 5927 RVA: 0x00084836 File Offset: 0x00082A36
		public CertificateParsingException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
