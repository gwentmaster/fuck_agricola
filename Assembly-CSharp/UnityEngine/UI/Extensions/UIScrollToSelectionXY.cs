using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D1 RID: 465
	[AddComponentMenu("UI/Extensions/UI ScrollTo Selection XY")]
	[RequireComponent(typeof(ScrollRect))]
	public class UIScrollToSelectionXY : MonoBehaviour
	{
		// Token: 0x060011CA RID: 4554 RVA: 0x0006DE7E File Offset: 0x0006C07E
		private void Start()
		{
			this.targetScrollRect = base.GetComponent<ScrollRect>();
			this.scrollWindow = this.targetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x0006DE9D File Offset: 0x0006C09D
		private void Update()
		{
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0006DEA8 File Offset: 0x0006C0A8
		private void ScrollRectToLevelSelection()
		{
			EventSystem current = EventSystem.current;
			if (this.targetScrollRect == null || this.layoutListGroup == null || this.scrollWindow == null)
			{
				return;
			}
			RectTransform rectTransform = (current.currentSelectedGameObject != null) ? current.currentSelectedGameObject.GetComponent<RectTransform>() : null;
			if (rectTransform != this.targetScrollObject)
			{
				this.scrollToSelection = true;
			}
			if (rectTransform == null || !this.scrollToSelection || rectTransform.transform.parent != this.layoutListGroup.transform)
			{
				return;
			}
			bool flag = false;
			bool flag2 = false;
			if (this.targetScrollRect.vertical)
			{
				float num = -rectTransform.anchoredPosition.y;
				float num2 = this.layoutListGroup.anchoredPosition.y - num;
				this.targetScrollRect.verticalNormalizedPosition += num2 / this.layoutListGroup.sizeDelta.y * Time.deltaTime * this.scrollSpeed;
				flag2 = (Mathf.Abs(num2) < 2f);
			}
			if (this.targetScrollRect.horizontal)
			{
				float num3 = -rectTransform.anchoredPosition.x;
				float num4 = this.layoutListGroup.anchoredPosition.x - num3;
				this.targetScrollRect.horizontalNormalizedPosition += num4 / this.layoutListGroup.sizeDelta.x * Time.deltaTime * this.scrollSpeed;
				flag = (Mathf.Abs(num4) < 2f);
			}
			if (flag && flag2)
			{
				this.scrollToSelection = false;
			}
			this.targetScrollObject = rectTransform;
		}

		// Token: 0x04001032 RID: 4146
		public float scrollSpeed = 10f;

		// Token: 0x04001033 RID: 4147
		[SerializeField]
		private RectTransform layoutListGroup;

		// Token: 0x04001034 RID: 4148
		private RectTransform targetScrollObject;

		// Token: 0x04001035 RID: 4149
		private bool scrollToSelection = true;

		// Token: 0x04001036 RID: 4150
		private RectTransform scrollWindow;

		// Token: 0x04001037 RID: 4151
		private RectTransform currentCanvas;

		// Token: 0x04001038 RID: 4152
		private ScrollRect targetScrollRect;
	}
}
