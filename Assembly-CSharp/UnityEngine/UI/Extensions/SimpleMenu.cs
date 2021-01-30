using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001AB RID: 427
	public abstract class SimpleMenu<T> : Menu<!0> where T : SimpleMenu<!0>
	{
		// Token: 0x06001087 RID: 4231 RVA: 0x00067B44 File Offset: 0x00065D44
		public static void Show()
		{
			Menu<!0>.Open();
		}

		// Token: 0x06001088 RID: 4232 RVA: 0x00067B4B File Offset: 0x00065D4B
		public static void Hide()
		{
			Menu<!0>.Close();
		}
	}
}
