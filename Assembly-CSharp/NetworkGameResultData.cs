using System;
using System.Runtime.InteropServices;

// Token: 0x020000B8 RID: 184
public struct NetworkGameResultData
{
	// Token: 0x040007FF RID: 2047
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public NetworkGameResultPlayerData[] result_player_data;
}
