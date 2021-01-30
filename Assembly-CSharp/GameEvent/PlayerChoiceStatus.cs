using System;
using System.Runtime.InteropServices;

namespace GameEvent
{
	// Token: 0x0200022C RID: 556
	public struct PlayerChoiceStatus
	{
		// Token: 0x04001270 RID: 4720
		public int source_player_instance_id;

		// Token: 0x04001271 RID: 4721
		public int choice_count;

		// Token: 0x04001272 RID: 4722
		public int choice_hint;

		// Token: 0x04001273 RID: 4723
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public int[] choice_player_instance_id;

		// Token: 0x04001274 RID: 4724
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
		public int[] player_choice;
	}
}
