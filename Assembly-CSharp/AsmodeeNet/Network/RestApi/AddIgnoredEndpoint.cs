using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D5 RID: 1749
	public class AddIgnoredEndpoint : Endpoint
	{
		// Token: 0x06003E6E RID: 15982 RVA: 0x00130B10 File Offset: 0x0012ED10
		public AddIgnoredEndpoint(int ignoredId, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = string.Format("/main/v1/user/me/ignore/{0}", ignoredId);
			base._HttpMethod = HTTPMethods.Post;
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
