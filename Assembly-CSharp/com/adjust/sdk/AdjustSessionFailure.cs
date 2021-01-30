using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x02000749 RID: 1865
	public class AdjustSessionFailure
	{
		// Token: 0x17000950 RID: 2384
		// (get) Token: 0x0600415E RID: 16734 RVA: 0x0013B4EE File Offset: 0x001396EE
		// (set) Token: 0x0600415F RID: 16735 RVA: 0x0013B4F6 File Offset: 0x001396F6
		public string Adid { get; set; }

		// Token: 0x17000951 RID: 2385
		// (get) Token: 0x06004160 RID: 16736 RVA: 0x0013B4FF File Offset: 0x001396FF
		// (set) Token: 0x06004161 RID: 16737 RVA: 0x0013B507 File Offset: 0x00139707
		public string Message { get; set; }

		// Token: 0x17000952 RID: 2386
		// (get) Token: 0x06004162 RID: 16738 RVA: 0x0013B510 File Offset: 0x00139710
		// (set) Token: 0x06004163 RID: 16739 RVA: 0x0013B518 File Offset: 0x00139718
		public string Timestamp { get; set; }

		// Token: 0x17000953 RID: 2387
		// (get) Token: 0x06004164 RID: 16740 RVA: 0x0013B521 File Offset: 0x00139721
		// (set) Token: 0x06004165 RID: 16741 RVA: 0x0013B529 File Offset: 0x00139729
		public bool WillRetry { get; set; }

		// Token: 0x17000954 RID: 2388
		// (get) Token: 0x06004166 RID: 16742 RVA: 0x0013B532 File Offset: 0x00139732
		// (set) Token: 0x06004167 RID: 16743 RVA: 0x0013B53A File Offset: 0x0013973A
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x06004168 RID: 16744 RVA: 0x00003425 File Offset: 0x00001625
		public AdjustSessionFailure()
		{
		}

		// Token: 0x06004169 RID: 16745 RVA: 0x0013B544 File Offset: 0x00139744
		public AdjustSessionFailure(Dictionary<string, string> sessionFailureDataMap)
		{
			if (sessionFailureDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyTimestamp);
			bool willRetry;
			if (bool.TryParse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
			{
				this.WillRetry = willRetry;
			}
			JSONNode jsonnode = JSON.Parse(AdjustUtils.TryGetValue(sessionFailureDataMap, AdjustUtils.KeyJsonResponse));
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x0600416A RID: 16746 RVA: 0x0013B5F0 File Offset: 0x001397F0
		public AdjustSessionFailure(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
			this.WillRetry = Convert.ToBoolean(AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyWillRetry));
			JSONNode jsonnode2 = jsonnode[AdjustUtils.KeyJsonResponse];
			if (jsonnode2 == null)
			{
				return;
			}
			if (jsonnode2.AsObject == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode2.AsObject, this.JsonResponse);
		}

		// Token: 0x0600416B RID: 16747 RVA: 0x0013B6A0 File Offset: 0x001398A0
		public void BuildJsonResponseFromString(string jsonResponseString)
		{
			JSONNode jsonnode = JSON.Parse(jsonResponseString);
			if (jsonnode == null)
			{
				return;
			}
			this.JsonResponse = new Dictionary<string, object>();
			AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
		}

		// Token: 0x0600416C RID: 16748 RVA: 0x0013B6DA File Offset: 0x001398DA
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
