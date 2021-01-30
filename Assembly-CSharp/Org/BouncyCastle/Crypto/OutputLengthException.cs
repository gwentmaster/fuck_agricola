using System;

namespace Org.BouncyCastle.Crypto
{
	// Token: 0x0200038C RID: 908
	[Serializable]
	public class OutputLengthException : DataLengthException
	{
		// Token: 0x06002235 RID: 8757 RVA: 0x000B51E6 File Offset: 0x000B33E6
		public OutputLengthException()
		{
		}

		// Token: 0x06002236 RID: 8758 RVA: 0x000B51EE File Offset: 0x000B33EE
		public OutputLengthException(string message) : base(message)
		{
		}

		// Token: 0x06002237 RID: 8759 RVA: 0x000B51F7 File Offset: 0x000B33F7
		public OutputLengthException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
