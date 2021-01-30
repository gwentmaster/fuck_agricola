using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UTNotifications
{
	// Token: 0x0200014E RID: 334
	public class Settings : ScriptableObject
	{
		// Token: 0x1700003E RID: 62
		// (get) Token: 0x06000CBF RID: 3263 RVA: 0x00055A04 File Offset: 0x00053C04
		public static Settings Instance
		{
			get
			{
				if (Settings.m_instance == null)
				{
					Settings.m_instance = (Resources.Load("UTNotificationsSettings") as Settings);
					if (Settings.m_instance == null)
					{
						Settings.m_instance = ScriptableObject.CreateInstance<Settings>();
					}
				}
				return Settings.m_instance;
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x00055A44 File Offset: 0x00053C44
		public List<Settings.NotificationProfile> NotificationProfiles
		{
			get
			{
				if (this.m_notificationProfiles.Count == 0 || this.m_notificationProfiles[0].profileName != "default")
				{
					this.m_notificationProfiles.Insert(0, new Settings.NotificationProfile
					{
						profileName = "default"
					});
				}
				return this.m_notificationProfiles;
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x06000CC1 RID: 3265 RVA: 0x00055AA2 File Offset: 0x00053CA2
		// (set) Token: 0x06000CC2 RID: 3266 RVA: 0x00055AAA File Offset: 0x00053CAA
		public string PushPayloadTitleFieldName
		{
			get
			{
				return this.m_pushPayloadTitleFieldName;
			}
			set
			{
				if (this.m_pushPayloadTitleFieldName != value)
				{
					this.m_pushPayloadTitleFieldName = value;
				}
			}
		}

		// Token: 0x17000041 RID: 65
		// (get) Token: 0x06000CC3 RID: 3267 RVA: 0x00055AC1 File Offset: 0x00053CC1
		// (set) Token: 0x06000CC4 RID: 3268 RVA: 0x00055AC9 File Offset: 0x00053CC9
		public string PushPayloadTextFieldName
		{
			get
			{
				return this.m_pushPayloadTextFieldName;
			}
			set
			{
				if (this.m_pushPayloadTextFieldName != value)
				{
					this.m_pushPayloadTextFieldName = value;
				}
			}
		}

		// Token: 0x17000042 RID: 66
		// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00055AE0 File Offset: 0x00053CE0
		// (set) Token: 0x06000CC6 RID: 3270 RVA: 0x00055AE8 File Offset: 0x00053CE8
		public string PushPayloadIdFieldName
		{
			get
			{
				return this.m_pushPayloadIdFieldName;
			}
			set
			{
				if (this.m_pushPayloadIdFieldName != value)
				{
					this.m_pushPayloadIdFieldName = value;
				}
			}
		}

		// Token: 0x17000043 RID: 67
		// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x00055AFF File Offset: 0x00053CFF
		// (set) Token: 0x06000CC8 RID: 3272 RVA: 0x00055B07 File Offset: 0x00053D07
		public string PushPayloadUserDataParentFieldName
		{
			get
			{
				return this.m_pushPayloadUserDataParentFieldName;
			}
			set
			{
				if (this.m_pushPayloadUserDataParentFieldName != value)
				{
					this.m_pushPayloadUserDataParentFieldName = value;
				}
			}
		}

		// Token: 0x17000044 RID: 68
		// (get) Token: 0x06000CC9 RID: 3273 RVA: 0x00055B1E File Offset: 0x00053D1E
		// (set) Token: 0x06000CCA RID: 3274 RVA: 0x00055B26 File Offset: 0x00053D26
		public string PushPayloadNotificationProfileFieldName
		{
			get
			{
				return this.m_pushPayloadNotificationProfileFieldName;
			}
			set
			{
				if (this.m_pushPayloadNotificationProfileFieldName != value)
				{
					this.m_pushPayloadNotificationProfileFieldName = value;
				}
			}
		}

		// Token: 0x17000045 RID: 69
		// (get) Token: 0x06000CCB RID: 3275 RVA: 0x00055B3D File Offset: 0x00053D3D
		// (set) Token: 0x06000CCC RID: 3276 RVA: 0x00055B45 File Offset: 0x00053D45
		public string PushPayloadBadgeFieldName
		{
			get
			{
				return this.m_pushPayloadBadgeFieldName;
			}
			set
			{
				if (this.m_pushPayloadBadgeFieldName != value)
				{
					this.m_pushPayloadBadgeFieldName = value;
				}
			}
		}

		// Token: 0x17000046 RID: 70
		// (get) Token: 0x06000CCD RID: 3277 RVA: 0x00055B5C File Offset: 0x00053D5C
		// (set) Token: 0x06000CCE RID: 3278 RVA: 0x00055B64 File Offset: 0x00053D64
		public string PushPayloadButtonsParentName
		{
			get
			{
				return this.m_pushPayloadButtonsParentName;
			}
			set
			{
				if (this.m_pushPayloadButtonsParentName != value)
				{
					this.m_pushPayloadButtonsParentName = value;
				}
			}
		}

		// Token: 0x17000047 RID: 71
		// (get) Token: 0x06000CCF RID: 3279 RVA: 0x00055B7B File Offset: 0x00053D7B
		public string GooglePlayServicesLibVersionMin
		{
			get
			{
				return Settings.m_googlePlayServicesLibVersionMin;
			}
		}

		// Token: 0x17000048 RID: 72
		// (get) Token: 0x06000CD0 RID: 3280 RVA: 0x00055B82 File Offset: 0x00053D82
		// (set) Token: 0x06000CD1 RID: 3281 RVA: 0x00055B8A File Offset: 0x00053D8A
		public string GooglePlayServicesLibVersion
		{
			get
			{
				return this.m_googlePlayServicesLibVersion;
			}
			set
			{
				if (this.m_googlePlayServicesLibVersion != value)
				{
					this.m_googlePlayServicesLibVersion = value;
				}
			}
		}

		// Token: 0x17000049 RID: 73
		// (get) Token: 0x06000CD2 RID: 3282 RVA: 0x00055BA1 File Offset: 0x00053DA1
		public string AndroidSupportLibVersionMin
		{
			get
			{
				return Settings.m_androidSupportLibVersionMin;
			}
		}

		// Token: 0x1700004A RID: 74
		// (get) Token: 0x06000CD3 RID: 3283 RVA: 0x00055BA8 File Offset: 0x00053DA8
		// (set) Token: 0x06000CD4 RID: 3284 RVA: 0x00055BB0 File Offset: 0x00053DB0
		public string AndroidSupportLibVersion
		{
			get
			{
				return this.m_androidSupportLibVersion;
			}
			set
			{
				if (this.m_androidSupportLibVersion != value)
				{
					this.m_androidSupportLibVersion = value;
				}
			}
		}

		// Token: 0x1700004B RID: 75
		// (get) Token: 0x06000CD5 RID: 3285 RVA: 0x00055BC7 File Offset: 0x00053DC7
		public string ShortcutBadgerVersionMin
		{
			get
			{
				return Settings.m_shortcutBadgerVersionMin;
			}
		}

		// Token: 0x1700004C RID: 76
		// (get) Token: 0x06000CD6 RID: 3286 RVA: 0x00055BCE File Offset: 0x00053DCE
		// (set) Token: 0x06000CD7 RID: 3287 RVA: 0x00055BD6 File Offset: 0x00053DD6
		public string ShortcutBadgerVersion
		{
			get
			{
				return this.m_shortcutBadgerVersion;
			}
			set
			{
				if (this.m_shortcutBadgerVersion != value)
				{
					this.m_shortcutBadgerVersion = value;
				}
			}
		}

		// Token: 0x1700004D RID: 77
		// (get) Token: 0x06000CD8 RID: 3288 RVA: 0x00055BED File Offset: 0x00053DED
		// (set) Token: 0x06000CD9 RID: 3289 RVA: 0x00055BF5 File Offset: 0x00053DF5
		public bool PushNotificationsEnabledIOS
		{
			get
			{
				return this.m_pushNotificationsEnabledIOS;
			}
			set
			{
				if (this.m_pushNotificationsEnabledIOS != value)
				{
					this.m_pushNotificationsEnabledIOS = value;
				}
			}
		}

		// Token: 0x1700004E RID: 78
		// (get) Token: 0x06000CDA RID: 3290 RVA: 0x00055C07 File Offset: 0x00053E07
		// (set) Token: 0x06000CDB RID: 3291 RVA: 0x00055C0F File Offset: 0x00053E0F
		public bool PushNotificationsEnabledFirebase
		{
			get
			{
				return this.m_pushNotificationsEnabledFirebase;
			}
			set
			{
				if (this.m_pushNotificationsEnabledFirebase != value)
				{
					this.m_pushNotificationsEnabledFirebase = value;
				}
			}
		}

		// Token: 0x1700004F RID: 79
		// (get) Token: 0x06000CDC RID: 3292 RVA: 0x00055C21 File Offset: 0x00053E21
		// (set) Token: 0x06000CDD RID: 3293 RVA: 0x00055C29 File Offset: 0x00053E29
		public bool PushNotificationsEnabledAmazon
		{
			get
			{
				return this.m_pushNotificationsEnabledAmazon;
			}
			set
			{
				if (this.m_pushNotificationsEnabledAmazon != value)
				{
					this.m_pushNotificationsEnabledAmazon = value;
				}
			}
		}

		// Token: 0x17000050 RID: 80
		// (get) Token: 0x06000CDE RID: 3294 RVA: 0x00055C3B File Offset: 0x00053E3B
		// (set) Token: 0x06000CDF RID: 3295 RVA: 0x00055C43 File Offset: 0x00053E43
		public bool PushNotificationsEnabledWindows
		{
			get
			{
				return this.m_pushNotificationsEnabledWindows;
			}
			set
			{
				if (this.m_pushNotificationsEnabledWindows != value)
				{
					this.m_pushNotificationsEnabledWindows = value;
				}
			}
		}

		// Token: 0x17000051 RID: 81
		// (get) Token: 0x06000CE0 RID: 3296 RVA: 0x00055C55 File Offset: 0x00053E55
		// (set) Token: 0x06000CE1 RID: 3297 RVA: 0x00055C5D File Offset: 0x00053E5D
		public Settings.ShowNotifications AndroidShowNotificationsMode
		{
			get
			{
				return this.m_androidShowNotificationsMode;
			}
			set
			{
				if (this.m_androidShowNotificationsMode != value)
				{
					this.m_androidShowNotificationsMode = value;
				}
			}
		}

		// Token: 0x17000052 RID: 82
		// (get) Token: 0x06000CE2 RID: 3298 RVA: 0x00055C6F File Offset: 0x00053E6F
		public bool AndroidRestoreScheduledNotificationsAfterReboot
		{
			get
			{
				return this.m_androidRestoreScheduledNotificationsAfterReboot;
			}
		}

		// Token: 0x17000053 RID: 83
		// (get) Token: 0x06000CE3 RID: 3299 RVA: 0x00055C77 File Offset: 0x00053E77
		// (set) Token: 0x06000CE4 RID: 3300 RVA: 0x00055C7F File Offset: 0x00053E7F
		public Settings.NotificationsGroupingMode AndroidNotificationsGrouping
		{
			get
			{
				return this.m_androidNotificationsGrouping;
			}
			set
			{
				if (this.m_androidNotificationsGrouping != value)
				{
					this.m_androidNotificationsGrouping = value;
				}
			}
		}

		// Token: 0x17000054 RID: 84
		// (get) Token: 0x06000CE5 RID: 3301 RVA: 0x00055C91 File Offset: 0x00053E91
		// (set) Token: 0x06000CE6 RID: 3302 RVA: 0x00055C99 File Offset: 0x00053E99
		public Settings.ScheduleTimerType AndroidScheduleTimerType
		{
			get
			{
				return this.m_androidScheduleTimerType;
			}
			set
			{
				if (this.m_androidScheduleTimerType != value)
				{
					this.m_androidScheduleTimerType = value;
				}
			}
		}

		// Token: 0x17000055 RID: 85
		// (get) Token: 0x06000CE7 RID: 3303 RVA: 0x00055CAB File Offset: 0x00053EAB
		// (set) Token: 0x06000CE8 RID: 3304 RVA: 0x00055CB3 File Offset: 0x00053EB3
		public Settings.GooglePlayUpdatingIfRequiredMode AllowUpdatingGooglePlayIfRequired
		{
			get
			{
				return this.m_allowUpdatingGooglePlayIfRequired;
			}
			set
			{
				if (this.m_allowUpdatingGooglePlayIfRequired != value)
				{
					this.m_allowUpdatingGooglePlayIfRequired = value;
				}
			}
		}

		// Token: 0x17000056 RID: 86
		// (get) Token: 0x06000CE9 RID: 3305 RVA: 0x00055CC5 File Offset: 0x00053EC5
		public bool AndroidShowLatestNotificationOnly
		{
			get
			{
				return this.m_androidShowLatestNotificationOnly;
			}
		}

		// Token: 0x17000057 RID: 87
		// (get) Token: 0x06000CEA RID: 3306 RVA: 0x00055CCD File Offset: 0x00053ECD
		public bool AndroidScheduleExact
		{
			get
			{
				return this.m_androidScheduleExact;
			}
		}

		// Token: 0x17000058 RID: 88
		// (get) Token: 0x06000CEB RID: 3307 RVA: 0x00055CD5 File Offset: 0x00053ED5
		public bool WindowsDontShowWhenRunning
		{
			get
			{
				return this.m_windowsDontShowWhenRunning;
			}
		}

		// Token: 0x04000D24 RID: 3364
		public const string Version = "1.8.1";

		// Token: 0x04000D25 RID: 3365
		public const string DEFAULT_PROFILE_NAME = "default";

		// Token: 0x04000D26 RID: 3366
		public const string DEFAULT_PROFILE_NAME_INTERNAL = "__default_profile";

		// Token: 0x04000D27 RID: 3367
		[SerializeField]
		private List<Settings.NotificationProfile> m_notificationProfiles = new List<Settings.NotificationProfile>();

		// Token: 0x04000D28 RID: 3368
		[SerializeField]
		private string m_pushPayloadTitleFieldName = "title";

		// Token: 0x04000D29 RID: 3369
		[SerializeField]
		private string m_pushPayloadTextFieldName = "text";

		// Token: 0x04000D2A RID: 3370
		[SerializeField]
		private string m_pushPayloadIdFieldName = "id";

		// Token: 0x04000D2B RID: 3371
		[SerializeField]
		private string m_pushPayloadUserDataParentFieldName = "";

		// Token: 0x04000D2C RID: 3372
		[SerializeField]
		private string m_pushPayloadNotificationProfileFieldName = "notification_profile";

		// Token: 0x04000D2D RID: 3373
		[SerializeField]
		private string m_pushPayloadBadgeFieldName = "badge_number";

		// Token: 0x04000D2E RID: 3374
		[SerializeField]
		private string m_pushPayloadButtonsParentName = "buttons";

		// Token: 0x04000D2F RID: 3375
		[SerializeField]
		private string m_googlePlayServicesLibVersion = Settings.m_googlePlayServicesLibVersionMin;

		// Token: 0x04000D30 RID: 3376
		private static readonly string m_googlePlayServicesLibVersionMin = "17.3.4+";

		// Token: 0x04000D31 RID: 3377
		[SerializeField]
		private string m_androidSupportLibVersion = Settings.m_androidSupportLibVersionMin;

		// Token: 0x04000D32 RID: 3378
		private static readonly string m_androidSupportLibVersionMin = "28.0.0+";

		// Token: 0x04000D33 RID: 3379
		[SerializeField]
		private string m_shortcutBadgerVersion = Settings.m_shortcutBadgerVersionMin;

		// Token: 0x04000D34 RID: 3380
		private static readonly string m_shortcutBadgerVersionMin = "1.1.22+";

		// Token: 0x04000D35 RID: 3381
		[SerializeField]
		private Settings.ShowNotifications m_androidShowNotificationsMode;

		// Token: 0x04000D36 RID: 3382
		[SerializeField]
		private bool m_android4CompatibilityMode;

		// Token: 0x04000D37 RID: 3383
		[SerializeField]
		private bool m_androidRestoreScheduledNotificationsAfterReboot = true;

		// Token: 0x04000D38 RID: 3384
		[SerializeField]
		private Settings.NotificationsGroupingMode m_androidNotificationsGrouping = Settings.NotificationsGroupingMode.BY_NOTIFICATION_PROFILES;

		// Token: 0x04000D39 RID: 3385
		[SerializeField]
		private Settings.ScheduleTimerType m_androidScheduleTimerType = Settings.ScheduleTimerType.ELAPSED_REALTIME_WAKEUP;

		// Token: 0x04000D3A RID: 3386
		[SerializeField]
		private bool m_androidShowLatestNotificationOnly;

		// Token: 0x04000D3B RID: 3387
		[SerializeField]
		private bool m_androidScheduleExact;

		// Token: 0x04000D3C RID: 3388
		[SerializeField]
		private bool m_pushNotificationsEnabledIOS;

		// Token: 0x04000D3D RID: 3389
		[SerializeField]
		private bool m_pushNotificationsEnabledFirebase;

		// Token: 0x04000D3E RID: 3390
		[SerializeField]
		private bool m_pushNotificationsEnabledAmazon;

		// Token: 0x04000D3F RID: 3391
		[SerializeField]
		private bool m_pushNotificationsEnabledWindows;

		// Token: 0x04000D40 RID: 3392
		[SerializeField]
		private Settings.GooglePlayUpdatingIfRequiredMode m_allowUpdatingGooglePlayIfRequired = Settings.GooglePlayUpdatingIfRequiredMode.ONCE;

		// Token: 0x04000D41 RID: 3393
		[SerializeField]
		private string m_assetVersionSaved = "";

		// Token: 0x04000D42 RID: 3394
		[SerializeField]
		private bool m_windowsDontShowWhenRunning = true;

		// Token: 0x04000D43 RID: 3395
		private const string m_assetName = "UTNotificationsSettings";

		// Token: 0x04000D44 RID: 3396
		private const string m_settingsMenuItem = "Edit/Project Settings... -> UTNotifications";

		// Token: 0x04000D45 RID: 3397
		private static Settings m_instance;

		// Token: 0x02000823 RID: 2083
		[Serializable]
		public struct NotificationProfile
		{
			// Token: 0x04002E4F RID: 11855
			public string profileName;

			// Token: 0x04002E50 RID: 11856
			public string iosSound;

			// Token: 0x04002E51 RID: 11857
			public string androidChannelName;

			// Token: 0x04002E52 RID: 11858
			public string androidChannelDescription;

			// Token: 0x04002E53 RID: 11859
			public string androidIcon;

			// Token: 0x04002E54 RID: 11860
			public string androidLargeIcon;

			// Token: 0x04002E55 RID: 11861
			public string androidIcon5Plus;

			// Token: 0x04002E56 RID: 11862
			[FormerlySerializedAs("androidIconBGColorSpecified")]
			public bool colorSpecified;

			// Token: 0x04002E57 RID: 11863
			[FormerlySerializedAs("androidIconBGColor")]
			public Color androidColor;

			// Token: 0x04002E58 RID: 11864
			public string androidSound;

			// Token: 0x04002E59 RID: 11865
			public bool androidHighPriority;
		}

		// Token: 0x02000824 RID: 2084
		public enum ShowNotifications
		{
			// Token: 0x04002E5B RID: 11867
			WHEN_CLOSED_OR_IN_BACKGROUND,
			// Token: 0x04002E5C RID: 11868
			WHEN_CLOSED,
			// Token: 0x04002E5D RID: 11869
			ALWAYS
		}

		// Token: 0x02000825 RID: 2085
		public enum NotificationsGroupingMode
		{
			// Token: 0x04002E5F RID: 11871
			NONE,
			// Token: 0x04002E60 RID: 11872
			BY_NOTIFICATION_PROFILES,
			// Token: 0x04002E61 RID: 11873
			FROM_USER_DATA,
			// Token: 0x04002E62 RID: 11874
			ALL_IN_A_SINGLE_GROUP
		}

		// Token: 0x02000826 RID: 2086
		public enum ScheduleTimerType
		{
			// Token: 0x04002E64 RID: 11876
			RTC_WAKEUP,
			// Token: 0x04002E65 RID: 11877
			RTC,
			// Token: 0x04002E66 RID: 11878
			ELAPSED_REALTIME_WAKEUP,
			// Token: 0x04002E67 RID: 11879
			ELAPSED_REALTIME
		}

		// Token: 0x02000827 RID: 2087
		public enum GooglePlayUpdatingIfRequiredMode
		{
			// Token: 0x04002E69 RID: 11881
			DISABLED,
			// Token: 0x04002E6A RID: 11882
			ONCE,
			// Token: 0x04002E6B RID: 11883
			EVERY_INITIALIZE
		}

		// Token: 0x02000828 RID: 2088
		private class UpdateMessage
		{
			// Token: 0x06004457 RID: 17495 RVA: 0x00141F64 File Offset: 0x00140164
			public UpdateMessage(string version, string text)
			{
				this.version = version;
				this.text = text;
			}

			// Token: 0x04002E6C RID: 11884
			public readonly string version;

			// Token: 0x04002E6D RID: 11885
			public readonly string text;
		}
	}
}
