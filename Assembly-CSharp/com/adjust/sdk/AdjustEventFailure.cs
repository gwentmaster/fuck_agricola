using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x02000745 RID: 1861
	public class AdjustEventFailure
	{
		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x06004138 RID: 16696 RVA: 0x0013AFB0 File Offset: 0x001391B0
		// (set) Token: 0x06004139 RID: 16697 RVA: 0x0013AFB8 File Offset: 0x001391B8
		public string Adid { get; set; }

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x0600413A RID: 16698 RVA: 0x0013AFC1 File Offset: 0x001391C1
		// (set) Token: 0x0600413B RID: 16699 RVA: 0x0013AFC9 File Offset: 0x001391C9
		public string Message { get; set; }

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x0600413C RID: 16700 RVA: 0x0013AFD2 File Offset: 0x001391D2
		// (set) Token: 0x0600413D RID: 16701 RVA: 0x0013AFDA File Offset: 0x001391DA
		public string Timestamp { get; set; }

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x0600413E RID: 16702 RVA: 0x0013AFE3 File Offset: 0x001391E3
		// (set) Token: 0x0600413F RID: 16703 RVA: 0x0013AFEB File Offset: 0x001391EB
		public string EventToken { get; set; }

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x06004140 RID: 16704 RVA: 0x0013AFF4 File Offset: 0x001391F4
		// (set) Token: 0x06004141 RID: 16705 RVA: 0x0013AFFC File Offset: 0x001391FC
		public string CallbackId { get; set; }

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x06004142 RID: 16706 RVA: 0x0013B005 File Offset: 0x00139205
		// (set) Token: 0x06004143 RID: 16707 RVA: 0x0013B00D File Offset: 0x0013920D
		public bool WillRetry { get; set; }

		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x06004144 RID: 16708 RVA: 0x0013B016 File Offset: 0x00139216
		// (set) Token: 0x06004145 RID: 16709 RVA: 0x0013B01E File Offset: 0x0013921E
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x06004146 RID: 16710 RVA: 0x00003425 File Offset: 0x00001625
		public AdjustEventFailure()
		{
		}

		// Token: 0x06004147 RID: 16711 RVA: 0x0013B028 File Offset: 0x00139228
		public AdjustEventFailure(Dictionary<string, string> eventFailureDataMap)
		{
			if (eventFailureDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyTimestamp);
			this.EventToken = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyEventToken);
			this.CallbackId = AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyCallbackId);
			bool willRetry;
			if (bool.TryParse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyWillRetry), out willRetry))
			{
				this.WillRetry = willRetry;
			}
			JSONNode jsonnode = JSON.Parse(AdjustUtils.TryGetValue(eventFailureDataMap, AdjustUtils.KeyJsonResponse));
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06004148 RID: 16712 RVA: 0x0013B0F4 File Offset: 0x001392F4
		public AdjustEventFailure(string jsonString)
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

		// Token: 0x06004149 RID: 16713 RVA: 0x0013B1C4 File Offset: 0x001393C4
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

		// Token: 0x0600414A RID: 16714 RVA: 0x0013B1FE File Offset: 0x001393FE
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
