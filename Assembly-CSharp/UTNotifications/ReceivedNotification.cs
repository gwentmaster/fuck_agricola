using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x0200014C RID: 332
	public class ReceivedNotification
	{
		// Token: 0x06000CB1 RID: 3249 RVA: 0x00055850 File Offset: 0x00053A50
		public ReceivedNotification(Notification notification)
		{
			this.notification = notification;
		}

		// Token: 0x06000CB2 RID: 3250 RVA: 0x0005585F File Offset: 0x00053A5F
		public ReceivedNotification(JSONNode json)
		{
			this.notification = Notification.FromJson(json);
		}

		// Token: 0x17000035 RID: 53
		// (get) Token: 0x06000CB3 RID: 3251 RVA: 0x00055873 File Offset: 0x00053A73
		// (set) Token: 0x06000CB4 RID: 3252 RVA: 0x0005587B File Offset: 0x00053A7B
		public Notification notification { get; private set; }

		// Token: 0x17000036 RID: 54
		// (get) Token: 0x06000CB5 RID: 3253 RVA: 0x00055884 File Offset: 0x00053A84
		public string title
		{
			get
			{
				return this.notification.title;
			}
		}

		// Token: 0x17000037 RID: 55
		// (get) Token: 0x06000CB6 RID: 3254 RVA: 0x00055891 File Offset: 0x00053A91
		public string text
		{
			get
			{
				return this.notification.text;
			}
		}

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000CB7 RID: 3255 RVA: 0x0005589E File Offset: 0x00053A9E
		public int id
		{
			get
			{
				return this.notification.id;
			}
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x06000CB8 RID: 3256 RVA: 0x000558AB File Offset: 0x00053AAB
		public virtual IDictionary<string, string> userData
		{
			get
			{
				return this.notification.userData;
			}
		}

		// Token: 0x1700003A RID: 58
		// (get) Token: 0x06000CB9 RID: 3257 RVA: 0x000558B8 File Offset: 0x00053AB8
		public string notificationProfile
		{
			get
			{
				return this.notification.notificationProfile;
			}
		}

		// Token: 0x1700003B RID: 59
		// (get) Token: 0x06000CBA RID: 3258 RVA: 0x000558C5 File Offset: 0x00053AC5
		public int badgeNumber
		{
			get
			{
				return this.notification.badgeNumber;
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x06000CBB RID: 3259 RVA: 0x000558D2 File Offset: 0x00053AD2
		public ICollection<Button> buttons
		{
			get
			{
				return this.notification.buttons;
			}
		}
	}
}
