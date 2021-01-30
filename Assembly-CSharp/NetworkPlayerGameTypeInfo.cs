using System;
using System.Runtime.InteropServices;

// Token: 0x02000086 RID: 134
public struct NetworkPlayerGameTypeInfo
{
	// Token: 0x04000661 RID: 1633
	public ushort userRating;

	// Token: 0x04000662 RID: 1634
	public uint completedGames;

	// Token: 0x04000663 RID: 1635
	public uint forfeits;

	// Token: 0x04000664 RID: 1636
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	public uint[] wins;

	// Token: 0x04000665 RID: 1637
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 5)]
	public uint[] losses;
}
