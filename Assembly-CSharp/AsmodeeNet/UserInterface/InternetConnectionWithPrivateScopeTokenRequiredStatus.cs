using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000628 RID: 1576
	public class InternetConnectionWithPrivateScopeTokenRequiredStatus : RequiredOnlineStatus
	{
		// Token: 0x06003A16 RID: 14870 RVA: 0x00120F4C File Offset: 0x0011F14C
		public override void MeetRequirements(Action onSuccess, Action onFailure)
		{
			this.SetCallbacks(onSuccess, onFailure);
			OAuthGate oauthGate = CoreApplication.Instance.OAuthGate;
			if (!CoreApplication.Instance.CommunityHub.IsNetworkReachable)
			{
				base.CallOnFailure();
				return;
			}
			if (oauthGate.HasPrivateToken)
			{
				base.CallOnSuccess();
				return;
			}
			oauthGate.GetPrivateAccessToken(false, delegate(OAuthError error)
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
