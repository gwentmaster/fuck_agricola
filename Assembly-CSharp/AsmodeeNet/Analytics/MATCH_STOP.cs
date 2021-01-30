using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000711 RID: 1809
	public struct MATCH_STOP
	{
		// Token: 0x04002905 RID: 10501
		public string match_session_id;

		// Token: 0x04002906 RID: 10502
		public string mode;

		// Token: 0x04002907 RID: 10503
		public string map_id;

		// Token: 0x04002908 RID: 10504
		public string activated_dlc;

		// Token: 0x04002909 RID: 10505
		public int player_count_human;

		// Token: 0x0400290A RID: 10506
		public int player_count_ai;

		// Token: 0x0400290B RID: 10507
		public int? player_playorder;

		// Token: 0x0400290C RID: 10508
		public int time_active_sec;

		// Token: 0x0400290D RID: 10509
		public string end_reason;

		// Token: 0x0400290E RID: 10510
		public int? player_clock_sec;

		// Token: 0x0400290F RID: 10511
		public string difficulty;

		// Token: 0x04002910 RID: 10512
		public bool is_online;

		// Token: 0x04002911 RID: 10513
		public bool is_tutorial;

		// Token: 0x04002912 RID: 10514
		public bool? is_asynchronous;

		// Token: 0x04002913 RID: 10515
		public bool? is_private;

		// Token: 0x04002914 RID: 10516
		public bool? is_ranked;

		// Token: 0x04002915 RID: 10517
		public bool? is_observable;

		// Token: 0x04002916 RID: 10518
		public bool? obs_show_hidden_info;

		// Token: 0x04002917 RID: 10519
		public int? turn_count;

		// Token: 0x02000A00 RID: 2560
		public enum player_result
		{
			// Token: 0x040033ED RID: 13293
			victory,
			// Token: 0x040033EE RID: 13294
			defeat,
			// Token: 0x040033EF RID: 13295
			draw
		}

		// Token: 0x02000A01 RID: 2561
		public enum obs_access
		{
			// Token: 0x040033F1 RID: 13297
			friends_only,
			// Token: 0x040033F2 RID: 13298
			everyone
		}
	}
}
