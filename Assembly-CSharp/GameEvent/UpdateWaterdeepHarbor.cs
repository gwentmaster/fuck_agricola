using System;
using System.Runtime.InteropServices;

namespace GameEvent
{
	// Token: 0x0200023D RID: 573
	public struct UpdateWaterdeepHarbor
	{
		// Token: 0x040012A1 RID: 4769
		public uint waterdeep_harbor_closed;

		// Token: 0x040012A2 RID: 4770
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public ushort[] waterdeep_harbor_instance_id;

		// Token: 0x040012A3 RID: 4771
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 3)]
		public ushort[] waterdeep_harbor_open;
	}
}
