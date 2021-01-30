using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C9 RID: 1737
	public class UpdateNewsletterEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003E34 RID: 15924 RVA: 0x0012FF28 File Offset: 0x0012E128
		public UpdateNewsletterEndpoint(bool receiveNewsletter, OAuthGate oauthGate = null) : base(null, new bool?(receiveNewsletter), oauthGate)
		{
		}
	}
}
