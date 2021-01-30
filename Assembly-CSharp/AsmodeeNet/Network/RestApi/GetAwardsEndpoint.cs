using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200068E RID: 1678
	public class GetAwardsEndpoint : EndpointWithPaginatedResponse<Award>
	{
		// Token: 0x06003D41 RID: 15681 RVA: 0x0012D99A File Offset: 0x0012BB9A
		public GetAwardsEndpoint(int userId, string game = null, string type = null, int category = -1, int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			this.Initialize(string.Format("/main/v1/user/{0}/awards", userId), game, type, category, offset, limit, oauthGate);
		}

		// Token: 0x06003D42 RID: 15682 RVA: 0x0012D9C5 File Offset: 0x0012BBC5
		public GetAwardsEndpoint(string game = null, string type = null, int category = -1, int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate, true)
		{
			this.Initialize("/main/v1/user/me/awards", game, type, category, offset, limit, oauthGate);
		}

		// Token: 0x06003D43 RID: 15683 RVA: 0x0012D9E4 File Offset: 0x0012BBE4
		private void Initialize(string baseUrl, string game, string type, int category, int offset, int limit, OAuthGate oauthGate)
		{
			base.DebugModuleName += ".User.Award";
			if (game == string.Empty || type == string.Empty)
			{
				throw new ArgumentException("\"game\" and \"type\" parameters can not be empty if specified");
			}
			base._URL = baseUrl;
			string text = "?";
			if (!string.IsNullOrEmpty(game))
			{
				text = text + "game=" + game + "&";
			}
			if (!string.IsNullOrEmpty(type))
			{
				text = text + "type=" + type + "&";
			}
			if (category >= 0)
			{
				text = text + "category=" + category.ToString() + "&";
			}
			if (offset >= 0)
			{
				text = text + "offset=" + offset.ToString() + "&";
			}
			if (limit >= 0)
			{
				text = text + "limit=" + limit.ToString() + "&";
			}
			text = text.Substring(0, text.Length - 1);
			base._URL += text;
		}

		// Token: 0x06003D44 RID: 15684 RVA: 0x0012DAE8 File Offset: 0x0012BCE8
		protected override void ProcessResponse(Action<PaginatedResult<Award>, WebError> onCompletion)
		{
			ApiGetAwardListResponse apiGetAwardListResponse = JsonUtility.FromJson<ApiGetAwardListResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<Award> arg = new PaginatedResult<Award>(apiGetAwardListResponse.data.total, (from x in apiGetAwardListResponse.data.awards
			select new Award(x.id, x.tag, x.table_id, x.info_id, new DateTime?(DateTime.Parse(x.awarded_utc)))).ToArray<Award>(), base._LinkSetter(apiGetAwardListResponse.data._links.next), base._LinkSetter(apiGetAwardListResponse.data._links.prev), base._LinkSetter(apiGetAwardListResponse.data._links.first), base._LinkSetter(apiGetAwardListResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
