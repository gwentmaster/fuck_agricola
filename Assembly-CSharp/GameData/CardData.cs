using System;
using System.Runtime.InteropServices;

namespace GameData
{
	// Token: 0x0200020C RID: 524
	public struct CardData
	{
		// Token: 0x04001134 RID: 4404
		public short card_instance_id;

		// Token: 0x04001135 RID: 4405
		public short card_type;

		// Token: 0x04001136 RID: 4406
		public short card_category;

		// Token: 0x04001137 RID: 4407
		public ushort card_flags;

		// Token: 0x04001138 RID: 4408
		public short card_deck;

		// Token: 0x04001139 RID: 4409
		public short card_number;

		// Token: 0x0400113A RID: 4410
		public short card_player_count;

		// Token: 0x0400113B RID: 4411
		public short card_victory_points;

		// Token: 0x0400113C RID: 4412
		public short card_orchard_row;

		// Token: 0x0400113D RID: 4413
		public short card_orchard_size;

		// Token: 0x0400113E RID: 4414
		public short unique_id;

		// Token: 0x0400113F RID: 4415
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 10)]
		public byte[] resource_cost;

		// Token: 0x04001140 RID: 4416
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string card_name;

		// Token: 0x04001141 RID: 4417
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
		public string card_title;

		// Token: 0x04001142 RID: 4418
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string scene_name;

		// Token: 0x04001143 RID: 4419
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 256)]
		public string card_effect_text;

		// Token: 0x04001144 RID: 4420
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
		public string card_requirement_text;
	}
}
