using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006DE RID: 1758
	public class RecentGamesEndpoint : Endpoint<RecentGame[]>
	{
		// Token: 0x06003E80 RID: 16000 RVA: 0x00131468 File Offset: 0x0012F668
		public RecentGamesEndpoint(OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			base._URL = "/main/v1/user/me/lastgames";
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E81 RID: 16001 RVA: 0x0013149C File Offset: 0x0012F69C
		public RecentGamesEndpoint(string game, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastgames/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E82 RID: 16002 RVA: 0x001314F2 File Offset: 0x0012F6F2
		public RecentGamesEndpoint(GameStatus gameStatus, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			base._URL = string.Format("/main/v1/user/me/lastgames?status={0}", gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E83 RID: 16003 RVA: 0x00131530 File Offset: 0x0012F730
		public RecentGamesEndpoint(string game, GameStatus gameStatus, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentGames";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastgames/{0}?status={1}", game, gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E84 RID: 16004 RVA: 0x0013158C File Offset: 0x0012F78C
		protected override void ProcessResponse(Action<RecentGame[], WebError> onCompletion)
		{
			RecentGame[] arg = (from x in JsonUtility.FromJson<ApiRecentGameResponse>(base._HTTPResponse.DataAsText).data.games
			select new RecentGame(x)).ToArray<RecentGame>();
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
