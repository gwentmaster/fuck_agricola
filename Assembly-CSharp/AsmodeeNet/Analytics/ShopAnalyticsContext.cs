using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000735 RID: 1845
	public class ShopAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000911 RID: 2321
		// (get) Token: 0x06004059 RID: 16473 RVA: 0x001395DE File Offset: 0x001377DE
		// (set) Token: 0x0600405A RID: 16474 RVA: 0x001395E6 File Offset: 0x001377E6
		public string ShopSessionId { get; private set; }

		// Token: 0x17000912 RID: 2322
		// (get) Token: 0x0600405B RID: 16475 RVA: 0x001395EF File Offset: 0x001377EF
		// (set) Token: 0x0600405C RID: 16476 RVA: 0x001395F7 File Offset: 0x001377F7
		public string EntryPoint { get; private set; }

		// Token: 0x17000913 RID: 2323
		// (get) Token: 0x0600405D RID: 16477 RVA: 0x00139600 File Offset: 0x00137800
		// (set) Token: 0x0600405E RID: 16478 RVA: 0x00139608 File Offset: 0x00137808
		public string DefaultItemId { get; private set; }

		// Token: 0x17000914 RID: 2324
		// (get) Token: 0x0600405F RID: 16479 RVA: 0x00139611 File Offset: 0x00137811
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startShopTime);
			}
		}

		// Token: 0x06004060 RID: 16480 RVA: 0x00139620 File Offset: 0x00137820
		public ShopAnalyticsContext(string entryPoint, string itemId)
		{
			this.ShopSessionId = Guid.NewGuid().ToString();
			this.EntryPoint = entryPoint;
			this.DefaultItemId = itemId;
			this._startShopTime = Time.unscaledTime;
		}

		// Token: 0x06004061 RID: 16481 RVA: 0x00139665 File Offset: 0x00137865
		public override void Resume()
		{
			base.Resume();
			this._startShopTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040029C1 RID: 10689
		public bool DidPurchase;

		// Token: 0x040029C2 RID: 10690
		private float _startShopTime;
	}
}
