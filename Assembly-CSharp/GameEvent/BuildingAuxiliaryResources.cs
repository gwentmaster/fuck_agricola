using System;
using System.Runtime.InteropServices;

namespace GameEvent
{
	// Token: 0x0200023C RID: 572
	public struct BuildingAuxiliaryResources
	{
		// Token: 0x0400129F RID: 4767
		public int cardinplay_instance_id;

		// Token: 0x040012A0 RID: 4768
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public short[] auxiliary_resources;
	}
}
