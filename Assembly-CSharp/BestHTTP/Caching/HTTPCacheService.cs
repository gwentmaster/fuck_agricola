using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Threading;
using BestHTTP.Logger;

namespace BestHTTP.Caching
{
	// Token: 0x02000617 RID: 1559
	public static class HTTPCacheService
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600394E RID: 14670 RVA: 0x0011CF40 File Offset: 0x0011B140
		public static bool IsSupported
		{
			get
			{
				if (HTTPCacheService.IsSupportCheckDone)
				{
					return HTTPCacheService.isSupported;
				}
				try
				{
					File.Exists(HTTPManager.GetRootCacheFolder());
					HTTPCacheService.isSupported = true;
				}
				catch
				{
					HTTPCacheService.isSupported = false;
					HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
				}
				finally
				{
					HTTPCacheService.IsSupportCheckDone = true;
				}
				return HTTPCacheService.isSupported;
			}
		}

		// Token: 0x1700079B RID: 1947
		// (get) Token: 0x0600394F RID: 14671 RVA: 0x0011CFB4 File Offset: 0x0011B1B4
		private static Dictionary<Uri, HTTPCacheFileInfo> Library
		{
			get
			{
				HTTPCacheService.LoadLibrary();
				return HTTPCacheService.library;
			}
		}

		// Token: 0x1700079C RID: 1948
		// (get) Token: 0x06003950 RID: 14672 RVA: 0x0011CFC0 File Offset: 0x0011B1C0
		// (set) Token: 0x06003951 RID: 14673 RVA: 0x0011CFC7 File Offset: 0x0011B1C7
		internal static string CacheFolder { get; private set; }

		// Token: 0x1700079D RID: 1949
		// (get) Token: 0x06003952 RID: 14674 RVA: 0x0011CFCF File Offset: 0x0011B1CF
		// (set) Token: 0x06003953 RID: 14675 RVA: 0x0011CFD6 File Offset: 0x0011B1D6
		private static string LibraryPath { get; set; }

