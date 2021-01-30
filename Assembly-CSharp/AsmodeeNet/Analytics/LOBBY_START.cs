using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000712 RID: 1810
	public struct LOBBY_START
	{
		// Token: 0x04002918 RID: 10520
		public string lobby_session_id;

		// Token: 0x04002919 RID: 10521
		public int online_player_count_connected;

		// Token: 0x0400291A RID: 10522
		public int online_player_count_lobbyortable;

		// Token: 0x0400291B RID: 10523
		public int online_player_count_table;

		// Token: 0x0400291C RID: 10524
		public int online_player_count_match;

		// Token: 0x0400291D RID: 10525
		public int online_open_table_count;

		// Token: 0x0400291E RID: 10526
		public int online_ongoing_match_count;
	}
}
