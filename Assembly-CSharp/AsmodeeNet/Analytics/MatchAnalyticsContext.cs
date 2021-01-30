using System;
using UnityEngine;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000730 RID: 1840
	public class MatchAnalyticsContext : AnalyticsContext
	{
		// Token: 0x170008FB RID: 2299
		// (get) Token: 0x0600401C RID: 16412 RVA: 0x00138F08 File Offset: 0x00137108
		// (set) Token: 0x0600401D RID: 16413 RVA: 0x00138F10 File Offset: 0x00137110
		public string MatchSessionId { get; private set; }

		// Token: 0x170008FC RID: 2300
		// (get) Token: 0x0600401E RID: 16414 RVA: 0x00138F19 File Offset: 0x00137119
		// (set) Token: 0x0600401F RID: 16415 RVA: 0x00138F21 File Offset: 0x00137121
		public string Mode { get; private set; }

		// Token: 0x170008FD RID: 2301
		// (get) Token: 0x06004020 RID: 16416 RVA: 0x00138F2A File Offset: 0x0013712A
		// (set) Token: 0x06004021 RID: 16417 RVA: 0x00138F32 File Offset: 0x00137132
		public string MapId { get; private set; }

		// Token: 0x170008FE RID: 2302
		// (get) Token: 0x06004022 RID: 16418 RVA: 0x00138F3B File Offset: 0x0013713B
		// (set) Token: 0x06004023 RID: 16419 RVA: 0x00138F43 File Offset: 0x00137143
		public string ActivatedDlc { get; private set; }

		// Token: 0x170008FF RID: 2303
		// (get) Token: 0x06004024 RID: 16420 RVA: 0x00138F4C File Offset: 0x0013714C
		// (set) Token: 0x06004025 RID: 16421 RVA: 0x00138F54 File Offset: 0x00137154
		public int? PlayerPlayOrder { get; private set; }

		// Token: 0x17000900 RID: 2304
		// (get) Token: 0x06004026 RID: 16422 RVA: 0x00138F5D File Offset: 0x0013715D
		// (set) Token: 0x06004027 RID: 16423 RVA: 0x00138F65 File Offset: 0x00137165
		public int? PlayerClockSec { get; private set; }

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06004028 RID: 16424 RVA: 0x00138F6E File Offset: 0x0013716E
		// (set) Token: 0x06004029 RID: 16425 RVA: 0x00138F76 File Offset: 0x00137176
		public string Difficulty { get; private set; }

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x0600402A RID: 16426 RVA: 0x00138F7F File Offset: 0x0013717F
		// (set) Token: 0x0600402B RID: 16427 RVA: 0x00138F87 File Offset: 0x00137187
		public bool IsOnline { get; private set; }

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x0600402C RID: 16428 RVA: 0x00138F90 File Offset: 0x00137190
		// (set) Token: 0x0600402D RID: 16429 RVA: 0x00138F98 File Offset: 0x00137198
		public bool IsTutorial { get; private set; }

		// Token: 0x17000904 RID: 2308
		// (get) Token: 0x0600402E RID: 16430 RVA: 0x00138FA1 File Offset: 0x001371A1
		// (set) Token: 0x0600402F RID: 16431 RVA: 0x00138FA9 File Offset: 0x001371A9
		public bool? IsAsynchronous { get; private set; }

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x06004030 RID: 16432 RVA: 0x00138FB2 File Offset: 0x001371B2
		// (set) Token: 0x06004031 RID: 16433 RVA: 0x00138FBA File Offset: 0x001371BA
		public bool? IsPrivate { get; private set; }

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06004032 RID: 16434 RVA: 0x00138FC3 File Offset: 0x001371C3
		// (set) Token: 0x06004033 RID: 16435 RVA: 0x00138FCB File Offset: 0x001371CB
		public bool? IsRanked { get; private set; }

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06004034 RID: 16436 RVA: 0x00138FD4 File Offset: 0x001371D4
		// (set) Token: 0x06004035 RID: 16437 RVA: 0x00138FDC File Offset: 0x001371DC
		public bool? IsObservable { get; private set; }

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06004036 RID: 16438 RVA: 0x00138FE5 File Offset: 0x001371E5
		// (set) Token: 0x06004037 RID: 16439 RVA: 0x00138FED File Offset: 0x001371ED
		public MATCH_START.obs_access? ObsAccess { get; private set; }

		// Token: 0x17000909 RID: 2313
		// (get) Token: 0x06004038 RID: 16440 RVA: 0x00138FF6 File Offset: 0x001371F6
		// (set) Token: 0x06004039 RID: 16441 RVA: 0x00138FFE File Offset: 0x001371FE
		public bool? ObsShowHiddenInfo { get; private set; }

		// Token: 0x1700090A RID: 2314
		// (get) Token: 0x0600403A RID: 16442 RVA: 0x00139007 File Offset: 0x00137207
		public int TimeActiveSec
		{
			get
			{
				return (int)(Time.unscaledTime - this._startLobbyTime);
			}
		}

		// Token: 0x0600403B RID: 16443 RVA: 0x00139018 File Offset: 0x00137218
		public MatchAnalyticsContext(string matchSessionId, string mode, string mapId, string activatedDlc, int? playerPlayOrder, int? playerClockSec, string difficulty, bool isOnline, bool isTutorial, bool? isAsynchronous, bool? isPrivate, bool? isRanked, bool? isObservable, MATCH_START.obs_access? obsAccess, bool? obsShowHiddenInfo)
		{
			this.MatchSessionId = matchSessionId;
			this.Mode = mode;
			this.MapId = mapId;
			this.ActivatedDlc = activatedDlc;
			this.PlayerPlayOrder = playerPlayOrder;
			this.PlayerClockSec = playerClockSec;
			this.Difficulty = difficulty;
			this.IsOnline = isOnline;
			this.IsTutorial = isTutorial;
			this.IsAsynchronous = isAsynchronous;
			this.IsPrivate = isPrivate;
			this.IsRanked = isRanked;
			this.IsObservable = isObservable;
			this.ObsAccess = obsAccess;
			this.ObsShowHiddenInfo = obsShowHiddenInfo;
			this._startLobbyTime = Time.unscaledTime;
		}

		// Token: 0x0600403C RID: 16444 RVA: 0x001390AB File Offset: 0x001372AB
		public override void Resume()
		{
			base.Resume();
			this._startLobbyTime += Time.unscaledTime - this._startPauseTime;
		}

		// Token: 0x040029B1 RID: 10673
		private float _startLobbyTime;
	}
}
