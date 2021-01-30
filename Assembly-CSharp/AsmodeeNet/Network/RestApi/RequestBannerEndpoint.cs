using System;
using System.Text.RegularExpressions;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E7 RID: 1767
	public class RequestBannerEndpoint : Endpoint<ShowcaseProduct>
	{
		// Token: 0x06003E98 RID: 16024 RVA: 0x00132158 File Offset: 0x00130358
		public RequestBannerEndpoint(Channel channel, string lang, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base.DebugModuleName += ".Showcase.Banner";
			if (string.IsNullOrEmpty(lang))
			{
				throw new ArgumentException("'lang' argument cannot be null or empty");
			}
			base._URL = string.Format("{0}/{1}/{2}", RequestBannerEndpoint._endpoint, channel, lang);
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003E99 RID: 16025 RVA: 0x001321BC File Offset: 0x001303BC
		protected override void ProcessResponse(Action<ShowcaseProduct, WebError> onCompletion)
		{
			ShowcaseProduct arg = new ShowcaseProduct(JsonUtility.FromJson<ApiShowcaseBannerResponse>(base._HTTPResponse.DataAsText).data.product);
			if (onCompletion != null)
			{
				onCompletion(arg, null);
			}
		}

		// Token: 0x06003E9A RID: 16026 RVA: 0x001321F4 File Offset: 0x001303F4
		public static int GetEndpointVersion()
		{
			Match match = Regex.Match(RequestBannerEndpoint._endpoint, "v[0-9]+");
			if (!match.Success)
			{
				return -1;
			}
			return int.Parse(match.Value.Substring(1));
		}

		// Token: 0x0400284F RID: 10319
		private static readonly string _endpoint = "/main/v3/showcase/banner";
	}
}
