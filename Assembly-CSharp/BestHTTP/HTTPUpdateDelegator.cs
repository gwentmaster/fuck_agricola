using System;
using System.Threading;
using BestHTTP.Caching;
using BestHTTP.Cookies;
using UnityEngine;

namespace BestHTTP
{
	// Token: 0x0200057A RID: 1402
	[ExecuteInEditMode]
	public sealed class HTTPUpdateDelegator : MonoBehaviour
	{
		// Token: 0x17000662 RID: 1634
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x001049A3 File Offset: 0x00102BA3
		// (set) Token: 0x0600337C RID: 13180 RVA: 0x001049AA File Offset: 0x00102BAA
		public static HTTPUpdateDelegator Instance { get; private set; }

		// Token: 0x17000663 RID: 1635
		// (get) Token: 0x0600337D RID: 13181 RVA: 0x001049B2 File Offset: 0x00102BB2
		// (set) Token: 0x0600337E RID: 13182 RVA: 0x001049B9 File Offset: 0x00102BB9
		public static bool IsCreated { get; private set; }

		// Token: 0x17000664 RID: 1636
		// (get) Token: 0x0600337F RID: 13183 RVA: 0x001049C1 File Offset: 0x00102BC1
		// (set) Token: 0x06003380 RID: 13184 RVA: 0x001049C8 File Offset: 0x00102BC8
		public static bool IsThreaded { get; set; }

		// Token: 0x17000665 RID: 1637
		// (get) Token: 0x06003381 RID: 13185 RVA: 0x001049D0 File Offset: 0x00102BD0
		// (set) Token: 0x06003382 RID: 13186 RVA: 0x001049D7 File Offset: 0x00102BD7
		public static bool IsThreadRunning { get; private set; }

		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x06003383 RID: 13187 RVA: 0x001049DF File Offset: 0x00102BDF
		// (set) Token: 0x06003384 RID: 13188 RVA: 0x001049E6 File Offset: 0x00102BE6
		public static int ThreadFrequencyInMS { get; set; } = 100;

		// Token: 0x06003386 RID: 13190 RVA: 0x001049F8 File Offset: 0x00102BF8
		public static void CheckInstance()
		{
			try
			{
				if (!HTTPUpdateDelegator.IsCreated)
				{
					GameObject gameObject = GameObject.Find("HTTP Update Delegator");
					if (gameObject != null)
					{
						HTTPUpdateDelegator.Instance = gameObject.GetComponent<HTTPUpdateDelegator>();
					}
					if (HTTPUpdateDelegator.Instance == null)
					{
						HTTPUpdateDelegator.Instance = new GameObject("HTTP Update Delegator")
						{
							hideFlags = HideFlags.DontSave
						}.AddComponent<HTTPUpdateDelegator>();
					}
					HTTPUpdateDelegator.IsCreated = true;
				}
				HTTPManager.Logger.Information("HTTPUpdateDelegator", "Instance Created!");
			}
			catch
			{
				HTTPManager.Logger.Error("HTTPUpdateDelegator", "Please call the BestHTTP.HTTPManager.Setup() from one of Unity's event(eg. awake, start) before you send any request!");
			}
		}

		// Token: 0x06003387 RID: 13191 RVA: 0x00104A9C File Offset: 0x00102C9C
		private void Setup()
		{
			HTTPCacheService.SetupCacheFolder();
			CookieJar.SetupFolder();
			CookieJar.Load();
			if (HTTPUpdateDelegator.IsThreaded)
			{
				ThreadPool.QueueUserWorkItem(new WaitCallback(this.ThreadFunc));
			}
			HTTPUpdateDelegator.IsSetupCalled = true;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "Setup done!");
		}

		// Token: 0x06003388 RID: 13192 RVA: 0x00104AF8 File Offset: 0x00102CF8
		private void ThreadFunc(object obj)
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Started");
			try
			{
				HTTPUpdateDelegator.IsThreadRunning = true;
				while (HTTPUpdateDelegator.IsThreadRunning)
				{
					HTTPManager.OnUpdate();
					Thread.Sleep(HTTPUpdateDelegator.ThreadFrequencyInMS);
				}
			}
			finally
			{
				HTTPManager.Logger.Information("HTTPUpdateDelegator", "Update Thread Ended");
			}
		}

		// Token: 0x06003389 RID: 13193 RVA: 0x00104B60 File Offset: 0x00102D60
		private void Update()
		{
			if (!HTTPUpdateDelegator.IsSetupCalled)
			{
				HTTPUpdateDelegator.IsSetupCalled = true;
				this.Setup();
			}
			if (!HTTPUpdateDelegator.IsThreaded)
			{
				HTTPManager.OnUpdate();
			}
		}

		// Token: 0x0600338A RID: 13194 RVA: 0x00104B81 File Offset: 0x00102D81
		private void OnDisable()
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnDisable Called!");
			this.OnApplicationQuit();
		}

		// Token: 0x0600338B RID: 13195 RVA: 0x00104B9D File Offset: 0x00102D9D
		private void OnApplicationPause(bool isPaused)
		{
			if (HTTPUpdateDelegator.OnApplicationForegroundStateChanged != null)
			{
				HTTPUpdateDelegator.OnApplicationForegroundStateChanged(isPaused);
			}
		}

		// Token: 0x0600338C RID: 13196 RVA: 0x00104BB4 File Offset: 0x00102DB4
		private void OnApplicationQuit()
		{
			HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnApplicationQuit Called!");
			if (HTTPUpdateDelegator.OnBeforeApplicationQuit != null)
			{
				try
				{
					if (!HTTPUpdateDelegator.OnBeforeApplicationQuit())
					{
						HTTPManager.Logger.Information("HTTPUpdateDelegator", "OnBeforeApplicationQuit call returned false, postponing plugin shutdown.");
						return;
					}
				}
				catch (Exception ex)
				{
					HTTPManager.Logger.Exception("HTTPUpdateDelegator", string.Empty, ex);
				}
			}
			HTTPUpdateDelegator.IsThreadRunning = false;
			if (!HTTPUpdateDelegator.IsCreated)
			{
				return;
			}
			HTTPUpdateDelegator.IsCreated = false;
			HTTPManager.OnQuit();
		}

		// Token: 0x040021C5 RID: 8645
		public static Func<bool> OnBeforeApplicationQuit;

		// Token: 0x040021C6 RID: 8646
		public static Action<bool> OnApplicationForegroundStateChanged;

		// Token: 0x040021C7 RID: 8647
		private static bool IsSetupCalled;
	}
}
