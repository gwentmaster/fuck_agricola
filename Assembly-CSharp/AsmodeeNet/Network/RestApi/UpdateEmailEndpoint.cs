using System;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006C8 RID: 1736
	public class UpdateEmailEndpoint : EmailAndNewsletterEndpoint
	{
		// Token: 0x06003E33 RID: 15923 RVA: 0x0012FEFC File Offset: 0x0012E0FC
		public UpdateEmailEndpoint(string email, OAuthGate oauthGate = null) : base(email, null, oauthGate)
		{
			if (email == null)
			{
				throw new ArgumentException("\"email\" parameter cannot be null");
			}
		}
	}
}
