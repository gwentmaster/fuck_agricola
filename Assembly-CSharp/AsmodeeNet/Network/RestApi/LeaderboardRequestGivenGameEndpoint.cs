using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006DB RID: 1755
	public class LeaderboardRequestGivenGameEndpoint : EndpointWithPaginatedResponse<GameLeaderboard.Player>
	{
		// Token: 0x06003E79 RID: 15993 RVA: 0x00131134 File Offset: 0x0012F334
		public LeaderboardRequestGivenGameEndpoint(string game, string leaderboard, Period period, int offset = 0, int limit = 20, OAuthGate oauthGate = null) : base(oauthGate, false)
		{
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (string.IsNullOrEmpty(leaderboard))
			{
				throw new ArgumentException("'leaderboard' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/leaderboards/{1}/{2}?offset={3}&limit={4}", new object[]
			{
				game,
				leaderboard,
				period,
				offset,
				limit
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E7A RID: 15994 RVA: 0x001311B4 File Offset: 0x0012F3B4
		protected override void ProcessResponse(Action<PaginatedResult<GameLeaderboard.Player>, WebError> onCompletion)
		{
			ApiLeaderboardRequestGivenGameResponse apiLeaderboardRequestGivenGameResponse = JsonUtility.FromJson<ApiLeaderboardRequestGivenGameResponse>(base._HTTPResponse.DataAsText);
			GameLeaderboard gameLeaderboard = new GameLeaderboard(apiLeaderboardRequestGivenGameResponse.data.leaderboard);
			LeaderboardRequestGivenGameEndpoint.GameLeaderboardPaginatedResult<GameLeaderboard.Player> arg = new LeaderboardRequestGivenGameEndpoint.GameLeaderboardPaginatedResult<GameLeaderboard.Player>(apiLeaderboardRequestGivenGameResponse.data.leaderboard.total, gameLeaderboard.Players.Clone() as GameLeaderboard.Player[], base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.next), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.prev), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.first), base._LinkSetter(apiLeaderboardRequestGivenGameResponse.data.leaderboard._links.last), gameLeaderboard);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x020009D4 RID: 2516
		public class GameLeaderboardPaginatedResult<T> : PaginatedResult<!0> where T : class
		{
			// Token: 0x17000A92 RID: 2706
			// (get) Token: 0x06004934 RID: 18740 RVA: 0x0014D9BC File Offset: 0x0014BBBC
			// (set) Token: 0x06004935 RID: 18741 RVA: 0x0014D9C4 File Offset: 0x0014BBC4
			public GameLeaderboard GameLeaderboard { get; private set; }

			// Token: 0x06004936 RID: 18742 RVA: 0x0014D9CD File Offset: 0x0014BBCD
			public GameLeaderboardPaginatedResult(int totalElement, T[] elements, Action<Action<PaginatedResult<T>, WebError>> next, Action<Action<PaginatedResult<T>, WebError>> prev, Action<Action<PaginatedResult<T>, WebError>> first, Action<Action<PaginatedResult<T>, WebError>> last, GameLeaderboard leaderboard) : base(totalElement, elements, next, prev, first, last)
			{
				this.GameLeaderboard = leaderboard;
			}
		}
	}
}
