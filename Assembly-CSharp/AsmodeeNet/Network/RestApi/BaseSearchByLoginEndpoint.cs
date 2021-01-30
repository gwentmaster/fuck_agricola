using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E3 RID: 1763
	public abstract class BaseSearchByLoginEndpoint : EndpointWithPaginatedResponse<UserSearchResult>
	{
		// Token: 0x06003E90 RID: 16016 RVA: 0x00131B7E File Offset: 0x0012FD7E
		public BaseSearchByLoginEndpoint(OAuthGate oauthGate = null) : base(oauthGate, false)
		{
		}

		// Token: 0x06003E91 RID: 16017 RVA: 0x00131B88 File Offset: 0x0012FD88
		protected void CtorCore(Extras extras, int offset, int limit)
		{
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
			if (offset > 0)
			{
				base._URL = base._URL + "&offset=" + offset.ToString();
			}
			if (limit > 0)
			{
				base._URL = base._URL + "&limit=" + limit.ToString();
			}
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E92 RID: 16018 RVA: 0x00131C40 File Offset: 0x0012FE40
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

		// Token: 0x04002847 RID: 10311
		public const string kForbiddenChars = "()#|@^*%§!?:;.,$~";

		// Token: 0x04002848 RID: 10312
		public const string kNoTokenException = "A public token is needed to call this endpoint.";

		// Token: 0x04002849 RID: 10313
		public const string kEmptyLoginArrayException = "Logins array cannot be null and must at least contain one item.";

		// Token: 0x0400284A RID: 10314
		public const string kNullOrEmptyLoginException = "A login cannot be null or empty.";

		// Token: 0x0400284B RID: 10315
		public const string kLoginToShortException = "A login must be {0} characters long minimum.";

		// Token: 0x0400284C RID: 10316
		public const string kForbiddenCharacterException = "A login cannot contain item from the following set : ";
	}
}
