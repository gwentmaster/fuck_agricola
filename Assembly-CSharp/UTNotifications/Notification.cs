using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000147 RID: 327
	public abstract class Notification
	{
		// Token: 0x1700002F RID: 47
		// (get) Token: 0x06000C7E RID: 3198 RVA: 0x000552B3 File Offset: 0x000534B3
		// (set) Token: 0x06000C7F RID: 3199 RVA: 0x000552BB File Offset: 0x000534BB
		public IDictionary<string, string> userData { get; private set; }

		// Token: 0x17000030 RID: 48
		// (get) Token: 0x06000C80 RID: 3200 RVA: 0x000552C4 File Offset: 0x000534C4
		// (set) Token: 0x06000C81 RID: 3201 RVA: 0x000552CC File Offset: 0x000534CC
		public string notificationProfile { get; private set; }

		// Token: 0x17000031 RID: 49
		// (get) Token: 0x06000C82 RID: 3202 RVA: 0x000552D5 File Offset: 0x000534D5
		// (set) Token: 0x06000C83 RID: 3203 RVA: 0x000552DD File Offset: 0x000534DD
		public int badgeNumber { get; private set; }

		// Token: 0x17000032 RID: 50
		// (get) Token: 0x06000C84 RID: 3204 RVA: 0x000552E6 File Offset: 0x000534E6
		// (set) Token: 0x06000C85 RID: 3205 RVA: 0x000552EE File Offset: 0x000534EE
		public ICollection<Button> buttons { get; private set; }

		// Token: 0x06000C86 RID: 3206 RVA: 0x000552F7 File Offset: 0x000534F7
		public Notification(string title, string text, int id)
		{
			this.title = title;
			this.text = text;
			this.id = id;
			this.badgeNumber = -1;
		}

		// Token: 0x06000C87 RID: 3207 RVA: 0x0005531B File Offset: 0x0005351B
		public Notification(string title, string text, int id, IDictionary<string, string> userData, string notificationProfile, int badgeNumber, ICollection<Button> buttons) : this(title, text, id)
		{
			this.userData = userData;
			this.notificationProfile = notificationProfile;
			this.badgeNumber = badgeNumber;
			this.buttons = buttons;
		}

		// Token: 0x06000C88 RID: 3208 RVA: 0x00055348 File Offset: 0x00053548
		public Notification(JSONNode json)
		{
			this.title = json["title"].Value;
			this.text = json["text"].Value;
			this.id = json["id"].AsInt;
			this.userData = JsonUtils.ToUserData(json["userData"]);
			this.notificationProfile = (string.IsNullOrEmpty(json["notificationProfile"].Value) ? null : json["notificationProfile"].Value);
			this.badgeNumber = (string.IsNullOrEmpty(json["badgeNumber"].Value) ? -1 : json["badgeNumber"].AsInt);
			this.buttons = JsonUtils.ToButtons(json["buttons"]);
		}

		// Token: 0x06000C89 RID: 3209 RVA: 0x0005542C File Offset: 0x0005362C
		public virtual JSONClass ToJson()
		{
			JSONClass jsonclass = new JSONClass();
			jsonclass.Add("title", new JSONData(this.title));
			jsonclass.Add("text", new JSONData(this.text));
			jsonclass.Add("id", new JSONData(this.id));
			if (this.userData != null && this.userData.Count > 0)
			{
				jsonclass.Add("userData", JsonUtils.ToJson(this.userData));
			}
			if (!string.IsNullOrEmpty(this.notificationProfile))
			{
				jsonclass.Add("notificationProfile", new JSONData(this.notificationProfile));
			}
			if (this.badgeNumber != -1)
			{
				jsonclass.Add("badgeNumber", new JSONData(this.badgeNumber));
			}
			if (this.buttons != null && this.buttons.Count > 0)
			{
				jsonclass.Add("buttons", JsonUtils.ToJson(this.buttons));
			}
			return jsonclass;
		}

		// Token: 0x06000C8A RID: 3210 RVA: 0x0005551C File Offset: 0x0005371C
		public static Notification FromJson(JSONNode json)
		{
			if (!(json["provider"] is JSONLazyCreator))
			{
				return new PushNotification(json);
			}
			if (!(json["intervalSeconds"] is JSONLazyCreator))
			{
				return new ScheduledRepeatingNotification(json);
			}
			if (!(json["triggerDateTime"] is JSONLazyCreator) || !(json["triggerAtSystemTimeMillis"] is JSONLazyCreator))
			{
				return new ScheduledNotification(json);
			}
			return new LocalNotification(json);
		}

		// Token: 0x06000C8B RID: 3211 RVA: 0x0005558C File Offset: 0x0005378C
		public Notification SetUserData(IDictionary<string, string> userData)
		{
			this.userData = userData;
			return this;
		}

		// Token: 0x06000C8C RID: 3212 RVA: 0x00055596 File Offset: 0x00053796
		public Notification SetNotificationProfile(string notificationProfile)
		{
			this.notificationProfile = notificationProfile;
			return this;
		}

		// Token: 0x06000C8D RID: 3213 RVA: 0x000555A0 File Offset: 0x000537A0
		public Notification SetBadgeNumber(int badgeNumber)
		{
			this.badgeNumber = badgeNumber;
			return this;
		}

		// Token: 0x06000C8E RID: 3214 RVA: 0x000555AA File Offset: 0x000537AA
		public Notification SetButtons(ICollection<Button> buttons)
		{
			this.buttons = buttons;
			return this;
		}

		// Token: 0x06000C8F RID: 3215 RVA: 0x000555B4 File Offset: 0x000537B4
		public override string ToString()
		{
			return this.ToJson().ToString();
		}

		// Token: 0x04000D16 RID: 3350
		public const int BADGE_NOT_SPECIFIED = -1;

		// Token: 0x04000D17 RID: 3351
		public readonly string title;

		// Token: 0x04000D18 RID: 3352
		public readonly string text;

		// Token: 0x04000D19 RID: 3353
		public readonly int id;
	}
}
