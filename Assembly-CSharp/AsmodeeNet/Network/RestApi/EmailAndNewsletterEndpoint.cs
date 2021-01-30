using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C7 RID: 1735
	public abstract class EmailAndNewsletterEndpoint : Endpoint
	{
		// Token: 0x06003E32 RID: 15922 RVA: 0x0012FE80 File Offset: 0x0012E080
		public EmailAndNewsletterEndpoint(string email, bool? receiveNewsletter, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.Mail";
			base._URL = "/main/v1/user/me/email";
			base._HttpMethod = HTTPMethods.Put;
			base._Parameters = new Hashtable();
			if (email != null)
			{
				base._Parameters.Add("email", email);
			}
			if (receiveNewsletter != null)
			{
				base._Parameters.Add("newsletter", receiveNewsletter);
			}
		}
	}
}
