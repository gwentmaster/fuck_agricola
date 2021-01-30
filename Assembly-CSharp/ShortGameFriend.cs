using System;
using System.Runtime.InteropServices;

// Token: 0x020000AF RID: 175
[Serializable]
public struct ShortGameFriend
{
	// Token: 0x040007CE RID: 1998
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
	public string displayName;

	// Token: 0x040007CF RID: 1999
	public ushort avatarIndex;

	// Token: 0x040007D0 RID: 2000
	public uint userID;

	// Token: 0x040007D1 RID: 2001
	public ushort userRating;
}
