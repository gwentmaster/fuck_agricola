using System;
using System.Collections.Generic;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000173 RID: 371
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/Dropdown List")]
	public class DropDownList : MonoBehaviour
	{
		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06000E55 RID: 3669 RVA: 0x0005BBC7 File Offset: 0x00059DC7
		// (set) Token: 0x06000E56 RID: 3670 RVA: 0x0005BBCF File Offset: 0x00059DCF
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06000E57 RID: 3671 RVA: 0x0005BBD8 File Offset: 0x00059DD8
		// (set) Token: 0x06000E58 RID: 3672 RVA: 0x0005BBE0 File Offset: 0x00059DE0
		public float ScrollBarWidth
		{
			get
			{
				return this._scrollBarWidth;
			}
			set
			{
				this._scrollBarWidth = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06000E59 RID: 3673 RVA: 0x0005BBEF File Offset: 0x00059DEF
		// (set) Token: 0x06000E5A RID: 3674 RVA: 0x0005BBF7 File Offset: 0x00059DF7
		public int ItemsToDisplay
		{
			get
			{
				return this._itemsToDisplay;
			}
			set
			{
				this._itemsToDisplay = value;
				this.RedrawPanel();
			}
		}

		// Token: 0x06000E5B RID: 3675 RVA: 0x0005BC06 File Offset: 0x00059E06
		public void Start()
		{
			this.Initialize();
			if (this.SelectFirstItemOnStart && this.Items.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(0);
			}
		}

		// Token: 0x06000E5C RID: 3676 RVA: 0x0005BC34 File Offset: 0x00059E34
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._mainButton = new DropDownListButton(this._rectTransform.Find("MainButton").gameObject);
				this._overlayRT = this._rectTransform.Find("Overlay").GetComponent<RectTransform>();
				this._overlayRT.gameObject.SetActive(false);
				this._scrollPanelRT = this._overlayRT.Find("ScrollPanel").GetComponent<RectTransform>();
				this._scrollBarRT = this._scrollPanelRT.Find("Scrollbar").GetComponent<RectTransform>();
				this._slidingAreaRT = this._scrollBarRT.Find("SlidingArea").GetComponent<RectTransform>();
				this._itemsPanelRT = this._scrollPanelRT.Find("Items").GetComponent<RectTransform>();
				this._canvas = base.GetComponentInParent<Canvas>();
				this._canvasRT = this._canvas.GetComponent<RectTransform>();
				this._scrollRect = this._scrollPanelRT.GetComponent<ScrollRect>();
				this._scrollRect.scrollSensitivity = this._rectTransform.sizeDelta.y / 2f;
				this._scrollRect.movementType = ScrollRect.MovementType.Clamped;
				this._scrollRect.content = this._itemsPanelRT;
				this._itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this._itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Reference Exception");
				result = false;
			}
			this._panelItems = new List<DropDownListButton>();
			this.RebuildPanel();
			this.RedrawPanel();
			return result;
		}

		// Token: 0x06000E5D RID: 3677 RVA: 0x0005BDE4 File Offset: 0x00059FE4
		private void RebuildPanel()
		{
			if (this.Items.Count == 0)
			{
				return;
			}
			int num = this._panelItems.Count;
			while (this._panelItems.Count < this.Items.Count)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this._itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				this._panelItems.Add(new DropDownListButton(gameObject));
				num++;
			}
			for (int i = 0; i < this._panelItems.Count; i++)
			{
				if (i < this.Items.Count)
				{
					DropDownListItem item = this.Items[i];
					this._panelItems[i].txt.text = item.Caption;
					if (item.IsDisabled)
					{
						this._panelItems[i].txt.color = this.disabledTextColor;
					}
					if (this._panelItems[i].btnImg != null)
					{
						this._panelItems[i].btnImg.sprite = null;
					}
					this._panelItems[i].img.sprite = item.Image;
					this._panelItems[i].img.color = ((item.Image == null) ? new Color(1f, 1f, 1f, 0f) : (item.IsDisabled ? new Color(1f, 1f, 1f, 0.5f) : Color.white));
					int ii = i;
					this._panelItems[i].btn.onClick.RemoveAllListeners();
					this._panelItems[i].btn.onClick.AddListener(delegate()
					{
						this.OnItemClicked(ii);
						if (item.OnSelect != null)
						{
							item.OnSelect();
						}
					});
				}
				this._panelItems[i].gameobject.SetActive(i < this.Items.Count);
			}
		}

		// Token: 0x06000E5E RID: 3678 RVA: 0x0005C033 File Offset: 0x0005A233
		private void OnItemClicked(int indx)
		{
			if (indx != this._selectedIndex && this.OnSelectionChanged != null)
			{
				this.OnSelectionChanged.Invoke(indx);
			}
			this._selectedIndex = indx;
			this.ToggleDropdownPanel(true);
			this.UpdateSelected();
		}

		// Token: 0x06000E5F RID: 3679 RVA: 0x0005C068 File Offset: 0x0005A268
		private void UpdateSelected()
		{
			this.SelectedItem = ((this._selectedIndex > -1 && this._selectedIndex < this.Items.Count) ? this.Items[this._selectedIndex] : null);
			if (this.SelectedItem == null)
			{
				return;
			}
			if (this.SelectedItem.Image != null)
			{
				this._mainButton.img.sprite = this.SelectedItem.Image;
				this._mainButton.img.color = Color.white;
			}
			else
			{
				this._mainButton.img.sprite = null;
			}
			this._mainButton.txt.text = this.SelectedItem.Caption;
			if (this.OverrideHighlighted)
			{
				for (int i = 0; i < this._itemsPanelRT.childCount; i++)
				{
					this._panelItems[i].btnImg.color = ((this._selectedIndex == i) ? this._mainButton.btn.colors.highlightedColor : new Color(0f, 0f, 0f, 0f));
				}
			}
		}

		// Token: 0x06000E60 RID: 3680 RVA: 0x0005C198 File Offset: 0x0005A398
		private void RedrawPanel()
		{
			float num = (this.Items.Count > this.ItemsToDisplay) ? this._scrollBarWidth : 0f;
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._mainButton.rectTransform.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._mainButton.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._mainButton.txt.rectTransform.offsetMax = new Vector2(4f, 0f);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this.Items.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this.Items.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x06000E61 RID: 3681 RVA: 0x0005C3F4 File Offset: 0x0005A5F4
		public void ToggleDropdownPanel(bool directClick)
		{
			this._overlayRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._scrollBarRT.transform.localScale = new Vector3(1f, 1f, 1f);
			this._isPanelActive = !this._isPanelActive;
			this._overlayRT.gameObject.SetActive(this._isPanelActive);
			if (this._isPanelActive)
			{
				base.transform.SetAsLastSibling();
				return;
			}
		}

		// Token: 0x04000DFC RID: 3580
		public Color disabledTextColor;

		// Token: 0x04000DFE RID: 3582
		public List<DropDownListItem> Items;

		// Token: 0x04000DFF RID: 3583
		public bool OverrideHighlighted = true;

		// Token: 0x04000E00 RID: 3584
		private bool _isPanelActive;

		// Token: 0x04000E01 RID: 3585
		private bool _hasDrawnOnce;

		// Token: 0x04000E02 RID: 3586
		private DropDownListButton _mainButton;

		// Token: 0x04000E03 RID: 3587
		private RectTransform _rectTransform;

		// Token: 0x04000E04 RID: 3588
		private RectTransform _overlayRT;

		// Token: 0x04000E05 RID: 3589
		private RectTransform _scrollPanelRT;

		// Token: 0x04000E06 RID: 3590
		private RectTransform _scrollBarRT;

		// Token: 0x04000E07 RID: 3591
		private RectTransform _slidingAreaRT;

		// Token: 0x04000E08 RID: 3592
		private RectTransform _itemsPanelRT;

		// Token: 0x04000E09 RID: 3593
		private Canvas _canvas;

		// Token: 0x04000E0A RID: 3594
		private RectTransform _canvasRT;

		// Token: 0x04000E0B RID: 3595
		private ScrollRect _scrollRect;

		// Token: 0x04000E0C RID: 3596
		private List<DropDownListButton> _panelItems;

		// Token: 0x04000E0D RID: 3597
		private GameObject _itemTemplate;

		// Token: 0x04000E0E RID: 3598
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04000E0F RID: 3599
		private int _selectedIndex = -1;

		// Token: 0x04000E10 RID: 3600
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04000E11 RID: 3601
		public bool SelectFirstItemOnStart;

		// Token: 0x04000E12 RID: 3602
		public DropDownList.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x02000840 RID: 2112
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<int>
		{
		}
	}
}
