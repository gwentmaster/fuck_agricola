using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000143 RID: 323
	public class ManagerImpl : Manager
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool NotificationsEnabled()
		{
			return false;
		}

		// Token: 0x06000C28 RID: 3112 RVA: 0x0002A062 File Offset: 0x00028262
		public override bool NotificationsAllowed()
		{
			return false;
		}

		// Token: 0x06000C29 RID: 3113 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void SetNotificationsEnabled(bool enabled)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C2A RID: 3114 RVA: 0x000547C9 File Offset: 0x000529C9
		public override bool PushNotificationsEnabled()
		{
			base.NotSupported(null);
			return false;
		}

		// Token: 0x06000C2B RID: 3115 RVA: 0x000547C9 File Offset: 0x000529C9
		public override bool SetPushNotificationsEnabled(bool enable)
		{
			base.NotSupported(null);
			return false;
		}

		// Token: 0x06000C2C RID: 3116 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void HideNotification(int id)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C2D RID: 3117 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void HideAllNotifications()
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C2E RID: 3118 RVA: 0x000547C9 File Offset: 0x000529C9
		public override int GetBadge()
		{
			base.NotSupported(null);
			return 0;
		}

		// Token: 0x06000C2F RID: 3119 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void SetBadge(int bandgeNumber)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C30 RID: 3120 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void SubscribeToTopic(string topic)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C31 RID: 3121 RVA: 0x000547C0 File Offset: 0x000529C0
		public override void UnsubscribeFromTopic(string topic)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C32 RID: 3122 RVA: 0x000547C9 File Offset: 0x000529C9
		protected override bool InitializeImpl(bool willHandleReceivedNotifications, int startId, bool incrementalId)
		{
			base.NotSupported(null);
			return false;
		}

		// Token: 0x06000C33 RID: 3123 RVA: 0x000547C0 File Offset: 0x000529C0
		protected override void PostLocalNotificationImpl(LocalNotification notification)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C34 RID: 3124 RVA: 0x000547C0 File Offset: 0x000529C0
		protected override void ScheduleNotificationImpl(ScheduledNotification notification)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C35 RID: 3125 RVA: 0x000547C0 File Offset: 0x000529C0
		protected override void ScheduleNotificationRepeatingImpl(ScheduledRepeatingNotification notification)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C36 RID: 3126 RVA: 0x000547C0 File Offset: 0x000529C0
		protected override void CancelNotificationImpl(int id)
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C37 RID: 3127 RVA: 0x000547C0 File Offset: 0x000529C0
		protected override void CancelAllNotificationsImpl()
		{
			base.NotSupported(null);
		}

		// Token: 0x06000C38 RID: 3128 RVA: 0x0002A062 File Offset: 0x00028262
		protected override bool CleanupObsoleteScheduledNotifications(List<ScheduledNotification> scheduledNotifications)
		{
			return false;
		}
	}
}
