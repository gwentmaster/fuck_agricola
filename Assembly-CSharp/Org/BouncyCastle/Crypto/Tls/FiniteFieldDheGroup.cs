using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BA RID: 954
	public abstract class FiniteFieldDheGroup
	{
		// Token: 0x0600239C RID: 9116 RVA: 0x000B7BE4 File Offset: 0x000B5DE4
		public static bool IsValid(byte group)
		{
			return group >= 0 && group <= 4;
		}

		// Token: 0x04001865 RID: 6245
		public const byte ffdhe2432 = 0;

		// Token: 0x04001866 RID: 6246
		public const byte ffdhe3072 = 1;

		// Token: 0x04001867 RID: 6247
		public const byte ffdhe4096 = 2;

		// Token: 0x04001868 RID: 6248
		public const byte ffdhe6144 = 3;

		// Token: 0x04001869 RID: 6249
		public const byte ffdhe8192 = 4;
	}
}
