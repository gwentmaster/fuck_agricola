using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200068C RID: 1676
	public class GetAchievementByTagEndpoint : Endpoint<Achievement>
	{
		// Token: 0x06003D3A RID: 15674 RVA: 0x0012D614 File Offset: 0x0012B814
		public GetAchievementByTagEndpoint(string game, string tag, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (string.IsNullOrEmpty(tag))
			{
				throw new ArgumentException("'tag' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievement/{1}", game, tag);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x0012D680 File Offset: 0x0012B880
		protected override void ProcessResponse(Action<Achievement, WebError> onCompletion)
		{
			Achievement arg = new Achievement(JsonUtility.FromJson<ApiGetAchievementResponse>(base._HTTPResponse.DataAsText).data.achievement);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}
	}
}
