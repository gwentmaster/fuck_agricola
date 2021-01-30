using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002B4 RID: 692
	[Serializable]
	public class GeneralSecurityException : Exception
	{
		// Token: 0x060016D9 RID: 5849 RVA: 0x000735AD File Offset: 0x000717AD
		public GeneralSecurityException()
		{
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00073619 File Offset: 0x00071819
		public GeneralSecurityException(string message) : base(message)
		{
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00073622 File Offset: 0x00071822
		public GeneralSecurityException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
