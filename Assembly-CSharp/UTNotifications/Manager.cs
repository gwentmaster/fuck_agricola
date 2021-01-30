using System;
using System.Collections.Generic;
using UnityEngine;

namespace UTNotifications
{
	// Token: 0x02000145 RID: 325
	public abstract class Manager : MonoBehaviour
	{
		// Token: 0x1700002C RID: 44
		// (get) Token: 0x06000C3F RID: 3135 RVA: 0x00054A00 File Offset: 0x00052C00
		public static Manager Instance
		{
			get
			{
				Manager.InstanceRequired();
				return Manager.m_instance;
			}
		}

		// Token: 0x1700002D RID: 45
		// (get) Token: 0x06000C40 RID: 3136 RVA: 0x00054A0C File Offset: 0x00052C0C
		// (set) Token: 0x06000C41 RID: 3137 RVA: 0x00054A14 File Offset: 0x00052C14
		public bool Initialized
		{
			get
			{
				return this.m_initialized;
			}
			protected set
			{
				if (this.m_initialized != value)
				{
					this.m_initialized = value;
					if (value)
					{
						Manager.OnInitializedHandler onInitialized = this.OnInitialized;
						if (onInitialized != null)
						{
							onInitialized();
						}
					}
				}
			}
		}

		// Token: 0x06000C42 RID: 3138 RVA: 0x00054A44 File Offset: 0x00052C44
		public bool Initialize(bool willHandleReceivedNotifications, int startId = 0, bool incrementalId = false)
		{
			bool result;
			try
			{
				this.Initialized = false;
				this.LoadScheduledNotifications();
				result = this.InitializeImpl(willHandleReceivedNotifications, startId, incrementalId);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
				result = false;
			}
			return result;
		}

		// Token: 0x06000C43 RID: 3139 RVA: 0x00054A84 File Offset: 0x00052C84
		public void PostLocalNotification(string title, string text, int id, IDictionary<string, string> userData = null, string notificationProfile = null, int badgeNumber = -1, ICollection<Button> buttons = null)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.PostLocalNotification(new LocalNotification(title, text, id, userData, notificationProfile, badgeNumber, buttons));
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C44 RID: 3140 RVA: 0x00054ACC File Offset: 0x00052CCC
		public void PostLocalNotification(LocalNotification notification)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				if (notification is ScheduledNotification)
				{
					this.ScheduleNotification((ScheduledNotification)notification);
				}
				else
				{
					this.PostLocalNotificationImpl(notification);
					this.UnregisterScheduledNotification(notification.id);
				}
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C45 RID: 3141 RVA: 0x00054B28 File Offset: 0x00052D28
		public void ScheduleNotification(int triggerInSeconds, string title, string text, int id, IDictionary<string, string> userData = null, string notificationProfile = null, int badgeNumber = -1, ICollection<Button> buttons = null)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.ScheduleNotification(TimeUtils.ToDateTime(triggerInSeconds), title, text, id, userData, notificationProfile, badgeNumber, buttons);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C46 RID: 3142 RVA: 0x00054B70 File Offset: 0x00052D70
		public void ScheduleNotification(DateTime triggerDateTime, string title, string text, int id, IDictionary<string, string> userData = null, string notificationProfile = null, int badgeNumber = -1, ICollection<Button> buttons = null)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.ScheduleNotification(new ScheduledNotification(triggerDateTime, title, text, id, userData, notificationProfile, badgeNumber, buttons));
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C47 RID: 3143 RVA: 0x00054BB8 File Offset: 0x00052DB8
		public void ScheduleNotification(ScheduledNotification notification)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				if (notification.IsRepeating)
				{
					this.ScheduleNotificationRepeatingImpl((ScheduledRepeatingNotification)notification);
				}
				else
				{
					this.ScheduleNotificationImpl(notification);
				}
				this.RegisterScheduledNotification(notification);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C48 RID: 3144 RVA: 0x00054C0C File Offset: 0x00052E0C
		public void ScheduleNotificationRepeating(int firstTriggerInSeconds, int intervalSeconds, string title, string text, int id, IDictionary<string, string> userData = null, string notificationProfile = null, int badgeNumber = -1, ICollection<Button> buttons = null)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.ScheduleNotificationRepeating(TimeUtils.ToDateTime(firstTriggerInSeconds), intervalSeconds, title, text, id, userData, notificationProfile, badgeNumber, buttons);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C49 RID: 3145 RVA: 0x00054C58 File Offset: 0x00052E58
		public void ScheduleNotificationRepeating(DateTime firstTriggerDateTime, int intervalSeconds, string title, string text, int id, IDictionary<string, string> userData = null, string notificationProfile = null, int badgeNumber = -1, ICollection<Button> buttons = null)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.ScheduleNotification(new ScheduledRepeatingNotification(firstTriggerDateTime, intervalSeconds, title, text, id, userData, notificationProfile, badgeNumber, buttons));
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C4A RID: 3146
		public abstract bool NotificationsEnabled();

