using System;
using System.Collections.Generic;
using AsmodeeNet.Analytics;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network.RestApi;
using UnityEngine;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000651 RID: 1617
	public abstract class BaseGroupOfProductPopup : CrossPromoBasePopup
	{
		// Token: 0x06003BBF RID: 15295 RVA: 0x0012842C File Offset: 0x0012662C
		protected virtual void OnEnable()
		{
			CoreApplication.Instance.Preferences.AspectDidChange += this.SetNeedsUpdate;
			CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange += this.SetNeedsUpdate;
		}

		// Token: 0x06003BC0 RID: 15296 RVA: 0x00128464 File Offset: 0x00126664
		protected virtual void OnDisable()
		{
			if (!CoreApplication.IsQuitting)
			{
				CoreApplication.Instance.Preferences.AspectDidChange -= this.SetNeedsUpdate;
				CoreApplication.Instance.Preferences.InterfaceDisplayModeDidChange -= this.SetNeedsUpdate;
			}
		}

		// Token: 0x06003BC1 RID: 15297 RVA: 0x001284A3 File Offset: 0x001266A3
		private void SetNeedsUpdate()
		{
			this._needsUpdate = true;
		}

		// Token: 0x06003BC2 RID: 15298 RVA: 0x001284AC File Offset: 0x001266AC
		private void LateUpdate()
		{
			if (!this._needsUpdate)
			{
				return;
			}
			this.ReloadProducts(null);
		}

		// Token: 0x06003BC3 RID: 15299 RVA: 0x001284BE File Offset: 0x001266BE
		public void ReloadProducts(ShowcaseProduct[] products = null)
		{
			if (products != null)
			{
				this._products = products;
			}
			if (this._products == null)
			{
				return;
			}
			this._ShowGroupOfProducts(this._products);
		}

		// Token: 0x06003BC4 RID: 15300 RVA: 0x001284E0 File Offset: 0x001266E0
		protected virtual void _ShowGroupOfProducts(ShowcaseProduct[] products)
		{
			this._needsUpdate = false;
			this.content.anchoredPosition = Vector2.zero;
			this.content.sizeDelta = Vector2.zero;
			this._tilesUI.ForEach(delegate(TileContainer t)
			{
				UnityEngine.Object.Destroy(t.gameObject);
			});
			this._tilesUI.Clear();
			int cellSizeInPixels = (int)((this.content.rect.width - (float)((this.nbColumns - 1) * this.spacingX)) / (float)this.nbColumns);
			int num = this.nbColumns;
			int num2 = 100;
			bool[,] array = new bool[num, num2];
			foreach (ShowcaseProduct showcaseProduct in products)
			{
				bool flag = false;
				int num3 = 0;
				int num4 = 0;
				while (num4 < num2 && !flag)
				{
					num3 = 0;
					while (num3 < num && !flag)
					{
						bool flag2 = true;
						int num5 = 0;
						while (num5 < showcaseProduct.Tile.Width && flag2)
						{
							int num6 = 0;
							while (num6 < showcaseProduct.Tile.Height && flag2)
							{
								if (num3 + num5 > num - 1 || num4 + num6 > num2 - 1 || array[num3 + num5, num4 + num6])
								{
									flag2 = false;
								}
								num6++;
							}
							num5++;
						}
						if (flag2)
						{
							flag = true;
						}
						num3++;
					}
					num4++;
				}
				if (flag)
				{
					num3--;
					num4--;
					for (int j = 0; j < showcaseProduct.Tile.Width; j++)
					{
						for (int k = 0; k < showcaseProduct.Tile.Height; k++)
						{
							array[num3 + j, num4 + k] = true;
						}
					}
					TileContainer tileContainer = UnityEngine.Object.Instantiate<TileContainer>(this.TilePrefab, this.content, false);
					this._tilesUI.Add(tileContainer);
					Rect rect = tileContainer.Init(showcaseProduct, num3, num4, cellSizeInPixels, this.spacingX, this.spacingY, delegate
					{
						this.Dismiss();
					});
					Vector2 sizeDelta = new Vector2(0f, Math.Max(rect.y + rect.height, this.content.sizeDelta.y));
					this.content.sizeDelta = sizeDelta;
				}
			}
		}

		// Token: 0x06003BC5 RID: 15301 RVA: 0x0012873A File Offset: 0x0012693A
		public override void Dismiss()
		{
			base.Dismiss();
			AnalyticsEvents.LogCrossPromoClosedEvent(null);
		}

		// Token: 0x04002679 RID: 9849
		public RectTransform content;

		// Token: 0x0400267A RID: 9850
		public int nbColumns = 3;

		// Token: 0x0400267B RID: 9851
		public int spacingX = 10;

		// Token: 0x0400267C RID: 9852
		public int spacingY = 10;

		// Token: 0x0400267D RID: 9853
		public TileContainer TilePrefab;

		// Token: 0x0400267E RID: 9854
		protected ShowcaseProduct[] _products;

		// Token: 0x0400267F RID: 9855
		private List<TileContainer> _tilesUI = new List<TileContainer>();

		// Token: 0x04002680 RID: 9856
		private bool _needsUpdate;
	}
}
