using System;
using System.Collections.Generic;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001D0 RID: 464
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/UIScrollToSelection")]
	public class UIScrollToSelection : MonoBehaviour
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x060011AF RID: 4527 RVA: 0x0006DA69 File Offset: 0x0006BC69
		protected RectTransform LayoutListGroup
		{
			get
			{
				if (!(this.TargetScrollRect != null))
				{
					return null;
				}
				return this.TargetScrollRect.content;
			}
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x060011B0 RID: 4528 RVA: 0x0006DA86 File Offset: 0x0006BC86
		protected UIScrollToSelection.ScrollType ScrollDirection
		{
			get
			{
				return this.scrollDirection;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x060011B1 RID: 4529 RVA: 0x0006DA8E File Offset: 0x0006BC8E
		protected float ScrollSpeed
		{
			get
			{
				return this.scrollSpeed;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x060011B2 RID: 4530 RVA: 0x0006DA96 File Offset: 0x0006BC96
		protected bool CancelScrollOnInput
		{
			get
			{
				return this.cancelScrollOnInput;
			}
		}

		// Token: 0x17000143 RID: 323
		// (get) Token: 0x060011B3 RID: 4531 RVA: 0x0006DA9E File Offset: 0x0006BC9E
		protected List<KeyCode> CancelScrollKeycodes
		{
			get
			{
				return this.cancelScrollKeycodes;
			}
		}

		// Token: 0x17000144 RID: 324
		// (get) Token: 0x060011B4 RID: 4532 RVA: 0x0006DAA6 File Offset: 0x0006BCA6
		// (set) Token: 0x060011B5 RID: 4533 RVA: 0x0006DAAE File Offset: 0x0006BCAE
		protected RectTransform ScrollWindow { get; set; }

		// Token: 0x17000145 RID: 325
		// (get) Token: 0x060011B6 RID: 4534 RVA: 0x0006DAB7 File Offset: 0x0006BCB7
		// (set) Token: 0x060011B7 RID: 4535 RVA: 0x0006DABF File Offset: 0x0006BCBF
		protected ScrollRect TargetScrollRect { get; set; }

		// Token: 0x17000146 RID: 326
		// (get) Token: 0x060011B8 RID: 4536 RVA: 0x0006DAC8 File Offset: 0x0006BCC8
		protected EventSystem CurrentEventSystem
		{
			get
			{
				return EventSystem.current;
			}
		}

		// Token: 0x17000147 RID: 327
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x0006DACF File Offset: 0x0006BCCF
		// (set) Token: 0x060011BA RID: 4538 RVA: 0x0006DAD7 File Offset: 0x0006BCD7
		protected GameObject LastCheckedGameObject { get; set; }

		// Token: 0x17000148 RID: 328
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x0006DAE0 File Offset: 0x0006BCE0
		protected GameObject CurrentSelectedGameObject
		{
			get
			{
				return EventSystem.current.currentSelectedGameObject;
			}
		}

		// Token: 0x17000149 RID: 329
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x0006DAEC File Offset: 0x0006BCEC
		// (set) Token: 0x060011BD RID: 4541 RVA: 0x0006DAF4 File Offset: 0x0006BCF4
		protected RectTransform CurrentTargetRectTransform { get; set; }

		// Token: 0x1700014A RID: 330
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x0006DAFD File Offset: 0x0006BCFD
		// (set) Token: 0x060011BF RID: 4543 RVA: 0x0006DB05 File Offset: 0x0006BD05
		protected bool IsManualScrollingAvailable { get; set; }

		// Token: 0x060011C0 RID: 4544 RVA: 0x0006DB0E File Offset: 0x0006BD0E
		protected virtual void Awake()
		{
			this.TargetScrollRect = base.GetComponent<ScrollRect>();
			this.ScrollWindow = this.TargetScrollRect.GetComponent<RectTransform>();
		}

		// Token: 0x060011C1 RID: 4545 RVA: 0x00003022 File Offset: 0x00001222
		protected virtual void Start()
		{
		}

		// Token: 0x060011C2 RID: 4546 RVA: 0x0006DB2D File Offset: 0x0006BD2D
		protected virtual void Update()
		{
			this.UpdateReferences();
			this.CheckIfScrollingShouldBeLocked();
			this.ScrollRectToLevelSelection();
		}

		// Token: 0x060011C3 RID: 4547 RVA: 0x0006DB44 File Offset: 0x0006BD44
		private void UpdateReferences()
		{
			if (this.CurrentSelectedGameObject != this.LastCheckedGameObject)
			{
				this.CurrentTargetRectTransform = ((this.CurrentSelectedGameObject != null) ? this.CurrentSelectedGameObject.GetComponent<RectTransform>() : null);
				if (this.CurrentSelectedGameObject != null && this.CurrentSelectedGameObject.transform.parent == this.LayoutListGroup.transform)
				{
					this.IsManualScrollingAvailable = false;
				}
			}
			this.LastCheckedGameObject = this.CurrentSelectedGameObject;
		}

		// Token: 0x060011C4 RID: 4548 RVA: 0x0006DBCC File Offset: 0x0006BDCC
		private void CheckIfScrollingShouldBeLocked()
		{
			if (!this.CancelScrollOnInput || this.IsManualScrollingAvailable)
			{
				return;
			}
			for (int i = 0; i < this.CancelScrollKeycodes.Count; i++)
			{
				if (Input.GetKeyDown(this.CancelScrollKeycodes[i]))
				{
					this.IsManualScrollingAvailable = true;
					return;
				}
			}
		}

		// Token: 0x060011C5 RID: 4549 RVA: 0x0006DC1C File Offset: 0x0006BE1C
		private void ScrollRectToLevelSelection()
		{
			if (this.TargetScrollRect == null || this.LayoutListGroup == null || this.ScrollWindow == null || this.IsManualScrollingAvailable)
			{
				return;
			}
			RectTransform currentTargetRectTransform = this.CurrentTargetRectTransform;
			if (currentTargetRectTransform == null || currentTargetRectTransform.transform.parent != this.LayoutListGroup.transform)
			{
				return;
			}
			switch (this.ScrollDirection)
			{
			case UIScrollToSelection.ScrollType.VERTICAL:
				this.UpdateVerticalScrollPosition(currentTargetRectTransform);
				return;
			case UIScrollToSelection.ScrollType.HORIZONTAL:
				this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
				return;
			case UIScrollToSelection.ScrollType.BOTH:
				this.UpdateVerticalScrollPosition(currentTargetRectTransform);
				this.UpdateHorizontalScrollPosition(currentTargetRectTransform);
				return;
			default:
				return;
			}
		}

		// Token: 0x060011C6 RID: 4550 RVA: 0x0006DCC8 File Offset: 0x0006BEC8
		private void UpdateVerticalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.y - selection.rect.height * (1f - selection.pivot.y);
			float height = selection.rect.height;
			float height2 = this.ScrollWindow.rect.height;
			float y = this.LayoutListGroup.anchoredPosition.y;
			float scrollOffset = this.GetScrollOffset(position, y, height, height2);
			this.TargetScrollRect.verticalNormalizedPosition += scrollOffset / this.LayoutListGroup.rect.height * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x060011C7 RID: 4551 RVA: 0x0006DD7C File Offset: 0x0006BF7C
		private void UpdateHorizontalScrollPosition(RectTransform selection)
		{
			float position = -selection.anchoredPosition.x - selection.rect.width * (1f - selection.pivot.x);
			float width = selection.rect.width;
			float width2 = this.ScrollWindow.rect.width;
			float listAnchorPosition = -this.LayoutListGroup.anchoredPosition.x;
			float num = -this.GetScrollOffset(position, listAnchorPosition, width, width2);
			this.TargetScrollRect.horizontalNormalizedPosition += num / this.LayoutListGroup.rect.width * Time.unscaledDeltaTime * this.scrollSpeed;
		}

		// Token: 0x060011C8 RID: 4552 RVA: 0x0006DE32 File Offset: 0x0006C032
		private float GetScrollOffset(float position, float listAnchorPosition, float targetLength, float maskLength)
		{
			if (position < listAnchorPosition + targetLength / 2f)
			{
				return listAnchorPosition + maskLength - (position - targetLength);
			}
			if (position + targetLength > listAnchorPosition + maskLength)
			{
				return listAnchorPosition + maskLength - (position + targetLength);
			}
			return 0f;
		}

		// Token: 0x04001029 RID: 4137
		[Header("[ Settings ]")]
		[SerializeField]
		private UIScrollToSelection.ScrollType scrollDirection;

		// Token: 0x0400102A RID: 4138
		[SerializeField]
		private float scrollSpeed = 10f;

		// Token: 0x0400102B RID: 4139
		[Header("[ Input ]")]
		[SerializeField]
		private bool cancelScrollOnInput;

		// Token: 0x0400102C RID: 4140
		[SerializeField]
		private List<KeyCode> cancelScrollKeycodes = new List<KeyCode>();

		// Token: 0x0200086B RID: 2155
		public enum ScrollType
		{
			// Token: 0x04002EFB RID: 12027
			VERTICAL,
			// Token: 0x04002EFC RID: 12028
			HORIZONTAL,
			// Token: 0x04002EFD RID: 12029
			BOTH
		}
	}
}
