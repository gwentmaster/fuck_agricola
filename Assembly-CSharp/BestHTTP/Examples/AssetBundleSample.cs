using System;
using System.Collections;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200057B RID: 1403
	public sealed class AssetBundleSample : MonoBehaviour
	{
		// Token: 0x0600338E RID: 13198 RVA: 0x00104C44 File Offset: 0x00102E44
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				GUILayout.Label("Status: " + this.status, Array.Empty<GUILayoutOption>());
				if (this.texture != null)
				{
					GUILayout.Box(this.texture, new GUILayoutOption[]
					{
						GUILayout.MaxHeight(256f)
					});
				}
				if (!this.downloading && GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
				{
					this.UnloadBundle();
					base.StartCoroutine(this.DownloadAssetBundle());
				}
			});
		}

		// Token: 0x0600338F RID: 13199 RVA: 0x00104C5D File Offset: 0x00102E5D
		private void OnDestroy()
		{
			this.UnloadBundle();
		}

		// Token: 0x06003390 RID: 13200 RVA: 0x00104C65 File Offset: 0x00102E65
		private IEnumerator DownloadAssetBundle()
		{
			this.downloading = true;
			HTTPRequest request = new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/AssetBundle.html")).Send();
			this.status = "Download started";
			while (request.State < HTTPRequestStates.Finished)
			{
				yield return new WaitForSeconds(0.1f);
				this.status += ".";
			}
			switch (request.State)
			{
			case HTTPRequestStates.Finished:
				if (request.Response.IsSuccess)
				{
					this.status = string.Format("AssetBundle downloaded! Loaded from local cache: {0}", request.Response.IsFromCache.ToString());
					AssetBundleCreateRequest async = AssetBundle.LoadFromMemoryAsync(request.Response.Data);
					yield return async;
					yield return base.StartCoroutine(this.ProcessAssetBundle(async.assetBundle));
					async = null;
				}
				else
				{
					this.status = string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", request.Response.StatusCode, request.Response.Message, request.Response.DataAsText);
					Debug.LogWarning(this.status);
				}
				break;
			case HTTPRequestStates.Error:
				this.status = "Request Finished with Error! " + ((request.Exception != null) ? (request.Exception.Message + "\n" + request.Exception.StackTrace) : "No Exception");
				Debug.LogError(this.status);
				break;
			case HTTPRequestStates.Aborted:
				this.status = "Request Aborted!";
				Debug.LogWarning(this.status);
				break;
			case HTTPRequestStates.ConnectionTimedOut:
				this.status = "Connection Timed Out!";
				Debug.LogError(this.status);
				break;
			case HTTPRequestStates.TimedOut:
				this.status = "Processing the request Timed Out!";
				Debug.LogError(this.status);
				break;
			}
			this.downloading = false;
			yield break;
		}

		// Token: 0x06003391 RID: 13201 RVA: 0x00104C74 File Offset: 0x00102E74
		private IEnumerator ProcessAssetBundle(AssetBundle bundle)
		{
			if (bundle == null)
			{
				yield break;
			}
			this.cachedBundle = bundle;
			AssetBundleRequest asyncAsset = this.cachedBundle.LoadAssetAsync("9443182_orig", typeof(Texture2D));
			yield return asyncAsset;
			this.texture = (asyncAsset.asset as Texture2D);
			yield break;
		}

		// Token: 0x06003392 RID: 13202 RVA: 0x00104C8A File Offset: 0x00102E8A
		private void UnloadBundle()
		{
			if (this.cachedBundle != null)
			{
				this.cachedBundle.Unload(true);
				this.cachedBundle = null;
			}
		}

		// Token: 0x040021C8 RID: 8648
		private const string URL = "https://besthttp.azurewebsites.net/Content/AssetBundle.html";

		// Token: 0x040021C9 RID: 8649
		private string status = "Waiting for user interaction";

		// Token: 0x040021CA RID: 8650
		private AssetBundle cachedBundle;

		// Token: 0x040021CB RID: 8651
		private Texture2D texture;

		// Token: 0x040021CC RID: 8652
		private bool downloading;
	}
}
