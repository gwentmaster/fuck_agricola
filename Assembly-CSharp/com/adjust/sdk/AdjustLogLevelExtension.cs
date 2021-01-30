using System;

namespace com.adjust.sdk
{
	// Token: 0x02000748 RID: 1864
	public static class AdjustLogLevelExtension
	{
		// Token: 0x0600415C RID: 16732 RVA: 0x0013B428 File Offset: 0x00139628
		public static string ToLowercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "verbose";
			case AdjustLogLevel.Debug:
				return "debug";
			case AdjustLogLevel.Info:
				return "info";
			case AdjustLogLevel.Warn:
				return "warn";
			case AdjustLogLevel.Error:
				return "error";
			case AdjustLogLevel.Assert:
				return "assert";
			case AdjustLogLevel.Suppress:
				return "suppress";
			default:
				return "unknown";
			}
		}

		// Token: 0x0600415D RID: 16733 RVA: 0x0013B48C File Offset: 0x0013968C
		public static string ToUppercaseString(this AdjustLogLevel AdjustLogLevel)
		{
			switch (AdjustLogLevel)
			{
			case AdjustLogLevel.Verbose:
				return "VERBOSE";
			case AdjustLogLevel.Debug:
				return "DEBUG";
			case AdjustLogLevel.Info:
				return "INFO";
			case AdjustLogLevel.Warn:
				return "WARN";
			case AdjustLogLevel.Error:
				return "ERROR";
			case AdjustLogLevel.Assert:
				return "ASSERT";
			case AdjustLogLevel.Suppress:
				return "SUPPRESS";
			default:
				return "UNKNOWN";
			}
		}
	}
}
