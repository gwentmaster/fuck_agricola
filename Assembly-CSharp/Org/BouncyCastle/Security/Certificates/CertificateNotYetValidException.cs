using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002C1 RID: 705
	[Serializable]
	public class CertificateNotYetValidException : CertificateException
	{
		// Token: 0x06001722 RID: 5922 RVA: 0x00084825 File Offset: 0x00082A25
		public CertificateNotYetValidException()
		{
		}

		// Token: 0x06001723 RID: 5923 RVA: 0x0008482D File Offset: 0x00082A2D
		public CertificateNotYetValidException(string message) : base(message)
		{
		}

		// Token: 0x06001724 RID: 5924 RVA: 0x00084836 File Offset: 0x00082A36
		public CertificateNotYetValidException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
