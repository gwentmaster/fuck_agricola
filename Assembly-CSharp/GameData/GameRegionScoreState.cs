using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000214 RID: 532
	public struct GameRegionScoreState
	{
		// Token: 0x04001196 RID: 4502
		public int score_region_index;

		// Token: 0x04001197 RID: 4503
		public int score_card_number;

		// Token: 0x04001198 RID: 4504
		public int total_battleground_count;

		// Token: 0x04001199 RID: 4505
		public int control_battleground_count_ussr;

		// Token: 0x0400119A RID: 4506
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] control_battleground_list_ussr;

		// Token: 0x0400119B RID: 4507
		public int control_nonbattleground_count_ussr;

		// Token: 0x0400119C RID: 4508
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public int[] control_nonbattleground_list_ussr;

		// Token: 0x0400119D RID: 4509
		public int control_adjacent_to_enemy_count_ussr;

		// Token: 0x0400119E RID: 4510
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] control_adjacent_to_enemy_list_ussr;

		// Token: 0x0400119F RID: 4511
		public int player_score_state_ussr;

		// Token: 0x040011A0 RID: 4512
		public int player_state_victory_points_ussr;

		// Token: 0x040011A1 RID: 4513
		public int region_victory_points_ussr;

		// Token: 0x040011A2 RID: 4514
		public int control_battleground_count_usa;

		// Token: 0x040011A3 RID: 4515
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] control_battleground_list_usa;

		// Token: 0x040011A4 RID: 4516
		public int control_nonbattleground_count_usa;

		// Token: 0x040011A5 RID: 4517
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public int[] control_nonbattleground_list_usa;

		// Token: 0x040011A6 RID: 4518
		public int control_adjacent_to_enemy_count_usa;

		// Token: 0x040011A7 RID: 4519
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] control_adjacent_to_enemy_list_usa;

		// Token: 0x040011A8 RID: 4520
		public int player_score_state_usa;

		// Token: 0x040011A9 RID: 4521
		public int player_state_victory_points_usa;

		// Token: 0x040011AA RID: 4522
		public int region_victory_points_usa;
	}
}
