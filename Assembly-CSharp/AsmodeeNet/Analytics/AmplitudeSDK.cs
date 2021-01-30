using System;
using System.Collections.Generic;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000733 RID: 1843
	public class AmplitudeSDK : IAnalyticsService
	{
		// Token: 0x06004052 RID: 16466 RVA: 0x001395A3 File Offset: 0x001377A3
		public AmplitudeSDK(string apiKey)
		{
			Amplitude.Instance.init(apiKey);
		}

		// Token: 0x06004053 RID: 16467 RVA: 0x001395B6 File Offset: 0x001377B6
		public void LogEvent(string eventType, IDictionary<string, object> eventProperties)
		{
			Amplitude.Instance.logEvent(eventType, eventProperties);
		}

		// Token: 0x06004054 RID: 16468 RVA: 0x001395C4 File Offset: 0x001377C4
		public void UpdateUserProperties(IDictionary<string, object> userProperties)
		{
			Amplitude.Instance.setUserProperties(userProperties);
		}

		// Token: 0x06004055 RID: 16469 RVA: 0x001395D1 File Offset: 0x001377D1
		public void SetUserId(string userId)
		{
			Amplitude.Instance.setUserId(userId);
		}
	}
}
