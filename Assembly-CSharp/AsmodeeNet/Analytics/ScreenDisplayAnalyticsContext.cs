using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000731 RID: 1841
	public class ScreenDisplayAnalyticsContext : AnalyticsContext
	{
		// Token: 0x1700090B RID: 2315
		// (get) Token: 0x0600403D RID: 16445 RVA: 0x001390CC File Offset: 0x001372CC
		// (set) Token: 0x0600403E RID: 16446 RVA: 0x001390D4 File Offset: 0x001372D4
		public int ScreenCount { get; private set; }

		// Token: 0x1700090C RID: 2316
		// (get) Token: 0x0600403F RID: 16447 RVA: 0x001390DD File Offset: 0x001372DD
		// (set) Token: 0x06004040 RID: 16448 RVA: 0x001390E5 File Offset: 0x001372E5
		public string ScreenPrevious { get; private set; }

		// Token: 0x1700090D RID: 2317
		// (get) Token: 0x06004041 RID: 16449 RVA: 0x001390EE File Offset: 0x001372EE
		// (set) Token: 0x06004042 RID: 16450 RVA: 0x001390F6 File Offset: 0x001372F6
		public string Context { get; set; }

		// Token: 0x1700090E RID: 2318
		// (get) Token: 0x06004043 RID: 16451 RVA: 0x001390FF File Offset: 0x001372FF
		// (set) Token: 0x06004044 RID: 16452 RVA: 0x00139108 File Offset: 0x00137308
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
				this.ScreenPreviousTime = new int?((int)(Time.unscaledTime - this._startScreenTime));
				this._startScreenTime = Time.unscaledTime;
			}
		}

		// Token: 0x1700090F RID: 2319
		// (get) Token: 0x06004045 RID: 16453 RVA: 0x0013915B File Offset: 0x0013735B
		// (set) Token: 0x06004046 RID: 16454 RVA: 0x00139163 File Offset: 0x00137363
		public int? ScreenPreviousTime { get; private set; } = new int?(0);

		// Token: 0x06004047 RID: 16455 RVA: 0x0013916C File Offset: 0x0013736C
		public ScreenDisplayAnalyticsContext(string screenCurrent)
		{
			this._startScreenTime = Time.unscaledTime;
			this.ScreenCurrent = screenCurrent;
			this.ScreenPreviousTime = null;
		}

		// Token: 0x06004048 RID: 16456 RVA: 0x001391AC File Offset: 0x001373AC
		public override void Resume()
		{
			base.Resume();
			this._startScreenTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040029B5 RID: 10677
		private string _screenCurrent;

		// Token: 0x040029B6 RID: 10678
		private float _startScreenTime;
	}
}
