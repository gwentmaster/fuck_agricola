using System;
using System.Runtime.InteropServices;

// Token: 0x0200008E RID: 142
public struct GameQueryBuildImprovementCost
{
	// Token: 0x040006AA RID: 1706
	public uint buildingCardInstanceID;

	// Token: 0x040006AB RID: 1707
	public int possibleCostsCount;

	// Token: 0x040006AC RID: 1708
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 176)]
	public byte[] possibleCostsResources;

	// Token: 0x040006AD RID: 1709
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 112)]
	public ushort[] buildImprovementEffectCardIDs;
}
