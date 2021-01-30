using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000210 RID: 528
	public struct PlayerRealignmentCountryState
	{
		// Token: 0x04001166 RID: 4454
		public int realign_player_index;

		// Token: 0x04001167 RID: 4455
		public int realign_country_id;

		// Token: 0x04001168 RID: 4456
		public int realign_country_before_influence_ussr;

		// Token: 0x04001169 RID: 4457
		public int realign_country_before_influence_usa;

		// Token: 0x0400116A RID: 4458
		public int realign_source_card_id;

		// Token: 0x0400116B RID: 4459
		public int realign_roll_count;

		// Token: 0x0400116C RID: 4460
		public int realign_roll_total;

		// Token: 0x0400116D RID: 4461
		public int realign_die_roll_adjust_ussr;

		// Token: 0x0400116E RID: 4462
		public int realign_die_roll_bonus_ussr;

		// Token: 0x0400116F RID: 4463
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] realign_adjacent_control_ussr;

		// Token: 0x04001170 RID: 4464
		public int realign_die_roll_adjust_usa;

		// Token: 0x04001171 RID: 4465
		public int realign_die_roll_bonus_usa;

		// Token: 0x04001172 RID: 4466
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] realign_adjacent_control_usa;

		// Token: 0x04001173 RID: 4467
		public int realign_die_result_ussr;

		// Token: 0x04001174 RID: 4468
		public int realign_die_result_usa;

		// Token: 0x04001175 RID: 4469
		public int realign_country_after_influence_ussr;

		// Token: 0x04001176 RID: 4470
		public int realign_country_after_influence_usa;
	}
}
