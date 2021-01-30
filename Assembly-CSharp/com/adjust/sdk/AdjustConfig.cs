using System;

namespace com.adjust.sdk
{
	// Token: 0x02000741 RID: 1857
	public class AdjustConfig
	{
		// Token: 0x06004116 RID: 16662 RVA: 0x0013AD3F File Offset: 0x00138F3F
		public AdjustConfig(string appToken, AdjustEnvironment environment)
		{
			this.sceneName = "";
			this.processName = "";
			this.appToken = appToken;
			this.environment = environment;
		}

		// Token: 0x06004117 RID: 16663 RVA: 0x0013AD6B File Offset: 0x00138F6B
		public AdjustConfig(string appToken, AdjustEnvironment environment, bool allowSuppressLogLevel)
		{
			this.sceneName = "";
			this.processName = "";
			this.appToken = appToken;
			this.environment = environment;
			this.allowSuppressLogLevel = new bool?(allowSuppressLogLevel);
		}

		// Token: 0x06004118 RID: 16664 RVA: 0x0013ADA3 File Offset: 0x00138FA3
		public void setLogLevel(AdjustLogLevel logLevel)
		{
			this.logLevel = new AdjustLogLevel?(logLevel);
		}

		// Token: 0x06004119 RID: 16665 RVA: 0x0013ADB1 File Offset: 0x00138FB1
		public void setDefaultTracker(string defaultTracker)
		{
			this.defaultTracker = defaultTracker;
		}

		// Token: 0x0600411A RID: 16666 RVA: 0x0013ADBA File Offset: 0x00138FBA
		public void setLaunchDeferredDeeplink(bool launchDeferredDeeplink)
		{
			this.launchDeferredDeeplink = launchDeferredDeeplink;
		}

		// Token: 0x0600411B RID: 16667 RVA: 0x0013ADC3 File Offset: 0x00138FC3
		public void setSendInBackground(bool sendInBackground)
		{
			this.sendInBackground = new bool?(sendInBackground);
		}

		// Token: 0x0600411C RID: 16668 RVA: 0x0013ADD1 File Offset: 0x00138FD1
		public void setEventBufferingEnabled(bool eventBufferingEnabled)
		{
			this.eventBufferingEnabled = new bool?(eventBufferingEnabled);
		}

		// Token: 0x0600411D RID: 16669 RVA: 0x0013ADDF File Offset: 0x00138FDF
		public void setDelayStart(double delayStart)
		{
			this.delayStart = new double?(delayStart);
		}

		// Token: 0x0600411E RID: 16670 RVA: 0x0013ADED File Offset: 0x00138FED
		public void setUserAgent(string userAgent)
		{
			this.userAgent = userAgent;
		}

		// Token: 0x0600411F RID: 16671 RVA: 0x0013ADF6 File Offset: 0x00138FF6
		public void setIsDeviceKnown(bool isDeviceKnown)
		{
			this.isDeviceKnown = new bool?(isDeviceKnown);
		}

