using System;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x02000623 RID: 1571
	public class CommunityHubLayout : MonoBehaviour
	{
		// Token: 0x170007BD RID: 1981
		// (get) Token: 0x060039E1 RID: 14817 RVA: 0x0011F32D File Offset: 0x0011D52D
		public Vector2 SafeAreaAnchorMin
		{
			get
			{
				if (this._layoutContext == null)
				{
					return Vector2.zero;
				}
				return this._layoutContext.gameContentSafeAreaAnchorMin;
			}
		}

		// Token: 0x170007BE RID: 1982
		// (get) Token: 0x060039E2 RID: 14818 RVA: 0x0011F348 File Offset: 0x0011D548
		public Vector2 SafeAreaAnchorMax
		{
			get
			{
				if (this._layoutContext == null)
				{
					return Vector2.one;
				}
				return this._layoutContext.gameContentSafeAreaAnchorMax;
			}
		}

		// Token: 0x060039E3 RID: 14819 RVA: 0x0011F364 File Offset: 0x0011D564
		private void OnEnable()
		{
			this._needsUpdate = true;
			this.IsCollapsed = this._IsCollapsible;
			CoreApplication.Instance.Preferences.InterfaceOrientationDidChange += this._InterfaceOrientationDidChange;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this._InterfaceDisplayModeDidChange;
		}

		// Token: 0x060039E4 RID: 14820 RVA: 0x0011F3BA File Offset: 0x0011D5BA
		private void Update()
		{
			this.UpdateLayout(false);
			if (!base.enabled)
			{
				return;
			}
			this._tabBar.UpdateTabs();
			this._UpdateTransitions();
		}

		// Token: 0x060039E5 RID: 14821 RVA: 0x0011F3E0 File Offset: 0x0011D5E0
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.InterfaceOrientationDidChange -= this._InterfaceOrientationDidChange;
				CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this._InterfaceDisplayModeDidChange;
				this.UpdateLayout(true);
			}
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x0011F431 File Offset: 0x0011D631
		public void SetNeedsUpdateLayout()
		{
			this._needsUpdate = true;
		}

		// Token: 0x170007BF RID: 1983
		// (get) Token: 0x060039E7 RID: 14823 RVA: 0x0011F43A File Offset: 0x0011D63A
		// (set) Token: 0x060039E8 RID: 14824 RVA: 0x0011F442 File Offset: 0x0011D642
		public bool IsCollapsed
		{
			get
			{
				return this._collapsed;
			}
			set
			{
				this._collapsed = value;
				if (this._expandCollapseButton != null)
				{
					this._expandCollapseButton.IsCollapsed = this._collapsed;
				}
				this._needsUpdate = true;
			}
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x0011F471 File Offset: 0x0011D671
		public void SetIsCollapsed(bool isCollapsed, bool animated)
		{
			this._animated = animated;
			this.IsCollapsed = isCollapsed;
		}

		// Token: 0x170007C0 RID: 1984
		// (get) Token: 0x060039EA RID: 14826 RVA: 0x0011F481 File Offset: 0x0011D681
		// (set) Token: 0x060039EB RID: 14827 RVA: 0x0011F489 File Offset: 0x0011D689
		public bool HandleSafeArea
		{
			get
			{
				return this._handleSafeArea;
			}
			set
			{
				this._handleSafeArea = value;
				this._needsUpdate = true;
			}
		}

		// Token: 0x170007C1 RID: 1985
		// (get) Token: 0x060039EC RID: 14828 RVA: 0x0011F49C File Offset: 0x0011D69C
		private bool _IsCollapsible
		{
			get
			{
				return CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small || (!MathUtils.Approximately(this._aspect, this.regularAspectRatioLimitVertical, 0.01f) && !MathUtils.Approximately(this._aspect, this.regularAspectRatioLimitHorizontal, 0.01f) && this.regularAspectRatioLimitVertical + 0.01f < this._aspect && this._aspect < this.regularAspectRatioLimitHorizontal - 0.01f);
			}
		}

		// Token: 0x170007C2 RID: 1986
		// (get) Token: 0x060039ED RID: 14829 RVA: 0x0011F518 File Offset: 0x0011D718
		private bool _DisplayTabBarWhileCollapsed
		{
			get
			{
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
					{
						float num = this._tabBar.Thickness / this.container.rect.width;
						if (this._aspect * (1f - num) > this.compactAspectRatioLimitHorizontal)
						{
							return true;
						}
					}
					else
					{
						float num2 = this._tabBar.Thickness / this.container.rect.height;
						if (this._aspect / (1f - num2) < this.compactAspectRatioLimitVertical)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x060039EE RID: 14830 RVA: 0x0011F5C0 File Offset: 0x0011D7C0
		public void UpdateLayout(bool force = false)
		{
			Preferences.DisplayMode interfaceDisplayMode = CoreApplication.Instance.Preferences.InterfaceDisplayMode;
			bool flag = this._displayMode != interfaceDisplayMode;
			this._displayMode = interfaceDisplayMode;
			if (flag)
			{
				this._DestroyExpandCollapseButton();
				this._DestroyTabBar();
				this._CreateExpandCollapseButton();
				this._CreateTabBar();
			}
			float num = this._handleSafeArea ? (Screen.safeArea.width / Screen.safeArea.height) : CoreApplication.Instance.Preferences.Aspect;
			if (!MathUtils.Approximately(num, this._aspect, 0.01f))
			{
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small)
				{
					float num2 = this.regularAspectRatioLimitHorizontal - 0.01f;
					float num3 = this.regularAspectRatioLimitVertical + 0.01f;
					if ((this._aspect >= num2 && num < num2) || (num >= num2 && this._aspect < num2) || (this._aspect <= num3 && num > num3) || (num <= num3 && this._aspect > num3))
					{
						this._needsUpdate = true;
					}
				}
				else if (this._layoutContext != null)
				{
					if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
					{
						float num4 = this._tabBar.Thickness / this.container.rect.width;
						float num5 = this._aspect * (1f - num4);
						float num6 = num * (1f - num4);
						if ((num5 >= this.compactAspectRatioLimitHorizontal && num6 < this.compactAspectRatioLimitHorizontal) || (num5 < this.compactAspectRatioLimitHorizontal && num6 >= this.compactAspectRatioLimitHorizontal))
						{
							this._needsUpdate = true;
						}
					}
					else
					{
						float num7 = this._tabBar.Thickness / this.container.rect.height;
						float num8 = this._aspect / (1f - num7);
						float num9 = num / (1f - num7);
						if ((num8 >= this.compactAspectRatioLimitVertical && num9 < this.compactAspectRatioLimitVertical) || (num8 < this.compactAspectRatioLimitVertical && num9 >= this.compactAspectRatioLimitVertical))
						{
							this._needsUpdate = true;
						}
					}
				}
				this._aspect = num;
			}
			if (this._handleSafeArea)
			{
				Rect safeArea = Screen.safeArea;
				float num10 = 1f / (float)Screen.width;
				float num11 = 1f / (float)Screen.height;
				Vector2 vector = new Vector2(safeArea.x * num10, safeArea.y * num11);
				Vector2 vector2 = new Vector2((safeArea.x + safeArea.width) * num10, (safeArea.y + safeArea.height) * num11);
				if (!MathUtils.Approximately(vector, this._SafeAreaAnchorMin, 0.01f) || !MathUtils.Approximately(vector2, this._SafeAreaAnchorMax, 0.01f))
				{
					this._needsUpdate = true;
					this._SafeAreaAnchorMin = vector;
					this._SafeAreaAnchorMax = vector2;
				}
			}
			if (this._needsUpdate || force)
			{
				if (!this._IsCollapsible)
				{
					this.IsCollapsed = false;
				}
				if (MathUtils.Approximately(this.container.rect.size, this._containerSize, 1f))
				{
					this._needsUpdate = false;
				}
				this._containerSize = this.container.rect.size;
				this._layoutContext = new CommunityHubLayout.LayoutContext();
				this._layoutContext.tabsAnchorMin = this._SafeAreaAnchorMin;
				this._layoutContext.tabsAnchorMax = this._SafeAreaAnchorMax;
				this._UpdateContentParenthood(flag);
				this._UpdateContainerLayout();
				if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
				{
					this._UpdateHeadersLayout();
					this._UpdateTabsLayout();
					this._UpdateExpandCollapseButtonLayout();
				}
				else
				{
					this._UpdateTabsLayout();
					this._UpdateExpandCollapseButtonLayout();
					this._UpdateHeadersLayout();
				}
				this._UpdateAutoLayoutTransforms();
				CoreApplication.Instance.CommunityHub.CallLayoutDidChangeEvent();
			}
			this._animated = false;
		}

		// Token: 0x060039EF RID: 14831 RVA: 0x0011F978 File Offset: 0x0011DB78
		private void _UpdateContentParenthood(bool forceReload)
		{
			CommunityHub communityHub = CoreApplication.Instance.CommunityHub;
			foreach (object obj in this.container)
			{
				RectTransform rectTransform = (RectTransform)obj;
				bool flag = true;
				if (!forceReload)
				{
					foreach (CommunityHubContent communityHubContent in communityHub.CommunityHubContents)
					{
						if (rectTransform == communityHubContent.DisplayedContent)
						{
							flag = false;
						}
					}
				}
				if (rectTransform.transform == this._expandCollapseButtonRoot.transform || rectTransform.transform == this._tabBarRoot.transform)
				{
					flag = false;
				}
				if (flag)
				{
					rectTransform.SetParent(null);
					this._transformToContent[rectTransform].DisposeDisplayedContent();
					this._transformToContent.Remove(rectTransform);
				}
			}
			foreach (CommunityHubContent communityHubContent2 in communityHub.CommunityHubContents)
			{
				communityHubContent2.LoadDisplayedContent(this._displayMode);
				RectTransform displayedContent = communityHubContent2.DisplayedContent;
				if (displayedContent != null && displayedContent.parent != this.container)
				{
					displayedContent.SetParent(this.container, false);
					this._transformToContent.Add(displayedContent, communityHubContent2);
				}
			}
		}

		// Token: 0x060039F0 RID: 14832 RVA: 0x0011FB1C File Offset: 0x0011DD1C
		private void _UpdateContainerLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			this._layoutContext.gameContentSafeAreaAnchorMin = this._SafeAreaAnchorMin;
			this._layoutContext.gameContentSafeAreaAnchorMax = this._SafeAreaAnchorMax;
			if (list.Count > 0)
			{
				Preferences.Orientation interfaceOrientation = CoreApplication.Instance.Preferences.InterfaceOrientation;
				if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
				{
					if (interfaceOrientation == Preferences.Orientation.Horizontal)
					{
						float num = this._tabBar.Thickness / this.container.rect.width;
						if (this._aspect * (1f - num) > this.compactAspectRatioLimitHorizontal)
						{
							this._layoutContext.gameContentSafeAreaAnchorMax = new Vector2(this._SafeAreaAnchorMax.x - num, this._SafeAreaAnchorMax.y);
							return;
						}
					}
					else
					{
						float num2 = this._tabBar.Thickness / this.container.rect.height;
						if (this._aspect / (1f - num2) < this.compactAspectRatioLimitVertical)
						{
							this._layoutContext.gameContentSafeAreaAnchorMax = new Vector2(this._SafeAreaAnchorMax.x, this._SafeAreaAnchorMax.y - num2);
							return;
						}
					}
				}
				else if (!this._IsCollapsible)
				{
					if (interfaceOrientation == Preferences.Orientation.Horizontal)
					{
						this._layoutContext.gameContentSafeAreaAnchorMax = new Vector2((this._SafeAreaAnchorMax.x - this._SafeAreaAnchorMin.x) * 0.75f, this._SafeAreaAnchorMax.y);
						return;
					}
					this._layoutContext.gameContentSafeAreaAnchorMax = new Vector2(this._SafeAreaAnchorMax.x, (this._SafeAreaAnchorMax.y - this._SafeAreaAnchorMin.y) * 0.75f);
				}
			}
		}

		// Token: 0x060039F1 RID: 14833 RVA: 0x0011FCD0 File Offset: 0x0011DED0
		private void _UpdateHeadersLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Header);
			List<CommunityHubContent> list2 = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			Vector2 safeAreaAnchorMax = this._SafeAreaAnchorMax;
			if (list2.Count > 0 && CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Vertical && CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small && (!this._IsCollapsible || !this.IsCollapsed))
			{
				safeAreaAnchorMax = new Vector2(this._SafeAreaAnchorMax.x, this._layoutContext.tabsAnchorMin.y);
			}
			Vector3 vector = new Vector3(-this._layoutContext.expandCollapseButtonWidth, this._layoutContext.headersStartY, 0f);
			foreach (CommunityHubContent communityHubContent in list)
			{
				RectTransform displayedContent = communityHubContent.DisplayedContent;
				displayedContent.anchorMin = safeAreaAnchorMax;
				displayedContent.anchorMax = safeAreaAnchorMax;
				displayedContent.anchoredPosition = vector;
				vector.y -= displayedContent.rect.size.y * displayedContent.localScale.y;
			}
			this._layoutContext.headersHeight = -vector.y;
		}

		// Token: 0x060039F2 RID: 14834 RVA: 0x0011FE14 File Offset: 0x0011E014
		private void _UpdateTabsLayout()
		{
			if (!base.enabled)
			{
				return;
			}
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			if (list.Count <= 0)
			{
				this._SelectedContentInTabs = null;
				this._tabBar.TabBarItems = new List<TabBarItem>();
				this._tabBarRoot.SetActive(false);
				return;
			}
			Preferences.Orientation interfaceOrientation = CoreApplication.Instance.Preferences.InterfaceOrientation;
			int num = (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small) ? 1 : 2;
			bool flag = list.Count >= num && (!this.IsCollapsed || (this.IsCollapsed && this._DisplayTabBarWhileCollapsed));
			this._tabBarRoot.SetActive(flag);
			CommunityHubSkin skin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			RectTransform rectTransform = (RectTransform)this._tabBarRoot.transform;
			if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small || this.IsCollapsed)
			{
				this._layoutContext.tabsOffsetMax = new Vector2(0f, -this._layoutContext.headersHeight);
			}
			if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
			{
				if (interfaceOrientation == Preferences.Orientation.Horizontal)
				{
					if (this.IsCollapsed)
					{
						this._layoutContext.tabsAnchorMin = new Vector2(1f, this._SafeAreaAnchorMin.y);
						this._layoutContext.tabsAnchorMax = new Vector2(this._SafeAreaAnchorMax.x - this._SafeAreaAnchorMin.x + 1f, this._SafeAreaAnchorMax.y);
					}
					if (flag)
					{
						this._tabBar.IsHorizontal = false;
						float thickness = this._tabBar.Thickness;
						rectTransform.anchorMin = new Vector2(this._SafeAreaAnchorMax.x, this._SafeAreaAnchorMin.y);
						rectTransform.anchorMax = new Vector2(this._SafeAreaAnchorMax.x, this._SafeAreaAnchorMax.y);
						rectTransform.offsetMin = new Vector2(-thickness, 0f);
						rectTransform.offsetMax = this._layoutContext.tabsOffsetMax;
						if (!this.IsCollapsed)
						{
							CommunityHubLayout.LayoutContext layoutContext = this._layoutContext;
							layoutContext.tabsOffsetMax.x = layoutContext.tabsOffsetMax.x - thickness;
						}
					}
				}
				else
				{
					if (this.IsCollapsed)
					{
						this._layoutContext.tabsAnchorMin = new Vector2(this._SafeAreaAnchorMin.x, 1f);
						this._layoutContext.tabsAnchorMax = new Vector2(this._SafeAreaAnchorMax.x, this._SafeAreaAnchorMax.y - this._SafeAreaAnchorMin.y + 1f);
					}
					if (flag)
					{
						this._tabBar.IsHorizontal = true;
						float thickness2 = this._tabBar.Thickness;
						rectTransform.anchorMin = new Vector2(this._SafeAreaAnchorMin.x, this._SafeAreaAnchorMax.y);
						rectTransform.anchorMax = this._SafeAreaAnchorMax;
						rectTransform.offsetMin = Vector2.zero;
						rectTransform.offsetMax = new Vector2(0f, -this._layoutContext.headersHeight);
						this._layoutContext.headersStartY = -thickness2;
						if (!this.IsCollapsed)
						{
							CommunityHubLayout.LayoutContext layoutContext2 = this._layoutContext;
							layoutContext2.tabsOffsetMax.y = layoutContext2.tabsOffsetMax.y - thickness2;
						}
					}
				}
			}
			else if (interfaceOrientation == Preferences.Orientation.Horizontal)
			{
				if (this.IsCollapsed && this._IsCollapsible)
				{
					this._layoutContext.tabsAnchorMin = new Vector2(1f, this._SafeAreaAnchorMin.y);
					this._layoutContext.tabsAnchorMax = new Vector2(1.25f, this._SafeAreaAnchorMax.y);
				}
				else
				{
					this._layoutContext.tabsAnchorMin = new Vector2((this._SafeAreaAnchorMax.x - this._SafeAreaAnchorMin.x) * 0.75f, this._SafeAreaAnchorMin.y);
					this._layoutContext.tabsAnchorMax = this._SafeAreaAnchorMax;
				}
				if (flag)
				{
					this._tabBar.IsHorizontal = true;
					float thickness3 = this._tabBar.Thickness;
					rectTransform.anchorMin = new Vector2(this._layoutContext.tabsAnchorMin.x, this._layoutContext.tabsAnchorMax.y);
					rectTransform.anchorMax = this._layoutContext.tabsAnchorMax;
					rectTransform.offsetMin = new Vector2(0f, -thickness3);
					rectTransform.offsetMax = new Vector2(0f, -this._layoutContext.headersHeight);
					CommunityHubLayout.LayoutContext layoutContext3 = this._layoutContext;
					layoutContext3.tabsOffsetMax.y = layoutContext3.tabsOffsetMax.y - thickness3;
				}
			}
			else
			{
				if (this.IsCollapsed && this._IsCollapsible)
				{
					this._layoutContext.tabsAnchorMin = new Vector2(this._SafeAreaAnchorMin.x, 1f);
					this._layoutContext.tabsAnchorMax = new Vector2(this._SafeAreaAnchorMax.x, 1.25f);
				}
				else
				{
					this._layoutContext.tabsAnchorMin = new Vector2(this._SafeAreaAnchorMin.x, (this._SafeAreaAnchorMax.y - this._SafeAreaAnchorMin.y) * 0.75f);
					this._layoutContext.tabsAnchorMax = this._SafeAreaAnchorMax;
				}
				if (flag)
				{
					this._tabBar.IsHorizontal = false;
					float thickness4 = this._tabBar.Thickness;
					rectTransform.anchorMin = new Vector2(this._layoutContext.tabsAnchorMax.x, this._layoutContext.tabsAnchorMin.y);
					rectTransform.anchorMax = this._layoutContext.tabsAnchorMax;
					rectTransform.offsetMin = new Vector2(-thickness4, 0f);
					rectTransform.offsetMax = new Vector2(0f, -this._layoutContext.headersHeight);
					CommunityHubLayout.LayoutContext layoutContext4 = this._layoutContext;
					layoutContext4.tabsOffsetMax.x = layoutContext4.tabsOffsetMax.x - thickness4;
				}
			}
			if (!this._animated)
			{
				foreach (CommunityHubContent communityHubContent in list)
				{
					RectTransform displayedContent = communityHubContent.DisplayedContent;
					displayedContent.anchorMin = this._layoutContext.tabsAnchorMin;
					displayedContent.anchorMax = this._layoutContext.tabsAnchorMax;
					displayedContent.offsetMin = this._layoutContext.tabsOffsetMin;
					displayedContent.offsetMax = this._layoutContext.tabsOffsetMax;
				}
			}
			List<TabBarItem> list2 = new List<TabBarItem>();
			Func<CommunityHubSkin.TabIcons, int, TabBarItem> func = delegate(CommunityHubSkin.TabIcons tabIcons, int tag)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(skin.tabPrefabs[this._displayMode]);
				Tab component = gameObject.GetComponent<Tab>();
				RectTransform transform = gameObject.transform as RectTransform;
				component.Setup(tabIcons, this._tabBar.toggleGroup, this._tabBar.IsHorizontal);
				return new TabBarItem(tag, component, transform);
			};
			int i;
			for (i = 0; i < list.Count; i++)
			{
				CommunityHubContent communityHubContent2 = list[i];
				list2.Add(func(communityHubContent2.TabIcons, i));
			}
			if (CoreApplication.Instance.Preferences.InterfaceDisplayMode == Preferences.DisplayMode.Small)
			{
				Sprite spriteOff = (interfaceOrientation == Preferences.Orientation.Horizontal) ? (this.IsCollapsed ? skin.expandButtonInVerticalTabBarIcon : skin.collapseButtonInVerticalTabBarIcon) : (this.IsCollapsed ? skin.expandButtonInHorizontalTabBarIcon : skin.collapseButtonInHorizontalTabBarIcon);
				CommunityHubSkin.TabIcons arg = new CommunityHubSkin.TabIcons
				{
					spriteOff = spriteOff
				};
				list2.Insert(0, func(arg, i + 1));
			}
			this._tabBar.TabBarItems = list2;
			if (this._SelectedContentInTabs == null || !list.Contains(this._SelectedContentInTabs))
			{
				this._SelectedContentInTabs = list[0];
				this._tabBar.SelectedItemTag = 0;
				return;
			}
			this._SelectedContentInTabs = this._SelectedContentInTabs;
			this._tabBar.SelectedItemTag = list.IndexOf(this._SelectedContentInTabs);
		}

		// Token: 0x060039F3 RID: 14835 RVA: 0x001205AC File Offset: 0x0011E7AC
		private void _UpdateExpandCollapseButtonLayout()
		{
			if (this._ContentsWithLayout(CommunityHubContent.Layout.Tab).Count <= 0 || !this._IsCollapsible)
			{
				this._expandCollapseButtonRoot.SetActive(false);
				return;
			}
			if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small || (this.IsCollapsed && !this._DisplayTabBarWhileCollapsed))
			{
				this._expandCollapseButtonRoot.SetActive(true);
				Vector2 vector;
				if (CoreApplication.Instance.Preferences.InterfaceOrientation == Preferences.Orientation.Horizontal)
				{
					vector.x = this._layoutContext.tabsAnchorMin.x;
					vector.y = this._layoutContext.tabsAnchorMax.y;
				}
				else
				{
					vector.x = this._layoutContext.tabsAnchorMax.x;
					vector.y = this._layoutContext.tabsAnchorMin.y;
				}
				RectTransform rectTransform = (RectTransform)this._expandCollapseButtonRoot.transform;
				rectTransform.anchorMin = vector;
				rectTransform.anchorMax = vector;
				rectTransform.pivot = Vector2.one;
				rectTransform.anchoredPosition = new Vector2(0f, -this._layoutContext.headersHeight);
				this._layoutContext.expandCollapseButtonWidth = rectTransform.rect.size.x;
				return;
			}
			this._expandCollapseButtonRoot.SetActive(false);
		}

		// Token: 0x060039F4 RID: 14836 RVA: 0x001206F8 File Offset: 0x0011E8F8
		private void _UpdateAutoLayoutTransforms()
		{
			foreach (RectTransform rectTransform in CoreApplication.Instance.CommunityHub.TransformsToAutoLayout)
			{
				rectTransform.anchorMin = this._layoutContext.gameContentSafeAreaAnchorMin;
				rectTransform.anchorMax = this._layoutContext.gameContentSafeAreaAnchorMax;
			}
		}

		// Token: 0x060039F5 RID: 14837 RVA: 0x00120768 File Offset: 0x0011E968
		private List<CommunityHubContent> _ContentsWithLayout(CommunityHubContent.Layout layout)
		{
			return (from content in CoreApplication.Instance.CommunityHub.CommunityHubContents
			where content.DisplayedLayout == layout
			select content).ToList<CommunityHubContent>();
		}

		// Token: 0x170007C3 RID: 1987
		// (get) Token: 0x060039F6 RID: 14838 RVA: 0x001207A7 File Offset: 0x0011E9A7
		// (set) Token: 0x060039F7 RID: 14839 RVA: 0x001207B0 File Offset: 0x0011E9B0
		public CommunityHubContent _SelectedContentInTabs
		{
			get
			{
				return this._selectedContentInTabs;
			}
			set
			{
				this._selectedContentInTabs = value;
				if (this._SelectedContentInTabs != null)
				{
					this._SelectedContentInTabs.DisplayedContent.SetAsLastSibling();
					this._tabBarRoot.transform.SetAsLastSibling();
					if (CoreApplication.Instance.Preferences.InterfaceDisplayMode != Preferences.DisplayMode.Small)
					{
						foreach (CommunityHubContent communityHubContent in this._ContentsWithLayout(CommunityHubContent.Layout.Header))
						{
							communityHubContent.DisplayedContent.SetAsLastSibling();
						}
					}
				}
				foreach (CommunityHubContent communityHubContent2 in this._ContentsWithLayout(CommunityHubContent.Layout.Tab))
				{
					if (communityHubContent2 == this._SelectedContentInTabs)
					{
						communityHubContent2.DisplayedContent.gameObject.SetActive(true);
					}
					else if (communityHubContent2.BackgroundActivity == CommunityHubContent.ActivityType.Inactive)
					{
						communityHubContent2.DisplayedContent.gameObject.SetActive(false);
					}
				}
			}
		}

		// Token: 0x060039F8 RID: 14840 RVA: 0x0011F431 File Offset: 0x0011D631
		private void _InterfaceOrientationDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x060039F9 RID: 14841 RVA: 0x0011F431 File Offset: 0x0011D631
		private void _InterfaceDisplayModeDidChange()
		{
			this._needsUpdate = true;
		}

		// Token: 0x060039FA RID: 14842 RVA: 0x001208BC File Offset: 0x0011EABC
		private void _UpdateTransitions()
		{
			float t = Time.deltaTime * 10f;
			foreach (CommunityHubContent communityHubContent in this._ContentsWithLayout(CommunityHubContent.Layout.Tab))
			{
				RectTransform displayedContent = communityHubContent.DisplayedContent;
				displayedContent.anchorMin = Vector2.Lerp(displayedContent.anchorMin, this._layoutContext.tabsAnchorMin, t);
				displayedContent.anchorMax = Vector2.Lerp(displayedContent.anchorMax, this._layoutContext.tabsAnchorMax, t);
				displayedContent.offsetMin = Vector2.Lerp(displayedContent.offsetMin, this._layoutContext.tabsOffsetMin, t);
				displayedContent.offsetMax = Vector2.Lerp(displayedContent.offsetMax, this._layoutContext.tabsOffsetMax, t);
			}
		}

		// Token: 0x060039FB RID: 14843 RVA: 0x00120990 File Offset: 0x0011EB90
		private void _CreateExpandCollapseButton()
		{
			if (this._expandCollapseButtonRoot != null || this._expandCollapseButton != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._expandCollapseButtonRoot = UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.expandCollapseButtonPrefabs[this._displayMode]);
			this._expandCollapseButton = this._expandCollapseButtonRoot.GetComponent<ExpandCollapseButton>();
			if (this._expandCollapseButtonRoot == null || this._expandCollapseButton == null)
			{
				AsmoLogger.Debug("CommunityHubLayout", "Incomplete ExpandCollapseButton Prefab", null);
				this._DestroyExpandCollapseButton();
			}
			this._expandCollapseButton.IsCollapsed = this._collapsed;
			this._expandCollapseButton.onExpandCollapseButtonClicked += this._OnExpandCollapseButtonClicked;
			this._expandCollapseButtonRoot.transform.SetParent(this.container, false);
		}

		// Token: 0x060039FC RID: 14844 RVA: 0x00120A68 File Offset: 0x0011EC68
		private void _DestroyExpandCollapseButton()
		{
			if (this._expandCollapseButtonRoot != null)
			{
				this._expandCollapseButtonRoot.transform.SetParent(null);
				UnityEngine.Object.Destroy(this._expandCollapseButtonRoot);
				this._expandCollapseButtonRoot = null;
				this._expandCollapseButton = null;
			}
		}

		// Token: 0x060039FD RID: 14845 RVA: 0x00120AA2 File Offset: 0x0011ECA2
		private void _OnExpandCollapseButtonClicked()
		{
			this.SetIsCollapsed(!this.IsCollapsed, true);
		}

		// Token: 0x170007C4 RID: 1988
		// (get) Token: 0x060039FE RID: 14846 RVA: 0x00120AB4 File Offset: 0x0011ECB4
		// (set) Token: 0x060039FF RID: 14847 RVA: 0x00120ABC File Offset: 0x0011ECBC
		public TabBar.TabAlignment TabBarAlignment
		{
			get
			{
				return this._tabBarAlignment;
			}
			set
			{
				this._tabBarAlignment = value;
				if (this._tabBar != null)
				{
					this._tabBar.Alignment = this._tabBarAlignment;
				}
			}
		}

		// Token: 0x06003A00 RID: 14848 RVA: 0x00120AE4 File Offset: 0x0011ECE4
		private void _CreateTabBar()
		{
			if (this._tabBarRoot != null || this._tabBar != null)
			{
				return;
			}
			CommunityHubSkin communityHubSkin = CoreApplication.Instance.CommunityHubLauncher.communityHubSkin;
			this._tabBarRoot = UnityEngine.Object.Instantiate<GameObject>(communityHubSkin.tabBarPrefabs[this._displayMode]);
			this._tabBar = this._tabBarRoot.GetComponent<TabBar>();
			if (this._tabBarRoot == null || this._tabBar == null)
			{
				AsmoLogger.Debug("CommunityHubLayout", "Incomplete TabBar Prefab", null);
				this._DestroyTabBar();
			}
			if (this.TabBarAlignment == TabBar.TabAlignment.Unknown)
			{
				this.TabBarAlignment = this._tabBar.Alignment;
			}
			else
			{
				this._tabBar.Alignment = this.TabBarAlignment;
			}
			this._tabBar.OnTabBarDidSelectItem += this._OnTabBarDidSelectItem;
			this._tabBarRoot.transform.SetParent(this.container, false);
		}

		// Token: 0x06003A01 RID: 14849 RVA: 0x00120BD7 File Offset: 0x0011EDD7
		private void _DestroyTabBar()
		{
			if (this._tabBarRoot != null)
			{
				this._tabBarRoot.transform.SetParent(null);
				UnityEngine.Object.Destroy(this._tabBarRoot);
				this._tabBarRoot = null;
				this._tabBar = null;
			}
		}

		// Token: 0x06003A02 RID: 14850 RVA: 0x00120C14 File Offset: 0x0011EE14
		private void _OnTabBarDidSelectItem(int tag)
		{
			List<CommunityHubContent> list = this._ContentsWithLayout(CommunityHubContent.Layout.Tab);
			if (tag < list.Count)
			{
				this._SelectedContentInTabs = list[tag];
				if (this.IsCollapsed)
				{
					this._OnExpandCollapseButtonClicked();
					return;
				}
			}
			else
			{
				this._OnExpandCollapseButtonClicked();
			}
		}

		// Token: 0x0400255E RID: 9566
		private const string _debugModuleName = "CommunityHubLayout";

		// Token: 0x0400255F RID: 9567
		public RectTransform container;

		// Token: 0x04002560 RID: 9568
		private CommunityHubLayout.LayoutContext _layoutContext;

		// Token: 0x04002561 RID: 9569
		private Dictionary<RectTransform, CommunityHubContent> _transformToContent = new Dictionary<RectTransform, CommunityHubContent>();

		// Token: 0x04002562 RID: 9570
		private bool _needsUpdate;

		// Token: 0x04002563 RID: 9571
		private bool _animated;

		// Token: 0x04002564 RID: 9572
		private bool _collapsed;

		// Token: 0x04002565 RID: 9573
		private bool _handleSafeArea = true;

		// Token: 0x04002566 RID: 9574
		private Vector2 _SafeAreaAnchorMin = Vector2.zero;

		// Token: 0x04002567 RID: 9575
		private Vector2 _SafeAreaAnchorMax = Vector2.one;

		// Token: 0x04002568 RID: 9576
		private float _aspect;

		// Token: 0x04002569 RID: 9577
		private Preferences.DisplayMode _displayMode;

		// Token: 0x0400256A RID: 9578
		private const float kEpsilon = 0.01f;

		// Token: 0x0400256B RID: 9579
		public float regularAspectRatioLimitHorizontal = 1.4545455f;

		// Token: 0x0400256C RID: 9580
		public float regularAspectRatioLimitVertical = 0.6875f;

		// Token: 0x0400256D RID: 9581
		public float compactAspectRatioLimitHorizontal = 1.3333334f;

		// Token: 0x0400256E RID: 9582
		public float compactAspectRatioLimitVertical = 0.75f;

		// Token: 0x0400256F RID: 9583
		private Vector2 _containerSize;

		// Token: 0x04002570 RID: 9584
		private CommunityHubContent _selectedContentInTabs;

		// Token: 0x04002571 RID: 9585
		private GameObject _expandCollapseButtonRoot;

		// Token: 0x04002572 RID: 9586
		private ExpandCollapseButton _expandCollapseButton;

		// Token: 0x04002573 RID: 9587
		private GameObject _tabBarRoot;

		// Token: 0x04002574 RID: 9588
		private TabBar _tabBar;

		// Token: 0x04002575 RID: 9589
		private TabBar.TabAlignment _tabBarAlignment;

		// Token: 0x0200091C RID: 2332
		private class LayoutContext
		{
			// Token: 0x04003095 RID: 12437
			public Vector2 gameContentSafeAreaAnchorMin = Vector2.zero;

			// Token: 0x04003096 RID: 12438
			public Vector2 gameContentSafeAreaAnchorMax = Vector2.one;

			// Token: 0x04003097 RID: 12439
			public float headersStartY;

			// Token: 0x04003098 RID: 12440
			public float headersHeight;

			// Token: 0x04003099 RID: 12441
			public float expandCollapseButtonWidth;

			// Token: 0x0400309A RID: 12442
			public Vector2 tabsAnchorMin = Vector2.zero;

			// Token: 0x0400309B RID: 12443
			public Vector2 tabsAnchorMax = Vector2.one;

			// Token: 0x0400309C RID: 12444
			public Vector2 tabsOffsetMin = Vector2.zero;

			// Token: 0x0400309D RID: 12445
			public Vector2 tabsOffsetMax = Vector2.zero;
		}
	}
}
