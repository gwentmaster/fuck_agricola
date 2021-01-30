using System;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x020005D8 RID: 1496
	internal class HeaderAuthenticator : IAuthenticationProvider
	{
		// Token: 0x1700071F RID: 1823
		// (get) Token: 0x060036CE RID: 14030 RVA: 0x0010F87E File Offset: 0x0010DA7E
		// (set) Token: 0x060036CF RID: 14031 RVA: 0x0010F886 File Offset: 0x0010DA86
		public string User { get; private set; }

		// Token: 0x17000720 RID: 1824
		// (get) Token: 0x060036D0 RID: 14032 RVA: 0x0010F88F File Offset: 0x0010DA8F
		// (set) Token: 0x060036D1 RID: 14033 RVA: 0x0010F897 File Offset: 0x0010DA97
		public string Roles { get; private set; }

		// Token: 0x17000721 RID: 1825
		// (get) Token: 0x060036D2 RID: 14034 RVA: 0x0002A062 File Offset: 0x00028262
		public bool IsPreAuthRequired
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1400001E RID: 30
		// (add) Token: 0x060036D3 RID: 14035 RVA: 0x0010F8A0 File Offset: 0x0010DAA0
		// (remove) Token: 0x060036D4 RID: 14036 RVA: 0x0010F8D8 File Offset: 0x0010DAD8
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x1400001F RID: 31
		// (add) Token: 0x060036D5 RID: 14037 RVA: 0x0010F910 File Offset: 0x0010DB10
		// (remove) Token: 0x060036D6 RID: 14038 RVA: 0x0010F948 File Offset: 0x0010DB48
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x060036D7 RID: 14039 RVA: 0x0010F97D File Offset: 0x0010DB7D
		public HeaderAuthenticator(string user, string roles)
		{
			this.User = user;
			this.Roles = roles;
		}

		// Token: 0x060036D8 RID: 14040 RVA: 0x00003022 File Offset: 0x00001222
		public void StartAuthentication()
		{
		}

		// Token: 0x060036D9 RID: 14041 RVA: 0x0010F993 File Offset: 0x0010DB93
		public void PrepareRequest(HTTPRequest request, RequestTypes type)
		{
			request.SetHeader("username", this.User);
			request.SetHeader("roles", this.Roles);
		}
	}
}
