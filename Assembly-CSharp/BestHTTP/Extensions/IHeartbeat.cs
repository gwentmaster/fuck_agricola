using System;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F4 RID: 1524
	public interface IHeartbeat
	{
		// Token: 0x060037F1 RID: 14321
		void OnHeartbeatUpdate(TimeSpan dif);
	}
}
