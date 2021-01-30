using System;
using System.Runtime.InteropServices;

// Token: 0x0200008C RID: 140
public struct GamePlayerFarmTileState
{
	// Token: 0x0400069E RID: 1694
	public int tileType;

	// Token: 0x0400069F RID: 1695
	public int hasStable;

	// Token: 0x040006A0 RID: 1696
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
	public int[] data0;
}
