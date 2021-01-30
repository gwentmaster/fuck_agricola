using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000707 RID: 1799
	public struct HEADER
	{
		// Token: 0x040028B2 RID: 10418
		public string api_key;

		// Token: 0x040028B3 RID: 10419
		public string @event;

		// Token: 0x040028B4 RID: 10420
		public string user_id;

		// Token: 0x040028B5 RID: 10421
		public string device_id;

		// Token: 0x040028B6 RID: 10422
		public string event_type;

		// Token: 0x040028B7 RID: 10423
		public string time;

		// Token: 0x040028B8 RID: 10424
		public string event_properties;

		// Token: 0x040028B9 RID: 10425
		public string user_properties;

		// Token: 0x040028BA RID: 10426
		public string app_version;

		// Token: 0x040028BB RID: 10427
		public string platform;

		// Token: 0x040028BC RID: 10428
		public string os_name;

		// Token: 0x040028BD RID: 10429
		public string os_version;

		// Token: 0x040028BE RID: 10430
		public string device_model;

		// Token: 0x040028BF RID: 10431
		public string language;

		// Token: 0x040028C0 RID: 10432
		public string ip;

		// Token: 0x040028C1 RID: 10433
		public string event_id;

		// Token: 0x040028C2 RID: 10434
		public string session_id;

		// Token: 0x040028C3 RID: 10435
		public string version_build_number;

		// Token: 0x040028C4 RID: 10436
		public string app_boot_session_id;

		// Token: 0x040028C5 RID: 10437
		public string client_local_time;

		// Token: 0x040028C6 RID: 10438
		public string first_party;

		// Token: 0x040028C7 RID: 10439
		public long time_session;

		// Token: 0x040028C8 RID: 10440
		public long time_session_gameplay;

		// Token: 0x040028C9 RID: 10441
		public string screen_resolution;

		// Token: 0x040028CA RID: 10442
		public string unity_sdk_version;

		// Token: 0x040028CB RID: 10443
		public string backend_platform;

		// Token: 0x040028CC RID: 10444
		public string backend_user_id;

		// Token: 0x040028CD RID: 10445
		public string ua_platform;

		// Token: 0x040028CE RID: 10446
		public string ua_user_id;

		// Token: 0x040028CF RID: 10447
		public string ua_channel;

		// Token: 0x040028D0 RID: 10448
		public string push_platform;

		// Token: 0x040028D1 RID: 10449
		public string push_user_id;

		// Token: 0x040028D2 RID: 10450
		public string user_id_first_party;

		// Token: 0x040028D3 RID: 10451
		public int timezone_client;

		// Token: 0x040028D4 RID: 10452
		public long time_ltd;

		// Token: 0x040028D5 RID: 10453
		public long time_ltd_gameplay;

		// Token: 0x040028D6 RID: 10454
		public string ab_test_group;

		// Token: 0x040028D7 RID: 10455
		public int karma;

		// Token: 0x040028D8 RID: 10456
		public int elo_rating;

		// Token: 0x040028D9 RID: 10457
		public bool is_payer;

		// Token: 0x020009EF RID: 2543
		public enum environment
		{
			// Token: 0x040033A0 RID: 13216
			dev,
			// Token: 0x040033A1 RID: 13217
			prod
		}

		// Token: 0x020009F0 RID: 2544
		public enum connection_type
		{
			// Token: 0x040033A3 RID: 13219
			carrier_data_network,
			// Token: 0x040033A4 RID: 13220
			local_area_network,
			// Token: 0x040033A5 RID: 13221
			no_connection
		}
	}
}
