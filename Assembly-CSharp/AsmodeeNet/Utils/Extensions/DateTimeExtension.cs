using System;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x02000679 RID: 1657
	public static class DateTimeExtension
	{
		// Token: 0x06003CD2 RID: 15570 RVA: 0x0012BFB8 File Offset: 0x0012A1B8
		public static DateTime UnixTimeStampToDateTime(this DateTime dateTime, double unixTimeStamp)
		{
			DateTime result = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
			result = result.AddMilliseconds(unixTimeStamp).ToLocalTime();
			return result;
		}
	}
}
