using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000736 RID: 1846
	public class TableAnalyticsContext : AnalyticsContext
	{
		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x06004062 RID: 16482 RVA: 0x00139686 File Offset: 0x00137886
		// (set) Token: 0x06004063 RID: 16483 RVA: 0x0013968E File Offset: 0x0013788E
		public string MatchSessionId { get; private set; }

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06004064 RID: 16484 RVA: 0x00139697 File Offset: 0x00137897
		// (set) Token: 0x06004065 RID: 16485 RVA: 0x0013969F File Offset: 0x0013789F
		public int PlayerClockSec { get; private set; }

		// Token: 0x17000917 RID: 2327
		// (get) Token: 0x06004066 RID: 16486 RVA: 0x001396A8 File Offset: 0x001378A8
		// (set) Token: 0x06004067 RID: 16487 RVA: 0x001396B0 File Offset: 0x001378B0
		public bool IsAsynchronous { get; private set; }

		// Token: 0x17000918 RID: 2328
		// (get) Token: 0x06004068 RID: 16488 RVA: 0x001396B9 File Offset: 0x001378B9
		// (set) Token: 0x06004069 RID: 16489 RVA: 0x001396C1 File Offset: 0x001378C1
		public bool IsPrivate { get; private set; }

		// Token: 0x17000919 RID: 2329
		// (get) Token: 0x0600406A RID: 16490 RVA: 0x001396CA File Offset: 0x001378CA
		// (set) Token: 0x0600406B RID: 16491 RVA: 0x001396D2 File Offset: 0x001378D2
		public bool IsRanked { get; private set; }

		// Token: 0x1700091A RID: 2330
		// (get) Token: 0x0600406C RID: 16492 RVA: 0x001396DB File Offset: 0x001378DB
		// (set) Token: 0x0600406D RID: 16493 RVA: 0x001396E3 File Offset: 0x001378E3
		public bool IsObservable { get; private set; }

		// Token: 0x1700091B RID: 2331
		// (get) Token: 0x0600406E RID: 16494 RVA: 0x001396EC File Offset: 0x001378EC
		// (set) Token: 0x0600406F RID: 16495 RVA: 0x001396F4 File Offset: 0x001378F4
		public TABLE_START.obs_access ObsAccess { get; private set; }

		// Token: 0x1700091C RID: 2332
		// (get) Token: 0x06004070 RID: 16496 RVA: 0x001396FD File Offset: 0x001378FD
		// (set) Token: 0x06004071 RID: 16497 RVA: 0x00139705 File Offset: 0x00137905
		public bool ObsShowHiddenInfo { get; private set; }

		// Token: 0x1700091D RID: 2333
		// (get) Token: 0x06004072 RID: 16498 RVA: 0x0013970E File Offset: 0x0013790E
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x06004073 RID: 16499 RVA: 0x00139720 File Offset: 0x00137920
		public TableAnalyticsContext(string matchSessionId, int playerClockSec, bool isAsynchronous, bool isPrivate, bool isRanked, bool isObservable, TABLE_START.obs_access obsAccess, bool obsShowHiddenInfo)
		{
			this.MatchSessionId = matchSessionId;
			this.PlayerClockSec = playerClockSec;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.IsRanked = isRanked;
			this.IsObservable = isObservable;
			this.ObsAccess = obsAccess;
			this.ObsShowHiddenInfo = obsShowHiddenInfo;
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x06004074 RID: 16500 RVA: 0x0013977B File Offset: 0x0013797B
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040029CB RID: 10699
		private float _startLobbyTime;
	}
}
