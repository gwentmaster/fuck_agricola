using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000714 RID: 1812
	public struct TABLE_START
	{
		// Token: 0x04002928 RID: 10536
		public string lobby_session_id;

		// Token: 0x04002929 RID: 10537
		public string match_session_id;

		// Token: 0x0400292A RID: 10538
		public int player_count_slots;

		// Token: 0x0400292B RID: 10539
		public int player_count_human;

		// Token: 0x0400292C RID: 10540
		public int player_count_ai;

		// Token: 0x0400292D RID: 10541
		public int player_clock_sec;

		// Token: 0x0400292E RID: 10542
		public bool is_asynchronous;

		// Token: 0x0400292F RID: 10543
		public bool is_private;

		// Token: 0x04002930 RID: 10544
		public bool is_ranked;

		// Token: 0x04002931 RID: 10545
		public bool is_observable;

		// Token: 0x04002932 RID: 10546
		public bool obs_show_hidden_info;

		// Token: 0x02000A02 RID: 2562
		public enum launch_method
		{
			// Token: 0x040033F4 RID: 13300
			create,
			// Token: 0x040033F5 RID: 13301
			create_automatch,
			// Token: 0x040033F6 RID: 13302
			join,
			// Token: 0x040033F7 RID: 13303
			invite_received
		}

		// Token: 0x02000A03 RID: 2563
		public enum obs_access
		{
			// Token: 0x040033F9 RID: 13305
			friends_only,
			// Token: 0x040033FA RID: 13306
			everyone
		}
	}
}
