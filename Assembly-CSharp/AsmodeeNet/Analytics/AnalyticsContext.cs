using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000706 RID: 1798
	public abstract class AnalyticsContext
	{
		// Token: 0x170008E1 RID: 2273
		// (get) Token: 0x06003FA1 RID: 16289 RVA: 0x00135BE4 File Offset: 0x00133DE4
		public float LifeTime
		{
			get
			{
				return Time.unscaledTime - this._startContextTime;
			}
		}

		// Token: 0x06003FA2 RID: 16290 RVA: 0x00135BF2 File Offset: 0x00133DF2
		public AnalyticsContext()
		{
			this._startContextTime = Time.unscaledTime;
		}

		// Token: 0x06003FA3 RID: 16291 RVA: 0x00135C05 File Offset: 0x00133E05
		public virtual void Pause()
		{
			this._startPauseTime = Time.unscaledTime;
		}

		// Token: 0x06003FA4 RID: 16292 RVA: 0x00135C12 File Offset: 0x00133E12
		public virtual void Resume()
		{
			this._startContextTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x06003FA5 RID: 16293 RVA: 0x00003022 File Offset: 0x00001222
		public virtual void Quit()
		{
		}

		// Token: 0x040028B0 RID: 10416
		private float _startContextTime;

		// Token: 0x040028B1 RID: 10417
		protected float _startPauseTime;
	}
}
