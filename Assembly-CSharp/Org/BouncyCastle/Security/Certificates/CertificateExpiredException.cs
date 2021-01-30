using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002C0 RID: 704
	[Serializable]
	public class CertificateExpiredException : CertificateException
	{
		// Token: 0x0600171F RID: 5919 RVA: 0x00084825 File Offset: 0x00082A25
		public CertificateExpiredException()
		{
		}

		// Token: 0x06001720 RID: 5920 RVA: 0x0008482D File Offset: 0x00082A2D
		public CertificateExpiredException(string message) : base(message)
		{
		}

		// Token: 0x06001721 RID: 5921 RVA: 0x00084836 File Offset: 0x00082A36
		public CertificateExpiredException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
