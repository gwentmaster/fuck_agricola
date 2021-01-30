using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x0200013D RID: 317
	[RequireComponent(typeof(Text))]
	public class ScheduledNotificationsList : MonoBehaviour
	{
		// Token: 0x06000BF3 RID: 3059 RVA: 0x00053EF6 File Offset: 0x000520F6
		private void Awake()
		{
			this.text = base.GetComponent<Text>();
			this.originalText = this.text.text;
		}

		// Token: 0x06000BF4 RID: 3060 RVA: 0x00053F18 File Offset: 0x00052118
		private void Update()
		{
			Manager instance = Manager.Instance;
			if (!instance.Initialized)
			{
				return;
			}
			ICollection<ScheduledNotification> scheduledNotifications = instance.ScheduledNotifications;
			if (scheduledNotifications == null || scheduledNotifications.Count == 0)
			{
				this.text.text = this.originalText;
				return;
			}
			string text = "";
			bool flag = true;
			foreach (ScheduledNotification scheduledNotification in scheduledNotifications)
			{
				if (scheduledNotification is ScheduledRepeatingNotification == this.Repeated)
				{
					if (flag)
					{
						flag = false;
					}
					else
					{
						text += ", ";
					}
					text += scheduledNotification.id.ToString();
				}
			}
			if (flag)
			{
				this.text.text = this.originalText;
				return;
			}
			this.text.text = this.originalText + "\nIDs: [" + text + "]";
		}

		// Token: 0x04000CF1 RID: 3313
		public bool Repeated;

		// Token: 0x04000CF2 RID: 3314
		private string originalText;

		// Token: 0x04000CF3 RID: 3315
		private Text text;
	}
}
