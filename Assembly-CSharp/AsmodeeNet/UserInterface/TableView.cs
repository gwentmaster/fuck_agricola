using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063F RID: 1599
	[RequireComponent(typeof(ScrollRect))]
	public class TableView : MonoBehaviour
	{
		// Token: 0x14000037 RID: 55
		// (add) Token: 0x06003AC2 RID: 15042 RVA: 0x001241C8 File Offset: 0x001223C8
		// (remove) Token: 0x06003AC3 RID: 15043 RVA: 0x00124200 File Offset: 0x00122400
		public event Action<int, bool> OnCellVisibilityChanged;

		// Token: 0x14000038 RID: 56
		// (add) Token: 0x06003AC4 RID: 15044 RVA: 0x00124238 File Offset: 0x00122438
		// (remove) Token: 0x06003AC5 RID: 15045 RVA: 0x00124270 File Offset: 0x00122470
		public event Action OnScrollOver;

		// Token: 0x14000039 RID: 57
		// (add) Token: 0x06003AC6 RID: 15046 RVA: 0x001242A8 File Offset: 0x001224A8
		// (remove) Token: 0x06003AC7 RID: 15047 RVA: 0x001242E0 File Offset: 0x001224E0
		public event Action<int> OnCellSelected;

		// Token: 0x1400003A RID: 58
		// (add) Token: 0x06003AC8 RID: 15048 RVA: 0x00124318 File Offset: 0x00122518
		// (remove) Token: 0x06003AC9 RID: 15049 RVA: 0x00124350 File Offset: 0x00122550
		public event Action<int> OnCellDeselected;

		// Token: 0x170007DD RID: 2013
		// (get) Token: 0x06003ACA RID: 15050 RVA: 0x00124385 File Offset: 0x00122585
		// (set) Token: 0x06003ACB RID: 15051 RVA: 0x0012438D File Offset: 0x0012258D
		public ITableViewDataSource DataSource
		{
			get
			{
				return this._dataSource;
			}
			set
			{
				this._dataSource = value;
				this._requiresReload = true;
			}
		}

		// Token: 0x170007DE RID: 2014
		// (get) Token: 0x06003ACC RID: 15052 RVA: 0x001243A0 File Offset: 0x001225A0
		// (set) Token: 0x06003ACD RID: 15053 RVA: 0x001243D8 File Offset: 0x001225D8
		public int SelectedCellIndex
		{
			get
			{
				if (this._SelectedRow == null)
				{
					return -1;
				}
				return this._CellIndexFromRow(this._SelectedRow.Value, null);
			}
			set
			{
				this._SelectedRow = new int?(this._RowFromCellIndex(value, null));
			}
		}

		// Token: 0x170007DF RID: 2015
		// (get) Token: 0x06003ACE RID: 15054 RVA: 0x00124400 File Offset: 0x00122600
		public bool HasSelection
		{
			get
			{
				return this._SelectedRow != null;
			}
		}

		// Token: 0x06003ACF RID: 15055 RVA: 0x00124410 File Offset: 0x00122610
		public TableViewCell GetReusableCell(string reuseIdentifier)
		{
			LinkedList<TableViewCell> linkedList;
			if (!this._reusableCells.TryGetValue(reuseIdentifier, out linkedList))
			{
				return null;
			}
			if (linkedList.Count == 0)
			{
				return null;
			}
			TableViewCell value = linkedList.First.Value;
			linkedList.RemoveFirst();
			return value;
		}

		// Token: 0x06003AD0 RID: 15056 RVA: 0x0012444C File Offset: 0x0012264C
		private void _StoreCellForReuse(TableViewCell cell)
		{
			string reuseIdentifier = cell.ReuseIdentifier;
			if (string.IsNullOrEmpty(reuseIdentifier))
			{
				UnityEngine.Object.Destroy(cell.gameObject);
				return;
			}
			if (!this._reusableCells.ContainsKey(reuseIdentifier))
			{
				this._reusableCells.Add(reuseIdentifier, new LinkedList<TableViewCell>());
			}
			this._reusableCells[reuseIdentifier].AddLast(cell);
			cell.Clean();
			cell.transform.SetParent(this._reusableCellContainer, false);
		}

		// Token: 0x170007E0 RID: 2016
		// (get) Token: 0x06003AD1 RID: 15057 RVA: 0x001244BE File Offset: 0x001226BE
		public bool IsEmpty
		{
			get
			{
				ITableViewDataSource dataSource = this.DataSource;
				return dataSource != null && dataSource.GetNumberOfCellsInTableView(this) == 0;
			}
		}

		// Token: 0x170007E1 RID: 2017
		// (get) Token: 0x06003AD2 RID: 15058 RVA: 0x001244D5 File Offset: 0x001226D5
		// (set) Token: 0x06003AD3 RID: 15059 RVA: 0x001244DD File Offset: 0x001226DD
		public TableView.ScrollAnchor ScrollingStartingOrder
		{
			get
			{
				return this._scrollingStartingOrder;
			}
			set
			{
				this._scrollingStartingOrder = value;
				this._UpdateScrollAnchor();
				this._requiresReload = true;
			}
		}

		// Token: 0x06003AD4 RID: 15060 RVA: 0x001244F4 File Offset: 0x001226F4
		private void _UpdateScrollAnchor()
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Top)
			{
				this._verticalLayoutGroup.childAlignment = TextAnchor.UpperCenter;
				this._scrollRect.content.anchorMin = new Vector2(0f, 1f);
				this._scrollRect.content.anchorMax = new Vector2(1f, 1f);
				this._scrollRect.content.pivot = new Vector2(0.5f, 1f);
				return;
			}
			this._verticalLayoutGroup.childAlignment = TextAnchor.LowerCenter;
			this._scrollRect.content.anchorMin = new Vector2(0f, 0f);
			this._scrollRect.content.anchorMax = new Vector2(1f, 0f);
			this._scrollRect.content.pivot = new Vector2(0.5f, 0f);
		}

		// Token: 0x06003AD5 RID: 15061 RVA: 0x001245DC File Offset: 0x001227DC
		public void ReloadData()
		{
			this._ClearAllRows();
			int numberOfCellsInTableView = this.DataSource.GetNumberOfCellsInTableView(this);
			this._rowHeights = new List<float>(numberOfCellsInTableView);
			if (this.IsEmpty)
			{
				return;
			}
			for (int i = 0; i < numberOfCellsInTableView; i++)
			{
				int index = this._CellIndexFromRow(i, null);
				float item = this.DataSource.GetHeightForCellIndexInTableView(this, index) + this._verticalLayoutGroup.spacing;
				this._rowHeights.Add(item);
			}
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			this._RecalculateVisibleRows();
			this._requiresReload = false;
		}

		// Token: 0x06003AD6 RID: 15062 RVA: 0x001246B8 File Offset: 0x001228B8
		public bool InsertCellAtIndex(int index)
		{
			int num = this.DataSource.GetNumberOfCellsInTableView(this) - 1;
			if (index > num || index < 0)
			{
				AsmoLogger.Error("TableView", string.Format("Unable to add the target row: {0} is out of range [0..{1}]", index, num), null);
				return false;
			}
			int index2 = this._RowFromCellIndex(index, null);
			float item = this.DataSource.GetHeightForCellIndexInTableView(this, index) + this._verticalLayoutGroup.spacing;
			this._rowHeights.Insert(index2, item);
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			if (this.HasSelection && index <= this.SelectedCellIndex)
			{
				Action<int> onCellDeselected = this.OnCellDeselected;
				if (onCellDeselected != null)
				{
					onCellDeselected(this.SelectedCellIndex);
				}
				int selectedCellIndex = this.SelectedCellIndex;
				this.SelectedCellIndex = selectedCellIndex + 1;
				Action<int> onCellSelected = this.OnCellSelected;
				if (onCellSelected != null)
				{
					onCellSelected(this.SelectedCellIndex);
				}
			}
			this._requiresRefresh = true;
			return true;
		}

		// Token: 0x06003AD7 RID: 15063 RVA: 0x001247E4 File Offset: 0x001229E4
		public void InsertCellAtIndexAndFocusOnIt(int index, TableView.CellAnchor cellAnchor, bool animated)
		{
			this.InsertCellAtIndex(index);
			this.FocusOnCellAtIndex(index, cellAnchor, animated);
		}

		// Token: 0x06003AD8 RID: 15064 RVA: 0x001247F8 File Offset: 0x001229F8
		public bool RemoveCellAtIndex(int index)
		{
			int num = this._rowHeights.Count - 1;
			if (index > num || index < 0)
			{
				AsmoLogger.Error("TableView", string.Format("Unable to remove the target row: {0} is out of range [0..{1}]", index, num), null);
				return false;
			}
			int num2 = this._RowFromCellIndex(index, new int?(this._rowHeights.Count));
			if (this.HasSelection)
			{
				int num3 = num2;
				int? selectedRow = this._SelectedRow;
				if (num3 == selectedRow.GetValueOrDefault() & selectedRow != null)
				{
					Action<int> onCellDeselected = this.OnCellDeselected;
					if (onCellDeselected != null)
					{
						onCellDeselected(index);
					}
					this._SelectedRow = null;
				}
				else if (index < this.SelectedCellIndex)
				{
					Action<int> onCellDeselected2 = this.OnCellDeselected;
					if (onCellDeselected2 != null)
					{
						onCellDeselected2(this.SelectedCellIndex);
					}
					int selectedCellIndex = this.SelectedCellIndex;
					this.SelectedCellIndex = selectedCellIndex - 1;
					Action<int> onCellSelected = this.OnCellSelected;
					if (onCellSelected != null)
					{
						onCellSelected(this.SelectedCellIndex);
					}
				}
			}
			this._rowHeights.RemoveAt(num2);
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta[0], this._GetCumulativeRowHeight(this._rowHeights.Count - 1) + (float)this._verticalLayoutGroup.padding.vertical);
			this._requiresRefresh = true;
			return true;
		}

		// Token: 0x06003AD9 RID: 15065 RVA: 0x0012494C File Offset: 0x00122B4C
		public TableViewCell GetCellAtIndex(int index)
		{
			return this._GetCellAtRow(this._RowFromCellIndex(index, null));
		}

		// Token: 0x06003ADA RID: 15066 RVA: 0x00124970 File Offset: 0x00122B70
		private TableViewCell _GetCellAtRow(int row)
		{
			TableViewCell result = null;
			this._rowToVisibleCells.TryGetValue(row, out result);
			return result;
		}

		// Token: 0x06003ADB RID: 15067 RVA: 0x00124990 File Offset: 0x00122B90
		private int _GetRowOfCell(TableViewCell cell)
		{
			foreach (KeyValuePair<int, TableViewCell> keyValuePair in this._rowToVisibleCells)
			{
				if (keyValuePair.Value == cell)
				{
					return keyValuePair.Key;
				}
			}
			return -1;
		}

		// Token: 0x06003ADC RID: 15068 RVA: 0x001249F8 File Offset: 0x00122BF8
		public int GetIndexOfCell(TableViewCell cell)
		{
			int num = this._GetRowOfCell(cell);
			if (num > 0)
			{
				return this._CellIndexFromRow(num, null);
			}
			return -1;
		}

		// Token: 0x06003ADD RID: 15069 RVA: 0x00124A24 File Offset: 0x00122C24
		public bool IsCellVisibleAtIndex(int index)
		{
			int key = this._RowFromCellIndex(index, null);
			return this._rowToVisibleCells.ContainsKey(key);
		}

		// Token: 0x06003ADE RID: 15070 RVA: 0x00124A50 File Offset: 0x00122C50
		public void NotifyCellDimensionsChangedAtIndex(int index)
		{
			if (index < 0 || index >= this.DataSource.GetNumberOfCellsInTableView(this))
			{
				AsmoLogger.Error("TableView", string.Format("A cell is notifying that its dimension changed, but the index {0} is out of bounds", index), null);
				return;
			}
			int num = this._RowFromCellIndex(index, null);
			float num2 = this._rowHeights[num];
			float heightForCellIndexInTableView = this.DataSource.GetHeightForCellIndexInTableView(this, index);
			float num3 = heightForCellIndexInTableView + this._verticalLayoutGroup.spacing;
			this._rowHeights[num] = num3;
			TableViewCell tableViewCell = this._GetCellAtRow(num);
			if (tableViewCell != null)
			{
				tableViewCell.GetComponent<LayoutElement>().preferredHeight = heightForCellIndexInTableView;
			}
			float num4 = num3 - num2;
			this._scrollRect.content.sizeDelta = new Vector2(this._scrollRect.content.sizeDelta.x, this._scrollRect.content.sizeDelta.y + num4);
			this._requiresRefresh = true;
		}

		// Token: 0x06003ADF RID: 15071 RVA: 0x00124B44 File Offset: 0x00122D44
		public void FocusOnCellAtIndex(int index, TableView.CellAnchor cellAnchor, bool animated)
		{
			if (index < 0 || index >= this.DataSource.GetNumberOfCellsInTableView(this))
			{
				AsmoLogger.Error("TableView", string.Format("Unable to focus on the target index: {0} is out of bounds", index), null);
				return;
			}
			int row = this._RowFromCellIndex(index, null);
			float targetY = this._GetScrollYForRow(row, true) + this._verticalLayoutGroup.spacing;
			if (this._focusRoutine != null)
			{
				base.StopCoroutine(this._focusRoutine);
			}
			Action scrollPositionSetter = this._scrollPositionSetter;
			if (scrollPositionSetter != null)
			{
				scrollPositionSetter();
			}
			switch (cellAnchor)
			{
			case TableView.CellAnchor.Bottom:
				targetY -= (this._scrollRect.transform as RectTransform).rect.height - this.DataSource.GetHeightForCellIndexInTableView(this, index);
				break;
			case TableView.CellAnchor.Middle:
				targetY -= ((this._scrollRect.transform as RectTransform).rect.height - this.DataSource.GetHeightForCellIndexInTableView(this, index)) / 2f;
				break;
			}
			targetY = Mathf.Clamp(targetY, 0f, this._ScrollableHeight);
			this._scrollPositionSetter = delegate()
			{
				this._ScrollY = targetY;
				this._scrollPositionSetter = null;
			};
			if (animated)
			{
				this._focusRoutine = base.StartCoroutine(this._FocusOnRowAnimation(targetY));
				return;
			}
			this._scrollPositionSetter();
		}

		// Token: 0x06003AE0 RID: 15072 RVA: 0x00124CBE File Offset: 0x00122EBE
		private IEnumerator _FocusOnRowAnimation(float targetY)
		{
			float startTime = Time.time;
			float endTime = startTime + 1f;
			float StartY = this._ScrollY;
			while (Time.time < endTime)
			{
				float t = Mathf.InverseLerp(startTime, endTime, Time.time);
				this._ScrollY = Mathf.Lerp(StartY, targetY, t);
				yield return new WaitForEndOfFrame();
			}
			this._scrollPositionSetter();
			yield break;
		}

		// Token: 0x170007E2 RID: 2018
		// (get) Token: 0x06003AE1 RID: 15073 RVA: 0x00124CD4 File Offset: 0x00122ED4
		private float _ScrollableHeight
		{
			get
			{
				return this._scrollRect.content.rect.height - (base.transform as RectTransform).rect.height;
			}
		}

		// Token: 0x170007E3 RID: 2019
		// (get) Token: 0x06003AE2 RID: 15074 RVA: 0x00124D12 File Offset: 0x00122F12
		// (set) Token: 0x06003AE3 RID: 15075 RVA: 0x00124D1C File Offset: 0x00122F1C
		private float _ScrollY
		{
			get
			{
				return this._scrollY;
			}
			set
			{
				if (this.IsEmpty)
				{
					return;
				}
				value = Mathf.Clamp(value, 0f, this._GetScrollYForRow(this._rowHeights.Count - 1, true));
				if (this._scrollY != value)
				{
					this._scrollY = value;
					this._requiresRefresh = true;
					float num = value / this._ScrollableHeight;
					this._scrollRect.verticalNormalizedPosition = 1f - num;
				}
			}
		}

		// Token: 0x06003AE4 RID: 15076 RVA: 0x00124D88 File Offset: 0x00122F88
		private float _GetScrollYForRow(int row, bool above = true)
		{
			float num = this._GetCumulativeRowHeight(row);
			num += (float)this._verticalLayoutGroup.padding.top;
			if (above)
			{
				num -= this._rowHeights[row];
			}
			return num;
		}

		// Token: 0x06003AE5 RID: 15077 RVA: 0x00124DC4 File Offset: 0x00122FC4
		private void _ScrollViewValueChanged(Vector2 newScrollValue)
		{
			float num = (1f - newScrollValue.y) * this._ScrollableHeight;
			if (Mathf.Approximately(num, this._ScrollY))
			{
				return;
			}
			this._ScrollY = num;
			this._requiresRefresh = true;
		}

		// Token: 0x06003AE6 RID: 15078 RVA: 0x00124E02 File Offset: 0x00123002
		private void _RecalculateVisibleRows()
		{
			this._ClearAllRows();
			this._SetInitialVisibleRows();
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x00124E10 File Offset: 0x00123010
		private void _ClearAllRows()
		{
			while (this._rowToVisibleCells.Count > 0)
			{
				this._HideRow(false);
			}
			this._visibleRowRange = new Range(0, 0);
		}

		// Token: 0x06003AE8 RID: 15080 RVA: 0x00124E38 File Offset: 0x00123038
		private void Awake()
		{
			this._scrollRect = base.GetComponent<ScrollRect>();
			if (this._scrollRect == null)
			{
				AsmoLogger.Error("TableView", "ScrollRect is null", null);
				return;
			}
			this._verticalLayoutGroup = base.GetComponentInChildren<VerticalLayoutGroup>(true);
			if (this._verticalLayoutGroup == null)
			{
				AsmoLogger.Error("TableView", "VerticalLayoutGroup is null", null);
				return;
			}
			this._UpdateScrollAnchor();
			this._topContentPlaceholder = this._CreateEmptyContentPlaceHolderElement("TopContentPlaceholder");
			this._topContentPlaceholder.transform.SetParent(this._scrollRect.content, false);
			this._bottomContentPlaceholder = this._CreateEmptyContentPlaceHolderElement("BottomContentPlaceholder");
			this._bottomContentPlaceholder.transform.SetParent(this._scrollRect.content, false);
			this._rowToVisibleCells = new Dictionary<int, TableViewCell>();
			this._reusableCellContainer = new GameObject("ReusableCells", new Type[]
			{
				typeof(RectTransform)
			}).GetComponent<RectTransform>();
			this._reusableCellContainer.SetParent(base.transform, false);
			this._reusableCellContainer.gameObject.SetActive(false);
			this._reusableCells = new Dictionary<string, LinkedList<TableViewCell>>();
		}

		// Token: 0x06003AE9 RID: 15081 RVA: 0x00124F60 File Offset: 0x00123160
		private void Update()
		{
			if (this._requiresReload)
			{
				this.ReloadData();
			}
			if (!Mathf.Approximately(this._previousPos.y, this._scrollRect.normalizedPosition.y))
			{
				this._previousPos = this._scrollRect.normalizedPosition;
				if (this._stabilizationRoutine != null)
				{
					base.StopCoroutine(this._stabilizationRoutine);
				}
				this._stabilizationRoutine = base.StartCoroutine(this._WaitForScrollingStabilization());
			}
		}

		// Token: 0x06003AEA RID: 15082 RVA: 0x00124FD4 File Offset: 0x001231D4
		private IEnumerator _WaitForScrollingStabilization()
		{
			yield return new WaitForSeconds(this._stabilizationDelay);
			Action onScrollOver = this.OnScrollOver;
			if (onScrollOver != null)
			{
				onScrollOver();
			}
			yield break;
		}

		// Token: 0x06003AEB RID: 15083 RVA: 0x00124FE3 File Offset: 0x001231E3
		private void LateUpdate()
		{
			if (this._requiresRefresh)
			{
				this._RefreshVisibleRows();
			}
		}

		// Token: 0x06003AEC RID: 15084 RVA: 0x00124FF3 File Offset: 0x001231F3
		private void OnEnable()
		{
			this._scrollRect.onValueChanged.AddListener(new UnityAction<Vector2>(this._ScrollViewValueChanged));
		}

		// Token: 0x06003AED RID: 15085 RVA: 0x00125011 File Offset: 0x00123211
		private void OnDisable()
		{
			this._scrollRect.onValueChanged.RemoveListener(new UnityAction<Vector2>(this._ScrollViewValueChanged));
		}

		// Token: 0x06003AEE RID: 15086 RVA: 0x00125030 File Offset: 0x00123230
		private Range _CalculateCurrentVisibleRowRange()
		{
			float num = Math.Max(this._ScrollY - (float)this._verticalLayoutGroup.padding.top, 0f);
			float num2 = Math.Max((float)this._verticalLayoutGroup.padding.top - this._ScrollY, 0f);
			float y = num + (base.transform as RectTransform).rect.height - num2;
			int num3 = this._FindIndexOfRowAtY(num);
			int num4 = this._FindIndexOfRowAtY(y);
			return new Range(num3, num4 - num3 + 1);
		}

		// Token: 0x06003AEF RID: 15087 RVA: 0x001250C0 File Offset: 0x001232C0
		private void _SetInitialVisibleRows()
		{
			Range range = this._CalculateCurrentVisibleRowRange();
			for (int i = 0; i < range.count; i++)
			{
				this._AddRow(range.from + i);
			}
			this._visibleRowRange = range;
			this._UpdatePaddingElements();
			this._UpdateCellsSiblingIndices();
		}

		// Token: 0x06003AF0 RID: 15088 RVA: 0x00125108 File Offset: 0x00123308
		private void _AddRow(int row)
		{
			TableViewCell cellForIndexInTableView = this.DataSource.GetCellForIndexInTableView(this, this._CellIndexFromRow(row, null));
			cellForIndexInTableView.transform.SetParent(this._scrollRect.content, false);
			cellForIndexInTableView.OnSelection = delegate(TableViewCell cell)
			{
				if (this._SelectedRow != null)
				{
					Action<int> onCellDeselected = this.OnCellDeselected;
					if (onCellDeselected != null)
					{
						onCellDeselected(this._CellIndexFromRow(this._SelectedRow.Value, null));
					}
				}
				this._SelectedRow = new int?(this._GetRowOfCell(cell));
				Action<int> onCellSelected = this.OnCellSelected;
				if (onCellSelected == null)
				{
					return;
				}
				onCellSelected(this._CellIndexFromRow(this._SelectedRow.Value, null));
			};
			LayoutElement layoutElement = cellForIndexInTableView.GetComponent<LayoutElement>();
			if (layoutElement == null)
			{
				layoutElement = cellForIndexInTableView.gameObject.AddComponent<LayoutElement>();
			}
			layoutElement.preferredHeight = this._rowHeights[row];
			if (row > 0)
			{
				layoutElement.preferredHeight -= this._verticalLayoutGroup.spacing;
			}
			this._rowToVisibleCells[row] = cellForIndexInTableView;
			Action<int, bool> onCellVisibilityChanged = this.OnCellVisibilityChanged;
			if (onCellVisibilityChanged == null)
			{
				return;
			}
			onCellVisibilityChanged(this._CellIndexFromRow(row, null), true);
		}

		// Token: 0x06003AF1 RID: 15089 RVA: 0x001251D4 File Offset: 0x001233D4
		private void _RefreshVisibleRows()
		{
			this._requiresRefresh = false;
			if (this.IsEmpty)
			{
				return;
			}
			Range range = this._CalculateCurrentVisibleRowRange();
			int num = this._visibleRowRange.Last();
			int num2 = range.Last();
			if (range.from > num || num2 < this._visibleRowRange.from)
			{
				this._RecalculateVisibleRows();
				return;
			}
			for (int i = this._visibleRowRange.from; i < range.from; i++)
			{
				this._HideRow(false);
			}
			for (int j = num2; j < num; j++)
			{
				this._HideRow(true);
			}
			for (int k = this._visibleRowRange.from - 1; k >= range.from; k--)
			{
				this._AddRow(k);
			}
			for (int l = num + 1; l <= num2; l++)
			{
				this._AddRow(l);
			}
			this._visibleRowRange = range;
			this._UpdatePaddingElements();
			this._UpdateCellsSiblingIndices();
		}

		// Token: 0x06003AF2 RID: 15090 RVA: 0x001252B8 File Offset: 0x001234B8
		private void _UpdatePaddingElements()
		{
			float num = 0f;
			for (int i = 0; i < this._visibleRowRange.from; i++)
			{
				num += this._rowHeights[i];
			}
			this._topContentPlaceholder.preferredHeight = num;
			this._topContentPlaceholder.gameObject.SetActive(this._topContentPlaceholder.preferredHeight > 0f);
			for (int j = this._visibleRowRange.from; j <= this._visibleRowRange.Last(); j++)
			{
				num += this._rowHeights[j];
			}
			float num2 = this._scrollRect.content.rect.height - num;
			num2 -= (float)this._verticalLayoutGroup.padding.top;
			num2 -= (float)this._verticalLayoutGroup.padding.bottom;
			this._bottomContentPlaceholder.preferredHeight = num2 - this._verticalLayoutGroup.spacing;
			this._bottomContentPlaceholder.gameObject.SetActive(this._bottomContentPlaceholder.preferredHeight > 0f);
		}

		// Token: 0x06003AF3 RID: 15091 RVA: 0x001253CC File Offset: 0x001235CC
		private void _UpdateCellsSiblingIndices()
		{
			int num = 0;
			this._topContentPlaceholder.transform.SetSiblingIndex(num++);
			for (int i = this._visibleRowRange.from; i <= this._visibleRowRange.Last(); i++)
			{
				this._rowToVisibleCells[i].transform.SetSiblingIndex(num++);
			}
			this._bottomContentPlaceholder.transform.SetSiblingIndex(num++);
		}

		// Token: 0x06003AF4 RID: 15092 RVA: 0x00125440 File Offset: 0x00123640
		private void _HideRow(bool last)
		{
			int num = last ? this._visibleRowRange.Last() : this._visibleRowRange.from;
			int arg = this._CellIndexFromRow(num, null);
			TableViewCell cell = this._rowToVisibleCells[num];
			this._StoreCellForReuse(cell);
			this._rowToVisibleCells.Remove(num);
			this._visibleRowRange.count = this._visibleRowRange.count - 1;
			if (!last)
			{
				this._visibleRowRange.from = this._visibleRowRange.from + 1;
			}
			Action<int, bool> onCellVisibilityChanged = this.OnCellVisibilityChanged;
			if (onCellVisibilityChanged == null)
			{
				return;
			}
			onCellVisibilityChanged(arg, false);
		}

		// Token: 0x06003AF5 RID: 15093 RVA: 0x001254D0 File Offset: 0x001236D0
		private LayoutElement _CreateEmptyContentPlaceHolderElement(string name)
		{
			return new GameObject(name, new Type[]
			{
				typeof(RectTransform),
				typeof(LayoutElement)
			}).GetComponent<LayoutElement>();
		}

		// Token: 0x06003AF6 RID: 15094 RVA: 0x001254FD File Offset: 0x001236FD
		private int _FindIndexOfRowAtY(float y)
		{
			return this._FindIndexOfRowAtY(y, 0, this._rowHeights.Count - 1);
		}

		// Token: 0x06003AF7 RID: 15095 RVA: 0x00125514 File Offset: 0x00123714
		private int _FindIndexOfRowAtY(float y, int startIndex, int endIndex)
		{
			if (startIndex >= endIndex)
			{
				return startIndex;
			}
			int num = (startIndex + endIndex) / 2;
			if (this._GetCumulativeRowHeight(num) >= y)
			{
				return this._FindIndexOfRowAtY(y, startIndex, num);
			}
			return this._FindIndexOfRowAtY(y, num + 1, endIndex);
		}

		// Token: 0x06003AF8 RID: 15096 RVA: 0x0012554C File Offset: 0x0012374C
		private int _CellIndexFromRow(int row, int? rowCount = null)
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Bottom)
			{
				return (rowCount ?? this.DataSource.GetNumberOfCellsInTableView(this)) - row - 1;
			}
			return row;
		}

		// Token: 0x06003AF9 RID: 15097 RVA: 0x00125588 File Offset: 0x00123788
		private int _RowFromCellIndex(int index, int? rowCount = null)
		{
			if (this.ScrollingStartingOrder == TableView.ScrollAnchor.Bottom)
			{
				return (rowCount ?? this.DataSource.GetNumberOfCellsInTableView(this)) - index - 1;
			}
			return index;
		}

		// Token: 0x06003AFA RID: 15098 RVA: 0x001255C4 File Offset: 0x001237C4
		private float _GetCumulativeRowHeight(int row)
		{
			float num = 0f;
			for (int i = 0; i <= row; i++)
			{
				num += this._rowHeights[i];
			}
			return num;
		}

		// Token: 0x0400261F RID: 9759
		private const string _documentation = "A reusable table for for (vertical) tables. API inspired by Cocoa's UITableView.\nHierarchy structure should be:\nGameObject + TableView (this) + Mask + Scroll Rect (point to child)\n- Child GameObject + Vertical Layout Group";

		// Token: 0x04002620 RID: 9760
		private const string _kModuleName = "TableView";

		// Token: 0x04002625 RID: 9765
		private ITableViewDataSource _dataSource;

		// Token: 0x04002626 RID: 9766
		private int? _SelectedRow;

		// Token: 0x04002627 RID: 9767
		[SerializeField]
		private TableView.ScrollAnchor _scrollingStartingOrder;

		// Token: 0x04002628 RID: 9768
		private Action _scrollPositionSetter;

		// Token: 0x04002629 RID: 9769
		private float _scrollY;

		// Token: 0x0400262A RID: 9770
		[SerializeField]
		private float _stabilizationDelay = 0.15f;

		// Token: 0x0400262B RID: 9771
		private bool _requiresReload;

		// Token: 0x0400262C RID: 9772
		private VerticalLayoutGroup _verticalLayoutGroup;

		// Token: 0x0400262D RID: 9773
		private ScrollRect _scrollRect;

		// Token: 0x0400262E RID: 9774
		private LayoutElement _topContentPlaceholder;

		// Token: 0x0400262F RID: 9775
		private LayoutElement _bottomContentPlaceholder;

		// Token: 0x04002630 RID: 9776
		private List<float> _rowHeights;

		// Token: 0x04002631 RID: 9777
		private Dictionary<int, TableViewCell> _rowToVisibleCells;

		// Token: 0x04002632 RID: 9778
		private Range _visibleRowRange;

		// Token: 0x04002633 RID: 9779
		private RectTransform _reusableCellContainer;

		// Token: 0x04002634 RID: 9780
		private Dictionary<string, LinkedList<TableViewCell>> _reusableCells;

		// Token: 0x04002635 RID: 9781
		private bool _requiresRefresh;

		// Token: 0x04002636 RID: 9782
		private Coroutine _focusRoutine;

		// Token: 0x04002637 RID: 9783
		private Coroutine _stabilizationRoutine;

		// Token: 0x04002638 RID: 9784
		private Vector2 _previousPos;

		// Token: 0x02000940 RID: 2368
		public enum ScrollAnchor
		{
			// Token: 0x04003109 RID: 12553
			Top,
			// Token: 0x0400310A RID: 12554
			Bottom
		}

		// Token: 0x02000941 RID: 2369
		public enum CellAnchor
		{
			// Token: 0x0400310C RID: 12556
			Top,
			// Token: 0x0400310D RID: 12557
			Bottom,
			// Token: 0x0400310E RID: 12558
			Middle
		}
	}
}
