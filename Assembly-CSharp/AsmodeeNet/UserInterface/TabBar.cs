using System;
using System.Collections;
using System.Collections.Generic;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000633 RID: 1587
	public class TabBar : MonoBehaviour
	{
		// Token: 0x14000031 RID: 49
		// (add) Token: 0x06003A56 RID: 14934 RVA: 0x00121BC4 File Offset: 0x0011FDC4
		// (remove) Token: 0x06003A57 RID: 14935 RVA: 0x00121BFC File Offset: 0x0011FDFC
		public event Action<int> OnTabBarDidSelectItem;

		// Token: 0x06003A58 RID: 14936 RVA: 0x00121C34 File Offset: 0x0011FE34
		public void OnTabSelected(Tab tab)
		{
			this._selectedItemTag = this._tabBarItems.Find((TabBarItem item) => item.tab == tab).tag;
			Action<int> onTabBarDidSelectItem = this.OnTabBarDidSelectItem;
			if (onTabBarDidSelectItem == null)
			{
				return;
			}
			onTabBarDidSelectItem(this._selectedItemTag);
		}

		// Token: 0x170007CF RID: 1999
		// (get) Token: 0x06003A59 RID: 14937 RVA: 0x00121C86 File Offset: 0x0011FE86
		// (set) Token: 0x06003A5A RID: 14938 RVA: 0x00121C90 File Offset: 0x0011FE90
		public int SelectedItemTag
		{
			get
			{
				return this._selectedItemTag;
			}
			set
			{
				this._selectedItemTag = value;
				foreach (TabBarItem tabBarItem in this._tabBarItems)
				{
					tabBarItem.tab.IsOn = (tabBarItem.tag == this._selectedItemTag);
				}
			}
		}

		// Token: 0x170007D0 RID: 2000
		// (get) Token: 0x06003A5B RID: 14939 RVA: 0x00121CFC File Offset: 0x0011FEFC
		public RectTransform Background
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalBackground;
				}
				return this._horizontalBackground;
			}
		}

		// Token: 0x170007D1 RID: 2001
		// (get) Token: 0x06003A5C RID: 14940 RVA: 0x00121D13 File Offset: 0x0011FF13
		public RectTransform TabContainer
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalTabContainer;
				}
				return this._horizontalTabContainer;
			}
		}

		// Token: 0x170007D2 RID: 2002
		// (set) Token: 0x06003A5D RID: 14941 RVA: 0x00121D2A File Offset: 0x0011FF2A
		public List<TabBarItem> TabBarItems
		{
			set
			{
				if (!this._tabBarItems.Equals(value))
				{
					this._tabBarItems = value;
					this._selectedItemTag = -1;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x170007D3 RID: 2003
		// (get) Token: 0x06003A5E RID: 14942 RVA: 0x00121D4F File Offset: 0x0011FF4F
		// (set) Token: 0x06003A5F RID: 14943 RVA: 0x00121D57 File Offset: 0x0011FF57
		public TabBar.TabAlignment Alignment
		{
			get
			{
				return this._alignment;
			}
			set
			{
				if (this._alignment != value)
				{
					this._alignment = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x170007D4 RID: 2004
		// (get) Token: 0x06003A60 RID: 14944 RVA: 0x00121D70 File Offset: 0x0011FF70
		public float Thickness
		{
			get
			{
				if (!this.IsHorizontal)
				{
					return this._verticalBackground.rect.size.x;
				}
				return this._horizontalBackground.rect.size.y;
			}
		}

		// Token: 0x170007D5 RID: 2005
		// (get) Token: 0x06003A61 RID: 14945 RVA: 0x00121DB6 File Offset: 0x0011FFB6
		// (set) Token: 0x06003A62 RID: 14946 RVA: 0x00121DBE File Offset: 0x0011FFBE
		public bool IsHorizontal
		{
			get
			{
				return this._isHorizontal;
			}
			set
			{
				if (this._isHorizontal != value)
				{
					this._isHorizontal = value;
					this._needsUpdate = true;
				}
			}
		}

		// Token: 0x06003A63 RID: 14947 RVA: 0x00121DD8 File Offset: 0x0011FFD8
		public void UpdateTabs()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			List<Transform> list = new List<Transform>(this._horizontalTabContainer.childCount + this._verticalTabContainer.childCount);
			foreach (Transform transform in new List<Transform>
			{
				this._horizontalTabContainer,
				this._verticalTabContainer
			})
			{
				using (IEnumerator enumerator2 = ((RectTransform)transform).GetEnumerator())
				{
					while (enumerator2.MoveNext())
					{
						RectTransform tabTransform = (RectTransform)enumerator2.Current;
						if (this._tabBarItems.Find((TabBarItem item) => item.transform == tabTransform).tab == null)
						{
							list.Add(tabTransform);
						}
					}
				}
			}
			foreach (Transform transform2 in list)
			{
				RectTransform rectTransform = (RectTransform)transform2;
				rectTransform.SetParent(null);
				UnityEngine.Object.Destroy(rectTransform.gameObject);
			}
			foreach (TabBarItem tabBarItem in this._tabBarItems)
			{
				if (tabBarItem.transform.parent != this.TabContainer)
				{
					tabBarItem.tab.OnTabSelected += this.OnTabSelected;
					tabBarItem.transform.SetParent(this.TabContainer, false);
				}
			}
			if (base.enabled)
			{
				this._needsUpdate = false;
				if (this.IsHorizontal)
				{
					this._horizontalBackground.gameObject.SetActive(true);
					this._verticalBackground.gameObject.SetActive(false);
				}
				else
				{
					this._horizontalBackground.gameObject.SetActive(false);
					this._verticalBackground.gameObject.SetActive(true);
				}
				float num = 0f;
				int num2 = 0;
				foreach (TabBarItem ptr in this._tabBarItems)
				{
					num2++;
					RectTransform transform3 = ptr.transform;
					float num3 = this.IsHorizontal ? (this.Thickness / transform3.rect.size.y) : (this.Thickness / transform3.rect.size.x);
					transform3.localScale = new Vector3(num3, num3, num3);
					if (this.IsHorizontal)
					{
						if ((this.Alignment & TabBar.TabAlignment.Left) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Right) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = Vector2.zero);
							transform3.pivot = Vector2.zero;
							transform3.anchoredPosition = new Vector2(num, 0f);
							num += transform3.rect.size.x * num3;
						}
						else if ((this.Alignment & TabBar.TabAlignment.Right) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Left) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = new Vector2(1f, 0f));
							transform3.pivot = new Vector2(1f, 0f);
							transform3.anchoredPosition = new Vector2(num, 0f);
							num -= transform3.rect.size.x * num3;
						}
						else
						{
							float x = (float)num2 / ((float)this._tabBarItems.Count + 1f);
							transform3.anchorMin = new Vector2(x, 0.5f);
							transform3.anchorMax = new Vector2(x, 0.5f);
							transform3.pivot = new Vector2(0.5f, 0.5f);
							transform3.anchoredPosition = Vector2.zero;
						}
					}
					else
					{
						transform3.anchorMin = (transform3.anchorMax = Vector2.one);
						if ((this.Alignment & TabBar.TabAlignment.Top) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Bottom) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = Vector2.one);
							transform3.pivot = Vector2.one;
							transform3.anchoredPosition = new Vector2(0f, num);
							num -= transform3.rect.size.y * num3;
						}
						else if ((this.Alignment & TabBar.TabAlignment.Bottom) != TabBar.TabAlignment.Unknown && (this.Alignment & TabBar.TabAlignment.Top) == TabBar.TabAlignment.Unknown)
						{
							transform3.anchorMin = (transform3.anchorMax = new Vector2(1f, 0f));
							transform3.pivot = new Vector2(1f, 0f);
							transform3.anchoredPosition = new Vector2(0f, num);
							num += transform3.rect.size.y * num3;
						}
						else
						{
							float y = (float)num2 / ((float)this._tabBarItems.Count + 1f);
							transform3.anchorMin = new Vector2(0.5f, y);
							transform3.anchorMax = new Vector2(0.5f, y);
							transform3.pivot = new Vector2(0.5f, 0.5f);
							transform3.anchoredPosition = Vector2.zero;
						}
					}
				}
			}
		}

		// Token: 0x040025B8 RID: 9656
		private int _selectedItemTag = -1;

		// Token: 0x040025B9 RID: 9657
		public ToggleGroup toggleGroup;

		// Token: 0x040025BA RID: 9658
		[SerializeField]
		private RectTransform _horizontalBackground;

		// Token: 0x040025BB RID: 9659
		[SerializeField]
		private RectTransform _verticalBackground;

		// Token: 0x040025BC RID: 9660
		[SerializeField]
		private RectTransform _horizontalTabContainer;

		// Token: 0x040025BD RID: 9661
		[SerializeField]
		private RectTransform _verticalTabContainer;

		// Token: 0x040025BE RID: 9662
		private List<TabBarItem> _tabBarItems = new List<TabBarItem>();

		// Token: 0x040025BF RID: 9663
		[SerializeField]
		[EnumFlag]
		private TabBar.TabAlignment _alignment = TabBar.TabAlignment.Distributed;

		// Token: 0x040025C0 RID: 9664
		private bool _isHorizontal = true;

		// Token: 0x040025C1 RID: 9665
		private bool _needsUpdate;

		// Token: 0x02000925 RID: 2341
		[Flags]
		public enum TabAlignment
		{
			// Token: 0x040030B9 RID: 12473
			Unknown = 0,
			// Token: 0x040030BA RID: 12474
			Left = 1,
			// Token: 0x040030BB RID: 12475
			Right = 2,
			// Token: 0x040030BC RID: 12476
			Top = 4,
			// Token: 0x040030BD RID: 12477
			Bottom = 8,
			// Token: 0x040030BE RID: 12478
			Distributed = 15
		}
	}
}
