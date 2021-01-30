using System;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x020005D6 RID: 1494
	public interface IAuthenticationProvider
	{
		// Token: 0x17000719 RID: 1817
		// (get) Token: 0x060036B5 RID: 14005
		bool IsPreAuthRequired { get; }

		// Token: 0x1400001A RID: 26
		// (add) Token: 0x060036B6 RID: 14006
		// (remove) Token: 0x060036B7 RID: 14007
		event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x1400001B RID: 27
		// (add) Token: 0x060036B8 RID: 14008
		// (remove) Token: 0x060036B9 RID: 14009
		event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x060036BA RID: 14010
		void StartAuthentication();

		// Token: 0x060036BB RID: 14011
		void PrepareRequest(HTTPRequest request, RequestTypes type);
	}
}
