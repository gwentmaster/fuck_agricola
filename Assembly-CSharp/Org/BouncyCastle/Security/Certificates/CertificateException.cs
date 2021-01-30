using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002BF RID: 703
	[Serializable]
	public class CertificateException : GeneralSecurityException
	{
		// Token: 0x0600171C RID: 5916 RVA: 0x000823D5 File Offset: 0x000805D5
		public CertificateException()
		{
		}

		// Token: 0x0600171D RID: 5917 RVA: 0x000823DD File Offset: 0x000805DD
		public CertificateException(string message) : base(message)
		{
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x000823E6 File Offset: 0x000805E6
		public CertificateException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
