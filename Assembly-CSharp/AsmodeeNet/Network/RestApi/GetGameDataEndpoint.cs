using System;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D2 RID: 1746
	public class GetGameDataEndpoint : Endpoint<Dictionary<string, string>>
	{
		// Token: 0x06003E67 RID: 15975 RVA: 0x001307D8 File Offset: 0x0012E9D8
		public GetGameDataEndpoint(string game, int userId, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/{0}/data/{1}", userId, game);
			base._HttpMethod = HTTPMethods.Get;
			this._game = game;
		}

		// Token: 0x06003E68 RID: 15976 RVA: 0x0013083C File Offset: 0x0012EA3C
		public GetGameDataEndpoint(string game, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Get;
			this._game = game;
		}

		// Token: 0x06003E69 RID: 15977 RVA: 0x0013089C File Offset: 0x0012EA9C
		protected override void ProcessResponse(Action<Dictionary<string, string>, WebError> onCompletion)
		{
			Dictionary<string, string> arg = (((Json.Deserialize(base._HTTPResponse.DataAsText) as Dictionary<string, object>)["data"] as Dictionary<string, object>)[this._game] as Dictionary<string, object>).ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => x.Value as string);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x04002840 RID: 10304
		private string _game;
	}
}
