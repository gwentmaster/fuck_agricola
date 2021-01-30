using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C6 RID: 1478
	public sealed class MethodCallMessage : IServerMessage
	{
		// Token: 0x17000702 RID: 1794
		// (get) Token: 0x06003649 RID: 13897 RVA: 0x000AE7B9 File Offset: 0x000AC9B9
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.MethodCall;
			}
		}

		// Token: 0x17000703 RID: 1795
		// (get) Token: 0x0600364A RID: 13898 RVA: 0x0010EA2A File Offset: 0x0010CC2A
		// (set) Token: 0x0600364B RID: 13899 RVA: 0x0010EA32 File Offset: 0x0010CC32
		public string Hub { get; private set; }

		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x0600364C RID: 13900 RVA: 0x0010EA3B File Offset: 0x0010CC3B
		// (set) Token: 0x0600364D RID: 13901 RVA: 0x0010EA43 File Offset: 0x0010CC43
		public string Method { get; private set; }

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x0600364E RID: 13902 RVA: 0x0010EA4C File Offset: 0x0010CC4C
		// (set) Token: 0x0600364F RID: 13903 RVA: 0x0010EA54 File Offset: 0x0010CC54
		public object[] Arguments { get; private set; }

		// Token: 0x17000706 RID: 1798
		// (get) Token: 0x06003650 RID: 13904 RVA: 0x0010EA5D File Offset: 0x0010CC5D
		// (set) Token: 0x06003651 RID: 13905 RVA: 0x0010EA65 File Offset: 0x0010CC65
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x06003652 RID: 13906 RVA: 0x0010EA70 File Offset: 0x0010CC70
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.Hub = dictionary["H"].ToString();
			this.Method = dictionary["M"].ToString();
			List<object> list = new List<object>();
			foreach (object item in (dictionary["A"] as IEnumerable))
			{
				list.Add(item);
			}
			this.Arguments = list.ToArray();
			object obj;
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
