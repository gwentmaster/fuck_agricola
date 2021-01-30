using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x0200014A RID: 330
	public class ScheduledNotification : LocalNotification
	{
		// Token: 0x06000C9F RID: 3231 RVA: 0x0005569E File Offset: 0x0005389E
		public ScheduledNotification(DateTime triggerDateTime, string title, string text, int id, IDictionary<string, string> userData, string notificationProfile, int badgeNumber, ICollection<Button> buttons) : base(title, text, id, userData, notificationProfile, badgeNumber, buttons)
		{
			this.triggerDateTime = triggerDateTime;
		}

		// Token: 0x06000CA0 RID: 3232 RVA: 0x000556B9 File Offset: 0x000538B9
		public ScheduledNotification(DateTime triggerDateTime, string title, string text, int id) : base(title, text, id)
		{
			this.triggerDateTime = triggerDateTime;
		}

		// Token: 0x06000CA1 RID: 3233 RVA: 0x000556CC File Offset: 0x000538CC
		public ScheduledNotification(JSONNode json) : base(json)
		{
			this.triggerDateTime = ((!(json["triggerDateTime"] is JSONLazyCreator)) ? DateTime.Parse(json["triggerDateTime"].Value) : TimeUtils.UnixTimestampMillisToDateTime(double.Parse(json["triggerAtSystemTimeMillis"].Value)));
		}

		// Token: 0x06000CA2 RID: 3234 RVA: 0x0005572C File Offset: 0x0005392C
		public override JSONClass ToJson()
		{
			JSONClass jsonclass = base.ToJson();
			jsonclass.Add("triggerDateTime", new JSONData(this.triggerDateTime.ToString()));
			return jsonclass;
		}

		// Token: 0x06000CA3 RID: 3235 RVA: 0x0005575D File Offset: 0x0005395D
		public new ScheduledNotification SetUserData(IDictionary<string, string> userData)
		{
			base.SetUserData(userData);
			return this;
		}

		// Token: 0x06000CA4 RID: 3236 RVA: 0x00055768 File Offset: 0x00053968
		public new ScheduledNotification SetNotificationProfile(string notificationProfile)
		{
			base.SetNotificationProfile(notificationProfile);
			return this;
		}

		// Token: 0x06000CA5 RID: 3237 RVA: 0x00055773 File Offset: 0x00053973
		public new ScheduledNotification SetBadgeNumber(int badgeNumber)
		{
			base.SetBadgeNumber(badgeNumber);
			return this;
		}

		// Token: 0x06000CA6 RID: 3238 RVA: 0x0005577E File Offset: 0x0005397E
		public new ScheduledNotification SetButtons(ICollection<Button> buttons)
		{
			base.SetButtons(buttons);
			return this;
		}

		// Token: 0x17000033 RID: 51
		// (get) Token: 0x06000CA7 RID: 3239 RVA: 0x0002A062 File Offset: 0x00028262
		public virtual bool IsRepeating
		{
			get
			{
				return false;
			}
		}

		// Token: 0x04000D1F RID: 3359
		public readonly DateTime triggerDateTime;
	}
}
