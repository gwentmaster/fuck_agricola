using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x02000746 RID: 1862
	public class AdjustEventSuccess
	{
		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x0600414B RID: 16715 RVA: 0x0013B20B File Offset: 0x0013940B
		// (set) Token: 0x0600414C RID: 16716 RVA: 0x0013B213 File Offset: 0x00139413
		public string Adid { get; set; }

		// Token: 0x1700094B RID: 2379
		// (get) Token: 0x0600414D RID: 16717 RVA: 0x0013B21C File Offset: 0x0013941C
		// (set) Token: 0x0600414E RID: 16718 RVA: 0x0013B224 File Offset: 0x00139424
		public string Message { get; set; }

		// Token: 0x1700094C RID: 2380
		// (get) Token: 0x0600414F RID: 16719 RVA: 0x0013B22D File Offset: 0x0013942D
		// (set) Token: 0x06004150 RID: 16720 RVA: 0x0013B235 File Offset: 0x00139435
		public string Timestamp { get; set; }

		// Token: 0x1700094D RID: 2381
		// (get) Token: 0x06004151 RID: 16721 RVA: 0x0013B23E File Offset: 0x0013943E
		// (set) Token: 0x06004152 RID: 16722 RVA: 0x0013B246 File Offset: 0x00139446
		public string EventToken { get; set; }

		// Token: 0x1700094E RID: 2382
		// (get) Token: 0x06004153 RID: 16723 RVA: 0x0013B24F File Offset: 0x0013944F
		// (set) Token: 0x06004154 RID: 16724 RVA: 0x0013B257 File Offset: 0x00139457
		public string CallbackId { get; set; }

		// Token: 0x1700094F RID: 2383
		// (get) Token: 0x06004155 RID: 16725 RVA: 0x0013B260 File Offset: 0x00139460
		// (set) Token: 0x06004156 RID: 16726 RVA: 0x0013B268 File Offset: 0x00139468
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x06004157 RID: 16727 RVA: 0x00003425 File Offset: 0x00001625
		public AdjustEventSuccess()
		{
		}

		// Token: 0x06004158 RID: 16728 RVA: 0x0013B274 File Offset: 0x00139474
		public AdjustEventSuccess(Dictionary<string, string> eventSuccessDataMap)
		{
			if (eventSuccessDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyEventToken);
			this.CallbackId = AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyCallbackId);
			JSONNode jsonnode = JSON.Parse(AdjustUtils.TryGetValue(eventSuccessDataMap, AdjustUtils.KeyJsonResponse));
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06004159 RID: 16729 RVA: 0x0013B324 File Offset: 0x00139524
		public AdjustEventSuccess(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyEventToken);
			this.CallbackId = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCallbackId);
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

		// Token: 0x0600415A RID: 16730 RVA: 0x0013B3E0 File Offset: 0x001395E0
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

		// Token: 0x0600415B RID: 16731 RVA: 0x0013B41A File Offset: 0x0013961A
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
