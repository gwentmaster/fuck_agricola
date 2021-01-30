using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000217 RID: 535
	public struct GameTurnLogReward
	{
		// Token: 0x040011B7 RID: 4535
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] cardRewardType;

		// Token: 0x040011B8 RID: 4536
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] cardRewardInstanceIDs;

		// Token: 0x040011B9 RID: 4537
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
		public int[] resources;

		// Token: 0x040011BA RID: 4538
		public int bFirstPlayer;

		// Token: 0x040011BB RID: 4539
		public int bLiutenant;

		// Token: 0x040011BC RID: 4540
		public int bAmbassador;

		// Token: 0x040011BD RID: 4541
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] sourceInstanceIDs;
	}
}
