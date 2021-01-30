using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072A RID: 1834
	public class AnalyticsManager : MonoBehaviour
	{
		// Token: 0x170008E2 RID: 2274
		// (get) Token: 0x06003FC7 RID: 16327 RVA: 0x0013822A File Offset: 0x0013642A
		private string _AmplitudeAPIKey
		{
			get
			{
				return this._amplitudeAPIKeys.prod;
			}
		}

		// Token: 0x06003FC8 RID: 16328 RVA: 0x00138237 File Offset: 0x00136437
		private void Start()
		{
			this._analyticsService = new AmplitudeHttp(this._AmplitudeAPIKey);
		}

		// Token: 0x06003FC9 RID: 16329 RVA: 0x0013824C File Offset: 0x0013644C
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			Dictionary<string, object> dictionary = eventProperties.Concat(from _eP in this._EventProperties
			where !eventProperties.Keys.Contains(_eP.Key)
			select _eP).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			dictionary["client_local_time"] = DateTime.Now.ToString("s", CultureInfo.InvariantCulture);
			if (this.HasContext(typeof(ApplicationAnalyticsContext)))
			{
				ApplicationAnalyticsContext applicationAnalyticsContext = this.GetContext(typeof(ApplicationAnalyticsContext)) as ApplicationAnalyticsContext;
				dictionary["app_boot_session_id"] = applicationAnalyticsContext.AppBootSessionId;
			}
			IAnalyticsService analyticsService = this._analyticsService;
			if (analyticsService == null)
			{
				return;
			}
			analyticsService.LogEvent(eventType, dictionary);
		}

		// Token: 0x06003FCA RID: 16330 RVA: 0x0013833C File Offset: 0x0013653C
		public void SetUserProperties(IDictionary<string, object> userProperties)
		{
			Dictionary<string, object> userProperties2 = userProperties.Concat(from _uP in this._UserProperties
			where !userProperties.Keys.Contains(_uP.Key)
			select _uP).ToDictionary((KeyValuePair<string, object> prop) => prop.Key, (KeyValuePair<string, object> prop) => prop.Value);
			IAnalyticsService analyticsService = this._analyticsService;
			if (analyticsService == null)
			{
				return;
			}
			analyticsService.UpdateUserProperties(userProperties2);
		}

		// Token: 0x170008E3 RID: 2275
		// (set) Token: 0x06003FCB RID: 16331 RVA: 0x001383CD File Offset: 0x001365CD
		public string UserId
		{
			set
			{
				IAnalyticsService analyticsService = this._analyticsService;
				if (analyticsService == null)
				{
					return;
				}
				analyticsService.SetUserId(value);
			}
		}

		// Token: 0x06003FCC RID: 16332 RVA: 0x001383E0 File Offset: 0x001365E0
		public void SetVersionBuildNumber(string value)
		{
			this._EventProperties["version_build_number"] = value;
		}

		// Token: 0x06003FCD RID: 16333 RVA: 0x001383F3 File Offset: 0x001365F3
		public void SetEnvironment(string value)
		{
			this._EventProperties["environment"] = value;
		}

		// Token: 0x06003FCE RID: 16334 RVA: 0x00138406 File Offset: 0x00136606
		public void SetScreenResolution(string value)
		{
			this._EventProperties["screen_resolution"] = value;
		}

		// Token: 0x06003FCF RID: 16335 RVA: 0x00138419 File Offset: 0x00136619
		public void SetConnectionType(string value)
		{
			this._EventProperties["connection_type"] = value;
		}

		// Token: 0x06003FD0 RID: 16336 RVA: 0x0013842C File Offset: 0x0013662C
		public void SetUnitySDKVersion(string value)
		{
			this._EventProperties["unity_sdk_version"] = value;
		}

		// Token: 0x170008E4 RID: 2276
		// (get) Token: 0x06003FD1 RID: 16337 RVA: 0x00138440 File Offset: 0x00136640
		private Dictionary<string, object> _EventProperties
		{
			get
			{
				if (this._eventProperties == null)
				{
					this._eventProperties = new Dictionary<string, object>();
					this._eventProperties.Add("screen_resolution", string.Format("{0}*{1}", Screen.width, Screen.height));
					this._eventProperties.Add("connection_type", this._ConnectionType());
					this._eventProperties.Add("unity_sdk_version", SDKVersionManager.Version());
					this._eventProperties.Add("version_build_number", null);
					this._eventProperties.Add("app_boot_session_id", null);
					this._eventProperties.Add("client_local_time", null);
				}
				return this._eventProperties;
			}
		}

		// Token: 0x06003FD2 RID: 16338 RVA: 0x001384F8 File Offset: 0x001366F8
		private string _ConnectionType()
		{
			switch (Application.internetReachability)
			{
			case NetworkReachability.ReachableViaCarrierDataNetwork:
				return "carrier_data_network";
			case NetworkReachability.ReachableViaLocalAreaNetwork:
				return "local_area_network";
			}
			return "no_connection";
		}

		// Token: 0x06003FD3 RID: 16339 RVA: 0x00138530 File Offset: 0x00136730
		public void SetBackendPlatform(string value)
		{
			this._UserProperties["backend_platform"] = value;
		}

		// Token: 0x06003FD4 RID: 16340 RVA: 0x00138543 File Offset: 0x00136743
		public void SetBackendUserId(string value)
		{
			this._UserProperties["backend_user_id"] = value;
		}

		// Token: 0x06003FD5 RID: 16341 RVA: 0x00138556 File Offset: 0x00136756
		public void SetUAPlatform(string value)
		{
			this._UserProperties["ua_platform"] = value;
		}

		// Token: 0x06003FD6 RID: 16342 RVA: 0x00138569 File Offset: 0x00136769
		public void SetUAUserId(string value)
		{
			this._UserProperties["ua_user_id"] = value;
		}

		// Token: 0x06003FD7 RID: 16343 RVA: 0x0013857C File Offset: 0x0013677C
		public void SetUAChannel(string value)
		{
			this._UserProperties["ua_channel"] = value;
		}

		// Token: 0x06003FD8 RID: 16344 RVA: 0x0013858F File Offset: 0x0013678F
		public void SetPushPlatform(string value)
		{
			this._UserProperties["push_platform"] = value;
		}

		// Token: 0x06003FD9 RID: 16345 RVA: 0x001385A2 File Offset: 0x001367A2
		public void SetPushUserId(string value)
		{
			this._UserProperties["push_user_id"] = value;
		}

		// Token: 0x06003FDA RID: 16346 RVA: 0x001385B5 File Offset: 0x001367B5
		public void SetFirstParty(string value)
		{
			this._UserProperties["first_party"] = value;
		}

		// Token: 0x06003FDB RID: 16347 RVA: 0x001385C8 File Offset: 0x001367C8
		public void SetUserIdFirstParty(string value)
		{
			this._UserProperties["user_id_first_party"] = value;
		}

		// Token: 0x06003FDC RID: 16348 RVA: 0x00003022 File Offset: 0x00001222
		public void SetTimeLtd(long? value)
		{
		}

		// Token: 0x06003FDD RID: 16349 RVA: 0x00003022 File Offset: 0x00001222
		public void SetTimeLtdGameplay(long? value)
		{
		}

		// Token: 0x06003FDE RID: 16350 RVA: 0x001385DB File Offset: 0x001367DB
		public void SetABTestGroup(string value)
		{
			this._UserProperties["ab_test_group"] = value;
		}

		// Token: 0x06003FDF RID: 16351 RVA: 0x001385EE File Offset: 0x001367EE
		public void SetKarma(int? value)
		{
			this._UserProperties["karma"] = value;
		}

		// Token: 0x06003FE0 RID: 16352 RVA: 0x00138606 File Offset: 0x00136806
		public void SetEloRating(int? value)
		{
			this._UserProperties["elo_rating"] = value;
		}

		// Token: 0x06003FE1 RID: 16353 RVA: 0x0013861E File Offset: 0x0013681E
		public void SetIsPayer(bool? value)
		{
			this._UserProperties["is_payer"] = value;
		}

		// Token: 0x06003FE2 RID: 16354 RVA: 0x00138636 File Offset: 0x00136836
		public void SetTimezoneClient(int value)
		{
			this._UserProperties["timezone_client"] = value;
		}

		// Token: 0x170008E5 RID: 2277
		// (get) Token: 0x06003FE3 RID: 16355 RVA: 0x00138650 File Offset: 0x00136850
		private Dictionary<string, object> _UserProperties
		{
			get
			{
				if (this._userProperties == null)
				{
					this._userProperties = new Dictionary<string, object>();
					this._userProperties.Add("timezone_client", TimeZone.CurrentTimeZone.GetUtcOffset(DateTime.Now).TotalMinutes);
					this._userProperties.Add("backend_platform", null);
					this._userProperties.Add("backend_user_id", null);
					this._userProperties.Add("ua_platform", null);
					this._userProperties.Add("ua_user_id", null);
					this._userProperties.Add("ua_channel", null);
					this._userProperties.Add("push_platform", null);
					this._userProperties.Add("push_user_id", null);
					this._userProperties.Add("first_party", null);
					this._userProperties.Add("user_id_first_party", null);
					this._userProperties.Add("ab_test_group", null);
					this._userProperties.Add("karma", null);
					this._userProperties.Add("elo_rating", null);
					this._userProperties.Add("is_payer", null);
				}
				return this._userProperties;
			}
		}

		// Token: 0x06003FE4 RID: 16356 RVA: 0x00138784 File Offset: 0x00136984
		private void OnApplicationPause(bool pause)
		{
			foreach (KeyValuePair<Type, AnalyticsContext> keyValuePair in this._contextData)
			{
				if (pause)
				{
					keyValuePair.Value.Pause();
				}
				else
				{
					keyValuePair.Value.Resume();
				}
			}
		}

		// Token: 0x06003FE5 RID: 16357 RVA: 0x001387F0 File Offset: 0x001369F0
		private void OnApplicationFocus(bool focus)
		{
			this.OnApplicationPause(!focus);
		}

		// Token: 0x06003FE6 RID: 16358 RVA: 0x001387FC File Offset: 0x001369FC
		private void OnApplicationQuit()
		{
			foreach (KeyValuePair<Type, AnalyticsContext> keyValuePair in this._contextData)
			{
				keyValuePair.Value.Quit();
			}
		}

		// Token: 0x06003FE7 RID: 16359 RVA: 0x00138854 File Offset: 0x00136A54
		public bool HasContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			return this._contextData.ContainsKey(contextType);
		}

		// Token: 0x06003FE8 RID: 16360 RVA: 0x00138890 File Offset: 0x00136A90
		public AnalyticsContext GetContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			if (this._contextData.ContainsKey(contextType))
			{
				return this._contextData[contextType];
			}
			AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "The required context " + contextType.ToString() + " does not exist", null);
			return null;
		}

		// Token: 0x06003FE9 RID: 16361 RVA: 0x00138914 File Offset: 0x00136B14
		public void AddContext(AnalyticsContext context)
		{
			Type type = context.GetType();
			if (this._contextData.ContainsKey(type))
			{
				AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "Context " + type.ToString() + " already exist", null);
			}
			this._contextData[type] = context;
		}

		// Token: 0x06003FEA RID: 16362 RVA: 0x00138970 File Offset: 0x00136B70
		public void RemoveContext(Type contextType)
		{
			if (!contextType.IsSubclassOf(typeof(AnalyticsContext)))
			{
				throw new ArgumentException("Wrong context " + contextType.ToString() + ". Must be a subclass of AnalyticsContext");
			}
			if (this._contextData.ContainsKey(contextType))
			{
				this._contextData.Remove(contextType);
				return;
			}
			AsmoLogger.Log(Application.isEditor ? AsmoLogger.Severity.Warning : AsmoLogger.Severity.Error, "AnalyticsManager", "Try to remove unknown context " + contextType.ToString(), null);
		}

		// Token: 0x04002984 RID: 10628
		private const string _kModuleName = "AnalyticsManager";

		// Token: 0x04002985 RID: 10629
		[SerializeField]
		private AnalyticsManager.AmplitudeApiKeys _amplitudeAPIKeys;

		// Token: 0x04002986 RID: 10630
		private IAnalyticsService _analyticsService;

		// Token: 0x04002987 RID: 10631
		private Dictionary<string, object> _eventProperties;

		// Token: 0x04002988 RID: 10632
		private Dictionary<string, object> _userProperties;

		// Token: 0x04002989 RID: 10633
		private Dictionary<Type, AnalyticsContext> _contextData = new Dictionary<Type, AnalyticsContext>();

		// Token: 0x02000A28 RID: 2600
		[Serializable]
		public struct AmplitudeApiKeys
		{
			// Token: 0x04003468 RID: 13416
			public string dev;

			// Token: 0x04003469 RID: 13417
			public string prod;
		}
	}
}
