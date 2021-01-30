using System;
using System.Runtime.InteropServices;

// Token: 0x02000085 RID: 133
public struct FriendInfo
{
	// Token: 0x0400065D RID: 1629
	public uint userID;

	// Token: 0x0400065E RID: 1630
	public ushort userAvatar;

	// Token: 0x0400065F RID: 1631
	public ushort userRating;

	// Token: 0x04000660 RID: 1632
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
	public string displayName;
}
