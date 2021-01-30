using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200017F RID: 383
	[AddComponentMenu("UI/Extensions/RescalePanels/RescalePanel")]
	public class RescalePanel : MonoBehaviour, IPointerDownHandler, IEventSystemHandler, IDragHandler
	{
		// Token: 0x06000EBE RID: 3774 RVA: 0x0005DB58 File Offset: 0x0005BD58
		private void Awake()
		{
			this.rectTransform = base.transform.parent.GetComponent<RectTransform>();
			this.goTransform = base.transform.parent;
			this.thisRectTransform = base.GetComponent<RectTransform>();
			this.sizeDelta = this.thisRectTransform.sizeDelta;
		}

		// Token: 0x06000EBF RID: 3775 RVA: 0x0005DBA9 File Offset: 0x0005BDA9
		public void OnPointerDown(PointerEventData data)
		{
			this.rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.previousPointerPosition);
		}

		// Token: 0x06000EC0 RID: 3776 RVA: 0x0005DBD4 File Offset: 0x0005BDD4
		public void OnDrag(PointerEventData data)
		{
			if (this.rectTransform == null)
			{
				return;
			}
			Vector3 vector = this.goTransform.localScale;
			RectTransformUtility.ScreenPointToLocalPointInRectangle(this.rectTransform, data.position, data.pressEventCamera, out this.currentPointerPosition);
			Vector2 vector2 = this.currentPointerPosition - this.previousPointerPosition;
			vector += new Vector3(-vector2.y * 0.001f, -vector2.y * 0.001f, 0f);
			vector = new Vector3(Mathf.Clamp(vector.x, this.minSize.x, this.maxSize.x), Mathf.Clamp(vector.y, this.minSize.y, this.maxSize.y), 1f);
			this.goTransform.localScale = vector;
			this.previousPointerPosition = this.currentPointerPosition;
			float num = this.sizeDelta.x / this.goTransform.localScale.x;
			Vector2 vector3 = new Vector2(num, num);
			this.thisRectTransform.sizeDelta = vector3;
		}

		// Token: 0x04000E63 RID: 3683
		public Vector2 minSize;

		// Token: 0x04000E64 RID: 3684
		public Vector2 maxSize;

		// Token: 0x04000E65 RID: 3685
		private RectTransform rectTransform;

		// Token: 0x04000E66 RID: 3686
		private Transform goTransform;

		// Token: 0x04000E67 RID: 3687
		private Vector2 currentPointerPosition;

		// Token: 0x04000E68 RID: 3688
		private Vector2 previousPointerPosition;

		// Token: 0x04000E69 RID: 3689
		private RectTransform thisRectTransform;

		// Token: 0x04000E6A RID: 3690
		private Vector2 sizeDelta;
	}
}
