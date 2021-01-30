using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C1 RID: 1473
	public interface IServerMessage
	{
		// Token: 0x170006F6 RID: 1782
		// (get) Token: 0x0600362F RID: 13871
		MessageTypes Type { get; }

		// Token: 0x06003630 RID: 13872
		void Parse(object data);
	}
}
