using System;

namespace com.adjust.sdk
{
	// Token: 0x02000743 RID: 1859
	public static class AdjustEnvironmentExtension
	{
		// Token: 0x06004130 RID: 16688 RVA: 0x0013AEE6 File Offset: 0x001390E6
		public static string ToLowercaseString(this AdjustEnvironment adjustEnvironment)
		{
			if (adjustEnvironment == AdjustEnvironment.Sandbox)
			{
				return "sandbox";
			}
			if (adjustEnvironment != AdjustEnvironment.Production)
			{
				return "unknown";
			}
			return "production";
		}
	}
}
