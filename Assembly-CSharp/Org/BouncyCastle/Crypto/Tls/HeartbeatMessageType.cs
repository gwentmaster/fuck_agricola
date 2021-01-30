using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x020003BE RID: 958
	public abstract class HeartbeatMessageType
	{
		// Token: 0x060023A7 RID: 9127 RVA: 0x000B7BC0 File Offset: 0x000B5DC0
		public static bool IsValid(byte heartbeatMessageType)
		{
			return heartbeatMessageType >= 1 && heartbeatMessageType <= 2;
		}

		// Token: 0x04001881 RID: 6273
		public const byte heartbeat_request = 1;

		// Token: 0x04001882 RID: 6274
		public const byte heartbeat_response = 2;
	}
}
