using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000715 RID: 1813
	public struct TABLE_STOP
	{
		// Token: 0x04002933 RID: 10547
		public string match_session_id;

		// Token: 0x04002934 RID: 10548
		public string end_reason;

		// Token: 0x04002935 RID: 10549
		public int player_count_slots;

		// Token: 0x04002936 RID: 10550
		public int player_count_human;

		// Token: 0x04002937 RID: 10551
		public int player_count_ai;

		// Token: 0x04002938 RID: 10552
		public int player_clock_sec;

		// Token: 0x04002939 RID: 10553
		public bool is_asynchronous;

		// Token: 0x0400293A RID: 10554
		public bool is_private;

		// Token: 0x0400293B RID: 10555
		public bool is_ranked;

		// Token: 0x0400293C RID: 10556
		public bool is_observable;

		// Token: 0x0400293D RID: 10557
		public bool obs_show_hidden_info;

		// Token: 0x0400293E RID: 10558
		public int time_active_sec;

		// Token: 0x02000A04 RID: 2564
		public enum obs_access
		{
			// Token: 0x040033FC RID: 13308
			friends_only,
			// Token: 0x040033FD RID: 13309
			everyone
		}
	}
}
