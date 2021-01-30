using System;
using System.Collections;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D8 RID: 1752
	public class LeaderboardGetRankAndScoreEndpoint : Endpoint<LeaderboardScoringInfo>
	{
		// Token: 0x06003E72 RID: 15986 RVA: 0x00130D58 File Offset: 0x0012EF58
		public LeaderboardGetRankAndScoreEndpoint(int userId, string game, string leaderboard, Period period, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Scores";
			base._URL = string.Format("/main/v1/user/{0}/scores/{1}/{2}/{3}", new object[]
			{
				userId,
				game,
				leaderboard,
				period.ToString().ToLower()
			});
			base._Parameters = new Hashtable
			{
				{
					"userId",
					userId
				},
				{
					"game",
					game
				},
				{
					"leaderboard",
					leaderboard
				},
				{
					"period",
					period
				}
			};
		}

		// Token: 0x06003E73 RID: 15987 RVA: 0x00130E08 File Offset: 0x0012F008
		protected override void ProcessResponse(Action<LeaderboardScoringInfo, WebError> onCompletion)
		{
			Either<LeaderboardScoringInfo, Builder<LeaderboardScoringInfo>.BuilderErrors[]> either = new LeaderboardScoringInfo.Builder(JsonUtility.FromJson<ApiLeaderboardGetRankAndScoreResponse>(base._HTTPResponse.DataAsText)).Build(false);
			if (onCompletion != null)
			{
				if (either.Error == null)
				{
					onCompletion(either.Value, null);
					return;
				}
				onCompletion(null, new LeaderboardScoringInfoError(either.Error));
			}
		}
	}
}
