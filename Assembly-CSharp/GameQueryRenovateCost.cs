using System;
using System.Runtime.InteropServices;

// Token: 0x0200008F RID: 143
public struct GameQueryRenovateCost
{
	// Token: 0x040006AE RID: 1710
	public uint availableResourcesCardInstanceID;

	// Token: 0x040006AF RID: 1711
	public int possibleCostsCount;

	// Token: 0x040006B0 RID: 1712
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
	public int[] houseType;

	// Token: 0x040006B1 RID: 1713
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 176)]
	public byte[] possibleCostsResources;

	// Token: 0x040006B2 RID: 1714
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
	public ushort[] renovateEffectCardIDs;
}
