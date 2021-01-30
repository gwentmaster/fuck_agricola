using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A3 RID: 419
	public class ScrollSnapScrollbarHelper : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IPointerUpHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
	{
		// Token: 0x0600102B RID: 4139 RVA: 0x000663CE File Offset: 0x000645CE
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x0600102C RID: 4140 RVA: 0x000663D6 File Offset: 0x000645D6
		public void OnDrag(PointerEventData eventData)
		{
			this.ss.CurrentPage();
		}

		// Token: 0x0600102D RID: 4141 RVA: 0x000663E4 File Offset: 0x000645E4
		public void OnEndDrag(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x0600102E RID: 4142 RVA: 0x000663CE File Offset: 0x000645CE
		public void OnPointerDown(PointerEventData eventData)
		{
			this.OnScrollBarDown();
		}

		// Token: 0x0600102F RID: 4143 RVA: 0x000663E4 File Offset: 0x000645E4
		public void OnPointerUp(PointerEventData eventData)
		{
			this.OnScrollBarUp();
		}

		// Token: 0x06001030 RID: 4144 RVA: 0x000663EC File Offset: 0x000645EC
		private void OnScrollBarDown()
		{
			if (this.ss != null)
			{
				this.ss.SetLerp(false);
				this.ss.StartScreenChange();
			}
		}

		// Token: 0x06001031 RID: 4145 RVA: 0x0006640D File Offset: 0x0006460D
		private void OnScrollBarUp()
		{
			this.ss.SetLerp(true);
			this.ss.ChangePage(this.ss.CurrentPage());
		}

		// Token: 0x04000F54 RID: 3924
		internal IScrollSnap ss;
	}
}
