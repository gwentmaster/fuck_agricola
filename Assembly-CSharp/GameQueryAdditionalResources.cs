using System;
using System.Runtime.InteropServices;

// Token: 0x0200008D RID: 141
public struct GameQueryAdditionalResources
{
	// Token: 0x040006A1 RID: 1697
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
	public byte[] baseResources;

	// Token: 0x040006A2 RID: 1698
	public int additionalEntryCount;

	// Token: 0x040006A3 RID: 1699
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public uint[] sourceCardInstanceID;

	// Token: 0x040006A4 RID: 1700
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public uint[] additionalResourceType;

	// Token: 0x040006A5 RID: 1701
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public int[] triggerAdditionalResourceIndex;

	// Token: 0x040006A6 RID: 1702
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public byte[] costType;

	// Token: 0x040006A7 RID: 1703
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public uint[] costAmount;

	// Token: 0x040006A8 RID: 1704
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 12)]
	public uint[] opponentInstanceID;

	// Token: 0x040006A9 RID: 1705
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 132)]
	public byte[] additionalResources;
}
