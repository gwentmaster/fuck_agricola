using System;
using System.Diagnostics;
using Fabric.Crashlytics.Internal;

namespace Fabric.Crashlytics
{
	// Token: 0x0200025C RID: 604
	public class Crashlytics
	{
		// Token: 0x06001311 RID: 4881 RVA: 0x000721AA File Offset: 0x000703AA
		public static void SetDebugMode(bool debugMode)
		{
			Crashlytics.impl.SetDebugMode(debugMode);
		}

		// Token: 0x06001312 RID: 4882 RVA: 0x000721B7 File Offset: 0x000703B7
		public static void Crash()
		{
			Crashlytics.impl.Crash();
		}

		// Token: 0x06001313 RID: 4883 RVA: 0x000721C3 File Offset: 0x000703C3
		public static void ThrowNonFatal()
		{
			Crashlytics.impl.ThrowNonFatal();
		}

		// Token: 0x06001314 RID: 4884 RVA: 0x000721CF File Offset: 0x000703CF
		public static void Log(string message)
		{
			Crashlytics.impl.Log(message);
		}

		// Token: 0x06001315 RID: 4885 RVA: 0x000721DC File Offset: 0x000703DC
		public static void SetKeyValue(string key, string value)
		{
			Crashlytics.impl.SetKeyValue(key, value);
		}

		// Token: 0x06001316 RID: 4886 RVA: 0x000721EA File Offset: 0x000703EA
		public static void SetUserIdentifier(string identifier)
		{
			Crashlytics.impl.SetUserIdentifier(identifier);
		}

		// Token: 0x06001317 RID: 4887 RVA: 0x000721F7 File Offset: 0x000703F7
		public static void SetUserEmail(string email)
		{
			Crashlytics.impl.SetUserEmail(email);
		}

		// Token: 0x06001318 RID: 4888 RVA: 0x00072204 File Offset: 0x00070404
		public static void SetUserName(string name)
		{
			Crashlytics.impl.SetUserName(name);
		}

		// Token: 0x06001319 RID: 4889 RVA: 0x00072211 File Offset: 0x00070411
		public static void RecordCustomException(string name, string reason, StackTrace stackTrace)
		{
			Crashlytics.impl.RecordCustomException(name, reason, stackTrace);
		}

		// Token: 0x0600131A RID: 4890 RVA: 0x00072220 File Offset: 0x00070420
		public static void RecordCustomException(string name, string reason, string stackTraceString)
		{
			Crashlytics.impl.RecordCustomException(name, reason, stackTraceString);
		}

		// Token: 0x040012EC RID: 4844
		private static readonly Impl impl = Impl.Make();
	}
}
