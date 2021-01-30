using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000690 RID: 1680
	public class AddBuddyEndpoint : Endpoint
	{
		// Token: 0x06003D47 RID: 15687 RVA: 0x0012DD10 File Offset: 0x0012BF10
		public AddBuddyEndpoint(int buddyId, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = string.Format("/main/v1/user/me/buddies/{0}", buddyId);
			base._HttpMethod = HTTPMethods.Post;
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
