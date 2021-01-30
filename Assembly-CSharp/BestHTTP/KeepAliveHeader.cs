using System;
using System.Collections.Generic;
using BestHTTP.Extensions;

namespace BestHTTP
{
	// Token: 0x02000566 RID: 1382
	internal sealed class KeepAliveHeader
	{
		// Token: 0x17000601 RID: 1537
		// (get) Token: 0x0600322D RID: 12845 RVA: 0x000FF9E8 File Offset: 0x000FDBE8
		// (set) Token: 0x0600322E RID: 12846 RVA: 0x000FF9F0 File Offset: 0x000FDBF0
		public TimeSpan TimeOut { get; private set; }

		// Token: 0x17000602 RID: 1538
		// (get) Token: 0x0600322F RID: 12847 RVA: 0x000FF9F9 File Offset: 0x000FDBF9
		// (set) Token: 0x06003230 RID: 12848 RVA: 0x000FFA01 File Offset: 0x000FDC01
		public int MaxRequests { get; private set; }

		// Token: 0x06003231 RID: 12849 RVA: 0x000FFA0C File Offset: 0x000FDC0C
		public void Parse(List<string> headerValues)
		{
			HeaderParser headerParser = new HeaderParser(headerValues[0]);
			HeaderValue headerValue;
			if (headerParser.TryGet("timeout", out headerValue) && headerValue.HasValue)
			{
				int num = 0;
				if (int.TryParse(headerValue.Value, out num))
				{
					this.TimeOut = TimeSpan.FromSeconds((double)num);
				}
				else
				{
					this.TimeOut = TimeSpan.MaxValue;
				}
			}
			if (headerParser.TryGet("max", out headerValue) && headerValue.HasValue)
			{
				int maxRequests = 0;
				if (int.TryParse("max", out maxRequests))
				{
					this.MaxRequests = maxRequests;
					return;
				}
				this.MaxRequests = int.MaxValue;
			}
		}
	}
}
