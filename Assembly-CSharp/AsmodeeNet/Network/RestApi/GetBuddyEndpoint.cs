using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x02000691 RID: 1681
	public class GetBuddyEndpoint : EndpointWithPaginatedResponse<BuddyOrIgnored>
	{
		// Token: 0x06003D48 RID: 15688 RVA: 0x0012DD74 File Offset: 0x0012BF74
		public GetBuddyEndpoint(int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate, true)
		{
			base.DebugModuleName += ".User.Buddies";
			base._URL = "/main/v1/user/me/buddies";
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

		// Token: 0x06003D49 RID: 15689 RVA: 0x0012DE2C File Offset: 0x0012C02C
		protected override void ProcessResponse(Action<PaginatedResult<BuddyOrIgnored>, WebError> onCompletion)
		{
			ApiGetBuddiesOrIgnoredResponse apiGetBuddiesOrIgnoredResponse = JsonUtility.FromJson<ApiGetBuddiesOrIgnoredResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<BuddyOrIgnored> arg = new PaginatedResult<BuddyOrIgnored>(apiGetBuddiesOrIgnoredResponse.data.total, (from x in apiGetBuddiesOrIgnoredResponse.data.buddies
			select new BuddyOrIgnored(x.id, x.login_name)).ToArray<BuddyOrIgnored>(), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.next), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.prev), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.first), base._LinkSetter(apiGetBuddiesOrIgnoredResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
