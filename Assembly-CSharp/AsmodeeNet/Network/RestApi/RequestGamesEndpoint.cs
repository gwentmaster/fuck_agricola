using System;
using System.Linq;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E9 RID: 1769
	public class RequestGamesEndpoint : Endpoint<ShowcaseProduct[]>
	{
		// Token: 0x06003E9C RID: 16028 RVA: 0x00132238 File Offset: 0x00130438
		public RequestGamesEndpoint(Channel channel, string lang, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Games";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestGamesEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E9D RID: 16029 RVA: 0x0013229C File Offset: 0x0013049C
		public RequestGamesEndpoint(Channel channel, string lang, GameProductTag tag, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Games";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}?tag={3}", new object[]
			{
				RequestGamesEndpoint._endpoint,
				channel,
				lang,
				tag
			});
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E9E RID: 16030 RVA: 0x00132318 File Offset: 0x00130518
		protected override void ProcessResponse(Action<ShowcaseProduct[], WebError> onCompletion)
		{
			ShowcaseProduct[] arg = (from x in JsonUtility.FromJson<ApiShowcaseInterstitialOrGamesResponse>(base._HTTPResponse.DataAsText).data.products
			select new ShowcaseProduct(x)).ToArray<ShowcaseProduct>();
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x06003E9F RID: 16031 RVA: 0x00132374 File Offset: 0x00130574
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestGamesEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x04002855 RID: 10325
		private static readonly string _endpoint = "/main/v3/showcase/games";
	}
}
