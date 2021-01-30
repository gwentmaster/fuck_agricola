using System;
using System.Runtime.InteropServices;

// Token: 0x02000091 RID: 145
public struct GamePlayerCalendar
{
	// Token: 0x040006B8 RID: 1720
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] round;

	// Token: 0x040006B9 RID: 1721
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] flags;

	// Token: 0x040006BA RID: 1722
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] producedType;

	// Token: 0x040006BB RID: 1723
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] producedAmount;

	// Token: 0x040006BC RID: 1724
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] costType;

	// Token: 0x040006BD RID: 1725
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public byte[] costAmount;

	// Token: 0x040006BE RID: 1726
	[MarshalAs(UnmanagedType.ByValArray, SizeConst = 64)]
	public ushort[] cardID;
}
