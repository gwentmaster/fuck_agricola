using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006CA RID: 1738
	public class UpdateEmailAndNewsletterEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003E35 RID: 15925 RVA: 0x0012FF38 File Offset: 0x0012E138
		public UpdateEmailAndNewsletterEndpoint(string email, bool receiveNewsletter, OAuthGate oauthGate = null) : base(email, new bool?(receiveNewsletter), oauthGate)
		{
			if (email == null)
			{
				throw new ArgumentException("\"email\" parameter cannot be null");
			}
		}
	}
}
