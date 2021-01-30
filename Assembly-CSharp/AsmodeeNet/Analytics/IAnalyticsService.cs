using System;
using System.Collections.Generic;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000734 RID: 1844
	public interface IAnalyticsService
	{
		// Token: 0x06004056 RID: 16470
		void LogEvent(string eventType, IDictionary<string, object> eventProperties);

		// Token: 0x06004057 RID: 16471
		void UpdateUserProperties(IDictionary<string, object> userProperties);

		// Token: 0x06004058 RID: 16472
		void SetUserId(string userId);
	}
}
