using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200020A RID: 522
	public struct BuildingData
	{
		// Token: 0x04001126 RID: 4390
		public short building_instance_id;

		// Token: 0x04001127 RID: 4391
		public short sourcecard_instance_id;

		// Token: 0x04001128 RID: 4392
		public short owner_index;

		// Token: 0x04001129 RID: 4393
		public short owner_faction_index;

		// Token: 0x0400112A RID: 4394
		public short order_id;

		// Token: 0x0400112B RID: 4395
		public short purchase_vp;

		// Token: 0x0400112C RID: 4396
		public short accumulate_resource;

		// Token: 0x0400112D RID: 4397
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string building_name;
	}
}
