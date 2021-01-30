using System;
using System.Collections.Generic;
using BestHTTP.JSON;

namespace BestHTTP.SignalR
{
	// Token: 0x020005B9 RID: 1465
	public sealed class NegotiationData
	{
		// Token: 0x170006E0 RID: 1760
		// (get) Token: 0x060035CA RID: 13770 RVA: 0x0010D040 File Offset: 0x0010B240
		// (set) Token: 0x060035CB RID: 13771 RVA: 0x0010D048 File Offset: 0x0010B248
		public string Url { get; private set; }

		// Token: 0x170006E1 RID: 1761
		// (get) Token: 0x060035CC RID: 13772 RVA: 0x0010D051 File Offset: 0x0010B251
		// (set) Token: 0x060035CD RID: 13773 RVA: 0x0010D059 File Offset: 0x0010B259
		public string WebSocketServerUrl { get; private set; }

		// Token: 0x170006E2 RID: 1762
		// (get) Token: 0x060035CE RID: 13774 RVA: 0x0010D062 File Offset: 0x0010B262
		// (set) Token: 0x060035CF RID: 13775 RVA: 0x0010D06A File Offset: 0x0010B26A
		public string ConnectionToken { get; private set; }

		// Token: 0x170006E3 RID: 1763
		// (get) Token: 0x060035D0 RID: 13776 RVA: 0x0010D073 File Offset: 0x0010B273
		// (set) Token: 0x060035D1 RID: 13777 RVA: 0x0010D07B File Offset: 0x0010B27B
		public string ConnectionId { get; private set; }

		// Token: 0x170006E4 RID: 1764
		// (get) Token: 0x060035D2 RID: 13778 RVA: 0x0010D084 File Offset: 0x0010B284
		// (set) Token: 0x060035D3 RID: 13779 RVA: 0x0010D08C File Offset: 0x0010B28C
		public TimeSpan? KeepAliveTimeout { get; private set; }

		// Token: 0x170006E5 RID: 1765
		// (get) Token: 0x060035D4 RID: 13780 RVA: 0x0010D095 File Offset: 0x0010B295
		// (set) Token: 0x060035D5 RID: 13781 RVA: 0x0010D09D File Offset: 0x0010B29D
		public TimeSpan DisconnectTimeout { get; private set; }

		// Token: 0x170006E6 RID: 1766
		// (get) Token: 0x060035D6 RID: 13782 RVA: 0x0010D0A6 File Offset: 0x0010B2A6
		// (set) Token: 0x060035D7 RID: 13783 RVA: 0x0010D0AE File Offset: 0x0010B2AE
		public TimeSpan ConnectionTimeout { get; private set; }

		// Token: 0x170006E7 RID: 1767
		// (get) Token: 0x060035D8 RID: 13784 RVA: 0x0010D0B7 File Offset: 0x0010B2B7
		// (set) Token: 0x060035D9 RID: 13785 RVA: 0x0010D0BF File Offset: 0x0010B2BF
		public bool TryWebSockets { get; private set; }

		// Token: 0x170006E8 RID: 1768
		// (get) Token: 0x060035DA RID: 13786 RVA: 0x0010D0C8 File Offset: 0x0010B2C8
		// (set) Token: 0x060035DB RID: 13787 RVA: 0x0010D0D0 File Offset: 0x0010B2D0
		public string ProtocolVersion { get; private set; }

		// Token: 0x170006E9 RID: 1769
		// (get) Token: 0x060035DC RID: 13788 RVA: 0x0010D0D9 File Offset: 0x0010B2D9
		// (set) Token: 0x060035DD RID: 13789 RVA: 0x0010D0E1 File Offset: 0x0010B2E1
		public TimeSpan TransportConnectTimeout { get; private set; }

		// Token: 0x170006EA RID: 1770
		// (get) Token: 0x060035DE RID: 13790 RVA: 0x0010D0EA File Offset: 0x0010B2EA
		// (set) Token: 0x060035DF RID: 13791 RVA: 0x0010D0F2 File Offset: 0x0010B2F2
		public TimeSpan LongPollDelay { get; private set; }

