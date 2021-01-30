using System;

namespace Org.BouncyCastle.Security
{
	// Token: 0x020002B5 RID: 693
	[Serializable]
	public class InvalidKeyException : KeyException
	{
		// Token: 0x060016DC RID: 5852 RVA: 0x000823BA File Offset: 0x000805BA
		public InvalidKeyException()
		{
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x000823C2 File Offset: 0x000805C2
		public InvalidKeyException(string message) : base(message)
		{
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x000823CB File Offset: 0x000805CB
		public InvalidKeyException(string message, Exception exception) : base(message, exception)
		{
		}
	}
}
