using System;
using System.Collections.Generic;
using System.IO;

namespace BestHTTP.Cookies
{
	// Token: 0x02000612 RID: 1554
	public static class CookieJar
	{
		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x06003903 RID: 14595 RVA: 0x0011BB50 File Offset: 0x00119D50
		public static bool IsSavingSupported
		{
			get
			{
				if (CookieJar.IsSupportCheckDone)
				{
					return CookieJar._isSavingSupported;
				}
				try
				{
					File.Exists(HTTPManager.GetRootCacheFolder());
					CookieJar._isSavingSupported = true;
				}
				catch
				{
					CookieJar._isSavingSupported = false;
					HTTPManager.Logger.Warning("CookieJar", "Cookie saving and loading disabled!");
				}
				finally
				{
					CookieJar.IsSupportCheckDone = true;
				}
				return CookieJar._isSavingSupported;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x06003904 RID: 14596 RVA: 0x0011BBC4 File Offset: 0x00119DC4
		// (set) Token: 0x06003905 RID: 14597 RVA: 0x0011BBCB File Offset: 0x00119DCB
		private static string CookieFolder { get; set; }

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06003906 RID: 14598 RVA: 0x0011BBD3 File Offset: 0x00119DD3
		// (set) Token: 0x06003907 RID: 14599 RVA: 0x0011BBDA File Offset: 0x00119DDA
		private static string LibraryPath { get; set; }

		// Token: 0x06003908 RID: 14600 RVA: 0x0011BBE4 File Offset: 0x00119DE4
		internal static void SetupFolder()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			try
			{
				if (string.IsNullOrEmpty(CookieJar.CookieFolder) || string.IsNullOrEmpty(CookieJar.LibraryPath))
				{
					CookieJar.CookieFolder = Path.Combine(HTTPManager.GetRootCacheFolder(), "Cookies");
					CookieJar.LibraryPath = Path.Combine(CookieJar.CookieFolder, "Library");
				}
			}
			catch
			{
			}
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0011BC50 File Offset: 0x00119E50
		internal static void Set(HTTPResponse response)
		{
			if (response == null)
			{
				return;
			}
			object locker = CookieJar.Locker;
			lock (locker)
			{
				try
				{
					CookieJar.Maintain();
					List<Cookie> list = new List<Cookie>();
					List<string> headerValues = response.GetHeaderValues("set-cookie");
					if (headerValues != null)
					{
						foreach (string header in headerValues)
						{
							try
							{
								Cookie cookie = Cookie.Parse(header, response.baseRequest.CurrentUri);
								if (cookie != null)
								{
									int num;
									Cookie cookie2 = CookieJar.Find(cookie, out num);
									if (!string.IsNullOrEmpty(cookie.Value) && cookie.WillExpireInTheFuture())
									{
										if (cookie2 == null)
										{
											CookieJar.Cookies.Add(cookie);
											list.Add(cookie);
										}
										else
										{
											cookie.Date = cookie2.Date;
											CookieJar.Cookies[num] = cookie;
											list.Add(cookie);
										}
									}
									else if (num != -1)
									{
										CookieJar.Cookies.RemoveAt(num);
									}
								}
							}
							catch
							{
							}
						}
						response.Cookies = list;
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0011BDA4 File Offset: 0x00119FA4
		internal static void Maintain()
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				try
				{
					uint num = 0U;
					TimeSpan t = TimeSpan.FromDays(7.0);
					int i = 0;
					while (i < CookieJar.Cookies.Count)
					{
						Cookie cookie = CookieJar.Cookies[i];
						if (!cookie.WillExpireInTheFuture() || cookie.LastAccess + t < DateTime.UtcNow)
						{
							CookieJar.Cookies.RemoveAt(i);
						}
						else
						{
							if (!cookie.IsSession)
							{
								num += cookie.GuessSize();
							}
							i++;
						}
					}
					if (num > HTTPManager.CookieJarSize)
					{
						CookieJar.Cookies.Sort();
						while (num > HTTPManager.CookieJarSize && CookieJar.Cookies.Count > 0)
						{
							Cookie cookie2 = CookieJar.Cookies[0];
							CookieJar.Cookies.RemoveAt(0);
							num -= cookie2.GuessSize();
						}
					}
				}
				catch
				{
				}
			}
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0011BEB4 File Offset: 0x0011A0B4
		internal static void Persist()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			object locker = CookieJar.Locker;
			lock (locker)
			{
				if (CookieJar.Loaded)
				{
					try
					{
						CookieJar.Maintain();
						if (!Directory.Exists(CookieJar.CookieFolder))
						{
							Directory.CreateDirectory(CookieJar.CookieFolder);
						}
						using (FileStream fileStream = new FileStream(CookieJar.LibraryPath, FileMode.Create))
						{
							using (BinaryWriter binaryWriter = new BinaryWriter(fileStream))
							{
								binaryWriter.Write(1);
								int num = 0;
								using (List<Cookie>.Enumerator enumerator = CookieJar.Cookies.GetEnumerator())
								{
									while (enumerator.MoveNext())
									{
										if (!enumerator.Current.IsSession)
										{
											num++;
										}
									}
								}
								binaryWriter.Write(num);
								foreach (Cookie cookie in CookieJar.Cookies)
								{
									if (!cookie.IsSession)
									{
										cookie.SaveTo(binaryWriter);
									}
								}
							}
						}
					}
					catch
					{
					}
				}
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0011C018 File Offset: 0x0011A218
		internal static void Load()
		{
			if (!CookieJar.IsSavingSupported)
			{
				return;
			}
			object locker = CookieJar.Locker;
			lock (locker)
			{
				if (!CookieJar.Loaded)
				{
					CookieJar.SetupFolder();
					try
					{
						CookieJar.Cookies.Clear();
						if (!Directory.Exists(CookieJar.CookieFolder))
						{
							Directory.CreateDirectory(CookieJar.CookieFolder);
						}
						if (File.Exists(CookieJar.LibraryPath))
						{
							using (FileStream fileStream = new FileStream(CookieJar.LibraryPath, FileMode.Open))
							{
								using (BinaryReader binaryReader = new BinaryReader(fileStream))
								{
									binaryReader.ReadInt32();
									int num = binaryReader.ReadInt32();
									for (int i = 0; i < num; i++)
									{
										Cookie cookie = new Cookie();
										cookie.LoadFrom(binaryReader);
										if (cookie.WillExpireInTheFuture())
										{
											CookieJar.Cookies.Add(cookie);
										}
									}
								}
							}
						}
					}
					catch
					{
						CookieJar.Cookies.Clear();
					}
					finally
					{
						CookieJar.Loaded = true;
					}
				}
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x0011C150 File Offset: 0x0011A350
		public static List<Cookie> Get(Uri uri)
		{
			object locker = CookieJar.Locker;
			List<Cookie> result;
			lock (locker)
			{
				CookieJar.Load();
				List<Cookie> list = null;
				for (int i = 0; i < CookieJar.Cookies.Count; i++)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (cookie.WillExpireInTheFuture() && uri.Host.IndexOf(cookie.Domain) != -1 && uri.AbsolutePath.StartsWith(cookie.Path))
					{
						if (list == null)
						{
							list = new List<Cookie>();
						}
						list.Add(cookie);
					}
				}
				result = list;
			}
			return result;
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0011C1FC File Offset: 0x0011A3FC
		public static void Set(Uri uri, Cookie cookie)
		{
			CookieJar.Set(cookie);
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0011C204 File Offset: 0x0011A404
		public static void Set(Cookie cookie)
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				CookieJar.Load();
				int num;
				CookieJar.Find(cookie, out num);
				if (num >= 0)
				{
					CookieJar.Cookies[num] = cookie;
				}
				else
				{
					CookieJar.Cookies.Add(cookie);
				}
			}
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0011C268 File Offset: 0x0011A468
		public static List<Cookie> GetAll()
		{
			object locker = CookieJar.Locker;
			List<Cookie> cookies;
			lock (locker)
			{
				CookieJar.Load();
				cookies = CookieJar.Cookies;
			}
			return cookies;
		}

		// Token: 0x06003911 RID: 14609 RVA: 0x0011C2B0 File Offset: 0x0011A4B0
		public static void Clear()
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				CookieJar.Load();
				CookieJar.Cookies.Clear();
			}
		}

		// Token: 0x06003912 RID: 14610 RVA: 0x0011C2F8 File Offset: 0x0011A4F8
		public static void Clear(TimeSpan olderThan)
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				CookieJar.Load();
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (!cookie.WillExpireInTheFuture() || cookie.Date + olderThan < DateTime.UtcNow)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06003913 RID: 14611 RVA: 0x0011C384 File Offset: 0x0011A584
		public static void Clear(string domain)
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				CookieJar.Load();
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (!cookie.WillExpireInTheFuture() || cookie.Domain.IndexOf(domain) != -1)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06003914 RID: 14612 RVA: 0x0011C408 File Offset: 0x0011A608
		public static void Remove(Uri uri, string name)
		{
			object locker = CookieJar.Locker;
			lock (locker)
			{
				CookieJar.Load();
				int i = 0;
				while (i < CookieJar.Cookies.Count)
				{
					Cookie cookie = CookieJar.Cookies[i];
					if (cookie.Name.Equals(name, StringComparison.OrdinalIgnoreCase) && uri.Host.IndexOf(cookie.Domain) != -1)
					{
						CookieJar.Cookies.RemoveAt(i);
					}
					else
					{
						i++;
					}
				}
			}
		}

		// Token: 0x06003915 RID: 14613 RVA: 0x0011C498 File Offset: 0x0011A698
		private static Cookie Find(Cookie cookie, out int idx)
		{
			for (int i = 0; i < CookieJar.Cookies.Count; i++)
			{
				Cookie cookie2 = CookieJar.Cookies[i];
				if (cookie2.Equals(cookie))
				{
					idx = i;
					return cookie2;
				}
			}
			idx = -1;
			return null;
		}

		// Token: 0x04002502 RID: 9474
		private const int Version = 1;

		// Token: 0x04002503 RID: 9475
		private static List<Cookie> Cookies = new List<Cookie>();

		// Token: 0x04002506 RID: 9478
		private static object Locker = new object();

		// Token: 0x04002507 RID: 9479
		private static bool _isSavingSupported;

		// Token: 0x04002508 RID: 9480
		private static bool IsSupportCheckDone;

		// Token: 0x04002509 RID: 9481
		private static bool Loaded;
	}
}
