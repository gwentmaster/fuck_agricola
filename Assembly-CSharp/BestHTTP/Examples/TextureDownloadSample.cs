using System;
using UnityEngine;

namespace BestHTTP.Examples
{
	// Token: 0x0200057D RID: 1405
	public sealed class TextureDownloadSample : MonoBehaviour
	{
		// Token: 0x0600339D RID: 13213 RVA: 0x001051C8 File Offset: 0x001033C8
		private void Awake()
		{
			HTTPManager.MaxConnectionPerServer = 1;
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Textures[i] = new Texture2D(100, 150);
			}
		}

		// Token: 0x0600339E RID: 13214 RVA: 0x00105202 File Offset: 0x00103402
		private void OnDestroy()
		{
			HTTPManager.MaxConnectionPerServer = 4;
		}

		// Token: 0x0600339F RID: 13215 RVA: 0x0010520A File Offset: 0x0010340A
		private void OnGUI()
		{
			GUIHelper.DrawArea(GUIHelper.ClientArea, true, delegate
			{
				this.scrollPos = GUILayout.BeginScrollView(this.scrollPos, Array.Empty<GUILayoutOption>());
				int selected = 0;
				Texture[] textures = this.Textures;
				GUILayout.SelectionGrid(selected, textures, 3, Array.Empty<GUILayoutOption>());
				if (this.finishedCount == this.Images.Length && this.allDownloadedFromLocalCache)
				{
					GUIHelper.DrawCenteredText("All images loaded from the local cache!");
				}
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal(Array.Empty<GUILayoutOption>());
				GUILayout.Label("Max Connection/Server: ", new GUILayoutOption[]
				{
					GUILayout.Width(150f)
				});
				GUILayout.Label(HTTPManager.MaxConnectionPerServer.ToString(), new GUILayoutOption[]
				{
					GUILayout.Width(20f)
				});
				HTTPManager.MaxConnectionPerServer = (byte)GUILayout.HorizontalSlider((float)HTTPManager.MaxConnectionPerServer, 1f, 10f, Array.Empty<GUILayoutOption>());
				GUILayout.EndHorizontal();
				if (GUILayout.Button("Start Download", Array.Empty<GUILayoutOption>()))
				{
					this.DownloadImages();
				}
				GUILayout.EndScrollView();
			});
		}

		// Token: 0x060033A0 RID: 13216 RVA: 0x00105224 File Offset: 0x00103424
		private void DownloadImages()
		{
			this.allDownloadedFromLocalCache = true;
			this.finishedCount = 0;
			for (int i = 0; i < this.Images.Length; i++)
			{
				this.Textures[i] = new Texture2D(100, 150);
				new HTTPRequest(new Uri("https://besthttp.azurewebsites.net/Content/" + this.Images[i]), new OnRequestFinishedDelegate(this.ImageDownloaded))
				{
					Tag = this.Textures[i]
				}.Send();
			}
		}

		// Token: 0x060033A1 RID: 13217 RVA: 0x001052A4 File Offset: 0x001034A4
		private void ImageDownloaded(HTTPRequest req, HTTPResponse resp)
		{
			this.finishedCount++;
			switch (req.State)
			{
			case HTTPRequestStates.Finished:
				if (resp.IsSuccess)
				{
					(req.Tag as Texture2D).LoadImage(resp.Data);
					this.allDownloadedFromLocalCache = (this.allDownloadedFromLocalCache && resp.IsFromCache);
					return;
				}
				Debug.LogWarning(string.Format("Request finished Successfully, but the server sent an error. Status Code: {0}-{1} Message: {2}", resp.StatusCode, resp.Message, resp.DataAsText));
				return;
			case HTTPRequestStates.Error:
				Debug.LogError("Request Finished with Error! " + ((req.Exception != null) ? (req.Exception.Message + "\n" + req.Exception.StackTrace) : "No Exception"));
				return;
			case HTTPRequestStates.Aborted:
				Debug.LogWarning("Request Aborted!");
				return;
			case HTTPRequestStates.ConnectionTimedOut:
				Debug.LogError("Connection Timed Out!");
				return;
			case HTTPRequestStates.TimedOut:
				Debug.LogError("Processing the request Timed Out!");
				return;
			default:
				return;
			}
		}

		// Token: 0x040021D2 RID: 8658
		private const string BaseURL = "https://besthttp.azurewebsites.net/Content/";

		// Token: 0x040021D3 RID: 8659
		private string[] Images = new string[]
		{
			"One.png",
			"Two.png",
			"Three.png",
			"Four.png",
			"Five.png",
			"Six.png",
			"Seven.png",
			"Eight.png",
			"Nine.png"
		};

		// Token: 0x040021D4 RID: 8660
		private Texture2D[] Textures = new Texture2D[9];

		// Token: 0x040021D5 RID: 8661
		private bool allDownloadedFromLocalCache;

		// Token: 0x040021D6 RID: 8662
		private int finishedCount;

		// Token: 0x040021D7 RID: 8663
		private Vector2 scrollPos;
	}
}
