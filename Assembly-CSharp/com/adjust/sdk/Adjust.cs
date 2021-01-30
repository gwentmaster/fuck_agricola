using System;
using System.Collections.Generic;
using UnityEngine;

namespace com.adjust.sdk
{
	// Token: 0x0200073F RID: 1855
	public class Adjust : MonoBehaviour
	{
		// Token: 0x060040E6 RID: 16614 RVA: 0x0013AA04 File Offset: 0x00138C04
		private void Awake()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			UnityEngine.Object.DontDestroyOnLoad(base.transform.gameObject);
			if (!this.startManually)
			{
				AdjustConfig adjustConfig = new AdjustConfig(this.appToken, this.environment, this.logLevel == AdjustLogLevel.Suppress);
				adjustConfig.setLogLevel(this.logLevel);
				adjustConfig.setSendInBackground(this.sendInBackground);
				adjustConfig.setEventBufferingEnabled(this.eventBuffering);
				adjustConfig.setLaunchDeferredDeeplink(this.launchDeferredDeeplink);
				Adjust.start(adjustConfig);
			}
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x0013AA80 File Offset: 0x00138C80
		private void OnApplicationPause(bool pauseStatus)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x0013AA94 File Offset: 0x00138C94
		public static void start(AdjustConfig adjustConfig)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			if (adjustConfig == null)
			{
				Debug.Log("Adjust: Missing config to start.");
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0013AAB6 File Offset: 0x00138CB6
		public static void trackEvent(AdjustEvent adjustEvent)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			if (adjustEvent == null)
			{
				Debug.Log("Adjust: Missing event to track.");
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void setEnabled(bool enabled)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x0013AAD8 File Offset: 0x00138CD8
		public static bool isEnabled()
		{
			if (Adjust.IsEditor())
			{
				return false;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return false;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void setOfflineMode(bool enabled)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040ED RID: 16621 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void setDeviceToken(string deviceToken)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040EE RID: 16622 RVA: 0x0013AAEE File Offset: 0x00138CEE
		public static void gdprForgetMe()
		{
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void appWillOpenUrl(string url)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void sendFirstPackages()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void addSessionPartnerParameter(string key, string value)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void addSessionCallbackParameter(string key, string value)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void removeSessionPartnerParameter(string key)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void removeSessionCallbackParameter(string key)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void resetSessionPartnerParameters()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void resetSessionCallbackParameters()
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F7 RID: 16631 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void trackAdRevenue(string source, string payload)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040F8 RID: 16632 RVA: 0x0013AAFA File Offset: 0x00138CFA
		public static string getAdid()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return string.Empty;
		}

		// Token: 0x060040F9 RID: 16633 RVA: 0x0013AB18 File Offset: 0x00138D18
		public static AdjustAttribution getAttribution()
		{
			if (Adjust.IsEditor())
			{
				return null;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return null;
		}

		// Token: 0x060040FA RID: 16634 RVA: 0x0013AAFA File Offset: 0x00138CFA
		public static string getWinAdid()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return string.Empty;
		}

		// Token: 0x060040FB RID: 16635 RVA: 0x0013AAFA File Offset: 0x00138CFA
		public static string getIdfa()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return string.Empty;
		}

		// Token: 0x060040FC RID: 16636 RVA: 0x0013AAFA File Offset: 0x00138CFA
		public static string getSdkVersion()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return string.Empty;
		}

		// Token: 0x060040FD RID: 16637 RVA: 0x0013AA80 File Offset: 0x00138C80
		[Obsolete("This method is intended for testing purposes only. Do not use it.")]
		public static void setReferrer(string referrer)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040FE RID: 16638 RVA: 0x0013AA80 File Offset: 0x00138C80
		public static void getGoogleAdId(Action<string> onDeviceIdsRead)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
		}

		// Token: 0x060040FF RID: 16639 RVA: 0x0013AAFA File Offset: 0x00138CFA
		public static string getAmazonAdId()
		{
			if (Adjust.IsEditor())
			{
				return string.Empty;
			}
			Debug.Log("Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.");
			return string.Empty;
		}

		// Token: 0x06004100 RID: 16640 RVA: 0x0002A062 File Offset: 0x00028262
		private static bool IsEditor()
		{
			return false;
		}

		// Token: 0x06004101 RID: 16641 RVA: 0x0013AB2E File Offset: 0x00138D2E
		public static void SetTestOptions(Dictionary<string, string> testOptions)
		{
			if (Adjust.IsEditor())
			{
				return;
			}
			Debug.Log("Cannot run integration tests. None of the supported platforms selected.");
		}

		// Token: 0x040029D9 RID: 10713
		private const string errorMsgEditor = "Adjust: SDK can not be used in Editor.";

		// Token: 0x040029DA RID: 10714
		private const string errorMsgStart = "Adjust: SDK not started. Start it manually using the 'start' method.";

		// Token: 0x040029DB RID: 10715
		private const string errorMsgPlatform = "Adjust: SDK can only be used in Android, iOS, Windows Phone 8.1, Windows Store or Universal Windows apps.";

		// Token: 0x040029DC RID: 10716
		public bool startManually = true;

		// Token: 0x040029DD RID: 10717
		public bool eventBuffering;

		// Token: 0x040029DE RID: 10718
		public bool sendInBackground;

		// Token: 0x040029DF RID: 10719
		public bool launchDeferredDeeplink = true;

		// Token: 0x040029E0 RID: 10720
		public string appToken = "{Your App Token}";

		// Token: 0x040029E1 RID: 10721
		public AdjustLogLevel logLevel = AdjustLogLevel.Info;

		// Token: 0x040029E2 RID: 10722
		public AdjustEnvironment environment;
	}
}
