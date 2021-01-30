using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SocketIO
{
	// Token: 0x02000597 RID: 1431
	public sealed class HandshakeData
	{
		// Token: 0x1700068A RID: 1674
		// (get) Token: 0x06003451 RID: 13393 RVA: 0x00107DA5 File Offset: 0x00105FA5
		// (set) Token: 0x06003452 RID: 13394 RVA: 0x00107DAD File Offset: 0x00105FAD
		public string Sid { get; private set; }

		// Token: 0x1700068B RID: 1675
		// (get) Token: 0x06003453 RID: 13395 RVA: 0x00107DB6 File Offset: 0x00105FB6
		// (set) Token: 0x06003454 RID: 13396 RVA: 0x00107DBE File Offset: 0x00105FBE
		public List<string> Upgrades { get; private set; }

		// Token: 0x1700068C RID: 1676
		// (get) Token: 0x06003455 RID: 13397 RVA: 0x00107DC7 File Offset: 0x00105FC7
		// (set) Token: 0x06003456 RID: 13398 RVA: 0x00107DCF File Offset: 0x00105FCF
		public TimeSpan PingInterval { get; private set; }

		// Token: 0x1700068D RID: 1677
		// (get) Token: 0x06003457 RID: 13399 RVA: 0x00107DD8 File Offset: 0x00105FD8
		// (set) Token: 0x06003458 RID: 13400 RVA: 0x00107DE0 File Offset: 0x00105FE0
		public TimeSpan PingTimeout { get; private set; }

		// Token: 0x06003459 RID: 13401 RVA: 0x00107DEC File Offset: 0x00105FEC
		public bool Parse(string str)
		{
			bool flag = false;
			Dictionary<string, object> from = Json.Decode(str, ref flag) as Dictionary<string, object>;
			if (!flag)
			{
				return false;
			}
			try
			{
				this.Sid = HandshakeData.GetString(from, "sid");
				this.Upgrades = HandshakeData.GetStringList(from, "upgrades");
				this.PingInterval = TimeSpan.FromMilliseconds((double)HandshakeData.GetInt(from, "pingInterval"));
				this.PingTimeout = TimeSpan.FromMilliseconds((double)HandshakeData.GetInt(from, "pingTimeout"));
			}
			catch
			{
				return false;
			}
			return true;
		}

		// Token: 0x0600345A RID: 13402 RVA: 0x00107E7C File Offset: 0x0010607C
		private static object Get(Dictionary<string, object> from, string key)
		{
			object result;
			if (!from.TryGetValue(key, out result))
			{
				throw new Exception(string.Format("Can't get {0} from Handshake data!", key));
			}
			return result;
		}

		// Token: 0x0600345B RID: 13403 RVA: 0x00107EA6 File Offset: 0x001060A6
		private static string GetString(Dictionary<string, object> from, string key)
		{
			return HandshakeData.Get(from, key) as string;
		}

		// Token: 0x0600345C RID: 13404 RVA: 0x00107EB4 File Offset: 0x001060B4
		private static List<string> GetStringList(Dictionary<string, object> from, string key)
		{
			List<object> list = HandshakeData.Get(from, key) as List<object>;
			List<string> list2 = new List<string>(list.Count);
			for (int i = 0; i < list.Count; i++)
			{
				string text = list[i] as string;
				if (text != null)
				{
					list2.Add(text);
				}
			}
			return list2;
		}

		// Token: 0x0600345D RID: 13405 RVA: 0x00107F03 File Offset: 0x00106103
		private static int GetInt(Dictionary<string, object> from, string key)
		{
			return (int)((double)HandshakeData.Get(from, key));
		}
	}
}
