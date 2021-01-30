using System;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x0200068B RID: 1675
	public class GetAchievementByIdEndpoint : Endpoint<Achievement>
	{
		// Token: 0x06003D38 RID: 15672 RVA: 0x0012D580 File Offset: 0x0012B780
		public GetAchievementByIdEndpoint(string game, int id, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".User.Achievement";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/game/{0}/achievement/{1}", game, id);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x0012D5DC File Offset: 0x0012B7DC
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