		// Token: 0x06003955 RID: 14677 RVA: 0x0011CFF4 File Offset: 0x0011B1F4
		internal static void CheckSetup()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				HTTPCacheService.SetupCacheFolder();
				HTTPCacheService.LoadLibrary();
			}
			catch
			{
			}
		}

		// Token: 0x06003956 RID: 14678 RVA: 0x0011D02C File Offset: 0x0011B22C
		internal static void SetupCacheFolder()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				if (string.IsNullOrEmpty(HTTPCacheService.CacheFolder) || string.IsNullOrEmpty(HTTPCacheService.LibraryPath))
				{
					HTTPCacheService.CacheFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "HTTPCache");
					if (!Directory.Exists(HTTPCacheService.CacheFolder))
					{
						Directory.CreateDirectory(HTTPCacheService.CacheFolder);
					}
					HTTPCacheService.LibraryPath = Path.Combine(HTTPManager.GetRootCacheFolder(), "Library");
				}
			}
			catch
			{
				HTTPCacheService.isSupported = false;
				HTTPManager.Logger.Warning("HTTPCacheService", "Cache Service Disabled!");
			}
		}

		// Token: 0x06003957 RID: 14679 RVA: 0x0011D0C8 File Offset: 0x0011B2C8
		internal static ulong GetNameIdx()
		{
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			ulong result;
			lock (obj)
			{
				ulong nextNameIDX = HTTPCacheService.NextNameIDX;
				do
				{
					HTTPCacheService.NextNameIDX = (HTTPCacheService.NextNameIDX += 1UL) % ulong.MaxValue;
				}
				while (HTTPCacheService.UsedIndexes.ContainsKey(HTTPCacheService.NextNameIDX));
				result = nextNameIDX;
			}
			return result;
		}

		// Token: 0x06003958 RID: 14680 RVA: 0x0011D134 File Offset: 0x0011B334
		internal static bool HasEntity(Uri uri)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			bool result;
			lock (obj)
			{
				result = HTTPCacheService.Library.ContainsKey(uri);
			}
			return result;
		}

		// Token: 0x06003959 RID: 14681 RVA: 0x0011D184 File Offset: 0x0011B384
		internal static bool DeleteEntity(Uri uri, bool removeFromLibrary = true)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			object obj = HTTPCacheFileLock.Acquire(uri);
			bool result;
			lock (obj)
			{
				Dictionary<Uri, HTTPCacheFileInfo> obj2 = HTTPCacheService.Library;
				lock (obj2)
				{
					HTTPCacheFileInfo httpcacheFileInfo;
					bool flag3 = HTTPCacheService.Library.TryGetValue(uri, out httpcacheFileInfo);
					if (flag3)
					{
						httpcacheFileInfo.Delete();
					}
					if (flag3 && removeFromLibrary)
					{
						HTTPCacheService.Library.Remove(uri);
						HTTPCacheService.UsedIndexes.Remove(httpcacheFileInfo.MappedNameIDX);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x0600395A RID: 14682 RVA: 0x0011D22C File Offset: 0x0011B42C
		internal static bool IsCachedEntityExpiresInTheFuture(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (HTTPCacheService.Library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					return httpcacheFileInfo.WillExpireInTheFuture();
				}
			}
			return false;
		}

		// Token: 0x0600395B RID: 14683 RVA: 0x0011D290 File Offset: 0x0011B490
		internal static void SetHeaders(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (HTTPCacheService.Library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					httpcacheFileInfo.SetUpRevalidationHeaders(request);
				}
			}
		}

		// Token: 0x0600395C RID: 14684 RVA: 0x0011D2EC File Offset: 0x0011B4EC
		internal static HTTPCacheFileInfo GetEntity(Uri uri)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheFileInfo result = null;
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				HTTPCacheService.Library.TryGetValue(uri, out result);
			}
			return result;
		}

		// Token: 0x0600395D RID: 14685 RVA: 0x0011D340 File Offset: 0x0011B540
		internal static HTTPResponse GetFullResponse(HTTPRequest request)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (HTTPCacheService.Library.TryGetValue(request.CurrentUri, out httpcacheFileInfo))
				{
					return httpcacheFileInfo.ReadResponseTo(request);
				}
			}
			return null;
		}

		// Token: 0x0600395E RID: 14686 RVA: 0x0011D3A4 File Offset: 0x0011B5A4
		internal static bool IsCacheble(Uri uri, HTTPMethods method, HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return false;
			}
			if (method != HTTPMethods.Get)
			{
				return false;
			}
			if (response == null)
			{
				return false;
			}
			if (response.StatusCode < 200 || response.StatusCode >= 400)
			{
				return false;
			}
			List<string> headerValues = response.GetHeaderValues("cache-control");
			if (headerValues != null)
			{
				if (headerValues.Exists(delegate(string headerValue)
				{
					string text = headerValue.ToLower();
					return text.Contains("no-store") || text.Contains("no-cache");
				}))
				{
					return false;
				}
			}
			List<string> headerValues2 = response.GetHeaderValues("pragma");
			if (headerValues2 != null)
			{
				if (headerValues2.Exists(delegate(string headerValue)
				{
					string text = headerValue.ToLower();
					return text.Contains("no-store") || text.Contains("no-cache");
				}))
				{
					return false;
				}
			}
			return response.GetHeaderValues("content-range") == null;
		}

		// Token: 0x0600395F RID: 14687 RVA: 0x0011D460 File Offset: 0x0011B660
		internal static HTTPCacheFileInfo Store(Uri uri, HTTPMethods method, HTTPResponse response)
		{
			if (response == null || response.Data == null || response.Data.Length == 0)
			{
				return null;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			HTTPCacheFileInfo httpcacheFileInfo = null;
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				if (!HTTPCacheService.Library.TryGetValue(uri, out httpcacheFileInfo))
				{
					HTTPCacheService.Library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri));
					HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
				}
				try
				{
					httpcacheFileInfo.Store(response);
					if (HTTPManager.Logger.Level == Loglevels.All)
					{
						HTTPManager.Logger.Verbose("HTTPCacheService", string.Format("{0} - Saved to cache", uri.ToString()));
					}
				}
				catch
				{
					HTTPCacheService.DeleteEntity(uri, true);
					throw;
				}
			}
			return httpcacheFileInfo;
		}

		// Token: 0x06003960 RID: 14688 RVA: 0x0011D538 File Offset: 0x0011B738
		internal static Stream PrepareStreamed(Uri uri, HTTPResponse response)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return null;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			Stream saveStream;
			lock (obj)
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (!HTTPCacheService.Library.TryGetValue(uri, out httpcacheFileInfo))
				{
					HTTPCacheService.Library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri));
					HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
				}
				try
				{
					saveStream = httpcacheFileInfo.GetSaveStream(response);
				}
				catch
				{
					HTTPCacheService.DeleteEntity(uri, true);
					throw;
				}
			}
			return saveStream;
		}

		// Token: 0x06003961 RID: 14689 RVA: 0x0011D5D0 File Offset: 0x0011B7D0
		public static void BeginClear()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			if (HTTPCacheService.InClearThread)
			{
				return;
			}
			HTTPCacheService.InClearThread = true;
			HTTPCacheService.SetupCacheFolder();
			ThreadPool.QueueUserWorkItem(delegate(object param)
			{
				HTTPCacheService.ClearImpl(param);
			});
		}

		// Token: 0x06003962 RID: 14690 RVA: 0x0011D620 File Offset: 0x0011B820
		private static void ClearImpl(object param)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				string[] files = Directory.GetFiles(HTTPCacheService.CacheFolder);
				for (int i = 0; i < files.Length; i++)
				{
					try
					{
						File.Delete(files[i]);
					}
					catch
					{
					}
				}
			}
			finally
			{
				HTTPCacheService.UsedIndexes.Clear();
				HTTPCacheService.library.Clear();
				HTTPCacheService.NextNameIDX = 1UL;
				HTTPCacheService.SaveLibrary();
				HTTPCacheService.InClearThread = false;
			}
		}

		// Token: 0x06003963 RID: 14691 RVA: 0x0011D6A4 File Offset: 0x0011B8A4
		public static void BeginMaintainence(HTTPCacheMaintananceParams maintananceParam)
		{
			if (maintananceParam == null)
			{
				throw new ArgumentNullException("maintananceParams == null");
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			if (HTTPCacheService.InMaintainenceThread)
			{
				return;
			}
			HTTPCacheService.InMaintainenceThread = true;
			HTTPCacheService.SetupCacheFolder();
			ThreadPool.QueueUserWorkItem(delegate(object param)
			{
				try
				{
					Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
					lock (obj)
					{
						DateTime t = DateTime.UtcNow - maintananceParam.DeleteOlder;
						List<HTTPCacheFileInfo> list = new List<HTTPCacheFileInfo>();
						foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.Library)
						{
							if (keyValuePair.Value.LastAccess < t && HTTPCacheService.DeleteEntity(keyValuePair.Key, false))
							{
								list.Add(keyValuePair.Value);
							}
						}
						for (int i = 0; i < list.Count; i++)
						{
							HTTPCacheService.Library.Remove(list[i].Uri);
							HTTPCacheService.UsedIndexes.Remove(list[i].MappedNameIDX);
						}
						list.Clear();
						ulong num = HTTPCacheService.GetCacheSize();
						if (num > maintananceParam.MaxCacheSize)
						{
							List<HTTPCacheFileInfo> list2 = new List<HTTPCacheFileInfo>(HTTPCacheService.library.Count);
							foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair2 in HTTPCacheService.library)
							{
								list2.Add(keyValuePair2.Value);
							}
							list2.Sort();
							int num2 = 0;
							while (num >= maintananceParam.MaxCacheSize && num2 < list2.Count)
							{
								try
								{
									HTTPCacheFileInfo httpcacheFileInfo = list2[num2];
									ulong num3 = (ulong)((long)httpcacheFileInfo.BodyLength);
									HTTPCacheService.DeleteEntity(httpcacheFileInfo.Uri, true);
									num -= num3;
								}
								catch
								{
								}
								finally
								{
									num2++;
								}
							}
						}
					}
				}
				finally
				{
					HTTPCacheService.SaveLibrary();
					HTTPCacheService.InMaintainenceThread = false;
				}
			});
		}

		// Token: 0x06003964 RID: 14692 RVA: 0x0011D700 File Offset: 0x0011B900
		public static int GetCacheEntityCount()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return 0;
			}
			HTTPCacheService.CheckSetup();
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			int count;
			lock (obj)
			{
				count = HTTPCacheService.Library.Count;
			}
			return count;
		}

		// Token: 0x06003965 RID: 14693 RVA: 0x0011D754 File Offset: 0x0011B954
		public static ulong GetCacheSize()
		{
			ulong num = 0UL;
			if (!HTTPCacheService.IsSupported)
			{
				return num;
			}
			HTTPCacheService.CheckSetup();
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.Library)
				{
					if (keyValuePair.Value.BodyLength > 0)
					{
						num += (ulong)((long)keyValuePair.Value.BodyLength);
					}
				}
			}
			return num;
		}

		// Token: 0x06003966 RID: 14694 RVA: 0x0011D7F8 File Offset: 0x0011B9F8
		private static void LoadLibrary()
		{
			if (HTTPCacheService.library != null)
			{
				return;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.library = new Dictionary<Uri, HTTPCacheFileInfo>(new UriComparer());
			if (!File.Exists(HTTPCacheService.LibraryPath))
			{
				HTTPCacheService.DeleteUnusedFiles();
				return;
			}
			try
			{
				Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.library;
				int num;
				lock (obj)
				{
					using (FileStream fileStream = new FileStream(HTTPCacheService.LibraryPath, FileMode.Open))
					{
						using (BinaryReader binaryReader = new BinaryReader(fileStream))
						{
							num = binaryReader.ReadInt32();
							if (num > 1)
							{
								HTTPCacheService.NextNameIDX = binaryReader.ReadUInt64();
							}
							int num2 = binaryReader.ReadInt32();
							for (int i = 0; i < num2; i++)
							{
								Uri uri = new Uri(binaryReader.ReadString());
								HTTPCacheFileInfo httpcacheFileInfo = new HTTPCacheFileInfo(uri, binaryReader, num);
								if (httpcacheFileInfo.IsExists())
								{
									HTTPCacheService.library.Add(uri, httpcacheFileInfo);
									if (num > 1)
									{
										HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
									}
								}
							}
						}
					}
				}
				if (num == 1)
				{
					HTTPCacheService.BeginClear();
				}
				else
				{
					HTTPCacheService.DeleteUnusedFiles();
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003967 RID: 14695 RVA: 0x0011D944 File Offset: 0x0011BB44
		internal static void SaveLibrary()
		{
			if (HTTPCacheService.library == null)
			{
				return;
			}
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			try
			{
				Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
				lock (obj)
				{
					using (FileStream fileStream = new FileStream(HTTPCacheService.LibraryPath, FileMode.Create))
					{
						using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
						{
							binaryWriter.Write(2);
							binaryWriter.Write(HTTPCacheService.NextNameIDX);
							binaryWriter.Write(HTTPCacheService.Library.Count);
							foreach (KeyValuePair<Uri, HTTPCacheFileInfo> keyValuePair in HTTPCacheService.Library)
							{
								binaryWriter.Write(keyValuePair.Key.ToString());
								keyValuePair.Value.SaveTo(binaryWriter);
							}
						}
					}
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003968 RID: 14696 RVA: 0x0011DA60 File Offset: 0x0011BC60
		internal static void SetBodyLength(Uri uri, int bodyLength)
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
			lock (obj)
			{
				HTTPCacheFileInfo httpcacheFileInfo;
				if (HTTPCacheService.Library.TryGetValue(uri, out httpcacheFileInfo))
				{
					httpcacheFileInfo.BodyLength = bodyLength;
				}
				else
				{
					HTTPCacheService.Library.Add(uri, httpcacheFileInfo = new HTTPCacheFileInfo(uri, DateTime.UtcNow, bodyLength));
					HTTPCacheService.UsedIndexes.Add(httpcacheFileInfo.MappedNameIDX, httpcacheFileInfo);
				}
			}
		}

		// Token: 0x06003969 RID: 14697 RVA: 0x0011DAE4 File Offset: 0x0011BCE4
		private static void DeleteUnusedFiles()
		{
			if (!HTTPCacheService.IsSupported)
			{
				return;
			}
			HTTPCacheService.CheckSetup();
			string[] files = Directory.GetFiles(HTTPCacheService.CacheFolder);
			for (int i = 0; i < files.Length; i++)
			{
				try
				{
					string fileName = Path.GetFileName(files[i]);
					ulong key = 0UL;
					bool flag = false;
					if (ulong.TryParse(fileName, NumberStyles.AllowHexSpecifier, null, out key))
					{
						Dictionary<Uri, HTTPCacheFileInfo> obj = HTTPCacheService.Library;
						lock (obj)
						{
							flag = !HTTPCacheService.UsedIndexes.ContainsKey(key);
							goto IL_6B;
						}
					}
					flag = true;
					IL_6B:
					if (flag)
					{
						File.Delete(files[i]);
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0400251B RID: 9499
		private const int LibraryVersion = 2;

		// Token: 0x0400251C RID: 9500
		private static bool isSupported;

		// Token: 0x0400251D RID: 9501
		private static bool IsSupportCheckDone;

		// Token: 0x0400251E RID: 9502
		private static Dictionary<Uri, HTTPCacheFileInfo> library;

		// Token: 0x0400251F RID: 9503
		private static Dictionary<ulong, HTTPCacheFileInfo> UsedIndexes = new Dictionary<ulong, HTTPCacheFileInfo>();

		// Token: 0x04002522 RID: 9506
		private static bool InClearThread;

		// Token: 0x04002523 RID: 9507
		private static bool InMaintainenceThread;

		// Token: 0x04002524 RID: 9508
		private static ulong NextNameIDX = 1UL;
	}
}
