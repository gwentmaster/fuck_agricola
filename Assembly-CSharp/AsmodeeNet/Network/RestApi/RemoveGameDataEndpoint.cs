using System;
using System.Text;
using BestHTTP;
using MiniJSON;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006D3 RID: 1747
	public class RemoveGameDataEndpoint : Endpoint
	{
		// Token: 0x06003E6A RID: 15978 RVA: 0x0013092C File Offset: 0x0012EB2C
		public RemoveGameDataEndpoint(string game, string[] keys, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base.DebugModuleName += ".User.GameData";
			if (string.IsNullOrEmpty(game))
			{
				throw new ArgumentException("'game' argument cannot be null or empty");
			}
			if (keys == null || keys.Length == 0)
			{
				throw new ArgumentException("'keys' argument cannot be null or empty");
			}
			base._URL = string.Format("/main/v1/user/me/data/{0}", game);
			base._HttpMethod = HTTPMethods.Delete;
			this._keys = keys;
		}

		// Token: 0x06003E6B RID: 15979 RVA: 0x0013099C File Offset: 0x0012EB9C
		protected override void _SetRequestParameters()
		{
			string s = Json.Serialize(this._keys);
			base._HTTPRequest.RawData = Encoding.UTF8.GetBytes(s);
		}

		// Token: 0x04002841 RID: 10305
		public const int kMaxKeyLength = 32;

		// Token: 0x04002842 RID: 10306
		public const int kMaxValueLength = 2048;

		// Token: 0x04002843 RID: 10307
		private string[] _keys;
	}
}
