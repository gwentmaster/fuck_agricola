using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x02000372 RID: 882
	[Serializable]
	public class DataLengthException : CryptoException
	{
		// Token: 0x060021D2 RID: 8658 RVA: 0x000B5183 File Offset: 0x000B3383
		public DataLengthException()
		{
		}

		// Token: 0x060021D3 RID: 8659 RVA: 0x000B518B File Offset: 0x000B338B
		public DataLengthException(string message) : base(message)
		{
		}

		// Token: 0x060021D4 RID: 8660 RVA: 0x000B5194 File Offset: 0x000B3394
		public DataLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
