using System;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x0200064C RID: 1612
	public class SSOOKPanel : MonoBehaviour
	{
		// Token: 0x06003B94 RID: 15252 RVA: 0x00003022 File Offset: 0x00001222
		private void Awake()
		{
		}

		// Token: 0x06003B95 RID: 15253 RVA: 0x00127EC8 File Offset: 0x001260C8
		public void Show(string title, string message, Action onOkAction, SSOOKPanel.MessageType messageType)
		{
			this._ui.title.text = title;
			this._ui.message.text = message;
			GameObject standardIllustration = this._ui.standardIllustration;
			if (standardIllustration != null)
			{
				standardIllustration.SetActive(messageType == SSOOKPanel.MessageType.Standard);
			}
			GameObject errorIllustration = this._ui.errorIllustration;
			if (errorIllustration != null)
			{
				errorIllustration.SetActive(messageType == SSOOKPanel.MessageType.Error);
			}
			this._onOkAction = onOkAction;
			base.gameObject.SetActive(true);
		}

		// Token: 0x06003B96 RID: 15254 RVA: 0x0002A073 File Offset: 0x00028273
		public void Hide()
		{
			base.gameObject.SetActive(false);
		}

		// Token: 0x06003B97 RID: 15255 RVA: 0x00127F40 File Offset: 0x00126140
		public void OnOkButtonClicked()
		{
			Action onOkAction = this._onOkAction;
			if (onOkAction != null)
			{
				onOkAction();
			}
			this._onOkAction = null;
		}

		// Token: 0x0400266D RID: 9837
		[SerializeField]
		private SSOOKPanel.UI _ui;

		// Token: 0x0400266E RID: 9838
		private Action _onOkAction;

		// Token: 0x0200095A RID: 2394
		[Serializable]
		public class UI
		{
			// Token: 0x04003171 RID: 12657
			public TextMeshProUGUI title;

			// Token: 0x04003172 RID: 12658
			public TextMeshProUGUI message;

			// Token: 0x04003173 RID: 12659
			public GameObject standardIllustration;

			// Token: 0x04003174 RID: 12660
			public GameObject errorIllustration;
		}

		// Token: 0x0200095B RID: 2395
		public enum MessageType
		{
			// Token: 0x04003176 RID: 12662
			Standard,
			// Token: 0x04003177 RID: 12663
			Error
		}
	}
}
