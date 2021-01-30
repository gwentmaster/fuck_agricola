using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x0200019D RID: 413
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Horizontal Scroll Snap")]
	public class HorizontalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06000FC6 RID: 4038 RVA: 0x00063F6C File Offset: 0x0006216C
		private void Start()
		{
			this._isVertical = false;
			this._childAnchorPoint = new Vector2(0f, 0.5f);
			this._currentPage = this.StartingScreen;
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			this.UpdateLayout();
		}

		// Token: 0x06000FC7 RID: 4039 RVA: 0x00063FC0 File Offset: 0x000621C0
		private void Update()
		{
			if (!this._lerp && this._scroll_rect.velocity == Vector2.zero)
			{
				if (!this._settled && !this._pointerDown && !base.IsRectSettledOnaPage(this._screensContainer.localPosition))
				{
					base.ScrollToClosestElement();
				}
				return;
			}
			if (this._lerp)
			{
				this._screensContainer.localPosition = Vector3.Lerp(this._screensContainer.localPosition, this._lerp_target, this.transitionSpeed * Time.deltaTime);
				if (Vector3.Distance(this._screensContainer.localPosition, this._lerp_target) < 0.1f)
				{
					this._screensContainer.localPosition = this._lerp_target;
					this._lerp = false;
					base.EndScreenChange();
				}
			}
			base.CurrentPage = base.GetPageforPosition(this._screensContainer.localPosition);
			if (!this._pointerDown && ((double)this._scroll_rect.velocity.x > 0.01 || (double)this._scroll_rect.velocity.x < 0.01) && this.IsRectMovingSlowerThanThreshold(0f))
			{
				base.ScrollToClosestElement();
			}
		}

		// Token: 0x06000FC8 RID: 4040 RVA: 0x000640F0 File Offset: 0x000622F0
		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return (this._scroll_rect.velocity.x > startingSpeed && this._scroll_rect.velocity.x < (float)this.SwipeVelocityThreshold) || (this._scroll_rect.velocity.x < startingSpeed && this._scroll_rect.velocity.x > (float)(-(float)this.SwipeVelocityThreshold));
		}

		// Token: 0x06000FC9 RID: 4041 RVA: 0x0006415C File Offset: 0x0006235C
		private void DistributePages()
		{
			this._screens = this._screensContainer.childCount;
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			int num = 0;
			float num2 = 0f;
			float num3 = this._childSize = (float)((int)this.panelDimensions.width) * ((this.PageStep == 0f) ? 3f : this.PageStep);
			for (int i = 0; i < this._screensContainer.transform.childCount; i++)
			{
				RectTransform component = this._screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num2 = (float)(num + (int)((float)i * num3));
				component.sizeDelta = new Vector2(this.panelDimensions.width, this.panelDimensions.height);
				component.anchoredPosition = new Vector2(num2, 0f);
				component.anchorMin = (component.anchorMax = (component.pivot = this._childAnchorPoint));
			}
			float x = num2 + (float)(num * -1);
			this._screensContainer.offsetMax = new Vector2(x, 0f);
		}

		// Token: 0x06000FCA RID: 4042 RVA: 0x00064284 File Offset: 0x00062484
		public void AddChild(GameObject GO)
		{
			this.AddChild(GO, false);
		}

		// Token: 0x06000FCB RID: 4043 RVA: 0x00064290 File Offset: 0x00062490
		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			GO.transform.SetParent(this._screensContainer, WorldPositionStays);
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06000FCC RID: 4044 RVA: 0x000642E4 File Offset: 0x000624E4
		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			this.RemoveChild(index, false, out ChildRemoved);
		}

		// Token: 0x06000FCD RID: 4045 RVA: 0x000642F0 File Offset: 0x000624F0
		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
			if (index < 0 || index > this._screensContainer.childCount)
			{
				return;
			}
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			Transform child = this._screensContainer.transform.GetChild(index);
			child.SetParent(null, WorldPositionStays);
			ChildRemoved = child.gameObject;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this._currentPage > this._screens - 1)
			{
				base.CurrentPage = this._screens - 1;
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06000FCE RID: 4046 RVA: 0x00064388 File Offset: 0x00062588
		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			this.RemoveAllChildren(false, out ChildrenRemoved);
		}

		// Token: 0x06000FCF RID: 4047 RVA: 0x00064394 File Offset: 0x00062594
		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			int childCount = this._screensContainer.childCount;
			ChildrenRemoved = new GameObject[childCount];
			for (int i = childCount - 1; i >= 0; i--)
			{
				ChildrenRemoved[i] = this._screensContainer.GetChild(i).gameObject;
				ChildrenRemoved[i].transform.SetParent(null, WorldPositionStays);
			}
			this._scroll_rect.horizontalNormalizedPosition = 0f;
			base.CurrentPage = 0;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
		}

		// Token: 0x06000FD0 RID: 4048 RVA: 0x0006441E File Offset: 0x0006261E
		private void SetScrollContainerPosition()
		{
			this._scrollStartPosition = this._screensContainer.localPosition.x;
			this._scroll_rect.horizontalNormalizedPosition = (float)this._currentPage / (float)(this._screens - 1);
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06000FD1 RID: 4049 RVA: 0x0006445E File Offset: 0x0006265E
		public void UpdateLayout()
		{
			this._lerp = false;
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x06000FD2 RID: 4050 RVA: 0x00064492 File Offset: 0x00062692
		private void OnRectTransformDimensionsChange()
		{
			if (this._childAnchorPoint != Vector2.zero)
			{
				this.UpdateLayout();
			}
		}

		// Token: 0x06000FD3 RID: 4051 RVA: 0x000644AC File Offset: 0x000626AC
		private void OnEnable()
		{
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			if (this.JumpOnEnable || !this.RestartOnEnable)
			{
				this.SetScrollContainerPosition();
			}
			if (this.RestartOnEnable)
			{
				base.GoToScreen(this.StartingScreen);
			}
		}

		// Token: 0x06000FD4 RID: 4052 RVA: 0x00064504 File Offset: 0x00062704
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			if (this._scroll_rect.horizontal)
			{
				float num = Vector3.Distance(this._startPosition, this._screensContainer.localPosition);
				if (this.UseFastSwipe && num < this.panelDimensions.width && num >= (float)this.FastSwipeThreshold)
				{
					this._scroll_rect.velocity = Vector3.zero;
					if (this._startPosition.x - this._screensContainer.localPosition.x > 0f)
					{
						base.NextScreen();
						return;
					}
					base.PreviousScreen();
				}
			}
		}
	}
}
