using System;
using System.Collections;
using AsmodeeNet.Foundation;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface
{
	// Token: 0x0200063D RID: 1597
	[RequireComponent(typeof(Canvas))]
	[RequireComponent(typeof(CanvasScaler))]
	public class ResponsivePopUp : MonoBehaviour
	{
		// Token: 0x170007DC RID: 2012
		// (get) Token: 0x06003AB2 RID: 15026 RVA: 0x00123CE8 File Offset: 0x00121EE8
		public float Ratio
		{
			get
			{
				switch (this._prefs.InterfaceDisplayMode)
				{
				case Preferences.DisplayMode.Small:
					return this.smallRatio;
				case Preferences.DisplayMode.Big:
					return this.bigRatio;
				}
				return this.regularRatio;
			}
		}

		// Token: 0x06003AB3 RID: 15027 RVA: 0x00123D2C File Offset: 0x00121F2C
		private void Awake()
		{
			UnityEngine.Object component = base.GetComponent<Canvas>();
			this._canvasScaler = base.GetComponent<CanvasScaler>();
			if (component == null || this._canvasScaler == null)
			{
				AsmoLogger.Error("ResponsivePopUp", "ResponsivePopUp component should be added to the root element of your popup (containing the Canvas and <b>CanvasScaler</b>)", null);
				base.gameObject.SetActive(false);
				return;
			}
			this._root = (base.transform as RectTransform);
			this._canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
			this._canvasScalerOriginalReferenceResolution = this._canvasScaler.referenceResolution;
			this.container.anchorMin = new Vector2(0.5f, 0.5f);
			this.container.anchorMax = new Vector2(0.5f, 0.5f);
			this.container.localPosition = new Vector3(0f, 0f, 0f);
			this._prefs = CoreApplication.Instance.Preferences;
			if (this.fadeDisplay)
			{
				this._canvasGroup = base.GetComponent<CanvasGroup>();
			}
		}

		// Token: 0x06003AB4 RID: 15028 RVA: 0x00123E24 File Offset: 0x00122024
		private void OnEnable()
		{
			this._prefs.AspectDidChange += this.SetNeedsUpdate;
			this._prefs.InterfaceDisplayModeDidChange += this.SetNeedsUpdate;
			this._needsUpdate = true;
			this.Update();
			if (this.autoFadeOnEnable)
			{
				this.FadeIn(null);
			}
		}

		// Token: 0x06003AB5 RID: 15029 RVA: 0x00123E7B File Offset: 0x0012207B
		private void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				this._prefs.AspectDidChange -= this.SetNeedsUpdate;
				this._prefs.InterfaceDisplayModeDidChange -= this.SetNeedsUpdate;
			}
		}

		// Token: 0x06003AB6 RID: 15030 RVA: 0x00123EB2 File Offset: 0x001220B2
		private void SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x14000036 RID: 54
		// (add) Token: 0x06003AB7 RID: 15031 RVA: 0x00123EBC File Offset: 0x001220BC
		// (remove) Token: 0x06003AB8 RID: 15032 RVA: 0x00123EF4 File Offset: 0x001220F4
		public event Action OnUpdateFinished;

		// Token: 0x06003AB9 RID: 15033 RVA: 0x00123F2C File Offset: 0x0012212C
		private void Update()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this._needsUpdate = false;
			ResponsivePopUp.ResponsiveSettings responsiveSettings;
			float num;
			switch (this._prefs.InterfaceDisplayMode)
			{
			case Preferences.DisplayMode.Small:
				responsiveSettings = this.smallSettings;
				num = this.smallRatio;
				goto IL_60;
			case Preferences.DisplayMode.Big:
				responsiveSettings = this.bigSettings;
				num = this.bigRatio;
				goto IL_60;
			}
			responsiveSettings = this.regularSettings;
			num = this.regularRatio;
			IL_60:
			if (this.responsiveScope == ResponsivePopUp.ResponsiveScope.Global)
			{
				responsiveSettings = this.globalSettings;
			}
			if (responsiveSettings.strategy == ResponsivePopUp.ResponsiveStrategy.FixedSize)
			{
				this._canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Expand;
				Vector2 vector = responsiveSettings.size;
				float num2 = vector.x / vector.y;
				float num3 = Mathf.Max(this._canvasScalerOriginalReferenceResolution.x, this._canvasScalerOriginalReferenceResolution.y);
				vector = ((responsiveSettings.size.x >= responsiveSettings.size.y) ? new Vector2(num3, num3 / num2) : new Vector2(num3 * num2, num3));
				this._canvasScaler.referenceResolution = vector;
				this.container.sizeDelta = responsiveSettings.size;
			}
			else
			{
				this._canvasScaler.screenMatchMode = CanvasScaler.ScreenMatchMode.Shrink;
				float num4 = Mathf.Max(this._canvasScalerOriginalReferenceResolution.x, this._canvasScalerOriginalReferenceResolution.y);
				this._canvasScaler.referenceResolution = new Vector2(num4, num4);
				float d = (this._root.sizeDelta.x >= this._root.sizeDelta.y) ? (num4 / this._root.sizeDelta.x) : (num4 / this._root.sizeDelta.y);
				this.container.sizeDelta = this._root.sizeDelta * d;
			}
			this.container.localScale = new Vector3(num, num, num);
			if (this.OnUpdateFinished != null)
			{
				this.OnUpdateFinished();
			}
		}

		// Token: 0x06003ABA RID: 15034 RVA: 0x00124113 File Offset: 0x00122313
		public void FadeIn(Action completion = null)
		{
			this._Fade(ResponsivePopUp.FadeType.In, completion);
		}

		// Token: 0x06003ABB RID: 15035 RVA: 0x0012411D File Offset: 0x0012231D
		public void FadeOut(Action completion = null)
		{
			this._Fade(ResponsivePopUp.FadeType.Out, completion);
		}

		// Token: 0x06003ABC RID: 15036 RVA: 0x00124128 File Offset: 0x00122328
		private void _Fade(ResponsivePopUp.FadeType type, Action completion)
		{
			if (!this.fadeDisplay)
			{
				if (completion != null)
				{
					completion();
				}
				return;
			}
			if (this._canvasGroup == null)
			{
				AsmoLogger.Error("ResponsivePopUp", "Fade In/Out requires a CanvasGroup", null);
			}
			base.StartCoroutine(this._FadeAnimation(type, completion));
		}

		// Token: 0x06003ABD RID: 15037 RVA: 0x00124174 File Offset: 0x00122374
		private IEnumerator _FadeAnimation(ResponsivePopUp.FadeType type, Action completion)
		{
			if (type == ResponsivePopUp.FadeType.In)
			{
				this._canvasGroup.alpha = 0f;
				float invDuration = 3.3333333f;
				while (this._canvasGroup.alpha < 1f)
				{
					this._canvasGroup.alpha += Time.deltaTime * invDuration;
					yield return null;
				}
			}
			else
			{
				this._canvasGroup.alpha = 1f;
				float invDuration = 3.3333333f;
				while (this._canvasGroup.alpha > 0f)
				{
					this._canvasGroup.alpha -= Time.deltaTime * invDuration;
					yield return null;
				}
			}
			if (completion != null)
			{
				completion();
			}
			yield break;
		}

		// Token: 0x0400260A RID: 9738
		private const string _documentation = "<b>ResponsivePopUp</b> will handle the basic layout of a popup window according to the <b>DisplayMode</b> [<b>Small</b>|<b>Regular</b>|<b>Big</b>]\nIt should be added to the root element of your popup (containing the <b>Canvas</b> and <b>CanvasScaler</b>)";

		// Token: 0x0400260B RID: 9739
		public RectTransform background;

		// Token: 0x0400260C RID: 9740
		public RectTransform container;

		// Token: 0x0400260D RID: 9741
		private RectTransform _root;

		// Token: 0x0400260E RID: 9742
		private CanvasGroup _canvasGroup;

		// Token: 0x0400260F RID: 9743
		private CanvasScaler _canvasScaler;

		// Token: 0x04002610 RID: 9744
		private Vector2 _canvasScalerOriginalReferenceResolution;

		// Token: 0x04002611 RID: 9745
		public ResponsivePopUp.ResponsiveScope responsiveScope;

		// Token: 0x04002612 RID: 9746
		public ResponsivePopUp.ResponsiveSettings globalSettings;

		// Token: 0x04002613 RID: 9747
		public ResponsivePopUp.ResponsiveSettings smallSettings;

		// Token: 0x04002614 RID: 9748
		public ResponsivePopUp.ResponsiveSettings regularSettings;

		// Token: 0x04002615 RID: 9749
		public ResponsivePopUp.ResponsiveSettings bigSettings;

		// Token: 0x04002616 RID: 9750
		public float smallRatio = 0.9f;

		// Token: 0x04002617 RID: 9751
		public float regularRatio = 0.6f;

		// Token: 0x04002618 RID: 9752
		public float bigRatio = 0.4f;

		// Token: 0x04002619 RID: 9753
		public bool fadeDisplay = true;

		// Token: 0x0400261A RID: 9754
		public const float fadeDuration = 0.3f;

		// Token: 0x0400261B RID: 9755
		public bool autoFadeOnEnable = true;

		// Token: 0x0400261C RID: 9756
		private Preferences _prefs;

		// Token: 0x0400261D RID: 9757
		private bool _needsUpdate;

		// Token: 0x0200093B RID: 2363
		[Serializable]
		public enum ResponsiveScope
		{
			// Token: 0x040030F8 RID: 12536
			Global,
			// Token: 0x040030F9 RID: 12537
			PerDisplayMode
		}

		// Token: 0x0200093C RID: 2364
		[Serializable]
		public enum ResponsiveStrategy
		{
			// Token: 0x040030FB RID: 12539
			FixedSize,
			// Token: 0x040030FC RID: 12540
			FillSpace
		}

		// Token: 0x0200093D RID: 2365
		[Serializable]
		public struct ResponsiveSettings
		{
			// Token: 0x040030FD RID: 12541
			public ResponsivePopUp.ResponsiveStrategy strategy;

			// Token: 0x040030FE RID: 12542
			public Vector2 size;
		}

		// Token: 0x0200093E RID: 2366
		private enum FadeType
		{
			// Token: 0x04003100 RID: 12544
			In,
			// Token: 0x04003101 RID: 12545
			Out
		}
	}
}
