using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x0200064D RID: 1613
	public class SSOWelcomePanel : SSOBasePanel
	{
		// Token: 0x170007F5 RID: 2037
		// (get) Token: 0x06003B99 RID: 15257 RVA: 0x00127F5A File Offset: 0x0012615A
		// (set) Token: 0x06003B9A RID: 15258 RVA: 0x00127F6C File Offset: 0x0012616C
		public string Email
		{
			get
			{
				return this._ui.emailInputField.text;
			}
			set
			{
				this._ui.emailInputField.text = value;
			}
		}

		// Token: 0x170007F6 RID: 2038
		// (get) Token: 0x06003B9B RID: 15259 RVA: 0x00127F7F File Offset: 0x0012617F
		// (set) Token: 0x06003B9C RID: 15260 RVA: 0x00127F91 File Offset: 0x00126191
		public bool SubscribeToNewsletter
		{
			get
			{
				return this._ui.subscribeToNewsletterButton.IsOn;
			}
			set
			{
				this._ui.subscribeToNewsletterButton.IsOn = value;
			}
		}

		// Token: 0x170007F7 RID: 2039
		// (get) Token: 0x06003B9D RID: 15261 RVA: 0x00127FA4 File Offset: 0x001261A4
		public bool AreRequirementsMet
		{
			get
			{
				return EmailFormatValidator.IsValidEmail(this.Email);
			}
		}

		// Token: 0x06003B9E RID: 15262 RVA: 0x00127FB1 File Offset: 0x001261B1
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.emailInputField);
		}

		// Token: 0x06003B9F RID: 15263 RVA: 0x00127FC3 File Offset: 0x001261C3
		public void Show(bool resetEmail = true)
		{
			if (resetEmail)
			{
				this._ui.emailInputField.text = string.Empty;
			}
			base.Show();
			this._UpdateUI();
		}

		// Token: 0x06003BA0 RID: 15264 RVA: 0x00127FE9 File Offset: 0x001261E9
		public void OnValueChanged(string value)
		{
			this._UpdateUI();
		}

		// Token: 0x06003BA1 RID: 15265 RVA: 0x00127FF1 File Offset: 0x001261F1
		private void _UpdateUI()
		{
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x0400266F RID: 9839
		[SerializeField]
		private SSOWelcomePanel.UI _ui;

		// Token: 0x0200095C RID: 2396
		[Serializable]
		public class UI
		{
			// Token: 0x04003178 RID: 12664
			public TMP_InputField emailInputField;

			// Token: 0x04003179 RID: 12665
			public ToggleButton subscribeToNewsletterButton;
		}
	}
}
