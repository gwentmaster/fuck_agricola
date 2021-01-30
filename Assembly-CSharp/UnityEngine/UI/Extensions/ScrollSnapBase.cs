using System;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x020001A2 RID: 418
	public class ScrollSnapBase : MonoBehaviour, IBeginDragHandler, IEventSystemHandler, IDragHandler, IScrollSnap
	{
		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x06001006 RID: 4102 RVA: 0x000658E5 File Offset: 0x00063AE5
		// (set) Token: 0x06001007 RID: 4103 RVA: 0x000658F0 File Offset: 0x00063AF0
		public int CurrentPage
		{
			get
			{
				return this._currentPage;
			}
			internal set
			{
				if ((value != this._currentPage && value >= 0 && value < this._screensContainer.childCount) || (value == 0 && this._screensContainer.childCount == 0))
				{
					this._previousPage = this._currentPage;
					this._currentPage = value;
					if (this.MaskArea)
					{
						this.UpdateVisible();
					}
					if (!this._lerp)
					{
						this.ScreenChange();
					}
					this.OnCurrentScreenChange(this._currentPage);
				}
			}
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06001008 RID: 4104 RVA: 0x00065968 File Offset: 0x00063B68
		// (set) Token: 0x06001009 RID: 4105 RVA: 0x00065970 File Offset: 0x00063B70
		public ScrollSnapBase.SelectionChangeStartEvent OnSelectionChangeStartEvent
		{
			get
			{
				return this.m_OnSelectionChangeStartEvent;
			}
			set
			{
				this.m_OnSelectionChangeStartEvent = value;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x0600100A RID: 4106 RVA: 0x00065979 File Offset: 0x00063B79
		// (set) Token: 0x0600100B RID: 4107 RVA: 0x00065981 File Offset: 0x00063B81
		public ScrollSnapBase.SelectionPageChangedEvent OnSelectionPageChangedEvent
		{
			get
			{
				return this.m_OnSelectionPageChangedEvent;
			}
			set
			{
				this.m_OnSelectionPageChangedEvent = value;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x0600100C RID: 4108 RVA: 0x0006598A File Offset: 0x00063B8A
		// (set) Token: 0x0600100D RID: 4109 RVA: 0x00065992 File Offset: 0x00063B92
		public ScrollSnapBase.SelectionChangeEndEvent OnSelectionChangeEndEvent
		{
			get
			{
				return this.m_OnSelectionChangeEndEvent;
			}
			set
			{
				this.m_OnSelectionChangeEndEvent = value;
			}
		}

		// Token: 0x0600100E RID: 4110 RVA: 0x0006599C File Offset: 0x00063B9C
		private void Awake()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.gameObject.GetComponent<ScrollRect>();
			}
			if (this._scroll_rect.horizontalScrollbar && this._scroll_rect.horizontal)
			{
				this._scroll_rect.horizontalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>().ss = this;
			}
			if (this._scroll_rect.verticalScrollbar && this._scroll_rect.vertical)
			{
				this._scroll_rect.verticalScrollbar.gameObject.AddComponent<ScrollSnapScrollbarHelper>().ss = this;
			}
			this.panelDimensions = base.gameObject.GetComponent<RectTransform>().rect;
			if (this.StartingScreen < 0)
			{
				this.StartingScreen = 0;
			}
			this._screensContainer = this._scroll_rect.content;
			this.InitialiseChildObjects();
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
		}

		// Token: 0x0600100F RID: 4111 RVA: 0x00065AD5 File Offset: 0x00063CD5
		internal void InitialiseChildObjects()
		{
			if (this.ChildObjects == null || this.ChildObjects.Length == 0)
			{
				this.InitialiseChildObjectsFromScene();
				return;
			}
			if (this._screensContainer.transform.childCount > 0)
			{
				Debug.LogError("ScrollRect Content has children, this is not supported when using managed Child Objects\n Either remove the ScrollRect Content children or clear the ChildObjects array");
				return;
			}
			this.InitialiseChildObjectsFromArray();
		}

		// Token: 0x06001010 RID: 4112 RVA: 0x00065B14 File Offset: 0x00063D14
		internal void InitialiseChildObjectsFromScene()
		{
			int childCount = this._screensContainer.childCount;
			this.ChildObjects = new GameObject[childCount];
			for (int i = 0; i < childCount; i++)
			{
				this.ChildObjects[i] = this._screensContainer.transform.GetChild(i).gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x06001011 RID: 4113 RVA: 0x00065B90 File Offset: 0x00063D90
		internal void InitialiseChildObjectsFromArray()
		{
			int num = this.ChildObjects.Length;
			for (int i = 0; i < num; i++)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.ChildObjects[i]);
				if (this.UseParentTransform)
				{
					RectTransform component = gameObject.GetComponent<RectTransform>();
					component.rotation = this._screensContainer.rotation;
					component.localScale = this._screensContainer.localScale;
					component.position = this._screensContainer.position;
				}
				gameObject.transform.SetParent(this._screensContainer.transform);
				this.ChildObjects[i] = gameObject;
				if (this.MaskArea && this.ChildObjects[i].activeSelf)
				{
					this.ChildObjects[i].SetActive(false);
				}
			}
		}

		// Token: 0x06001012 RID: 4114 RVA: 0x00065C50 File Offset: 0x00063E50
		internal void UpdateVisible()
		{
			if (!this.MaskArea || this.ChildObjects == null || this.ChildObjects.Length < 1 || this._screensContainer.childCount < 1)
			{
				return;
			}
			this._maskSize = (this._isVertical ? this.MaskArea.rect.height : this.MaskArea.rect.width);
			this._halfNoVisibleItems = (int)Math.Round((double)(this._maskSize / (this._childSize * this.MaskBuffer)), MidpointRounding.AwayFromZero) / 2;
			this._bottomItem = (this._topItem = 0);
			for (int i = this._halfNoVisibleItems + 1; i > 0; i--)
			{
				this._bottomItem = ((this._currentPage - i < 0) ? 0 : i);
				if (this._bottomItem > 0)
				{
					break;
				}
			}
			for (int j = this._halfNoVisibleItems + 1; j > 0; j--)
			{
				this._topItem = ((this._screensContainer.childCount - this._currentPage - j < 0) ? 0 : j);
				if (this._topItem > 0)
				{
					break;
				}
			}
			for (int k = this.CurrentPage - this._bottomItem; k < this.CurrentPage + this._topItem; k++)
			{
				try
				{
					this.ChildObjects[k].SetActive(true);
				}
				catch
				{
					Debug.Log("Failed to setactive child [" + k + "]");
				}
			}
			if (this._currentPage > this._halfNoVisibleItems)
			{
				this.ChildObjects[this.CurrentPage - this._bottomItem].SetActive(false);
			}
			if (this._screensContainer.childCount - this._currentPage > this._topItem)
			{
				this.ChildObjects[this.CurrentPage + this._topItem].SetActive(false);
			}
		}

		// Token: 0x06001013 RID: 4115 RVA: 0x00065E28 File Offset: 0x00064028
		public void NextScreen()
		{
			if (this._currentPage < this._screens - 1)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage + 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x06001014 RID: 4116 RVA: 0x00065E80 File Offset: 0x00064080
		public void PreviousScreen()
		{
			if (this._currentPage > 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = this._currentPage - 1;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x06001015 RID: 4117 RVA: 0x00065ED4 File Offset: 0x000640D4
		public void GoToScreen(int screenIndex)
		{
			if (screenIndex <= this._screens - 1 && screenIndex >= 0)
			{
				if (!this._lerp)
				{
					this.StartScreenChange();
				}
				this._lerp = true;
				this.CurrentPage = screenIndex;
				this.GetPositionforPage(this._currentPage, ref this._lerp_target);
				this.ScreenChange();
			}
		}

		// Token: 0x06001016 RID: 4118 RVA: 0x00065F24 File Offset: 0x00064124
		internal int GetPageforPosition(Vector3 pos)
		{
			if (!this._isVertical)
			{
				return (int)Math.Round((double)((this._scrollStartPosition - pos.x) / this._childSize));
			}
			return (int)Math.Round((double)((this._scrollStartPosition - pos.y) / this._childSize));
		}

		// Token: 0x06001017 RID: 4119 RVA: 0x00065F70 File Offset: 0x00064170
		internal bool IsRectSettledOnaPage(Vector3 pos)
		{
			if (!this._isVertical)
			{
				return -((pos.x - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.x - this._scrollStartPosition) / this._childSize))));
			}
			return -((pos.y - this._scrollStartPosition) / this._childSize) == (float)(-(float)((int)Math.Round((double)((pos.y - this._scrollStartPosition) / this._childSize))));
		}

		// Token: 0x06001018 RID: 4120 RVA: 0x00065FF0 File Offset: 0x000641F0
		internal void GetPositionforPage(int page, ref Vector3 target)
		{
			this._childPos = -this._childSize * (float)page;
			if (this._isVertical)
			{
				target.y = this._childPos + this._scrollStartPosition;
				return;
			}
			target.x = this._childPos + this._scrollStartPosition;
		}

		// Token: 0x06001019 RID: 4121 RVA: 0x0006603C File Offset: 0x0006423C
		internal void ScrollToClosestElement()
		{
			this._lerp = true;
			this.CurrentPage = this.GetPageforPosition(this._screensContainer.localPosition);
			this.GetPositionforPage(this._currentPage, ref this._lerp_target);
			this.OnCurrentScreenChange(this._currentPage);
		}

		// Token: 0x0600101A RID: 4122 RVA: 0x0006607A File Offset: 0x0006427A
		internal void OnCurrentScreenChange(int currentScreen)
		{
			this.ChangeBulletsInfo(currentScreen);
			this.ToggleNavigationButtons(currentScreen);
		}

		// Token: 0x0600101B RID: 4123 RVA: 0x0006608C File Offset: 0x0006428C
		private void ChangeBulletsInfo(int targetScreen)
		{
			if (this.Pagination)
			{
				for (int i = 0; i < this.Pagination.transform.childCount; i++)
				{
					this.Pagination.transform.GetChild(i).GetComponent<Toggle>().isOn = (targetScreen == i);
				}
			}
		}

		// Token: 0x0600101C RID: 4124 RVA: 0x000660E4 File Offset: 0x000642E4
		private void ToggleNavigationButtons(int targetScreen)
		{
			if (this.PrevButton)
			{
				this.PrevButton.GetComponent<Button>().interactable = (targetScreen > 0);
			}
			if (this.NextButton)
			{
				this.NextButton.GetComponent<Button>().interactable = (targetScreen < this._screensContainer.transform.childCount - 1);
			}
		}

		// Token: 0x0600101D RID: 4125 RVA: 0x00066144 File Offset: 0x00064344
		private void OnValidate()
		{
			if (this._scroll_rect == null)
			{
				this._scroll_rect = base.GetComponent<ScrollRect>();
			}
			if (!this._scroll_rect.horizontal && !this._scroll_rect.vertical)
			{
				Debug.LogError("ScrollRect has to have a direction, please select either Horizontal OR Vertical with the appropriate control.");
			}
			if (this._scroll_rect.horizontal && this._scroll_rect.vertical)
			{
				Debug.LogError("ScrollRect has to be unidirectional, only use either Horizontal or Vertical on the ScrollRect, NOT both.");
			}
			int childCount = base.gameObject.GetComponent<ScrollRect>().content.childCount;
			if (childCount != 0 || this.ChildObjects != null)
			{
				int num = (this.ChildObjects == null || this.ChildObjects.Length == 0) ? childCount : this.ChildObjects.Length;
				if (this.StartingScreen > num - 1)
				{
					this.StartingScreen = num - 1;
				}
				if (this.StartingScreen < 0)
				{
					this.StartingScreen = 0;
				}
			}
			if (this.MaskBuffer <= 0f)
			{
				this.MaskBuffer = 1f;
			}
			if (this.PageStep < 0f)
			{
				this.PageStep = 0f;
			}
			if (this.PageStep > 8f)
			{
				this.PageStep = 9f;
			}
		}

		// Token: 0x0600101E RID: 4126 RVA: 0x0006625D File Offset: 0x0006445D
		public void StartScreenChange()
		{
			if (!this._moveStarted)
			{
				this._moveStarted = true;
				this.OnSelectionChangeStartEvent.Invoke();
			}
		}

		// Token: 0x0600101F RID: 4127 RVA: 0x00066279 File Offset: 0x00064479
		internal void ScreenChange()
		{
			this.OnSelectionPageChangedEvent.Invoke(this._currentPage);
		}

		// Token: 0x06001020 RID: 4128 RVA: 0x0006628C File Offset: 0x0006448C
		internal void EndScreenChange()
		{
			this.OnSelectionChangeEndEvent.Invoke(this._currentPage);
			this._settled = true;
			this._moveStarted = false;
		}

		// Token: 0x06001021 RID: 4129 RVA: 0x000662AD File Offset: 0x000644AD
		public Transform CurrentPageObject()
		{
			return this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x06001022 RID: 4130 RVA: 0x000662C0 File Offset: 0x000644C0
		public void CurrentPageObject(out Transform returnObject)
		{
			returnObject = this._screensContainer.GetChild(this.CurrentPage);
		}

		// Token: 0x06001023 RID: 4131 RVA: 0x000662D5 File Offset: 0x000644D5
		public void OnBeginDrag(PointerEventData eventData)
		{
			this._pointerDown = true;
			this._settled = false;
			this.StartScreenChange();
			this._startPosition = this._screensContainer.localPosition;
		}

		// Token: 0x06001024 RID: 4132 RVA: 0x000662FC File Offset: 0x000644FC
		public void OnDrag(PointerEventData eventData)
		{
			this._lerp = false;
		}

		// Token: 0x06001025 RID: 4133 RVA: 0x00066308 File Offset: 0x00064508
		int IScrollSnap.CurrentPage()
		{
			return this.CurrentPage = this.GetPageforPosition(this._screensContainer.localPosition);
		}

		// Token: 0x06001026 RID: 4134 RVA: 0x0006632F File Offset: 0x0006452F
		public void SetLerp(bool value)
		{
			this._lerp = value;
		}

		// Token: 0x06001027 RID: 4135 RVA: 0x00066338 File Offset: 0x00064538
		public void ChangePage(int page)
		{
			this.GoToScreen(page);
		}

		// Token: 0x04000F2D RID: 3885
		internal Rect panelDimensions;

		// Token: 0x04000F2E RID: 3886
		internal RectTransform _screensContainer;

		// Token: 0x04000F2F RID: 3887
		internal bool _isVertical;

		// Token: 0x04000F30 RID: 3888
		internal int _screens = 1;

		// Token: 0x04000F31 RID: 3889
		internal float _scrollStartPosition;

		// Token: 0x04000F32 RID: 3890
		internal float _childSize;

		// Token: 0x04000F33 RID: 3891
		private float _childPos;

		// Token: 0x04000F34 RID: 3892
		private float _maskSize;

		// Token: 0x04000F35 RID: 3893
		internal Vector2 _childAnchorPoint;

		// Token: 0x04000F36 RID: 3894
		internal ScrollRect _scroll_rect;

		// Token: 0x04000F37 RID: 3895
		internal Vector3 _lerp_target;

		// Token: 0x04000F38 RID: 3896
		internal bool _lerp;

		// Token: 0x04000F39 RID: 3897
		internal bool _pointerDown;

		// Token: 0x04000F3A RID: 3898
		internal bool _settled = true;

		// Token: 0x04000F3B RID: 3899
		internal Vector3 _startPosition;

		// Token: 0x04000F3C RID: 3900
		[Tooltip("The currently active page")]
		internal int _currentPage;

		// Token: 0x04000F3D RID: 3901
		internal int _previousPage;

		// Token: 0x04000F3E RID: 3902
		internal int _halfNoVisibleItems;

		// Token: 0x04000F3F RID: 3903
		internal bool _moveStarted;

		// Token: 0x04000F40 RID: 3904
		private int _bottomItem;

		// Token: 0x04000F41 RID: 3905
		private int _topItem;

		// Token: 0x04000F42 RID: 3906
		[Tooltip("The screen / page to start the control on\n*Note, this is a 0 indexed array")]
		[SerializeField]
		public int StartingScreen;

		// Token: 0x04000F43 RID: 3907
		[Tooltip("The distance between two pages based on page height, by default pages are next to each other")]
		[SerializeField]
		[Range(0f, 8f)]
		public float PageStep = 1f;

		// Token: 0x04000F44 RID: 3908
		[Tooltip("The gameobject that contains toggles which suggest pagination. (optional)")]
		public GameObject Pagination;

		// Token: 0x04000F45 RID: 3909
		[Tooltip("Button to go to the previous page. (optional)")]
		public GameObject PrevButton;

		// Token: 0x04000F46 RID: 3910
		[Tooltip("Button to go to the next page. (optional)")]
		public GameObject NextButton;

		// Token: 0x04000F47 RID: 3911
		[Tooltip("Transition speed between pages. (optional)")]
		public float transitionSpeed = 7.5f;

		// Token: 0x04000F48 RID: 3912
		[Tooltip("Fast Swipe makes swiping page next / previous (optional)")]
		public bool UseFastSwipe;

		// Token: 0x04000F49 RID: 3913
		[Tooltip("Offset for how far a swipe has to travel to initiate a page change (optional)")]
		public int FastSwipeThreshold = 100;

		// Token: 0x04000F4A RID: 3914
		[Tooltip("Speed at which the ScrollRect will keep scrolling before slowing down and stopping (optional)")]
		public int SwipeVelocityThreshold = 100;

		// Token: 0x04000F4B RID: 3915
		[Tooltip("The visible bounds area, controls which items are visible/enabled. *Note Should use a RectMask. (optional)")]
		public RectTransform MaskArea;

		// Token: 0x04000F4C RID: 3916
		[Tooltip("Pixel size to buffer arround Mask Area. (optional)")]
		public float MaskBuffer = 1f;

		// Token: 0x04000F4D RID: 3917
		[Tooltip("By default the container will lerp to the start when enabled in the scene, this option overrides this and forces it to simply jump without lerping")]
		public bool JumpOnEnable;

		// Token: 0x04000F4E RID: 3918
		[Tooltip("By default the container will return to the original starting page when enabled, this option overrides this behaviour and stays on the current selection")]
		public bool RestartOnEnable;

		// Token: 0x04000F4F RID: 3919
		[Tooltip("(Experimental)\nBy default, child array objects will use the parent transform\nHowever you can disable this for some interesting effects")]
		public bool UseParentTransform = true;

		// Token: 0x04000F50 RID: 3920
		[Tooltip("Scroll Snap children. (optional)\nEither place objects in the scene as children OR\nPrefabs in this array, NOT BOTH")]
		public GameObject[] ChildObjects;

		// Token: 0x04000F51 RID: 3921
		[SerializeField]
		[Tooltip("Event fires when a user starts to change the selection")]
		private ScrollSnapBase.SelectionChangeStartEvent m_OnSelectionChangeStartEvent = new ScrollSnapBase.SelectionChangeStartEvent();

		// Token: 0x04000F52 RID: 3922
		[SerializeField]
		[Tooltip("Event fires as the page changes, while dragging or jumping")]
		private ScrollSnapBase.SelectionPageChangedEvent m_OnSelectionPageChangedEvent = new ScrollSnapBase.SelectionPageChangedEvent();

		// Token: 0x04000F53 RID: 3923
		[SerializeField]
		[Tooltip("Event fires when the page settles after a user has dragged")]
		private ScrollSnapBase.SelectionChangeEndEvent m_OnSelectionChangeEndEvent = new ScrollSnapBase.SelectionChangeEndEvent();

		// Token: 0x02000859 RID: 2137
		[Serializable]
		public class SelectionChangeStartEvent : UnityEvent
		{
		}

		// Token: 0x0200085A RID: 2138
		[Serializable]
		public class SelectionPageChangedEvent : UnityEvent<int>
		{
		}

		// Token: 0x0200085B RID: 2139
		[Serializable]
		public class SelectionChangeEndEvent : UnityEvent<int>
		{
		}
	}
}
