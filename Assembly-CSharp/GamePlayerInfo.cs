using System;
using System.Runtime.InteropServices;

// Token: 0x02000088 RID: 136
public struct GamePlayerInfo
{
	// Token: 0x0400066B RID: 1643
	public uint userID;

	// Token: 0x0400066C RID: 1644
	public uint forfeit;

	// Token: 0x0400066D RID: 1645
	public ushort avatarIndex;

	// Token: 0x0400066E RID: 1646
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string displayName;
}
