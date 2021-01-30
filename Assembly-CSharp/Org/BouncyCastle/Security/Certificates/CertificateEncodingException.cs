using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002BE RID: 702
	[Serializable]
	public class CertificateEncodingException : CertificateException
	{
		// Token: 0x06001719 RID: 5913 RVA: 0x00084825 File Offset: 0x00082A25
		public CertificateEncodingException()
		{
		}

		// Token: 0x0600171A RID: 5914 RVA: 0x0008482D File Offset: 0x00082A2D
		public CertificateEncodingException(string msg) : base(msg)
		{
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00084836 File Offset: 0x00082A36
		public CertificateEncodingException(string msg, Exception e) : base(msg, e)
		{
		}
	}
}
