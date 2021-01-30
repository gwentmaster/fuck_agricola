using System;
using System.Collections;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006EE RID: 1774
	public class UserSignUpEndpoint : Endpoint<ApiSignUpResponse>
	{
		// Token: 0x06003EAD RID: 16045 RVA: 0x00132848 File Offset: 0x00130A48
		public UserSignUpEndpoint(string loginName, string password, string email, bool subscribeNewsletter, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			if (string.IsNullOrEmpty(email))
			{
				throw new ArgumentException("'email' must not be null or empty");
			}
			if (string.IsNullOrEmpty(password))
			{
				throw new ArgumentException("'password' must not be null or empty");
			}
			base.DebugModuleName += ".User.SignUp";
			base._HttpMethod = HTTPMethods.Post;
			base._URL = "/main/v2/user";
			base._Parameters = new Hashtable
			{
				{
					"password",
					password
				},
				{
					"email",
					email
				},
				{
					"newsletter",
					subscribeNewsletter
				}
			};
			if (!string.IsNullOrEmpty(loginName))
			{
				base._Parameters.Add("login_name", loginName);
			}
			if (base._OAuthGate.SteamManager != null && base._OAuthGate.SteamManager.HasClient)
			{
				PartnerAccount me = base._OAuthGate.SteamManager.Me;
				if (me != null)
				{
					base._Parameters.Add("partner", me.PartnerId);
					base._Parameters.Add("partner_user", me.PartnerUser);
				}
			}
		}

		// Token: 0x06003EAE RID: 16046 RVA: 0x00132960 File Offset: 0x00130B60
		protected override void ProcessResponse(Action<ApiSignUpResponse, WebError> onCompletion)
		{
			ApiSignUpResponse arg = JsonUtility.FromJson<ApiSignUpResponse>(base._HTTPResponse.DataAsText);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x04002858 RID: 10328
		public const int kLoginMinimalLength = 4;

		// Token: 0x04002859 RID: 10329
		public const int kPasswordMinimalLength = 1;
	}
}
