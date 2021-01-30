using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000710 RID: 1808
	public struct MATCH_START
	{
		// Token: 0x040028F3 RID: 10483
		public string match_session_id;

		// Token: 0x040028F4 RID: 10484
		public string lobby_session_id;

		// Token: 0x040028F5 RID: 10485
		public string mode;

		// Token: 0x040028F6 RID: 10486
		public string map_id;

		// Token: 0x040028F7 RID: 10487
		public string activated_dlc;

		// Token: 0x040028F8 RID: 10488
		public int player_count_human;

		// Token: 0x040028F9 RID: 10489
		public int player_count_ai;

		// Token: 0x040028FA RID: 10490
		public int? player_playorder;

		// Token: 0x040028FB RID: 10491
		public string launch_method;

		// Token: 0x040028FC RID: 10492
		public int? player_clock_sec;

		// Token: 0x040028FD RID: 10493
		public string difficulty;

		// Token: 0x040028FE RID: 10494
		public bool is_online;

		// Token: 0x040028FF RID: 10495
		public bool is_tutorial;

		// Token: 0x04002900 RID: 10496
		public bool? is_asynchronous;

		// Token: 0x04002901 RID: 10497
		public bool? is_private;

		// Token: 0x04002902 RID: 10498
		public bool? is_ranked;

		// Token: 0x04002903 RID: 10499
		public bool? is_observable;

		// Token: 0x04002904 RID: 10500
		public bool? obs_show_hidden_info;

		// Token: 0x020009FF RID: 2559
		public enum obs_access
		{
			// Token: 0x040033EA RID: 13290
			friends_only,
			// Token: 0x040033EB RID: 13291
			everyone
		}
	}
}
