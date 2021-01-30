using System;
using System.Collections;
using BestHTTP;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E0 RID: 1760
	public class ResetPasswordEndpoint : Endpoint
	{
		// Token: 0x06003E8A RID: 16010 RVA: 0x00131768 File Offset: 0x0012F968
		public ResetPasswordEndpoint(int userId, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.ResetPassword";
			base._URL = string.Format("/main/v1/user/{0}/password", userId);
			base._Parameters = new Hashtable();
			base._HttpMethod = HTTPMethods.Delete;
		}

		// Token: 0x06003E8B RID: 16011 RVA: 0x001317BB File Offset: 0x0012F9BB
		public ResetPasswordEndpoint(OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.ResetPassword";
			base._URL = "/main/v1/user/me/password";
			base._Parameters = new Hashtable();
			base._HttpMethod = HTTPMethods.Delete;
		}
	}
}
