using System;

namespace UTNotifications
{
	// Token: 0x02000156 RID: 342
	public sealed class TimeUtils
	{
		// Token: 0x06000D60 RID: 3424 RVA: 0x00057114 File Offset: 0x00055314
		public static DateTime ToDateTime(int secondsFromNow)
		{
			return DateTime.Now.AddSeconds((double)secondsFromNow);
		}

		// Token: 0x06000D61 RID: 3425 RVA: 0x00057130 File Offset: 0x00055330
		public static DateTime UnixTimestampMillisToDateTime(double unixTimestampMillis)
		{
			return TimeUtils.unixBaseDateTime.AddMilliseconds(unixTimestampMillis).ToLocalTime();
		}

		// Token: 0x06000D62 RID: 3426 RVA: 0x00057154 File Offset: 0x00055354
		public static int ToSecondsFromNow(DateTime dateTime)
		{
			return (int)(dateTime - DateTime.Now).TotalSeconds;
		}

		// Token: 0x06000D63 RID: 3427 RVA: 0x00057175 File Offset: 0x00055375
		public static int MinutesToSeconds(int minutes)
		{
			return minutes * 60;
		}

		// Token: 0x06000D64 RID: 3428 RVA: 0x0005717B File Offset: 0x0005537B
		public static int HoursToSeconds(int hours)
		{
			return hours * 3600;
		}

		// Token: 0x06000D65 RID: 3429 RVA: 0x00057184 File Offset: 0x00055384
		public static int DaysToSeconds(int days)
		{
			return days * 86400;
		}

		// Token: 0x06000D66 RID: 3430 RVA: 0x0005718D File Offset: 0x0005538D
		public static int WeeksToSeconds(int weeks)
		{
			return weeks * 604800;
		}

		// Token: 0x04000D53 RID: 3411
		private static readonly DateTime unixBaseDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
	}
}
