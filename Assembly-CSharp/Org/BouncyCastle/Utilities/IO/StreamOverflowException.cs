using System;
using System.IO;

namespace Org.BouncyCastle.Utilities.IO
{
	// Token: 0x0200029C RID: 668
	[Serializable]
	public class StreamOverflowException : IOException
	{
		// Token: 0x06001648 RID: 5704 RVA: 0x00080662 File Offset: 0x0007E862
		public StreamOverflowException()
		{
		}

		// Token: 0x06001649 RID: 5705 RVA: 0x0008066A File Offset: 0x0007E86A
		public StreamOverflowException(string message) : base(message)
		{
		}

		// Token: 0x0600164A RID: 5706 RVA: 0x00080673 File Offset: 0x0007E873
		public StreamOverflowException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
