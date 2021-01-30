using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200072F RID: 1839
	public class LobbyAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008F9 RID: 2297
		// (get) Token: 0x06004017 RID: 16407 RVA: 0x00138E8F File Offset: 0x0013708F
		// (set) Token: 0x06004018 RID: 16408 RVA: 0x00138E97 File Offset: 0x00137097
		public string LobbySessionId { get; private set; }

		// Token: 0x170008FA RID: 2298
		// (get) Token: 0x06004019 RID: 16409 RVA: 0x00138EA0 File Offset: 0x001370A0
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x0600401A RID: 16410 RVA: 0x00138EB0 File Offset: 0x001370B0
		public LobbyAnalyticsContext()
		{
			this.LobbySessionId = Guid.NewGuid().ToString();
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x0600401B RID: 16411 RVA: 0x00138EE7 File Offset: 0x001370E7
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040029A1 RID: 10657
		private float _startLobbyTime;
	}
}
