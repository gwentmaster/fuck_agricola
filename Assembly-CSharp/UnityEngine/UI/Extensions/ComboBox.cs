using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000172 RID: 370
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/ComboBox")]
	public class ComboBox : MonoBehaviour
	{
		// Token: 0x1700009D RID: 157
		// (get) Token: 0x06000E45 RID: 3653 RVA: 0x0005B474 File Offset: 0x00059674
		// (set) Token: 0x06000E46 RID: 3654 RVA: 0x0005B47C File Offset: 0x0005967C
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x06000E47 RID: 3655 RVA: 0x0005B485 File Offset: 0x00059685
		// (set) Token: 0x06000E48 RID: 3656 RVA: 0x0005B48D File Offset: 0x0005968D
		public string Text { get; private set; }

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x06000E49 RID: 3657 RVA: 0x0005B496 File Offset: 0x00059696
		// (set) Token: 0x06000E4A RID: 3658 RVA: 0x0005B49E File Offset: 0x0005969E
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

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x06000E4B RID: 3659 RVA: 0x0005B4AD File Offset: 0x000596AD
		// (set) Token: 0x06000E4C RID: 3660 RVA: 0x0005B4B5 File Offset: 0x000596B5
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

		// Token: 0x06000E4D RID: 3661 RVA: 0x0005B4C4 File Offset: 0x000596C4
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06000E4E RID: 3662 RVA: 0x0005B4D0 File Offset: 0x000596D0
		private bool Initialize()
		{
			bool result = true;
			try
			{
				this._rectTransform = base.GetComponent<RectTransform>();
				this._inputRT = this._rectTransform.Find("InputField").GetComponent<RectTransform>();
				this._mainInput = this._inputRT.GetComponent<InputField>();
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
				this.itemTemplate = this._rectTransform.Find("ItemTemplate").gameObject;
				this.itemTemplate.SetActive(false);
			}
			catch (NullReferenceException exception)
			{
				Debug.LogException(exception);
				Debug.LogError("Something is setup incorrectly with the dropdownlist component causing a Null Refernece Exception");
				result = false;
			}
			this.panelObjects = new Dictionary<string, GameObject>();
			this._panelItems = this.AvailableOptions.ToList<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x06000E4F RID: 3663 RVA: 0x0005B698 File Offset: 0x00059898
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			this._panelItems.Sort();
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
			this.panelObjects.Clear();
			int num = 0;
			while (list.Count < this.AvailableOptions.Count)
			{
				GameObject gameObject = Object.Instantiate<GameObject>(this.itemTemplate);
				gameObject.name = "Item " + num;
				gameObject.transform.SetParent(this._itemsPanelRT, false);
				list.Add(gameObject);
				num++;
			}
			for (int i = 0; i < list.Count; i++)
			{
				list[i].SetActive(i <= this.AvailableOptions.Count);
				if (i < this.AvailableOptions.Count)
				{
					list[i].name = string.Concat(new object[]
					{
						"Item ",
						i,
						" ",
						this._panelItems[i]
					});
					list[i].transform.Find("Text").GetComponent<Text>().text = this._panelItems[i];
					Button component = list[i].GetComponent<Button>();
					component.onClick.RemoveAllListeners();
					string textOfItem = this._panelItems[i];
					component.onClick.AddListener(delegate()
					{
						this.OnItemClicked(textOfItem);
					});
					this.panelObjects[this._panelItems[i]] = list[i];
				}
			}
		}

		// Token: 0x06000E50 RID: 3664 RVA: 0x0005B8B0 File Offset: 0x00059AB0
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x06000E51 RID: 3665 RVA: 0x0005B8D4 File Offset: 0x00059AD4
		private void RedrawPanel()
		{
			float num = (this._panelItems.Count > this.ItemsToDisplay) ? this._scrollBarWidth : 0f;
			this._scrollBarRT.gameObject.SetActive(this._panelItems.Count > this.ItemsToDisplay);
			if (!this._hasDrawnOnce || this._rectTransform.sizeDelta != this._inputRT.sizeDelta)
			{
				this._hasDrawnOnce = true;
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
				this._inputRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._rectTransform.sizeDelta.y);
				this._scrollPanelRT.SetParent(base.transform, true);
				this._scrollPanelRT.anchoredPosition = new Vector2(0f, -this._rectTransform.sizeDelta.y);
				this._overlayRT.SetParent(this._canvas.transform, false);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._canvasRT.sizeDelta.x);
				this._overlayRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, this._canvasRT.sizeDelta.y);
				this._overlayRT.SetParent(base.transform, true);
				this._scrollPanelRT.SetParent(this._overlayRT, true);
			}
			if (this._panelItems.Count < 1)
			{
				return;
			}
			float num2 = this._rectTransform.sizeDelta.y * (float)Mathf.Min(this._itemsToDisplay, this._panelItems.Count);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._scrollPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._rectTransform.sizeDelta.x);
			this._itemsPanelRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, this._scrollPanelRT.sizeDelta.x - num - 5f);
			this._itemsPanelRT.anchoredPosition = new Vector2(5f, 0f);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, num);
			this._scrollBarRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0f);
			this._slidingAreaRT.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, num2 - this._scrollBarRT.sizeDelta.x);
		}

		// Token: 0x06000E52 RID: 3666 RVA: 0x0005BB20 File Offset: 0x00059D20
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.RedrawPanel();
			if (this._panelItems.Count == 0)
			{
				this._isPanelActive = true;
				this.ToggleDropdownPanel(false);
			}
			else if (!this._isPanelActive)
			{
				this.ToggleDropdownPanel(false);
			}
			this.OnSelectionChanged.Invoke(this.Text);
		}

		// Token: 0x06000E53 RID: 3667 RVA: 0x0005BB77 File Offset: 0x00059D77
		public void ToggleDropdownPanel(bool directClick)
		{
			this._isPanelActive = !this._isPanelActive;
			this._overlayRT.gameObject.SetActive(this._isPanelActive);
			if (this._isPanelActive)
			{
				base.transform.SetAsLastSibling();
				return;
			}
		}

		// Token: 0x04000DE5 RID: 3557
		public Color disabledTextColor;

		// Token: 0x04000DE7 RID: 3559
		public List<string> AvailableOptions;

		// Token: 0x04000DE8 RID: 3560
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04000DE9 RID: 3561
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04000DEA RID: 3562
		public ComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x04000DEB RID: 3563
		private bool _isPanelActive;

		// Token: 0x04000DEC RID: 3564
		private bool _hasDrawnOnce;

		// Token: 0x04000DED RID: 3565
		private InputField _mainInput;

		// Token: 0x04000DEE RID: 3566
		private RectTransform _inputRT;

		// Token: 0x04000DEF RID: 3567
		private RectTransform _rectTransform;

		// Token: 0x04000DF0 RID: 3568
		private RectTransform _overlayRT;

		// Token: 0x04000DF1 RID: 3569
		private RectTransform _scrollPanelRT;

		// Token: 0x04000DF2 RID: 3570
		private RectTransform _scrollBarRT;

		// Token: 0x04000DF3 RID: 3571
		private RectTransform _slidingAreaRT;

		// Token: 0x04000DF4 RID: 3572
		private RectTransform _itemsPanelRT;

		// Token: 0x04000DF5 RID: 3573
		private Canvas _canvas;

		// Token: 0x04000DF6 RID: 3574
		private RectTransform _canvasRT;

		// Token: 0x04000DF7 RID: 3575
		private ScrollRect _scrollRect;

		// Token: 0x04000DF8 RID: 3576
		private List<string> _panelItems;

		// Token: 0x04000DF9 RID: 3577
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x04000DFA RID: 3578
		private GameObject itemTemplate;

		// Token: 0x0200083E RID: 2110
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string>
		{
		}
	}
}
