using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000657 RID: 1623
	public class GameDetailsPopup : CrossPromoBasePopup
	{
		// Token: 0x06003BEA RID: 15338 RVA: 0x00128E4A File Offset: 0x0012704A
		public void OnBackClicked()
		{
			if (this.ContainerZoomed != null)
			{
				this.Zoom(this.ContainerZoomed);
				return;
			}
			this.Close(this._onClickPreviousButton != null);
		}

		// Token: 0x06003BEB RID: 15339 RVA: 0x00128E78 File Offset: 0x00127078
		public static GameDetailsPopup InstantiateGameDetails(ShowcaseProduct product, Action<GameDetailsPopup> onClickCloseButton = null, Action<GameDetailsPopup> onClickPreviousButton = null)
		{
			GameDetailsPopup gameDetailsPopup = (GameDetailsPopup)UnityEngine.Object.FindObjectOfType(typeof(GameDetailsPopup));
			if (gameDetailsPopup == null)
			{
				gameDetailsPopup = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.GameDetailsPopupPrefab).GetComponent<GameDetailsPopup>();
				gameDetailsPopup._Init(product, onClickCloseButton, onClickPreviousButton);
			}
			else
			{
				AsmoLogger.Error("GameDetailsPopup", "Try to InstantiateMoreGames twice", null);
			}
			return gameDetailsPopup;
		}

		// Token: 0x06003BEC RID: 15340 RVA: 0x00128ED9 File Offset: 0x001270D9
		private void _Init(ShowcaseProduct product, Action<GameDetailsPopup> onClickCloseButton, Action<GameDetailsPopup> onClickPreviousButton)
		{
			this._onClickCloseButton = onClickCloseButton;
			this._onClickPreviousButton = onClickPreviousButton;
			this._product = product;
		}

		// Token: 0x06003BED RID: 15341 RVA: 0x00128EF0 File Offset: 0x001270F0
		private void Start()
		{
			if (this._product == null)
			{
				AsmoLogger.Error("", "Cannot show a GameDetailsPopup which has not been initialized. Consider calling Init() before", null);
				return;
			}
			this._CleanPopup();
			this.uiGameDetailsPopup.BackButton.gameObject.SetActive(this._onClickPreviousButton != null);
			this.uiGameDetailsPopup.Name.text = this._product.Name;
			this.uiGameDetailsPopup.Description.text = this._product.Description;
			this.uiGameDetailsPopup.DescriptionIcon.Init(this, -1);
			base.StartCoroutine(CrossPromoCacheManager.LoadProductIcon(this._product, this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image, delegate(bool success)
			{
				this.uiGameDetailsPopup.DescriptionIcon.HideLoading();
				if (!success)
				{
					this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.gameObject.SetActive(true);
					Texture2D texture2D = Resources.Load("DefaultTextures/DefaultTile1x1") as Texture2D;
					this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), new Vector2(0.5f, 0.5f));
				}
			}));
			this.uiGameDetailsPopup.BuyDigitalButton.gameObject.SetActive(!string.IsNullOrEmpty(this._product.ShopDigitalUrl));
			this.uiGameDetailsPopup.BuyPhysicalButton.gameObject.SetActive(!string.IsNullOrEmpty(this._product.ShopPhysicalUrl));
			int num = 0;
			foreach (ShowcaseImage showcaseImage in this._product.Images)
			{
				CrossPromoContainer crossPromoContainer = UnityEngine.Object.Instantiate<CrossPromoContainer>(this.ImageContainerPrefab, this.uiGameDetailsPopup.Content);
				crossPromoContainer.transform.localScale = Vector3.one;
				crossPromoContainer.GetComponent<LayoutElement>().preferredHeight = this.uiGameDetailsPopup.Content.rect.height;
				crossPromoContainer.GetComponent<LayoutElement>().preferredWidth = this.uiGameDetailsPopup.Content.rect.height;
				crossPromoContainer.Init(this, num++);
				crossPromoContainer.LoadThumbnail(this._product);
			}
			foreach (ShowcaseAward showcaseAward in this._product.Awards)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.AwardContainerPrefab, this.uiGameDetailsPopup.DescriptionLeftColumn);
				gameObject.transform.localScale = Vector3.one;
				this._awardContainers.Add(gameObject);
				base.StartCoroutine(CrossPromoCacheManager.LoadProductAward(this._product, showcaseAward.ImageUrl, gameObject.GetComponentInChildren<Image>(), null));
			}
		}

		// Token: 0x06003BEE RID: 15342 RVA: 0x00129128 File Offset: 0x00127328
		private void _CleanPopup()
		{
			base.StopAllCoroutines();
			if (this.ContainerZoomed != null)
			{
				UnityEngine.Object.Destroy(this.uiGameDetailsPopup.ZoomedImageContainer.GetChild(0).gameObject);
				this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, 0f);
				this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = true;
				this._zoomState = GameDetailsPopup.ZoomState.NoZoom;
				this.ContainerZoomed = null;
			}
			this.uiGameDetailsPopup.Content.anchoredPosition = Vector2.zero;
			this.uiGameDetailsPopup.DescriptionIcon.uiContainer.Image.sprite = null;
			for (int i = 0; i < this.uiGameDetailsPopup.Content.childCount; i++)
			{
				UnityEngine.Object.Destroy(this.uiGameDetailsPopup.Content.GetChild(i).gameObject);
			}
			foreach (GameObject gameObject in this._awardContainers)
			{
				UnityEngine.Object.Destroy(gameObject.gameObject);
			}
			this._awardContainers.Clear();
		}

		// Token: 0x06003BEF RID: 15343 RVA: 0x00129268 File Offset: 0x00127468
		public void Zoom(CrossPromoContainer container)
		{
			float num = 0.25f;
			Easer cubeInOut = Ease.CubeInOut;
			if (this._zoomState == GameDetailsPopup.ZoomState.Zoomed)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_image, this._product, null, null);
				container.LoadThumbnail(this._product);
				this._zoomState = GameDetailsPopup.ZoomState.ZoomAnimation;
				int compteur = 4;
				Action endAnim = delegate()
				{
					int compteur = compteur;
					compteur--;
					if (compteur == 0)
					{
						this.ContainerZoomed.transform.SetParent(this.uiGameDetailsPopup.Content);
						this.ContainerZoomed.uiContainer.RectTransform.SetSiblingIndex(this._indexSibling);
						this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = true;
						this.ContainerZoomed = null;
						this._zoomState = GameDetailsPopup.ZoomState.NoZoom;
					}
				};
				base.StartCoroutine(Easing.EaseFromTo(0.75f, 0f, num, cubeInOut, delegate(float value)
				{
					this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, value);
				}, endAnim));
				base.StartCoroutine(Easing.EaseFromTo(0f, 1f, num + 0.15f, cubeInOut, delegate(float value)
				{
				}, delegate()
				{
					this.uiGameDetailsPopup.ImagesScrollView.horizontal = true;
					endAnim();
				}));
				base.StartCoroutine(Easing.EaseFromTo(this.ContainerZoomed.uiContainer.RectTransform.offsetMin, this._prevOffsetMin, num, cubeInOut, delegate(Vector2 value)
				{
					this.ContainerZoomed.uiContainer.RectTransform.offsetMin = value;
				}, endAnim));
				base.StartCoroutine(Easing.EaseFromTo(this.ContainerZoomed.uiContainer.RectTransform.offsetMax, this._prevOffsetMax, num, cubeInOut, delegate(Vector2 value)
				{
					this.ContainerZoomed.uiContainer.RectTransform.offsetMax = value;
				}, endAnim));
				return;
			}
			if (this._zoomState == GameDetailsPopup.ZoomState.NoZoom)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.zoom_image, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_image, null, null, null);
				container.LoadImage(this._product);
				this._zoomState = GameDetailsPopup.ZoomState.ZoomAnimation;
				this.uiGameDetailsPopup.ImagesScrollView.horizontal = false;
				this.uiGameDetailsPopup.ImagesHorizontalLayoutGroup.enabled = false;
				this.ContainerZoomed = container;
				this._indexSibling = container.uiContainer.RectTransform.GetSiblingIndex();
				container.transform.SetParent(this.uiGameDetailsPopup.ZoomedImageContainer, true);
				Vector3 position = container.transform.position;
				Vector2 pivot = new Vector2(0.5f, 0.5f);
				container.uiContainer.RectTransform.pivot = pivot;
				container.uiContainer.RectTransform.anchorMin = new Vector2(0f, 0f);
				container.uiContainer.RectTransform.anchorMax = new Vector2(1f, 1f);
				container.uiContainer.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, container.uiContainer.Layout.preferredWidth);
				container.uiContainer.RectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, container.uiContainer.Layout.preferredHeight);
				container.transform.position = position;
				int compteur = 4;
				Action actionAfterEasing = delegate()
				{
					int compteur = compteur;
					compteur--;
					if (compteur == 0)
					{
						this._zoomState = GameDetailsPopup.ZoomState.Zoomed;
					}
				};
				this._prevOffsetMin = container.uiContainer.RectTransform.offsetMin;
				this._prevOffsetMax = container.uiContainer.RectTransform.offsetMax;
				base.StartCoroutine(Easing.EaseFromTo(0f, 0.75f, num, cubeInOut, delegate(float value)
				{
					this.uiGameDetailsPopup.BackgroundZoomContainer.color = new Color(0f, 0f, 0f, value);
				}, actionAfterEasing));
				base.StartCoroutine(Easing.EaseFromTo(container.uiContainer.RectTransform.offsetMin, Vector2.zero, num, cubeInOut, delegate(Vector2 value)
				{
					container.uiContainer.RectTransform.offsetMin = value;
				}, actionAfterEasing));
				base.StartCoroutine(Easing.EaseFromTo(container.uiContainer.RectTransform.offsetMax, Vector2.zero, num, cubeInOut, delegate(Vector2 value)
				{
					container.uiContainer.RectTransform.offsetMax = value;
				}, actionAfterEasing));
				base.StartCoroutine(container.transform.MoveTo(Vector3.zero, num, cubeInOut, actionAfterEasing));
			}
		}

		// Token: 0x06003BF0 RID: 15344 RVA: 0x00129706 File Offset: 0x00127906
		public void DigitalShop()
		{
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopDigitalUrl);
		}

		// Token: 0x06003BF1 RID: 15345 RVA: 0x00129724 File Offset: 0x00127924
		public void PhysicalShop()
		{
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopPhysicalUrl);
		}

		// Token: 0x06003BF2 RID: 15346 RVA: 0x00129742 File Offset: 0x00127942
		public void Close(bool goBackToPreviousPopup)
		{
			if (goBackToPreviousPopup)
			{
				this._onClickPreviousButton(this);
				return;
			}
			if (this._onClickCloseButton != null)
			{
				this._onClickCloseButton(this);
				return;
			}
			AnalyticsEvents.LogCrossPromoClosedEvent(null);
			this.Dismiss();
		}

		// Token: 0x040026B6 RID: 9910
		public GameDetailsPopup.UIGameDetailsPopup uiGameDetailsPopup;

		// Token: 0x040026B7 RID: 9911
		private Action<GameDetailsPopup> _onClickCloseButton;

		// Token: 0x040026B8 RID: 9912
		private Action<GameDetailsPopup> _onClickPreviousButton;

		// Token: 0x040026B9 RID: 9913
		private ShowcaseProduct _product;

		// Token: 0x040026BA RID: 9914
		[Header("Prefabs")]
		public CrossPromoContainer ImageContainerPrefab;

		// Token: 0x040026BB RID: 9915
		public GameObject AwardContainerPrefab;

		// Token: 0x040026BC RID: 9916
		private List<GameObject> _awardContainers = new List<GameObject>();

		// Token: 0x040026BD RID: 9917
		private CrossPromoContainer ContainerZoomed;

		// Token: 0x040026BE RID: 9918
		private GameDetailsPopup.ZoomState _zoomState;

		// Token: 0x040026BF RID: 9919
		private int _indexSibling;

		// Token: 0x040026C0 RID: 9920
		private Vector2 _prevOffsetMin;

		// Token: 0x040026C1 RID: 9921
		private Vector2 _prevOffsetMax;

		// Token: 0x040026C2 RID: 9922
		private const string _consoleModuleName = "GameDetailsPopup";

		// Token: 0x0200096F RID: 2415
		[Serializable]
		public class UIGameDetailsPopup
		{
			// Token: 0x040031BE RID: 12734
			[Header("Header")]
			public TextMeshProUGUI Name;

			// Token: 0x040031BF RID: 12735
			public Button BackButton;

			// Token: 0x040031C0 RID: 12736
			[Header("Main")]
			public RectTransform MainContent;

			// Token: 0x040031C1 RID: 12737
			[Header("Images and Videos")]
			public RectTransform Content;

			// Token: 0x040031C2 RID: 12738
			public ScrollRect ImagesScrollView;

			// Token: 0x040031C3 RID: 12739
			public HorizontalLayoutGroup ImagesHorizontalLayoutGroup;

			// Token: 0x040031C4 RID: 12740
			[Header("Description - Left")]
			public Transform DescriptionLeftColumn;

			// Token: 0x040031C5 RID: 12741
			public CrossPromoContainer DescriptionIcon;

			// Token: 0x040031C6 RID: 12742
			public Button BuyDigitalButton;

			// Token: 0x040031C7 RID: 12743
			public Button BuyPhysicalButton;

			// Token: 0x040031C8 RID: 12744
			[Header("Description - Right")]
			public TextMeshProUGUI Description;

			// Token: 0x040031C9 RID: 12745
			[Header("Zoomed Image")]
			public RectTransform ZoomedImageContainer;

			// Token: 0x040031CA RID: 12746
			public Image BackgroundZoomContainer;
		}

		// Token: 0x02000970 RID: 2416
		public enum ZoomState
		{
			// Token: 0x040031CC RID: 12748
			NoZoom,
			// Token: 0x040031CD RID: 12749
			ZoomAnimation,
			// Token: 0x040031CE RID: 12750
			Zoomed
		}
	}
}
