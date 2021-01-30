using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E4 RID: 1764
	public class SearchLoginsStartingWithEndpoint : BaseSearchByLoginEndpoint
	{
		// Token: 0x06003E93 RID: 16019 RVA: 0x00131D08 File Offset: 0x0012FF08
		public SearchLoginsStartingWithEndpoint(string loginStart, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(loginStart))
			{
				throw new ArgumentException("A login cannot be null or empty.");
			}
			if (loginStart.Any((char x) => "()#|@^*%§!?:;.,$~".Contains(x)))
			{
				throw new ArgumentException(string.Format("{0}{1}", "A login cannot contain item from the following set : ", "()#|@^*%§!?:;.,$~"));
			}
			if (loginStart.Length < 2)
			{
				throw new ArgumentException(string.Format("A login must be {0} characters long minimum.", 2));
			}
			loginStart += "%";
			base._URL = string.Format("/main/v1/users?login={0}", WWW.EscapeURL(loginStart));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x0400284D RID: 10317
		public const int kLoginMinimalLength = 2;
	}
}
