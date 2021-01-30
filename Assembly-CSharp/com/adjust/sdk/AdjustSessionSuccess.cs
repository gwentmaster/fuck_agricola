using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x0200074A RID: 1866
	public class AdjustSessionSuccess
	{
		// Token: 0x17000955 RID: 2389
		// (get) Token: 0x0600416D RID: 16749 RVA: 0x0013B6E7 File Offset: 0x001398E7
		// (set) Token: 0x0600416E RID: 16750 RVA: 0x0013B6EF File Offset: 0x001398EF
		public string Adid { get; set; }

		// Token: 0x17000956 RID: 2390
		// (get) Token: 0x0600416F RID: 16751 RVA: 0x0013B6F8 File Offset: 0x001398F8
		// (set) Token: 0x06004170 RID: 16752 RVA: 0x0013B700 File Offset: 0x00139900
		public string Message { get; set; }

		// Token: 0x17000957 RID: 2391
		// (get) Token: 0x06004171 RID: 16753 RVA: 0x0013B709 File Offset: 0x00139909
		// (set) Token: 0x06004172 RID: 16754 RVA: 0x0013B711 File Offset: 0x00139911
		public string Timestamp { get; set; }

		// Token: 0x17000958 RID: 2392
		// (get) Token: 0x06004173 RID: 16755 RVA: 0x0013B71A File Offset: 0x0013991A
		// (set) Token: 0x06004174 RID: 16756 RVA: 0x0013B722 File Offset: 0x00139922
		public Dictionary<string, object> JsonResponse { get; set; }

		// Token: 0x06004175 RID: 16757 RVA: 0x00003425 File Offset: 0x00001625
		public AdjustSessionSuccess()
		{
		}

		// Token: 0x06004176 RID: 16758 RVA: 0x0013B72C File Offset: 0x0013992C
		public AdjustSessionSuccess(Dictionary<string, string> sessionSuccessDataMap)
		{
			if (sessionSuccessDataMap == null)
			{
				return;
			}
			this.Adid = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyTimestamp);
			JSONNode jsonnode = JSON.Parse(AdjustUtils.TryGetValue(sessionSuccessDataMap, AdjustUtils.KeyJsonResponse));
			if (jsonnode != null && jsonnode.AsObject != null)
			{
				this.JsonResponse = new Dictionary<string, object>();
				AdjustUtils.WriteJsonResponseDictionary(jsonnode.AsObject, this.JsonResponse);
			}
		}

		// Token: 0x06004177 RID: 16759 RVA: 0x0013B7BC File Offset: 0x001399BC
		public AdjustSessionSuccess(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.Adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
			this.Message = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyMessage);
			this.Timestamp = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTimestamp);
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

		// Token: 0x06004178 RID: 16760 RVA: 0x0013B854 File Offset: 0x00139A54
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

		// Token: 0x06004179 RID: 16761 RVA: 0x0013B88E File Offset: 0x00139A8E
		public string GetJsonResponse()
		{
			return AdjustUtils.GetJsonResponseCompact(this.JsonResponse);
		}
	}
}