		// Token: 0x060035E0 RID: 13792 RVA: 0x0010D0FB File Offset: 0x0010B2FB
		public NegotiationData(Connection connection)
		{
			this.Connection = connection;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0010D10C File Offset: 0x0010B30C
		public void Start()
		{
			this.NegotiationRequest = new HTTPRequest(this.Connection.BuildUri(RequestTypes.Negotiate), HTTPMethods.Get, true, true, new OnRequestFinishedDelegate(this.OnNegotiationRequestFinished));
			this.Connection.PrepareRequest(this.NegotiationRequest, RequestTypes.Negotiate);
			this.NegotiationRequest.Send();
			HTTPManager.Logger.Information("NegotiationData", "Negotiation request sent");
		}

		// Token: 0x060035E2 RID: 13794 RVA: 0x0010D172 File Offset: 0x0010B372
		public void Abort()
		{
			if (this.NegotiationRequest != null)
			{
				this.OnReceived = null;
				this.OnError = null;
				this.NegotiationRequest.Abort();
			}
		}

		// Token: 0x060035E3 RID: 13795 RVA: 0x0010D198 File Offset: 0x0010B398
		private void OnNegotiationRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.NegotiationRequest = null;
			HTTPRequestStates state = req.State;
			if (state != HTTPRequestStates.Finished)
			{
				if (state == HTTPRequestStates.Error)
				{
					this.RaiseOnError((req.Exception != null) ? (req.Exception.Message + " " + req.Exception.StackTrace) : string.Empty);
					return;
				}
				this.RaiseOnError(req.State.ToString());
			}
			else
			{
				if (!resp.IsSuccess)
				{
					this.RaiseOnError(string.Format("Negotiation request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2} Uri: {3}", new object[]
					{
						resp.StatusCode,
						resp.Message,
						resp.DataAsText,
						req.CurrentUri
					}));
					return;
				}
				HTTPManager.Logger.Information("NegotiationData", "Negotiation data arrived: " + resp.DataAsText);
				int num = resp.DataAsText.IndexOf("{");
				if (num < 0)
				{
					this.RaiseOnError("Invalid negotiation text: " + resp.DataAsText);
					return;
				}
				if (this.Parse(resp.DataAsText.Substring(num)) == null)
				{
					this.RaiseOnError("Parsing Negotiation data failed: " + resp.DataAsText);
					return;
				}
				if (this.OnReceived != null)
				{
					this.OnReceived(this);
					this.OnReceived = null;
					return;
				}
			}
		}

		// Token: 0x060035E4 RID: 13796 RVA: 0x0010D2F3 File Offset: 0x0010B4F3
		private void RaiseOnError(string err)
		{
			HTTPManager.Logger.Error("NegotiationData", "Negotiation request failed with error: " + err);
			if (this.OnError != null)
			{
				this.OnError(this, err);
				this.OnError = null;
			}
		}

		// Token: 0x060035E5 RID: 13797 RVA: 0x0010D32C File Offset: 0x0010B52C
		private NegotiationData Parse(string str)
		{
			bool flag = false;
			Dictionary<string, object> dictionary = Json.Decode(str, ref flag) as Dictionary<string, object>;
			if (!flag)
			{
				return null;
			}
			try
			{
				this.Url = NegotiationData.GetString(dictionary, "Url");
				if (dictionary.ContainsKey("webSocketServerUrl"))
				{
					this.WebSocketServerUrl = NegotiationData.GetString(dictionary, "webSocketServerUrl");
				}
				this.ConnectionToken = Uri.EscapeDataString(NegotiationData.GetString(dictionary, "ConnectionToken"));
				this.ConnectionId = NegotiationData.GetString(dictionary, "ConnectionId");
				if (dictionary.ContainsKey("KeepAliveTimeout"))
				{
					this.KeepAliveTimeout = new TimeSpan?(TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "KeepAliveTimeout")));
				}
				this.DisconnectTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "DisconnectTimeout"));
				if (dictionary.ContainsKey("ConnectionTimeout"))
				{
					this.ConnectionTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "ConnectionTimeout"));
				}
				else
				{
					this.ConnectionTimeout = TimeSpan.FromSeconds(120.0);
				}
				this.TryWebSockets = (bool)NegotiationData.Get(dictionary, "TryWebSockets");
				this.ProtocolVersion = NegotiationData.GetString(dictionary, "ProtocolVersion");
				this.TransportConnectTimeout = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "TransportConnectTimeout"));
				if (dictionary.ContainsKey("LongPollDelay"))
				{
					this.LongPollDelay = TimeSpan.FromSeconds(NegotiationData.GetDouble(dictionary, "LongPollDelay"));
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("NegotiationData", "Parse", ex);
				return null;
			}
			return this;
		}

		// Token: 0x060035E6 RID: 13798 RVA: 0x0010D4B8 File Offset: 0x0010B6B8
		private static object Get(Dictionary<string, object> from, string key)
		{
			object result;
			if (!from.TryGetValue(key, out result))
			{
				throw new Exception(string.Format("Can't get {0} from Negotiation data!", key));
			}
			return result;
		}

		// Token: 0x060035E7 RID: 13799 RVA: 0x0010D4E2 File Offset: 0x0010B6E2
		private static string GetString(Dictionary<string, object> from, string key)
		{
			return NegotiationData.Get(from, key) as string;
		}

		// Token: 0x060035E8 RID: 13800 RVA: 0x0010D4F0 File Offset: 0x0010B6F0
		private static List<string> GetStringList(Dictionary<string, object> from, string key)
		{
			List<object> list = NegotiationData.Get(from, key) as List<object>;
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

		// Token: 0x060035E9 RID: 13801 RVA: 0x0010D53F File Offset: 0x0010B73F
		private static int GetInt(Dictionary<string, object> from, string key)
		{
			return (int)((double)NegotiationData.Get(from, key));
		}

		// Token: 0x060035EA RID: 13802 RVA: 0x0010D54E File Offset: 0x0010B74E
		private static double GetDouble(Dictionary<string, object> from, string key)
		{
			return (double)NegotiationData.Get(from, key);
		}

		// Token: 0x04002322 RID: 8994
		public Action<NegotiationData> OnReceived;

		// Token: 0x04002323 RID: 8995
		public Action<NegotiationData, string> OnError;

		// Token: 0x04002324 RID: 8996
		private HTTPRequest NegotiationRequest;

		// Token: 0x04002325 RID: 8997
		private IConnection Connection;
	}
}
