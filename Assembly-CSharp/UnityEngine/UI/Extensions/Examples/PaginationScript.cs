using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions.Examples
{
	// Token: 0x020001F9 RID: 505
	public class PaginationScript : MonoBehaviour, IPointerClickHandler, IEventSystemHandler
	{
		// Token: 0x06001294 RID: 4756 RVA: 0x00070FDB File Offset: 0x0006F1DB
		public void OnPointerClick(PointerEventData eventData)
		{
			if (this.hss != null)
			{
				this.hss.GoToScreen(this.Page);
			}
		}

		// Token: 0x040010C9 RID: 4297
		public HorizontalScrollSnap hss;

		// Token: 0x040010CA RID: 4298
		public int Page;
	}
}
