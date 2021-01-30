using System;

namespace Org.BouncyCastle.Utilities.Date
{
	// Token: 0x020002AC RID: 684
	public class DateTimeUtilities
	{
		// Token: 0x060016A0 RID: 5792 RVA: 0x00003425 File Offset: 0x00001625
		private DateTimeUtilities()
		{
		}

		// Token: 0x060016A1 RID: 5793 RVA: 0x00081838 File Offset: 0x0007FA38
		public static long DateTimeToUnixMs(DateTime dateTime)
		{
			if (dateTime.CompareTo(DateTimeUtilities.UnixEpoch) < 0)
			{
				throw new ArgumentException("DateTime value may not be before the epoch", "dateTime");
			}
			return (dateTime.Ticks - DateTimeUtilities.UnixEpoch.Ticks) / 10000L;
		}

		// Token: 0x060016A2 RID: 5794 RVA: 0x00081880 File Offset: 0x0007FA80
		public static DateTime UnixMsToDateTime(long unixMs)
		{
			return new DateTime(unixMs * 10000L + DateTimeUtilities.UnixEpoch.Ticks);
		}

		// Token: 0x060016A3 RID: 5795 RVA: 0x000818A8 File Offset: 0x0007FAA8
		public static long CurrentUnixMs()
		{
			return DateTimeUtilities.DateTimeToUnixMs(DateTime.UtcNow);
		}

		// Token: 0x0400151A RID: 5402
		public static readonly DateTime UnixEpoch = new DateTime(1970, 1, 1);
	}
}
