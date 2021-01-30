using System;
using System.Runtime.InteropServices;

// Token: 0x02000098 RID: 152
public struct GamePlayerAnimalContainers
{
	// Token: 0x040006DE RID: 1758
	public int containerCount;

	// Token: 0x040006DF RID: 1759
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public ushort[] id;

	// Token: 0x040006E0 RID: 1760
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] type;

	// Token: 0x040006E1 RID: 1761
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] capacity;

	// Token: 0x040006E2 RID: 1762
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] capacityType;

	// Token: 0x040006E3 RID: 1763
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] sheep;

	// Token: 0x040006E4 RID: 1764
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] boar;

	// Token: 0x040006E5 RID: 1765
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
	public byte[] cattle;
}
