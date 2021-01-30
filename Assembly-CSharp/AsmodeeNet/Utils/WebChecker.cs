using System;
using System.Collections;
using UnityEngine;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000677 RID: 1655
	public static class WebChecker
	{
		// Token: 0x17000814 RID: 2068
		// (get) Token: 0x06003CCD RID: 15565 RVA: 0x0012BF02 File Offset: 0x0012A102
		public static bool IsNetworkReachable
		{
			get
			{
				return Application.internetReachability > NetworkReachability.NotReachable;
			}
		}

		// Token: 0x06003CCE RID: 15566 RVA: 0x0012BF0C File Offset: 0x0012A10C
		public static IEnumerator WebRequest(IEnumerator connectionSuccess, IEnumerator connectionError, string targetURL = "https://www.daysofwonder.com")
		{
			if (!WebChecker.IsNetworkReachable)
			{
				yield return connectionError;
			}
			else
			{
				WWW www = new WWW(targetURL);
				yield return www;
				if (www.error != null)
				{
					yield return connectionError;
				}
				else
				{
					yield return connectionSuccess;
				}
				www = null;
			}
			yield break;
		}

		// Token: 0x06003CCF RID: 15567 RVA: 0x0012BF29 File Offset: 0x0012A129
		public static IEnumerator WebRequest(Action connectionSuccess, Action connectionError, string targetURL = "https://www.daysofwonder.com")
		{
			if (!WebChecker.IsNetworkReachable)
			{
				if (connectionError != null)
				{
					connectionError();
				}
			}
			else
			{
				WWW www = new WWW(targetURL);
				yield return www;
				if (www.error != null)
				{
					if (connectionError != null)
					{
						connectionError();
					}
				}
				else if (connectionSuccess != null)
				{
					connectionSuccess();
				}
				www = null;
			}
			yield break;
		}

		// Token: 0x0400270A RID: 9994
		private const string _defaultTargetURL = "https://www.daysofwonder.com";
	}
}
