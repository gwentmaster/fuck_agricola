using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017E RID: 382
	[AddComponentMenu("UI/Extensions/RescalePanels/RescaleDragPanel")]
	public class RescaleDragPanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06000EB9 RID: 3769 RVA: 0x0005D9D4 File Offset: 0x0005BBD4
		private void Awake()
		{
			Canvas componentInParent = base.GetComponentInParent<Canvas>();
			if (componentInParent != null)
			{
				this.canvasRectTransform = (componentInParent.transform as RectTransform);
				this.panelRectTransform = (base.transform.parent as RectTransform);
				this.goTransform = base.transform.parent;
			}
		}

		// Token: 0x06000EBA RID: 3770 RVA: 0x0005DA29 File Offset: 0x0005BC29
		public void OnPointerDown(PointerEventData data)
		{
			this.panelRectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.panelRectTransform, data.position, data.pressEventCamera, out this.pointerOffset);
		}

		// Token: 0x06000EBB RID: 3771 RVA: 0x0005DA54 File Offset: 0x0005BC54
		public void OnDrag(PointerEventData data)
		{
			if (this.panelRectTransform == null)
			{
				return;
			}
			Vector2 screenPoint = this.ClampToWindow(data);
			Vector2 a;
			if (RectTransformUtility.ScreenPointToLocalPointInRectangle(this.canvasRectTransform, screenPoint, data.pressEventCamera, out a))
			{
				this.panelRectTransform.localPosition = a - new Vector2(this.pointerOffset.x * this.goTransform.localScale.x, this.pointerOffset.y * this.goTransform.localScale.y);
			}
		}

		// Token: 0x06000EBC RID: 3772 RVA: 0x0005DAE4 File Offset: 0x0005BCE4
		private Vector2 ClampToWindow(PointerEventData data)
		{
			Vector2 position = data.position;
			Vector3[] array = new Vector3[4];
			this.canvasRectTransform.GetWorldCorners(array);
			float x = Mathf.Clamp(position.x, array[0].x, array[2].x);
			float y = Mathf.Clamp(position.y, array[0].y, array[2].y);
			return new Vector2(x, y);
		}

		// Token: 0x04000E5F RID: 3679
		private Vector2 pointerOffset;

		// Token: 0x04000E60 RID: 3680
		private RectTransform canvasRectTransform;

		// Token: 0x04000E61 RID: 3681
		private RectTransform panelRectTransform;

		// Token: 0x04000E62 RID: 3682
		private Transform goTransform;
	}
}
