using System;

namespace Org.BouncyCastle.Crypto.Tls
{
	// Token: 0x0200039B RID: 923
	public abstract class AlertLevel
	{
		// Token: 0x060022ED RID: 8941 RVA: 0x000B6396 File Offset: 0x000B4596
		public static string GetName(byte alertDescription)
		{
			if (alertDescription == 1)
			{
				return "warning";
			}
			if (alertDescription != 2)
			{
				return "UNKNOWN";
			}
			return "fatal";
		}

		// Token: 0x060022EE RID: 8942 RVA: 0x000B63B3 File Offset: 0x000B45B3
		public static string GetText(byte alertDescription)
		{
			return string.Concat(new object[]
			{
				AlertLevel.GetName(alertDescription),
				"(",
				alertDescription,
				")"
			});
		}

		// Token: 0x040016D7 RID: 5847
		public const byte warning = 1;

		// Token: 0x040016D8 RID: 5848
		public const byte fatal = 2;
	}
}
