using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001B8 RID: 440
	[AddComponentMenu("UI/Extensions/Bound Tooltip/Tooltip Trigger")]
	public class BoundTooltipTrigger : MonoBehaviour, IPointerEnterHandler, IEventSystemHandler, IPointerExitHandler, ISelectHandler, IDeselectHandler
	{
		// Token: 0x0600110D RID: 4365 RVA: 0x0006AB18 File Offset: 0x00068D18
		public void OnPointerEnter(PointerEventData eventData)
		{
			if (this.useMousePosition)
			{
				this.StartHover(new Vector3(eventData.position.x, eventData.position.y, 0f));
				return;
			}
			this.StartHover(base.transform.position + this.offset);
		}

		// Token: 0x0600110E RID: 4366 RVA: 0x0006AB70 File Offset: 0x00068D70
		public void OnSelect(BaseEventData eventData)
		{
			this.StartHover(base.transform.position);
		}

		// Token: 0x0600110F RID: 4367 RVA: 0x0006AB83 File Offset: 0x00068D83
		public void OnPointerExit(PointerEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x06001110 RID: 4368 RVA: 0x0006AB83 File Offset: 0x00068D83
		public void OnDeselect(BaseEventData eventData)
		{
			this.StopHover();
		}

		// Token: 0x06001111 RID: 4369 RVA: 0x0006AB8B File Offset: 0x00068D8B
		private void StartHover(Vector3 position)
		{
			BoundTooltipItem.Instance.ShowTooltip(this.text, position);
		}

		// Token: 0x06001112 RID: 4370 RVA: 0x0006AB9E File Offset: 0x00068D9E
		private void StopHover()
		{
			BoundTooltipItem.Instance.HideTooltip();
		}

		// Token: 0x04000FC3 RID: 4035
		[TextArea]
		public string text;

		// Token: 0x04000FC4 RID: 4036
		public bool useMousePosition;

		// Token: 0x04000FC5 RID: 4037
		public Vector3 offset;
	}
}
