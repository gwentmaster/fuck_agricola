using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x02000218 RID: 536
	public struct GameTurnLogEntry
	{
		// Token: 0x040011BE RID: 4542
		public uint player;

		// Token: 0x040011BF RID: 4543
		public byte round;

		// Token: 0x040011C0 RID: 4544
		public byte cardType;

		// Token: 0x040011C1 RID: 4545
		public byte logType;

		// Token: 0x040011C2 RID: 4546
		public ushort cardInstanceID;

		// Token: 0x040011C3 RID: 4547
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string cardName;

		// Token: 0x040011C4 RID: 4548
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 11)]
		public short[] scoreChange;
	}
}
