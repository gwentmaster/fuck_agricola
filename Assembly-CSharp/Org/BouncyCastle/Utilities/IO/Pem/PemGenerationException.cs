using System;

namespace Org.BouncyCastle.Utilities.IO.Pem
{
	// Token: 0x020002A0 RID: 672
	[Serializable]
	public class PemGenerationException : Exception
	{
		// Token: 0x0600165B RID: 5723 RVA: 0x000735AD File Offset: 0x000717AD
		public PemGenerationException()
		{
		}

		// Token: 0x0600165C RID: 5724 RVA: 0x00073619 File Offset: 0x00071819
		public PemGenerationException(string message) : base(message)
		{
		}

		// Token: 0x0600165D RID: 5725 RVA: 0x00073622 File Offset: 0x00071822
		public PemGenerationException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
