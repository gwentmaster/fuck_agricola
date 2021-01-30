using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D6 RID: 1750
	public class GetIgnoredEndpoint : EndpointWithPaginatedResponse<BuddyOrIgnored>
	{
		// Token: 0x06003E6F RID: 15983 RVA: 0x00130B74 File Offset: 0x0012ED74
		public GetIgnoredEndpoint(int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate, true)
		{
			base.DebugModuleName += ".User.Ignore";
			base._URL = "/main/v1/user/me/ignore";
			string text = "?";
			if (offset > 0)
			{
				text = string.Concat(new object[]
				{
					text,
					"offset=",
					offset,
					"&"
				});
			}
			if (limit > 0)
			{
				text = string.Concat(new object[]
				{
					text,
					"limit=",
					limit,
					"&"
				});
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
		}

		// Token: 0x06003E70 RID: 15984 RVA: 0x00130C2C File Offset: 0x0012EE2C
		protected override void ProcessResponse(Action<PaginatedResult<BuddyOrIgnored>, WebError> onCompletion)
		{
			ApiGetBuddiesOrIgnoredResponse apiGetBuddiesOrIgnoredResponse = JsonUtility.FromJson<ApiGetBuddiesOrIgnoredResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<BuddyOrIgnored> arg = new PaginatedResult<BuddyOrIgnored>(apiGetBuddiesOrIgnoredResponse.data.total, (from x in apiGetBuddiesOrIgnoredResponse.data.ignored
			select new BuddyOrIgnored(x.id, x.login_name)).ToArray<BuddyOrIgnored>(), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.next), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.prev), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.first), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
