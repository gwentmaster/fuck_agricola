using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200020F RID: 527
	public struct PlayerCoupCountryState
	{
		// Token: 0x04001150 RID: 4432
		public int coup_player_index;

		// Token: 0x04001151 RID: 4433
		public int coup_country_id;

		// Token: 0x04001152 RID: 4434
		public int coup_country_stability;

		// Token: 0x04001153 RID: 4435
		public int coup_country_before_influence_ussr;

		// Token: 0x04001154 RID: 4436
		public int coup_country_before_influence_usa;

		// Token: 0x04001155 RID: 4437
		public int coup_source_card_id;

		// Token: 0x04001156 RID: 4438
		public int coup_operations_points;

		// Token: 0x04001157 RID: 4439
		public int coup_operations_bonus;

		// Token: 0x04001158 RID: 4440
		public int coup_operations_adjust_count;

		// Token: 0x04001159 RID: 4441
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] coup_operations_adjust_id;

		// Token: 0x0400115A RID: 4442
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] coup_operations_adjust_value;

		// Token: 0x0400115B RID: 4443
		public int coup_stability_target;

		// Token: 0x0400115C RID: 4444
		public int coup_die_roll_adjust;

		// Token: 0x0400115D RID: 4445
		public int coup_gain_military_ops;

		// Token: 0x0400115E RID: 4446
		public int coup_cuban_missile_crisis;

		// Token: 0x0400115F RID: 4447
		public int coup_degrade_defcon;

		// Token: 0x04001160 RID: 4448
		public int coup_current_defcon;

		// Token: 0x04001161 RID: 4449
		public int coup_phasing_player;

		// Token: 0x04001162 RID: 4450
		public int coup_die_reroll;

		// Token: 0x04001163 RID: 4451
		public int coup_die_result;

		// Token: 0x04001164 RID: 4452
		public int coup_country_after_influence_ussr;

		// Token: 0x04001165 RID: 4453
		public int coup_country_after_influence_usa;
	}
}
