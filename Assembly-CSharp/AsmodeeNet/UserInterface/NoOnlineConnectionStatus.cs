using System;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200062C RID: 1580
	public class NoOnlineConnectionStatus : RequiredOnlineStatus
	{
		// Token: 0x06003A3D RID: 14909 RVA: 0x001217DF File Offset: 0x0011F9DF
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			base.CallOnSuccess();
		}
	}
}
