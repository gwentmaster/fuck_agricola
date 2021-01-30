using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000389 RID: 905
	[Serializable]
	public class InvalidCipherTextException : CryptoException
	{
		// Token: 0x0600222C RID: 8748 RVA: 0x000B5183 File Offset: 0x000B3383
		public InvalidCipherTextException()
		{
		}

		// Token: 0x0600222D RID: 8749 RVA: 0x000B518B File Offset: 0x000B338B
		public InvalidCipherTextException(string message) : base(message)
		{
		}

		// Token: 0x0600222E RID: 8750 RVA: 0x000B5194 File Offset: 0x000B3394
		public InvalidCipherTextException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
