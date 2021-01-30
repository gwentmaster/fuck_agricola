using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000149 RID: 329
	public class PushNotification : Notification
	{
		// Token: 0x06000C97 RID: 3223 RVA: 0x00055614 File Offset: 0x00053814
		public PushNotification(PushNotificationsProvider provider, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile, int badgeNumber, ICollection<Button> buttons) : base(title, text, id, userData, notificationProfile, badgeNumber, buttons)
		{
			this.provider = provider;
		}

		// Token: 0x06000C98 RID: 3224 RVA: 0x0005562F File Offset: 0x0005382F
		public PushNotification(PushNotificationsProvider provider, string title, string text, int id) : base(title, text, id)
		{
			this.provider = provider;
		}

		// Token: 0x06000C99 RID: 3225 RVA: 0x00055642 File Offset: 0x00053842
		public PushNotification(JSONNode json) : base(json)
		{
			this.provider = (PushNotificationsProvider)Enum.Parse(typeof(PushNotificationsProvider), json["provider"].Value);
		}

		// Token: 0x06000C9A RID: 3226 RVA: 0x00055675 File Offset: 0x00053875
		public override JSONClass ToJson()
		{
			JSONClass jsonclass = base.ToJson();
			jsonclass.Add("provider", new JSONData(this.provider.ToString()));
			return jsonclass;
		}

		// Token: 0x06000C9B RID: 3227 RVA: 0x000555E8 File Offset: 0x000537E8
		public new PushNotification SetUserData(IDictionary<string, string> userData)
		{
			base.SetUserData(userData);
			return this;
		}

		// Token: 0x06000C9C RID: 3228 RVA: 0x000555F3 File Offset: 0x000537F3
		public new PushNotification SetNotificationProfile(string notificationProfile)
		{
			base.SetNotificationProfile(notificationProfile);
			return this;
		}

		// Token: 0x06000C9D RID: 3229 RVA: 0x000555FE File Offset: 0x000537FE
		public new PushNotification SetBadgeNumber(int badgeNumber)
		{
			base.SetBadgeNumber(badgeNumber);
			return this;
		}

		// Token: 0x06000C9E RID: 3230 RVA: 0x00055609 File Offset: 0x00053809
		public new PushNotification SetButtons(ICollection<Button> buttons)
		{
			base.SetButtons(buttons);
			return this;
		}

		// Token: 0x04000D1E RID: 3358
		public readonly PushNotificationsProvider provider;
	}
}
