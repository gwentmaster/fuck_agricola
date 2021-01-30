using System;
using System.Runtime.InteropServices;

// Token: 0x02000094 RID: 148
public struct GameActionDefinition
{
	// Token: 0x040006C9 RID: 1737
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
	public string displayName;

	// Token: 0x040006CA RID: 1738
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
	public int[] resources;
}
