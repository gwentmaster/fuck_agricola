using System;
using System.Runtime.InteropServices;

// Token: 0x02000055 RID: 85
public struct GameOption
{
	// Token: 0x04000446 RID: 1094
	public int optionIndex;

	// Token: 0x04000447 RID: 1095
	public ushort selectionID;

	// Token: 0x04000448 RID: 1096
	public ushort selectionHint;

	// Token: 0x04000449 RID: 1097
	public byte isHidden;

	// Token: 0x0400044A RID: 1098
	[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 75)]
	public string optionText;
}
