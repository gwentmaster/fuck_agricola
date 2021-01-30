using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AsmodeeNet.Network;
using AsmodeeNet.Network.RestApi;
using BestHTTP;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.Utils
{
	// Token: 0x02000660 RID: 1632
	public class AvatarManager : MonoBehaviour
	{
		// Token: 0x06003C3A RID: 15418 RVA: 0x0012A359 File Offset: 0x00128559
		public void ClearCache()
		{
			this._userIdToAvatarURL.Clear();
			this._cachedAvatars.Clear();
		}

		// Token: 0x06003C3B RID: 15419 RVA: 0x0012A371 File Offset: 0x00128571
		public void UpdateCacheURLForUserId(int userId, string url)
		{
			if (userId <= 0)
			{
				throw new ArgumentException("UpdateCacheURLForUserId called with invalid userId");
			}
			if (string.IsNullOrEmpty(url))
			{
				throw new ArgumentException("UpdateCacheURLForUserId called with empty url");
			}
			this._userIdToAvatarURL[userId] = url;
		}

		// Token: 0x06003C3C RID: 15420 RVA: 0x0012A3A4 File Offset: 0x001285A4
		public AvatarManager.RetrievalHandle CacheURLForUserIds(int[] userIds, Action<bool> onCompletion, AvatarManager.RetrievalHandle handle = null)
		{
			if (userIds == null || userIds.Length == 0)
			{
				throw new ArgumentException("CacheURLForUserIds called without userIds");
			}
			if (handle == null)
			{
				handle = new AvatarManager.RetrievalHandle();
			}
			AsmoLogger.Debug("AvatarManager", "Searching for avatar URLs", new Hashtable
			{
				{
					"userIds",
					userIds
				}
			});
			handle.endpoint = new SearchByIdEndpoint(userIds, Extras.None, 0, 100, null);
			Action<PaginatedResult<UserSearchResult>, WebError> searchCompletion = null;
			searchCompletion = delegate(PaginatedResult<UserSearchResult> result, WebError webError)
			{
				if (webError == null)
				{
					foreach (KeyValuePair<int, string> keyValuePair in from x in result.Elements
					select new KeyValuePair<int, string>(x.UserId, x.Avatar))
					{
						this.UpdateCacheURLForUserId(keyValuePair.Key, keyValuePair.Value);
					}
					if (result.Next != null)
					{
						result.Next(searchCompletion);
						return;
					}
					handle.endpoint = null;
					if (onCompletion != null)
					{
						onCompletion(true);
						return;
					}
				}
				else if (onCompletion != null)
				{
					onCompletion(false);
				}
			};
			handle.endpoint.Execute(searchCompletion);
			return handle;
		}

		// Token: 0x06003C3D RID: 15421 RVA: 0x0012A45C File Offset: 0x0012865C
		public AvatarManager.RetrievalHandle LoadPlayerAvatar(int userId, Image image, Action<bool> onCompletion)
		{
			AvatarManager.<>c__DisplayClass11_0 CS$<>8__locals1 = new AvatarManager.<>c__DisplayClass11_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.userId = userId;
			CS$<>8__locals1.image = image;
			CS$<>8__locals1.onCompletion = onCompletion;
			if (CS$<>8__locals1.userId <= 0)
			{
				throw new ArgumentException("LoadPlayerAvatar called with invalid userId");
			}
			if (CS$<>8__locals1.image == null)
			{
				throw new ArgumentException("LoadPlayerAvatar called with null image");
			}
			if (this._userIdToAvatarURL.ContainsKey(CS$<>8__locals1.userId))
			{
				return this.LoadPlayerAvatar(CS$<>8__locals1.userId, this._userIdToAvatarURL[CS$<>8__locals1.userId], CS$<>8__locals1.image, CS$<>8__locals1.onCompletion, null);
			}
			AvatarManager.RetrievalHandle handle = new AvatarManager.RetrievalHandle();
			this.CacheURLForUserIds(new int[]
			{
				CS$<>8__locals1.userId
			}, delegate(bool cacheSucceeded)
			{
				if (cacheSucceeded && CS$<>8__locals1.<>4__this._userIdToAvatarURL.ContainsKey(CS$<>8__locals1.userId))
				{
					CS$<>8__locals1.<>4__this.LoadPlayerAvatar(CS$<>8__locals1.userId, CS$<>8__locals1.<>4__this._userIdToAvatarURL[CS$<>8__locals1.userId], CS$<>8__locals1.image, CS$<>8__locals1.onCompletion, handle);
					return;
				}
				CS$<>8__locals1.image.sprite = CS$<>8__locals1.<>4__this.defaultAvatar;
				if (CS$<>8__locals1.onCompletion != null)
				{
					CS$<>8__locals1.onCompletion(false);
				}
			}, handle);
			return handle;
		}

		// Token: 0x06003C3E RID: 15422 RVA: 0x0012A544 File Offset: 0x00128744
		public AvatarManager.RetrievalHandle LoadPlayerAvatar(int userId, string url, Image image, Action<bool> onCompletion = null, AvatarManager.RetrievalHandle handle = null)
		{
			if (image == null)
			{
				throw new ArgumentException("LoadPlayerAvatar called with null image");
			}
			for (int i = 0; i < this._cachedAvatars.Count; i++)
			{
				AvatarManager.Avatar avatar = this._cachedAvatars[i];
				if (avatar.userId == userId)
				{
					this._cachedAvatars.RemoveAt(i);
					this._cachedAvatars.Insert(0, avatar);
					image.sprite = avatar.sprite;
					if (onCompletion != null)
					{
						onCompletion(true);
					}
					return null;
				}
			}
			string uriString = string.IsNullOrEmpty(url) ? "https://cdn.daysofwonder.com/images/avatars/avatar-neutral.jpg" : url;
			if (handle == null)
			{
				handle = new AvatarManager.RetrievalHandle();
			}
			AsmoLogger.Debug("AvatarManager", "Downloading avatar", new Hashtable
			{
				{
					"userId",
					userId
				}
			});
			handle.httpRequest = new HTTPRequest(new Uri(uriString), delegate(HTTPRequest req, HTTPResponse response)
			{
				handle.httpRequest = null;
				bool obj = false;
				if (response != null && response.Data != null)
				{
					if (TextureLoader.StartWithJPEGHeader(response.Data) || TextureLoader.StartWithPNGHeader(response.Data))
					{
						Texture2D texture2D = new Texture2D(0, 0);
						texture2D.LoadImage(response.Data);
						Sprite sprite = Sprite.Create(texture2D, new Rect(0f, 0f, (float)texture2D.width, (float)texture2D.height), Vector2.zero);
						AvatarManager.Avatar item = default(AvatarManager.Avatar);
						item.userId = userId;
						item.sprite = sprite;
						this._cachedAvatars.Insert(0, item);
						if ((long)this._cachedAvatars.Count > (long)((ulong)this.cacheCapacity))
						{
							this._cachedAvatars.RemoveRange((int)this.cacheCapacity, this._cachedAvatars.Count - (int)this.cacheCapacity);
						}
						image.sprite = sprite;
						obj = true;
					}
					else
					{
						image.sprite = this.defaultAvatar;
					}
				}
				if (onCompletion != null)
				{
					onCompletion(obj);
				}
			});
			handle.httpRequest.Send();
			return handle;
		}

		// Token: 0x040026E4 RID: 9956
		public Sprite defaultAvatar;

		// Token: 0x040026E5 RID: 9957
		public uint cacheCapacity = 100U;

		// Token: 0x040026E6 RID: 9958
		private const string _debugModuleName = "AvatarManager";

		// Token: 0x040026E7 RID: 9959
		private const string _defaultAvatarLocation = "https://cdn.daysofwonder.com/images/avatars/avatar-neutral.jpg";

		// Token: 0x040026E8 RID: 9960
		private Dictionary<int, string> _userIdToAvatarURL = new Dictionary<int, string>();

		// Token: 0x040026E9 RID: 9961
		private List<AvatarManager.Avatar> _cachedAvatars = new List<AvatarManager.Avatar>();

		// Token: 0x0200097C RID: 2428
		public class RetrievalHandle
		{
			// Token: 0x060047E9 RID: 18409 RVA: 0x0014A4FE File Offset: 0x001486FE
			public void Abort()
			{
				if (this.endpoint != null)
				{
					this.endpoint.Abort();
					this.endpoint = null;
				}
				if (this.httpRequest != null)
				{
					this.httpRequest.Abort();
					this.httpRequest = null;
				}
			}

			// Token: 0x040031EE RID: 12782
			public SearchByIdEndpoint endpoint;

			// Token: 0x040031EF RID: 12783
			public HTTPRequest httpRequest;
		}

		// Token: 0x0200097D RID: 2429
		private struct Avatar
		{
			// Token: 0x040031F0 RID: 12784
			public int userId;

			// Token: 0x040031F1 RID: 12785
			public Sprite sprite;
		}
	}
}
