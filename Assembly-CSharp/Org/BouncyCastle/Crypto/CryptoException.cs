using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000371 RID: 881
	[Serializable]
	public class CryptoException : Exception
	{
		// Token: 0x060021CF RID: 8655 RVA: 0x000735AD File Offset: 0x000717AD
		public CryptoException()
		{
		}

		// Token: 0x060021D0 RID: 8656 RVA: 0x00073619 File Offset: 0x00071819
		public CryptoException(string message) : base(message)
		{
		}

		// Token: 0x060021D1 RID: 8657 RVA: 0x00073622 File Offset: 0x00071822
		public CryptoException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
