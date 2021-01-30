using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003C7 RID: 967
	public abstract class NameType
	{
		// Token: 0x060023B6 RID: 9142 RVA: 0x000B7D9A File Offset: 0x000B5F9A
		public static bool IsValid(byte nameType)
		{
			return nameType == 0;
		}

		// Token: 0x040018B0 RID: 6320
		public const byte host_name = 0;
	}
}
