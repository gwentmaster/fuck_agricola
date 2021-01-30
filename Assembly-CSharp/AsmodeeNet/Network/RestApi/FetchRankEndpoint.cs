using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D1 RID: 1745
	public class FetchRankEndpoint : Endpoint<FetchRank>
	{
		// Token: 0x170008AE RID: 2222
		// (get) Token: 0x06003E63 RID: 15971 RVA: 0x00130733 File Offset: 0x0012E933
		// (set) Token: 0x06003E64 RID: 15972 RVA: 0x0013073B File Offset: 0x0012E93B
		public FetchRank FetchRankResult { get; private set; }

		// Token: 0x06003E65 RID: 15973 RVA: 0x00130744 File Offset: 0x0012E944
		public FetchRankEndpoint(int user, string gameNameOrVariant, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.FetchRank";
			if (string.IsNullOrEmpty(gameNameOrVariant))
			{
				throw new ArgumentException("'gameNameOrVariant' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/{0}/rank/{1}", user, gameNameOrVariant);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E66 RID: 15974 RVA: 0x001307A0 File Offset: 0x0012E9A0
		protected override void ProcessResponse(Action<FetchRank, WebError> onCompletion)
		{
			FetchRank arg = new FetchRank(JsonUtility.FromJson<ApiFetchRankResponse>(base._HTTPResponse.DataAsText).data.user);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
