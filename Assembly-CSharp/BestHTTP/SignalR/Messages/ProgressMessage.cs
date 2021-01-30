using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C9 RID: 1481
	public sealed class ProgressMessage : IServerMessage, IHubMessage
	{
		// Token: 0x17000712 RID: 1810
		// (get) Token: 0x0600366C RID: 13932 RVA: 0x000A85D1 File Offset: 0x000A67D1
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Progress;
			}
		}

		// Token: 0x17000713 RID: 1811
		// (get) Token: 0x0600366D RID: 13933 RVA: 0x0010ECF2 File Offset: 0x0010CEF2
		// (set) Token: 0x0600366E RID: 13934 RVA: 0x0010ECFA File Offset: 0x0010CEFA
		public ulong InvocationId { get; private set; }

		// Token: 0x17000714 RID: 1812
		// (get) Token: 0x0600366F RID: 13935 RVA: 0x0010ED03 File Offset: 0x0010CF03
		// (set) Token: 0x06003670 RID: 13936 RVA: 0x0010ED0B File Offset: 0x0010CF0B
		public double Progress { get; private set; }

		// Token: 0x06003671 RID: 13937 RVA: 0x0010ED14 File Offset: 0x0010CF14
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = (data as IDictionary<string, object>)["P"] as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			this.Progress = double.Parse(dictionary["D"].ToString());
		}
	}
}
