using System;
using System.Runtime.InteropServices;

namespace GameEvent
{
	// Token: 0x0200023F RID: 575
	public struct TurnNumber
	{
		// Token: 0x040012A5 RID: 4773
		public int roundNumber;

		// Token: 0x040012A6 RID: 4774
		public int actionCardInstanceID;

		// Token: 0x040012A7 RID: 4775
		public int playerFaction;

		// Token: 0x040012A8 RID: 4776
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string playerName;
	}
}
