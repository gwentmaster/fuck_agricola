using System;
using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000674 RID: 1652
	public class TextureLoader
	{
		// Token: 0x06003CC3 RID: 15555 RVA: 0x0012BC9D File Offset: 0x00129E9D
		public static IEnumerator LoadTexture(string url, MaskableGraphic image, Action<bool, byte[]> afterLoading = null)
		{
			byte[] downloadedBytes = null;
			Texture2D texture2D;
			if (url.StartsWith("http"))
			{
				WWW www = new WWW(url);
				while (!www.isDone)
				{
					yield return null;
				}
				if (!string.IsNullOrEmpty(www.error))
				{
					Hashtable extraInfo = new Hashtable
					{
						{
							"url",
							url
						},
						{
							"error",
							www.error
						}
					};
					AsmoLogger.Error("TextureLoader", "Download failed", extraInfo);
					if (afterLoading != null)
					{
						afterLoading(false, null);
					}
					yield break;
				}
				downloadedBytes = www.bytes;
				if (!TextureLoader.StartWithJPEGHeader(downloadedBytes) && !TextureLoader.StartWithPNGHeader(downloadedBytes))
				{
					AsmoLogger.Error("TextureLoader", "Only JPEG and PNG supported", null);
					if (afterLoading != null)
					{
						afterLoading(false, null);
					}
					yield break;
				}
				texture2D = www.texture;
				www = null;
			}
			else
			{
				downloadedBytes = File.ReadAllBytes(url);
				texture2D = new Texture2D(2, 2, TextureFormat.RGBA32, false, false);
				texture2D.LoadImage(downloadedBytes);
				texture2D.anisoLevel = 16;
			}
			if (image != null && image.gameObject != null && texture2D != null)
			{
				if (image is Image)
				{
					((Image)image).sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
				}
				else if (image is RawImage)
				{
					((RawImage)image).texture = texture2D;
				}
				else
				{
					AsmoLogger.Error("TextureLoader", "Parameter 'image' is not of type Image or RawImage", null);
				}
				yield return null;
				image.enabled = true;
				image.gameObject.SetActive(true);
				yield return null;
				if (afterLoading != null)
				{
					afterLoading(true, downloadedBytes);
				}
			}
			else
			{
				Hashtable extraInfo2 = new Hashtable
				{
					{
						"url",
						url
					},
					{
						"image",
						image
					},
					{
						"texture",
						texture2D
					}
				};
				AsmoLogger.Error("TextureLoader", "Something was wrong with the image or the texture", extraInfo2);
				afterLoading(false, downloadedBytes);
			}
			yield break;
		}

		// Token: 0x06003CC4 RID: 15556 RVA: 0x0012BCBC File Offset: 0x00129EBC
		private static string GetResourcePath(string path)
		{
			path = Path.Combine(Path.GetDirectoryName(path), Path.GetFileNameWithoutExtension(path));
			int num = path.IndexOf("Resources");
			path = path.Substring(num + "Resources/".Length).Replace("\\", "/");
			return path;
		}

		// Token: 0x06003CC5 RID: 15557 RVA: 0x0012BD0C File Offset: 0x00129F0C
		public static bool StartWithJPEGHeader(byte[] data)
		{
			return data != null && data.Length > 4 && (data[0] == byte.MaxValue && data[1] == 216 && data[data.Length - 2] == byte.MaxValue) && data[data.Length - 1] == 217;
		}

		// Token: 0x06003CC6 RID: 15558 RVA: 0x0012BD4B File Offset: 0x00129F4B
		public static bool StartWithPNGHeader(byte[] data)
		{
			return data[0] == 137 && data[1] == 80 && data[2] == 78 && data[3] == 71 && data[4] == 13 && data[5] == 10 && data[6] == 26 && data[7] == 10;
		}

		// Token: 0x04002706 RID: 9990
		private const string _debugModuleName = "TextureLoader";
	}
}
