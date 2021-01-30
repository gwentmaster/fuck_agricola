using System;

namespace BestHTTP
{
	// Token: 0x02000574 RID: 1396
	// (Invoke) Token: 0x060032A2 RID: 12962
	public delegate bool OnBeforeRedirectionDelegate(HTTPRequest originalRequest, HTTPResponse response, Uri redirectUri);
}
