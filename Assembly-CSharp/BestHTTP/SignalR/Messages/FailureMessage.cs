using System;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C8 RID: 1480
	public sealed class FailureMessage : IServerMessage, IHubMessage
	{
		// Token: 0x1700070B RID: 1803
		// (get) Token: 0x0600365D RID: 13917 RVA: 0x000CC364 File Offset: 0x000CA564
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Failure;
			}
		}

		// Token: 0x1700070C RID: 1804
		// (get) Token: 0x0600365E RID: 13918 RVA: 0x0010EBC8 File Offset: 0x0010CDC8
		// (set) Token: 0x0600365F RID: 13919 RVA: 0x0010EBD0 File Offset: 0x0010CDD0
		public ulong InvocationId { get; private set; }

		// Token: 0x1700070D RID: 1805
		// (get) Token: 0x06003660 RID: 13920 RVA: 0x0010EBD9 File Offset: 0x0010CDD9
		// (set) Token: 0x06003661 RID: 13921 RVA: 0x0010EBE1 File Offset: 0x0010CDE1
		public bool IsHubError { get; private set; }

		// Token: 0x1700070E RID: 1806
		// (get) Token: 0x06003662 RID: 13922 RVA: 0x0010EBEA File Offset: 0x0010CDEA
		// (set) Token: 0x06003663 RID: 13923 RVA: 0x0010EBF2 File Offset: 0x0010CDF2
		public string ErrorMessage { get; private set; }

		// Token: 0x1700070F RID: 1807
		// (get) Token: 0x06003664 RID: 13924 RVA: 0x0010EBFB File Offset: 0x0010CDFB
		// (set) Token: 0x06003665 RID: 13925 RVA: 0x0010EC03 File Offset: 0x0010CE03
		public IDictionary<string, object> AdditionalData { get; private set; }

		// Token: 0x17000710 RID: 1808
		// (get) Token: 0x06003666 RID: 13926 RVA: 0x0010EC0C File Offset: 0x0010CE0C
		// (set) Token: 0x06003667 RID: 13927 RVA: 0x0010EC14 File Offset: 0x0010CE14
		public string StackTrace { get; private set; }

		// Token: 0x17000711 RID: 1809
		// (get) Token: 0x06003668 RID: 13928 RVA: 0x0010EC1D File Offset: 0x0010CE1D
		// (set) Token: 0x06003669 RID: 13929 RVA: 0x0010EC25 File Offset: 0x0010CE25
		public IDictionary<string, object> State { get; private set; }

		// Token: 0x0600366A RID: 13930 RVA: 0x0010EC30 File Offset: 0x0010CE30
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.InvocationId = ulong.Parse(dictionary["I"].ToString());
			object obj;
			if (dictionary.TryGetValue("E", out obj))
			{
				this.ErrorMessage = obj.ToString();
			}
			if (dictionary.TryGetValue("H", out obj))
			{
				this.IsHubError = (int.Parse(obj.ToString()) == 1);
			}
			if (dictionary.TryGetValue("D", out obj))
			{
				this.AdditionalData = (obj as IDictionary<string, object>);
			}
			if (dictionary.TryGetValue("T", out obj))
			{
				this.StackTrace = obj.ToString();
			}
			if (dictionary.TryGetValue("S", out obj))
			{
				this.State = (obj as IDictionary<string, object>);
			}
		}
	}
}
