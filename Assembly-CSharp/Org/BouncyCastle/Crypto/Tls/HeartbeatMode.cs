using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BF RID: 959
	public abstract class HeartbeatMode
	{
		// Token: 0x060023A9 RID: 9129 RVA: 0x000B7BC0 File Offset: 0x000B5DC0
		public static bool IsValid(byte heartbeatMode)
		{
			return heartbeatMode >= 1 && heartbeatMode <= 2;
		}

		// Token: 0x04001883 RID: 6275
		public const byte peer_allowed_to_send = 1;

		// Token: 0x04001884 RID: 6276
		public const byte peer_not_allowed_to_send = 2;
	}
}
