using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001CD RID: 461
	public static class UIExtensionMethods
	{
		// Token: 0x060011A0 RID: 4512 RVA: 0x0006D6C0 File Offset: 0x0006B8C0
		public static Canvas GetParentCanvas(this RectTransform rt)
		{
			RectTransform rectTransform = rt;
			Canvas canvas = rt.GetComponent<Canvas>();
			int num = 0;
			while (canvas == null || num > 50)
			{
				canvas = rt.GetComponentInParent<Canvas>();
				if (canvas == null)
				{
					rectTransform = rectTransform.parent.GetComponent<RectTransform>();
					num++;
				}
			}
			return canvas;
		}
	}
}
