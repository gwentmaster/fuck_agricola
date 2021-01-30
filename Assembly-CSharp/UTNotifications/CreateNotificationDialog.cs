using System;
using UnityEngine;
using UnityEngine.UI;

namespace UTNotifications
{
	// Token: 0x02000136 RID: 310
	public class CreateNotificationDialog : MonoBehaviour
	{
		// Token: 0x06000BD5 RID: 3029 RVA: 0x00053908 File Offset: 0x00051B08
		public void Show(string dialogTitle, bool showHasImage, bool showHasButtons, CreateNotificationDialog.OnComplete onComplete)
		{
			if (base.gameObject.activeSelf)
			{
				throw new InvalidOperationException();
			}
			if (onComplete == null)
			{
				throw new ArgumentNullException("onComplete");
			}
			this.HasImage.gameObject.SetActive(false);
			this.HasButtons.gameObject.SetActive(false);
			this.DialogTitle.text = dialogTitle;
			this.onComplete = onComplete;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06000BD6 RID: 3030 RVA: 0x0005397C File Offset: 0x00051B7C
		public void OK()
		{
			this.onComplete(this.Title.text, this.Text.text, string.IsNullOrEmpty(this.ID.text) ? 1 : int.Parse(this.ID.text), this.NotificationProfile.text, string.IsNullOrEmpty(this.Badge.text) ? -1 : int.Parse(this.Badge.text), this.HasImage.isOn, this.HasButtons.isOn);
			this.Cancel();
		}

		// Token: 0x06000BD7 RID: 3031 RVA: 0x00053A1B File Offset: 0x00051C1B
		public void Cancel()
		{
			this.onComplete = null;
			base.gameObject.SetActive(false);
		}

		// Token: 0x06000BD8 RID: 3032 RVA: 0x00003022 File Offset: 0x00001222
		private void Start()
		{
		}

		// Token: 0x04000CD7 RID: 3287
		public Text DialogTitle;

		// Token: 0x04000CD8 RID: 3288
		public Text Title;

		// Token: 0x04000CD9 RID: 3289
		public Text Text;

		// Token: 0x04000CDA RID: 3290
		public Text ID;

		// Token: 0x04000CDB RID: 3291
		public Text NotificationProfile;

		// Token: 0x04000CDC RID: 3292
		public Text Badge;

		// Token: 0x04000CDD RID: 3293
		public Toggle HasImage;

		// Token: 0x04000CDE RID: 3294
		public Toggle HasButtons;

		// Token: 0x04000CDF RID: 3295
		private CreateNotificationDialog.OnComplete onComplete;

		// Token: 0x02000812 RID: 2066
		// (Invoke) Token: 0x0600441B RID: 17435
		public delegate void OnComplete(string title, string text, int id, string notificationProfile, int badge, bool hasImage, bool hasButtons);
	}
}
