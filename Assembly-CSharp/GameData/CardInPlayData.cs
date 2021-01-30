using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200020D RID: 525
	public struct CardInPlayData
	{
		// Token: 0x04001145 RID: 4421
		public short cardinplay_instance_id;

		// Token: 0x04001146 RID: 4422
		public short sourcecard_instance_id;

		// Token: 0x04001147 RID: 4423
		public short card_orchard_row;

		// Token: 0x04001148 RID: 4424
		public short owner_instance_id;

		// Token: 0x04001149 RID: 4425
		public int convert_count;

		// Token: 0x0400114A RID: 4426
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] convert_instance_id_list;

		// Token: 0x0400114B RID: 4427
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
		public int[] convert_resourcetype_list;

		// Token: 0x0400114C RID: 4428
		public int ability_count;

		// Token: 0x0400114D RID: 4429
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
		public int[] ability_instance_id_list;
	}
}
