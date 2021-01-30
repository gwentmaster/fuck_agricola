using System;

namespace AsmodeeNet.Analytics
{
	// Token: 0x02000722 RID: 1826
	public struct TUTORIAL_STEP
	{
		// Token: 0x04002970 RID: 10608
		public string step_id;

		// Token: 0x04002971 RID: 10609
		public float step_sequence_number;

		// Token: 0x04002972 RID: 10610
		public int time_on_step;

		// Token: 0x04002973 RID: 10611
		public bool is_tuto_complete;

		// Token: 0x02000A05 RID: 2565
		public enum step_status
		{
			// Token: 0x040033FF RID: 13311
			completed,
			// Token: 0x04003400 RID: 13312
			failed,
			// Token: 0x04003401 RID: 13313
			quit
		}
	}
}
