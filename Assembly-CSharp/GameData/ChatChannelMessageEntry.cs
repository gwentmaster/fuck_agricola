using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200021C RID: 540
	public struct ChatChannelMessageEntry
	{
		// Token: 0x040011D2 RID: 4562
		public uint chatMessageIndex;

		// Token: 0x040011D3 RID: 4563
		public uint chatUserID;

		// Token: 0x040011D4 RID: 4564
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chatUserDisplayName;

		// Token: 0x040011D5 RID: 4565
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string chatTimestamp;

		// Token: 0x040011D6 RID: 4566
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 140)]
		public string chatMessage;
	}
}
