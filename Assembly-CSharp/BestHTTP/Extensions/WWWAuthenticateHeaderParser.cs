using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F7 RID: 1527
	public sealed class WWWAuthenticateHeaderParser : KeyValuePairList
	{
		// Token: 0x060037FA RID: 14330 RVA: 0x001130B4 File Offset: 0x001112B4
		public WWWAuthenticateHeaderParser(string headerValue)
		{
			base.Values = this.ParseQuotedHeader(headerValue);
		}

		// Token: 0x060037FB RID: 14331 RVA: 0x001130CC File Offset: 0x001112CC
		private List<HeaderValue> ParseQuotedHeader(string str)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			if (str != null)
			{
				int i = 0;
				string key = str.Read(ref i, (char ch) => !char.IsWhiteSpace(ch) && !char.IsControl(ch), true).TrimAndLower();
				list.Add(new HeaderValue(key));
				while (i < str.Length)
				{
					HeaderValue headerValue = new HeaderValue(str.Read(ref i, '=', true).TrimAndLower());
					str.SkipWhiteSpace(ref i);
					headerValue.Value = str.ReadPossibleQuotedText(ref i);
					list.Add(headerValue);
				}
			}
			return list;
		}
	}
}
