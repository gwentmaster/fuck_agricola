using System;
using System.Runtime.InteropServices;

// Token: 0x02000087 RID: 135
public struct NetworkPlayerProfileInfo
{
	// Token: 0x04000666 RID: 1638
	public uint userID;

	// Token: 0x04000667 RID: 1639
	public uint inProgressGames;

	// Token: 0x04000668 RID: 1640
	public NetworkPlayerGameTypeInfo userGameStats;

	// Token: 0x04000669 RID: 1641
	public ushort userAvatar;

	// Token: 0x0400066A RID: 1642
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string displayName;
}
