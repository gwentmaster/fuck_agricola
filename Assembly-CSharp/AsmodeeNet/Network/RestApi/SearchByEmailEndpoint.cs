using System;
using System.Linq;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E1 RID: 1761
	public class SearchByEmailEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003E8C RID: 16012 RVA: 0x001317F8 File Offset: 0x0012F9F8
		public SearchByEmailEndpoint(string email, Extras extras = Extras.None, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(email) || !EmailFormatValidator.IsValidEmail(email))
			{
				throw new ArgumentException("'email' parameter cannot be null or empty, and it must have a valid email format");
			}
			base._URL = "/main/v1/users?email=" + WWW.EscapeURL(email);
			string text = "&extras=avatar,";
			if (extras != Extras.None && extras != Extras.Partners)
			{
				if ((extras & Extras.Boardgames) != Extras.None)
				{
					text += "boardgames,";
				}
				if ((extras & Extras.Onlinegames) != Extras.None)
				{
					text += "onlinegames,";
				}
				if ((extras & Extras.Features) != Extras.None)
				{
					text += "features,";
				}
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E8D RID: 16013 RVA: 0x001318C0 File Offset: 0x0012FAC0
		protected override void ProcessResponse(Action<PaginatedResult<UserSearchResult>, WebError> onCompletion)
		{
			ApiSearchUserResponse apiSearchUserResponse = JsonUtility.FromJson<ApiSearchUserResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<UserSearchResult> arg = new PaginatedResult<UserSearchResult>(apiSearchUserResponse.data.total, (from x in apiSearchUserResponse.data.users
			select new UserSearchResult(x)).ToArray<UserSearchResult>(), base._LinkSetter(apiSearchUserResponse.data._links.next), base._LinkSetter(apiSearchUserResponse.data._links.prev), base._LinkSetter(apiSearchUserResponse.data._links.first), base._LinkSetter(apiSearchUserResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
