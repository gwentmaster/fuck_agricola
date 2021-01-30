using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	// Token: 0x02000171 RID: 369
	[RequireComponent(typeof(RectTransform))]
	[AddComponentMenu("UI/Extensions/AutoComplete ComboBox")]
	public class AutoCompleteComboBox : MonoBehaviour
	{
		// Token: 0x17000098 RID: 152
		// (get) Token: 0x06000E2E RID: 3630 RVA: 0x0005A9E5 File Offset: 0x00058BE5
		// (set) Token: 0x06000E2F RID: 3631 RVA: 0x0005A9ED File Offset: 0x00058BED
		public DropDownListItem SelectedItem { get; private set; }

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06000E30 RID: 3632 RVA: 0x0005A9F6 File Offset: 0x00058BF6
		// (set) Token: 0x06000E31 RID: 3633 RVA: 0x0005A9FE File Offset: 0x00058BFE
		public string Text { get; private set; }

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06000E32 RID: 3634 RVA: 0x0005AA07 File Offset: 0x00058C07
		// (set) Token: 0x06000E33 RID: 3635 RVA: 0x0005AA0F File Offset: 0x00058C0F
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

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x06000E34 RID: 3636 RVA: 0x0005AA1E File Offset: 0x00058C1E
		// (set) Token: 0x06000E35 RID: 3637 RVA: 0x0005AA26 File Offset: 0x00058C26
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

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x06000E36 RID: 3638 RVA: 0x0005AA35 File Offset: 0x00058C35
		// (set) Token: 0x06000E37 RID: 3639 RVA: 0x0005AA3D File Offset: 0x00058C3D
		public bool InputColorMatching
		{
			get
			{
				return this._ChangeInputTextColorBasedOnMatchingItems;
			}
			set
			{
				this._ChangeInputTextColorBasedOnMatchingItems = value;
				if (this._ChangeInputTextColorBasedOnMatchingItems)
				{
					this.SetInputTextColor();
				}
			}
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0005AA54 File Offset: 0x00058C54
		public void Awake()
		{
			this.Initialize();
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0005AA5D File Offset: 0x00058C5D
		public void Start()
		{
			if (this.SelectFirstItemOnStart && this.AvailableOptions.Count > 0)
			{
				this.ToggleDropdownPanel(false);
				this.OnItemClicked(this.AvailableOptions[0]);
			}
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0005AA90 File Offset: 0x00058C90
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
			this._prunedPanelItems = new List<string>();
			this._panelItems = new List<string>();
			this.RebuildPanel();
			return result;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0005AC5C File Offset: 0x00058E5C
		private void RebuildPanel()
		{
			this._panelItems.Clear();
			this._prunedPanelItems.Clear();
			this.panelObjects.Clear();
			foreach (string text in this.AvailableOptions)
			{
				this._panelItems.Add(text.ToLower());
			}
			List<GameObject> list = new List<GameObject>(this.panelObjects.Values);
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
			this.SetInputTextColor();
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0005AE7C File Offset: 0x0005907C
		private void OnItemClicked(string item)
		{
			this.Text = item;
			this._mainInput.text = this.Text;
			this.ToggleDropdownPanel(true);
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0005AEA0 File Offset: 0x000590A0
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

		// Token: 0x06000E3E RID: 3646 RVA: 0x0005B0EC File Offset: 0x000592EC
		public void OnValueChanged(string currText)
		{
			this.Text = currText;
			this.PruneItems(currText);
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
			bool flag = this._panelItems.Contains(this.Text) != this._selectionIsValid;
			this._selectionIsValid = this._panelItems.Contains(this.Text);
			this.OnSelectionChanged.Invoke(this.Text, this._selectionIsValid);
			this.OnSelectionTextChanged.Invoke(this.Text);
			if (flag)
			{
				this.OnSelectionValidityChanged.Invoke(this._selectionIsValid);
			}
			this.SetInputTextColor();
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0005B1B0 File Offset: 0x000593B0
		private void SetInputTextColor()
		{
			if (this.InputColorMatching)
			{
				if (this._selectionIsValid)
				{
					this._mainInput.textComponent.color = this.ValidSelectionTextColor;
					return;
				}
				if (this._panelItems.Count > 0)
				{
					this._mainInput.textComponent.color = this.MatchingItemsRemainingTextColor;
					return;
				}
				this._mainInput.textComponent.color = this.NoItemsRemainingTextColor;
			}
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0005B21F File Offset: 0x0005941F
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

		// Token: 0x06000E41 RID: 3649 RVA: 0x0005B25C File Offset: 0x0005945C
		private void PruneItems(string currText)
		{
			if (this.autocompleteSearchType == AutoCompleteSearchType.Linq)
			{
				this.PruneItemsLinq(currText);
				return;
			}
			this.PruneItemsArray(currText);
		}

		// Token: 0x06000E42 RID: 3650 RVA: 0x0005B278 File Offset: 0x00059478
		private void PruneItemsLinq(string currText)
		{
			currText = currText.ToLower();
			foreach (string text in (from x in this._panelItems
			where !x.Contains(currText)
			select x).ToArray<string>())
			{
				this.panelObjects[text].SetActive(false);
				this._panelItems.Remove(text);
				this._prunedPanelItems.Add(text);
			}
			foreach (string text2 in (from x in this._prunedPanelItems
			where x.Contains(currText)
			select x).ToArray<string>())
			{
				this.panelObjects[text2].SetActive(true);
				this._panelItems.Add(text2);
				this._prunedPanelItems.Remove(text2);
			}
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0005B35C File Offset: 0x0005955C
		private void PruneItemsArray(string currText)
		{
			string value = currText.ToLower();
			for (int i = this._panelItems.Count - 1; i >= 0; i--)
			{
				string text = this._panelItems[i];
				if (!text.Contains(value))
				{
					this.panelObjects[this._panelItems[i]].SetActive(false);
					this._panelItems.RemoveAt(i);
					this._prunedPanelItems.Add(text);
				}
			}
			for (int j = this._prunedPanelItems.Count - 1; j >= 0; j--)
			{
				string text2 = this._prunedPanelItems[j];
				if (text2.Contains(value))
				{
					this.panelObjects[this._prunedPanelItems[j]].SetActive(true);
					this._prunedPanelItems.RemoveAt(j);
					this._panelItems.Add(text2);
				}
			}
		}

		// Token: 0x04000DC4 RID: 3524
		public Color disabledTextColor;

		// Token: 0x04000DC6 RID: 3526
		public List<string> AvailableOptions;

		// Token: 0x04000DC7 RID: 3527
		private bool _isPanelActive;

		// Token: 0x04000DC8 RID: 3528
		private bool _hasDrawnOnce;

		// Token: 0x04000DC9 RID: 3529
		private InputField _mainInput;

		// Token: 0x04000DCA RID: 3530
		private RectTransform _inputRT;

		// Token: 0x04000DCB RID: 3531
		private RectTransform _rectTransform;

		// Token: 0x04000DCC RID: 3532
		private RectTransform _overlayRT;

		// Token: 0x04000DCD RID: 3533
		private RectTransform _scrollPanelRT;

		// Token: 0x04000DCE RID: 3534
		private RectTransform _scrollBarRT;

		// Token: 0x04000DCF RID: 3535
		private RectTransform _slidingAreaRT;

		// Token: 0x04000DD0 RID: 3536
		private RectTransform _itemsPanelRT;

		// Token: 0x04000DD1 RID: 3537
		private Canvas _canvas;

		// Token: 0x04000DD2 RID: 3538
		private RectTransform _canvasRT;

		// Token: 0x04000DD3 RID: 3539
		private ScrollRect _scrollRect;

		// Token: 0x04000DD4 RID: 3540
		private List<string> _panelItems;

		// Token: 0x04000DD5 RID: 3541
		private List<string> _prunedPanelItems;

		// Token: 0x04000DD6 RID: 3542
		private Dictionary<string, GameObject> panelObjects;

		// Token: 0x04000DD7 RID: 3543
		private GameObject itemTemplate;

		// Token: 0x04000DD9 RID: 3545
		[SerializeField]
		private float _scrollBarWidth = 20f;

		// Token: 0x04000DDA RID: 3546
		[SerializeField]
		private int _itemsToDisplay;

		// Token: 0x04000DDB RID: 3547
		public bool SelectFirstItemOnStart;

		// Token: 0x04000DDC RID: 3548
		[SerializeField]
		[Tooltip("Change input text color based on matching items")]
		private bool _ChangeInputTextColorBasedOnMatchingItems;

		// Token: 0x04000DDD RID: 3549
		public Color ValidSelectionTextColor = Color.green;

		// Token: 0x04000DDE RID: 3550
		public Color MatchingItemsRemainingTextColor = Color.black;

		// Token: 0x04000DDF RID: 3551
		public Color NoItemsRemainingTextColor = Color.red;

		// Token: 0x04000DE0 RID: 3552
		public AutoCompleteSearchType autocompleteSearchType = AutoCompleteSearchType.Linq;

		// Token: 0x04000DE1 RID: 3553
		private bool _selectionIsValid;

		// Token: 0x04000DE2 RID: 3554
		public AutoCompleteComboBox.SelectionTextChangedEvent OnSelectionTextChanged;

		// Token: 0x04000DE3 RID: 3555
		public AutoCompleteComboBox.SelectionValidityChangedEvent OnSelectionValidityChanged;

		// Token: 0x04000DE4 RID: 3556
		public AutoCompleteComboBox.SelectionChangedEvent OnSelectionChanged;

		// Token: 0x02000839 RID: 2105
		[Serializable]
		public class SelectionChangedEvent : UnityEvent<string, bool>
		{
		}

		// Token: 0x0200083A RID: 2106
		[Serializable]
		public class SelectionTextChangedEvent : UnityEvent<string>
		{
		}

		// Token: 0x0200083B RID: 2107
		[Serializable]
		public class SelectionValidityChangedEvent : UnityEvent<bool>
		{
		}
	}
}
