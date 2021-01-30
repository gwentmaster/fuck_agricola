using System;

namespace AsmodeeNet.Network
{
	// Token: 0x02000682 RID: 1666
	[Serializable]
	public class OAuthError : WebError
	{
		// Token: 0x06003CEC RID: 15596 RVA: 0x0012C753 File Offset: 0x0012A953
		public OAuthError()
		{
		}

		// Token: 0x06003CED RID: 15597 RVA: 0x0012C75B File Offset: 0x0012A95B
		public OAuthError(string errorName, string errorDescription, int status = -1) : base(errorName, status)
		{
			this.error_description = errorDescription;
		}

		// Token: 0x06003CEE RID: 15598 RVA: 0x0012C76C File Offset: 0x0012A96C
		public new static OAuthError MakeNoResponseError()
		{
			return new OAuthError("no_response", "There was no response", -1);
		}

		// Token: 0x06003CEF RID: 15599 RVA: 0x0012C77E File Offset: 0x0012A97E
		public new static OAuthError MakeTimeoutError()
		{
			return new OAuthError("timeout", "The request timed out", -1);
		}

		// Token: 0x06003CF0 RID: 15600 RVA: 0x0012C790 File Offset: 0x0012A990
		public new static OAuthError MakePublicKeyPinningError()
		{
			return new OAuthError("public_key_pinning_error", "The signature of the server's SSL certificate doesn't match expectations", -1);
		}

		// Token: 0x06003CF1 RID: 15601 RVA: 0x0012C7A2 File Offset: 0x0012A9A2
		public static OAuthError MakeSilentAuthError()
		{
			return new OAuthError("no_private_token", "The OAuthGate required a private token but silently failed.", -1);
		}

		// Token: 0x0400271D RID: 10013
		public string error_description;
	}
}
