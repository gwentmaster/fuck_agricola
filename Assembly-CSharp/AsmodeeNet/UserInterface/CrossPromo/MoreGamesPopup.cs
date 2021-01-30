using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000659 RID: 1625
	public class MoreGamesPopup : BaseGroupOfProductPopup
	{
		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06003BFE RID: 15358 RVA: 0x001299C4 File Offset: 0x00127BC4
		// (set) Token: 0x06003BFF RID: 15359 RVA: 0x001299CC File Offset: 0x00127BCC
		public GameProductTag CurrentFilter
		{
			get
			{
				return this._currentFilter;
			}
			set
			{
				this._currentFilter = value;
				if (this._groupOfProducts[value] == null)
				{
					this.spinner.SetActive(true);
					CrossPromoCacheManager.CancelLoadMoreGame();
					CrossPromoCacheManager.LoadMoreGame(new GameProductTag?(this.CurrentFilter), delegate(ShowcaseProduct[] products)
					{
						this.spinner.SetActive(false);
						this._groupOfProducts[value] = products;
						this.ReloadProducts(products);
					}, delegate
					{
						AlertController.InstantiateAlertController(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Error.Title"), CoreApplication.Instance.LocalizationManager.GetLocalizedText("SSO.Error.UnableToConnect")).AddAction(CoreApplication.Instance.LocalizationManager.GetLocalizedText("Standard.Alert.Ok"), AlertController.ButtonStyle.Default, new Action(this.Dismiss));
					});
				}
				else
				{
					base.ReloadProducts(this._groupOfProducts[value]);
				}
				CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action action;
				switch (value)
				{
				case GameProductTag.featured:
					action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_featured;
					break;
				case GameProductTag.gamer:
					action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_gamer;
					break;
				case GameProductTag.family:
					action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_family;
					break;
				case GameProductTag.board:
					action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_filter_boardgame;
					break;
				default:
					action = CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.automatic;
					break;
				}
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.more_games, action, null, null, null);
			}
		}

		// Token: 0x06003C00 RID: 15360 RVA: 0x00129AA4 File Offset: 0x00127CA4
		public static MoreGamesPopup InstantiateMoreGames()
		{
			MoreGamesPopup moreGamesPopup = (MoreGamesPopup)UnityEngine.Object.FindObjectOfType(typeof(MoreGamesPopup));
			if (moreGamesPopup == null)
			{
				moreGamesPopup = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.MoreGamesPopupPrefab).GetComponent<MoreGamesPopup>();
			}
			else
			{
				AsmoLogger.Error("InterstitialPopup", "Try to InstantiateMoreGames twice", null);
			}
			return moreGamesPopup;
		}

		// Token: 0x06003C01 RID: 15361 RVA: 0x00129AFC File Offset: 0x00127CFC
		private void Start()
		{
			AnalyticsEvents.LogCrossPromoOpenedEvent(new CROSSPROMO_OPENED.crosspromo_type?(CROSSPROMO_OPENED.crosspromo_type.more_games), null);
			this.CurrentFilter = GameProductTag.featured;
		}

		// Token: 0x06003C02 RID: 15362 RVA: 0x00129B11 File Offset: 0x00127D11
		public void ShowFeatured(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.featured)
			{
				this.CurrentFilter = GameProductTag.featured;
			}
		}

		// Token: 0x06003C03 RID: 15363 RVA: 0x00129B26 File Offset: 0x00127D26
		public void ShowGamer(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.gamer)
			{
				this.CurrentFilter = GameProductTag.gamer;
			}
		}

		// Token: 0x06003C04 RID: 15364 RVA: 0x00129B3C File Offset: 0x00127D3C
		public void ShowFamily(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.family)
			{
				this.CurrentFilter = GameProductTag.family;
			}
		}

		// Token: 0x06003C05 RID: 15365 RVA: 0x00129B52 File Offset: 0x00127D52
		public void ShowBoard(bool isOn)
		{
			if (!isOn)
			{
				return;
			}
			if (this.CurrentFilter != GameProductTag.board)
			{
				this.CurrentFilter = GameProductTag.board;
			}
		}

		// Token: 0x06003C06 RID: 15366 RVA: 0x001299BC File Offset: 0x00127BBC
		public override void Dismiss()
		{
			base.Dismiss();
		}

		// Token: 0x040026C9 RID: 9929
		public TabToggleTMP TabToggleNew;

		// Token: 0x040026CA RID: 9930
		public GameObject spinner;

		// Token: 0x040026CB RID: 9931
		private const string _consoleModuleName = "InterstitialPopup";

		// Token: 0x040026CC RID: 9932
		private Dictionary<GameProductTag, ShowcaseProduct[]> _groupOfProducts = new Dictionary<GameProductTag, ShowcaseProduct[]>
		{
			{
				GameProductTag.board,
				null
			},
			{
				GameProductTag.family,
				null
			},
			{
				GameProductTag.featured,
				null
			},
			{
				GameProductTag.gamer,
				null
			}
		};

		// Token: 0x040026CD RID: 9933
		private GameProductTag _currentFilter;
	}
}
