using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000629 RID: 1577
	public class InternetConnectionWithPublicScopeTokenStatus : RequiredOnlineStatus
	{
		// Token: 0x06003A19 RID: 14873 RVA: 0x00120FA8 File Offset: 0x0011F1A8
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnFailure();
				return;
			}
			if (oauthGate.HasPublicToken)
			{
				base.CallOnSuccess();
				return;
			}
			oauthGate.GetPublicAccessToken(delegate(OAuthError error)
			{
				if (error == null)
				{
					base.CallOnSuccess();
					return;
				}
				base.CallOnFailure();
			});
		}
	}
}
