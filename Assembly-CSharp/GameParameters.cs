using System;
using System.Runtime.InteropServices;

// Token: 0x020000A4 RID: 164
public struct GameParameters
{
	// Token: 0x0400073E RID: 1854
	public ushort gameType;

	// Token: 0x0400073F RID: 1855
	public ushort deckFlags;

	// Token: 0x04000740 RID: 1856
	public ushort soloGameStartFood;

	// Token: 0x04000741 RID: 1857
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 7)]
	public ushort[] soloGameStartOccupations;

	// Token: 0x04000742 RID: 1858
	public ushort soloGameCount;
}
