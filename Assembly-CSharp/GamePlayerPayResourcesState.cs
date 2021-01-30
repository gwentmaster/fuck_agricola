using System;
using System.Runtime.InteropServices;

// Token: 0x02000093 RID: 147
public struct GamePlayerPayResourcesState
{
	// Token: 0x040006C4 RID: 1732
	public int m_PayResourceCount;

	// Token: 0x040006C5 RID: 1733
	public int m_bMayPayPartialCount;

	// Token: 0x040006C6 RID: 1734
	public int m_bLimitedByPlayerSupply;

	// Token: 0x040006C7 RID: 1735
	public int m_bGainResources;

	// Token: 0x040006C8 RID: 1736
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
	public int[] m_PayResourceValues;
}
