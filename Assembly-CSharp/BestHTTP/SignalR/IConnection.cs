using System;
using BestHTTP.SignalR.JsonEncoders;
using BestHTTP.SignalR.Messages;
using BestHTTP.SignalR.Transports;

namespace BestHTTP.SignalR
{
	// Token: 0x020005B1 RID: 1457
	public interface IConnection
	{
		// Token: 0x170006CA RID: 1738
		// (get) Token: 0x06003570 RID: 13680
		ProtocolVersions Protocol { get; }

		// Token: 0x170006CB RID: 1739
		// (get) Token: 0x06003571 RID: 13681
		NegotiationData NegotiationResult { get; }

		// Token: 0x170006CC RID: 1740
		// (get) Token: 0x06003572 RID: 13682
		// (set) Token: 0x06003573 RID: 13683
		IJsonEncoder JsonEncoder { get; set; }

		// Token: 0x06003574 RID: 13684
		void OnMessage(IServerMessage msg);

		// Token: 0x06003575 RID: 13685
		void TransportStarted();

		// Token: 0x06003576 RID: 13686
		void TransportReconnected();

		// Token: 0x06003577 RID: 13687
		void TransportAborted();

		// Token: 0x06003578 RID: 13688
		void Error(string reason);

		// Token: 0x06003579 RID: 13689
		Uri BuildUri(RequestTypes type);

		// Token: 0x0600357A RID: 13690
		Uri BuildUri(RequestTypes type, TransportBase transport);

		// Token: 0x0600357B RID: 13691
		HTTPRequest PrepareRequest(HTTPRequest req, RequestTypes type);

		// Token: 0x0600357C RID: 13692
		string ParseResponse(string responseStr);
	}
}
