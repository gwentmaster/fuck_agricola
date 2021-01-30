using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200021D RID: 541
	public struct ChatChannelMessageList
	{
		// Token: 0x040011D7 RID: 4567
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
		public ChatChannelMessageEntry[] entries;
	}
}
