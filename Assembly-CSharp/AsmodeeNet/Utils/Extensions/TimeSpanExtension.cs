using System;

namespace AsmodeeNet.Utils.Extensions
{
	// Token: 0x0200067D RID: 1661
	public static class TimeSpanExtension
	{
		// Token: 0x06003CDC RID: 15580 RVA: 0x0012C230 File Offset: 0x0012A430
		public static string ToStringExtended(this TimeSpan timeSpan, string format)
		{
			TimeSpan timeSpan2 = TimeSpan.FromSeconds(timeSpan.TotalSeconds);
			string result = string.Empty;
			if (timeSpan2.TotalDays >= 7.0)
			{
				int num = (int)timeSpan2.TotalDays / 7;
				result = num + " week" + ((num >= 2) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromDays((double)(num * 7)));
			}
			if (timeSpan2.TotalDays >= 1.0)
			{
				result = (int)timeSpan2.TotalDays + " day" + ((timeSpan2.TotalDays >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromDays((double)((int)timeSpan2.TotalDays)));
			}
			if (timeSpan2.TotalHours >= 1.0)
			{
				result = (int)timeSpan2.TotalHours + " hour" + ((timeSpan2.TotalHours >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromHours((double)((int)timeSpan2.TotalHours)));
			}
			if (timeSpan2.TotalMinutes >= 1.0)
			{
				result = (int)timeSpan2.TotalMinutes + " minute" + ((timeSpan2.TotalMinutes >= 2.0) ? "s" : "");
				timeSpan2 = timeSpan2.Subtract(TimeSpan.FromMinutes((double)((int)timeSpan2.TotalMinutes)));
			}
			if (timeSpan2.TotalSeconds >= 1.0)
			{
				result = (int)timeSpan2.TotalSeconds + " second" + ((timeSpan2.TotalSeconds >= 2.0) ? "s" : "");
			}
			return result;
		}
	}
}
