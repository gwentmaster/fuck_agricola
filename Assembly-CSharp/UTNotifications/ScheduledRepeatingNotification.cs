using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x0200014B RID: 331
	public class ScheduledRepeatingNotification : ScheduledNotification
	{
		// Token: 0x06000CA8 RID: 3240 RVA: 0x0005578C File Offset: 0x0005398C
		public ScheduledRepeatingNotification(DateTime triggerDateTime, int intervalSeconds, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile, int badgeNumber, ICollection<Button> buttons) : base(triggerDateTime, title, text, id, userData, notificationProfile, badgeNumber, buttons)
		{
			this.intervalSeconds = intervalSeconds;
		}

		// Token: 0x06000CA9 RID: 3241 RVA: 0x000557B4 File Offset: 0x000539B4
		public ScheduledRepeatingNotification(DateTime triggerDateTime, int intervalSeconds, string title, string text, int id) : base(triggerDateTime, title, text, id)
		{
			this.intervalSeconds = intervalSeconds;
		}

		// Token: 0x06000CAA RID: 3242 RVA: 0x000557C9 File Offset: 0x000539C9
		public ScheduledRepeatingNotification(JSONNode json) : base(json)
		{
			this.intervalSeconds = json["intervalSeconds"].AsInt;
		}

		// Token: 0x06000CAB RID: 3243 RVA: 0x000557E8 File Offset: 0x000539E8
		public override JSONClass ToJson()
		{
			JSONClass jsonclass = base.ToJson();
			jsonclass.Add("intervalSeconds", new JSONData(this.intervalSeconds.ToString()));
			return jsonclass;
		}

		// Token: 0x06000CAC RID: 3244 RVA: 0x00055819 File Offset: 0x00053A19
		public new ScheduledRepeatingNotification SetUserData(IDictionary<string, string> userData)
		{
			base.SetUserData(userData);
			return this;
		}

		// Token: 0x06000CAD RID: 3245 RVA: 0x00055824 File Offset: 0x00053A24
		public new ScheduledRepeatingNotification SetNotificationProfile(string notificationProfile)
		{
			base.SetNotificationProfile(notificationProfile);
			return this;
		}

		// Token: 0x06000CAE RID: 3246 RVA: 0x0005582F File Offset: 0x00053A2F
		public new ScheduledRepeatingNotification SetBadgeNumber(int badgeNumber)
		{
			base.SetBadgeNumber(badgeNumber);
			return this;
		}

		// Token: 0x06000CAF RID: 3247 RVA: 0x0005583A File Offset: 0x00053A3A
		public new ScheduledRepeatingNotification SetButtons(ICollection<Button> buttons)
		{
			base.SetButtons(buttons);
			return this;
		}

		// Token: 0x17000034 RID: 52
		// (get) Token: 0x06000CB0 RID: 3248 RVA: 0x00055845 File Offset: 0x00053A45
		public override bool IsRepeating
		{
			get
			{
				return this.intervalSeconds > 0;
			}
		}

		// Token: 0x04000D20 RID: 3360
		public readonly int intervalSeconds;
	}
}
