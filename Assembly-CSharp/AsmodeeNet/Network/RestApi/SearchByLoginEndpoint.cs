using System;
using System.Linq;
using UnityEngine;

namespace AsmodeeNet.Network.RestApi
{
	// Token: 0x020006E5 RID: 1765
	public class SearchByLoginEndpoint : BaseSearchByLoginEndpoint
	{
		// Token: 0x06003E94 RID: 16020 RVA: 0x00131DD4 File Offset: 0x0012FFD4
		public SearchByLoginEndpoint(string[] logins, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (logins == null || logins.Length == 0)
			{
				throw new ArgumentException("Logins array cannot be null and must at least contain one item.");
			}
			if (logins.Any((string x) => string.IsNullOrEmpty(x)))
			{
				throw new ArgumentException("A login cannot be null or empty.");
			}
			if (logins.Any((string x) => x.Length < 4))
			{
				throw new ArgumentException(string.Format("A login must be {0} characters long minimum.", 4));
			}
			if (logins.Any((string x) => x.Any((char y) => "()#|@^*%§!?:;.,$~".Contains(y))))
			{
				throw new ArgumentException(string.Format("{0}{1}", "A login cannot contain item from the following set : ", "()#|@^*%§!?:;.,$~"));
			}
			base._URL = string.Format("/main/v1/users?login={0}", string.Join(",", (from x in logins
			select WWW.EscapeURL(x)).ToArray<string>()));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x06003E95 RID: 16021 RVA: 0x00131F10 File Offset: 0x00130110
		public SearchByLoginEndpoint(string login, Extras extras, int offset = -1, int limit = -1, OAuthGate oauthGate = null) : base(oauthGate)
		{
			base.DebugModuleName += ".User.Search";
			if (string.IsNullOrEmpty(login))
			{
				throw new ArgumentException("A login cannot be null or empty.");
			}
			if (login.Length < 4)
			{
				throw new ArgumentException(string.Format("A login must be {0} characters long minimum.", 4));
			}
			if (login.Any((char x) => "()#|@^*%§!?:;.,$~".Contains(x)))
			{
				throw new ArgumentException(string.Format("{0}{1}", "A login cannot contain item from the following set : ", "()#|@^*%§!?:;.,$~"));
			}
			base._URL = string.Format("/main/v1/users?login={0}", WWW.EscapeURL(login));
			base.CtorCore(extras, offset, limit);
		}

		// Token: 0x0400284E RID: 10318
		public const int kLoginMinimalLength = 4;
	}
}
