using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072B RID: 1835
	public class ApplicationAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008E6 RID: 2278
		// (get) Token: 0x06003FEC RID: 16364 RVA: 0x001389FF File Offset: 0x00136BFF
		// (set) Token: 0x06003FED RID: 16365 RVA: 0x00138A07 File Offset: 0x00136C07
		public string AppBootSessionId { get; private set; }

		// Token: 0x170008E7 RID: 2279
		// (get) Token: 0x06003FEE RID: 16366 RVA: 0x00138A10 File Offset: 0x00136C10
		public int TimeSession
		{
			get
			{
				return (int)(Time.unscaledTime - this._startApplicationTime);
			}
		}

		// Token: 0x170008E8 RID: 2280
		// (get) Token: 0x06003FEF RID: 16367 RVA: 0x00138A1F File Offset: 0x00136C1F
		public int TimeSessionGamePlay
		{
			get
			{
				return (int)(this._finishedTimeSessionGameplay + this._RunningTimeSessionGameplay);
			}
		}

		// Token: 0x170008E9 RID: 2281
		// (get) Token: 0x06003FF0 RID: 16368 RVA: 0x00138A2F File Offset: 0x00136C2F
		public int TimeLifeToDate
		{
			get
			{
				return KeyValueStore.GetInt("CumulatedTimeFinishedSession", 0) + this.TimeSession;
			}
		}

		// Token: 0x170008EA RID: 2282
		// (get) Token: 0x06003FF1 RID: 16369 RVA: 0x00138A43 File Offset: 0x00136C43
		public int TimeLifeToDateGameplay
		{
			get
			{
				return KeyValueStore.GetInt("CumulatedTimeGameplayFinishedSession", 0) + this.TimeSessionGamePlay;
			}
		}

		// Token: 0x170008EB RID: 2283
		// (get) Token: 0x06003FF2 RID: 16370 RVA: 0x00138A58 File Offset: 0x00136C58
		private float _RunningTimeSessionGameplay
		{
			get
			{
				return (float)((this._startGameplayTime != null) ? ((int)(Time.unscaledTime - this._startGameplayTime).Value) : 0);
			}
		}

		// Token: 0x06003FF3 RID: 16371 RVA: 0x00138AB0 File Offset: 0x00136CB0
		public ApplicationAnalyticsContext()
		{
			this.AppBootSessionId = Guid.NewGuid().ToString();
			this._startApplicationTime = Time.unscaledTime;
			int @int = KeyValueStore.GetInt("TimePreviousSession", 0);
			KeyValueStore.SetInt("CumulatedTimeFinishedSession", KeyValueStore.GetInt("CumulatedTimeFinishedSession", 0) + @int);
			int int2 = KeyValueStore.GetInt("TimeGameplayPreviousSession", 0);
			KeyValueStore.SetInt("CumulatedTimeGameplayFinishedSession", KeyValueStore.GetInt("CumulatedTimeGameplayFinishedSession", 0) + int2);
		}

		// Token: 0x06003FF4 RID: 16372 RVA: 0x00138B2D File Offset: 0x00136D2D
		public override void Pause()
		{
			base.Pause();
			this._SaveLifeToDateInfo();
		}

		// Token: 0x06003FF5 RID: 16373 RVA: 0x00138B3C File Offset: 0x00136D3C
		public override void Resume()
		{
			base.Resume();
			float num = Time.unscaledTime - this._startPauseTime;
			this._startApplicationTime += num;
			if (this._startGameplayTime != null)
			{
				this._startGameplayTime += num;
			}
		}

		// Token: 0x06003FF6 RID: 16374 RVA: 0x00138BA8 File Offset: 0x00136DA8
		public override void Quit()
		{
			base.Quit();
			this._SaveLifeToDateInfo();
		}

		// Token: 0x06003FF7 RID: 16375 RVA: 0x00138BB6 File Offset: 0x00136DB6
		private void _SaveLifeToDateInfo()
		{
			KeyValueStore.SetInt("TimePreviousSession", this.TimeSession);
			KeyValueStore.SetInt("TimeGameplayPreviousSession", this.TimeSessionGamePlay);
		}

		// Token: 0x06003FF8 RID: 16376 RVA: 0x00138BD8 File Offset: 0x00136DD8
		public void StartGameplay()
		{
			this._startGameplayTime = new float?(Time.unscaledTime);
		}

		// Token: 0x06003FF9 RID: 16377 RVA: 0x00138BEA File Offset: 0x00136DEA
		public void StopGameplay()
		{
			this._finishedTimeSessionGameplay += this._RunningTimeSessionGameplay;
			this._startGameplayTime = null;
		}

		// Token: 0x0400298A RID: 10634
		private const string _kTimePreviousSession = "TimePreviousSession";

		// Token: 0x0400298B RID: 10635
		private const string _kCumulatedTimeFinishedSession = "CumulatedTimeFinishedSession";

		// Token: 0x0400298C RID: 10636
		private const string _kTimeGameplayPreviousSession = "TimeGameplayPreviousSession";

		// Token: 0x0400298D RID: 10637
		private const string _kCumulatedTimeGameplayFinishedSession = "CumulatedTimeGameplayFinishedSession";

		// Token: 0x0400298F RID: 10639
		private float _startApplicationTime;

		// Token: 0x04002990 RID: 10640
		private float? _startGameplayTime;

		// Token: 0x04002991 RID: 10641
		private float _finishedTimeSessionGameplay;
	}
}
