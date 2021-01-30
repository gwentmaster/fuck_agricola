using System;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000650 RID: 1616
	public class BannerManager : MonoBehaviour
	{
		// Token: 0x170007FB RID: 2043
		// (get) Token: 0x06003BB3 RID: 15283 RVA: 0x001282B3 File Offset: 0x001264B3
		// (set) Token: 0x06003BB4 RID: 15284 RVA: 0x001282C0 File Offset: 0x001264C0
		public BannerImageFitter.BannerPosition Position
		{
			get
			{
				return this._bannerImageFitter.Position;
			}
			set
			{
				this._bannerImageFitter.Position = value;
			}
		}

		// Token: 0x06003BB5 RID: 15285 RVA: 0x001282CE File Offset: 0x001264CE
		private void Awake()
		{
			this._bannerImage = base.GetComponentInChildren<RawImage>(true);
		}

		// Token: 0x06003BB6 RID: 15286 RVA: 0x001282DD File Offset: 0x001264DD
		private void Start()
		{
			CrossPromoCacheManager.LoadBanner(delegate(ShowcaseProduct product)
			{
				this._product = product;
				base.StartCoroutine(CrossPromoCacheManager.LoadBannerImage(product, this._bannerImage, delegate(bool success)
				{
					if (success)
					{
						AnalyticsEvents.LogCrossPromoDisplayedEvent(new ShowcaseProduct[]
						{
							this._product
						}, CROSSPROMO_DISPLAYED.crosspromo_type.banner, null);
						this._bannerImage.gameObject.SetActive(true);
						return;
					}
					this._CleanBanner();
				}));
			}, delegate
			{
				this._CleanBanner();
			});
		}

		// Token: 0x06003BB7 RID: 15287 RVA: 0x001282FC File Offset: 0x001264FC
		public static BannerManager InstantiateBanner()
		{
			BannerManager bannerManager = (BannerManager)UnityEngine.Object.FindObjectOfType(typeof(BannerManager));
			if (bannerManager == null)
			{
				bannerManager = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.BannerPrefab).GetComponent<BannerManager>();
			}
			else
			{
				AsmoLogger.Error("BannerManager", "Try to InstantiateBanner twice", null);
			}
			return bannerManager;
		}

		// Token: 0x06003BB8 RID: 15288 RVA: 0x00128354 File Offset: 0x00126554
		public void Dismiss()
		{
			CoreApplication.Instance.AnalyticsManager.RemoveContext(typeof(CrossPromoAnalyticsContext));
			this._CleanBanner();
		}

		// Token: 0x06003BB9 RID: 15289 RVA: 0x00128375 File Offset: 0x00126575
		private void _CleanBanner()
		{
			CrossPromoCacheManager.CancelLoadBanner();
			UnityEngine.Object.Destroy(base.gameObject);
		}

		// Token: 0x06003BBA RID: 15290 RVA: 0x00128388 File Offset: 0x00126588
		public void OnBannerClicked()
		{
			AnalyticsEvents.LogCrossPromoOpenedEvent(null, null);
			AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.game_detail, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.click_banner, this._product, null, null);
			GameDetailsPopup.InstantiateGameDetails(this._product, null, null);
		}

		// Token: 0x04002675 RID: 9845
		private RawImage _bannerImage;

		// Token: 0x04002676 RID: 9846
		[SerializeField]
		private BannerImageFitter _bannerImageFitter;

		// Token: 0x04002677 RID: 9847
		private ShowcaseProduct _product;

		// Token: 0x04002678 RID: 9848
		private const string _kModuleName = "BannerManager";
	}
}
