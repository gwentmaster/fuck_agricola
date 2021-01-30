using System;
using System.Linq;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006DF RID: 1759
	public class RecentOpponentsEndpoint : Endpoint<RecentOpponent[]>
	{
		// Token: 0x06003E85 RID: 16005 RVA: 0x001315E8 File Offset: 0x0012F7E8
		public RecentOpponentsEndpoint(OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			base._URL = "/main/v1/user/me/lastopponents";
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E86 RID: 16006 RVA: 0x0013161C File Offset: 0x0012F81C
		public RecentOpponentsEndpoint(string game, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastopponents/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E87 RID: 16007 RVA: 0x00131672 File Offset: 0x0012F872
		public RecentOpponentsEndpoint(GameStatus gameStatus, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			base._URL = string.Format("/main/v1/user/me/lastopponents?status={0}", gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E88 RID: 16008 RVA: 0x001316B0 File Offset: 0x0012F8B0
		public RecentOpponentsEndpoint(string game, GameStatus gameStatus, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.RecentOpponents";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' parameter cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/lastopponents/{0}?status={1}", game, gameStatus);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E89 RID: 16009 RVA: 0x0013170C File Offset: 0x0012F90C
		protected override void ProcessResponse(Action<RecentOpponent[], WebError> onCompletion)
		{
			RecentOpponent[] arg = (from x in JsonUtility.FromJson<ApiRecentOpponentsResponse>(base._HTTPResponse.DataAsText).data.opponents
			select new RecentOpponent(x)).ToArray<RecentOpponent>();
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
