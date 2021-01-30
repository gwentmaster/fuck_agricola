using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200021A RID: 538
	public struct GameTurnPlayedCards
	{
		// Token: 0x040011C6 RID: 4550
		public int action_round_count_ussr;

		// Token: 0x040011C7 RID: 4551
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] played_cards_ussr;

		// Token: 0x040011C8 RID: 4552
		public int action_round_count_usa;

		// Token: 0x040011C9 RID: 4553
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public int[] played_cards_usa;
	}
}
