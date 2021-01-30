using System;
using AsmodeeNet.Foundation;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x0200064F RID: 1615
	public class BannerImageFitter : MonoBehaviour
	{
		// Token: 0x170007FA RID: 2042
		// (get) Token: 0x06003BAD RID: 15277 RVA: 0x0012813F File Offset: 0x0012633F
		// (set) Token: 0x06003BAE RID: 15278 RVA: 0x00128147 File Offset: 0x00126347
		public BannerImageFitter.BannerPosition Position
		{
			get
			{
				return this._position;
			}
			set
			{
				if (this._position == value)
				{
					return;
				}
				this._position = value;
				this._RefreshUI();
			}
		}

		// Token: 0x06003BAF RID: 15279 RVA: 0x00128160 File Offset: 0x00126360
		private void _RefreshUI()
		{
			RectTransform rectTransform = base.transform.parent as RectTransform;
			float num = Mathf.Max(rectTransform.rect.width, rectTransform.rect.height) / 8f;
			RectTransform rectTransform2 = base.transform as RectTransform;
			rectTransform2.sizeDelta = new Vector2(num * 5f, num);
			if (this.Position == BannerImageFitter.BannerPosition.Top)
			{
				rectTransform2.anchorMax = new Vector2(0.5f, 1f);
				rectTransform2.anchorMin = new Vector2(0.5f, 1f);
				rectTransform2.anchoredPosition = new Vector2(0f, -rectTransform2.rect.height / 2f);
				return;
			}
			rectTransform2.anchorMax = new Vector2(0.5f, 0f);
			rectTransform2.anchorMin = new Vector2(0.5f, 0f);
			rectTransform2.anchoredPosition = new Vector2(0f, rectTransform2.rect.height / 2f);
		}

		// Token: 0x06003BB0 RID: 15280 RVA: 0x0012826B File Offset: 0x0012646B
		private void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this._RefreshUI;
			this._RefreshUI();
		}

		// Token: 0x06003BB1 RID: 15281 RVA: 0x0012828E File Offset: 0x0012648E
		private void OnDisable()
		{
			if (CoreApplication.IsQuitting)
			{
				return;
			}
			CoreApplication.Instance.Preferences.AspectDidChange -= this._RefreshUI;
		}

		// Token: 0x04002674 RID: 9844
		private BannerImageFitter.BannerPosition _position;

		// Token: 0x0200095F RID: 2399
		public enum BannerPosition
		{
			// Token: 0x04003180 RID: 12672
			Top,
			// Token: 0x04003181 RID: 12673
			Bottom
		}
	}
}