		// Token: 0x06000C4B RID: 3147
		public abstract bool NotificationsAllowed();

		// Token: 0x06000C4C RID: 3148
		public abstract void SetNotificationsEnabled(bool enabled);

		// Token: 0x06000C4D RID: 3149
		public abstract bool PushNotificationsEnabled();

		// Token: 0x06000C4E RID: 3150
		public abstract bool SetPushNotificationsEnabled(bool enable);

		// Token: 0x06000C4F RID: 3151 RVA: 0x00054CA4 File Offset: 0x00052EA4
		public void CancelNotification(int id)
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.CancelNotificationImpl(id);
				this.UnregisterScheduledNotification(id);
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C50 RID: 3152
		public abstract void HideNotification(int id);

		// Token: 0x06000C51 RID: 3153 RVA: 0x00054CE4 File Offset: 0x00052EE4
		public void CancelAllNotifications()
		{
			if (!this.CheckInitialized())
			{
				return;
			}
			try
			{
				this.CancelAllNotificationsImpl();
				this.UnregisterAllScheduledNotifications();
			}
			catch (Exception exception)
			{
				Debug.LogException(exception);
			}
		}

		// Token: 0x06000C52 RID: 3154
		public abstract void HideAllNotifications();

		// Token: 0x06000C53 RID: 3155
		public abstract int GetBadge();

		// Token: 0x06000C54 RID: 3156
		public abstract void SetBadge(int bandgeNumber);

		// Token: 0x06000C55 RID: 3157
		public abstract void SubscribeToTopic(string topic);

		// Token: 0x06000C56 RID: 3158
		public abstract void UnsubscribeFromTopic(string topic);

