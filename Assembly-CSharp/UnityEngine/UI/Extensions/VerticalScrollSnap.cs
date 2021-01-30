using System;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A7 RID: 423
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("Layout/Extensions/Vertical Scroll Snap")]
	public class VerticalScrollSnap : ScrollSnapBase, IEndDragHandler, IEventSystemHandler
	{
		// Token: 0x06001060 RID: 4192 RVA: 0x000670A8 File Offset: 0x000652A8
		private void Start()
		{
			this._isVertical = true;
			this._childAnchorPoint = new Vector2(0.5f, 0f);
			this._currentPage = this.StartingScreen;
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			this.UpdateLayout();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x000670FC File Offset: 0x000652FC
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
			if (!this._pointerDown && ((double)this._scroll_rect.velocity.y > 0.01 || (double)this._scroll_rect.velocity.y < -0.01) && this.IsRectMovingSlowerThanThreshold(0f))
			{
				base.ScrollToClosestElement();
			}
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0006722C File Offset: 0x0006542C
		private bool IsRectMovingSlowerThanThreshold(float startingSpeed)
		{
			return (this._scroll_rect.velocity.y > startingSpeed && this._scroll_rect.velocity.y < (float)this.SwipeVelocityThreshold) || (this._scroll_rect.velocity.y < startingSpeed && this._scroll_rect.velocity.y > (float)(-(float)this.SwipeVelocityThreshold));
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x00067298 File Offset: 0x00065498
		public void DistributePages()
		{
			this._screens = this._screensContainer.childCount;
			this._scroll_rect.verticalNormalizedPosition = 0f;
			float num = 0f;
			Rect rect = base.gameObject.GetComponent<RectTransform>().rect;
			float num2 = 0f;
			float num3 = this._childSize = (float)((int)rect.height) * ((this.PageStep == 0f) ? 3f : this.PageStep);
			for (int i = 0; i < this._screensContainer.transform.childCount; i++)
			{
				RectTransform component = this._screensContainer.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
				num2 = num + (float)i * num3;
				component.sizeDelta = new Vector2(rect.width, rect.height);
				component.anchoredPosition = new Vector2(0f, num2);
				component.anchorMin = (component.anchorMax = (component.pivot = this._childAnchorPoint));
			}
			float y = num2 + num * -1f;
			this._screensContainer.GetComponent<RectTransform>().offsetMax = new Vector2(0f, y);
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x000673D1 File Offset: 0x000655D1
		public void AddChild(GameObject GO)
		{
			this.AddChild(GO, false);
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x000673DC File Offset: 0x000655DC
		public void AddChild(GameObject GO, bool WorldPositionStays)
		{
			this._scroll_rect.verticalNormalizedPosition = 0f;
			GO.transform.SetParent(this._screensContainer, WorldPositionStays);
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
			this.SetScrollContainerPosition();
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x00067430 File Offset: 0x00065630
		public void RemoveChild(int index, out GameObject ChildRemoved)
		{
			this.RemoveChild(index, false, out ChildRemoved);
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0006743C File Offset: 0x0006563C
		public void RemoveChild(int index, bool WorldPositionStays, out GameObject ChildRemoved)
		{
			ChildRemoved = null;
			if (index < 0 || index > this._screensContainer.childCount)
			{
				return;
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
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

		// Token: 0x06001068 RID: 4200 RVA: 0x000674D4 File Offset: 0x000656D4
		public void RemoveAllChildren(out GameObject[] ChildrenRemoved)
		{
			this.RemoveAllChildren(false, out ChildrenRemoved);
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x000674E0 File Offset: 0x000656E0
		public void RemoveAllChildren(bool WorldPositionStays, out GameObject[] ChildrenRemoved)
		{
			int childCount = this._screensContainer.childCount;
			ChildrenRemoved = new GameObject[childCount];
			for (int i = childCount - 1; i >= 0; i--)
			{
				ChildrenRemoved[i] = this._screensContainer.GetChild(i).gameObject;
				ChildrenRemoved[i].transform.SetParent(null, WorldPositionStays);
			}
			this._scroll_rect.verticalNormalizedPosition = 0f;
			base.CurrentPage = 0;
			base.InitialiseChildObjectsFromScene();
			this.DistributePages();
			if (this.MaskArea)
			{
				base.UpdateVisible();
			}
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0006756A File Offset: 0x0006576A
		private void SetScrollContainerPosition()
		{
			this._scrollStartPosition = this._screensContainer.localPosition.y;
			this._scroll_rect.verticalNormalizedPosition = (float)this._currentPage / (float)(this._screens - 1);
			base.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x0600106B RID: 4203 RVA: 0x000675AA File Offset: 0x000657AA
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

		// Token: 0x0600106C RID: 4204 RVA: 0x000675DE File Offset: 0x000657DE
		private void OnRectTransformDimensionsChange()
		{
			if (this._childAnchorPoint != Vector2.zero)
			{
				this.UpdateLayout();
			}
		}

		// Token: 0x0600106D RID: 4205 RVA: 0x000675F8 File Offset: 0x000657F8
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

		// Token: 0x0600106E RID: 4206 RVA: 0x00067650 File Offset: 0x00065850
		public void OnEndDrag(PointerEventData eventData)
		{
			this._pointerDown = false;
			if (this._scroll_rect.vertical)
			{
				float num = Vector3.Distance(this._startPosition, this._screensContainer.localPosition);
				if (this.UseFastSwipe && num < this.panelDimensions.height + (float)this.FastSwipeThreshold && num >= 1f)
				{
					this._scroll_rect.velocity = Vector3.zero;
					if (this._startPosition.y - this._screensContainer.localPosition.y > 0f)
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
