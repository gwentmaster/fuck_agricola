using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C3 RID: 1475
	public sealed class KeepAliveMessage : IServerMessage
	{
		// Token: 0x170006F8 RID: 1784
		// (get) Token: 0x06003632 RID: 13874 RVA: 0x0002A062 File Offset: 0x00028262
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.KeepAlive;
			}
		}

		// Token: 0x06003633 RID: 13875 RVA: 0x00003022 File Offset: 0x00001222
		void IServerMessage.Parse(object data)
		{
		}
	}
}
