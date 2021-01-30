using System;

namespace Org.BouncyCastle.Security.Certificates
{
	// Token: 0x020002C3 RID: 707
	[Serializable]
	public class CrlException : GeneralSecurityException
	{
		// Token: 0x06001728 RID: 5928 RVA: 0x000823D5 File Offset: 0x000805D5
		public CrlException()
		{
		}

		// Token: 0x06001729 RID: 5929 RVA: 0x000823DD File Offset: 0x000805DD
		public CrlException(string msg) : base(msg)
		{
		}

		// Token: 0x0600172A RID: 5930 RVA: 0x000823E6 File Offset: 0x000805E6
		public CrlException(string msg, Exception e) : base(msg, e)
		{
		}
	}
}
