using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D7 RID: 1751
	public class RemoveIgnoredEndpoint : Endpoint
	{
		// Token: 0x06003E71 RID: 15985 RVA: 0x00130CF4 File Offset: 0x0012EEF4
		public RemoveIgnoredEndpoint(int ignoredId, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = string.Format("/main/v1/user/me/ignore/{0}", ignoredId);
			base._HttpMethod = HTTPMethods.Delete;
			base._Parameters = new Hashtable
			{
				{
					"ignoredId",
					ignoredId
				}
			};
		}
	}
}
