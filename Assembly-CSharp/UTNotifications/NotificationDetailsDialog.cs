using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000138 RID: 312
	public class NotificationDetailsDialog : MonoBehaviour
	{
		// Token: 0x17000029 RID: 41
		// (get) Token: 0x06000BDE RID: 3038 RVA: 0x00053ADE File Offset: 0x00051CDE
		public ReceivedNotification Current
		{
			get
			{
				if (this.clicked != null)
				{
					return this.clicked;
				}
				if (this.received.Count > 0)
				{
					return this.received[0];
				}
				return null;
			}
		}

		// Token: 0x06000BDF RID: 3039 RVA: 0x00053B0B File Offset: 0x00051D0B
		public void OnReceived(ReceivedNotification received)
		{
			this.received.Add(received);
			if (this.clicked == null && this.received.Count == 1)
			{
				this.UpdateContents();
			}
		}

		// Token: 0x06000BE0 RID: 3040 RVA: 0x00053B35 File Offset: 0x00051D35
		public void OnClicked(ReceivedNotification clicked)
		{
			this.clicked = clicked;
			this.UpdateContents();
		}

		// Token: 0x06000BE1 RID: 3041 RVA: 0x00053B44 File Offset: 0x00051D44
		public void Hide()
		{
			ReceivedNotification receivedNotification = this.Current;
			UTNotificationsSample.Instance.Hide(receivedNotification.id);
		}

		// Token: 0x06000BE2 RID: 3042 RVA: 0x00053B68 File Offset: 0x00051D68
		public void Hide(int id)
		{
			if (this.clicked != null && this.clicked.id == id)
			{
				this.clicked = null;
			}
			this.received.RemoveAll((ReceivedNotification it) => it.id == id);
			this.UpdateContents();
		}

		// Token: 0x06000BE3 RID: 3043 RVA: 0x00053BC4 File Offset: 0x00051DC4
		public void Cancel()
		{
			ReceivedNotification receivedNotification = this.Current;
			UTNotificationsSample.Instance.Cancel(receivedNotification.id);
		}

		// Token: 0x06000BE4 RID: 3044 RVA: 0x00053BE8 File Offset: 0x00051DE8
		public void CancelAll()
		{
			this.clicked = null;
			this.received.Clear();
			this.UpdateContents();
		}

		// Token: 0x06000BE5 RID: 3045 RVA: 0x00003022 File Offset: 0x00001222
		private void Start()
		{
		}

		// Token: 0x06000BE6 RID: 3046 RVA: 0x00053C04 File Offset: 0x00051E04
		private void UpdateContents()
		{
			ReceivedNotification receivedNotification = this.Current;
			if (receivedNotification == null)
			{
				base.gameObject.SetActive(false);
				return;
			}
			string str;
			if (receivedNotification.notification is PushNotification)
			{
				str = "Push Notification";
			}
			else if (receivedNotification.notification is ScheduledRepeatingNotification)
			{
				str = "Scheduled Repeating Notification";
			}
			else if (receivedNotification.notification is ScheduledNotification)
			{
				str = "Scheduled Notification";
			}
			else if (receivedNotification.notification is LocalNotification)
			{
				str = "Immediate Local Notification";
			}
			else
			{
				str = receivedNotification.notification.GetType().ToString();
			}
			this.DialogTitle.text = str + ((receivedNotification == this.clicked) ? " Clicked" : " Received");
			this.ID.text = receivedNotification.id.ToString();
			this.Title.text = receivedNotification.title;
			this.Text.text = receivedNotification.text;
			this.Profile.text = (receivedNotification.notificationProfile ?? "");
			this.UserData.text = this.UserDataString(receivedNotification.userData);
			this.Badge.text = ((receivedNotification.badgeNumber != -1) ? receivedNotification.badgeNumber.ToString() : "");
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000BE7 RID: 3047 RVA: 0x00053D54 File Offset: 0x00051F54
		private string UserDataString(IDictionary<string, string> userData)
		{
			if (userData == null)
			{
				return "{}";
			}
			return JsonUtils.ToJson(userData).ToString();
		}

		// Token: 0x04000CE3 RID: 3299
		public Text DialogTitle;

		// Token: 0x04000CE4 RID: 3300
		public Text ID;

		// Token: 0x04000CE5 RID: 3301
		public Text Title;

		// Token: 0x04000CE6 RID: 3302
		public Text Text;

		// Token: 0x04000CE7 RID: 3303
		public Text Profile;

		// Token: 0x04000CE8 RID: 3304
		public Text UserData;

		// Token: 0x04000CE9 RID: 3305
		public Text Badge;

		// Token: 0x04000CEA RID: 3306
		private readonly List<ReceivedNotification> received = new List<ReceivedNotification>();

		// Token: 0x04000CEB RID: 3307
		private ReceivedNotification clicked;
	}
}
