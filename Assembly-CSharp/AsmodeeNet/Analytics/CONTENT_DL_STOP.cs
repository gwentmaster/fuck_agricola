using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x0200070F RID: 1807
	public struct CONTENT_DL_STOP
	{
		// Token: 0x040028EF RID: 10479
		public string dl_session_id;

		// Token: 0x040028F0 RID: 10480
		public string dl_content_id;

		// Token: 0x040028F1 RID: 10481
		public bool dl_is_complete;

		// Token: 0x040028F2 RID: 10482
		public int dl_time;

		// Token: 0x020009FE RID: 2558
		public enum dl_end_reason
		{
			// Token: 0x040033E6 RID: 13286
			completed,
			// Token: 0x040033E7 RID: 13287
			aborted_by_user,
			// Token: 0x040033E8 RID: 13288
			network_issue
		}
	}
}
