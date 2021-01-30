using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002B6 RID: 694
	[Serializable]
	public class InvalidParameterException : KeyException
	{
		// Token: 0x060016DF RID: 5855 RVA: 0x000823BA File Offset: 0x000805BA
		public InvalidParameterException()
		{
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x000823C2 File Offset: 0x000805C2
		public InvalidParameterException(string message) : base(message)
		{
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x000823CB File Offset: 0x000805CB
		public InvalidParameterException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
