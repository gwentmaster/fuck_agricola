using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002BC RID: 700
	[Serializable]
	public class SignatureException : GeneralSecurityException
	{
		// Token: 0x0600170C RID: 5900 RVA: 0x000823D5 File Offset: 0x000805D5
		public SignatureException()
		{
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x000823DD File Offset: 0x000805DD
		public SignatureException(string message) : base(message)
		{
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x000823E6 File Offset: 0x000805E6
		public SignatureException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
