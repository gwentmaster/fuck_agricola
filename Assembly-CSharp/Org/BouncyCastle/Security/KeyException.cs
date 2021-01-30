using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002B7 RID: 695
	[Serializable]
	public class KeyException : GeneralSecurityException
	{
		// Token: 0x060016E2 RID: 5858 RVA: 0x000823D5 File Offset: 0x000805D5
		public KeyException()
		{
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x000823DD File Offset: 0x000805DD
		public KeyException(string message) : base(message)
		{
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x000823E6 File Offset: 0x000805E6
		public KeyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
