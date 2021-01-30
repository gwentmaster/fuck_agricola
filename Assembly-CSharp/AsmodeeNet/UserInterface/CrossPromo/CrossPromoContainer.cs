using System;
using AsmodeeNet.Network.RestApi;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000654 RID: 1620
	public class CrossPromoContainer : UIBehaviour
	{
		// Token: 0x06003BE0 RID: 15328 RVA: 0x00128C94 File Offset: 0x00126E94
		public virtual void Init(GameDetailsPopup popup, int imageIndex = -1)
		{
			this._popup = popup;
			this.uiContainer.Image.sprite = null;
			this.uiContainer.Image.gameObject.SetActive(false);
			this._imagesIndex = imageIndex;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003BE1 RID: 15329 RVA: 0x00128CE2 File Offset: 0x00126EE2
		public void LoadImage(ShowcaseProduct product)
		{
			this._DisplayImage(product, CrossPromoContainer.Imagetype.Screenshoot);
		}

		// Token: 0x06003BE2 RID: 15330 RVA: 0x00128CEC File Offset: 0x00126EEC
		public void LoadThumbnail(ShowcaseProduct product)
		{
			this._DisplayImage(product, CrossPromoContainer.Imagetype.Thumbnail);
		}

		// Token: 0x06003BE3 RID: 15331 RVA: 0x00128CF8 File Offset: 0x00126EF8
		private void _DisplayImage(ShowcaseProduct product, CrossPromoContainer.Imagetype type)
		{
			if (this._imagesIndex == -1)
			{
				return;
			}
			if (this._image[(int)type] == null || this._imageLoadFailed[(int)type])
			{
				this.uiContainer.Loading.gameObject.SetActive(true);
				base.StartCoroutine(CrossPromoCacheManager.LoadProductImage(product, (type == CrossPromoContainer.Imagetype.Thumbnail) ? product.Images[this._imagesIndex].ThumbUrl : product.Images[this._imagesIndex].ImageUrl, this.uiContainer.Image, delegate(bool success)
				{
					this._imageLoadFailed[(int)type] = !success;
					this._image[(int)type] = this.uiContainer.Image;
					this.uiContainer.Loading.gameObject.SetActive(false);
				}));
				return;
			}
			this.uiContainer.Image = this._image[(int)type];
		}

		// Token: 0x06003BE4 RID: 15332 RVA: 0x00128DC8 File Offset: 0x00126FC8
		public void HideLoading()
		{
			this.uiContainer.Loading.gameObject.SetActive(false);
		}

		// Token: 0x06003BE5 RID: 15333 RVA: 0x00128DE0 File Offset: 0x00126FE0
		public void ImageContainer_Clicked()
		{
			this._popup.Zoom(this);
		}

		// Token: 0x06003BE6 RID: 15334 RVA: 0x00128DEE File Offset: 0x00126FEE
		protected override void OnDisable()
		{
			base.OnDestroy();
			if (this.destroyOnDisable)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}

		// Token: 0x04002686 RID: 9862
		public bool destroyOnDisable;

		// Token: 0x04002687 RID: 9863
		public CrossPromoContainer.UIContainer uiContainer;

		// Token: 0x04002688 RID: 9864
		protected GameDetailsPopup _popup;

		// Token: 0x04002689 RID: 9865
		protected ShowcaseProduct _showcaseProduct;

		// Token: 0x0400268A RID: 9866
		protected int _imagesIndex = -1;

		// Token: 0x0400268B RID: 9867
		private Image[] _image = new Image[2];

		// Token: 0x0400268C RID: 9868
		private bool[] _imageLoadFailed = new bool[2];

		// Token: 0x0400268D RID: 9869
		private CrossPromoContainer.Imagetype _imagetype;

		// Token: 0x0200096C RID: 2412
		[Serializable]
		public class UIContainer
		{
			// Token: 0x040031B5 RID: 12725
			public RectTransform RectTransform;

			// Token: 0x040031B6 RID: 12726
			public Transform Loading;

			// Token: 0x040031B7 RID: 12727
			public LayoutElement Layout;

			// Token: 0x040031B8 RID: 12728
			public Image Image;
		}

		// Token: 0x0200096D RID: 2413
		private enum Imagetype
		{
			// Token: 0x040031BA RID: 12730
			Thumbnail,
			// Token: 0x040031BB RID: 12731
			Screenshoot
		}
	}
}
