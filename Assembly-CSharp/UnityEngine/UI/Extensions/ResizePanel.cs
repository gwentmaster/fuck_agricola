using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000180 RID: 384
	[AddComponentMenu("UI/Extensions/RescalePanels/ResizePanel")]
	public class ResizePanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06000EC2 RID: 3778 RVA: 0x0005DCF0 File Offset: 0x0005BEF0
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			float width = this.rectTransform.rect.width;
			float height = this.rectTransform.rect.height;
			this.ratio = height / width;
			this.minSize = new Vector2(0.1f * width, 0.1f * height);
			this.maxSize = new Vector2(10f * width, 10f * height);
		}

		// Token: 0x06000EC3 RID: 3779 RVA: 0x0005DD76 File Offset: 0x0005BF76
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x06000EC4 RID: 3780 RVA: 0x0005DDA4 File Offset: 0x0005BFA4
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector2 vector = this.rectTransform.sizeDelta;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector2(vector2.x, this.ratio * vector2.x);
			vector = new Vector2(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y));
			this.rectTransform.sizeDelta = vector;
			this.previousPointerPosition = this.currentPointerPosition;
		}

		// Token: 0x04000E6B RID: 3691
		public Vector2 minSize;

		// Token: 0x04000E6C RID: 3692
		public Vector2 maxSize;

		// Token: 0x04000E6D RID: 3693
		private RectTransform rectTransform;

		// Token: 0x04000E6E RID: 3694
		private Vector2 currentPointerPosition;

		// Token: 0x04000E6F RID: 3695
		private Vector2 previousPointerPosition;

		// Token: 0x04000E70 RID: 3696
		private float ratio;
	}
}
