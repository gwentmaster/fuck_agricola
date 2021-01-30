using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using AsmodeeNet.Utils;
using BestHTTP;
using MiniJSON;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000732 RID: 1842
	public class AmplitudeHttp : IAnalyticsService
	{
		// Token: 0x06004049 RID: 16457 RVA: 0x001391CD File Offset: 0x001373CD
		public AmplitudeHttp(string apiKey)
		{
			this._api_key = apiKey;
		}

		// Token: 0x0600404A RID: 16458 RVA: 0x001391E8 File Offset: 0x001373E8
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			HTTPRequest httprequest = this._GenerateAmplitudeHttpApiRequest();
			Dictionary<string, object> dictionary = this._GenerateDefaultProperties();
			dictionary.Add("event_type", eventType);
			dictionary.Add("event_properties", eventProperties);
			dictionary.Add("user_properties", this._userProperties);
			string json = Json.Serialize(dictionary);
			httprequest.AddField("event", json);
			httprequest.Send();
			AsmoLogger.Trace("AmplitudeHttp", () => "LogEvent " + json, null);
		}

		// Token: 0x0600404B RID: 16459 RVA: 0x0013926C File Offset: 0x0013746C
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			this._userProperties = userProperties.Concat(from _uP in this._userProperties
			where !userProperties.Keys.Contains(_uP.Key)
			select _uP).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
		}

		// Token: 0x0600404C RID: 16460 RVA: 0x001392F1 File Offset: 0x001374F1
		public void SetUserId(string userId)
		{
			this._userId = userId;
		}

		// Token: 0x0600404D RID: 16461 RVA: 0x001392FC File Offset: 0x001374FC
		private HTTPRequest _GenerateAmplitudeHttpApiRequest()
		{
			HTTPRequest httprequest = new HTTPRequest(new Uri("https://api.amplitude.com/httpapi"), HTTPMethods.Post, delegate(HTTPRequest req, HTTPResponse resp)
			{
				HTTPRequestStates state = req.State;
				if (state - HTTPRequestStates.Error <= 3)
				{
					AsmoLogger.Error("AmplitudeHttp", string.Format("LogEvent Error {0} [{1}] {2}", req.State.ToString(), resp.StatusCode, resp.Message), null);
				}
			});
			httprequest.AddHeader("Content-Type", "application/x-www-form-urlencoded");
			httprequest.AddField("api_key", this._api_key);
			return httprequest;
		}

		// Token: 0x17000910 RID: 2320
		// (get) Token: 0x0600404E RID: 16462 RVA: 0x0013935C File Offset: 0x0013755C
		public Dictionary<string, object> FixedProperties
		{
			get
			{
				if (this._fixedProperties == null)
				{
					this._fixedProperties = new Dictionary<string, object>();
					this._fixedProperties.Add("device_id", SystemInfo.deviceUniqueIdentifier);
					this._fixedProperties.Add("app_version", Application.version);
					this._fixedProperties.Add("platform", SystemInfo.operatingSystemFamily.ToString());
					this._fixedProperties.Add("os_name", Environment.OSVersion.Platform.ToString());
					this._fixedProperties.Add("os_version", Environment.OSVersion.Version.ToString());
					this._fixedProperties.Add("device_model", SystemInfo.deviceModel);
					this._fixedProperties.Add("language", LocalizationService.Instance.GetLocalizationLanguage());
					this._fixedProperties.Add("ip", this._IP());
					this._fixedProperties.Add("session_id", this._GenerateTimestamp());
				}
				return this._fixedProperties;
			}
		}

		// Token: 0x0600404F RID: 16463 RVA: 0x0013947C File Offset: 0x0013767C
		private Dictionary<string, object> _GenerateDefaultProperties()
		{
			Dictionary<string, object> dictionary = new Dictionary<string, object>(this.FixedProperties);
			dictionary.Add("user_id", this._userId);
			dictionary.Add("time", this._GenerateTimestamp());
			string key = "event_id";
			int event_id = this._event_id;
			this._event_id = event_id + 1;
			dictionary.Add(key, event_id);
			return dictionary;
		}

		// Token: 0x06004050 RID: 16464 RVA: 0x001394DC File Offset: 0x001376DC
		private long _GenerateTimestamp()
		{
			return (long)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalSeconds;
		}

		// Token: 0x06004051 RID: 16465 RVA: 0x00139510 File Offset: 0x00137710
		private string _IP()
		{
			List<string> list = new List<string>();
			foreach (IPAddress ipaddress in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
			{
				if (!ipaddress.IsIPv6Multicast && !ipaddress.IsIPv6Teredo && !ipaddress.IsIPv6LinkLocal && !ipaddress.IsIPv6SiteLocal)
				{
					if (ipaddress.AddressFamily == AddressFamily.InterNetwork)
					{
						list.Add(ipaddress.ToString());
					}
					else if (ipaddress.AddressFamily == AddressFamily.InterNetworkV6)
					{
						list.Insert(0, ipaddress.ToString());
					}
				}
			}
			if (list.Count != 0)
			{
				return list.First<string>();
			}
			return null;
		}

		// Token: 0x040029B8 RID: 10680
		private const string _kModuleName = "AmplitudeHttp";

		// Token: 0x040029B9 RID: 10681
		private string _api_key;

		// Token: 0x040029BA RID: 10682
		private int _event_id;

		// Token: 0x040029BB RID: 10683
		private IDictionary<string, object> _userProperties = new Dictionary<string, object>();

		// Token: 0x040029BC RID: 10684
		private string _userId;

		// Token: 0x040029BD RID: 10685
		private Dictionary<string, object> _fixedProperties;
	}
}
