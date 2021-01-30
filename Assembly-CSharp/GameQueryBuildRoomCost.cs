using System;
using System.Runtime.InteropServices;

// Token: 0x02000090 RID: 144
public struct GameQueryBuildRoomCost
{
	// Token: 0x040006B3 RID: 1715
	public int houseType;

	// Token: 0x040006B4 RID: 1716
	public uint availableResourcesCardInstanceID;

	// Token: 0x040006B5 RID: 1717
	public int possibleCostsCount;

	// Token: 0x040006B6 RID: 1718
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 264)]
	public byte[] possibleCostsResources;

	// Token: 0x040006B7 RID: 1719
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 192)]
	public ushort[] buildRoomEffectCardIDs;
}
