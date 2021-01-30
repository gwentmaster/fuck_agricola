using System;
using System.Collections.Generic;

namespace com.adjust.sdk
{
	// Token: 0x02000740 RID: 1856
	public class AdjustAttribution
	{
		// Token: 0x1700093B RID: 2363
		// (get) Token: 0x06004103 RID: 16643 RVA: 0x0013AB6A File Offset: 0x00138D6A
		// (set) Token: 0x06004104 RID: 16644 RVA: 0x0013AB72 File Offset: 0x00138D72
		public string adid { get; set; }

		// Token: 0x1700093C RID: 2364
		// (get) Token: 0x06004105 RID: 16645 RVA: 0x0013AB7B File Offset: 0x00138D7B
		// (set) Token: 0x06004106 RID: 16646 RVA: 0x0013AB83 File Offset: 0x00138D83
		public string network { get; set; }

		// Token: 0x1700093D RID: 2365
		// (get) Token: 0x06004107 RID: 16647 RVA: 0x0013AB8C File Offset: 0x00138D8C
		// (set) Token: 0x06004108 RID: 16648 RVA: 0x0013AB94 File Offset: 0x00138D94
		public string adgroup { get; set; }

		// Token: 0x1700093E RID: 2366
		// (get) Token: 0x06004109 RID: 16649 RVA: 0x0013AB9D File Offset: 0x00138D9D
		// (set) Token: 0x0600410A RID: 16650 RVA: 0x0013ABA5 File Offset: 0x00138DA5
		public string campaign { get; set; }

		// Token: 0x1700093F RID: 2367
		// (get) Token: 0x0600410B RID: 16651 RVA: 0x0013ABAE File Offset: 0x00138DAE
		// (set) Token: 0x0600410C RID: 16652 RVA: 0x0013ABB6 File Offset: 0x00138DB6
		public string creative { get; set; }

		// Token: 0x17000940 RID: 2368
		// (get) Token: 0x0600410D RID: 16653 RVA: 0x0013ABBF File Offset: 0x00138DBF
		// (set) Token: 0x0600410E RID: 16654 RVA: 0x0013ABC7 File Offset: 0x00138DC7
		public string clickLabel { get; set; }

		// Token: 0x17000941 RID: 2369
		// (get) Token: 0x0600410F RID: 16655 RVA: 0x0013ABD0 File Offset: 0x00138DD0
		// (set) Token: 0x06004110 RID: 16656 RVA: 0x0013ABD8 File Offset: 0x00138DD8
		public string trackerName { get; set; }

		// Token: 0x17000942 RID: 2370
		// (get) Token: 0x06004111 RID: 16657 RVA: 0x0013ABE1 File Offset: 0x00138DE1
		// (set) Token: 0x06004112 RID: 16658 RVA: 0x0013ABE9 File Offset: 0x00138DE9
		public string trackerToken { get; set; }

		// Token: 0x06004113 RID: 16659 RVA: 0x00003425 File Offset: 0x00001625
		public AdjustAttribution()
		{
		}

		// Token: 0x06004114 RID: 16660 RVA: 0x0013ABF4 File Offset: 0x00138DF4
		public AdjustAttribution(string jsonString)
		{
			JSONNode jsonnode = JSON.Parse(jsonString);
			if (jsonnode == null)
			{
				return;
			}
			this.trackerName = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyTrackerToken);
			this.network = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyNetwork);
			this.campaign = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdgroup);
			this.creative = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyClickLabel);
			this.adid = AdjustUtils.GetJsonString(jsonnode, AdjustUtils.KeyAdid);
		}

		// Token: 0x06004115 RID: 16661 RVA: 0x0013ACA0 File Offset: 0x00138EA0
		public AdjustAttribution(Dictionary<string, string> dicAttributionData)
		{
			if (dicAttributionData == null)
			{
				return;
			}
			this.trackerName = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerName);
			this.trackerToken = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyTrackerToken);
			this.network = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyNetwork);
			this.campaign = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCampaign);
			this.adgroup = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdgroup);
			this.creative = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyCreative);
			this.clickLabel = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyClickLabel);
			this.adid = AdjustUtils.TryGetValue(dicAttributionData, AdjustUtils.KeyAdid);
		}
	}
}
