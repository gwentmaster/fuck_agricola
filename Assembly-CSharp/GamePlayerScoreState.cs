using System;
using System.Runtime.InteropServices;

// Token: 0x02000096 RID: 150
public struct GamePlayerScoreState
{
	// Token: 0x040006D0 RID: 1744
	public int playerFaction;

	// Token: 0x040006D1 RID: 1745
	public int workerAvatarID;

	// Token: 0x040006D2 RID: 1746
	public int soloGameCount;

	// Token: 0x040006D3 RID: 1747
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string displayName;

	// Token: 0x040006D4 RID: 1748
	public int total_points;

	// Token: 0x040006D5 RID: 1749
	public int total_resources;

	// Token: 0x040006D6 RID: 1750
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
	public int[] count;

	// Token: 0x040006D7 RID: 1751
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
	public int[] score;

	// Token: 0x040006D8 RID: 1752
	public int bonus_point_entry_count;

	// Token: 0x040006D9 RID: 1753
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public ushort[] bonusIDs;

	// Token: 0x040006DA RID: 1754
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public int[] bonusPoints;
}
