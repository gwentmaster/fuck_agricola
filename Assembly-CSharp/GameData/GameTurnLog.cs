using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000219 RID: 537
	public struct GameTurnLog
	{
		// Token: 0x040011C5 RID: 4549
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 40)]
		public GameTurnLogEntry[] entries;
	}
}
