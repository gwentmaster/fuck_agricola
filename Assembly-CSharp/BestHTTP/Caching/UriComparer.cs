using System;
using System.Collections.Generic;

namespace BestHTTP.Caching
{
	// Token: 0x02000616 RID: 1558
	public sealed class UriComparer : IEqualityComparer<Uri>
	{
		// Token: 0x0600394B RID: 14667 RVA: 0x0011CF22 File Offset: 0x0011B122
		public bool Equals(Uri x, Uri y)
		{
			return Uri.Compare(x, y, UriComponents.HttpRequestUrl, UriFormat.SafeUnescaped, StringComparison.Ordinal) == 0;
		}

		// Token: 0x0600394C RID: 14668 RVA: 0x0011CF32 File Offset: 0x0011B132
		public int GetHashCode(Uri uri)
		{
			return uri.ToString().GetHashCode();
		}
	}
}
