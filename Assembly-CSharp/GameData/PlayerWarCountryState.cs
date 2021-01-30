using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000211 RID: 529
	public struct PlayerWarCountryState
	{
		// Token: 0x04001177 RID: 4471
		public int war_player_index;

		// Token: 0x04001178 RID: 4472
		public int war_country_id;

		// Token: 0x04001179 RID: 4473
		public int war_country_before_influence_ussr;

		// Token: 0x0400117A RID: 4474
		public int war_country_before_influence_usa;

		// Token: 0x0400117B RID: 4475
		public int war_source_card_id;

		// Token: 0x0400117C RID: 4476
		public int war_success_roll_target;

		// Token: 0x0400117D RID: 4477
		public int war_victory_point_gain;

		// Token: 0x0400117E RID: 4478
		public int war_military_ops_gain;

		// Token: 0x0400117F RID: 4479
		public int war_die_adjust_opponent_control;

		// Token: 0x04001180 RID: 4480
		public int war_die_adjust_chinese_civil_war;

		// Token: 0x04001181 RID: 4481
		public int war_die_adjust_adjacent_control;

		// Token: 0x04001182 RID: 4482
		public int war_adjacent_control_count;

		// Token: 0x04001183 RID: 4483
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] war_adjacent_control;

		// Token: 0x04001184 RID: 4484
		public int war_die_result;

		// Token: 0x04001185 RID: 4485
		public int war_country_after_influence_ussr;

		// Token: 0x04001186 RID: 4486
		public int war_country_after_influence_usa;
	}
}
