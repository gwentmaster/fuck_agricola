using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C7 RID: 1479
	public sealed class ResultMessage : IServerMessage, IHubMessage
	{
		// Token: 0x17000707 RID: 1799
		// (get) Token: 0x06003654 RID: 13908 RVA: 0x000A6D3D File Offset: 0x000A4F3D
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Result;
			}
		}

		// Token: 0x17000708 RID: 1800
		// (get) Token: 0x06003655 RID: 13909 RVA: 0x0010EB34 File Offset: 0x0010CD34
		// (set) Token: 0x06003656 RID: 13910 RVA: 0x0010EB3C File Offset: 0x0010CD3C
		public ulong InvocationId { get; private set; }

		// Token: 0x17000709 RID: 1801
		// (get) Token: 0x06003657 RID: 13911 RVA: 0x0010EB45 File Offset: 0x0010CD45
		// (set) Token: 0x06003658 RID: 13912 RVA: 0x0010EB4D File Offset: 0x0010CD4D
		public object ReturnValue { get; private set; }

		// Token: 0x1700070A RID: 1802
		// (get) Token: 0x06003659 RID: 13913 RVA: 0x0010EB56 File Offset: 0x0010CD56
		// (set) Token: 0x0600365A RID: 13914 RVA: 0x0010EB5E File Offset: 0x0010CD5E
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x0600365B RID: 13915 RVA: 0x0010EB68 File Offset: 0x0010CD68
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			object obj;
			if (dictionary.TryGetValue("R", out obj))
			{
				this.ReturnValue = obj;
			}
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
