using System;
using AsmodeeNet.Utils;
using BestHTTP;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006EB RID: 1771
	public class GetUserDetailsEndpoint : Endpoint<User>
	{
		// Token: 0x06003EA5 RID: 16037 RVA: 0x001324BC File Offset: 0x001306BC
		public GetUserDetailsEndpoint(int userId, Extras extras = Extras.None, OAuthGate oauthGate = null) : base(false, oauthGate)
		{
			base._URL = string.Format("/main/v1/user/{0}", userId);
			this.CtorCore(extras);
		}

		// Token: 0x06003EA6 RID: 16038 RVA: 0x001324E3 File Offset: 0x001306E3
		public GetUserDetailsEndpoint(Extras extras = Extras.None, OAuthGate oauthGate = null) : base(true, oauthGate)
		{
			base._URL = "/main/v1/user/me";
			this.CtorCore(extras);
		}

		// Token: 0x06003EA7 RID: 16039 RVA: 0x00132500 File Offset: 0x00130700
		private void CtorCore(Extras extras)
		{
			base.DebugModuleName += ".User";
			if (extras != Extras.None)
			{
				string text = "?extras=";
				if ((extras & Extras.Partners) != Extras.None)
				{
					text += "partners,";
				}
				if ((extras & Extras.Boardgames) != Extras.None)
				{
					text += "boardgames,";
				}
				if ((extras & Extras.Onlinegames) != Extras.None)
				{
					text += "onlinegames,";
				}
				if ((extras & Extras.Features) != Extras.None)
				{
					text += "features,";
				}
				text = text.Substring(0, text.Length - 1);
				base._URL += text;
			}
			base._HttpMethod = HTTPMethods.Get;
		}

		// Token: 0x06003EA8 RID: 16040 RVA: 0x0013259C File Offset: 0x0013079C
		protected override void ProcessResponse(Action<User, WebError> onCompletion)
		{
			Either<User, Builder<User>.BuilderErrors[]> either = new User.Builder(JsonUtility.FromJson<ApiGetUserDetailsResponse>(base._HTTPResponse.DataAsText)).Build(false);
			if (onCompletion != null)
			{
				if (either.Error == null)
				{
					onCompletion(either.Value, null);
					return;
				}
				onCompletion(null, new UserError(either.Error));
			}
		}
	}
}
