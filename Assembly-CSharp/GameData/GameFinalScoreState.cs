using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000216 RID: 534
	public struct GameFinalScoreState
	{
		// Token: 0x040011B5 RID: 4533
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public GameFinalRegionScoreState[] region;

		// Token: 0x040011B6 RID: 4534
		public int china_card;
	}
}
