using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000626 RID: 1574
	public class InternetConnectionStatus : RequiredOnlineStatus
	{
		// Token: 0x06003A0F RID: 14863 RVA: 0x00120E54 File Offset: 0x0011F054
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			InternetConnectionStatus.Method method = this.method;
			if (method != InternetConnectionStatus.Method.ApplicationInternetReachability && method == InternetConnectionStatus.Method.HttpRequestToURL)
			{
				CoreApplication.Instance.StartCoroutine(WebChecker.WebRequest(delegate()
				{
					base.CallOnSuccess();
				}, delegate()
				{
					base.CallOnFailure();
				}, this.targetURL));
				return;
			}
			if (CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnSuccess();
				return;
			}
			base.CallOnFailure();
		}

		// Token: 0x0400258A RID: 9610
		public InternetConnectionStatus.Method method;

		// Token: 0x0400258B RID: 9611
		public string targetURL;

		// Token: 0x02000921 RID: 2337
		public enum Method
		{
			// Token: 0x040030A8 RID: 12456
			ApplicationInternetReachability,
			// Token: 0x040030A9 RID: 12457
			HttpRequestToURL
		}
	}
}
