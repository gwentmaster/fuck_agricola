using System;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C5 RID: 1477
	public sealed class DataMessage : IServerMessage
	{
		// Token: 0x17000700 RID: 1792
		// (get) Token: 0x06003644 RID: 13892 RVA: 0x0000900B File Offset: 0x0000720B
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Data;
			}
		}

		// Token: 0x17000701 RID: 1793
		// (get) Token: 0x06003645 RID: 13893 RVA: 0x0010EA10 File Offset: 0x0010CC10
		// (set) Token: 0x06003646 RID: 13894 RVA: 0x0010EA18 File Offset: 0x0010CC18
		public object Data { get; private set; }

		// Token: 0x06003647 RID: 13895 RVA: 0x0010EA21 File Offset: 0x0010CC21
		void IServerMessage.Parse(object data)
		{
			this.Data = data;
		}
	}
}
