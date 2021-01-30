using System;
using BestHTTP.Cookies;

namespace BestHTTP.SignalR.Authentication
{
	// Token: 0x020005D7 RID: 1495
	public sealed class SampleCookieAuthentication : IAuthenticationProvider
	{
		// Token: 0x1700071A RID: 1818
		// (get) Token: 0x060036BC RID: 14012 RVA: 0x0010F4DC File Offset: 0x0010D6DC
		// (set) Token: 0x060036BD RID: 14013 RVA: 0x0010F4E4 File Offset: 0x0010D6E4
		public Uri AuthUri { get; private set; }

		// Token: 0x1700071B RID: 1819
		// (get) Token: 0x060036BE RID: 14014 RVA: 0x0010F4ED File Offset: 0x0010D6ED
		// (set) Token: 0x060036BF RID: 14015 RVA: 0x0010F4F5 File Offset: 0x0010D6F5
		public string UserName { get; private set; }

		// Token: 0x1700071C RID: 1820
		// (get) Token: 0x060036C0 RID: 14016 RVA: 0x0010F4FE File Offset: 0x0010D6FE
		// (set) Token: 0x060036C1 RID: 14017 RVA: 0x0010F506 File Offset: 0x0010D706
		public string Password { get; private set; }

		// Token: 0x1700071D RID: 1821
		// (get) Token: 0x060036C2 RID: 14018 RVA: 0x0010F50F File Offset: 0x0010D70F
		// (set) Token: 0x060036C3 RID: 14019 RVA: 0x0010F517 File Offset: 0x0010D717
		public string UserRoles { get; private set; }

		// Token: 0x1700071E RID: 1822
		// (get) Token: 0x060036C4 RID: 14020 RVA: 0x0010F520 File Offset: 0x0010D720
		// (set) Token: 0x060036C5 RID: 14021 RVA: 0x0010F528 File Offset: 0x0010D728
		public bool IsPreAuthRequired { get; private set; }

		// Token: 0x1400001C RID: 28
		// (add) Token: 0x060036C6 RID: 14022 RVA: 0x0010F534 File Offset: 0x0010D734
		// (remove) Token: 0x060036C7 RID: 14023 RVA: 0x0010F56C File Offset: 0x0010D76C
		public event OnAuthenticationSuccededDelegate OnAuthenticationSucceded;

		// Token: 0x1400001D RID: 29
		// (add) Token: 0x060036C8 RID: 14024 RVA: 0x0010F5A4 File Offset: 0x0010D7A4
		// (remove) Token: 0x060036C9 RID: 14025 RVA: 0x0010F5DC File Offset: 0x0010D7DC
		public event OnAuthenticationFailedDelegate OnAuthenticationFailed;

		// Token: 0x060036CA RID: 14026 RVA: 0x0010F611 File Offset: 0x0010D811
		public SampleCookieAuthentication(Uri authUri, string user, string passwd, string roles)
		{
			this.AuthUri = authUri;
			this.UserName = user;
			this.Password = passwd;
			this.UserRoles = roles;
			this.IsPreAuthRequired = true;
		}

		// Token: 0x060036CB RID: 14027 RVA: 0x0010F640 File Offset: 0x0010D840
		public void StartAuthentication()
		{
			this.AuthRequest = new HTTPRequest(this.AuthUri, HTTPMethods.Post, new OnRequestFinishedDelegate(this.OnAuthRequestFinished));
			this.AuthRequest.AddField("userName", this.UserName);
			this.AuthRequest.AddField("Password", this.Password);
			this.AuthRequest.AddField("roles", this.UserRoles);
			this.AuthRequest.Send();
		}

		// Token: 0x060036CC RID: 14028 RVA: 0x0010F6B9 File Offset: 0x0010D8B9
		public void PrepareRequest(HTTPRequest request, RequestTypes type)
		{
			request.Cookies.Add(this.Cookie);
		}

		// Token: 0x060036CD RID: 14029 RVA: 0x0010F6CC File Offset: 0x0010D8CC
		private void OnAuthRequestFinished(HTTPRequest req, HTTPResponse resp)
		{
			this.AuthRequest = null;
			string reason = string.Empty;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					Cookie cookie;
					if (resp.Cookies == null)
					{
						cookie = null;
					}
					else
					{
						cookie = resp.Cookies.Find((Cookie c) => c.Name.Equals(".ASPXAUTH"));
					}
					this.Cookie = cookie;
					if (this.Cookie != null)
					{
						HTTPManager.Logger.Information("CookieAuthentication", "Auth. Cookie found!");
						if (this.OnAuthenticationSucceded != null)
						{
							this.OnAuthenticationSucceded(this);
						}
						return;
					}
					HTTPManager.Logger.Warning("CookieAuthentication", reason = "Auth. Cookie NOT found!");
				}
				else
				{
					HTTPManager.Logger.Warning("CookieAuthentication", reason = string.Format("Request Finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				}
				break;
			case HTTPRequestStates.Error:
				HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
				break;
			case HTTPRequestStates.Aborted:
				HTTPManager.Logger.Warning("CookieAuthentication", reason = "Request Aborted!");
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				HTTPManager.Logger.Error("CookieAuthentication", reason = "Connection Timed Out!");
				break;
			case HTTPRequestStates.TimedOut:
				HTTPManager.Logger.Error("CookieAuthentication", reason = "Processing the request Timed Out!");
				break;
			}
			if (this.OnAuthenticationFailed != null)
			{
				this.OnAuthenticationFailed(this, reason);
			}
		}

		// Token: 0x0400235D RID: 9053
		private HTTPRequest AuthRequest;

		// Token: 0x0400235E RID: 9054
		private Cookie Cookie;
	}
}
