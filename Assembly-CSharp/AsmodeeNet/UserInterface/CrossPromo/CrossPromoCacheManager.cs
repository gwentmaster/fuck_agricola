using System;
using System.Collections;
using System.IO;
using AsmodeeNet.Foundation;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using AsmodeeNet.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.CrossPromo
{
	// Token: 0x02000653 RID: 1619
	public static class CrossPromoCacheManager
	{
		// Token: 0x170007FC RID: 2044
		// (get) Token: 0x06003BCC RID: 15308 RVA: 0x001287D8 File Offset: 0x001269D8
		private static string _LanguageCode
		{
			get
			{
				if (CoreApplication.Instance != null)
				{
					return CoreApplication.Instance.LocalizationManager.CurrentLanguageCode;
				}
				return null;
			}
		}

		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x06003BCD RID: 15309 RVA: 0x001287F8 File Offset: 0x001269F8
		private static Channel _Channel
		{
			get
			{
				return CoreApplication.Instance.Channel;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06003BCE RID: 15310 RVA: 0x00128804 File Offset: 0x00126A04
		private static string _BasePath
		{
			get
			{
				return Path.Combine(Application.persistentDataPath, "AsmodeeDigital/CrossPromo/Cache");
			}
		}

		// Token: 0x06003BCF RID: 15311 RVA: 0x00128818 File Offset: 0x00126A18
		public static void SaveGroupProductInCache(ShowcaseProduct[] groupProduct, GameProductTag? filter = null)
		{
			string path = string.Empty;
			if (filter == null)
			{
				path = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("interstitial.json", Array.Empty<object>()));
			}
			else
			{
				path = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("GroupProducts/{0}.json", filter.ToString()));
			}
			Directory.CreateDirectory(Path.GetDirectoryName(path));
			foreach (ShowcaseProduct showcaseProduct in groupProduct)
			{
				string pathProductJson = Path.Combine(Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/", showcaseProduct.Id)), string.Format("product_{0}.json", showcaseProduct.Id));
				CrossPromoCacheManager.SaveProductInCache(showcaseProduct, pathProductJson);
			}
		}

		// Token: 0x06003BD0 RID: 15312 RVA: 0x001288D8 File Offset: 0x00126AD8
		public static void SaveBannerInCache(ShowcaseProduct product)
		{
			string pathProductJson = Path.Combine(CrossPromoCacheManager._BasePath, "banner.json");
			CrossPromoCacheManager.SaveProductInCache(product, pathProductJson);
		}

		// Token: 0x06003BD1 RID: 15313 RVA: 0x001288FC File Offset: 0x00126AFC
		private static void SaveProductInCache(ShowcaseProduct product, string pathProductJson)
		{
			string path = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/", product.Id));
			if (!CrossPromoCacheManager.FreshResourceInCache(pathProductJson))
			{
				Directory.CreateDirectory(path);
				Directory.CreateDirectory(Path.GetDirectoryName(pathProductJson));
				string contents = product.ToJson();
				File.WriteAllText(pathProductJson, contents);
			}
		}

		// Token: 0x06003BD2 RID: 15314 RVA: 0x00128954 File Offset: 0x00126B54
		public static void LoadMoreGame(GameProductTag? filter, Action<ShowcaseProduct[]> onSucceed, Action onCannotLoadMoreGames)
		{
			if (CrossPromoCacheManager._requestGamesEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager._requestGamesEndpoint = ((filter == null) ? new RequestGamesEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null) : new RequestGamesEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, filter.Value, null));
			}
			catch
			{
				if (onCannotLoadMoreGames != null)
				{
					onCannotLoadMoreGames();
				}
				CrossPromoCacheManager._requestGamesEndpoint = null;
				return;
			}
			CrossPromoCacheManager._requestGamesEndpoint.Execute(delegate(ShowcaseProduct[] result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveGroupProductInCache(result, filter);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadMoreGames != null)
				{
					onCannotLoadMoreGames();
				}
				CrossPromoCacheManager._requestGamesEndpoint = null;
			});
		}

		// Token: 0x06003BD3 RID: 15315 RVA: 0x00128A08 File Offset: 0x00126C08
		public static void CancelLoadMoreGame()
		{
			if (CrossPromoCacheManager._requestGamesEndpoint != null)
			{
				CrossPromoCacheManager._requestGamesEndpoint = null;
				CrossPromoCacheManager._requestGamesEndpoint.Abort();
			}
		}

		// Token: 0x06003BD4 RID: 15316 RVA: 0x00128A24 File Offset: 0x00126C24
		public static void LoadInterstitial(Action<ShowcaseProduct[]> onSucceed, Action onCannotLoadInterstitial)
		{
			if (CrossPromoCacheManager._requestInterstitialEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager._requestInterstitialEndpoint = new RequestInterstitialEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null);
			}
			catch
			{
				if (onCannotLoadInterstitial != null)
				{
					onCannotLoadInterstitial();
				}
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
				return;
			}
			CrossPromoCacheManager._requestInterstitialEndpoint.Execute(delegate(ShowcaseProduct[] result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveGroupProductInCache(result, null);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadInterstitial != null)
				{
					onCannotLoadInterstitial();
				}
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
			});
		}

		// Token: 0x06003BD5 RID: 15317 RVA: 0x00128AA8 File Offset: 0x00126CA8
		public static void CancelLoadInterstitial()
		{
			if (CrossPromoCacheManager._requestInterstitialEndpoint != null)
			{
				CrossPromoCacheManager._requestInterstitialEndpoint = null;
				CrossPromoCacheManager._requestInterstitialEndpoint.Abort();
			}
		}

		// Token: 0x06003BD6 RID: 15318 RVA: 0x00128AC4 File Offset: 0x00126CC4
		public static void LoadBanner(Action<ShowcaseProduct> onSucceed, Action onCannotLoadBanner)
		{
			if (CrossPromoCacheManager.requestBannerEndpoint != null)
			{
				return;
			}
			try
			{
				CrossPromoCacheManager.requestBannerEndpoint = new RequestBannerEndpoint(CrossPromoCacheManager._Channel, CrossPromoCacheManager._LanguageCode, null);
			}
			catch
			{
				if (onCannotLoadBanner != null)
				{
					onCannotLoadBanner();
				}
				CrossPromoCacheManager.requestBannerEndpoint = null;
				return;
			}
			CrossPromoCacheManager.requestBannerEndpoint.Execute(delegate(ShowcaseProduct result, WebError webError)
			{
				if (webError == null)
				{
					CrossPromoCacheManager.SaveBannerInCache(result);
					if (onSucceed != null)
					{
						onSucceed(result);
					}
				}
				else if (onCannotLoadBanner != null)
				{
					onCannotLoadBanner();
				}
				CrossPromoCacheManager.requestBannerEndpoint = null;
			});
		}

		// Token: 0x06003BD7 RID: 15319 RVA: 0x00128B48 File Offset: 0x00126D48
		public static void CancelLoadBanner()
		{
			if (CrossPromoCacheManager.requestBannerEndpoint != null)
			{
				CrossPromoCacheManager.requestBannerEndpoint.Abort();
				CrossPromoCacheManager.requestBannerEndpoint = null;
			}
		}

		// Token: 0x06003BD8 RID: 15320 RVA: 0x00128B61 File Offset: 0x00126D61
		public static IEnumerator LoadProductTileImage(ShowcaseProduct product, RawImage image, Action<bool> afterLoading)
		{
			string pathLocalCache = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/tile_url_{1}x{2}{3}", new object[]
			{
				product.Id,
				product.Tile.Width,
				product.Tile.Height,
				Path.GetExtension(product.Tile.ImageUrl)
			}));
			yield return CrossPromoCacheManager.LoadTexture(product.Tile.ImageUrl, pathLocalCache, image, afterLoading);
			yield break;
		}

		// Token: 0x06003BD9 RID: 15321 RVA: 0x00128B7E File Offset: 0x00126D7E
		public static IEnumerator LoadProductIcon(ShowcaseProduct product, Image image, Action<bool> afterLoading)
		{
			string pathLocalCache = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/icon_url{1}", product.Id, Path.GetExtension(product.IconUrl)));
			yield return CrossPromoCacheManager.LoadTexture(product.IconUrl, pathLocalCache, image, afterLoading);
			yield break;
		}

		// Token: 0x06003BDA RID: 15322 RVA: 0x00128B9B File Offset: 0x00126D9B
		public static IEnumerator LoadProductImage(ShowcaseProduct product, string pathImage, Image image, Action<bool> afterLoading)
		{
			string pathLocalCache = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/Images/{1}", product.Id, Path.GetFileName(pathImage)));
			yield return CrossPromoCacheManager.LoadTexture(pathImage, pathLocalCache, image, afterLoading);
			yield break;
		}

		// Token: 0x06003BDB RID: 15323 RVA: 0x00128BBF File Offset: 0x00126DBF
		public static IEnumerator LoadProductAward(ShowcaseProduct product, string pathImage, Image image, Action<bool> afterLoading)
		{
			string pathLocalCache = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/Awards/{1}", product.Id, Path.GetFileName(pathImage)));
			yield return CrossPromoCacheManager.LoadTexture(pathImage, pathLocalCache, image, afterLoading);
			yield break;
		}

		// Token: 0x06003BDC RID: 15324 RVA: 0x00128BE3 File Offset: 0x00126DE3
		public static IEnumerator LoadBannerImage(ShowcaseProduct product, RawImage image, Action<bool> afterLoading)
		{
			string pathLocalCache = Path.Combine(CrossPromoCacheManager._BasePath, string.Format("Product/{0}/banner_url{1}", product.Id, Path.GetExtension(product.BannerUrl)));
			yield return CrossPromoCacheManager.LoadTexture(product.BannerUrl, pathLocalCache, image, afterLoading);
			yield break;
		}

		// Token: 0x06003BDD RID: 15325 RVA: 0x00128C00 File Offset: 0x00126E00
		private static bool FreshResourceInCache(string pathCache)
		{
			if (!string.IsNullOrEmpty(pathCache) && File.Exists(pathCache))
			{
				DateTime lastWriteTime = File.GetLastWriteTime(pathCache);
				return DateTime.Now.Subtract(lastWriteTime).TotalDays <= 1.0;
			}
			return false;
		}

		// Token: 0x06003BDE RID: 15326 RVA: 0x00128C4A File Offset: 0x00126E4A
		private static IEnumerator LoadTexture(string path, string pathLocalCache, MaskableGraphic image, Action<bool> onComplete)
		{
			if (CrossPromoCacheManager.FreshResourceInCache(pathLocalCache))
			{
				yield return CrossPromoCacheManager.LoadAndWriteTexture(pathLocalCache, pathLocalCache, image, onComplete);
			}
			else
			{
				AsmoLogger.Warning("CrossPromoCacheManager", "Asset needs to be refreshed", new Hashtable
				{
					{
						"path",
						path
					}
				});
				yield return CrossPromoCacheManager.LoadAndWriteTexture(path, pathLocalCache, image, onComplete);
			}
			yield break;
		}

		// Token: 0x06003BDF RID: 15327 RVA: 0x00128C6E File Offset: 0x00126E6E
		private static IEnumerator LoadAndWriteTexture(string path, string pathLocalCache, MaskableGraphic image, Action<bool> OnComplete)
		{
			yield return TextureLoader.LoadTexture(path, image, delegate(bool success, byte[] bytes)
			{
				if (!success)
				{
					if (OnComplete != null)
					{
						OnComplete(false);
					}
					return;
				}
				if (OnComplete != null)
				{
					OnComplete(true);
				}
				if (path.StartsWith("http"))
				{
					if (!Directory.Exists(Path.GetDirectoryName(pathLocalCache)))
					{
						Directory.CreateDirectory(Path.GetDirectoryName(pathLocalCache));
					}
					File.WriteAllBytes(pathLocalCache, bytes);
				}
			});
			yield break;
		}

		// Token: 0x04002683 RID: 9859
		private static RequestGamesEndpoint _requestGamesEndpoint;

		// Token: 0x04002684 RID: 9860
		private static RequestInterstitialEndpoint _requestInterstitialEndpoint;

		// Token: 0x04002685 RID: 9861
		private static RequestBannerEndpoint requestBannerEndpoint;
	}
}
