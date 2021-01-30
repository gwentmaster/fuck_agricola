using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000713 RID: 1811
	public struct LOBBY_STOP
	{
		// Token: 0x0400291F RID: 10527
		public string lobby_session_id;

		// Token: 0x04002920 RID: 10528
		public int online_player_count_connected;

		// Token: 0x04002921 RID: 10529
		public int online_player_count_lobbyortable;

		// Token: 0x04002922 RID: 10530
		public int online_player_count_table;

		// Token: 0x04002923 RID: 10531
		public int online_player_count_match;

		// Token: 0x04002924 RID: 10532
		public int online_open_table_count;

		// Token: 0x04002925 RID: 10533
		public int online_ongoing_match_count;

		// Token: 0x04002926 RID: 10534
		public int time_active_sec;

		// Token: 0x04002927 RID: 10535
		public string end_reason;
	}
}
