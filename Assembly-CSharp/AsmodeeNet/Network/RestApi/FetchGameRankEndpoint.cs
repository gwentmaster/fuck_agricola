using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D0 RID: 1744
	public class FetchGameRankEndpoint : EndpointWithPaginatedResponse<FetchGameRank>
	{
		// Token: 0x06003E61 RID: 15969 RVA: 0x001305F4 File Offset: 0x0012E7F4
		public FetchGameRankEndpoint(string gameVariant, RankingType rankingType, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			base.DebugModuleName += ".Game.FetchRank";
			if (string.IsNullOrEmpty(gameVariant))
			{
				throw new ArgumentException("'gameVariant' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/rank/{1}?offset={2}&limit={3}", new object[]
			{
				gameVariant,
				rankingType,
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E62 RID: 15970 RVA: 0x00130670 File Offset: 0x0012E870
		protected override void ProcessResponse(Action<PaginatedResult<FetchGameRank>, WebError> onCompletion)
		{
			ApiFetchGameRankResponse apiFetchGameRankResponse = JsonUtility.FromJson<ApiFetchGameRankResponse>(base._HTTPResponse.DataAsText);
			PaginatedResult<FetchGameRank> arg = new PaginatedResult<FetchGameRank>(apiFetchGameRankResponse.data.total, (from x in apiFetchGameRankResponse.data.ranks
			select new FetchGameRank(x)).ToArray<FetchGameRank>(), base._LinkSetter(apiFetchGameRankResponse.data._links.next), base._LinkSetter(apiFetchGameRankResponse.data._links.prev), base._LinkSetter(apiFetchGameRankResponse.data._links.first), base._LinkSetter(apiFetchGameRankResponse.data._links.last));
			onCompletion(arg, null);
		}
	}
}
