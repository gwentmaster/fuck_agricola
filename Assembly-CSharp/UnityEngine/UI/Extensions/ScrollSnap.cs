using System;
using System.Collections;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A1 RID: 417
	[ExecuteInEditMode]
	[RequireComponent(typeof(ScrollRect))]
	[AddComponentMenu("UI/Extensions/Scroll Snap")]
	public class ScrollSnap : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IEndDragHandler, IDragHandler, IScrollSnap
	{
		// Token: 0x1400000A RID: 10
		// (add) Token: 0x06000FEF RID: 4079 RVA: 0x00064CB4 File Offset: 0x00062EB4
		// (remove) Token: 0x06000FF0 RID: 4080 RVA: 0x00064CEC File Offset: 0x00062EEC
		public event ScrollSnap.PageSnapChange onPageChange;

		// Token: 0x06000FF1 RID: 4081 RVA: 0x00064D24 File Offset: 0x00062F24
		private void Start()
		{
			this._lerp = false;
			this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			this._scrollRectTransform = base.gameObject.GetComponent<RectTransform>();
			this._listContainerTransform = this._scroll_rect.content;
			this._listContainerRectTransform = this._listContainerTransform.GetComponent<RectTransform>();
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			this.PageChanged(this.CurrentPage());
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.NextScreen();
				});
			}
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().onClick.AddListener(delegate()
				{
					this.PreviousScreen();
				});
			}
			if (this._scroll_rect.horizontalScrollbar != null && this._scroll_rect.horizontal)
			{
				this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>().ss = this;
			}
			if (this._scroll_rect.verticalScrollbar != null && this._scroll_rect.vertical)
			{
				this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>().ss = this;
			}
		}

		// Token: 0x06000FF2 RID: 4082 RVA: 0x00064E68 File Offset: 0x00063068
		public void UpdateListItemsSize()
		{
			float num;
			float num2;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._scrollRectTransform.rect.width / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.width / (float)this._itemsCount;
			}
			else
			{
				num = this._scrollRectTransform.rect.height / (float)this.ItemsVisibleAtOnce;
				num2 = this._listContainerRectTransform.rect.height / (float)this._itemsCount;
			}
			this._itemSize = num;
			if (this.LinkScrolrectScrollSensitivity)
			{
				this._scroll_rect.scrollSensitivity = this._itemSize;
			}
			if (this.AutoLayoutItems && num2 != num && this._itemsCount > 0)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					using (IEnumerator enumerator = this._listContainerTransform.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							object obj = enumerator.Current;
							GameObject gameObject = ((Transform)obj).gameObject;
							if (gameObject.activeInHierarchy)
							{
								LayoutElement layoutElement = gameObject.GetComponent<LayoutElement>();
								if (layoutElement == null)
								{
									layoutElement = gameObject.AddComponent<LayoutElement>();
								}
								layoutElement.minWidth = this._itemSize;
							}
						}
						return;
					}
				}
				foreach (object obj2 in this._listContainerTransform)
				{
					GameObject gameObject2 = ((Transform)obj2).gameObject;
					if (gameObject2.activeInHierarchy)
					{
						LayoutElement layoutElement2 = gameObject2.GetComponent<LayoutElement>();
						if (layoutElement2 == null)
						{
							layoutElement2 = gameObject2.AddComponent<LayoutElement>();
						}
						layoutElement2.minHeight = this._itemSize;
					}
				}
			}
		}

		// Token: 0x06000FF3 RID: 4083 RVA: 0x00065040 File Offset: 0x00063240
		public void UpdateListItemPositions()
		{
			if (!this._listContainerRectTransform.rect.size.Equals(this._listContainerCachedSize))
			{
				int num = 0;
				using (IEnumerator enumerator = this._listContainerTransform.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (((Transform)enumerator.Current).gameObject.activeInHierarchy)
						{
							num++;
						}
					}
				}
				this._itemsCount = 0;
				Array.Resize<Vector3>(ref this._pageAnchorPositions, num);
				if (num > 0)
				{
					this._pages = Mathf.Max(num - this.ItemsVisibleAtOnce + 1, 1);
					if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
					{
						this._scroll_rect.horizontalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.x;
						this._scroll_rect.horizontalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.x;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int i = 0; i < this._pages; i++)
						{
							this._pageAnchorPositions[i] = new Vector3(this._listContainerMaxPosition - this._itemSize * (float)i, this._listContainerTransform.localPosition.y, this._listContainerTransform.localPosition.z);
						}
					}
					else
					{
						this._scroll_rect.verticalNormalizedPosition = 1f;
						this._listContainerMinPosition = this._listContainerTransform.localPosition.y;
						this._scroll_rect.verticalNormalizedPosition = 0f;
						this._listContainerMaxPosition = this._listContainerTransform.localPosition.y;
						this._listContainerSize = this._listContainerMaxPosition - this._listContainerMinPosition;
						for (int j = 0; j < this._pages; j++)
						{
							this._pageAnchorPositions[j] = new Vector3(this._listContainerTransform.localPosition.x, this._listContainerMinPosition + this._itemSize * (float)j, this._listContainerTransform.localPosition.z);
						}
					}
					this.UpdateScrollbar(this.LinkScrolbarSteps);
					this._startingPage = Mathf.Min(this._startingPage, this._pages);
					this.ResetPage();
				}
				if (this._itemsCount != num)
				{
					this.PageChanged(this.CurrentPage());
				}
				this._itemsCount = num;
				this._listContainerCachedSize.Set(this._listContainerRectTransform.rect.size.x, this._listContainerRectTransform.rect.size.y);
			}
		}

		// Token: 0x06000FF4 RID: 4084 RVA: 0x000652FC File Offset: 0x000634FC
		public void ResetPage()
		{
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				this._scroll_rect.horizontalNormalizedPosition = ((this._pages > 1) ? ((float)this._startingPage / (float)(this._pages - 1)) : 0f);
				return;
			}
			this._scroll_rect.verticalNormalizedPosition = ((this._pages > 1) ? ((float)(this._pages - this._startingPage - 1) / (float)(this._pages - 1)) : 0f);
		}

		// Token: 0x06000FF5 RID: 4085 RVA: 0x00065374 File Offset: 0x00063574
		private void UpdateScrollbar(bool linkSteps)
		{
			if (linkSteps)
			{
				if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
				{
					if (this._scroll_rect.horizontalScrollbar != null)
					{
						this._scroll_rect.horizontalScrollbar.numberOfSteps = this._pages;
						return;
					}
				}
				else if (this._scroll_rect.verticalScrollbar != null)
				{
					this._scroll_rect.verticalScrollbar.numberOfSteps = this._pages;
					return;
				}
			}
			else if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				if (this._scroll_rect.horizontalScrollbar != null)
				{
					this._scroll_rect.horizontalScrollbar.numberOfSteps = 0;
					return;
				}
			}
			else if (this._scroll_rect.verticalScrollbar != null)
			{
				this._scroll_rect.verticalScrollbar.numberOfSteps = 0;
			}
		}

		// Token: 0x06000FF6 RID: 4086 RVA: 0x00065434 File Offset: 0x00063634
		private void LateUpdate()
		{
			this.UpdateListItemsSize();
			this.UpdateListItemPositions();
			if (this._lerp)
			{
				this.UpdateScrollbar(false);
				this._listContainerTransform.localPosition = Vector3.Lerp(this._listContainerTransform.localPosition, this._lerpTarget, 7.5f * Time.deltaTime);
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 0.001f)
				{
					this._listContainerTransform.localPosition = this._lerpTarget;
					this._lerp = false;
					this.UpdateScrollbar(this.LinkScrolbarSteps);
				}
				if (Vector3.Distance(this._listContainerTransform.localPosition, this._lerpTarget) < 10f)
				{
					this.PageChanged(this.CurrentPage());
				}
			}
			if (this._fastSwipeTimer)
			{
				this._fastSwipeCounter++;
			}
		}

		// Token: 0x06000FF7 RID: 4087 RVA: 0x0006550C File Offset: 0x0006370C
		public void NextScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() < this._pages - 1)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() + 1];
				this.PageChanged(this.CurrentPage() + 1);
			}
		}

		// Token: 0x06000FF8 RID: 4088 RVA: 0x0006555D File Offset: 0x0006375D
		public void PreviousScreen()
		{
			this.UpdateListItemPositions();
			if (this.CurrentPage() > 0)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage() - 1];
				this.PageChanged(this.CurrentPage() - 1);
			}
		}

		// Token: 0x06000FF9 RID: 4089 RVA: 0x0006559C File Offset: 0x0006379C
		private void NextScreenCommand()
		{
			if (this._pageOnDragStart < this._pages - 1)
			{
				int num = Mathf.Min(this._pages - 1, this._pageOnDragStart + this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x06000FFA RID: 4090 RVA: 0x000655F4 File Offset: 0x000637F4
		private void PrevScreenCommand()
		{
			if (this._pageOnDragStart > 0)
			{
				int num = Mathf.Max(0, this._pageOnDragStart - this.ItemsVisibleAtOnce);
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[num];
				this.PageChanged(num);
			}
		}

		// Token: 0x06000FFB RID: 4091 RVA: 0x00065640 File Offset: 0x00063840
		public int CurrentPage()
		{
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._listContainerMaxPosition - this._listContainerTransform.localPosition.x;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			else
			{
				num = this._listContainerTransform.localPosition.y - this._listContainerMinPosition;
				num = Mathf.Clamp(num, 0f, this._listContainerSize);
			}
			return Mathf.Clamp(Mathf.RoundToInt(num / this._itemSize), 0, this._pages);
		}

		// Token: 0x06000FFC RID: 4092 RVA: 0x000656C4 File Offset: 0x000638C4
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x06000FFD RID: 4093 RVA: 0x000656CD File Offset: 0x000638CD
		public void ChangePage(int page)
		{
			if (0 <= page && page < this._pages)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[page];
				this.PageChanged(page);
			}
		}

		// Token: 0x06000FFE RID: 4094 RVA: 0x000656FC File Offset: 0x000638FC
		private void PageChanged(int currentPage)
		{
			this._startingPage = currentPage;
			if (this.NextButton)
			{
				this.NextButton.interactable = (currentPage < this._pages - 1);
			}
			if (this.PrevButton)
			{
				this.PrevButton.interactable = (currentPage > 0);
			}
			if (this.onPageChange != null)
			{
				this.onPageChange(currentPage);
			}
		}

		// Token: 0x06000FFF RID: 4095 RVA: 0x00065763 File Offset: 0x00063963
		public void OnBeginDrag(PointerEventData eventData)
		{
			this.UpdateScrollbar(false);
			this._fastSwipeCounter = 0;
			this._fastSwipeTimer = true;
			this._positionOnDragStart = eventData.position;
			this._pageOnDragStart = this.CurrentPage();
		}

		// Token: 0x06001000 RID: 4096 RVA: 0x00065798 File Offset: 0x00063998
		public void OnEndDrag(PointerEventData eventData)
		{
			this._startDrag = true;
			float num;
			if (this.direction == ScrollSnap.ScrollDirection.Horizontal)
			{
				num = this._positionOnDragStart.x - eventData.position.x;
			}
			else
			{
				num = -this._positionOnDragStart.y + eventData.position.y;
			}
			if (!this.UseFastSwipe)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				return;
			}
			this.fastSwipe = false;
			this._fastSwipeTimer = false;
			if (this._fastSwipeCounter <= this._fastSwipeTarget && Math.Abs(num) > (float)this.FastSwipeThreshold)
			{
				this.fastSwipe = true;
			}
			if (!this.fastSwipe)
			{
				this._lerp = true;
				this._lerpTarget = this._pageAnchorPositions[this.CurrentPage()];
				return;
			}
			if (num > 0f)
			{
				this.NextScreenCommand();
				return;
			}
			this.PrevScreenCommand();
		}

		// Token: 0x06001001 RID: 4097 RVA: 0x00065882 File Offset: 0x00063A82
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
			if (this._startDrag)
			{
				this.OnBeginDrag(eventData);
				this._startDrag = false;
			}
		}

		// Token: 0x06001002 RID: 4098 RVA: 0x00003022 File Offset: 0x00001222
		public void StartScreenChange()
		{
		}

		// Token: 0x04000F0D RID: 3853
		private ScrollRect _scroll_rect;

		// Token: 0x04000F0E RID: 3854
		private RectTransform _scrollRectTransform;

		// Token: 0x04000F0F RID: 3855
		private Transform _listContainerTransform;

		// Token: 0x04000F10 RID: 3856
		private int _pages;

		// Token: 0x04000F11 RID: 3857
		private int _startingPage;

		// Token: 0x04000F12 RID: 3858
		private Vector3[] _pageAnchorPositions;

		// Token: 0x04000F13 RID: 3859
		private Vector3 _lerpTarget;

		// Token: 0x04000F14 RID: 3860
		private bool _lerp;

		// Token: 0x04000F15 RID: 3861
		private float _listContainerMinPosition;

		// Token: 0x04000F16 RID: 3862
		private float _listContainerMaxPosition;

		// Token: 0x04000F17 RID: 3863
		private float _listContainerSize;

		// Token: 0x04000F18 RID: 3864
		private RectTransform _listContainerRectTransform;

		// Token: 0x04000F19 RID: 3865
		private Vector2 _listContainerCachedSize;

		// Token: 0x04000F1A RID: 3866
		private float _itemSize;

		// Token: 0x04000F1B RID: 3867
		private int _itemsCount;

		// Token: 0x04000F1C RID: 3868
		private bool _startDrag = true;

		// Token: 0x04000F1D RID: 3869
		private Vector3 _positionOnDragStart;

		// Token: 0x04000F1E RID: 3870
		private int _pageOnDragStart;

		// Token: 0x04000F1F RID: 3871
		private bool _fastSwipeTimer;

		// Token: 0x04000F20 RID: 3872
		private int _fastSwipeCounter;

		// Token: 0x04000F21 RID: 3873
		private int _fastSwipeTarget = 10;

		// Token: 0x04000F22 RID: 3874
		[Tooltip("Button to go to the next page. (optional)")]
		public Button NextButton;

		// Token: 0x04000F23 RID: 3875
		[Tooltip("Button to go to the previous page. (optional)")]
		public Button PrevButton;

		// Token: 0x04000F24 RID: 3876
		[Tooltip("Number of items visible in one page of scroll frame.")]
		[Range(1f, 100f)]
		public int ItemsVisibleAtOnce = 1;

		// Token: 0x04000F25 RID: 3877
		[Tooltip("Sets minimum width of list items to 1/itemsVisibleAtOnce.")]
		public bool AutoLayoutItems = true;

		// Token: 0x04000F26 RID: 3878
		[Tooltip("If you wish to update scrollbar numberOfSteps to number of active children on list.")]
		public bool LinkScrolbarSteps;

		// Token: 0x04000F27 RID: 3879
		[Tooltip("If you wish to update scrollrect sensitivity to size of list element.")]
		public bool LinkScrolrectScrollSensitivity;

		// Token: 0x04000F28 RID: 3880
		public bool UseFastSwipe = true;

		// Token: 0x04000F29 RID: 3881
		public int FastSwipeThreshold = 100;

		// Token: 0x04000F2B RID: 3883
		public ScrollSnap.ScrollDirection direction;

		// Token: 0x04000F2C RID: 3884
		private bool fastSwipe;

		// Token: 0x02000857 RID: 2135
		public enum ScrollDirection
		{
			// Token: 0x04002ECE RID: 11982
			Horizontal,
			// Token: 0x04002ECF RID: 11983
			Vertical
		}

		// Token: 0x02000858 RID: 2136
		// (Invoke) Token: 0x060044B8 RID: 17592
		public delegate void PageSnapChange(int page);
	}
}