		// Token: 0x06004120 RID: 16672 RVA: 0x0013AE04 File Offset: 0x00139004
		public void setDeferredDeeplinkDelegate(Action<string> deferredDeeplinkDelegate, string sceneName = "Adjust")
		{
			this.deferredDeeplinkDelegate = deferredDeeplinkDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x06004121 RID: 16673 RVA: 0x0013AE14 File Offset: 0x00139014
		public Action<string> getDeferredDeeplinkDelegate()
		{
			return this.deferredDeeplinkDelegate;
		}

		// Token: 0x06004122 RID: 16674 RVA: 0x0013AE1C File Offset: 0x0013901C
		public void setAttributionChangedDelegate(Action<AdjustAttribution> attributionChangedDelegate, string sceneName = "Adjust")
		{
			this.attributionChangedDelegate = attributionChangedDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x06004123 RID: 16675 RVA: 0x0013AE2C File Offset: 0x0013902C
		public Action<AdjustAttribution> getAttributionChangedDelegate()
		{
			return this.attributionChangedDelegate;
		}

		// Token: 0x06004124 RID: 16676 RVA: 0x0013AE34 File Offset: 0x00139034
		public void setEventSuccessDelegate(Action<AdjustEventSuccess> eventSuccessDelegate, string sceneName = "Adjust")
		{
			this.eventSuccessDelegate = eventSuccessDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x06004125 RID: 16677 RVA: 0x0013AE44 File Offset: 0x00139044
		public Action<AdjustEventSuccess> getEventSuccessDelegate()
		{
			return this.eventSuccessDelegate;
		}

		// Token: 0x06004126 RID: 16678 RVA: 0x0013AE4C File Offset: 0x0013904C
		public void setEventFailureDelegate(Action<AdjustEventFailure> eventFailureDelegate, string sceneName = "Adjust")
		{
			this.eventFailureDelegate = eventFailureDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x06004127 RID: 16679 RVA: 0x0013AE5C File Offset: 0x0013905C
		public Action<AdjustEventFailure> getEventFailureDelegate()
		{
			return this.eventFailureDelegate;
		}

		// Token: 0x06004128 RID: 16680 RVA: 0x0013AE64 File Offset: 0x00139064
		public void setSessionSuccessDelegate(Action<AdjustSessionSuccess> sessionSuccessDelegate, string sceneName = "Adjust")
		{
			this.sessionSuccessDelegate = sessionSuccessDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x06004129 RID: 16681 RVA: 0x0013AE74 File Offset: 0x00139074
		public Action<AdjustSessionSuccess> getSessionSuccessDelegate()
		{
			return this.sessionSuccessDelegate;
		}

		// Token: 0x0600412A RID: 16682 RVA: 0x0013AE7C File Offset: 0x0013907C
		public void setSessionFailureDelegate(Action<AdjustSessionFailure> sessionFailureDelegate, string sceneName = "Adjust")
		{
			this.sessionFailureDelegate = sessionFailureDelegate;
			this.sceneName = sceneName;
		}

		// Token: 0x0600412B RID: 16683 RVA: 0x0013AE8C File Offset: 0x0013908C
		public Action<AdjustSessionFailure> getSessionFailureDelegate()
		{
			return this.sessionFailureDelegate;
		}

		// Token: 0x0600412C RID: 16684 RVA: 0x0013AE94 File Offset: 0x00139094
		public void setAppSecret(long secretId, long info1, long info2, long info3, long info4)
		{
			this.secretId = new long?(secretId);
			this.info1 = new long?(info1);
			this.info2 = new long?(info2);
			this.info3 = new long?(info3);
			this.info4 = new long?(info4);
		}

		// Token: 0x0600412D RID: 16685 RVA: 0x0013AED4 File Offset: 0x001390D4
		public void setProcessName(string processName)
		{
			this.processName = processName;
		}

		// Token: 0x0600412E RID: 16686 RVA: 0x00003022 File Offset: 0x00001222
		[Obsolete("This is an obsolete method.")]
		public void setReadMobileEquipmentIdentity(bool readMobileEquipmentIdentity)
		{
		}

		// Token: 0x0600412F RID: 16687 RVA: 0x0013AEDD File Offset: 0x001390DD
		public void setLogDelegate(Action<string> logDelegate)
		{
			this.logDelegate = logDelegate;
		}

		// Token: 0x040029EB RID: 10731
		public const string AdjustAdRevenueSourceMopub = "mopub";

		// Token: 0x040029EC RID: 10732
		internal string appToken;

		// Token: 0x040029ED RID: 10733
		internal string sceneName;

		// Token: 0x040029EE RID: 10734
		internal string userAgent;

		// Token: 0x040029EF RID: 10735
		internal string defaultTracker;

		// Token: 0x040029F0 RID: 10736
		internal long? info1;

		// Token: 0x040029F1 RID: 10737
		internal long? info2;

		// Token: 0x040029F2 RID: 10738
		internal long? info3;

		// Token: 0x040029F3 RID: 10739
		internal long? info4;

		// Token: 0x040029F4 RID: 10740
		internal long? secretId;

		// Token: 0x040029F5 RID: 10741
		internal double? delayStart;

		// Token: 0x040029F6 RID: 10742
		internal bool? isDeviceKnown;

		// Token: 0x040029F7 RID: 10743
		internal bool? sendInBackground;

		// Token: 0x040029F8 RID: 10744
		internal bool? eventBufferingEnabled;

		// Token: 0x040029F9 RID: 10745
		internal bool? allowSuppressLogLevel;

		// Token: 0x040029FA RID: 10746
		internal bool launchDeferredDeeplink;

		// Token: 0x040029FB RID: 10747
		internal AdjustLogLevel? logLevel;

		// Token: 0x040029FC RID: 10748
		internal AdjustEnvironment environment;

		// Token: 0x040029FD RID: 10749
		internal Action<string> deferredDeeplinkDelegate;

		// Token: 0x040029FE RID: 10750
		internal Action<AdjustEventSuccess> eventSuccessDelegate;

		// Token: 0x040029FF RID: 10751
		internal Action<AdjustEventFailure> eventFailureDelegate;

		// Token: 0x04002A00 RID: 10752
		internal Action<AdjustSessionSuccess> sessionSuccessDelegate;

		// Token: 0x04002A01 RID: 10753
		internal Action<AdjustSessionFailure> sessionFailureDelegate;

		// Token: 0x04002A02 RID: 10754
		internal Action<AdjustAttribution> attributionChangedDelegate;

		// Token: 0x04002A03 RID: 10755
		internal bool? readImei;

		// Token: 0x04002A04 RID: 10756
		internal string processName;

		// Token: 0x04002A05 RID: 10757
		internal Action<string> logDelegate;
	}
}
