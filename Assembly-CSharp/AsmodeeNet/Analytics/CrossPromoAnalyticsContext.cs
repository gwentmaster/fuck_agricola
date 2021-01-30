using System;
using AsmodeeNet.Network.RestApi;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072E RID: 1838
	public class CrossPromoAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008F1 RID: 2289
		// (get) Token: 0x06004006 RID: 16390 RVA: 0x00138CF3 File Offset: 0x00136EF3
		// (set) Token: 0x06004007 RID: 16391 RVA: 0x00138CFB File Offset: 0x00136EFB
		public string CrossPromoSessionId { get; private set; }

		// Token: 0x170008F2 RID: 2290
		// (get) Token: 0x06004008 RID: 16392 RVA: 0x00138D04 File Offset: 0x00136F04
		// (set) Token: 0x06004009 RID: 16393 RVA: 0x00138D0C File Offset: 0x00136F0C
		public string CrossPromoType { get; private set; }

		// Token: 0x170008F3 RID: 2291
		// (get) Token: 0x0600400A RID: 16394 RVA: 0x00138D15 File Offset: 0x00136F15
		// (set) Token: 0x0600400B RID: 16395 RVA: 0x00138D1D File Offset: 0x00136F1D
		public string MoreGameCategory { get; set; }

		// Token: 0x170008F4 RID: 2292
		// (get) Token: 0x0600400C RID: 16396 RVA: 0x00138D28 File Offset: 0x00136F28
		public string ApiVersion
		{
			get
			{
				string crossPromoType = this.CrossPromoType;
				if (crossPromoType == "banner")
				{
					return string.Format("{0}", RequestBannerEndpoint.GetEndpointVersion());
				}
				if (crossPromoType == "interstitial")
				{
					return string.Format("{0}", RequestInterstitialEndpoint.GetEndpointVersion());
				}
				if (!(crossPromoType == "more_games"))
				{
					return null;
				}
				return string.Format("{0}", RequestGamesEndpoint.GetEndpointVersion());
			}
		}

		// Token: 0x170008F5 RID: 2293
		// (get) Token: 0x0600400D RID: 16397 RVA: 0x00138DA5 File Offset: 0x00136FA5
		// (set) Token: 0x0600400E RID: 16398 RVA: 0x00138DAD File Offset: 0x00136FAD
		public int ScreenCount { get; private set; }

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x0600400F RID: 16399 RVA: 0x00138DB6 File Offset: 0x00136FB6
		// (set) Token: 0x06004010 RID: 16400 RVA: 0x00138DBE File Offset: 0x00136FBE
		public string ScreenPrevious { get; private set; }

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x06004011 RID: 16401 RVA: 0x00138DC7 File Offset: 0x00136FC7
		// (set) Token: 0x06004012 RID: 16402 RVA: 0x00138DD0 File Offset: 0x00136FD0
		public string ScreenCurrent
		{
			get
			{
				return this._screenCurrent;
			}
			set
			{
				this.ScreenPrevious = this._screenCurrent;
				this._screenCurrent = value;
				int screenCount = this.ScreenCount;
				this.ScreenCount = screenCount + 1;
				this.ScreenPreviousTime = (int)(Time.unscaledTime - this._startScreenTime);
				this._startScreenTime = Time.unscaledTime;
			}
		}

		// Token: 0x170008F8 RID: 2296
		// (get) Token: 0x06004013 RID: 16403 RVA: 0x00138E1E File Offset: 0x0013701E
		// (set) Token: 0x06004014 RID: 16404 RVA: 0x00138E26 File Offset: 0x00137026
		public int ScreenPreviousTime { get; private set; }

		// Token: 0x06004015 RID: 16405 RVA: 0x00138E30 File Offset: 0x00137030
		public CrossPromoAnalyticsContext(string crossPromoType)
		{
			this.CrossPromoSessionId = Guid.NewGuid().ToString();
			this.CrossPromoType = crossPromoType;
			this._startScreenTime = Time.unscaledTime;
		}

		// Token: 0x06004016 RID: 16406 RVA: 0x00138E6E File Offset: 0x0013706E
		public override void Resume()
		{
			base.Resume();
			this._startScreenTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x0400299C RID: 10652
		protected string _screenCurrent;

		// Token: 0x0400299D RID: 10653
		private float _startScreenTime;

		// Token: 0x0400299F RID: 10655
		public bool CrossPromoOpenedEventLogged;
	}
}
