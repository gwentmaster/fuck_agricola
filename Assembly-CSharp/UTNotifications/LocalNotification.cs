using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000148 RID: 328
	public class LocalNotification : Notification
	{
		// Token: 0x06000C90 RID: 3216 RVA: 0x000555C1 File Offset: 0x000537C1
		public LocalNotification(string title, string text, int id, IDictionary<string, string> userData, string notificationProfile, int badgeNumber, ICollection<Button> buttons) : base(title, text, id, userData, notificationProfile, badgeNumber, buttons)
		{
		}

		// Token: 0x06000C91 RID: 3217 RVA: 0x000555D4 File Offset: 0x000537D4
		public LocalNotification(string title, string text, int id) : base(title, text, id)
		{
		}

		// Token: 0x06000C92 RID: 3218 RVA: 0x000555DF File Offset: 0x000537DF
		public LocalNotification(JSONNode json) : base(json)
		{
		}

		// Token: 0x06000C93 RID: 3219 RVA: 0x000555E8 File Offset: 0x000537E8
		public new LocalNotification SetUserData(IDictionary<string, string> userData)
		{
			base.SetUserData(userData);
			return this;
		}

		// Token: 0x06000C94 RID: 3220 RVA: 0x000555F3 File Offset: 0x000537F3
		public new LocalNotification SetNotificationProfile(string notificationProfile)
		{
			base.SetNotificationProfile(notificationProfile);
			return this;
		}

		// Token: 0x06000C95 RID: 3221 RVA: 0x000555FE File Offset: 0x000537FE
		public new LocalNotification SetBadgeNumber(int badgeNumber)
		{
			base.SetBadgeNumber(badgeNumber);
			return this;
		}

		// Token: 0x06000C96 RID: 3222 RVA: 0x00055609 File Offset: 0x00053809
		public new LocalNotification SetButtons(ICollection<Button> buttons)
		{
			base.SetButtons(buttons);
			return this;
		}
	}
}
