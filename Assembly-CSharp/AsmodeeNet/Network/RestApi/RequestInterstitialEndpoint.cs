using System;
using System.Linq;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006EA RID: 1770
	public class RequestInterstitialEndpoint : Endpoint<ShowcaseProduct[]>
	{
		// Token: 0x06003EA1 RID: 16033 RVA: 0x001323B8 File Offset: 0x001305B8
		public RequestInterstitialEndpoint(Channel channel, string lang, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Interstitial";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestInterstitialEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003EA2 RID: 16034 RVA: 0x0013241C File Offset: 0x0013061C
		protected override void ProcessResponse(Action<ShowcaseProduct[], WebError> onCompletion)
		{
			ShowcaseProduct[] arg = (from x in JsonUtility.FromJson<ApiShowcaseInterstitialOrGamesResponse>(base._HTTPResponse.DataAsText).data.products
			select new ShowcaseProduct(x)).ToArray<ShowcaseProduct>();
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x06003EA3 RID: 16035 RVA: 0x00132478 File Offset: 0x00130678
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestInterstitialEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x04002856 RID: 10326
		private static readonly string _endpoint = "/main/v3/showcase/interstitial";
	}
}
