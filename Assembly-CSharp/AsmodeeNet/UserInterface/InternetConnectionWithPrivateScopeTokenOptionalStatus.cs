using System;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000627 RID: 1575
	public class InternetConnectionWithPrivateScopeTokenOptionalStatus : RequiredOnlineStatus
	{
		// Token: 0x06003A13 RID: 14867 RVA: 0x00120EDC File Offset: 0x0011F0DC
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
			oauthGate.GetPrivateAccessToken(true, delegate(OAuthError error)
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
