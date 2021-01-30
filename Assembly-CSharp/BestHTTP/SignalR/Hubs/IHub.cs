using System;
using BestHTTP.SignalR.Messages;

namespace BestHTTP.SignalR.Hubs
{
	// Token: 0x020005D3 RID: 1491
	public interface IHub
	{
		// Token: 0x17000718 RID: 1816
		// (get) Token: 0x060036A6 RID: 13990
		// (set) Token: 0x060036A7 RID: 13991
		Connection Connection { get; set; }

		// Token: 0x060036A8 RID: 13992
		bool Call(ClientMessage msg);

		// Token: 0x060036A9 RID: 13993
		bool HasSentMessageId(ulong id);

		// Token: 0x060036AA RID: 13994
		void Close();

		// Token: 0x060036AB RID: 13995
		void OnMethod(MethodCallMessage msg);

		// Token: 0x060036AC RID: 13996
		void OnMessage(IServerMessage msg);
	}
}
