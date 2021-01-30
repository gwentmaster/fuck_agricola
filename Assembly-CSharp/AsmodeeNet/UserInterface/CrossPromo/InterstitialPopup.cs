using System;
using System.Collections;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000658 RID: 1624
	public class InterstitialPopup : BaseGroupOfProductPopup
	{
		// Token: 0x06003BF5 RID: 15349 RVA: 0x00129824 File Offset: 0x00127A24
		public static InterstitialPopup InstantiateInterstitial()
		{
			InterstitialPopup interstitialPopup = (InterstitialPopup)UnityEngine.Object.FindObjectOfType(typeof(InterstitialPopup));
			if (interstitialPopup == null)
			{
				interstitialPopup = UnityEngine.Object.Instantiate<GameObject>(CoreApplication.Instance.InterfaceSkin.InterstitialPopupPrefab).GetComponent<InterstitialPopup>();
			}
			else
			{
				AsmoLogger.Error("InterstitialPopup", "Try to InstantiateInterstitial twice", null);
			}
			return interstitialPopup;
		}

		// Token: 0x06003BF6 RID: 15350 RVA: 0x0012987C File Offset: 0x00127A7C
		private void Start()
		{
			this._startTime = Time.time;
			this.skipButton.interactable = false;
			this.skipButton.gameObject.SetActive(false);
			this.spinner.SetActive(true);
			CrossPromoCacheManager.CancelLoadInterstitial();
			CrossPromoCacheManager.LoadInterstitial(delegate(ShowcaseProduct[] products)
			{
				this._products = products;
				AnalyticsEvents.LogCrossPromoDisplayedEvent(this._products, CROSSPROMO_DISPLAYED.crosspromo_type.interstitial, null);
				AnalyticsEvents.LogCrossPromoScreenDisplayEvent(CROSSPROMO_SCREEN_DISPLAY.screen_current.interstitial, CROSSPROMO_SCREEN_DISPLAY.screen_previous_nav_action.automatic, null, null, null);
				this.skipButton.gameObject.SetActive(true);
				this.spinner.SetActive(false);
				base.ReloadProducts(products);
				base.StartCoroutine(this._ActivateCloseButton());
			}, delegate
			{
				base.Dismiss();
			});
		}

		// Token: 0x06003BF7 RID: 15351 RVA: 0x001298DF File Offset: 0x00127ADF
		public override void Dismiss()
		{
			if (Time.time - this._startTime < (float)this.delayBeforeSkip)
			{
				if (!base.gameObject.activeSelf)
				{
					base.gameObject.SetActive(true);
				}
				return;
			}
			base.Dismiss();
		}

		// Token: 0x06003BF8 RID: 15352 RVA: 0x00129916 File Offset: 0x00127B16
		protected override void OnEnable()
		{
			base.OnEnable();
			base.StartCoroutine(this._ActivateCloseButton());
		}

		// Token: 0x06003BF9 RID: 15353 RVA: 0x0012992B File Offset: 0x00127B2B
		protected override void OnDisable()
		{
			base.OnDisable();
			base.StopAllCoroutines();
		}

		// Token: 0x06003BFA RID: 15354 RVA: 0x00129939 File Offset: 0x00127B39
		private IEnumerator _ActivateCloseButton()
		{
			while (Time.time - this._startTime < (float)this.delayBeforeSkip)
			{
				this.textSkipButton.text = string.Format("{0} ({1})", CoreApplication.Instance.LocalizationManager.GetLocalizedText("CrossPromo.close"), this.delayBeforeSkip - Mathf.FloorToInt(Time.time - this._startTime));
				yield return null;
			}
			this.skipButton.interactable = true;
			this.textSkipButton.text = CoreApplication.Instance.LocalizationManager.GetLocalizedText("CrossPromo.close");
			yield break;
		}

		// Token: 0x040026C3 RID: 9923
		public Button skipButton;

		// Token: 0x040026C4 RID: 9924
		public TextMeshProUGUI textSkipButton;

		// Token: 0x040026C5 RID: 9925
		public GameObject spinner;

		// Token: 0x040026C6 RID: 9926
		public int delayBeforeSkip = 20;

		// Token: 0x040026C7 RID: 9927
		private float _startTime;

		// Token: 0x040026C8 RID: 9928
		private const string _kModuleName = "InterstitialPopup";
	}
}
