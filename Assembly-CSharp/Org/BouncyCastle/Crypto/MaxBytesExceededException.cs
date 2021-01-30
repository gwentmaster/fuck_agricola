using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200038B RID: 907
	[Serializable]
	public class MaxBytesExceededException : CryptoException
	{
		// Token: 0x06002232 RID: 8754 RVA: 0x000B5183 File Offset: 0x000B3383
		public MaxBytesExceededException()
		{
		}

		// Token: 0x06002233 RID: 8755 RVA: 0x000B518B File Offset: 0x000B338B
		public MaxBytesExceededException(string message) : base(message)
		{
		}

		// Token: 0x06002234 RID: 8756 RVA: 0x000B5194 File Offset: 0x000B3394
		public MaxBytesExceededException(string message, Exception e) : base(message, e)
		{
		}
	}
}
