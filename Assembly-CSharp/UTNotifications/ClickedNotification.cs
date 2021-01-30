using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x0200014D RID: 333
	public class ClickedNotification : ReceivedNotification
	{
		// Token: 0x06000CBC RID: 3260 RVA: 0x000558DF File Offset: 0x00053ADF
		public ClickedNotification(Notification notification, int clickedButtonIndex) : base(notification)
		{
			this.clickedButtonIndex = clickedButtonIndex;
		}

		// Token: 0x06000CBD RID: 3261 RVA: 0x000558F0 File Offset: 0x00053AF0
		public ClickedNotification(JSONNode json) : base(json)
		{
			int num;
			if (!int.TryParse(json["buttonIndex"].Value, out num))
			{
				num = -1;
			}
			this.clickedButtonIndex = num;
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x06000CBE RID: 3262 RVA: 0x00055928 File Offset: 0x00053B28
		public override IDictionary<string, string> userData
		{
			get
			{
				if (this.clickedButtonIndex < 0 || base.notification.buttons == null || this.clickedButtonIndex >= base.notification.buttons.Count)
				{
					return base.userData;
				}
				if (base.notification.buttons is IList<Button>)
				{
					return ((IList<Button>)base.notification.buttons)[this.clickedButtonIndex].userData;
				}
				int num = 0;
				foreach (Button button in base.notification.buttons)
				{
					if (num++ == this.clickedButtonIndex)
					{
						return button.userData;
					}
				}
				return base.userData;
			}
		}

		// Token: 0x04000D22 RID: 3362
		public const int BUTTON_NONE = -1;

		// Token: 0x04000D23 RID: 3363
		public readonly int clickedButtonIndex;
	}
}
