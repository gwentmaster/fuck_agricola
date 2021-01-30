using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200020B RID: 523
	public struct WorkerData
	{
		// Token: 0x0400112E RID: 4398
		public short worker_instance_id;

		// Token: 0x0400112F RID: 4399
		public short owner_instance_id;

		// Token: 0x04001130 RID: 4400
		public short owner_faction_index;

		// Token: 0x04001131 RID: 4401
		public ushort avatar_id;

		// Token: 0x04001132 RID: 4402
		public short assigned_location_instance_id;

		// Token: 0x04001133 RID: 4403
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string owner_name;
	}
}
