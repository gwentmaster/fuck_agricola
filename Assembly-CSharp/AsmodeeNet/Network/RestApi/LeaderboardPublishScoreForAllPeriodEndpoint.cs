using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D9 RID: 1753
	public class LeaderboardPublishScoreForAllPeriodEndpoint : Endpoint<LeaderboardPublishScoreForAllPeriodEndpoint.Result>
	{
		// Token: 0x06003E74 RID: 15988 RVA: 0x00130E5C File Offset: 0x0012F05C
		public LeaderboardPublishScoreForAllPeriodEndpoint(string game, string leaderboard, LeaderboardScoringInfo userRankAndScore, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			if (userRankAndScore == null || userRankAndScore.Score == -1)
			{
				throw new ArgumentException("A score must be specified to call this endpoint");
			}
			base.DebugModuleName += ".User.Scores";
			base._URL = string.Format("/main/v1/user/me/scores/{0}/{1}", game, leaderboard);
			base._Parameters = new Hashtable();
			base._Parameters.Add("score", userRankAndScore.Score);
			if (!string.IsNullOrEmpty(userRankAndScore.Context))
			{
				base._Parameters.Add("context", userRankAndScore.Context);
			}
			base._HttpMethod = HTTPMethods.Put;
		}

		// Token: 0x06003E75 RID: 15989 RVA: 0x00130F04 File Offset: 0x0012F104
		private LeaderboardScoringInfo _BuildLeaderboard(ApiLeaderboardGetRankAndScoreAllPeriodResponse.Data.User.Period raw, ref List<Builder<LeaderboardScoringInfo>.BuilderErrors> errors)
		{
			Either<LeaderboardScoringInfo, Builder<LeaderboardScoringInfo>.BuilderErrors[]> either = new LeaderboardScoringInfo.Builder().Score(raw.score).Context(raw.context).Rank(raw.rank).When((raw.when == null) ? null : new DateTime?(DateTime.Parse(raw.when))).IsNew(raw.isNew).Build(false);
			if (either.Error != null)
			{
				errors.AddRange(either.Error);
			}
			return either.Value;
		}

		// Token: 0x06003E76 RID: 15990 RVA: 0x00130F8C File Offset: 0x0012F18C
		protected override void ProcessResponse(Action<LeaderboardPublishScoreForAllPeriodEndpoint.Result, WebError> onCompletion)
		{
			ApiLeaderboardGetRankAndScoreAllPeriodResponse apiLeaderboardGetRankAndScoreAllPeriodResponse = JsonUtility.FromJson<ApiLeaderboardGetRankAndScoreAllPeriodResponse>(base._HTTPResponse.DataAsText);
			List<Builder<LeaderboardScoringInfo>.BuilderErrors> list = new List<Builder<LeaderboardScoringInfo>.BuilderErrors>();
			LeaderboardPublishScoreForAllPeriodEndpoint.Result arg = new LeaderboardPublishScoreForAllPeriodEndpoint.Result(this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.day, ref list), this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.week, ref list), this._BuildLeaderboard(apiLeaderboardGetRankAndScoreAllPeriodResponse.data.user.ever, ref list));
			if (onCompletion != null)
			{
				if (list.Count == 0)
				{
					onCompletion(arg, null);
					return;
				}
				onCompletion(null, new LeaderboardScoringInfoError(list.ToArray()));
			}
		}

		// Token: 0x020009D3 RID: 2515
		public class Result
		{
			// Token: 0x17000A8F RID: 2703
			// (get) Token: 0x0600492D RID: 18733 RVA: 0x0014D96C File Offset: 0x0014BB6C
			// (set) Token: 0x0600492E RID: 18734 RVA: 0x0014D974 File Offset: 0x0014BB74
			public LeaderboardScoringInfo DailyLeaderboard { get; private set; }

			// Token: 0x17000A90 RID: 2704
			// (get) Token: 0x0600492F RID: 18735 RVA: 0x0014D97D File Offset: 0x0014BB7D
			// (set) Token: 0x06004930 RID: 18736 RVA: 0x0014D985 File Offset: 0x0014BB85
			public LeaderboardScoringInfo WeeklyLeaderboard { get; private set; }

			// Token: 0x17000A91 RID: 2705
			// (get) Token: 0x06004931 RID: 18737 RVA: 0x0014D98E File Offset: 0x0014BB8E
			// (set) Token: 0x06004932 RID: 18738 RVA: 0x0014D996 File Offset: 0x0014BB96
			public LeaderboardScoringInfo ForeverLeaderboard { get; private set; }

			// Token: 0x06004933 RID: 18739 RVA: 0x0014D99F File Offset: 0x0014BB9F
			public Result(LeaderboardScoringInfo day, LeaderboardScoringInfo week, LeaderboardScoringInfo ever)
			{
				this.DailyLeaderboard = day;
				this.WeeklyLeaderboard = week;
				this.ForeverLeaderboard = ever;
			}
		}
	}
}
