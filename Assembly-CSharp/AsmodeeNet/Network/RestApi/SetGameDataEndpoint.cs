using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D4 RID: 1748
	public class SetGameDataEndpoint : Endpoint<Dictionary<string, string>>
	{
		// Token: 0x06003E6C RID: 15980 RVA: 0x001309CC File Offset: 0x0012EBCC
		public SetGameDataEndpoint(string game, Dictionary<string, string> keyAndValue, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (keyAndValue == null || keyAndValue.Count == 0)
			{
				throw new ArgumentException("'keyAndValue' argument cannot be null or empty");
			}
			if (keyAndValue.Any((KeyValuePair<string, string> x) => x.Key.Length > 32 || x.Value.Length > 2048))
			{
				throw new ArgumentException("'keyAndValue' argument cannot contain a key whose length is longer than 32 bytes or a value whose length is longer than 2048 bytes");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Post;
			base._Parameters = new Hashtable(keyAndValue);
			this._game = game;
		}

		// Token: 0x06003E6D RID: 15981 RVA: 0x00130A80 File Offset: 0x0012EC80
		protected override void ProcessResponse(Action<Dictionary<string, string>, WebError> onCompletion)
		{
			Dictionary<string, string> arg = (((Json.Deserialize(base._HTTPResponse.DataAsText) as Dictionary<string, object>)["data"] as Dictionary<string, object>)[this._game] as Dictionary<string, object>).ToDictionary((KeyValuePair<string, object> x) => x.Key, (KeyValuePair<string, object> x) => x.Value as string);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x04002844 RID: 10308
		public const int kMaxKeyLength = 32;

		// Token: 0x04002845 RID: 10309
		public const int kMaxValueLength = 2048;

		// Token: 0x04002846 RID: 10310
		private string _game;
	}
}
