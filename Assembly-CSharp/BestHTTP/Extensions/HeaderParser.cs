using System;
using System.Collections.Generic;

namespace BestHTTP.Extensions
{
	// Token: 0x020005F2 RID: 1522
	public sealed class HeaderParser : KeyValuePairList
	{
		// Token: 0x060037E2 RID: 14306 RVA: 0x00112B6C File Offset: 0x00110D6C
		public HeaderParser(string headerStr)
		{
			base.Values = this.Parse(headerStr);
		}

		// Token: 0x060037E3 RID: 14307 RVA: 0x00112B84 File Offset: 0x00110D84
		private List<HeaderValue> Parse(string headerStr)
		{
			List<HeaderValue> list = new List<HeaderValue>();
			int i = 0;
			try
			{
				while (i < headerStr.Length)
				{
					HeaderValue headerValue = new HeaderValue();
					headerValue.Parse(headerStr, ref i);
					list.Add(headerValue);
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HeaderParser - Parse", headerStr, ex);
			}
			return list;
		}
	}
}
