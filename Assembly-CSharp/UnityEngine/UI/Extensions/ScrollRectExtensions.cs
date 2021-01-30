using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001C8 RID: 456
	public static class ScrollRectExtensions
	{
		// Token: 0x0600117F RID: 4479 RVA: 0x0006D1FA File Offset: 0x0006B3FA
		public static void ScrollToTop(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 1f);
		}

		// Token: 0x06001180 RID: 4480 RVA: 0x0006D211 File Offset: 0x0006B411
		public static void ScrollToBottom(this ScrollRect scrollRect)
		{
			scrollRect.normalizedPosition = new Vector2(0f, 0f);
		}
	}
}
