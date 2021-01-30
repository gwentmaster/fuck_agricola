using System;
using AsmodeeNet.Utils;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace AsmodeeNet.UserInterface.SSO
{
	// Token: 0x0200064A RID: 1610
	public class SSOLoginAndPasswordPanel : SSOBasePanel
	{
		// Token: 0x170007F3 RID: 2035
		// (get) Token: 0x06003B55 RID: 15189 RVA: 0x00126FDF File Offset: 0x001251DF
		// (set) Token: 0x06003B56 RID: 15190 RVA: 0x00126FF1 File Offset: 0x001251F1
		public string Text
		{
			get
			{
				return this._ui.inputField.text;
			}
			set
			{
				this._ui.inputField.text = value;
			}
		}

		// Token: 0x170007F4 RID: 2036
		// (get) Token: 0x06003B57 RID: 15191 RVA: 0x00127004 File Offset: 0x00125204
		public bool AreRequirementsMet
		{
			get
			{
				return !string.IsNullOrEmpty(this.Text);
			}
		}

		// Token: 0x06003B58 RID: 15192 RVA: 0x00127014 File Offset: 0x00125214
		public void DisplayErrorMessage(string message)
		{
			if (string.IsNullOrEmpty(message))
			{
				this._ui.errorIndicator.gameObject.SetActive(false);
				return;
			}
			this._ui.errorIndicator.gameObject.SetActive(true);
			this._ui.errorMessage.text = message;
		}

		// Token: 0x06003B59 RID: 15193 RVA: 0x00127067 File Offset: 0x00125267
		private void Awake()
		{
			ModifyInputFieldOnEndEditBehaviour.ModifyBehaviour(this._ui.inputField);
		}

		// Token: 0x06003B5A RID: 15194 RVA: 0x0012707C File Offset: 0x0012527C
		public void Show(bool resetLogin = true)
		{
			if (resetLogin)
			{
				this._ui.inputField.text = string.Empty;
			}
			this._ui.errorIndicator.gameObject.SetActive(false);
			if (this._ui.showPasswordToggle != null)
			{
				this._ui.showPasswordToggle.isOn = (this._ui.inputField.contentType == TMP_InputField.ContentType.Password);
			}
			base.Show();
			this._UpdateUI();
		}

		// Token: 0x06003B5B RID: 15195 RVA: 0x001270FC File Offset: 0x001252FC
		public void SelectInputFieldContent()
		{
			this._ui.inputField.Select();
			this._ui.inputField.ActivateInputField();
			this._ui.inputField.selectionFocusPosition = 0;
			this._ui.inputField.MoveTextEnd(false);
		}

		// Token: 0x06003B5C RID: 15196 RVA: 0x0012714B File Offset: 0x0012534B
		public void OnValueChanged(string value)
		{
			this._UpdateUI();
		}

		// Token: 0x06003B5D RID: 15197 RVA: 0x00127153 File Offset: 0x00125353
		private void _UpdateUI()
		{
			this._ui.errorIndicator.gameObject.SetActive(false);
			this._buttons[1].interactable = this.AreRequirementsMet;
		}

		// Token: 0x06003B5E RID: 15198 RVA: 0x0012717E File Offset: 0x0012537E
		public void ToggleInputFieldContentType()
		{
			this._ui.inputField.contentType = (this._ui.showPasswordToggle.isOn ? TMP_InputField.ContentType.Password : TMP_InputField.ContentType.Standard);
			this._ui.inputField.ForceLabelUpdate();
		}

		// Token: 0x0400265E RID: 9822
		[SerializeField]
		private SSOLoginAndPasswordPanel.UI _ui;

		// Token: 0x02000952 RID: 2386
		[Serializable]
		public class UI
		{
			// Token: 0x04003156 RID: 12630
			public TMP_InputField inputField;

			// Token: 0x04003157 RID: 12631
			public Toggle showPasswordToggle;

			// Token: 0x04003158 RID: 12632
			public GameObject errorIndicator;

			// Token: 0x04003159 RID: 12633
			public TextMeshProUGUI errorMessage;
		}
	}
}
