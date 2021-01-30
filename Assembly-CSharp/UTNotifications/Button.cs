using System;
using System.Collections.Generic;

namespace UTNotifications
{
	// Token: 0x02000142 RID: 322
	public class Button
	{
		// Token: 0x06000C26 RID: 3110 RVA: 0x000547AA File Offset: 0x000529AA
		public Button(string title, IDictionary<string, string> userData = null)
		{
			this.title = title;
			this.userData = userData;
		}

		// Token: 0x04000D05 RID: 3333
		public string title;

		// Token: 0x04000D06 RID: 3334
		public IDictionary<string, string> userData;
	}
}
