using System;
using System.Collections;
using System.Collections.Generic;

namespace BestHTTP.SignalR.Messages
{
	// Token: 0x020005C4 RID: 1476
	public sealed class MultiMessage : IServerMessage
	{
		// Token: 0x170006F9 RID: 1785
		// (get) Token: 0x06003635 RID: 13877 RVA: 0x000A5319 File Offset: 0x000A3519
		MessageTypes IServerMessage.Type
		{
			get
			{
				return MessageTypes.Multiple;
			}
		}

		// Token: 0x170006FA RID: 1786
		// (get) Token: 0x06003636 RID: 13878 RVA: 0x0010E815 File Offset: 0x0010CA15
		// (set) Token: 0x06003637 RID: 13879 RVA: 0x0010E81D File Offset: 0x0010CA1D
		public string MessageId { get; private set; }

		// Token: 0x170006FB RID: 1787
		// (get) Token: 0x06003638 RID: 13880 RVA: 0x0010E826 File Offset: 0x0010CA26
		// (set) Token: 0x06003639 RID: 13881 RVA: 0x0010E82E File Offset: 0x0010CA2E
		public bool IsInitialization { get; private set; }

		// Token: 0x170006FC RID: 1788
		// (get) Token: 0x0600363A RID: 13882 RVA: 0x0010E837 File Offset: 0x0010CA37
		// (set) Token: 0x0600363B RID: 13883 RVA: 0x0010E83F File Offset: 0x0010CA3F
		public string GroupsToken { get; private set; }

		// Token: 0x170006FD RID: 1789
		// (get) Token: 0x0600363C RID: 13884 RVA: 0x0010E848 File Offset: 0x0010CA48
		// (set) Token: 0x0600363D RID: 13885 RVA: 0x0010E850 File Offset: 0x0010CA50
		public bool ShouldReconnect { get; private set; }

		// Token: 0x170006FE RID: 1790
		// (get) Token: 0x0600363E RID: 13886 RVA: 0x0010E859 File Offset: 0x0010CA59
		// (set) Token: 0x0600363F RID: 13887 RVA: 0x0010E861 File Offset: 0x0010CA61
		public TimeSpan? PollDelay { get; private set; }

		// Token: 0x170006FF RID: 1791
		// (get) Token: 0x06003640 RID: 13888 RVA: 0x0010E86A File Offset: 0x0010CA6A
		// (set) Token: 0x06003641 RID: 13889 RVA: 0x0010E872 File Offset: 0x0010CA72
		public List<IServerMessage> Data { get; private set; }

		// Token: 0x06003642 RID: 13890 RVA: 0x0010E87C File Offset: 0x0010CA7C
		void IServerMessage.Parse(object data)
		{
			IDictionary<string, object> dictionary = data as IDictionary<string, object>;
			this.MessageId = dictionary["C"].ToString();
			object obj;
			if (dictionary.TryGetValue("S", out obj))
			{
				this.IsInitialization = (int.Parse(obj.ToString()) == 1);
			}
			else
			{
				this.IsInitialization = false;
			}
			if (dictionary.TryGetValue("G", out obj))
			{
				this.GroupsToken = obj.ToString();
			}
			if (dictionary.TryGetValue("T", out obj))
			{
				this.ShouldReconnect = (int.Parse(obj.ToString()) == 1);
			}
			else
			{
				this.ShouldReconnect = false;
			}
			if (dictionary.TryGetValue("L", out obj))
			{
				this.PollDelay = new TimeSpan?(TimeSpan.FromMilliseconds(double.Parse(obj.ToString())));
			}
			IEnumerable enumerable = dictionary["M"] as IEnumerable;
			if (enumerable != null)
			{
				this.Data = new List<IServerMessage>();
				foreach (object obj2 in enumerable)
				{
					IDictionary<string, object> dictionary2 = obj2 as IDictionary<string, object>;
					IServerMessage serverMessage;
					if (dictionary2 != null)
					{
						if (dictionary2.ContainsKey("H"))
						{
							serverMessage = new MethodCallMessage();
						}
						else if (dictionary2.ContainsKey("I"))
						{
							serverMessage = new ProgressMessage();
						}
						else
						{
							serverMessage = new DataMessage();
						}
					}
					else
					{
						serverMessage = new DataMessage();
					}
					serverMessage.Parse(obj2);
					this.Data.Add(serverMessage);
				}
			}
		}
	}
}
