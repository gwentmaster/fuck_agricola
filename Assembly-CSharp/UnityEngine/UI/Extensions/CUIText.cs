using System;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200018F RID: 399
	[RequireComponent(typeof(RectTransform))]
	[RequireComponent(typeof(Text))]
	[AddComponentMenu("UI/Effects/Extensions/Curly UI Text")]
	public class CUIText : CUIGraphic
	{
		// Token: 0x06000F73 RID: 3955 RVA: 0x00061F38 File Offset: 0x00060138
		public override void ReportSet()
		{
			if (this.uiGraphic == null)
			{
				this.uiGraphic = base.GetComponent<Text>();
			}
			base.ReportSet();
		}
	}
}
