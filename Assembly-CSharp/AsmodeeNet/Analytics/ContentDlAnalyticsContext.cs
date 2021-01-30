using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072D RID: 1837
	public class ContentDlAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008EE RID: 2286
		// (get) Token: 0x06003FFF RID: 16383 RVA: 0x00138C63 File Offset: 0x00136E63
		// (set) Token: 0x06004000 RID: 16384 RVA: 0x00138C6B File Offset: 0x00136E6B
		public string DlSessionId { get; private set; }

		// Token: 0x170008EF RID: 2287
		// (get) Token: 0x06004001 RID: 16385 RVA: 0x00138C74 File Offset: 0x00136E74
		// (set) Token: 0x06004002 RID: 16386 RVA: 0x00138C7C File Offset: 0x00136E7C
		public string DlContentId { get; private set; }

		// Token: 0x170008F0 RID: 2288
		// (get) Token: 0x06004003 RID: 16387 RVA: 0x00138C85 File Offset: 0x00136E85
		public int DlTime
		{
			get
			{
				return (int)(Time.unscaledTime - this._startDlTime);
			}
		}

		// Token: 0x06004004 RID: 16388 RVA: 0x00138C94 File Offset: 0x00136E94
		public ContentDlAnalyticsContext(string dlContentId)
		{
			this.DlSessionId = Guid.NewGuid().ToString();
			this.DlContentId = dlContentId;
			this._startDlTime = Time.unscaledTime;
		}

		// Token: 0x06004005 RID: 16389 RVA: 0x00138CD2 File Offset: 0x00136ED2
		public override void Resume()
		{
			base.Resume();
			this._startDlTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x04002996 RID: 10646
		private float _startDlTime;
	}
}