		// Token: 0x1700002E RID: 46
		// (get) Token: 0x06000C57 RID: 3159 RVA: 0x00054D20 File Offset: 0x00052F20
		public ICollection<ScheduledNotification> ScheduledNotifications
		{
			get
			{
				if (!this.CheckInitialized())
				{
					return null;
				}
				this.CleanupObsoleteScheduledNotifications();
				return this.m_scheduledNotifications;
			}
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000C58 RID: 3160 RVA: 0x00054D38 File Offset: 0x00052F38
		// (remove) Token: 0x06000C59 RID: 3161 RVA: 0x00054D70 File Offset: 0x00052F70
		public event Manager.OnInitializedHandler OnInitialized;

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000C5A RID: 3162 RVA: 0x00054DA8 File Offset: 0x00052FA8
		// (remove) Token: 0x06000C5B RID: 3163 RVA: 0x00054DE0 File Offset: 0x00052FE0
		public event Manager.OnSendRegistrationIdHandler OnSendRegistrationId;

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x06000C5C RID: 3164 RVA: 0x00054E18 File Offset: 0x00053018
		// (remove) Token: 0x06000C5D RID: 3165 RVA: 0x00054E50 File Offset: 0x00053050
		public event Manager.OnPushRegistrationFailedHandler OnPushRegistrationFailed;

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x06000C5E RID: 3166 RVA: 0x00054E88 File Offset: 0x00053088
		// (remove) Token: 0x06000C5F RID: 3167 RVA: 0x00054EC0 File Offset: 0x000530C0
		public event Manager.OnNotificationClickedHandler OnNotificationClicked;

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x06000C60 RID: 3168 RVA: 0x00054EF8 File Offset: 0x000530F8
		// (remove) Token: 0x06000C61 RID: 3169 RVA: 0x00054F30 File Offset: 0x00053130
		public event Manager.OnNotificationsReceivedHandler OnNotificationsReceived;

		// Token: 0x06000C62 RID: 3170
		protected abstract bool InitializeImpl(bool willHandleReceivedNotifications, int startId, bool incrementalId);

		// Token: 0x06000C63 RID: 3171
		protected abstract void PostLocalNotificationImpl(LocalNotification notification);

		// Token: 0x06000C64 RID: 3172
		protected abstract void ScheduleNotificationImpl(ScheduledNotification notification);

		// Token: 0x06000C65 RID: 3173
		protected abstract void ScheduleNotificationRepeatingImpl(ScheduledRepeatingNotification notification);

		// Token: 0x06000C66 RID: 3174
		protected abstract void CancelNotificationImpl(int id);

		// Token: 0x06000C67 RID: 3175
		protected abstract void CancelAllNotificationsImpl();

		// Token: 0x06000C68 RID: 3176
		protected abstract bool CleanupObsoleteScheduledNotifications(List<ScheduledNotification> scheduledNotifications);

		// Token: 0x06000C69 RID: 3177 RVA: 0x00054F65 File Offset: 0x00053165
		protected bool OnSendRegistrationIdHasSubscribers()
		{
			return this.OnSendRegistrationId != null;
		}

		// Token: 0x06000C6A RID: 3178 RVA: 0x00054F70 File Offset: 0x00053170
		protected void _OnSendRegistrationId(string providerName, string registrationId)
		{
			Manager.OnSendRegistrationIdHandler onSendRegistrationId = this.OnSendRegistrationId;
			if (onSendRegistrationId != null)
			{
				onSendRegistrationId(providerName, registrationId);
			}
		}

		// Token: 0x06000C6B RID: 3179 RVA: 0x00054F8F File Offset: 0x0005318F
		protected bool OnPushRegistrationHasSubscribers()
		{
			return this.OnPushRegistrationFailed != null;
		}

		// Token: 0x06000C6C RID: 3180 RVA: 0x00054F9C File Offset: 0x0005319C
		protected void _OnPushRegistrationFailed(string error)
		{
			Manager.OnPushRegistrationFailedHandler onPushRegistrationFailed = this.OnPushRegistrationFailed;
			if (onPushRegistrationFailed != null)
			{
				onPushRegistrationFailed(error);
			}
		}

		// Token: 0x06000C6D RID: 3181 RVA: 0x00054FBA File Offset: 0x000531BA
		protected bool OnNotificationClickedHasSubscribers()
		{
			return this.OnNotificationClicked != null;
		}

		// Token: 0x06000C6E RID: 3182 RVA: 0x00054FC8 File Offset: 0x000531C8
		protected void _OnNotificationClicked(ReceivedNotification notification)
		{
			this.CleanupReceivedScheduledNotification(new List<ReceivedNotification>
			{
				notification
			});
			Manager.OnNotificationClickedHandler onNotificationClicked = this.OnNotificationClicked;
			if (onNotificationClicked != null)
			{
				onNotificationClicked(notification);
			}
		}

		// Token: 0x06000C6F RID: 3183 RVA: 0x00054FF8 File Offset: 0x000531F8
		protected bool OnNotificationsReceivedHasSubscribers()
		{
			return this.OnNotificationsReceived != null;
		}

		// Token: 0x06000C70 RID: 3184 RVA: 0x00055004 File Offset: 0x00053204
		protected void _OnNotificationsReceived(IList<ReceivedNotification> receivedNotifications)
		{
			this.CleanupReceivedScheduledNotification((receivedNotifications is List<ReceivedNotification>) ? ((List<ReceivedNotification>)receivedNotifications) : new List<ReceivedNotification>(receivedNotifications));
			Manager.OnNotificationsReceivedHandler onNotificationsReceived = this.OnNotificationsReceived;
			if (onNotificationsReceived != null)
			{
				onNotificationsReceived(receivedNotifications);
			}
		}

		// Token: 0x06000C71 RID: 3185 RVA: 0x0005503E File Offset: 0x0005323E
		protected virtual void OnDestroy()
		{
			Manager.m_instance = null;
			Manager.m_destroyed = true;
		}

		// Token: 0x06000C72 RID: 3186 RVA: 0x0005504C File Offset: 0x0005324C
		protected void NotSupported(string feature = null)
		{
			if (feature == null)
			{
				Debug.LogWarning("UTNotifications: not supported on this platform");
				return;
			}
			Debug.LogWarning("UTNotifications: " + feature + " feature is not supported on this platform");
		}

		// Token: 0x06000C73 RID: 3187 RVA: 0x00055071 File Offset: 0x00053271
		protected bool CheckInitialized()
		{
			if (!this.m_initialized)
			{
				Debug.LogError("Please call UTNotifications.Manager.Instance.Initialize(...) first!");
			}
			return this.m_initialized;
		}

		// Token: 0x06000C74 RID: 3188 RVA: 0x0005508B File Offset: 0x0005328B
		private static void InstanceRequired()
		{
			if (!Manager.m_instance && !Manager.m_destroyed)
			{
				GameObject gameObject = new GameObject("UTNotificationsManager");
				Manager.m_instance = gameObject.AddComponent<ManagerImpl>();
				UnityEngine.Object.DontDestroyOnLoad(gameObject);
			}
		}

		// Token: 0x06000C75 RID: 3189 RVA: 0x000550BA File Offset: 0x000532BA
		private void CleanupObsoleteScheduledNotifications()
		{
			if (this.CleanupObsoleteScheduledNotifications(this.m_scheduledNotifications))
			{
				this.SaveScheduledNotifications();
			}
		}

		// Token: 0x06000C76 RID: 3190 RVA: 0x000550D0 File Offset: 0x000532D0
		private void CleanupReceivedScheduledNotification(List<ReceivedNotification> list)
		{
			if (this.m_scheduledNotifications.RemoveAll((ScheduledNotification it) => !it.IsRepeating && list.Find((ReceivedNotification received) => received.id == it.id) != null) > 0)
			{
				this.SaveScheduledNotifications();
			}
		}

		// Token: 0x06000C77 RID: 3191 RVA: 0x0005510A File Offset: 0x0005330A
		private void RegisterScheduledNotification(ScheduledNotification notification)
		{
			this.CleanupObsoleteScheduledNotifications();
			this.UnregisterScheduledNotification(notification.id);
			this.m_scheduledNotifications.Add(notification);
			this.SaveScheduledNotifications();
		}

		// Token: 0x06000C78 RID: 3192 RVA: 0x00055130 File Offset: 0x00053330
		private void UnregisterScheduledNotification(int id)
		{
			if (this.m_scheduledNotifications.RemoveAll((ScheduledNotification it) => it.id == id) > 0)
			{
				this.SaveScheduledNotifications();
			}
		}

		// Token: 0x06000C79 RID: 3193 RVA: 0x0005516A File Offset: 0x0005336A
		private void UnregisterAllScheduledNotifications()
		{
			if (this.m_scheduledNotifications.Count > 0)
			{
				this.m_scheduledNotifications.Clear();
				this.SaveScheduledNotifications();
			}
		}

		// Token: 0x06000C7A RID: 3194 RVA: 0x0005518C File Offset: 0x0005338C
		private void SaveScheduledNotifications()
		{
			if (!this.Initialized)
			{
				Debug.LogError("UTNotifications.Manager must be initialized!");
				return;
			}
			JSONArray jsonarray = new JSONArray();
			foreach (ScheduledNotification scheduledNotification in this.m_scheduledNotifications)
			{
				jsonarray.Add(scheduledNotification.ToJson());
			}
			PlayerPrefs.SetString(Manager.SCHEDULED_NOTIFICATIONS_PREFS_KEY, jsonarray.ToString());
		}

		// Token: 0x06000C7B RID: 3195 RVA: 0x00055210 File Offset: 0x00053410
		private void LoadScheduledNotifications()
		{
			this.m_scheduledNotifications.Clear();
			string @string = PlayerPrefs.GetString(Manager.SCHEDULED_NOTIFICATIONS_PREFS_KEY);
			if (!string.IsNullOrEmpty(@string))
			{
				JSONArray jsonarray = JSON.Parse(@string) as JSONArray;
				if (jsonarray != null)
				{
					DateTime now = DateTime.Now;
					for (int i = 0; i < jsonarray.Count; i++)
					{
						ScheduledNotification scheduledNotification = Notification.FromJson(jsonarray[i]) as ScheduledNotification;
						if (scheduledNotification != null)
						{
							this.m_scheduledNotifications.Add(scheduledNotification);
						}
					}
				}
			}
		}

		// Token: 0x04000D0C RID: 3340
		private static readonly string SCHEDULED_NOTIFICATIONS_PREFS_KEY = "_UT_NOTIFICATIONS_SCHEDULED_NOTIFICATIONS";

		// Token: 0x04000D0D RID: 3341
		private static Manager m_instance = null;

		// Token: 0x04000D0E RID: 3342
		private static bool m_destroyed = false;

		// Token: 0x04000D0F RID: 3343
		private bool m_initialized;

		// Token: 0x04000D10 RID: 3344
		private readonly List<ScheduledNotification> m_scheduledNotifications = new List<ScheduledNotification>();

		// Token: 0x0200081B RID: 2075
		// (Invoke) Token: 0x0600443E RID: 17470
		public delegate void OnInitializedHandler();

		// Token: 0x0200081C RID: 2076
		// (Invoke) Token: 0x06004442 RID: 17474
		public delegate void OnSendRegistrationIdHandler(string providerName, string registrationId);

		// Token: 0x0200081D RID: 2077
		// (Invoke) Token: 0x06004446 RID: 17478
		public delegate void OnPushRegistrationFailedHandler(string error);

		// Token: 0x0200081E RID: 2078
		// (Invoke) Token: 0x0600444A RID: 17482
		public delegate void OnNotificationClickedHandler(ReceivedNotification notification);

		// Token: 0x0200081F RID: 2079
		// (Invoke) Token: 0x0600444E RID: 17486
		public delegate void OnNotificationsReceivedHandler(IList<ReceivedNotification> receivedNotifications);
	}
}
