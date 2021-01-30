using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200070B RID: 1803
	public struct CROSSPROMO_SCREEN_DISPLAY
	{
		// Token: 0x040028E1 RID: 10465
		public string crosspromo_session_id;

		// Token: 0x040028E2 RID: 10466
		public int screen_count;

		// Token: 0x040028E3 RID: 10467
		public int screen_previous_time_sec;

		// Token: 0x040028E4 RID: 10468
		public string game_detail_product_id;

		// Token: 0x040028E5 RID: 10469
		public string game_detail_product_name;

		// Token: 0x040028E6 RID: 10470
		public string clicked_crosspromo_tile_size;

		// Token: 0x040028E7 RID: 10471
		public string clicked_crosspromo_tile_position_xy;

		// Token: 0x020009F4 RID: 2548
		public enum crosspromo_type
		{
			// Token: 0x040033B2 RID: 13234
			more_games,
			// Token: 0x040033B3 RID: 13235
			interstitial,
			// Token: 0x040033B4 RID: 13236
			banner
		}

		// Token: 0x020009F5 RID: 2549
		public enum screen_current
		{
			// Token: 0x040033B6 RID: 13238
			more_games,
			// Token: 0x040033B7 RID: 13239
			game_detail,
			// Token: 0x040033B8 RID: 13240
			zoom_image,
			// Token: 0x040033B9 RID: 13241
			interstitial,
			// Token: 0x040033BA RID: 13242
			banner
		}

		// Token: 0x020009F6 RID: 2550
		public enum screen_previous
		{
			// Token: 0x040033BC RID: 13244
			more_games,
			// Token: 0x040033BD RID: 13245
			interstitial,
			// Token: 0x040033BE RID: 13246
			banner,
			// Token: 0x040033BF RID: 13247
			ingame
		}

		// Token: 0x020009F7 RID: 2551
		public enum screen_previous_nav_action
		{
			// Token: 0x040033C1 RID: 13249
			automatic,
			// Token: 0x040033C2 RID: 13250
			click_image,
			// Token: 0x040033C3 RID: 13251
			click_banner,
			// Token: 0x040033C4 RID: 13252
			click_tile,
			// Token: 0x040033C5 RID: 13253
			click_learn_more,
			// Token: 0x040033C6 RID: 13254
			click_close,
			// Token: 0x040033C7 RID: 13255
			click_board_game,
			// Token: 0x040033C8 RID: 13256
			click_back,
			// Token: 0x040033C9 RID: 13257
			click_filter_featured,
			// Token: 0x040033CA RID: 13258
			click_filter_gamer,
			// Token: 0x040033CB RID: 13259
			click_filter_family,
			// Token: 0x040033CC RID: 13260
			click_filter_boardgame
		}

		// Token: 0x020009F8 RID: 2552
		public enum more_game_category
		{
			// Token: 0x040033CE RID: 13262
			featured,
			// Token: 0x040033CF RID: 13263
			family,
			// Token: 0x040033D0 RID: 13264
			advanced,
			// Token: 0x040033D1 RID: 13265
			tabletop
		}

		// Token: 0x020009F9 RID: 2553
		public enum game_detail_product_type
		{
			// Token: 0x040033D3 RID: 13267
			digital,
			// Token: 0x040033D4 RID: 13268
			boardgame
		}
	}
}
