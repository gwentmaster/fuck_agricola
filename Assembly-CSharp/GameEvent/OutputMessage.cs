using System;
using System.Runtime.InteropServices;

namespace GameEvent
{
	// Token: 0x02000221 RID: 545
	public struct OutputMessage
	{
		// Token: 0x0400123F RID: 4671
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 128)]
		public string message;

		// Token: 0x04001240 RID: 4672
		public uint messageIndex;
	}
}
