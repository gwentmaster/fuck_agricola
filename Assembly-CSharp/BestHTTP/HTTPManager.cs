using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using BestHTTP.Extensions;
using BestHTTP.Logger;
using BestHTTP.Statistics;
using Org.BouncyCastle.Crypto.Tls;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x0200056A RID: 1386
	public static class HTTPManager
	{
		// Token: 0x06003240 RID: 12864 RVA: 0x00100E70 File Offset: 0x000FF070
		static HTTPManager()
		{
			HTTPManager.MaxConnectionIdleTime = TimeSpan.FromSeconds(20.0);
			HTTPManager.IsCookiesEnabled = true;
			HTTPManager.CookieJarSize = 10485760U;
			HTTPManager.EnablePrivateBrowsing = false;
			HTTPManager.ConnectTimeout = TimeSpan.FromSeconds(20.0);
			HTTPManager.RequestTimeout = TimeSpan.FromSeconds(60.0);
			HTTPManager.logger = new DefaultLogger();
			HTTPManager.DefaultCertificateVerifyer = null;
			HTTPManager.UseAlternateSSLDefaultValue = true;
		}

		// Token: 0x17000604 RID: 1540
		// (get) Token: 0x06003241 RID: 12865 RVA: 0x00100F3A File Offset: 0x000FF13A
		// (set) Token: 0x06003242 RID: 12866 RVA: 0x00100F41 File Offset: 0x000FF141
		public static byte MaxConnectionPerServer
		{
			get
			{
				return HTTPManager.maxConnectionPerServer;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("MaxConnectionPerServer must be greater than 0!");
				}
				HTTPManager.maxConnectionPerServer = value;
			}
		} = 4;

		// Token: 0x17000605 RID: 1541
		// (get) Token: 0x06003243 RID: 12867 RVA: 0x00100F58 File Offset: 0x000FF158
		// (set) Token: 0x06003244 RID: 12868 RVA: 0x00100F5F File Offset: 0x000FF15F
		public static bool KeepAliveDefaultValue { get; set; } = true;

		// Token: 0x17000606 RID: 1542
		// (get) Token: 0x06003245 RID: 12869 RVA: 0x00100F67 File Offset: 0x000FF167
		// (set) Token: 0x06003246 RID: 12870 RVA: 0x00100F6E File Offset: 0x000FF16E
		public static bool IsCachingDisabled { get; set; }

		// Token: 0x17000607 RID: 1543
		// (get) Token: 0x06003247 RID: 12871 RVA: 0x00100F76 File Offset: 0x000FF176
		// (set) Token: 0x06003248 RID: 12872 RVA: 0x00100F7D File Offset: 0x000FF17D
		public static TimeSpan MaxConnectionIdleTime { get; set; }

		// Token: 0x17000608 RID: 1544
		// (get) Token: 0x06003249 RID: 12873 RVA: 0x00100F85 File Offset: 0x000FF185
		// (set) Token: 0x0600324A RID: 12874 RVA: 0x00100F8C File Offset: 0x000FF18C
		public static bool IsCookiesEnabled { get; set; }

		// Token: 0x17000609 RID: 1545
		// (get) Token: 0x0600324B RID: 12875 RVA: 0x00100F94 File Offset: 0x000FF194
		// (set) Token: 0x0600324C RID: 12876 RVA: 0x00100F9B File Offset: 0x000FF19B
		public static uint CookieJarSize { get; set; }

		// Token: 0x1700060A RID: 1546
		// (get) Token: 0x0600324D RID: 12877 RVA: 0x00100FA3 File Offset: 0x000FF1A3
		// (set) Token: 0x0600324E RID: 12878 RVA: 0x00100FAA File Offset: 0x000FF1AA
		public static bool EnablePrivateBrowsing { get; set; }

		// Token: 0x1700060B RID: 1547
		// (get) Token: 0x0600324F RID: 12879 RVA: 0x00100FB2 File Offset: 0x000FF1B2
		// (set) Token: 0x06003250 RID: 12880 RVA: 0x00100FB9 File Offset: 0x000FF1B9
		public static TimeSpan ConnectTimeout { get; set; }

		// Token: 0x1700060C RID: 1548
		// (get) Token: 0x06003251 RID: 12881 RVA: 0x00100FC1 File Offset: 0x000FF1C1
		// (set) Token: 0x06003252 RID: 12882 RVA: 0x00100FC8 File Offset: 0x000FF1C8
		public static TimeSpan RequestTimeout { get; set; }

		// Token: 0x1700060D RID: 1549
		// (get) Token: 0x06003253 RID: 12883 RVA: 0x00100FD0 File Offset: 0x000FF1D0
		// (set) Token: 0x06003254 RID: 12884 RVA: 0x00100FD7 File Offset: 0x000FF1D7
		public static Func<string> RootCacheFolderProvider { get; set; }

		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06003255 RID: 12885 RVA: 0x00100FDF File Offset: 0x000FF1DF
		// (set) Token: 0x06003256 RID: 12886 RVA: 0x00100FE6 File Offset: 0x000FF1E6
		public static HTTPProxy Proxy { get; set; }

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06003257 RID: 12887 RVA: 0x00100FEE File Offset: 0x000FF1EE
		public static HeartbeatManager Heartbeats
		{
			get
			{
				if (HTTPManager.heartbeats == null)
				{
					HTTPManager.heartbeats = new HeartbeatManager();
				}
				return HTTPManager.heartbeats;
			}
		}

		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06003258 RID: 12888 RVA: 0x00101006 File Offset: 0x000FF206
		// (set) Token: 0x06003259 RID: 12889 RVA: 0x00101029 File Offset: 0x000FF229
		public static BestHTTP.Logger.ILogger Logger
		{
			get
			{
				if (HTTPManager.logger == null)
				{
					HTTPManager.logger = new DefaultLogger();
					HTTPManager.logger.Level = Loglevels.None;
				}
				return HTTPManager.logger;
			}
			set
			{
				HTTPManager.logger = value;
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x0600325A RID: 12890 RVA: 0x00101031 File Offset: 0x000FF231
		// (set) Token: 0x0600325B RID: 12891 RVA: 0x00101038 File Offset: 0x000FF238
		public static ICertificateVerifyer DefaultCertificateVerifyer { get; set; }

		// Token: 0x17000612 RID: 1554
		// (get) Token: 0x0600325C RID: 12892 RVA: 0x00101040 File Offset: 0x000FF240
		// (set) Token: 0x0600325D RID: 12893 RVA: 0x00101047 File Offset: 0x000FF247
		public static IClientCredentialsProvider DefaultClientCredentialsProvider { get; set; }

		// Token: 0x17000613 RID: 1555
		// (get) Token: 0x0600325E RID: 12894 RVA: 0x0010104F File Offset: 0x000FF24F
		// (set) Token: 0x0600325F RID: 12895 RVA: 0x00101056 File Offset: 0x000FF256
		public static bool UseAlternateSSLDefaultValue { get; set; }

		// Token: 0x17000614 RID: 1556
		// (get) Token: 0x06003260 RID: 12896 RVA: 0x0010105E File Offset: 0x000FF25E
		// (set) Token: 0x06003261 RID: 12897 RVA: 0x00101065 File Offset: 0x000FF265
		public static Func<HTTPRequest, X509Certificate, X509Chain, bool> DefaultCertificationValidator { get; set; }

		// Token: 0x17000615 RID: 1557
		// (get) Token: 0x06003262 RID: 12898 RVA: 0x0010106D File Offset: 0x000FF26D
		// (set) Token: 0x06003263 RID: 12899 RVA: 0x00101074 File Offset: 0x000FF274
		internal static int MaxPathLength { get; set; } = 255;

		// Token: 0x06003264 RID: 12900 RVA: 0x0010107C File Offset: 0x000FF27C
		public static void Setup()
		{
			HTTPUpdateDelegator.CheckInstance();
			HTTPCacheService.CheckSetup();
			CookieJar.SetupFolder();
		}

		// Token: 0x06003265 RID: 12901 RVA: 0x0010108D File Offset: 0x000FF28D
		public static HTTPRequest SendRequest(string url, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), HTTPMethods.Get, callback));
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x001010A1 File Offset: 0x000FF2A1
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, callback));
		}

		// Token: 0x06003267 RID: 12903 RVA: 0x001010B5 File Offset: 0x000FF2B5
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, callback));
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x001010CA File Offset: 0x000FF2CA
		public static HTTPRequest SendRequest(string url, HTTPMethods methodType, bool isKeepAlive, bool disableCache, OnRequestFinishedDelegate callback)
		{
			return HTTPManager.SendRequest(new HTTPRequest(new Uri(url), methodType, isKeepAlive, disableCache, callback));
		}

		// Token: 0x06003269 RID: 12905 RVA: 0x001010E4 File Offset: 0x000FF2E4
		public static HTTPRequest SendRequest(HTTPRequest request)
		{
			object locker = HTTPManager.Locker;
			lock (locker)
			{
				HTTPManager.Setup();
				if (HTTPManager.IsCallingCallbacks)
				{
					request.State = HTTPRequestStates.Queued;
					HTTPManager.RequestQueue.Add(request);
				}
				else
				{
					HTTPManager.SendRequestImpl(request);
				}
			}
			return request;
		}

		// Token: 0x0600326A RID: 12906 RVA: 0x00101148 File Offset: 0x000FF348
		public static GeneralStatistics GetGeneralStatistics(StatisticsQueryFlags queryFlags)
		{
			GeneralStatistics result = default(GeneralStatistics);
			result.QueryFlags = queryFlags;
			if ((queryFlags & StatisticsQueryFlags.Connections) != (StatisticsQueryFlags)0)
			{
				int num = 0;
				foreach (KeyValuePair<string, List<ConnectionBase>> keyValuePair in HTTPManager.Connections)
				{
					if (keyValuePair.Value != null)
					{
						num += keyValuePair.Value.Count;
					}
				}
				result.Connections = num;
				result.ActiveConnections = HTTPManager.ActiveConnections.Count;
				result.FreeConnections = HTTPManager.FreeConnections.Count;
				result.RecycledConnections = HTTPManager.RecycledConnections.Count;
				result.RequestsInQueue = HTTPManager.RequestQueue.Count;
			}
			if ((queryFlags & StatisticsQueryFlags.Cache) != (StatisticsQueryFlags)0)
			{
				result.CacheEntityCount = HTTPCacheService.GetCacheEntityCount();
				result.CacheSize = HTTPCacheService.GetCacheSize();
			}
			if ((queryFlags & StatisticsQueryFlags.Cookies) != (StatisticsQueryFlags)0)
			{
				List<Cookie> all = CookieJar.GetAll();
				result.CookieCount = all.Count;
				uint num2 = 0U;
				for (int i = 0; i < all.Count; i++)
				{
					num2 += all[i].GuessSize();
				}
				result.CookieJarSize = num2;
			}
			return result;
		}

		// Token: 0x0600326B RID: 12907 RVA: 0x00101280 File Offset: 0x000FF480
		private static void SendRequestImpl(HTTPRequest request)
		{
			ConnectionBase conn = HTTPManager.FindOrCreateFreeConnection(request);
			if (conn != null)
			{
				if (HTTPManager.ActiveConnections.Find((ConnectionBase c) => c == conn) == null)
				{
					HTTPManager.ActiveConnections.Add(conn);
				}
				HTTPManager.FreeConnections.Remove(conn);
				request.State = HTTPRequestStates.Processing;
				request.Prepare();
				conn.Process(request);
				return;
			}
			request.State = HTTPRequestStates.Queued;
			HTTPManager.RequestQueue.Add(request);
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x0010130C File Offset: 0x000FF50C
		private static string GetKeyForRequest(HTTPRequest request)
		{
			if (request.CurrentUri.IsFile)
			{
				return request.CurrentUri.ToString();
			}
			return ((request.Proxy != null) ? new UriBuilder(request.Proxy.Address.Scheme, request.Proxy.Address.Host, request.Proxy.Address.Port).Uri.ToString() : string.Empty) + new UriBuilder(request.CurrentUri.Scheme, request.CurrentUri.Host, request.CurrentUri.Port).Uri.ToString();
		}

		// Token: 0x0600326D RID: 12909 RVA: 0x001013B5 File Offset: 0x000FF5B5
		private static ConnectionBase CreateConnection(HTTPRequest request, string serverUrl)
		{
			if (request.CurrentUri.IsFile && Application.platform != RuntimePlatform.WebGLPlayer)
			{
				return new FileConnection(serverUrl);
			}
			return new HTTPConnection(serverUrl);
		}

		// Token: 0x0600326E RID: 12910 RVA: 0x001013DC File Offset: 0x000FF5DC
		private static ConnectionBase FindOrCreateFreeConnection(HTTPRequest request)
		{
			ConnectionBase connectionBase = null;
			string keyForRequest = HTTPManager.GetKeyForRequest(request);
			List<ConnectionBase> list;
			if (HTTPManager.Connections.TryGetValue(keyForRequest, out list))
			{
				int num = 0;
				for (int i = 0; i < list.Count; i++)
				{
					if (list[i].IsActive)
					{
						num++;
					}
				}
				if (num <= (int)HTTPManager.MaxConnectionPerServer)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (connectionBase != null)
						{
							break;
						}
						ConnectionBase connectionBase2 = list[j];
						if (connectionBase2 != null && connectionBase2.IsFree && (!connectionBase2.HasProxy || connectionBase2.LastProcessedUri == null || connectionBase2.LastProcessedUri.Host.Equals(request.CurrentUri.Host, StringComparison.OrdinalIgnoreCase)))
						{
							connectionBase = connectionBase2;
						}
					}
				}
			}
			else
			{
				HTTPManager.Connections.Add(keyForRequest, list = new List<ConnectionBase>((int)HTTPManager.MaxConnectionPerServer));
			}
			if (connectionBase == null)
			{
				if (list.Count >= (int)HTTPManager.MaxConnectionPerServer)
				{
					return null;
				}
				list.Add(connectionBase = HTTPManager.CreateConnection(request, keyForRequest));
			}
			return connectionBase;
		}

		// Token: 0x0600326F RID: 12911 RVA: 0x001014E0 File Offset: 0x000FF6E0
		private static bool CanProcessFromQueue()
		{
			for (int i = 0; i < HTTPManager.RequestQueue.Count; i++)
			{
				if (HTTPManager.FindOrCreateFreeConnection(HTTPManager.RequestQueue[i]) != null)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x00101517 File Offset: 0x000FF717
		private static void RecycleConnection(ConnectionBase conn)
		{
			conn.Recycle(new HTTPConnectionRecycledDelegate(HTTPManager.OnConnectionRecylced));
		}

		// Token: 0x06003271 RID: 12913 RVA: 0x0010152C File Offset: 0x000FF72C
		private static void OnConnectionRecylced(ConnectionBase conn)
		{
			List<ConnectionBase> recycledConnections = HTTPManager.RecycledConnections;
			lock (recycledConnections)
			{
				HTTPManager.RecycledConnections.Add(conn);
			}
		}

		// Token: 0x06003272 RID: 12914 RVA: 0x00101570 File Offset: 0x000FF770
		internal static ConnectionBase GetConnectionWith(HTTPRequest request)
		{
			object locker = HTTPManager.Locker;
			ConnectionBase result;
			lock (locker)
			{
				for (int i = 0; i < HTTPManager.ActiveConnections.Count; i++)
				{
					ConnectionBase connectionBase = HTTPManager.ActiveConnections[i];
					if (connectionBase.CurrentRequest == request)
					{
						return connectionBase;
					}
				}
				result = null;
			}
			return result;
		}

		// Token: 0x06003273 RID: 12915 RVA: 0x001015E0 File Offset: 0x000FF7E0
		internal static bool RemoveFromQueue(HTTPRequest request)
		{
			return HTTPManager.RequestQueue.Remove(request);
		}

		// Token: 0x06003274 RID: 12916 RVA: 0x001015F0 File Offset: 0x000FF7F0
		internal static string GetRootCacheFolder()
		{
			try
			{
				if (HTTPManager.RootCacheFolderProvider != null)
				{
					return HTTPManager.RootCacheFolderProvider();
				}
			}
			catch (Exception ex)
			{
				HTTPManager.Logger.Exception("HTTPManager", "GetRootCacheFolder", ex);
			}
			return Application.persistentDataPath;
		}

		// Token: 0x06003275 RID: 12917 RVA: 0x00101644 File Offset: 0x000FF844
		public static void OnUpdate()
		{
			if (Monitor.TryEnter(HTTPManager.Locker))
			{
				try
				{
					HTTPManager.IsCallingCallbacks = true;
					try
					{
						int i = 0;
						while (i < HTTPManager.ActiveConnections.Count)
						{
							ConnectionBase connectionBase = HTTPManager.ActiveConnections[i];
							switch (connectionBase.State)
							{
							case HTTPConnectionStates.Processing:
								connectionBase.HandleProgressCallback();
								if (connectionBase.CurrentRequest.UseStreaming && connectionBase.CurrentRequest.Response != null && connectionBase.CurrentRequest.Response.HasStreamedFragments())
								{
									connectionBase.HandleCallback();
								}
								try
								{
									if (((!connectionBase.CurrentRequest.UseStreaming && connectionBase.CurrentRequest.UploadStream == null) || connectionBase.CurrentRequest.EnableTimoutForStreaming) && DateTime.UtcNow - connectionBase.StartTime > connectionBase.CurrentRequest.Timeout)
									{
										connectionBase.Abort(HTTPConnectionStates.TimedOut);
									}
									break;
								}
								catch (OverflowException)
								{
									HTTPManager.Logger.Warning("HTTPManager", "TimeSpan overflow");
									break;
								}
								goto IL_108;
							case HTTPConnectionStates.Redirected:
								goto IL_185;
							case HTTPConnectionStates.Upgraded:
								connectionBase.HandleCallback();
								break;
							case HTTPConnectionStates.WaitForProtocolShutdown:
							{
								IProtocol protocol = connectionBase.CurrentRequest.Response as IProtocol;
								if (protocol != null)
								{
									protocol.HandleEvents();
								}
								if (protocol == null || protocol.IsClosed)
								{
									connectionBase.HandleCallback();
									connectionBase.Dispose();
									HTTPManager.RecycleConnection(connectionBase);
								}
								break;
							}
							case HTTPConnectionStates.WaitForRecycle:
								connectionBase.CurrentRequest.FinishStreaming();
								connectionBase.HandleCallback();
								HTTPManager.RecycleConnection(connectionBase);
								break;
							case HTTPConnectionStates.Free:
								HTTPManager.RecycleConnection(connectionBase);
								break;
							case HTTPConnectionStates.AbortRequested:
							{
								IProtocol protocol = connectionBase.CurrentRequest.Response as IProtocol;
								if (protocol != null)
								{
									protocol.HandleEvents();
									if (protocol.IsClosed)
									{
										connectionBase.HandleCallback();
										connectionBase.Dispose();
										HTTPManager.RecycleConnection(connectionBase);
									}
								}
								break;
							}
							case HTTPConnectionStates.TimedOut:
								goto IL_108;
							case HTTPConnectionStates.Closed:
								connectionBase.CurrentRequest.FinishStreaming();
								connectionBase.HandleCallback();
								HTTPManager.RecycleConnection(connectionBase);
								break;
							}
							IL_251:
							i++;
							continue;
							IL_108:
							try
							{
								if (DateTime.UtcNow - connectionBase.TimedOutStart > TimeSpan.FromMilliseconds(500.0))
								{
									HTTPManager.Logger.Information("HTTPManager", "Hard aborting connection because of a long waiting TimedOut state");
									connectionBase.CurrentRequest.Response = null;
									connectionBase.CurrentRequest.State = HTTPRequestStates.TimedOut;
									connectionBase.HandleCallback();
									HTTPManager.RecycleConnection(connectionBase);
								}
								goto IL_251;
							}
							catch (OverflowException)
							{
								HTTPManager.Logger.Warning("HTTPManager", "TimeSpan overflow");
								goto IL_251;
							}
							IL_185:
							HTTPManager.SendRequest(connectionBase.CurrentRequest);
							HTTPManager.RecycleConnection(connectionBase);
							goto IL_251;
						}
					}
					finally
					{
						HTTPManager.IsCallingCallbacks = false;
					}
					if (Monitor.TryEnter(HTTPManager.RecycledConnections))
					{
						try
						{
							if (HTTPManager.RecycledConnections.Count > 0)
							{
								for (int j = 0; j < HTTPManager.RecycledConnections.Count; j++)
								{
									ConnectionBase connectionBase2 = HTTPManager.RecycledConnections[j];
									if (connectionBase2.IsFree)
									{
										HTTPManager.ActiveConnections.Remove(connectionBase2);
										HTTPManager.FreeConnections.Add(connectionBase2);
									}
								}
								HTTPManager.RecycledConnections.Clear();
							}
						}
						finally
						{
							Monitor.Exit(HTTPManager.RecycledConnections);
						}
					}
					if (HTTPManager.FreeConnections.Count > 0)
					{
						for (int k = 0; k < HTTPManager.FreeConnections.Count; k++)
						{
							ConnectionBase connectionBase3 = HTTPManager.FreeConnections[k];
							if (connectionBase3.IsRemovable)
							{
								List<ConnectionBase> list = null;
								if (HTTPManager.Connections.TryGetValue(connectionBase3.ServerAddress, out list))
								{
									list.Remove(connectionBase3);
								}
								connectionBase3.Dispose();
								HTTPManager.FreeConnections.RemoveAt(k);
								k--;
							}
						}
					}
					if (HTTPManager.CanProcessFromQueue())
					{
						if (HTTPManager.RequestQueue.Find((HTTPRequest req) => req.Priority != 0) != null)
						{
							HTTPManager.RequestQueue.Sort((HTTPRequest req1, HTTPRequest req2) => req1.Priority - req2.Priority);
						}
						HTTPRequest[] array = HTTPManager.RequestQueue.ToArray();
						HTTPManager.RequestQueue.Clear();
						for (int l = 0; l < array.Length; l++)
						{
							HTTPManager.SendRequest(array[l]);
						}
					}
				}
				finally
				{
					Monitor.Exit(HTTPManager.Locker);
				}
			}
			if (HTTPManager.heartbeats != null)
			{
				HTTPManager.heartbeats.Update();
			}
		}

		// Token: 0x06003276 RID: 12918 RVA: 0x00101ADC File Offset: 0x000FFCDC
		public static void OnQuit()
		{
			object locker = HTTPManager.Locker;
			lock (locker)
			{
				HTTPCacheService.SaveLibrary();
				HTTPRequest[] array = HTTPManager.RequestQueue.ToArray();
				HTTPManager.RequestQueue.Clear();
				foreach (HTTPRequest httprequest in array)
				{
					try
					{
						httprequest.Abort();
					}
					catch
					{
					}
				}
				foreach (KeyValuePair<string, List<ConnectionBase>> keyValuePair in HTTPManager.Connections)
				{
					foreach (ConnectionBase connectionBase in keyValuePair.Value)
					{
						try
						{
							if (connectionBase.CurrentRequest != null)
							{
								connectionBase.CurrentRequest.State = HTTPRequestStates.Aborted;
							}
							connectionBase.Abort(HTTPConnectionStates.Closed);
							connectionBase.Dispose();
						}
						catch
						{
						}
					}
					keyValuePair.Value.Clear();
				}
				HTTPManager.Connections.Clear();
				HTTPManager.OnUpdate();
			}
		}

		// Token: 0x04002130 RID: 8496
		private static byte maxConnectionPerServer;

		// Token: 0x0400213B RID: 8507
		private static HeartbeatManager heartbeats;

		// Token: 0x0400213C RID: 8508
		private static BestHTTP.Logger.ILogger logger;

		// Token: 0x04002141 RID: 8513
		public static bool TryToMinimizeTCPLatency = false;

		// Token: 0x04002143 RID: 8515
		private static Dictionary<string, List<ConnectionBase>> Connections = new Dictionary<string, List<ConnectionBase>>();

		// Token: 0x04002144 RID: 8516
		private static List<ConnectionBase> ActiveConnections = new List<ConnectionBase>();

		// Token: 0x04002145 RID: 8517
		private static List<ConnectionBase> FreeConnections = new List<ConnectionBase>();

		// Token: 0x04002146 RID: 8518
		private static List<ConnectionBase> RecycledConnections = new List<ConnectionBase>();

		// Token: 0x04002147 RID: 8519
		private static List<HTTPRequest> RequestQueue = new List<HTTPRequest>();

		// Token: 0x04002148 RID: 8520
		private static bool IsCallingCallbacks;

		// Token: 0x04002149 RID: 8521
		internal static object Locker = new object();
	}
}
