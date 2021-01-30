using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000692 RID: 1682
	public class RemoveBuddyEndpoint : Endpoint
	{
		// Token: 0x06003D4A RID: 15690 RVA: 0x0012DEF4 File Offset: 0x0012C0F4
		public RemoveBuddyEndpoint(int buddyId, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = string.Format("/main/v1/user/me/buddies/{0}", buddyId);
			base._HttpMethod = HTTPMethods.Delete;
			base._Parameters = new Hashtable
			{
				{
					"buddyId",
					buddyId
				}
			};
		}
	}
}
