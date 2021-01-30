using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CC RID: 460
	internal static class SetPropertyUtility
	{
		// Token: 0x0600119D RID: 4509 RVA: 0x0006D600 File Offset: 0x0006B800
		public static bool SetColor(ref Color currentValue, Color newValue)
		{
			if (currentValue.r == newValue.r && currentValue.g == newValue.g && currentValue.b == newValue.b && currentValue.a == newValue.a)
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600119E RID: 4510 RVA: 0x0006D64F File Offset: 0x0006B84F
		public static bool SetStruct<T>(ref T currentValue, T newValue) where T : struct
		{
			if (currentValue.Equals(newValue))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}

		// Token: 0x0600119F RID: 4511 RVA: 0x0006D670 File Offset: 0x0006B870
		public static bool SetClass<T>(ref T currentValue, T newValue) where T : class
		{
			if ((currentValue == null && newValue == null) || (currentValue != null && currentValue.Equals(newValue)))
			{
				return false;
			}
			currentValue = newValue;
			return true;
		}
	}
}
