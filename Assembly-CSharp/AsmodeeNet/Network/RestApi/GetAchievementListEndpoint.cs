using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200068D RID: 1677
	public class GetAchievementListEndpoint : EndpointWithPaginatedResponse<Achievement>
	{
		// Token: 0x06003D3C RID: 15676 RVA: 0x0012D6B8 File Offset: 0x0012B8B8
		public GetAchievementListEndpoint(string game, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?offset={1}&limit={2}", game, offset, limit);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D3D RID: 15677 RVA: 0x0012D71C File Offset: 0x0012B91C
		public GetAchievementListEndpoint(int category, string game, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?category={1}&offset={2}&limit={3}", new object[]
			{
				game,
				category,
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D3E RID: 15678 RVA: 0x0012D798 File Offset: 0x0012B998
		public GetAchievementListEndpoint(string game, AchievementType type, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			if (type == AchievementType.Null)
			{
				throw new ArgumentException("'type' parameter value cannot be 'Null'");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?type={1}&offset={2}&limit={3}", new object[]
			{
				game,
				type.ToString().ToLower(),
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D3F RID: 15679 RVA: 0x0012D830 File Offset: 0x0012BA30
		public GetAchievementListEndpoint(int category, string game, AchievementType type, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			if (type == AchievementType.Null)
			{
				throw new ArgumentException("'type' parameter value cannot be 'Null'");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievements?type={1}&category={2}&offset={3}&limit={4}", new object[]
			{
				game,
				type.ToString().ToLower(),
				category,
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D40 RID: 15680 RVA: 0x0012D8D4 File Offset: 0x0012BAD4
		protected override void ProcessResponse(Action<PaginatedResult<Achievement>, WebError> onCompletion)
		{
			ApiGetAchievementListResponse apiGetAchievementListResponse = JsonUtility.FromJson<ApiGetAchievementListResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<Achievement> arg = new PaginatedResult<Achievement>(apiGetAchievementListResponse.data.total, (from x in apiGetAchievementListResponse.data.achievements
			select new Achievement(x)).ToArray<Achievement>(), base._LinkSetter(apiGetAchievementListResponse.data._links.next), base._LinkSetter(apiGetAchievementListResponse.data._links.prev), base._LinkSetter(apiGetAchievementListResponse.data._links.first), base._LinkSetter(apiGetAchievementListResponse.data._links.last));
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
