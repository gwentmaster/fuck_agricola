using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000178 RID: 376
	[AddComponentMenu("UI/Extensions/MultiTouchScrollRect")]
	public class MultiTouchScrollRect : ScrollRect
	{
		// Token: 0x06000E85 RID: 3717 RVA: 0x0005C8B6 File Offset: 0x0005AAB6
		public override void OnBeginDrag(PointerEventData eventData)
		{
			this.pid = eventData.pointerId;
			base.OnBeginDrag(eventData);
		}

		// Token: 0x06000E86 RID: 3718 RVA: 0x0005C8CB File Offset: 0x0005AACB
		public override void OnDrag(PointerEventData eventData)
		{
			if (this.pid == eventData.pointerId)
			{
				base.OnDrag(eventData);
			}
		}

		// Token: 0x06000E87 RID: 3719 RVA: 0x0005C8E2 File Offset: 0x0005AAE2
		public override void OnEndDrag(PointerEventData eventData)
		{
			this.pid = -100;
			base.OnEndDrag(eventData);
		}

		// Token: 0x04000E2D RID: 3629
		private int pid = -100;
	}
}
