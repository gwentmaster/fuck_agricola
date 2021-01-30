using System;

namespace AsmodeeNet.Network
{
	// Token: 0x0200067F RID: 1663
	[Serializable]
	public class AuthenticationTokens
	{
		// Token: 0x17000816 RID: 2070
		// (get) Token: 0x06003CDF RID: 15583 RVA: 0x0012C510 File Offset: 0x0012A710
		public bool HasExpired
		{
			get
			{
				return (this.expirationDate - DateTime.Now).TotalSeconds < 0.0;
			}
		}

		// Token: 0x17000817 RID: 2071
		// (get) Token: 0x06003CE0 RID: 15584 RVA: 0x0012C540 File Offset: 0x0012A740
		public bool HasPublicToken
		{
			get
			{
				return !this.HasExpired && this.scope.Contains("public");
			}
		}

		// Token: 0x17000818 RID: 2072
		// (get) Token: 0x06003CE1 RID: 15585 RVA: 0x0012C55C File Offset: 0x0012A75C
		public bool HasPrivateToken
		{
			get
			{
				return !this.HasExpired && this.scope.Contains("private");
			}
		}

		// Token: 0x06003CE2 RID: 15586 RVA: 0x0012C578 File Offset: 0x0012A778
		public void InitExpiration()
		{
			this.expirationDate = DateTime.Now.AddSeconds((double)this.expires_in);
		}

		// Token: 0x06003CE3 RID: 15587 RVA: 0x0012C59F File Offset: 0x0012A79F
		public AuthenticationTokens()
		{
			this.InitExpiration();
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0012C5B8 File Offset: 0x0012A7B8
		public AuthenticationTokens(string accessToken, string refreshToken)
		{
			this.InitExpiration();
			this.access_token = accessToken;
			this.refresh_token = refreshToken;
		}

		// Token: 0x06003CE5 RID: 15589 RVA: 0x0012C5DF File Offset: 0x0012A7DF
		public AuthenticationTokens(string accessToken, string refreshToken, int expiresIn, string scope)
		{
			this.access_token = accessToken;
			this.refresh_token = refreshToken;
			this.expires_in = expiresIn;
			this.scope = scope;
			this.InitExpiration();
		}

		// Token: 0x0400270B RID: 9995
		public string access_token;

		// Token: 0x0400270C RID: 9996
		public string refresh_token;

		// Token: 0x0400270D RID: 9997
		public int expires_in;

		// Token: 0x0400270E RID: 9998
		public string scope = "";

		// Token: 0x0400270F RID: 9999
		private DateTime expirationDate;
	}
}
