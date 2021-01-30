using System;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Foundation.Localization;
using AsmodeeNet.Network.RestApi;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200065B RID: 1627
	public class TileContainer : MonoBehaviour
	{
		// Token: 0x06003C10 RID: 15376 RVA: 0x00129C78 File Offset: 0x00127E78
		public Rect Init(ShowcaseProduct product, int xCellPositionInGrid, int yCellPositionInGrid, int cellSizeInPixels, int xSpacing, int ySpacing, Action closeDetailsAction)
		{
			this._product = product;
			this._cellPositionInGrid = new Vector2((float)xCellPositionInGrid, (float)yCellPositionInGrid);
			this._closeDetailsAction = closeDetailsAction;
			base.gameObject.SetActive(true);
			this.ui.Loading.gameObject.SetActive(true);
			this.ui.Image.gameObject.SetActive(false);
			this.ui.ImageButton.color = this.PublishedColor;
			LocalizationManager localizationManager = CoreApplication.Instance.LocalizationManager;
			if (product.Status == ProductStatus.soon)
			{
				this.ui.TextButton.text = localizationManager.GetLocalizedText("CrossPromo.button.comingsoon");
				this.ui.ImageButton.color = this.ComingSoonColor;
			}
			else
			{
				string key = string.IsNullOrEmpty(this._product.ShopDigitalUrl) ? "CrossPromo.button.learnmore" : "CrossPromo.button.playnow";
				this.ui.TextButton.text = localizationManager.GetLocalizedText(key);
			}
			base.StartCoroutine(CrossPromoCacheManager.LoadProductTileImage(product, this.ui.Image, delegate(bool success)
			{
				this.ui.Loading.gameObject.SetActive(false);
				if (!success)
				{
					this.ui.Image.gameObject.SetActive(true);
					this.ui.Image.texture = (Resources.Load(string.Format("DefaultTextures/DefaultTile{0}x{1}", product.Tile.Width, product.Tile.Height)) as Texture2D);
				}
			}));
			this.ui.RectTransform.localScale = Vector3.one;
			int num = (int)((float)cellSizeInPixels * 0.2f);
			int num2 = (cellSizeInPixels + xSpacing) * xCellPositionInGrid;
			int num3 = (cellSizeInPixels + num + ySpacing) * yCellPositionInGrid;
			int num4 = product.Tile.Width * cellSizeInPixels + (product.Tile.Width - 1) * xSpacing;
			int num5 = product.Tile.Height * (cellSizeInPixels + num) + (product.Tile.Height - 1) * ySpacing;
			this._position = new Vector3((float)num2, (float)(-(float)num3), 0f);
			this.ui.RectTransform.sizeDelta = new Vector2((float)num4, (float)num5);
			return new Rect((float)num2, (float)num3, (float)num4, (float)num5);
		}

		// Token: 0x06003C11 RID: 15377 RVA: 0x00129E83 File Offset: 0x00128083
		private void Update()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this._needsUpdate = false;
			base.transform.localPosition = this._position;
		}

		// Token: 0x06003C12 RID: 15378 RVA: 0x00129EA8 File Offset: 0x001280A8
		private void _ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action action)
		{
			base.transform.root.gameObject.SetActive(false);
			CrossPromoAnalyticsContext crossPromoAnalyticsContext = CoreApplication.Instance.AnalyticsManager.GetContext(typeof(CrossPromoAnalyticsContext)) as CrossPromoAnalyticsContext;
			CROSSPROMO_SCREEN_DISPLAY.screen_current screen = (CROSSPROMO_SCREEN_DISPLAY.screen_current)Enum.Parse(typeof(CROSSPROMO_SCREEN_DISPLAY.screen_current), crossPromoAnalyticsContext.ScreenCurrent);
			AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, action, this._product, new Vector2?(this._cellPositionInGrid), null);
			GameDetailsPopup.InstantiateGameDetails(this._product, delegate(GameDetailsPopup p)
			{
				p.Dismiss();
				if (this._closeDetailsAction != null)
				{
					this._closeDetailsAction();
				}
			}, delegate(GameDetailsPopup p)
			{
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(screen, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_back, this._product, new Vector2?(this._cellPositionInGrid), null);
				this.transform.root.gameObject.SetActive(true);
				p.Dismiss();
			});
		}

		// Token: 0x06003C13 RID: 15379 RVA: 0x00129F54 File Offset: 0x00128154
		public void TileImage_Clicked()
		{
			this._ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_tile);
		}

		// Token: 0x06003C14 RID: 15380 RVA: 0x00129F5D File Offset: 0x0012815D
		public void TileButton_Clicked()
		{
			if (string.IsNullOrEmpty(this._product.ShopDigitalUrl))
			{
				this._ShowDetails(CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_learn_more);
				return;
			}
			AnalyticsEvents.LogCrossPromoRedirectedEvent(this._product, null);
			Application.OpenURL(this._product.ShopDigitalUrl);
		}

		// Token: 0x040026D2 RID: 9938
		public TileContainer.UI ui;

		// Token: 0x040026D3 RID: 9939
		public Color ComingSoonColor;

		// Token: 0x040026D4 RID: 9940
		public Color PublishedColor;

		// Token: 0x040026D5 RID: 9941
		public GameObject gameDetailsPopupPrefab;

		// Token: 0x040026D6 RID: 9942
		private ShowcaseProduct _product;

		// Token: 0x040026D7 RID: 9943
		private Vector2 _cellPositionInGrid;

		// Token: 0x040026D8 RID: 9944
		private Action _closeDetailsAction;

		// Token: 0x040026D9 RID: 9945
		private Vector3 _position;

		// Token: 0x040026DA RID: 9946
		private bool _needsUpdate = true;

		// Token: 0x02000977 RID: 2423
		[Serializable]
		public class UI
		{
			// Token: 0x040031DD RID: 12765
			public RectTransform RectTransform;

			// Token: 0x040031DE RID: 12766
			public RawImage Image;

			// Token: 0x040031DF RID: 12767
			public TextMeshProUGUI TextButton;

			// Token: 0x040031E0 RID: 12768
			public Transform Loading;

			// Token: 0x040031E1 RID: 12769
			public Image ImageButton;
		}
	}
}
