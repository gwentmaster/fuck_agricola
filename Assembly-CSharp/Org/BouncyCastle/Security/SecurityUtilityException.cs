using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002BB RID: 699
	[Serializable]
	public class SecurityUtilityException : Exception
	{
		// Token: 0x06001709 RID: 5897 RVA: 0x000735AD File Offset: 0x000717AD
		public SecurityUtilityException()
		{
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00073619 File Offset: 0x00071819
		public SecurityUtilityException(string message) : base(message)
		{
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00073622 File Offset: 0x00071822
		public SecurityUtilityException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
